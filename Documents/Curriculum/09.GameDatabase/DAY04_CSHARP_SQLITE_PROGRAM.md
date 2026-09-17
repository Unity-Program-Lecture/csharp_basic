# 4일차: C# SQLite 프로그램 만들기 (6교시)

오늘은 C# 콘솔 프로그램에 SQLite 라이브러리를 내려받아 연결하고, 2일차에 설계하고 3일차에 사용한 `Player`, `Item`, `Inventory` 표를 코드에서 관리합니다. 표·키·제약 조건의 뜻은 다시 설명하지 않고, C# 연결·명령 실행·초기 데이터 준비에 집중합니다.

> 이 과정의 C# 예제는 Unity 6이 지원하는 **C# 9.0**을 기준으로 작성합니다. C# 10 이후 문법인 file-scoped namespace, global using, raw string literal은 사용하지 않습니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 프로그래밍하기
- 주요 학습 내용: 관계형 DB를 생성·관리하는 프로그램 작성, 프로그램 테스트로 코드 완성하기

## 1. 프로젝트와 패키지 준비

패키지를 처음 설치한다면 [NuGet 패키지 사용 가이드](Supplement/NUGET_PACKAGE_GUIDE.md)를 먼저 읽습니다. NuGet은 .NET (닷넷) 프로젝트에 필요한 라이브러리를 내려받아 연결하는 패키지 관리자이며, .NET은 C# 프로그램을 빌드하고 실행하는 개발 플랫폼입니다. 프로젝트 생성부터 패키지 설치까지 **터미널 방식**과 **Visual Studio 방식**이 모두 정리되어 있습니다. 아래 터미널 절차가 낯설다면, 참고 문서의 "Visual Studio에서 콘솔 프로젝트 만들기" 후 "Visual Studio 메뉴에서 패키지 설치하기"를 따라 해도 됩니다.

`SqliteConnection`, `SqliteCommand`, `ExecuteNonQuery()`의 역할이 헷갈리면 [SQLite API 빠른 참조](Supplement/SQLITE_API_REFERENCE.md)를 함께 봅니다.

1. 터미널에서 실습 폴더로 이동합니다.
2. 아래 명령으로 새 콘솔 프로젝트를 만듭니다.

```powershell
dotnet new console -n GameDatabaseLab
cd GameDatabaseLab
```

3. 아래 명령을 실행해 `Microsoft.Data.Sqlite` 패키지를 다운로드하고 프로젝트에 추가합니다.

```powershell
dotnet add package Microsoft.Data.Sqlite
```

4. 명령이 끝난 뒤 `.csproj` 파일 (C# 프로젝트의 설정 파일)에 `PackageReference`가 생겼는지 확인합니다.
5. 패키지 다운로드가 실패하면 인터넷 연결, NuGet 접근 권한, 프로젝트 폴더 쓰기 권한을 확인합니다. 버전 번호는 수업 당일 최신 안정판을 사용합니다.

## 2. 연결 문자열

```csharp
string connectionString = "Data Source=GameShop.db";
```

`GameShop.db` 파일이 없으면 SQLite가 새 파일을 만듭니다. 파일은 실행 폴더에 생기므로, 프로젝트 밖으로 복사할 때는 DB 파일도 함께 관리합니다.

### 2~3일차 DB 파일을 계속 사용하기

3일차까지 DB Browser에서 사용한 `GameShop.db`를 4일차의 `GameDatabaseLab` 프로젝트 폴더 (`.csproj` 파일이 있는 폴더)에 복사합니다. 터미널도 반드시 그 폴더에서 열고 `dotnet run`을 실행합니다. 그러면 `Data Source=GameShop.db`가 같은 파일을 열어, 앞날의 `Player`·`Item`·`Inventory` 데이터와 3일차 실습 결과를 이어서 확인할 수 있습니다.

```text
2~3일차: DB Browser에서 GameShop.db 사용
                    ↓ 복사
4~5일차: GameDatabaseLab/GameShop.db를 C#과 DB Browser에서 함께 사용
```

이후에는 C# 프로젝트 폴더에 둔 복사본을 이 과정의 실습 DB로 사용합니다. 다른 폴더의 동명 파일을 열면 데이터가 달라 보일 수 있으므로, DB Browser에서도 같은 경로의 파일을 열었는지 먼저 확인합니다.

## 3. 안내형 실습: 표를 만드는 프로그램

**미션:** 코드를 위에서 아래로 읽으며 `CREATE TABLE` 세 문장이 어떤 표를 만드는지 확인합니다.

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
            string connectionString = "Data Source=GameShop.db";
            string createTablesSql = @"
CREATE TABLE IF NOT EXISTS Player (
    PlayerId INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Gold INTEGER NOT NULL CHECK (Gold >= 0)
);

CREATE TABLE IF NOT EXISTS Item (
    ItemId INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Price INTEGER NOT NULL CHECK (Price >= 0)
);

CREATE TABLE IF NOT EXISTS Inventory (
    PlayerId INTEGER NOT NULL,
    ItemId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL CHECK (Quantity >= 0),
    PRIMARY KEY (PlayerId, ItemId),
    FOREIGN KEY (PlayerId) REFERENCES Player(PlayerId),
    FOREIGN KEY (ItemId) REFERENCES Item(ItemId)
);";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "PRAGMA foreign_keys = ON;\n" + createTablesSql + @"

INSERT OR IGNORE INTO Player (PlayerId, Name, Gold)
VALUES (1, '민지', 100);

INSERT OR IGNORE INTO Item (ItemId, Name, Price)
VALUES (1, '회복 포션', 30);

INSERT OR IGNORE INTO Inventory (PlayerId, ItemId, Quantity)
VALUES (1, 1, 0);";
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("GameShop.db, 3개 표와 초기 플레이어·포션·인벤토리를 준비했습니다.");
        }
    }
}
```

</details>

### 코드에서 달라지는 점

SQL 문법과 키·제약 조건의 뜻은 1~2일차에서 이미 확인했습니다. 여기서는 같은 SQL을 C# 문자열로 `CommandText`에 넣고, `ExecuteNonQuery()`로 실행한다는 점에 집중합니다. `PRAGMA foreign_keys = ON;`은 새 `SqliteConnection`을 열 때마다 실행해야 외래 키 검사가 켜집니다. `CREATE TABLE IF NOT EXISTS`와 `INSERT OR IGNORE`는 프로그램을 다시 실행해도 초기화 오류·중복 데이터가 생기지 않게 합니다.

## 4. 실행과 확인

1. `dotnet run`을 실행합니다.
2. 출력 메시지를 확인합니다.
3. DB Browser for SQLite에서 생성된 `GameShop.db`를 엽니다.
4. `Database Structure`에 세 표가 있는지 확인합니다.
5. `Browse Data`에서 Player 1번의 골드가 100이고, Item 1번이 회복 포션인지 확인합니다.

### 완료 확인

- [ ] `GameShop.db`에 `Player`, `Item`, `Inventory` 표가 생성됐다.
- [ ] 연결을 열 때 외래 키 검사를 켰다.
- [ ] Player 1번, Item 1번, Inventory의 포션 수량 0을 확인했다.

## 응용 실습: 두 번째 아이템 초기화

`INSERT OR IGNORE`를 사용해 Item 2번 `철 검`을 초기 데이터로 추가하세요. 프로그램을 두 번 실행해도 같은 ItemId가 중복되지 않는 이유를 설명하세요.

## 생각해보기

1. `IF NOT EXISTS`가 없으면 프로그램을 두 번 실행할 때 어떤 일이 생길까요?
2. `Gold`에 `CHECK (Gold >= 0)`를 둔 이유는 무엇인가요?

## 전반 정리 및 다음 교시

- NuGet 패키지는 C# 프로젝트에 필요한 라이브러리를 내려받아 연결합니다.
- 후반 교시에는 CRUD (Create, Read, Update, Delete)와 매개 변수로 안전하게 데이터를 관리합니다.

---

## 4일차 후반: C# SQLite CRUD와 테스트

3일차에 SQL로 실행한 등록·조회·수정·삭제를 C#에서 실행하고 DB 프로그램을 테스트합니다. SQL CRUD의 뜻은 다시 정의하지 않고, C# API와 매개 변수 사용에 집중합니다.

`Parameters.AddWithValue()`, `ExecuteReader()`, `ExecuteScalar()`의 사용법은 [SQLite API 빠른 참조](Supplement/SQLITE_API_REFERENCE.md)에서 확인할 수 있습니다.

### 값은 SQL 문자열에 붙이지 않습니다

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

                using (SqliteCommand pragma = connection.CreateCommand())
                {
                    pragma.CommandText = "PRAGMA foreign_keys = ON;";
                    pragma.ExecuteNonQuery();
                }

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

### 안내형 실습: 인벤토리 조회

**미션:** 플레이어 1번이 가진 아이템 이름과 수량을 출력합니다.

```sql
SELECT Item.Name, Inventory.Quantity
FROM Inventory
JOIN Item ON Inventory.ItemId = Item.ItemId
WHERE Inventory.PlayerId = $playerId;
```

| 코드 조각 | 이렇게 읽기 | 사용 목적 |
| :--- | :--- | :--- |
| `JOIN Item` | "Item 표를 이어 붙인다" | 인벤토리의 `ItemId`를 아이템 이름으로 함께 봄 |
| `ON Inventory.ItemId = Item.ItemId` | "두 아이템 번호가 같은 행을 연결한다" | 관계없는 아이템이 섞이지 않게 연결 기준을 정함 |
| `WHERE Inventory.PlayerId = $playerId` | "플레이어 번호가 같은 행만 고른다" | 조회 대상을 플레이어 한 명으로 제한함 |
| `$playerId` | C#에서 값을 넣을 이름표 | SQL 문자열을 이어 붙이지 않고 안전하게 번호를 전달함 |

앞 교시에서 Player 1번의 회복 포션 인벤토리를 수량 0으로 준비했습니다. 이 행을 이용해 조회하고, 5일차에는 같은 행의 수량을 트랜잭션 안에서 증가시킵니다.

### 테스트 기록하기

| 번호 | 입력 또는 상황 | 기대 결과 | 실제 결과 | 통과 |
| :--- | :--- | :--- | :--- | :--- |
| 1 | 철 검 등록 | Item에 철 검 1건 생성 |  |  |
| 2 | 플레이어 1 조회 | 이름과 골드 출력 |  |  |
| 3 | 없는 ItemId로 Inventory 등록 | 외래 키 오류 또는 등록 거부 |  |  |
| 4 | 가격 수정 | 수정한 가격으로 조회 |  |  |
| 5 | 테스트 아이템 삭제 | 조회 결과에서 사라짐 |  |  |

> SQLite에서 외래 키 제약을 사용할 때는 각 연결을 연 뒤 `PRAGMA foreign_keys = ON;`을 실행해야 합니다. 이 교시의 예제도 연결 직후 그 설정을 적용합니다.

#### 완료 확인

- [ ] SQL 값은 문자열을 이어 붙이지 않고 `Parameters`로 전달했다.
- [ ] Player 1번의 인벤토리 조회 결과를 확인했다.
- [ ] 존재하지 않는 ItemId의 인벤토리 등록이 거부됨을 기록했다.

### 응용 실습: 가격 범위 검색

최저 가격과 최고 가격을 매개 변수로 받아 그 범위의 아이템을 조회하는 SQL을 작성하세요. 두 값 모두 `Parameters`로 전달해야 합니다.

### 오늘의 정리

- CRUD는 DB 관리 프로그램의 기본 동작입니다.
- 테스트는 "실행됐다"가 아니라 기대한 데이터가 남았는지 확인하는 과정입니다.
- 다음 시간에는 구매 처리를 트랜잭션으로 묶습니다.
