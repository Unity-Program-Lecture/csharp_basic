# 참고: LiteDB API 빠른 참조

이 문서는 DAY09~DAY10에서 사용하는 LiteDB C# API를 찾아보는 참고 자료입니다. 패키지 설치는 [NuGet 패키지 사용 가이드](NUGET_PACKAGE_GUIDE.md)를 먼저 확인합니다.

> LiteDB는 문서형 DB입니다. C# 클래스 한 개를 문서 한 건처럼 저장하고, 같은 성격의 문서를 컬렉션으로 모아 관리합니다.

## API 전체 흐름

```text
LiteDatabase 생성
    -> GetCollection<T>()
        -> Insert / Find / FindOne / Update / Delete / EnsureIndex
    -> Dispose
```

## 1. `LiteDatabase`: DB 파일 열기

```csharp
using (LiteDatabase database = new LiteDatabase("GameLogs.db"))
{
    // 컬렉션을 가져와 문서를 관리합니다.
}
```

파일이 없으면 LiteDB가 새 DB 파일을 만듭니다. `using (...)` 블록이 끝나면 파일을 닫습니다.

## 2. `GetCollection<T>()`: 문서 상자 가져오기

```csharp
ILiteCollection<GameLog> logs =
    database.GetCollection<GameLog>("logs");
```

| 부분 | 뜻 |
| :--- | :--- |
| `GameLog` | 저장할 C# 클래스의 자료형 |
| `logs` | 컬렉션 이름 |
| `ILiteCollection<GameLog>` | GameLog 문서를 관리하는 객체 |

컬렉션은 처음 `Insert()` 또는 `EnsureIndex()`를 실행할 때 자동 생성될 수 있습니다. 단순 조회만 한다고 새 컬렉션이 생기지는 않습니다.

### LiteDB의 문서 ID: `_id`

문서형 DB에도 한 문서를 구분하는 고유 ID가 필요합니다. LiteDB는 각 문서에 `_id`를 두며, 일반 C# 클래스에서는 보통 `Id` 속성이 `_id`로 매핑됩니다.

```csharp
public class GameLog
{
    public int Id { get; set; } // LiteDB의 _id
    public int PlayerId { get; set; }
    public string Message { get; set; } = "";
}
```

| 관계형 DB | LiteDB 문서형 DB |
| :--- | :--- |
| `PRIMARY KEY` | 컬렉션 안의 `_id` |
| 행 하나를 고유하게 구분 | 문서 하나를 고유하게 구분 |
| 다른 표에 `FOREIGN KEY` 제약 가능 | 다른 컬렉션 ID의 존재를 자동 검사하는 외래 키 제약은 없음 |

따라서 LiteDB에서도 `Id`를 중복시키면 안 됩니다. `Update()`와 `Delete(id)`는 이 ID를 기준으로 어떤 문서를 다룰지 판단합니다.

`int Id`처럼 숫자 ID를 쓰는 초급 예제에서는 새 객체의 `Id`가 기본값 `0`이면 `Insert()`가 자동 번호를 부여합니다. 삽입 뒤에는 객체의 `Id`에도 그 번호가 반영됩니다. 이미 사용한 ID를 직접 지정해 `Insert()`하면 중복 오류가 납니다.

## 3. 문서 CRUD API

| API | 역할 | 로그 예시 |
| :--- | :--- | :--- |
| `Insert(document)` | 새 문서 추가 | 구매 실패 로그 남기기 |
| `FindAll()` | 모든 문서 읽기 | 전체 로그 출력 |
| `Find(condition)` | 조건에 맞는 여러 문서 읽기 | 1번 플레이어 로그 찾기 |
| `FindOne(condition)` | 조건에 맞는 문서 한 건 읽기 | 진행 중인 퀘스트 찾기 |
| `FindById(id)` | `_id`가 같은 문서 한 건 읽기 | 선택한 로그 한 건 다시 열기 |
| `Update(document)` | 기존 문서 수정 | 처치 수 증가 |
| `Delete(id)` | ID로 문서 삭제 | 테스트 로그 삭제 |
| `EnsureIndex(condition)` | 자주 찾는 값에 인덱스 준비 | PlayerId 조회 빠르게 하기 |

### 새 로그 넣기: `Insert()`

```csharp
logs.Insert(new GameLog
{
    EventType = "PurchaseFailed",
    PlayerId = 1,
    Message = "골드가 부족합니다.",
    OccurredAt = DateTime.Now
});
```

### 로그 찾기: `_id` 검색과 게임 조건 검색

`_id`는 이미 알고 있는 **정확한 문서 한 건**을 다시 찾을 때 사용합니다. 예를 들어 목록에서 사용자가 선택한 로그 ID가 12라면 다음처럼 찾습니다.

```csharp
GameLog selectedLog = logs.FindById(12);
```

자동으로 부여된 `_id`는 `Insert(log)` 뒤의 `log.Id`에서 확인할 수 있습니다. 목록 화면에서는 `Id`를 함께 출력하고, 사용자가 고른 번호를 `FindById()`에 전달합니다.

```csharp
foreach (GameLog log in logs.FindAll())
{
    Console.WriteLine(log.Id + ": " + log.EventType);
}

GameLog selectedLog = logs.FindById(12);
```

하지만 게임에서 "1번 플레이어의 구매 실패 로그를 모두 보여 주세요"처럼 **의미 있는 조건**으로 찾을 때는 `PlayerId`, `EventType`, `QuestId` 같은 필드를 사용합니다.

```csharp
foreach (GameLog log in logs.Find(x => x.PlayerId == 1))
{
    Console.WriteLine(log.Message);
}

GameLog questStartLog = logs.FindOne(x => x.EventType == "QuestStarted");
```

`Find()`는 여러 건을, `FindOne()`은 한 건을 찾습니다. 찾는 문서가 없을 수 있으므로 `FindOne()`의 결과를 사용하기 전에는 `null`인지 확인합니다.

| 상황 | 더 알맞은 검색 |
| :--- | :--- |
| 화면에서 선택한 "로그 12번"을 다시 열기 | `FindById(12)` |
| 1번 플레이어의 모든 로그 보기 | `Find(x => x.PlayerId == 1)` |
| 플레이어 1번의 고블린 퀘스트 진행 찾기 | `FindOne(x => x.PlayerId == 1 && x.QuestId == "GoblinHunt")` |

즉, `_id`는 문서의 **주소**, `PlayerId`·`QuestId` 등은 게임에서 무엇을 찾는지 설명하는 **검색 조건**이라고 생각하면 됩니다.

### 문서 수정: `Update()`

```csharp
QuestProgress quest = quests.FindOne(x =>
    x.PlayerId == 1 && x.QuestId == "GoblinHunt");

if (quest != null)
{
    quest.KillCount = quest.KillCount + 1;
    quests.Update(quest);
}
```

`Update()`는 이미 저장된 문서의 ID를 기준으로 수정합니다. 따라서 먼저 `FindOne()`으로 문서를 찾고 값을 바꾼 뒤 `Update()`를 호출합니다.

### 문서 삭제: `Delete()`

```csharp
bool deleted = logs.Delete(1);
Console.WriteLine("삭제 성공: " + deleted);
```

`Delete(1)`의 `1`은 문서의 ID입니다. 조건으로 여러 문서를 지우는 기능을 바로 사용하기보다, 수업에서는 먼저 찾은 문서와 ID를 확인하고 삭제합니다.

## 4. `EnsureIndex()`: 자주 찾는 길을 미리 만들기

```csharp
logs.EnsureIndex(x => x.PlayerId);
```

인덱스는 책의 색인처럼 특정 값을 찾는 길을 미리 만들어 줍니다. `PlayerId`로 로그를 자주 찾는다면 도움이 됩니다. 하지만 모든 필드에 무조건 인덱스를 만들면 저장·수정 비용이 늘 수 있으므로, 자주 조회하는 조건에만 만듭니다.

## 5. SQLite API와 비교

| 목적 | SQLite | LiteDB |
| :--- | :--- | :--- |
| DB 열기 | `SqliteConnection` | `LiteDatabase` |
| 데이터 묶음 | 표 | 컬렉션 |
| 새 데이터 | SQL `INSERT` + `ExecuteNonQuery()` | `Insert()` |
| 한 건/여러 건 찾기 | SQL `SELECT` + Reader | `FindOne()` / `Find()` |
| 수정 | SQL `UPDATE` | `Update()` |
| 삭제 | SQL `DELETE` | `Delete()` |
| 빠른 조회 준비 | SQL 인덱스 | `EnsureIndex()` |

## 6. 자주 발생하는 문제

| 증상 | 먼저 확인할 것 |
| :--- | :--- |
| `using LiteDB;`에 빨간 줄 | NuGet 패키지가 현재 프로젝트에 설치됐는지 |
| 컬렉션이 비어 있음 | `Insert()`를 호출했는지, 다른 DB 파일을 열지 않았는지 |
| `FindOne()` 뒤 오류 | 찾는 문서가 없어 `null`일 수 있는지 |
| `Update()`가 실패 | 수정할 문서의 ID가 있는지 |
| SQLite 도구로 DB를 열 수 없음 | LiteDB 파일은 SQLite 형식이 아님 |

## 공식 참고

- [LiteDB 시작하기](https://www.litedb.org/docs/getting-started/)
- [LiteDB 컬렉션 API](https://www.litedb.org/docs/collections/)
