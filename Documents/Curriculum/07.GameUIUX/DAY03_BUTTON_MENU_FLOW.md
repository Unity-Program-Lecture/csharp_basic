# DAY 03: 버튼, 메뉴, 화면 전환

오늘의 목표는 Button 이벤트와 화면 패널 전환을 사용해 타이틀, 플레이 HUD, 일시정지 메뉴, 결과 화면을 오가는 기본 UI 흐름을 만드는 것입니다.

## NCS 연결

- 능력단위 요소: 게임 UI/UX 요소 프로그래밍하기
- 관련 학습 내용: 컨트롤 인터페이스 구현, 메뉴 시스템 또는 게임 로비 요소 구현, UI/UX 요소 단위 테스트
- Unity 6 재구성: UGUI Button, EventSystem, 화면 Panel, `SetActive`, 화면 상태 enum을 이용해 메뉴 흐름을 구현합니다.

## 1. 핵심 개념: "무대 장면을 바꾸는 조명 스위치"

UI 화면 전환은 연극 무대에서 조명을 켜고 끄는 것과 비슷합니다. 타이틀 화면 조명을 켜면 시작 버튼이 보이고, 플레이 화면 조명을 켜면 HUD가 보입니다. 사용하지 않는 화면은 꺼 두면 됩니다.

Unity에서는 화면마다 Panel GameObject를 만들고, 필요한 Panel만 `SetActive(true)`로 켭니다.

### 이 단어는 무슨 뜻인가요?

- **Panel**: 여러 UI 요소를 묶는 사각형 컨테이너입니다.
- **Button On Click**: 버튼을 눌렀을 때 실행할 함수를 등록하는 목록입니다.
- **EventSystem**: 마우스, 키보드, 게임패드 입력을 UI 이벤트로 전달하는 오브젝트입니다.
- **화면 상태**: 현재 UI가 타이틀인지, 플레이인지, 일시정지인지 구분하는 값입니다.
- **Interactable**: 버튼이나 슬라이더를 조작 가능한 상태로 둘지 정하는 옵션입니다.

## 2. 화면 흐름 설계

```text
Title
  -> Start Button -> Play
  -> Option Button -> Option

Play
  -> Pause Input -> Pause
  -> Game Clear/Over -> Result

Pause
  -> Resume Button -> Play
  -> Title Button -> Title

Result
  -> Retry Button -> Play
  -> Title Button -> Title
```

이 흐름은 NCS에서 말하는 컨트롤 인터페이스와 메뉴 시스템 구현에 해당합니다. 버튼 하나하나가 단순 장식이 아니라 게임 상태를 바꾸는 입구입니다.

## 3. EventSystem 확인하기

Button이 눌리지 않을 때는 먼저 다음을 확인합니다.

1. 씬에 `EventSystem`이 있는가?
2. EventSystem의 입력 모듈이 `Input System UI Input Module`인가? Unity 6에서 Input System으로 UGUI를 조작할 때 필요합니다.
3. Canvas에 `Graphic Raycaster`가 있는가?
4. 버튼을 덮고 있는 Image의 `Raycast Target`이 클릭을 가로막고 있지 않은가?
5. Button의 `Interactable`이 켜져 있는가?
6. Button의 `On Click`에 올바른 오브젝트와 함수가 연결되어 있는가?

## 실습 예제: UI 화면 전환 컨트롤러

**미션:** 타이틀, 플레이 HUD, 일시정지, 결과 화면을 전환하는 `UIScreenFlowController`를 만듭니다.

### Unity 오브젝트 구조

```text
Canvas
└── UIRoot
    ├── Screen_Title
    │   ├── StartButton
    │   └── QuitButton
    ├── Screen_PlayHud
    │   └── PauseButton
    ├── Screen_Pause
    │   ├── ResumeButton
    │   └── TitleButton
    ├── Screen_Result
    │   ├── RetryButton
    │   └── TitleButton
    └── UIScreenFlowController
```

### 스크립트 작성

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class UIScreenFlowController : MonoBehaviour
{
    private enum ScreenState
    {
        Title,
        Play,
        Pause,
        Result
    }

    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject playHudScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject resultScreen;

    private ScreenState currentState;

    void Start()
    {
        ShowTitle();
    }

    public void ShowTitle()
    {
        ChangeScreen(ScreenState.Title);
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        ChangeScreen(ScreenState.Play);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        ChangeScreen(ScreenState.Pause);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        ChangeScreen(ScreenState.Play);
        Time.timeScale = 1f;
    }

    public void ShowResult()
    {
        ChangeScreen(ScreenState.Result);
        Time.timeScale = 1f;
    }

    private void ChangeScreen(ScreenState nextState)
    {
        currentState = nextState;

        titleScreen.SetActive(currentState == ScreenState.Title);
        playHudScreen.SetActive(currentState == ScreenState.Play);
        pauseScreen.SetActive(currentState == ScreenState.Pause);
        resultScreen.SetActive(currentState == ScreenState.Result);
    }
}
```

</details>

### Button 연결

| 버튼 | 연결할 함수 |
| :--- | :--- |
| StartButton | `UIScreenFlowController.StartGame` |
| PauseButton | `UIScreenFlowController.PauseGame` |
| ResumeButton | `UIScreenFlowController.ResumeGame` |
| RetryButton | `UIScreenFlowController.StartGame` |
| TitleButton | `UIScreenFlowController.ShowTitle` |

### 실행해보면

버튼을 누를 때마다 화면 Panel이 하나씩 켜지고 나머지는 꺼집니다. Pause 화면에서는 `Time.timeScale`이 `0`이 되어 게임 시간이 멈춥니다.

### 단위 테스트 체크

- 타이틀에서 시작 버튼을 누르면 HUD만 보이는가?
- 일시정지 버튼을 누르면 Pause 화면만 보이는가?
- 계속하기 버튼을 누르면 HUD로 돌아오는가?
- 여러 버튼을 빠르게 눌러도 화면이 겹치지 않는가?
- 화면이 바뀌어도 Button 클릭이 계속 동작하는가?

### 생각해보기

1. 화면마다 Panel을 나누면 어떤 점이 관리하기 쉬울까요?
2. `Time.timeScale = 0f`가 UI 클릭까지 멈추지는 않는 이유는 무엇일까요?
3. 화면 상태를 `enum`으로 표현하면 문자열보다 어떤 점이 안전할까요?
4. 버튼이 눌리지 않을 때 코드보다 먼저 확인해야 할 Unity 오브젝트는 무엇일까요?

## 오늘의 정리

- 메뉴 시스템은 여러 UI Panel을 켜고 끄는 화면 흐름입니다.
- Button의 `On Click`은 UI와 C# 함수를 연결하는 다리입니다.
- EventSystem, Graphic Raycaster, Raycast Target은 UI 입력 전달에 꼭 필요합니다.
- 화면 전환은 단위 테스트 항목을 만들어 직접 눌러 보며 검증해야 합니다.
