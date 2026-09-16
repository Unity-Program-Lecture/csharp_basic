# DAY 08: 트랜잭션, 롤백, 유지보수 문서 (4교시)

오늘은 포션 구매의 여러 변경을 하나로 묶고, 실패 시 원래 상태로 되돌립니다.

`SqliteTransaction`, `Commit()`, `Rollback()` API는 [SQLite API 빠른 참조](Supplement/SQLITE_API_REFERENCE.md)의 트랜잭션 항목을 함께 참고합니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 응용 프로그래밍하기
- 주요 학습 내용: 트랜잭션 작성, 롤백 수행, 유지보수 문서 작성

## 1. 핵심 개념: "같이 성공하거나 같이 취소되는 약속"

포션 구매는 골드 차감과 인벤토리 증가가 모두 성공해야 합니다. 둘 중 하나만 반영되면 데이터가 틀어집니다.

```text
BEGIN
  골드 30 차감
  포션 1개 증가
COMMIT  -> 둘 다 저장

오류 발생
ROLLBACK -> 둘 다 구매 전 상태로 복원
```

### 이 단어는 무슨 뜻인가요?

- **트랜잭션**: 하나의 결과로 처리해야 하는 DB 명령 묶음입니다.
- **커밋**: 묶음의 모든 변경을 확정합니다.
- **롤백**: 묶음에서 발생한 변경을 취소하고 시작 전으로 되돌립니다.
- **무결성**: 데이터가 규칙에 맞고 서로 모순되지 않는 상태입니다.

## 2. `ON CONFLICT`는 왜 필요한가요?

인벤토리의 기본 키는 `(PlayerId, ItemId)` 조합입니다. 따라서 플레이어 1번이 이미 포션 1번을 가지고 있다면, 같은 조합을 다시 `INSERT`할 때 기본 키 중복 오류가 납니다.

```sql
INSERT INTO Inventory (PlayerId, ItemId, Quantity)
VALUES (1, 1, 1);
```

`ON CONFLICT`는 이런 **중복 충돌(conflict)** 이 발생했을 때 어떻게 처리할지 정하는 SQLite 문법입니다. 아래 문장은 새 포션이면 1개를 등록하고, 이미 포션이 있으면 기존 수량에 1을 더합니다.

```sql
INSERT INTO Inventory (PlayerId, ItemId, Quantity)
VALUES (1, 1, 1)
ON CONFLICT (PlayerId, ItemId)
DO UPDATE SET Quantity = Quantity + 1;
```

| 코드 조각 | 뜻 |
| :--- | :--- |
| `INSERT INTO ... VALUES ...` | 새 인벤토리 행을 추가하려고 합니다. |
| `ON CONFLICT (PlayerId, ItemId)` | 이 두 열의 조합이 기존 행과 겹칠 때를 뜻합니다. |
| `DO UPDATE` | 중복 오류를 내는 대신 기존 행을 수정합니다. |
| `SET Quantity = Quantity + 1` | 기존 수량에 1을 더합니다. |

이 동작을 **UPSERT** (Update + Insert)라고도 부릅니다. `UPDATE`와 `INSERT`를 상황에 맞춰 한 번에 처리한다는 뜻입니다. 다만 `ON CONFLICT`의 세부 문법은 DBMS마다 다를 수 있으므로, 여기서는 SQLite 문법으로 학습합니다.

구매 SQL의 `WHERE PlayerId = $playerId AND Gold >= $price`도 함께 읽어 봅니다. `WHERE`는 바꿀 행을 고르는 조건이고, `AND`는 양쪽 조건이 **모두** 맞아야 한다는 뜻입니다. 따라서 이 문장은 "선택한 플레이어이면서 골드가 가격 이상인 경우에만" 골드를 차감합니다. `Gold = Gold - $price`는 기존 골드에서 전달받은 가격만큼 빼라는 계산입니다.

### 충돌 처리 방법 고르기

| 문법 | 같은 기본 키가 이미 있을 때 | 알맞은 상황 |
| :--- | :--- | :--- |
| `INSERT OR IGNORE` | 아무것도 바꾸지 않고 넘어감 | DAY02처럼 초기 데이터를 한 번만 준비할 때 |
| `INSERT OR REPLACE` | 기존 행을 삭제한 뒤 새 행을 넣음 | 모든 값을 새 값으로 교체해도 안전하다고 확신할 때 |
| `INSERT ... ON CONFLICT ... DO UPDATE` | 지정한 열만 수정함 | 수량 증가처럼 기존 값 일부를 안전하게 바꿀 때 |

`INSERT OR OVERWRITE`라는 SQLite 문법은 없습니다. `INSERT OR REPLACE`가 이름은 비슷하지만, 기존 행을 **수정**하는 것이 아니라 삭제 후 새로 넣는 방식입니다. 외래 키로 연결된 게임 데이터에서는 삭제에 따라 연결 데이터가 영향을 받을 수 있으므로, 이 과정의 구매·수량 변경에는 `ON CONFLICT ... DO UPDATE` 방식을 사용합니다.

## 3. 안내형 실습: 포션 구매

**미션:** `try` 안의 두 명령이 모두 성공해야 `Commit()`이 호출되는지 확인합니다.

<details>
<summary>코드 보기</summary>

```csharp
using System;
using Microsoft.Data.Sqlite;

namespace GameDatabaseLab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (SqliteConnection connection =
                   new SqliteConnection("Data Source=GameShop.db"))
            {
                connection.Open();

                using (SqliteCommand pragma = connection.CreateCommand())
                {
                    pragma.CommandText = "PRAGMA foreign_keys = ON;";
                    pragma.ExecuteNonQuery();
                }

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqliteCommand spendGold = connection.CreateCommand())
                        {
                            spendGold.Transaction = transaction;
                            spendGold.CommandText = @"
UPDATE Player SET Gold = Gold - $price
WHERE PlayerId = $playerId AND Gold >= $price;";
                            spendGold.Parameters.AddWithValue("$price", 30);
                            spendGold.Parameters.AddWithValue("$playerId", 1);

                            if (spendGold.ExecuteNonQuery() != 1)
                            {
                                throw new InvalidOperationException("골드가 부족합니다.");
                            }
                        }

                        using (SqliteCommand addPotion = connection.CreateCommand())
                        {
                            addPotion.Transaction = transaction;
                            addPotion.CommandText = @"
INSERT INTO Inventory (PlayerId, ItemId, Quantity)
VALUES ($playerId, $itemId, 1)
ON CONFLICT (PlayerId, ItemId)
DO UPDATE SET Quantity = Quantity + 1;";
                            addPotion.Parameters.AddWithValue("$playerId", 1);
                            addPotion.Parameters.AddWithValue("$itemId", 1);
                            addPotion.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        Console.WriteLine("구매를 완료했습니다.");
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        Console.WriteLine("구매를 취소했습니다: " + exception.Message);
                    }
                }
            }
        }
    }
}
```

</details>

## 4. 실패를 일부러 만들고 확인하기

1. 플레이어 골드를 10으로 바꿉니다.
2. 포션 가격 30으로 구매를 실행합니다.
3. 실패 메시지를 확인합니다.
4. DB Browser에서 골드가 10 그대로인지, 포션 수량이 늘지 않았는지 확인합니다.
5. 골드를 100으로 되돌린 뒤 정상 구매도 확인합니다.

### 완료 확인

- [ ] 골드 차감과 인벤토리 증가가 같은 트랜잭션에 포함됐다.
- [ ] 골드 부족 시 두 변경이 모두 롤백됨을 DB Browser에서 확인했다.
- [ ] 초기 골드 100에서 정상 구매 후 골드 70, 포션 수량 증가를 확인했다.

## 응용 실습: 구매 수량 입력

구매 수량을 매개 변수로 받고, `가격 × 수량`보다 골드가 적으면 전체 구매를 취소하도록 바꾸세요. 수량이 0 이하인 경우도 거절해야 합니다.

## 5. 유지보수 문서 최소 양식

```text
프로그램 이름: GameShop Database Lab
DB 파일 위치: GameShop.db
실행 방법: dotnet run
표 목록: Player, Item, Inventory
데이터 추가 방법: Item INSERT 또는 관리 기능 사용
테스트 결과: 정상 구매 / 골드 부족 롤백
알려진 제한: 로컬 단일 사용자 학습용 DB이며 온라인 게임 서버용이 아님
```

## 오늘의 정리

- 트랜잭션은 재화와 아이템처럼 함께 바뀌어야 하는 데이터를 보호합니다.
- 유지보수 문서는 다음 사람이 안전하게 실행·변경할 수 있도록 남기는 안내서입니다.
- 다음 시간에는 문서형 NoSQL DB인 LiteDB를 C# 프로젝트에 추가해 문서 데이터를 저장합니다.
