# DAY 10: LiteDB 문서 설계와 테스트 (4교시)

오늘은 퀘스트 진행 문서를 저장·수정·조회하고, 관계형 DB와 문서형 DB의 선택 기준을 다시 확인합니다.

`FindOne()`, `Update()`, `Delete()`, `EnsureIndex()`의 사용법은 [LiteDB API 빠른 참조](Supplement/LITEDB_API_REFERENCE.md)를 참고합니다. 두 DB의 코드 관점 차이는 [SQLite와 LiteDB API 비교](Supplement/SQLITE_LITEDB_API_COMPARISON.md)에서 확인합니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 프로그래밍하기
- 주요 학습 내용: 비관계형 DB 생성·관리 프로그램 작성, 테스트로 코드 완성하기

## 1. 중첩된 문서는 언제 편할까요?

퀘스트 진행은 목표, 보상, 현재 상태를 함께 읽는 일이 많습니다. 이때 관련 정보를 하나의 문서에 묶을 수 있습니다.

```csharp
using System.Collections.Generic;

public class QuestProgress
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string QuestId { get; set; } = "";
    public int KillCount { get; set; }
    public int TargetCount { get; set; }
    public bool IsCompleted { get; set; }
    public List<string> RewardIds { get; set; } = new List<string>();
}
```

## 2. 실습: 퀘스트 진행 갱신

**미션:** 고블린을 한 마리 처치할 때마다 `KillCount`를 올리고, 목표에 도달하면 완료 상태를 바꿉니다.

```csharp
QuestProgress quest = quests.FindOne(x =>
    x.PlayerId == 1 && x.QuestId == "GoblinHunt");

if (quest != null)
{
    quest.KillCount++;
    quest.IsCompleted = quest.KillCount >= quest.TargetCount;
    quests.Update(quest);
}
```

코드를 읽는 순서입니다.

1. 위에서 아래로: 조건에 맞는 퀘스트를 찾습니다.
2. 오른쪽에서 왼쪽으로: 처치 수를 하나 늘립니다.
3. 안에서 밖으로: 목표 수와 비교해 완료 여부를 계산한 뒤 문서를 저장합니다.

## 3. SQLite와 LiteDB를 고르는 질문

| 질문 | 예 | 더 어울리는 선택 |
| :--- | :--- | :--- |
| 여러 표를 정확히 연결하고 거래해야 하는가? | 재화, 인벤토리, 구매 | SQLite |
| 한 대상의 기록을 통째로 읽고 구조가 달라질 수 있는가? | 오류 로그, 퀘스트 스냅샷 | LiteDB |
| 여러 사용자가 네트워크로 동시에 접속하는가? | 온라인 게임 서버 | 서버형 DB를 별도 검토 |

> SQLite와 LiteDB는 수업과 로컬 도구에 좋습니다. 실제 온라인 게임에서는 Unity 클라이언트가 DB 파일을 직접 변경하지 않고, 서버/API를 거쳐 서버형 DB를 사용합니다.

### 문서 ID와 게임 검색 조건

LiteDB의 `_id`는 이미 알고 있는 문서 한 건을 다시 열거나 수정·삭제할 때 사용합니다. 반면 "1번 플레이어의 진행 중 퀘스트"처럼 게임 의미로 찾을 때는 `PlayerId`, `QuestId`를 함께 조건으로 사용합니다. 자세한 API 차이는 [LiteDB API 빠른 참조](Supplement/LITEDB_API_REFERENCE.md)의 `_id` 검색과 게임 조건 검색 항목을 확인합니다.

## 4. 테스트 체크

| 번호 | 상황 | 기대 결과 |
| :--- | :--- | :--- |
| 1 | 새 퀘스트 문서 생성 | `GameLogs.db`에 문서가 추가됨 |
| 2 | 고블린 1마리 처치 | `KillCount`가 1 증가 |
| 3 | 목표 수 도달 | `IsCompleted`가 `true` |
| 4 | 선택 항목 없는 이전 문서 읽기 | 프로그램이 중단되지 않음 |

## 오늘의 정리

- 문서형 DB도 설계와 테스트가 필요합니다.
- 다음 시간에는 Unity가 DB 데이터를 어떻게 보여 주고, 온라인 게임에서는 왜 서버를 거쳐야 하는지 확인합니다.
