# DAY 11: Unity 6와 게임 데이터 연결하기 (4교시)

오늘은 Unity의 기획 데이터, 플레이 데이터, DB의 책임을 구분하고, 앞에서 만든 SQLite `GameShop.db`를 Unity에서 실제로 읽어 상점 결과를 UI (User Interface, 사용자 인터페이스)로 보여 줍니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 응용 프로그래밍하기
- 주요 학습 내용: 게임 데이터를 관리하는 응용 프로그램 구현·유지보수의 확장 이해

## 1. Unity와 DB의 역할

```text
학습용 로컬 구조
Unity 상점 UI -> C# 데이터 관리 코드 -> SQLite 또는 LiteDB 파일

온라인 게임의 기본 구조
Unity 클라이언트 -> 게임 서버/API (Application Programming Interface, 응용 프로그래밍 인터페이스) -> 서버형 데이터베이스
```

Unity는 화면과 입력을 담당하고, DB는 데이터를 보관합니다. 온라인 게임에서 클라이언트가 DB 비밀번호를 갖고 직접 골드를 바꾸면 조작과 보안 문제가 생길 수 있으므로 서버가 규칙을 확인해야 합니다.

## 2. UPM (Unity Package Manager)으로 SQLite 연결하기

DAY11의 기본 실제 연동은 NuGet이 아니라 Unity Package Manager (UPM)로 설치하는 `com.gilzoide.sqlite-net`입니다. 이 패키지는 Windows, macOS, Linux, Android, iOS, WebGL (Web Graphics Library)을 지원한다고 안내하며, WebGL에서는 IndexedDB를 사용해 SQLite 데이터를 보관합니다.

1. [Unity용 SQLite-net 사용 가이드](Supplement/UNITY_SQLITE_NET_GUIDE.md)를 따라 UPM Git URL (Uniform Resource Locator, 웹 주소)로 패키지를 설치합니다.
2. DAY01에서 시작하고 DAY06~07에서 확장한 `GameShop.db`의 복사본을 Unity 프로젝트에 준비합니다.
3. `Item` 표를 `SELECT`하여 Unity Console에 출력합니다.
4. 조회 결과를 `ShopView`에 전달해 UI에 표시합니다.

> `Microsoft.Data.Sqlite`를 NuGetForUnity로 설치하는 방식은 이 수업의 기본 경로가 아닙니다. Unity용 네이티브 라이브러리와 WebGL 처리를 포함한 전용 UPM 패키지를 사용하면, 앞에서 만든 SQLite 파일을 더 직접적으로 연결할 수 있습니다.

## 3. ScriptableObject와 DB를 구분하기

| 데이터 | Unity 6에서의 추천 위치 | 이유 |
| :--- | :--- | :--- |
| 포션 이름, 아이콘, 설명 | `ItemDefinition` ScriptableObject | 빌드에 포함되는 기획 원본 |
| 포션 기본 가격 | ScriptableObject 또는 서버의 Item 데이터 | 기획자가 관리하는 공통 규칙 |
| 플레이어 골드, 보유 수량 | DB 또는 저장 데이터 | 플레이에 따라 달라짐 |
| 구매 실패 기록 | SQLite 로그 표 또는 서버 로그 | 문제 추적용 기록 |

## 4. 안내형 실습: Unity 6 상점 결과 표시와 SQLite 읽기

1. Unity 6에서 빈 씬을 만들고 `ShopLab`으로 저장합니다.
2. `Canvas` 안에 TextMeshPro 텍스트 두 개를 만듭니다: `GoldText`, `PotionText`.
3. `Buy Potion` 버튼 하나를 만듭니다.
4. [Unity용 SQLite-net 사용 가이드](Supplement/UNITY_SQLITE_NET_GUIDE.md)의 `GameShopDatabaseReader`를 빈 GameObject에 붙입니다. `SQLiteAsset`은 Unity 프로젝트에 넣은 읽기 전용 SQLite DB 원본을 Inspector에서 연결할 수 있게 하는 패키지의 자산 타입입니다.
5. Inspector에서 `Item Text`에 `PotionText`를 연결합니다.
6. Play Mode에서 Unity Console과 화면에 `Item` 표의 항목이 출력되는지 확인합니다.

<details>
<summary>UI 표시 스크립트</summary>

```csharp
using TMPro;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text potionText;

    public void SetInventory(int gold, int potionCount)
    {
        goldText.text = $"Gold: {gold}";
        potionText.text = $"Potion: {potionCount}";
    }
}
```

</details>

| Inspector 항목 | 연결할 대상 | 확인할 점 |
| :--- | :--- | :--- |
| `Gold Text` | GoldText | 골드 표시 TextMeshProUGUI |
| `Potion Text` | PotionText | 포션 표시 TextMeshProUGUI |

> 이 수업의 핵심은 Unity가 DB 파일을 직접 운영하는 기술이 아니라, 데이터 변경 결과를 화면에 반영하는 책임 분리입니다.

## 5. 실습 미션

- DB에서 읽었다고 가정한 골드 100, 포션 0을 먼저 표시합니다.
- 구매 성공 뒤 골드 70, 포션 1을 표시합니다.
- 구매 실패 뒤에는 원래 값이 유지됨을 표시합니다.
- 세 장면을 캡처하고 어떤 데이터가 바뀌었는지 설명합니다.
- `GameShop.db`의 `Item` 한 건이 Unity Console과 화면에 출력되는 장면을 확인합니다.

### 완료 확인

- [ ] `GameShop.sqlite` 원본과 `SQLiteAsset` 연결을 Inspector에서 확인했다.
- [ ] Item 한 건이 Unity Console과 화면에 함께 표시된다.
- [ ] 읽기 전용 원본과 `persistentDataPath`에 둘 저장용 DB의 역할 차이를 설명할 수 있다.

## 응용 실습: 읽기 실패 안내

`Item` 표가 비어 있거나 DB 연결이 실패했을 때, 화면에 원인을 알리는 문장을 표시하세요. 정상 아이템 표시와 오류 안내가 동시에 나오지 않아야 합니다.

## 6. 선택 확장 실습: Unity에서 LiteDB 로그 표시하기

DAY09에서 만든 문서형 로그도 Unity에서 읽어 표시할 수 있습니다. 이 실습은 Windows Editor 학습용이며, 기본 SQLite 실습을 완료한 학생이 진행합니다.

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

### 선택 확장 완료 확인

- [ ] `GameLogs.db`를 `StreamingAssets`에 복사했다.
- [ ] 첫 로그가 Unity Console과 화면에 함께 표시된다.
- [ ] LiteDB 로그 원본과 실행용 복사본의 역할을 설명할 수 있다.

## 오늘의 정리

- Unity UI는 DB의 결과를 보여 주는 소비자입니다.
- `com.gilzoide.sqlite-net`은 Unity에서 SQLite 파일을 읽는 DAY11의 UPM 패키지입니다.
- NuGetForUnity는 LiteDB처럼 NuGet으로 배포되는 라이브러리를 Unity에서 시험할 때 쓰는 별첨 도구입니다.
- 다음 시간에는 설계부터 트랜잭션, 테스트, 유지보수까지 한 번에 점검합니다.
