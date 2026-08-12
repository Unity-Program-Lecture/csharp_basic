# DAY 04: Input System과 UI 조작

오늘의 목표는 Unity 6 Input System으로 키보드, 마우스, 게임패드 입력을 UI 조작과 연결하고, 이벤트 방식 입력과 폴링 방식 입력의 차이를 이해하는 것입니다.

## NCS 연결

- 능력단위 요소: 게임 UI/UX 응용 프로그래밍하기
- 관련 학습 내용: UI/UX 인터페이스 구성, 입력 장치 동작 구현, 이벤트 방식 입력 알고리즘, 데이터 폴링 방식 입력 알고리즘
- Unity 6 재구성: Input System, EventSystem, Input System UI Input Module, Input Action을 사용해 UI 탐색과 단축키를 구현합니다.

## 1. 핵심 개념: "벨을 누르면 반응하기, 계속 CCTV 보기"

입력 처리에는 크게 두 감각이 있습니다.

이벤트 방식은 초인종입니다. 누군가 벨을 누르면 그때 반응합니다. 버튼 클릭, Submit, Cancel 같은 UI 입력에 잘 어울립니다.

폴링 방식은 CCTV입니다. 매 프레임 계속 상태를 확인합니다. 캐릭터 이동처럼 계속 누르고 있는 입력에 잘 어울립니다.

### 이 단어는 무슨 뜻인가요?

- **Input System**: Unity의 최신 입력 처리 패키지입니다.
- **Input Action**: 점프, 공격, 메뉴 열기처럼 의미 단위로 이름 붙인 입력입니다.
- **EventSystem**: 입력을 UI 이벤트로 전달하는 Unity 오브젝트입니다.
- **Input System UI Input Module**: Input System 입력을 UGUI 버튼, 슬라이더, 선택 이동에 연결하는 컴포넌트입니다.
- **Submit**: 선택된 UI를 실행하는 입력입니다.
- **Cancel**: 뒤로 가기 또는 닫기 입력입니다.
- **Navigate**: 키보드 방향키나 게임패드 스틱으로 UI 선택을 이동하는 입력입니다.

## 2. UI 입력 구성 순서

1. Package Manager에서 `Input System`이 설치되어 있는지 확인합니다.
2. 씬에 `EventSystem`을 둡니다.
3. EventSystem의 입력 모듈이 `Input System UI Input Module`인지 확인합니다.
4. UI용 Action을 구성합니다.
5. Button의 Navigation 설정을 확인합니다.
6. 키보드, 마우스, 게임패드로 같은 메뉴를 조작해 봅니다.

## 3. UI Action 기본 목록

| Action | 역할 | 예시 입력 |
| :--- | :--- | :--- |
| Navigate | 선택 이동 | 방향키, WASD, D-Pad, Left Stick |
| Submit | 선택 실행 | Enter, Space, Gamepad South |
| Cancel | 뒤로 가기 | Esc, Gamepad East |
| Point | 마우스/터치 위치 | Mouse Position, Touch Position |
| Click | 클릭 | Mouse Left Button, Touch Press |
| ScrollWheel | 스크롤 | Mouse Scroll |

## 실습 예제: Esc로 일시정지 열고 닫기

**미션:** 키보드 `Esc` 또는 게임패드 Cancel 입력으로 Pause 메뉴를 열고 닫습니다.

### 스크립트 작성

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInputController : MonoBehaviour
{
    [SerializeField] private UIScreenFlowController screenFlow;

    private InputAction pauseAction;
    private bool isPaused;

    void Awake()
    {
        pauseAction = new InputAction("Pause", InputActionType.Button);
        pauseAction.AddBinding("<Keyboard>/escape");
        pauseAction.AddBinding("<Gamepad>/start");
    }

    void OnEnable()
    {
        pauseAction.Enable();
        pauseAction.performed += OnPausePerformed;
    }

    void OnDisable()
    {
        pauseAction.performed -= OnPausePerformed;
        pauseAction.Disable();
    }

    void OnDestroy()
    {
        pauseAction.Dispose();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            screenFlow.PauseGame();
        }
        else
        {
            screenFlow.ResumeGame();
        }
    }
}
```

</details>

### 코드 읽기

- `Awake`에서는 입력 규칙을 만듭니다.
- `OnEnable`에서는 입력을 켜고 이벤트를 구독합니다.
- `OnDisable`에서는 구독을 해제하고 입력을 끕니다.
- `OnDestroy`에서는 직접 만든 Input Action을 정리합니다.
- `performed += OnPausePerformed`는 Pause 입력이 발생했을 때 실행할 함수를 등록한다는 뜻입니다.

## 4. UI Navigation 확인하기

Button은 마우스로만 누르는 것이 아닙니다. 게임패드와 키보드로 메뉴를 움직이려면 Navigation이 필요합니다.

| Navigation 모드 | 설명 |
| :--- | :--- |
| `Automatic` | Unity가 가까운 버튼을 자동으로 찾습니다. |
| `Explicit` | 위, 아래, 왼쪽, 오른쪽 이동 대상을 직접 지정합니다. |
| `None` | 키보드/게임패드 이동 대상에서 제외합니다. |

메뉴 버튼이 세로로 단순히 쌓여 있다면 `Automatic`으로도 충분합니다. 하지만 설정 화면처럼 버튼, 슬라이더, 토글이 섞이면 `Explicit`으로 직접 연결하는 편이 안전합니다.

### 단위 테스트 체크

- 마우스로 버튼을 누를 수 있는가?
- 방향키로 버튼 선택이 이동하는가?
- Enter 또는 Space로 선택된 버튼이 실행되는가?
- Esc로 Pause 메뉴가 열리고 닫히는가?
- 게임패드를 연결했을 때 D-Pad 또는 Stick으로 이동할 수 있는가?

### 생각해보기

1. 캐릭터 이동은 이벤트 방식보다 폴링 방식이 어울리는 이유가 무엇일까요?
2. 메뉴 버튼은 폴링 방식보다 이벤트 방식이 어울리는 이유가 무엇일까요?
3. 마우스 없이 게임패드만으로 조작할 수 없는 UI는 어떤 문제가 있을까요?
4. `OnDisable`에서 이벤트 구독을 해제하지 않으면 어떤 일이 생길 수 있을까요?

## 오늘의 정리

- 이벤트 방식 입력은 버튼 클릭, 메뉴 선택, 일시정지처럼 한 번 발생하는 UI 행동에 잘 어울립니다.
- 폴링 방식 입력은 이동처럼 매 프레임 상태를 확인해야 하는 행동에 어울립니다.
- Unity 6 UI 조작은 Input System과 `Input System UI Input Module`을 기준으로 구성합니다.
- UI는 마우스뿐 아니라 키보드와 게임패드 조작까지 확인해야 UX가 안정적입니다.
