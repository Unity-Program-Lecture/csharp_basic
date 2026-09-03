# 참고: Microsoft.Data.Sqlite API 빠른 참조

이 문서는 DAY06~DAY08에서 사용하는 SQLite C# API를 찾아보는 참고 자료입니다. SQL 문법 자체는 [DAY05](../DAY05_SQLITE_INSTALL_AND_SQL_BASICS.md)에서, 패키지 설치는 [NuGet 패키지 사용 가이드](NUGET_PACKAGE_GUIDE.md)에서 먼저 확인합니다.

> 예제는 Unity 6이 지원하는 C# 9.0 범위에서 읽을 수 있는 문법을 사용합니다. 실제 DB 실습은 .NET 콘솔 프로젝트에서 진행합니다.

## API 전체 흐름

```text
SqliteConnection 생성 -> Open()
    -> CreateCommand() -> CommandText와 Parameters 설정
        -> ExecuteNonQuery() 또는 ExecuteReader()
    -> Close/Dispose
```

트랜잭션이 필요하면 `BeginTransaction()`으로 묶고 `Commit()` 또는 `Rollback()`을 호출합니다.

## 1. `SqliteConnection`: DB 파일과 연결하기

| API | 역할 | 사용할 때 |
| :--- | :--- | :--- |
| `new SqliteConnection(connectionString)` | DB 연결 객체 생성 | DB 파일을 열기 전 |
| `Open()` | 실제 연결 열기 | 명령 실행 전 |
| `CreateCommand()` | SQL 명령 객체 준비 | SQL을 실행할 때 |
| `BeginTransaction()` | 트랜잭션 시작 | 여러 변경을 함께 처리할 때 |

```csharp
using (SqliteConnection connection =
       new SqliteConnection("Data Source=GameShop.db"))
{
    connection.Open();
    // 이 블록 안에서 SQL 명령을 만듭니다.
}
```

`using (...)` 블록을 벗어나면 연결이 정리됩니다. DB Browser가 같은 DB 파일을 열고 저장 중이면 잠금 오류가 날 수 있으므로, 실습 중에는 한 프로그램에서만 변경 작업을 합니다.

## 2. `SqliteCommand`: SQL 문장을 실행하기

| API | 역할 | 예 |
| :--- | :--- | :--- |
| `CommandText` | 실행할 SQL을 넣음 | `SELECT * FROM Item;` |
| `Parameters.AddWithValue()` | SQL의 값 자리에 실제 값을 안전하게 전달 | `$price`, `30` |
| `ExecuteNonQuery()` | 조회 결과 행이 필요 없는 SQL 실행 | `CREATE`, `INSERT`, `UPDATE`, `DELETE` |
| `ExecuteReader()` | `SELECT` 결과를 행 단위로 읽기 시작 | 인벤토리 목록 조회 |
| `ExecuteScalar()` | 값 하나만 읽기 | 아이템 개수, 최대 가격 |

### INSERT/UPDATE/DELETE: `ExecuteNonQuery()`

```csharp
using (SqliteCommand command = connection.CreateCommand())
{
    command.CommandText =
        "UPDATE Player SET Gold = $gold WHERE PlayerId = $playerId;";
    command.Parameters.AddWithValue("$gold", 100);
    command.Parameters.AddWithValue("$playerId", 1);

    int changedRows = command.ExecuteNonQuery();
    Console.WriteLine(changedRows + "건을 수정했습니다.");
}
```

반환값 `changedRows`는 보통 영향을 받은 행 수입니다. 0이면 조건에 맞는 행이 없었을 수 있으므로, 성공 메시지만 출력하기 전에 확인합니다.

### SELECT 여러 행: `ExecuteReader()`

```csharp
using (SqliteCommand command = connection.CreateCommand())
{
    command.CommandText = "SELECT Name, Price FROM Item;";

    using (SqliteDataReader reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            string name = reader.GetString(0);
            int price = reader.GetInt32(1);
            Console.WriteLine(name + ": " + price);
        }
    }
}
```

`reader.Read()`는 다음 행이 있으면 그 행으로 이동하고 `true`를 반환합니다. `GetString(0)`의 `0`은 SELECT에서 첫 번째로 적은 `Name` 열입니다.

### SELECT 값 하나: `ExecuteScalar()`

```csharp
using (SqliteCommand command = connection.CreateCommand())
{
    command.CommandText = "SELECT COUNT(*) FROM Item;";
    long itemCount = (long)command.ExecuteScalar();
    Console.WriteLine("아이템 수: " + itemCount);
}
```

## 3. 매개 변수: `Parameters.AddWithValue()`

SQL 문자열에 사용자 입력을 이어 붙이지 않고, `$이름` 자리에 값을 따로 넣습니다.

```csharp
command.CommandText =
    "INSERT INTO Item (ItemId, Name, Price) VALUES ($id, $name, $price);";
command.Parameters.AddWithValue("$id", 1);
command.Parameters.AddWithValue("$name", "회복 포션");
command.Parameters.AddWithValue("$price", 30);
```

| 확인할 것 | 이유 |
| :--- | :--- |
| SQL의 `$name`과 C#의 `"$name"`이 같은가 | 이름이 다르면 값을 찾지 못함 |
| 값 목록을 문자열로 이어 붙이지 않았는가 | 따옴표 오류와 안전 문제를 줄임 |
| 한 명령에 필요한 값을 모두 넣었는가 | 값이 빠지면 실행 오류가 남 |

## 4. `SqliteTransaction`: 함께 성공하거나 함께 취소하기

| API | 역할 |
| :--- | :--- |
| `connection.BeginTransaction()` | 트랜잭션 시작 |
| `command.Transaction = transaction` | 이 명령을 트랜잭션에 포함 |
| `transaction.Commit()` | 모든 변경 확정 |
| `transaction.Rollback()` | 변경 취소 |

```csharp
using (SqliteTransaction transaction = connection.BeginTransaction())
{
    try
    {
        // spendGold.Transaction = transaction;
        // addItem.Transaction = transaction;
        // 두 SQL 명령을 실행합니다.

        transaction.Commit();
    }
    catch (Exception)
    {
        transaction.Rollback();
        throw;
    }
}
```

포션 구매처럼 골드 차감과 인벤토리 증가가 함께 성공해야 할 때 사용합니다. SQLite는 동시에 쓰기 변경을 오래 잡아두면 다른 작업이 기다리거나 시간 초과될 수 있으므로, 트랜잭션 안에는 꼭 필요한 명령만 짧게 둡니다.

## 5. 자주 쓰는 조합

| 목적 | SQL | C# API |
| :--- | :--- |
| 표 만들기 | `CREATE TABLE` | `ExecuteNonQuery()` |
| 데이터 추가 | `INSERT` | `Parameters` + `ExecuteNonQuery()` |
| 목록 조회 | `SELECT` | `ExecuteReader()` |
| 값 하나 조회 | `SELECT COUNT`, `SELECT MAX` | `ExecuteScalar()` |
| 수정/삭제 | `UPDATE`, `DELETE` | `ExecuteNonQuery()` |
| 구매 처리 | 여러 `UPDATE`/`INSERT` | `SqliteTransaction` |

## 6. 자주 발생하는 문제

| 증상 | 확인할 것 |
| :--- | :--- |
| `no such table` | 표를 만드는 SQL을 실행했는지, 올바른 DB 파일을 열었는지 |
| `no such column` | SQL 열 이름과 스키마 열 이름이 같은지 |
| `database is locked` | DB Browser 또는 다른 프로그램이 파일 입출력 (I/O, Input/Output) 작업을 수행 중인지 |
| `ExecuteReader`에서 값 변환 오류 | SELECT 열 순서와 `GetString`/`GetInt32` 자료형이 맞는지 |
| 외래 키 규칙이 동작하지 않음 | 연결을 연 뒤 `PRAGMA foreign_keys = ON;`을 실행했는지 |

## 공식 참고

- [Microsoft.Data.Sqlite 네임스페이스](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlite)
- [SqliteCommand API](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlite.sqlitecommand)
- [Microsoft.Data.Sqlite 트랜잭션](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/transactions)
