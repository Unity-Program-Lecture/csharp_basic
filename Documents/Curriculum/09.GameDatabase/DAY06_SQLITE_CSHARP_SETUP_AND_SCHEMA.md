# DAY 06: C#에서 SQLite DB와 스키마 만들기 (4교시)

오늘은 C# 콘솔 프로그램에 SQLite 라이브러리를 내려받아 연결하고, 코드로 `Player`, `Item`, `Inventory` 표를 생성합니다.

> 이 과정의 C# 예제는 Unity 6이 지원하는 **C# 9.0**을 기준으로 작성합니다. C# 10 이후 문법인 file-scoped namespace, global using, raw string literal은 사용하지 않습니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 프로그래밍하기
- 주요 학습 내용: 관계형 DB를 생성하고 관리하는 프로그램 작성하기

## 1. 프로젝트와 패키지 준비

패키지를 처음 설치한다면 [NuGet 패키지 사용 가이드](Supplement/NUGET_PACKAGE_GUIDE.md)를 먼저 읽습니다. 프로젝트 생성부터 패키지 설치까지 **터미널 방식**과 **Visual Studio 방식**이 모두 정리되어 있습니다. 아래 터미널 절차가 낯설다면, 참고 문서의 "Visual Studio에서 콘솔 프로젝트 만들기" 후 "Visual Studio 메뉴에서 패키지 설치하기"를 따라 해도 됩니다.

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

4. 명령이 끝난 뒤 `.csproj` 파일에 `PackageReference`가 생겼는지 확인합니다.
5. 패키지 다운로드가 실패하면 인터넷 연결, NuGet 접근 권한, 프로젝트 폴더 쓰기 권한을 확인합니다. 버전 번호는 수업 당일 최신 안정판을 사용합니다.

## 2. 연결 문자열

```csharp
string connectionString = "Data Source=GameShop.db";
```

`GameShop.db` 파일이 없으면 SQLite가 새 파일을 만듭니다. 파일은 실행 폴더에 생기므로, 프로젝트 밖으로 복사할 때는 DB 파일도 함께 관리합니다.

## 3. 실습 예제: 표를 만드는 프로그램

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
                    command.CommandText = createTablesSql;
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("GameShop.db와 3개 표를 준비했습니다.");
        }
    }
}
```

</details>

## 4. 실행과 확인

1. `dotnet run`을 실행합니다.
2. 출력 메시지를 확인합니다.
3. DB Browser for SQLite에서 생성된 `GameShop.db`를 엽니다.
4. `Database Structure`에 세 표가 있는지 확인합니다.

## 생각해보기

1. `IF NOT EXISTS`가 없으면 프로그램을 두 번 실행할 때 어떤 일이 생길까요?
2. `Gold`에 `CHECK (Gold >= 0)`를 둔 이유는 무엇인가요?

## 오늘의 정리

- NuGet 패키지는 C# 프로젝트에 필요한 라이브러리를 내려받아 연결합니다.
- 다음 시간에는 CRUD (Create, Read, Update, Delete)와 매개 변수로 안전하게 데이터를 관리합니다.
