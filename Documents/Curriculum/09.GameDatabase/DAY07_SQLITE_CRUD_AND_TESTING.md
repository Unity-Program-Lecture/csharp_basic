# DAY 07: SQLite CRUD와 테스트 (4교시)

오늘은 C#에서 데이터를 등록, 조회, 수정, 삭제하고 DB 프로그램을 테스트합니다.

`Parameters.AddWithValue()`, `ExecuteReader()`, `ExecuteScalar()`의 사용법은 [SQLite API 빠른 참조](Supplement/SQLITE_API_REFERENCE.md)에서 확인할 수 있습니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 프로그래밍하기
- 주요 학습 내용: 관계형 DB 관리 프로그램 작성, 프로그램 테스트로 코드 완성하기

## 1. CRUD (Create, Read, Update, Delete): "장부를 다루는 네 동작"

| 동작 | SQL | 상점 예시 |
| :--- | :--- | :--- |
| Create | `INSERT` | 새 아이템 등록 |
| Read | `SELECT` | 플레이어 인벤토리 조회 |
| Update | `UPDATE` | 골드 또는 수량 변경 |
| Delete | `DELETE` | 테스트용 아이템 삭제 |

## 2. 값은 SQL 문자열에 붙이지 않습니다

사용자가 입력한 이름을 SQL 문장에 바로 이어 붙이면 따옴표나 악의적인 입력 때문에 오류가 날 수 있습니다. `Parameters`에 값을 따로 넣습니다.

<details>
<summary>안전하게 아이템 넣기</summary>

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

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
INSERT INTO Item (ItemId, Name, Price)
VALUES ($itemId, $name, $price);";
                    command.Parameters.AddWithValue("$itemId", 2);
                    command.Parameters.AddWithValue("$name", "철 검");
                    command.Parameters.AddWithValue("$price", 100);

                    int changedRows = command.ExecuteNonQuery();
                    Console.WriteLine(changedRows + "건의 아이템을 등록했습니다.");
                }
            }
        }
    }
}
```

</details>

## 3. 실습: 인벤토리 조회

**미션:** 플레이어 1번이 가진 아이템 이름과 수량을 출력합니다.

```sql
SELECT Item.Name, Inventory.Quantity
FROM Inventory
JOIN Item ON Inventory.ItemId = Item.ItemId
WHERE Inventory.PlayerId = $playerId;
```

`JOIN`은 두 표에서 관련 있는 행을 이어 읽는 명령입니다. `Inventory`의 아이템 번호와 `Item`의 아이템 번호가 같을 때 연결됩니다.

## 4. 테스트 기록하기

| 번호 | 입력 또는 상황 | 기대 결과 | 실제 결과 | 통과 |
| :--- | :--- | :--- | :--- | :--- |
| 1 | 포션 등록 | Item에 포션 1건 생성 |  |  |
| 2 | 플레이어 1 조회 | 이름과 골드 출력 |  |  |
| 3 | 없는 ItemId로 Inventory 등록 | 외래 키 오류 또는 등록 거부 |  |  |
| 4 | 가격 수정 | 수정한 가격으로 조회 |  |  |
| 5 | 테스트 아이템 삭제 | 조회 결과에서 사라짐 |  |  |

> SQLite에서 외래 키 제약을 사용할 때는 각 연결을 연 뒤 `PRAGMA foreign_keys = ON;`을 실행해야 합니다. DAY08 코드의 연결 설정에서 이를 추가합니다.

## 오늘의 정리

- CRUD는 DB 관리 프로그램의 기본 동작입니다.
- 테스트는 "실행됐다"가 아니라 기대한 데이터가 남았는지 확인하는 과정입니다.
- 다음 시간에는 구매 처리를 트랜잭션으로 묶습니다.
