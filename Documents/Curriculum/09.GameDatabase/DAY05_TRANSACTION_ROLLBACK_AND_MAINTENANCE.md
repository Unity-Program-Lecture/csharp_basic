# 5일차: C# 트랜잭션, 실패 테스트, 유지보수 문서 (6교시)

오늘은 3일차에 SQL로 직접 실행한 포션 구매 트랜잭션을 C# 코드로 자동화합니다. C#에서 오류가 나면 `Rollback()`을 호출하고, 정상일 때만 `Commit()`을 호출하는 흐름을 확인합니다.

`SqliteTransaction`, `Commit()`, `Rollback()` API는 [SQLite API 빠른 참조](Supplement/SQLITE_API_REFERENCE.md)의 트랜잭션 항목을 함께 참고합니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 응용 프로그래밍하기
- 주요 학습 내용: 트랜잭션 작성, 롤백 수행, 유지보수 문서 작성

## 1. SQL 트랜잭션을 C#으로 옮기기

3일차에서는 `BEGIN;`부터 시작해 두 SQL을 실행하고 `COMMIT;` 또는 `ROLLBACK;`을 직접 입력했습니다. C#에서는 같은 일을 `SqliteTransaction` 객체가 맡습니다.

```text
SQL `BEGIN;`        -> `connection.BeginTransaction()`
SQL `COMMIT;`       -> `transaction.Commit()`
SQL `ROLLBACK;`     -> `transaction.Rollback()`
```

### 이 단어는 무슨 뜻인가요?

- **트랜잭션**: 하나의 결과로 처리해야 하는 DB 명령 묶음입니다.
- **커밋**: 묶음의 모든 변경을 확정합니다.
- **롤백**: 묶음에서 발생한 변경을 취소하고 시작 전으로 되돌립니다.
- **무결성**: 데이터가 규칙에 맞고 서로 모순되지 않는 상태입니다.

## 2. 안내형 실습: C# 포션 구매

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
UPDATE Inventory
SET Quantity = Quantity + 1
WHERE PlayerId = $playerId AND ItemId = $itemId;";
                            addPotion.Parameters.AddWithValue("$playerId", 1);
                            addPotion.Parameters.AddWithValue("$itemId", 1);

                            if (addPotion.ExecuteNonQuery() != 1)
                            {
                                throw new InvalidOperationException(
                                    "포션 인벤토리 행이 없습니다.");
                            }
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

## 3. 실패를 일부러 만들고 확인하기

1. 플레이어 골드를 10으로 바꿉니다.
2. 포션 가격 30으로 구매를 실행합니다.
3. 실패 메시지를 확인합니다.
4. DB Browser에서 골드가 10 그대로인지, 포션 수량이 늘지 않았는지 확인합니다.
5. 골드를 100으로 되돌린 뒤 정상 구매도 확인합니다.

이번에는 두 번째 명령이 실패하는 경우도 확인합니다. `addPotion.Parameters.AddWithValue("$itemId", 1);`의 `1`을 잠시 `999`로 바꾼 뒤 실행합니다. 첫 번째 골드 차감은 성공하지만 인벤토리 행을 찾지 못해 예외가 발생하고, `Rollback()`이 첫 번째 변경까지 취소해야 합니다. 확인 뒤에는 `999`를 다시 `1`로 되돌립니다.

### 완료 확인

- [ ] 골드 차감과 인벤토리 증가가 같은 트랜잭션에 포함됐다.
- [ ] 골드 부족 시 두 변경이 모두 롤백됨을 DB Browser에서 확인했다.
- [ ] 두 번째 명령이 실패했을 때도 첫 번째 골드 차감이 롤백됨을 확인했다.
- [ ] 초기 골드 100에서 정상 구매 후 골드 70, 포션 수량 증가를 확인했다.

## 4. 확장: 인벤토리 행이 없을 때의 UPSERT

앞 실습은 2일차에서 만든 포션 인벤토리 행을 `UPDATE`하는 가장 단순한 흐름입니다. 새 아이템이라 인벤토리 행이 아직 없을 수도 있다면 `ON CONFLICT`로 삽입과 수량 증가를 함께 처리할 수 있습니다.

```sql
INSERT INTO Inventory (PlayerId, ItemId, Quantity)
VALUES ($playerId, $itemId, 1)
ON CONFLICT (PlayerId, ItemId)
DO UPDATE SET Quantity = Quantity + 1;
```

이 동작을 **UPSERT** (Update + Insert)라고 합니다. `INSERT OR OVERWRITE`는 SQLite 문법이 아닙니다. `INSERT OR REPLACE`는 기존 행을 삭제한 뒤 새 행을 넣으므로, 외래 키로 연결된 데이터의 수량 증가에는 위 UPSERT 방식이 더 안전합니다.

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
