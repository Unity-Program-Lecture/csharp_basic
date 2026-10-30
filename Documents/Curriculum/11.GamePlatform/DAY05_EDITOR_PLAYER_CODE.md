# DAY 05: 플랫폼 코드와 앱 생명주기

오늘은 Editor에서만 사용하는 코드를 분리하고, Player를 빌드할 때 드러나는 오류를 찾아봅니다. 이 활동은 NCS 학습 목표 G6·G7·G8에 해당합니다.

## 1. 핵심 개념: 공장 도구를 배송 상자에 넣지 않기

`AssetDatabase`는 공장 안에서 쓰는 도구처럼 Editor의 프로젝트 자산을 다루는 기능입니다. 배포한 앱에는 Editor와 같은 프로젝트 폴더가 없고, `UnityEditor` 타입도 사용할 수 없습니다. 따라서 `Application.isEditor`로 실행 여부만 검사하지 말고, Editor 전용 코드가 Player 컴파일에서 제외되도록 구성해야 합니다.

### 이 단어는 무슨 뜻인가요?

- **조건부 컴파일**: `#if`로 특정 코드를 컴파일할지 결정하는 기능입니다.
- **런타임 분기**: 이미 컴파일된 프로그램에서 if로 실행 경로를 고르는 기능입니다.
- **asmdef**: 코드가 어떤 어셈블리와 플랫폼에 포함되는지 지정하는 파일입니다.
- **IL2CPP/AOT**: AOT는 Ahead-of-Time Compilation (사전 컴파일)의 약자로, 프로그램 실행 전에 코드를 컴파일하는 방식입니다. IL2CPP는 관리 코드를 C++로 변환한 뒤 대상 플랫폼의 네이티브 코드로 사전 컴파일하며, 런타임 코드 생성에 제약이 있습니다.

> **참고: JIT**는 Just-in-Time Compilation (적시 컴파일)의 약자로, 프로그램 실행 중 필요한 코드를 기계어로 컴파일하는 방식입니다. AOT가 실행 전에 미리 준비하는 방식이라면, JIT는 실행 중 필요한 시점에 준비하는 방식입니다. 컴파일한 코드는 재사용할 수 있지만, 처음 컴파일할 때는 추가 비용이 발생할 수 있습니다.

## 2. 실습: 종료 버튼 만들기

**미션:** `Assets/Scripts/Runtime/ExitGame.cs`를 작성합니다.

```csharp
using UnityEngine;

public sealed class ExitGame : MonoBehaviour
{
    [SerializeField] private GameSession session;

    public void Exit()
    {
#if UNITY_EDITOR
        // 이 줄은 Editor에서만 컴파일됩니다. 실행 중인 게임 대신 Play Mode를 종료합니다.
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        // Web에서는 탭을 강제로 닫는 대신 메뉴로 돌아가 종료 방법을 안내합니다.
        session.ReturnToMenu();
        session.SetStatus("Use the page navigation or close the tab.");
#else
        Application.Quit();
#endif
    }
}
```

GameSession에 붙이고 Session 필드에 같은 GameSession을 연결합니다. Canvas/SafeAreaRoot/MenuPanel 아래 UI Button으로 QuitButton을 만듭니다. 자식 텍스트는 `QUIT`, 크기는 `(340,54)`, Anchor·Pivot은 중앙, 위치는 `(0,-130)`으로 둡니다. 버튼 배경의 Raycast Target은 켜고 자식 텍스트는 끕니다. On Click에 `ExitGame.Exit()`을 연결합니다. Web에서는 메뉴로 돌아가 페이지 이동이나 탭 닫기를 안내합니다. Android에서는 홈 이동이나 뒤로 가기 조작과 사용 흐름을 비교합니다.

## 3. 실습: Editor 전용 진단 메뉴

Assets/Scripts/Editor 폴더를 만들고 `PlatformDiagnostics.cs`에 다음 코드를 작성합니다. 오늘 처음 추가하는 Editor 전용 코드이며 Runtime 폴더에 넣지 않습니다.

```csharp
using UnityEditor;
using UnityEngine;

public static class PlatformDiagnostics
{
    [MenuItem("Tools/Platform Lab/Print Build Target")]
    public static void PrintBuildTarget()
    {
        Debug.Log($"Build Target={EditorUserBuildSettings.activeBuildTarget}");
    }
}
```

추가한 Editor 메뉴를 실행한 뒤 PC Player를 빌드합니다. 이 메뉴는 개발 중에 사용하는 도구이므로 Player에는 포함되지 않습니다. 나중에 Editor 전용 asmdef를 추가한다면 Include Platforms를 Editor로 제한합니다. asmdef가 있는 프로젝트에서는 Editor 폴더 이름만 확인하지 말고 어셈블리의 플랫폼 설정과 참조 방향도 살펴봅니다. 런타임 코드에서 Editor 전용 어셈블리를 참조하지 않습니다.

## 4. 빌드 차이 실습

실험용 복사본에서 Runtime 스크립트의 `UNITY_EDITOR` 조건을 잠시 제거해 봅니다. PC 빌드에서 발생하는 컴파일 오류를 기록한 뒤 조건을 원래대로 복원합니다. 다시 빌드하고 실행해 오류가 해결되었는지 확인합니다. 최종 프로젝트에는 Editor 전용 코드가 올바르게 분리된 상태를 남깁니다.

| 방식 | 의미 | 적용 사례 |
| :--- | :--- | :--- |
| `Application.isEditor` | 지금 Editor에서 실행 중인지 | 실습용 터치 UI 표시 |
| `Application.platform` | 현재 실행 환경 열거형 | 로그·지원 기능 판단 |
| `UNITY_EDITOR` | Editor 컴파일 포함 | EditorApplication·AssetDatabase |
| `UNITY_ANDROID` | Android 대상 코드 | Android 전용 플러그인 |
| `UNITY_WEBGL && !UNITY_EDITOR` | Web Player에서만 | 브라우저 전용 분기 |

Editor에서 선택한 빌드 대상의 심벌과 실제 Editor 실행 여부가 함께 작용합니다. 그래서 “Android 대상 코드가 컴파일된다”와 “실기기에서 실행됐다”는 다른 증거입니다.

리플렉션으로만 찾는 타입은 스트리핑 대상이 될 수 있습니다. 직접 참조와 `link.xml`·Preserve를 구분하고 해당 기능이 정말 필요한지 먼저 판단합니다. 스트리핑을 모두 끄는 것을 일반 해법으로 삼지 않습니다.

### 보충: Play를 다시 눌러도 남는 값

Unity 6.6의 새 프로젝트는 Play Mode에 들어갈 때 Domain Reload를 기본적으로 수행하지 않습니다. Domain Reload는 스크립트 실행 환경을 다시 만들며 정적 필드와 이벤트 상태 등을 초기화하는 과정입니다. 이 과정을 생략하면 시작은 빨라지지만, 이전 실행의 정적 값이나 이벤트 구독이 남을 수 있습니다.

예를 들어 정적 점수 변수에 값을 더했다면, Play를 종료하고 다시 시작했을 때도 그 값이 남는지 확인해야 합니다. 정적 이벤트를 사용할 때는 구독을 해제하는 시점도 정합니다. `Edit > Project Settings > Editor`에서 Enter Play Mode 설정을 확인하고, 같은 씬을 두 번 연속 실행해 초기 상태가 같은지 점검합니다. 필요한 정적 상태는 `RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)` 등으로 명시적으로 초기화할 수 있습니다.

이 과정의 `CoinRound`는 정적 점수 대신 인스턴스 상태를 사용합니다. DAY 14의 정적 `ProfilerMarker`도 점수나 구독 목록과는 역할이 다릅니다. 모든 정적 필드를 문제로 보기보다 실행마다 초기화할 상태인지 판단합니다. [Unity 6.6 변경 사항](https://docs.unity.com/en-us/engine/6000.6/manual/whats-new/unity66)

## 프로젝트 완료 확인

- [ ] Player 빌드에 Editor 전용 코드가 포함되지 않습니다.
- [ ] 두 번 연속 Play와 중단·복귀 후 상태를 확인했습니다.

오늘의 프로젝트를 `Progress/Day05`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. `if (Application.isEditor)` 안에서 AssetDatabase를 호출하면 Player 컴파일도 안전한가요?
2. UNITY_WEBGL 분기에 `!UNITY_EDITOR`를 함께 쓰는 이유는 무엇인가요?

## 중단·복귀를 실제 게임에서 확인

DAY 04에서 추가한 GameSession.OnApplicationPause를 사용합니다. 플레이 중 홈 화면으로 이동한 뒤 복귀해 남은 시간이 중단 동안 줄지 않았는지, 이동과 수집이 복구되는지 확인합니다. 메뉴나 결과에서 복귀하면 임의로 새 라운드를 시작하지 않아야 합니다. 창 포커스와 Pause의 전달 방식은 플랫폼마다 다르므로 각각 로그와 실행 상태를 기록합니다. 별도 ScoreSave 스크립트나 Time.timeScale 변경 코드를 추가하지 않습니다.

## 오늘의 정리

코드를 컴파일할 대상과 현재 실행 중인 환경을 구분하면 플랫폼별 코드를 올바르게 배치할 수 있습니다. 다음 수업에서는 Addressables를 사용해 실행 중에 자산을 불러옵니다.
