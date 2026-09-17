# 6일차: 문서형 NoSQL과 LiteDB (6교시)

오늘은 NoSQL DB 중 문서형 DB의 구조를 설계하고, 관계형 DB와 쓰임을 비교합니다. 문서형 DB는 NoSQL 전체가 아니라 그 안에 속한 한 유형입니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 설계하기, 게임 데이터베이스 프로그래밍하기
- 주요 학습 내용: 비관계형 데이터베이스의 게임 데이터 저장 구조 설계, 문서형 NoSQL DB를 생성·관리하는 프로그램 작성

## 1. 핵심 개념: "기록 봉투"

문서형 DB는 한 대상과 그에 딸린 정보를 JSON과 비슷한 봉투에 함께 담습니다. 모든 봉투가 꼭 같은 칸을 가질 필요는 없습니다.

```json
{
  "eventType": "PurchaseFailed",
  "playerId": 1,
  "occurredAt": "2026-09-22T10:15:00",
  "reason": "NotEnoughGold",
  "clientVersion": "0.1.0"
}
```

### 이 단어는 무슨 뜻인가요?

- **문서형 NoSQL DB**: JSON처럼 한 대상의 관련 정보를 한 덩어리 문서로 저장하는 NoSQL DB입니다.
- **관계형 DB (RDB, Relational Database, 관계형 데이터베이스)**: 표의 행·열과 그 사이 관계를 중심으로 데이터를 관리하는 DB입니다. SQLite가 이 과정에서 사용하는 RDB입니다.
- **문서**: 키와 값으로 구성된 한 건의 데이터 묶음입니다.
- **컬렉션**: 같은 성격의 문서를 모은 상자입니다.
- **스키마 유연성**: 문서마다 필요한 칸을 조금 다르게 둘 수 있는 성질입니다.
- **NoSQL** (**Not Only SQL**, 비관계형 데이터베이스를 포괄하는 이름): **SQL** (Structured Query Language, 구조화 질의 언어) 표 모델만을 사용하지 않는 여러 DB 방식을 통칭하는 말입니다.

> NoSQL은 "SQL을 전혀 쓸 수 없다"는 뜻이 아니라, 관계형 표와 SQL을 중심으로 설계되지 않은 DB 방식을 가리킵니다. 이 과정에서는 SQLite에서 표준적인 SQL 기초를 배우고, 문서형 NoSQL DB인 LiteDB에서는 C# API (Application Programming Interface, 응용 프로그래밍 인터페이스)로 문서를 다룹니다.

## 2. 오늘 다룰 NoSQL 범위

NoSQL의 유형과 대표 제품은 1일차에서 이미 분류했습니다. 오늘은 그중 **문서형**만 실습합니다. JSON처럼 관련 정보를 한 문서에 묶는 방식이 퀘스트 진행과 오류 로그에 왜 어울리는지 설계하고, 후반 교시에는 LiteDB에 실제로 저장합니다.

## 3. 무엇을 문서형 NoSQL DB에 둘까요?

| 데이터 | 추천 | 이유 |
| :--- | :--- | :--- |
| 골드와 인벤토리 | 관계형 DB | 정확한 거래와 관계가 중요 |
| 구매 실패 로그 | 문서형 NoSQL DB | 오류마다 추가 정보가 다름 |
| 퀘스트 진행 스냅샷 | 문서형 NoSQL DB | 목표와 보상 목록을 한 번에 읽기 좋음 |
| 아이템 마스터 | 관계형 DB 또는 ScriptableObject | 공통 규칙과 중복 관리가 중요 |

## 4. 안내형 실습: 퀘스트 진행 문서 설계

**미션:** "고블린 10마리 처치" 퀘스트를 문서 한 건으로 설계합니다.

- 반드시 포함: 플레이어 ID (Identifier, 식별자), 퀘스트 ID, 현재 처치 수, 목표 수, 완료 여부
- 선택 포함: 시작 시각, 보상 목록, 마지막 저장 시각
- 다른 학생의 문서와 비교하여 "어떤 칸은 선택이어도 되는가"를 토의합니다.

## 5. 실제 만들기: 퀘스트 JSON 파일 저장

설계한 문서를 실제 파일로 만듭니다. 실습 폴더에 `QuestProgress.json`을 만들고 아래 내용을 입력해 저장하세요.

```json
{
  "playerId": 1,
  "questId": "GoblinHunt",
  "killCount": 3,
  "targetCount": 10,
  "isCompleted": false,
  "rewardIds": ["Potion", "Gold"],
  "lastSavedAt": "2026-09-22T10:15:00"
}
```

PowerShell을 사용할 수 있다면 아래 명령으로 JSON 문법과 저장 내용을 확인합니다.

```powershell
Get-Content QuestProgress.json -Raw | ConvertFrom-Json | Format-List
```

오류가 나면 쉼표, 큰따옴표, 대괄호와 중괄호의 짝을 먼저 확인합니다. 이 파일은 문서 구조를 설계하는 예시입니다. 후반의 LiteDB 첫 실습은 API 사용에 집중하기 위해 더 단순한 `GameLog` 문서를 저장하고, 7일차에 다시 `QuestProgress`를 LiteDB 문서로 확장합니다.

### 완료 확인

- [ ] 필수 항목과 선택 항목을 구분해 문서 한 건을 설계했다.
- [ ] 같은 종류의 정보라도 관계형 DB와 문서형 NoSQL DB 중 선택한 이유를 설명할 수 있다.
- [ ] 문서형이 NoSQL의 한 유형이며, 다른 NoSQL 유형 한 가지와 대표 제품을 말할 수 있다.
- [ ] `QuestProgress.json`을 저장하고 문법 확인 결과를 확인했다.

## 응용 실습: 접속 오류 문서 설계

접속 오류를 남길 문서를 설계하세요. 오류 코드, 발생 시각, 클라이언트 버전은 포함하고, 기기 정보 중 선택 항목 하나를 추가하세요.

## 전반 정리 및 다음 교시

- 문서형 NoSQL DB는 관계형 DB의 대체품이 아니라 NoSQL 안에서 다른 모양의 데이터를 다루는 한 유형입니다.
- 후반 교시에는 문서 구조의 공통점을 확인하면서, 더 단순한 `GameLog`를 LiteDB에 저장해 API 사용법부터 익힙니다.

---

## 6일차 후반: LiteDB 시작 - 간단한 로그 문서 CRUD

앞 교시에서는 `QuestProgress.json`으로 문서 구조를 설계했습니다. 후반에는 그 구조가 LiteDB에서도 "클래스 한 개가 문서 한 건이 되는" 방식으로 이어짐을 확인하되, 처음 API를 익히기에는 항목이 적은 `GameLog`를 사용합니다. 7일차에는 다시 `QuestProgress`처럼 중첩된 문서로 확장합니다. .NET은 C# 프로그램을 빌드하고 실행하는 개발 플랫폼입니다.

### LiteDB는 무엇인가요?

LiteDB는 .NET 프로그램 안에서 사용하는 파일 기반 **문서형 NoSQL DB**입니다. 앞 교시에서 확인한 NoSQL 분류와 JSON 문서 개념을 바탕으로, 여기서는 별도 DB 서버 설치 없이 NuGet 패키지로 LiteDB를 추가하고 실제 로그 문서를 저장합니다.

### LiteDB 다운로드와 설치: NuGet 패키지 추가

NuGet 패키지 사용 자체가 처음이라면 [NuGet 패키지 사용 가이드](Supplement/NUGET_PACKAGE_GUIDE.md)를 먼저 읽습니다. 터미널, Visual Studio 메뉴, 패키지 관리자 콘솔의 설치 방법을 모두 확인할 수 있습니다.

`LiteDatabase`, `GetCollection()`, `Insert()`, `FindAll()`의 역할은 [LiteDB API 빠른 참조](Supplement/LITEDB_API_REFERENCE.md)에서 다시 확인할 수 있습니다.

1. 4일차의 `GameDatabaseLab` 프로젝트 폴더를 엽니다.
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

### 안내형 실습: 첫 문서 저장

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

#### 이 단어는 무슨 뜻인가요?

- **POCO 클래스** (Plain Old CLR Object, CLR은 Common Language Runtime): DB 전용 부모 클래스 없이 작성하는 일반 C# 클래스입니다.
- **컬렉션**: 같은 성격의 문서 모음입니다. 여기서는 `logs`입니다.
- **문서 ID (`_id`)**: 한 컬렉션 안에서 문서 한 건을 고유하게 구분하는 값입니다. 관계형 DB의 기본 키와 비슷한 역할을 합니다.
- **문서 CRUD**: `Insert`, `Find`, `Update`, `Delete`로 문서를 다룹니다.

#### 문서형 NoSQL DB에도 Primary Key가 있나요?

있습니다. LiteDB의 모든 문서는 `_id`라는 고유 ID를 가지며, C# 클래스에서 보통 `Id` 속성이 그 값으로 저장됩니다.

```csharp
public class GameLog
{
    public int Id { get; set; } // LiteDB 문서의 _id 역할
    public string EventType { get; set; } = "";
}
```

`Id`는 같은 컬렉션 안에서 중복될 수 없습니다. 그래서 `Update()`나 `Delete()`가 어느 문서를 수정·삭제할지 구분할 수 있습니다. 다만 LiteDB의 `_id`는 관계형 DB의 `FOREIGN KEY`처럼 다른 컬렉션에 있는 ID의 존재를 자동으로 검사하지는 않습니다. 컬렉션 사이 관계가 복잡하고 강한 참조 규칙이 필요하면 관계형 DB 설계를 우선 검토합니다.

#### `Insert()`하면 Id는 자동으로 생기나요?

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

#### `_id`를 실제로 사용하는 흐름

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

### 실습: 로그를 더 자세히 남기기

`GameLog`에 아래 중 하나를 추가합니다.

- `ItemId`
- `GoldBefore`
- `ClientVersion`

그 뒤 한 로그에는 새 값을 넣고, 이전 로그에는 값을 넣지 않아도 프로그램이 읽히는지 확인합니다.

#### 완료 확인

- [ ] `GameLogs.db`와 `logs` 컬렉션이 생성됐다.
- [ ] 새 로그의 `Id`가 자동으로 부여됐음을 확인했다.
- [ ] 선택 항목이 없는 기존 로그도 읽을 수 있음을 확인했다.

### 응용 실습: 특정 플레이어 로그 찾기

PlayerId를 입력받아 그 플레이어의 로그만 출력하세요. 입력이 숫자가 아닐 때는 조회하지 않고 다시 입력하도록 처리하세요.

### 오늘의 정리

- LiteDB는 NuGet으로 프로젝트에 추가하며 별도 서버 설치가 필요 없습니다.
- 다음 시간에는 퀘스트 진행처럼 중첩된 문서를 설계하고, SQLite와 선택 기준을 비교합니다.
