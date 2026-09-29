# 7일차: LiteDB 문서 설계와 Unity 연결 (6교시)

오늘은 6일차에서 만든 LiteDB 연결과 기본 로그 CRUD를 바탕으로 퀘스트 진행 문서를 저장·수정·조회합니다. `LiteDatabase`, 컬렉션, 기본 `Insert()` 사용법은 반복하지 않고 중첩 문서·수정·인덱스·테스트에 집중합니다.

`FindOne()`, `Update()`, `Delete()`, `EnsureIndex()`의 사용법은 [LiteDB API 빠른 참조](Supplement/LITEDB_API_REFERENCE.md)를 참고합니다. 두 DB의 코드 관점 차이는 [SQLite와 LiteDB API 비교](Supplement/SQLITE_LITEDB_API_COMPARISON.md)에서 확인합니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 프로그래밍하기, 게임 데이터베이스 응용 프로그래밍하기
- 주요 학습 내용: 문서형 NoSQL DB 생성·관리 프로그램 작성, 테스트로 코드 완성하기, 게임 데이터를 관리하는 응용 프로그램 구현·유지보수의 확장 이해

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

## 2. 안내형 실습: 퀘스트 진행 갱신

**미션:** 콘솔에서 `1`을 입력할 때마다 고블린을 한 마리 처치하고, 저장된 처치 수와 완료 여부를 바로 출력합니다. `r`을 입력하면 고블린 퀘스트의 저장된 진행 상태를 초기화하고, `q`를 입력하면 종료합니다. 프로그램을 다시 실행해도 기존 진행 상태에서 이어서 시작합니다.

```csharp
using System;
using System.Collections.Generic;
using LiteDB;

namespace GameDatabaseLab
{
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

    internal class Program
    {
        private static void Main(string[] args)
        {
            using (LiteDatabase database = new LiteDatabase("QuestProgress.db"))
            {
                ILiteCollection<QuestProgress> quests =
                    database.GetCollection<QuestProgress>("quests");
                quests.EnsureIndex(x => x.PlayerId);

                QuestProgress quest = quests.FindOne(x =>
                    x.PlayerId == 1 && x.QuestId == "GoblinHunt");

                if (quest == null)
                {
                    quest = new QuestProgress
                    {
                        PlayerId = 1,
                        QuestId = "GoblinHunt",
                        TargetCount = 3,
                        RewardIds = new List<string> { "Potion" }
                    };
                    quests.Insert(quest);
                }

                Console.WriteLine("고블린 퀘스트를 불러왔습니다.");
                Console.WriteLine("처치 수: " + quest.KillCount + "/" + quest.TargetCount);
                Console.WriteLine("완료 여부: " + quest.IsCompleted);

                while (true)
                {
                    Console.Write("1: 고블린 한 마리 처치 | r: 진행 상태 초기화 | q: 종료 > ");
                    string input = Console.ReadLine();

                    if (input == null || input == "q" || input == "Q")
                    {
                        break;
                    }

                    if (input == "r" || input == "R")
                    {
                        quest.KillCount = 0;
                        quest.IsCompleted = false;
                        quests.Update(quest);

                        Console.WriteLine("고블린 퀘스트 진행 상태를 초기화했습니다.");
                        Console.WriteLine("처치 수: " + quest.KillCount + "/" + quest.TargetCount);
                        Console.WriteLine("완료 여부: " + quest.IsCompleted);
                        continue;
                    }

                    if (input != "1")
                    {
                        Console.WriteLine("1, r 또는 q를 입력하세요.");
                        continue;
                    }

                    if (quest.IsCompleted)
                    {
                        Console.WriteLine("이미 완료한 퀘스트입니다.");
                        continue;
                    }

                    quest.KillCount++;
                    quest.IsCompleted = quest.KillCount >= quest.TargetCount;
                    quests.Update(quest);

                    Console.WriteLine("고블린을 한 마리 처치했습니다.");
                    Console.WriteLine("처치 수: " + quest.KillCount + "/" + quest.TargetCount);
                    Console.WriteLine("완료 여부: " + quest.IsCompleted);
                }
            }
        }
    }
}
```

같은 코드를 실행 가능한 프로젝트로 준비한 [QuestProgressConsole](../../../Code/GameDatabase/QuestProgressConsole/QuestProgressConsole.csproj)에서도 확인할 수 있습니다. 프로젝트 폴더에서 `dotnet run`을 실행하면 `QuestProgress.db`가 그 폴더에 생성됩니다.

코드를 읽는 순서입니다.

1. 위에서 아래로: 조건에 맞는 퀘스트를 찾고 현재 상태를 출력한 뒤 입력을 기다립니다.
2. 오른쪽에서 왼쪽으로: `1`이면 처치 수에 1을 더하고, `r`이면 처치 수를 0·완료 여부를 `false`로 되돌립니다.
3. 안에서 밖으로: 변경한 값을 `Update()`로 문서에 저장한 뒤 결과를 출력합니다.

`q`를 입력하거나 입력이 끝나면 반복문을 나갑니다. 목표에 도달한 뒤 `1`을 다시 입력해도 처치 수를 더하지 않습니다. `r`은 DB 파일을 삭제하거나 다른 퀘스트를 지우는 명령이 아니라, 현재 고블린 퀘스트 한 건의 진행 값만 `0/3`, `false`로 다시 저장합니다. **프로그램 실행 자체는 처치로 계산하지 않습니다.**

아래 테스트의 처음 상태 `0/3`은 해당 작업 폴더에 고블린 퀘스트 문서가 아직 없을 때의 결과입니다. 이미 실습한 `QuestProgress.db`가 있다면 저장된 값부터 표시됩니다. 재실행 테스트에서는 같은 작업 폴더의 DB 파일을 사용하세요.

## 3. 선택 기준을 실제 구조에 적용하기

1일차와 6일차에서 정한 선택 기준을 퀘스트 문서에 적용합니다. 지금 만든 `QuestProgress`는 목표·보상·진행 상태를 함께 읽으므로 LiteDB 문서로 다룹니다. 반대로 재화·인벤토리·구매처럼 여러 표를 정확히 연결하고 거래해야 하는 데이터는 SQLite가 더 알맞습니다.

> SQLite와 LiteDB는 수업과 로컬 도구에 좋습니다. 여러 사용자가 네트워크로 동시에 접속하는 온라인 게임에서는 Unity 클라이언트가 DB 파일을 직접 변경하지 않고, 서버/API를 거쳐 서버형 DB를 사용합니다.

### 문서 ID와 게임 검색 조건

LiteDB의 `_id`는 이미 알고 있는 문서 한 건을 다시 열거나 수정·삭제할 때 사용합니다. 반면 "1번 플레이어의 진행 중 퀘스트"처럼 게임 의미로 찾을 때는 `PlayerId`, `QuestId`를 함께 조건으로 사용합니다. 자세한 API 차이는 [LiteDB API 빠른 참조](Supplement/LITEDB_API_REFERENCE.md)의 `_id` 검색과 게임 조건 검색 항목을 확인합니다.

## 4. 테스트 체크

| 번호 | 상황 | 기대 결과 |
| :--- | :--- | :--- |
| 1 | 새 퀘스트 문서 생성 | `QuestProgress.db`의 `quests` 컬렉션에 문서가 추가됨 |
| 2 | 콘솔에서 `1`을 한 번 입력 | `KillCount`가 1 증가하고 결과가 출력됨 |
| 3 | `1`을 세 번 입력해 목표 수 도달 | `KillCount`가 3, `IsCompleted`가 `true` |
| 4 | `q`로 종료한 뒤 다시 실행 | 입력 없이 처치 수가 늘지 않고 이전 값이 출력됨 |
| 5 | 완료 후 `1`을 다시 입력 | 완료 안내가 출력되고 처치 수가 3으로 유지됨 |
| 6 | 완료 후 `r`을 입력하고 재실행 | 처치 수 `0/3`, 완료 여부 `false`가 저장·출력됨 |

### 완료 확인

- [ ] `QuestProgress.db`와 `quests` 컬렉션을 만들었다.
- [ ] 새 DB에서 처음 실행할 때 고블린 퀘스트가 생성되고 처치 수가 0으로 표시됐다.
- [ ] `1`을 입력할 때마다 처치 수가 하나씩 증가해 세 번째 입력 후 `IsCompleted`가 `true`가 됐다.
- [ ] 종료 후 재실행해도 처치 수가 유지되고, 추가 입력 없이 증가하지 않았다.
- [ ] `r`로 초기화한 뒤 재실행해도 `0/3`, `False`가 유지됐다.

## 응용 실습: 보상 수령 상태

`IsRewardClaimed` 속성을 추가하고, 완료 전에는 보상을 받지 못하게 하세요. 이미 받은 보상을 다시 받으려 할 때 출력할 메시지도 정하세요.

## 전반 정리 및 다음 교시

- 문서형 NoSQL DB도 설계와 테스트가 필요합니다.
- 후반 교시에는 문서형 LiteDB 실습을 잠시 마무리하고, 관계형 SQLite 게임 데이터를 Unity 화면에 표시하는 별도 역할을 확인합니다.

---

## 7일차 후반: Unity 6와 게임 데이터 연결하기

전반의 `QuestProgress.db`는 LiteDB 문서 설계·갱신을 익히기 위한 결과물입니다. 후반에는 초점을 바꾸어, 2~5일차에 사용한 관계형 SQLite `GameShop.db`의 `Item` 표를 Unity에서 읽고 아이템 이름과 가격을 UI (User Interface, 사용자 인터페이스)에 보여 줍니다. 이 실습은 **읽기 전용 조회**입니다. 구매 버튼, 골드 차감, 인벤토리 변경은 여기서 구현하지 않습니다.

### Unity와 DB의 역할

```text
이번 학습용 로컬 구조
GameShop.sqlite 읽기 전용 자산 -> GameShopDatabaseReader -> Unity 아이템 텍스트

앞서 만든 구매 프로그램
C# 콘솔 프로그램 -> 쓰기 가능한 GameShop.db -> 골드·인벤토리 변경

온라인 게임의 기본 구조
Unity 클라이언트 -> 게임 서버/API (Application Programming Interface, 응용 프로그래밍 인터페이스) -> 서버형 데이터베이스
```

Unity는 화면과 입력을 담당하고, DB는 데이터를 보관합니다. 온라인 게임에서 클라이언트가 DB 비밀번호를 갖고 직접 골드를 바꾸면 조작과 보안 문제가 생길 수 있으므로 서버가 규칙을 확인해야 합니다.

### UPM (Unity Package Manager)으로 SQLite 연결하기

Unity 실제 연동의 기본은 NuGet이 아니라 Unity Package Manager (UPM)로 설치하는 `com.gilzoide.sqlite-net`입니다. 이 패키지는 Windows, macOS, Linux, Android, iOS, WebGL (Web Graphics Library)을 지원한다고 안내하며, WebGL에서는 IndexedDB를 사용해 SQLite 데이터를 보관합니다.

1. [Unity용 SQLite-net 사용 가이드](Supplement/UNITY_SQLITE_NET_GUIDE.md)를 따라 UPM Git URL (Uniform Resource Locator, 웹 주소)로 패키지를 설치합니다.
2. 1일차에 시작하고 4일차에서 확장한 `GameShop.db`의 복사본을 Unity 프로젝트에 준비합니다.
3. `GameShopDatabaseReader`가 `Item` 표를 `SELECT`합니다.
4. 조회한 아이템 이름·가격을 같은 스크립트가 Unity Console과 UI에 표시합니다.

> `Microsoft.Data.Sqlite`를 NuGetForUnity로 설치하는 방식은 이 수업의 기본 경로가 아닙니다. Unity용 네이티브 라이브러리와 WebGL 처리를 포함한 전용 UPM 패키지를 사용하면, 앞에서 만든 SQLite 파일을 더 직접적으로 연결할 수 있습니다.

### ScriptableObject와 DB를 구분하기

| 데이터 | Unity 6에서의 추천 위치 | 이유 |
| :--- | :--- | :--- |
| 포션 이름, 아이콘, 설명 | `ItemDefinition` ScriptableObject | 빌드에 포함되는 기획 원본 |
| 포션 기본 가격 | ScriptableObject 또는 서버의 Item 데이터 | 기획자가 관리하는 공통 규칙 |
| 플레이어 골드, 보유 수량 | DB 또는 저장 데이터 | 플레이에 따라 달라짐 |
| 구매 실패 기록 | SQLite 로그 표 또는 서버 로그 | 문제 추적용 기록 |

위 표는 실제 게임에서 데이터를 배치할 때의 예시입니다. 이번 조회 실습은 SQL과 Unity 연결을 확인하려고 `Item` 이름·가격을 SQLite 복사본에서 읽습니다. 같은 값을 ScriptableObject에도 따로 넣어 두고 어느 쪽이 최신인지 비교하는 실습은 아닙니다.

### 안내형 실습: Unity 6에서 SQLite 아이템 읽기

1. [Unity용 SQLite-net 사용 가이드](Supplement/UNITY_SQLITE_NET_GUIDE.md)의 설치·DB 자산 준비 절차를 완료합니다. `SQLiteAsset`은 Unity에 넣은 읽기 전용 SQLite DB 원본을 Inspector에서 연결하는 패키지 자산 타입입니다.
2. Unity 6에서 빈 씬을 만들고 `ShopLab`으로 저장합니다.
3. `Canvas` 안에 TextMeshPro 텍스트 하나를 만들고 `ItemText`로 이름 붙입니다. 구매 버튼이나 골드·수량 텍스트는 만들지 않습니다.
4. 가이드 4절의 `GameShopDatabaseReader` 코드를 `Assets/Scripts/GameShopDatabaseReader.cs`에 입력하고 빈 GameObject에 붙입니다.
5. Inspector의 `Database Asset`에는 `GameShop.sqlite`, `Item Text`에는 `ItemText`를 연결합니다.
6. Play Mode에서 `GameShopDatabaseReader.Start()`가 `SELECT ItemId, Name, Price FROM Item ORDER BY ItemId`를 실행하는지 확인합니다. 첫 행의 이름·가격이 Unity Console과 `ItemText`에 함께 나타나야 합니다.

이 실습의 데이터 흐름은 **SQLite `Item` 행 → `ItemRow` 객체 → `ItemText`와 Console**입니다. 코드가 실제로 읽는 열은 `ItemId`, `Name`, `Price`뿐이므로 골드나 인벤토리 수량을 표시했다고 기록하지 않습니다. 콘솔 구매 프로그램의 결과를 Unity 화면에 자동 전달하는 코드도 없습니다.

### 실습 미션

- DB Browser에서 원본 `GameShop.db`의 첫 번째 `Item` 이름·가격을 확인합니다.
- Unity Play Mode에서 같은 값이 `ItemText`와 Console에 표시되는지 대조합니다.
- Play Mode를 멈추고 실습용 `GameShop.db` 복사본의 첫 아이템 가격을 SQL `UPDATE`로 바꾼 뒤 저장합니다. 바뀐 파일을 다시 `GameShop.sqlite`로 복사해 Unity가 자산을 다시 가져오게 하고, Play Mode에서 새 가격이 표시되는지 확인합니다. 앞서 가져온 Unity 자산은 콘솔의 DB 파일 변경을 자동으로 따라가지 않습니다.
- `ItemText`에 표시된 값이 코드에 적은 고정 문자열이 아니라 DB 조회 결과임을 설명합니다.

#### 완료 확인

- [ ] `GameShop.sqlite` 원본과 `SQLiteAsset` 연결을 Inspector에서 확인했다.
- [ ] DB의 Item 첫 행과 같은 이름·가격이 Unity Console과 `ItemText`에 함께 표시된다.
- [ ] 읽기 전용 원본과 `persistentDataPath`에 둘 저장용 DB의 역할 차이를 설명할 수 있다.

### 응용 실습: 읽기 실패 안내

`Item` 표가 비어 있거나 DB 연결이 실패했을 때, 화면에 원인을 알리는 문장을 표시하세요. 정상 아이템 표시와 오류 안내가 동시에 나오지 않아야 합니다.

### 선택 확장 실습 (수업 시간 외 또는 빠른 학생용): Unity에서 LiteDB 로그 표시하기

6일차에 만든 문서형 NoSQL 로그도 Unity에서 읽어 표시할 수 있습니다. 이 실습은 Windows Editor 학습용이며, 기본 SQLite 실습을 완료한 학생이 수업 시간 외 또는 남는 시간에 진행합니다.

1. [NuGetForUnity 가이드](Supplement/NUGET_FOR_UNITY_GUIDE.md)의 설치 절차를 따라 NuGetForUnity와 LiteDB 패키지를 추가합니다.
2. `GameLogs.db`를 `Assets/StreamingAssets/GameLogs.db`로 복사합니다.
3. 아래 `GameLogReader`를 빈 GameObject에 붙이고, `Log Text`에 TextMeshPro 텍스트를 연결합니다.

```csharp
using System.IO;
using LiteDB;
using TMPro;
using UnityEngine;

public class GameLog
{
    public int Id { get; set; }
    public string EventType { get; set; } = "";
    public int PlayerId { get; set; }
    public string Message { get; set; } = "";
}

public class GameLogReader : MonoBehaviour
{
    [SerializeField] private TMP_Text logText;

    private void Start()
    {
        string sourcePath = Path.Combine(
            Application.streamingAssetsPath,
            "GameLogs.db");
        string savePath = Path.Combine(
            Application.persistentDataPath,
            "GameLogs.db");

        if (!File.Exists(savePath))
        {
            File.Copy(sourcePath, savePath);
        }

        using (LiteDatabase database = new LiteDatabase(savePath))
        {
            ILiteCollection<GameLog> logs =
                database.GetCollection<GameLog>("logs");
            GameLog firstLog = logs.FindOne(x => x.Id > 0);

            if (firstLog != null)
            {
                logText.text = firstLog.EventType + ": " + firstLog.Message;
                Debug.Log(logText.text);
            }
            else
            {
                logText.text = "읽을 로그가 없습니다.";
            }
        }
    }
}
```

`StreamingAssets`는 빌드에 포함하는 읽기 전용 원본 위치이고, `persistentDataPath`는 실행 중 사용할 복사본 위치입니다. 위 복사 방식은 Windows Editor 학습 기준입니다. 실제 온라인 게임에서는 Unity 클라이언트가 운영 DB 파일을 직접 읽거나 바꾸지 않고 서버/API를 거칩니다.

#### 선택 확장 완료 확인

- [ ] `GameLogs.db`를 `StreamingAssets`에 복사했다.
- [ ] 첫 로그가 Unity Console과 화면에 함께 표시된다.
- [ ] LiteDB 로그 원본과 실행용 복사본의 역할을 설명할 수 있다.

### 오늘의 정리

- Unity UI는 DB의 결과를 보여 주는 소비자입니다.
- `com.gilzoide.sqlite-net`은 Unity에서 SQLite 파일을 읽는 7일차의 UPM 패키지입니다.
- NuGetForUnity는 LiteDB처럼 NuGet으로 배포되는 라이브러리를 Unity에서 시험할 때 쓰는 별첨 도구입니다.
- 다음 시간에는 설계부터 트랜잭션, 테스트, 유지보수까지 한 번에 점검합니다.
