# DAY 13: 설정 파일을 다음 라운드에 적용하기

오늘은 DAY 12의 게임에 제한 시간 설정을 연결합니다. JSON 읽기에 성공하면 다음 라운드에 반영하고, 실패하면 기본 30초를 사용합니다. NCS G4·G6·G7·G8을 다룹니다.

## 1. 핵심 개념: 경기가 시작되기 전에 규칙표 읽기

진행 중인 경기의 시간을 갑자기 바꾸지 않습니다. 메뉴에서 설정을 확인한 뒤 다음 Start에 적용합니다. 서버가 없어도 로컬 설정과 기본값으로 같은 게임을 실행할 수 있어야 합니다.

### 이 단어는 무슨 뜻인가요?

- **StreamingAssets**: 원본 파일을 빌드에 포함해 읽는 영역입니다. 런타임 저장 폴더와 구분합니다.
- **JSON**: 필드 이름과 값을 텍스트로 표현하는 자료 형식입니다.
- **UnityWebRequest**: URL 기반 요청을 비동기로 수행하는 기능입니다.
- **자료 계약**: 주고받을 필드 이름·타입·허용 범위를 정한 약속입니다.

## 2. 실습: 로컬 설정을 게임에 연결

먼저 File > Build Profiles에서 Windows 프로필로 돌아와 컴파일이 끝날 때까지 기다립니다. Addressables Groups는 Default 프로필·CourseStage 로컬 경로·Use Asset Database로 둡니다. DAY 12의 Web 빌드 출력은 보관하되 Web용 번들을 Editor의 Use Existing Build로 읽는 상태에서 오늘 실습을 시작하지 않습니다.

**미션:** `Assets/StreamingAssets/platform-config.json`에 다음을 입력합니다.

```json
{"roundSeconds":45}
```

StreamingAssets 폴더가 없다면 Assets 아래에 만듭니다. `roundSeconds`는 10~120 범위의 정수로 약속합니다. `Assets/Scripts/Runtime/PlatformConfigLoader.cs`를 작성해 기존 GameSession 오브젝트에 붙이고 Session에 같은 GameSession 컴포넌트를 연결합니다. Remote Url은 비워 둡니다.

```csharp
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public sealed class PlatformConfig
{
    public int roundSeconds;
}

public sealed class PlatformConfigLoader : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private string remoteUrl;

    public void ApplySettings()
    {
        // 진행 중인 라운드의 규칙을 바꾸거나, 스테이지 준비와 설정 읽기가 겹치지 않게 합니다.
        if (session.IsPlaying || session.IsLoading || !session.IsReady)
        {
            return;
        }

        session.SetBusy(true);
        StartCoroutine(LoadSettings());
    }

    private IEnumerator LoadSettings()
    {
        string url = remoteUrl;
        if (string.IsNullOrWhiteSpace(url))
        {
            string path = Application.streamingAssetsPath + "/platform-config.json";
            // 이미 URL 형태이면 그대로 쓰고, PC의 파일 경로는 file:// 형식의 URL로 바꿉니다.
            url = path.Contains("://") ? path : new Uri(path).AbsoluteUri;
        }

        // 실패 경로마다 값을 다시 넣지 않아도 되도록, 기본값 30초를 먼저 준비합니다.
        int seconds = 30;
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = 10;

            // 응답을 기다리는 동안 제어를 Unity에 돌려줍니다. 요청이 끝난 뒤 아래 코드를 이어 실행합니다.
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    // HTTP 성공만으로는 충분하지 않습니다. 본문을 JSON으로 읽고 값의 범위까지 확인합니다.
                    PlatformConfig config = JsonUtility.FromJson<PlatformConfig>(request.downloadHandler.text);
                    // &&는 왼쪽부터 확인하므로 config가 null이면 그 안의 roundSeconds를 읽지 않습니다.
                    if (config != null
                        && config.roundSeconds >= 10
                        && config.roundSeconds <= 120)
                    {
                        seconds = config.roundSeconds;
                    }
                    else
                    {
                        Debug.LogWarning("Invalid roundSeconds; use default 30.");
                    }
                }
                catch (Exception error)
                {
                    Debug.LogWarning($"Invalid JSON: {error.Message}; use default 30.");
                }
            }
            else
            {
                Debug.LogWarning($"Config failed: {request.error}; use default 30.");
            }
        }

        // 설정값 또는 기본값을 다음 라운드에 적용한 뒤 시작 잠금을 풉니다.
        session.SetRoundSeconds(seconds);
        session.SetBusy(false);
        session.SetStatus($"Next round: {seconds} seconds");
    }
}
```

1. Canvas/SafeAreaRoot/MenuPanel 아래 UI Button으로 LoadSettingsButton을 만듭니다. 자식 텍스트는 `LOAD SETTINGS`, 크기는 `(340,54)`, Anchor·Pivot은 중앙, 위치는 `(0,-60)`으로 둡니다. 버튼 배경의 Raycast Target은 켜고 자식 텍스트는 끕니다. On Click에 `PlatformConfigLoader.ApplySettings()`를 연결합니다.
2. 스테이지 준비가 끝난 메뉴에서 Load Settings를 누릅니다. 읽는 동안 Start와 스테이지 재로드가 차단되는지 확인합니다.
3. Status에 다음 라운드가 45초라는 안내가 나오면 Start를 눌러 HUD가 45초부터 줄어드는지 확인합니다.
4. 결과에서 메뉴로 돌아와 설정을 다시 읽고 재시작합니다. Restart도 마지막으로 적용한 제한 시간을 사용합니다.
5. 플레이 중 설정 읽기를 직접 호출해도 무시되는지 확인합니다. 로더 오브젝트는 계속 활성 상태로 두어 코루틴이 중간에 중단되지 않게 합니다.

StageLoader와 설정 로더가 모두 `IsLoading`을 확인하므로 준비 작업을 동시에 시작하지 않습니다. 로딩은 자동 Start가 아니라 사용자 클릭 전에 마치며, 브라우저 오디오도 Start 클릭 이후에 확인합니다.

## 3. 실패 처리와 선택 원격 설정 실습

기본 완료본은 `Assets/StreamingAssets/platform-config.json`의 45초 설정을 읽습니다. PlatformConfigLoader는 기존 GameSession 오브젝트에 붙이고 Session 필드를 연결합니다. LoadSettingsButton의 Interactable을 켜고 On Click에 `PlatformConfigLoader.ApplySettings()`를 연결합니다. Remote Url은 비워 둡니다. 메뉴에서 Load Settings → 완료 안내 → Start 순서로 다음 라운드에 45초가 표시되는지 확인합니다.

복사본에서 설정 파일 이름을 잠시 바꾸거나 JSON 값을 범위 밖으로 바꾸어 실패 시 기본 30초를 확인합니다. 확인 후 파일명과 정상 JSON을 복원합니다. HTTPS 비교는 선택 실습입니다. DAY 08에 만든 ContentServer 폴더에서 기존 서버를 종료하고 `dotnet run --no-launch-profile`로 실행하면 Content/platform-config.json을 제공합니다. Unity의 Remote Url에는 `https://localhost:5443/platform-config.json`을 넣습니다. Unity 게임의 스테이지는 로컬 콘텐츠로 둔 채 설정 요청만 비교합니다.

기본 실습은 같은 PC의 Windows 환경입니다. Web에서는 동일 출처부터 확인하고, 다른 출처의 요청은 CORS 구성이 필요합니다. Android로 확장하려면 서버 접근 주소뿐 아니라 인증서 이름과 장치 신뢰도 별도로 구성합니다. 비교 후 Remote Url을 비워 원래의 StreamingAssets 설정 읽기로 복원합니다.

| 테스트 | 기대 결과 |
| :--- | :--- |
| 정상 JSON 45 | 다음 라운드 45초 |
| JSON 구조 오류·범위 밖 값 | 경고와 기본 30초 |
| 404·서버 중지·타임아웃 | UI가 멈추지 않고 요청 종료 후 기본 30초 |
| 플레이 중 LoadSettings 호출 | 현재 라운드 유지 |
| 설정 읽는 동안 ReloadStage 호출 | 준비 작업 중복 방지 |

PC의 StreamingAssets는 파일 경로, Android는 패키지 내부 URL, Web은 HTTP URL일 수 있습니다. `File.ReadAllText`를 모든 플랫폼에 사용하지 않습니다. 메인 스레드에서 완료를 반복 대기하거나 Web에서 WaitForCompletion에 의존하지 않습니다.

## 프로젝트 완료 확인

- [ ] 메뉴에서 45초 설정을 읽고 다음 라운드에 반영합니다.
- [ ] 응답 실패 시 UI가 멈추지 않고 30초로 시작합니다.

오늘의 프로젝트를 `Progress/Day13`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. 설정 응답이 도착했을 때 진행 중인 라운드를 바로 초기화하면 어떤 문제가 생기나요?
2. HTTP 성공 응답만으로 설정을 게임에 적용해도 되나요?

## 오늘의 정리

설정을 로그로만 출력하던 실습을 실제 다음 라운드의 제한 시간에 연결했습니다. 다음 수업에서는 이 완성된 게임의 UI 비용을 측정하고 개선합니다.
