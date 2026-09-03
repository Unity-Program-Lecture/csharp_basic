# 참고: NuGetForUnity로 Unity 프로젝트에 패키지 추가하기

이 문서는 Unity 6 프로젝트에서 NuGet 패키지를 설치하는 방법을 설명합니다. DAY11의 기본 SQLite 실습은 [Unity용 SQLite-net 사용 가이드](UNITY_SQLITE_NET_GUIDE.md)를 사용합니다. 이 문서는 LiteDB처럼 NuGet으로 배포되는 라이브러리를 Unity에서 별도로 시험할 때 사용합니다.

> NuGetForUnity는 Unity Editor 안에서 NuGet 패키지를 설치·관리하는 도구입니다. Visual Studio의 NuGet 관리자와 역할은 비슷하지만, 설치 위치와 Unity 호환성 확인 방법은 다릅니다.

## 1. 왜 별도 도구가 필요한가요?

Unity Package Manager는 Unity 패키지용이고, `LiteDB`, `Microsoft.Data.Sqlite` 같은 .NET 라이브러리는 NuGet 패키지로 배포됩니다. NuGetForUnity는 Unity Editor에서 NuGet 패키지를 검색·설치하는 창을 제공합니다.

```text
Unity Package Manager
    -> NuGetForUnity 설치
        -> Unity 메뉴의 NuGet 창
            -> LiteDB 설치
                -> Unity C# 스크립트에서 using LiteDB;
```

## 2. 시작 전 준비

- Unity 6 프로젝트를 먼저 백업하거나 Git 커밋합니다.
- 인터넷 연결과 Git 실행 가능 여부를 확인합니다. Git URL 방식은 Unity가 저장소를 내려받는 과정이 필요합니다.
- 이 문서의 실습은 **Windows Editor에서 LiteDB 파일을 읽는 학습용**입니다.
- 실제 온라인 게임은 Unity 클라이언트가 DB 파일을 직접 관리하지 않고 `Unity -> 서버/API -> DB` 구조를 사용합니다.

## 3. NuGetForUnity 설치하기

1. Unity 6 프로젝트를 엽니다.
2. 위 메뉴에서 `Window > Package Manager`를 엽니다.
3. 왼쪽 위의 `+` 버튼을 누릅니다.
4. `Add package from git URL...`을 선택합니다.
5. 아래 주소를 붙여 넣습니다.

```text
https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity
```

6. `Add`를 누르고 설치가 끝날 때까지 기다립니다.
7. 상단 메뉴에 `NuGet` 항목이 생기는지 확인합니다.

> 특정 버전으로 고정해야 할 때만 Git URL 끝에 `#v버전번호`를 붙입니다. 수업에서는 강사가 한 번 검증한 버전을 모든 학생이 같이 사용합니다.

## 4. LiteDB 설치하기

1. Unity 메뉴에서 `NuGet > Manage NuGet Packages`를 엽니다.
2. 검색창에 `LiteDB`를 입력합니다.
3. 패키지 ID가 정확히 `LiteDB`인지, 안정 버전인지 확인합니다. `Prerelease`는 선택하지 않습니다.
4. `Install`을 누르고, 의존성 설치 및 Unity 재컴파일이 끝날 때까지 기다립니다.
5. Unity Console에 컴파일 오류가 없는지 확인합니다.
6. Project 창에서 NuGetForUnity가 관리하는 패키지 폴더와 `packages.config`가 생성·갱신되었는지 확인합니다.

NuGetForUnity는 기본적으로 프로젝트 안에 NuGet 패키지 정보를 기록하고, 패키지 파일을 프로젝트의 설정 경로에 둡니다. 이 경로와 버전 관리 포함 여부는 NuGetForUnity 설정과 팀 규칙에 맞춰 강사가 먼저 정합니다.

## 5. DAY11에서 이 도구를 기본 경로로 쓰지 않는 이유

`Microsoft.Data.Sqlite`는 기본적으로 SQLitePCLRaw와 네이티브 SQLite 번들을 함께 사용합니다. .NET 콘솔 프로젝트에서는 편리하지만, Unity에서는 Editor·Windows 빌드·Android·iOS·IL2CPP (Intermediate Language To C++, 중간 언어를 C++로 변환하는 Unity 스크립팅 백엔드)마다 네이티브 라이브러리와 플랫폼 설정을 별도로 검증해야 할 수 있습니다.

따라서 DAY11의 기본 실습은 **UPM용 `com.gilzoide.sqlite-net`**으로 합니다. 해당 패키지는 Unity 플랫폼용 네이티브 라이브러리와 WebGL용 저장 처리를 포함합니다. LiteDB는 이 문서에서 다루는 NuGet 연습 및 문서형 DB 확장 실습으로 유지합니다.

| 목적 | 권장 선택 | 이유 |
| :--- | :--- | :--- |
| DAY06~08 관계형 DB·SQL·트랜잭션 학습 | .NET 콘솔 + Microsoft.Data.Sqlite | SQL과 관계형 설계를 분명히 학습 |
| DAY09~10 문서형 DB 학습 | .NET 콘솔 + LiteDB | 문서 CRUD 학습 |
| DAY11 Unity에서 실제 SQLite 파일 읽기 | Unity + `com.gilzoide.sqlite-net` | UPM으로 설치하며 주요 Unity 플랫폼과 WebGL 지원을 안내함 |
| Unity에서 LiteDB를 실제로 시험 | Unity + NuGetForUnity + LiteDB | 문서형 DB를 Unity에서 읽는 확장 실습 |

## 6. `GameLogs.db`를 Unity 프로젝트에 준비하기

1. DAY09에서 만든 `GameLogs.db`를 찾습니다.
2. Unity 프로젝트의 `Assets/StreamingAssets/` 폴더를 만듭니다. `StreamingAssets`가 없다면 Project 창에서 `Assets`를 선택하고 `Create > Folder`로 만듭니다.
3. `GameLogs.db`를 `Assets/StreamingAssets/GameLogs.db`로 복사합니다.
4. Project 창에서 파일이 보이는지 확인합니다.

`StreamingAssets`는 빌드에 원본 파일을 포함하기 위한 폴더입니다. 하지만 실행 중인 플랫폼에서 이 위치가 항상 쓸 수 있는 것은 아닙니다. 따라서 실제 앱에서는 처음 실행할 때 `Application.persistentDataPath`로 파일을 복사해 그 복사본을 읽고 수정합니다.

## 7. Unity에서 LiteDB 파일을 복사하고 읽기

아래 스크립트를 `Assets/Scripts/GameLogReader.cs`로 만들고 빈 GameObject에 붙입니다. `GameLogs.db`에서 첫 로그를 읽어 Unity Console에 출력합니다.

```csharp
using System.IO;
using LiteDB;
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
                Debug.Log(firstLog.EventType + ": " + firstLog.Message);
            }
            else
            {
                Debug.Log("읽을 로그가 없습니다.");
            }
        }
    }
}
```

### 코드 흐름

1. `StreamingAssets`의 원본 DB 경로와 `persistentDataPath`의 복사본 경로를 만듭니다.
2. 처음 실행이라 복사본이 없으면 원본을 복사합니다.
3. 복사본을 `LiteDatabase`로 엽니다.
4. `logs` 컬렉션에서 로그 한 건을 읽어 `Debug.Log()`로 출력합니다.

> 위 복사 방식은 Windows Editor 학습 실습 기준입니다. Android처럼 `StreamingAssets`를 일반 파일 경로로 직접 읽을 수 없는 플랫폼에서는 `UnityWebRequest` 등의 별도 복사 방법이 필요합니다. 이 과정에서는 해당 플랫폼 배포까지 다루지 않습니다.

## 8. ShopLab UI에 연결하기

DAY11의 `ShopView`에 아래 메서드를 추가하면, 첫 로그의 메시지를 TextMeshPro UI에 표시할 수 있습니다.

```csharp
[SerializeField] private TMP_Text logText;

public void SetLogMessage(string message)
{
    logText.text = message;
}
```

`GameLogReader`에서 `ShopView` 참조를 받은 뒤 `SetLogMessage(firstLog.Message)`를 호출하는 것은 다음 작은 확장 실습으로 둡니다. 먼저 Unity Console에 DB 내용이 출력되는지 확인한 뒤 UI 연결로 넘어갑니다.

## 9. 실패했을 때 확인 순서

| 증상 | 먼저 확인할 것 |
| :--- | :--- |
| Git URL 설치 실패 | Git 설치·인터넷·URL 전체 입력 여부 |
| `NuGet` 메뉴가 안 보임 | Package Manager 설치 완료와 Console 오류 여부 |
| `using LiteDB` 빨간 줄 | NuGet 창에서 LiteDB 설치 완료, Unity 재컴파일 여부 |
| `GameLogs.db`를 찾지 못함 | `Assets/StreamingAssets/GameLogs.db` 경로와 파일 이름 |
| 로그가 없다고 출력됨 | DAY09에서 `logs` 컬렉션에 문서를 넣었는지 |
| DB 잠금 또는 쓰기 오류 | DB Browser와 콘솔 앱이 파일을 열고 있지 않은지, `persistentDataPath` 복사본을 여는지 |

## 공식 참고

- [NuGetForUnity 공식 설치·사용 안내](https://github.com/GlitchEnzo/NuGetForUnity)
- [Unity Package Manager 창](https://docs.unity3d.com/Manual/upm-ui.html)
- [Microsoft.Data.Sqlite의 SQLitePCLRaw 번들](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/custom-versions)
