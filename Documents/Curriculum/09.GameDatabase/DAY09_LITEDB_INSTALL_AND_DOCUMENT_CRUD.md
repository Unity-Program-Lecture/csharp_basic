# DAY 09: LiteDB 시작 - 설치와 문서 CRUD (4교시)

오늘은 LiteDB 패키지를 C# 프로젝트에 추가하고, 게임 로그 문서를 생성·조회합니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 프로그래밍하기
- 주요 학습 내용: 비관계형 DB를 생성하고 관리하는 프로그램 작성하기

## 1. LiteDB는 무엇인가요?

LiteDB는 .NET 프로그램 안에서 사용하는 파일 기반 문서형 DB입니다. 별도의 DB 서버 프로그램을 설치하지 않고 NuGet 패키지를 프로젝트에 추가합니다. `.db` 파일 안에 JSON과 비슷한 문서를 저장합니다.

## 2. LiteDB 다운로드와 설치: NuGet 패키지 추가

NuGet 패키지 사용 자체가 처음이라면 [NuGet 패키지 사용 가이드](Supplement/NUGET_PACKAGE_GUIDE.md)를 먼저 읽습니다. 터미널, Visual Studio 메뉴, 패키지 관리자 콘솔의 설치 방법을 모두 확인할 수 있습니다.

`LiteDatabase`, `GetCollection()`, `Insert()`, `FindAll()`의 역할은 [LiteDB API 빠른 참조](Supplement/LITEDB_API_REFERENCE.md)에서 다시 확인할 수 있습니다.

1. DAY06의 `GameDatabaseLab` 프로젝트 폴더를 엽니다.
2. 터미널에서 아래 명령을 실행합니다.

```powershell
dotnet add package LiteDB
```

3. NuGet이 LiteDB 라이브러리를 다운로드합니다. 완료되면 `.csproj`에 `PackageReference`가 추가됩니다.
4. 오류가 나면 다음을 순서대로 확인합니다.
   - 인터넷 연결과 NuGet 접근 가능 여부
   - 터미널이 `.csproj` 파일이 있는 프로젝트 폴더에서 실행됐는지
   - `dotnet --info`가 정상 출력되는지
   - 학교 PC의 보안 정책이 패키지 다운로드를 막는지
5. LiteDB는 서버리스 라이브러리라 별도 DB 서비스 설치나 실행은 필요하지 않습니다. 수업 당일에는 NuGet의 최신 안정판을 사용합니다.

## 3. 첫 문서 저장

**미션:** 구매 실패 기록 하나를 `GameLogs.db`에 저장합니다.

<details>
<summary>코드 보기</summary>

```csharp
using System;
using LiteDB;

public class GameLog
{
    public int Id { get; set; }
    public string EventType { get; set; } = "";
    public int PlayerId { get; set; }
    public string Message { get; set; } = "";
    public DateTime OccurredAt { get; set; }
}

internal class Program
{
    private static void Main(string[] args)
    {
        using (LiteDatabase database = new LiteDatabase("GameLogs.db"))
        {
            ILiteCollection<GameLog> logs =
                database.GetCollection<GameLog>("logs");

            logs.Insert(new GameLog
            {
                EventType = "PurchaseFailed",
                PlayerId = 1,
                Message = "골드가 부족합니다.",
                OccurredAt = DateTime.Now
            });

            foreach (GameLog log in logs.FindAll())
            {
                Console.WriteLine(log.EventType + ": " + log.Message);
            }
        }
    }
}
```

</details>

### 이 단어는 무슨 뜻인가요?

- **POCO 클래스**: DB 전용 부모 클래스 없이 작성하는 일반 C# 클래스입니다.
- **컬렉션**: 같은 성격의 문서 모음입니다. 여기서는 `logs`입니다.
- **문서 ID (`_id`)**: 한 컬렉션 안에서 문서 한 건을 고유하게 구분하는 값입니다. 관계형 DB의 기본 키와 비슷한 역할을 합니다.
- **문서 CRUD**: `Insert`, `Find`, `Update`, `Delete`로 문서를 다룹니다.

### 문서형 DB에도 Primary Key가 있나요?

있습니다. LiteDB의 모든 문서는 `_id`라는 고유 ID를 가지며, C# 클래스에서 보통 `Id` 속성이 그 값으로 저장됩니다.

```csharp
public class GameLog
{
    public int Id { get; set; } // LiteDB 문서의 _id 역할
    public string EventType { get; set; } = "";
}
```

`Id`는 같은 컬렉션 안에서 중복될 수 없습니다. 그래서 `Update()`나 `Delete()`가 어느 문서를 수정·삭제할지 구분할 수 있습니다. 다만 LiteDB의 `_id`는 관계형 DB의 `FOREIGN KEY`처럼 다른 컬렉션에 있는 ID의 존재를 자동으로 검사하지는 않습니다. 컬렉션 사이 관계가 복잡하고 강한 참조 규칙이 필요하면 관계형 DB 설계를 우선 검토합니다.

### `Insert()`하면 Id는 자동으로 생기나요?

아래처럼 `Id`에 값을 넣지 않고 새 `GameLog`를 만들면, `int`의 기본값인 `0`으로 시작합니다. LiteDB는 새 문서의 `Id = 0`을 자동 번호가 필요하다는 뜻으로 보고 `_id`를 부여합니다. `Insert()`가 끝난 뒤에는 같은 `log` 객체의 `Id`에도 부여된 번호가 들어 있습니다.

```csharp
GameLog log = new GameLog
{
    EventType = "PurchaseFailed",
    PlayerId = 1,
    Message = "골드가 부족합니다.",
    OccurredAt = DateTime.Now
};

logs.Insert(log);
Console.WriteLine("새 로그 ID: " + log.Id);
```

예를 들어 출력이 `새 로그 ID: 1`이면 LiteDB 문서의 `_id`도 1입니다. 이미 사용 중인 `Id` 값을 직접 넣고 다시 `Insert()`하면 중복 ID 오류가 납니다.

### `_id`를 실제로 사용하는 흐름

1. `Insert(log)` 뒤 `log.Id`에서 새 문서의 ID를 확인합니다.
2. 로그 목록을 보여 줄 때 각 문서의 `Id`도 함께 출력합니다.
3. 사용자가 선택한 ID를 `FindById()`에 전달해 정확히 한 문서를 다시 찾습니다.

```csharp
foreach (GameLog log in logs.FindAll())
{
    Console.WriteLine(log.Id + ": " + log.EventType);
}

Console.Write("열 로그 ID를 입력하세요: ");
string input = Console.ReadLine();
int selectedId;

if (int.TryParse(input, out selectedId))
{
    GameLog selectedLog = logs.FindById(selectedId);

    if (selectedLog != null)
    {
        Console.WriteLine(selectedLog.Message);
    }
    else
    {
        Console.WriteLine("해당 ID의 로그가 없습니다.");
    }
}
```

예를 들어 목록에 `3: PurchaseFailed`가 보이고 사용자가 `3`을 입력하면, `FindById(3)`이 그 문서를 찾습니다. 찾은 `selectedLog`를 수정한 뒤 `logs.Update(selectedLog)`을 호출하거나, `logs.Delete(selectedId)`로 삭제할 수 있습니다.

## 4. 실습: 로그를 더 자세히 남기기

`GameLog`에 아래 중 하나를 추가합니다.

- `ItemId`
- `GoldBefore`
- `ClientVersion`

그 뒤 한 로그에는 새 값을 넣고, 이전 로그에는 값을 넣지 않아도 프로그램이 읽히는지 확인합니다.

## 오늘의 정리

- LiteDB는 NuGet으로 프로젝트에 추가하며 별도 서버 설치가 필요 없습니다.
- 다음 시간에는 퀘스트 진행처럼 중첩된 문서를 설계하고, SQLite와 선택 기준을 비교합니다.
