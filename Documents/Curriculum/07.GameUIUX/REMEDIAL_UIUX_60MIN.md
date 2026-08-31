# 게임 UI/UX 프로그래밍: 보충 실습

이 자료는 능력단위평가에서 UI가 보이지만 **게임 상태·입력·화면 전환 중 하나가 연결되지 않아** 결과물을 완성하지 못한 학생을 위한 보충수업입니다. 새 게임을 만들지 않고, 하나의 작은 씬에서 `HUD 데이터 연결`, `Pause 메뉴 전환`, `Input System UI 조작`, `Prefab 재사용`을 다시 연결하고 직접 확인합니다.

## 오늘의 목표

수업이 끝나면 다음을 직접 설명하고 실행할 수 있습니다.

- 체력과 점수 값이 바뀔 때 HUD가 함께 바뀌게 연결한다.
- 버튼과 `Pause` 입력으로 일시정지 메뉴를 열고 닫는다.
- 마우스와 키보드로 같은 메뉴를 조작한다.
- 반복해서 쓰는 버튼을 Prefab으로 만들고, 확인 결과를 기록한다.

## 준비물과 시작 전 확인

- Unity 6 프로젝트와 빈 씬 1개
- TextMeshPro Essentials를 가져온 상태
- `Input System` 패키지가 설치된 상태

처음부터 오류를 찾는 시간으로 쓰지 않기 위해, 기존 평가 프로젝트가 복잡하거나 오류가 많다면 새 씬 `RemedialUIUXScene`에서 시작합니다. 기존 프로젝트의 기능을 고치는 일은 이 실습이 끝난 뒤에 합니다.

## 진행 순서

| 순서 | 할 일 | 끝났을 때 확인할 것 |
| :--- | :--- | :--- |
| 1 | 실패 원인을 한 문장으로 적고 씬을 준비한다. | "값이 안 바뀜", "버튼이 안 눌림"처럼 관찰 가능한 문장이다. |
| 2 | Canvas와 HUD를 만든다. | 체력, 점수, 안내 문구가 보인다. |
| 3 | HUD와 스크립트를 연결한다. | 버튼을 누르면 체력·점수가 바뀐다. |
| 4 | Pause Panel과 버튼 흐름을 만든다. | 열기, 계속하기, 종료 버튼이 의도한 화면을 보인다. |
| 5 | Input System UI 입력을 점검한다. | 마우스와 키보드로 메뉴를 조작한다. |
| 6 | 해상도·입력·전환을 확인하고 기록한다. | 통과한 항목과 남은 문제를 구분해 적는다. |

## 1. 핵심 개념: "UI는 전광판과 리모컨을 함께 연결하는 일"

HUD는 게임 상태를 보여 주는 **전광판**입니다. 체력이 80인데 화면에는 100이 계속 보이면 전광판의 선이 끊어진 것입니다. Button과 메뉴는 플레이어가 게임에 지시를 보내는 **리모컨**입니다. 리모컨을 눌러도 Panel이 바뀌지 않으면 버튼과 동작의 선이 끊어진 것입니다.

보충 실습에서는 예쁜 화면보다 이 두 선이 실제로 연결되는지를 먼저 확인합니다.

### 이 단어는 무슨 뜻인가요?

- **HUD**: 플레이 중 필요한 체력, 점수, 시간 같은 정보를 계속 보여 주는 화면입니다.
- **Panel**: UI를 한 덩어리로 묶는 상자입니다. Pause 메뉴처럼 화면을 켜고 끌 때 사용합니다.
- **Prefab**: 반복해서 쓰는 GameObject를 저장해 두는 틀입니다. 같은 모양의 메뉴 버튼을 매번 새로 만들지 않게 합니다.
- **EventSystem**: 클릭, 선택, Submit 같은 UI 입력을 각 Button에게 전달하는 안내원입니다.
- **Input System UI Input Module**: Input System의 입력을 EventSystem이 알아듣는 UI 입력으로 바꿔 주는 연결 장치입니다.

## 2. 최소 씬 만들기

### Hierarchy 목표

아래 이름은 권장 이름입니다. 이미 만든 UI가 있으면 같은 역할의 오브젝트를 사용해도 됩니다.

```text
Canvas
├── HudPanel
│   ├── HealthText
│   ├── ScoreText
│   ├── DamageButton
│   └── ScoreButton
├── PausePanel                 ← 시작할 때 비활성화
│   ├── ResumeButton
│   └── QuitButton
└── HelpText

EventSystem
GameUIRecoveryController
```

### Canvas와 입력 설정

1. `Canvas`의 `Canvas Scaler`를 `Scale With Screen Size`로 바꿉니다.
2. `Reference Resolution`은 `1920 x 1080`, `Match`는 `0.5`로 설정합니다.
3. `EventSystem`을 선택합니다. `Standalone Input Module`이 있다면 제거하고 `Input System UI Input Module`을 추가합니다.
4. `PausePanel`은 Inspector 맨 위의 활성화 체크를 꺼서 처음에는 보이지 않게 합니다.
5. `ResumeButton`의 Navigation을 `Automatic`으로 둡니다. Button이 한 개뿐인 상태에서도 입력 시험을 위해 선택 대상으로 지정할 수 있습니다.

> Button이 눌리지 않는다면 코드보다 먼저 `Canvas > Graphic Raycaster`, `EventSystem`, `Input System UI Input Module`이 모두 있는지 확인합니다.

## 3. HUD 데이터를 연결하기

`GameUIRecoveryController` GameObject에 아래 스크립트를 붙입니다. Text와 Panel은 Inspector에서 직접 끌어 놓습니다. 이는 "어떤 화면을 바꿀지"를 코드에 숨기지 않고 Inspector에서도 확인하게 하기 위한 방법입니다.

### 실습 예제: 전광판 값 갱신

**미션:** `DamageButton`과 `ScoreButton`을 눌렀을 때 HUD 숫자가 즉시 바뀌게 만듭니다.

<details>
<summary>GameUIRecoveryController.cs 코드 보기</summary>

```csharp
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameUIRecoveryController : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Menu")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private InputActionReference cancelAction;

    private int health = 100;
    private int score;
    private bool isPaused;

    private void OnEnable()
    {
        if (cancelAction != null)
        {
            cancelAction.action.Enable();
            cancelAction.action.performed += OnCancelPerformed;
        }
    }

    private void OnDisable()
    {
        if (cancelAction != null)
        {
            cancelAction.action.performed -= OnCancelPerformed;
            cancelAction.action.Disable();
        }
    }

    private void Start()
    {
        RefreshHud();
        pausePanel.SetActive(false);
    }

    public void TakeDamage()
    {
        health = Mathf.Max(0, health - 10);
        RefreshHud();
    }

    public void AddScore()
    {
        score += 100;
        RefreshHud();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        if (isPaused)
        {
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
    }

    public void QuitPractice()
    {
        Debug.Log("보충 실습 종료 버튼을 눌렀습니다.");
    }

    private void RefreshHud()
    {
        healthText.text = $"HP : {health}";
        scoreText.text = $"Score : {score}";
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        TogglePause();
    }
}
```

</details>

### Inspector 연결 순서

1. `HealthText`, `ScoreText`, `PausePanel`, `ResumeButton`을 각각 알맞은 칸에 끌어 놓습니다.
2. `DamageButton`의 `On Click()`에 `GameUIRecoveryController > TakeDamage()`를 연결합니다.
3. `ScoreButton`의 `On Click()`에 `GameUIRecoveryController > AddScore()`를 연결합니다.
4. `ResumeButton`의 `On Click()`에 `TogglePause()`를 연결합니다.
5. `QuitButton`의 `On Click()`에 `QuitPractice()`를 연결합니다.

### 실행해보면

- `DamageButton`을 누를 때마다 `HP : 100`이 `HP : 90`, `HP : 80`처럼 바뀝니다.
- `ScoreButton`을 누를 때마다 점수가 100씩 증가합니다.
- 값이 변하지 않으면 먼저 Button의 `On Click()` 대상과 `TextMeshProUGUI` 필드 연결을 확인합니다.

## 4. Pause 입력을 Input System에 연결하기

### Input Actions 설정

1. `Assets`에서 `Create > Input Actions`를 선택하고 `UIActions`로 이름을 정합니다.
2. Action Map `UI`에 Action `Cancel`을 추가합니다.
3. `Cancel`의 Action Type은 `Button`으로 설정합니다.
4. Binding에 `Keyboard > Escape`를 추가합니다. 게임패드도 시험할 수 있다면 `Gamepad > Start`를 하나 더 추가합니다.
5. Asset을 저장한 뒤 `GameUIRecoveryController`의 `Cancel Action` 칸에 `UI/Cancel` Action Reference를 넣습니다.

Play Mode에서 Esc를 누르면 `PausePanel`이 열리고 `ResumeButton`에 선택 표시가 나타나야 합니다. 다시 Esc를 누르거나 ResumeButton을 실행하면 Panel이 닫혀야 합니다.

### 자주 막히는 지점

| 관찰한 문제 | 먼저 확인할 곳 |
| :--- | :--- |
| Esc를 눌러도 아무 일도 없다. | `Cancel Action`에 실제 Action Reference가 연결됐는지, Binding이 저장됐는지 확인합니다. |
| 메뉴는 열리지만 Enter가 버튼을 실행하지 않는다. | `EventSystem`, `Input System UI Input Module`, 처음 선택할 `ResumeButton`을 확인합니다. |
| 메뉴가 닫혀도 게임이 멈춰 있다. | `TogglePause()`가 ResumeButton의 `On Click()`에 연결됐는지 확인합니다. |
| Panel이 안 보이는데 뒤 버튼이 클릭되지 않는다. | 숨긴 Panel의 활성 상태와 `CanvasGroup`의 `Blocks Raycasts`를 확인합니다. |

## 5. MenuButton을 Prefab으로 만들기

**미션:** Pause 메뉴의 Button 하나를 반복 사용 가능한 부품으로 저장합니다.

1. `ResumeButton`을 `Assets/Prefabs/UI` 폴더로 드래그해 `MenuButton` Prefab을 만듭니다.
2. Prefab을 열어 TextMeshPro 글자 크기와 Button 색 상태를 확인합니다.
3. Prefab을 다시 PausePanel에 하나 더 배치하고 `QuitButton`으로 이름과 글자를 바꿉니다.
4. 두 Button의 `On Click()`은 역할에 맞게 각각 다시 연결합니다.

Prefab은 같은 모양의 레고 블록입니다. 글자와 버튼 상태 규칙을 한 번 정해 두면 메뉴를 추가할 때 모양이 제각각 되는 문제를 줄일 수 있습니다.

## 6. 학생용 재확인 기록

아래는 채점표가 아니라, 다시 시도하기 전에 자신의 연결 상태를 확인하는 기록입니다. `통과`, `수정 중`, `미확인` 중 하나를 적고, 수정 중인 항목은 원인을 한 문장으로 남깁니다.

| 확인 항목 | 결과 | 관찰 또는 수정 내용 |
| :--- | :--- | :--- |
| `DamageButton`을 누르면 체력 Text가 바뀐다. |  |  |
| `ScoreButton`을 누르면 점수 Text가 바뀐다. |  |  |
| PausePanel은 시작 시 보이지 않는다. |  |  |
| Esc 또는 지정한 Cancel 입력으로 PausePanel을 열고 닫는다. |  |  |
| 마우스로 ResumeButton과 QuitButton을 실행할 수 있다. |  |  |
| 키보드로 Button을 선택하고 Submit할 수 있다. |  |  |
| MenuButton Prefab을 두 곳 이상에서 사용했다. |  |  |
| `1920 x 1080`과 `1024 x 768`에서 글자가 잘리거나 화면 밖으로 나가지 않는다. |  |  |

### 생각해보기

1. `health` 값만 바꾸고 `RefreshHud()`를 호출하지 않으면 플레이어는 무엇을 보게 될까요?
2. PausePanel의 Alpha만 0으로 만들고 입력 차단을 해제하지 않으면 어떤 일이 생길까요?
3. 메뉴 Button을 Prefab으로 만들면 다음 화면을 제작할 때 무엇을 덜 반복할 수 있을까요?

## 오늘의 정리

- UI가 동작한다는 말에는 화면 표시, 데이터 연결, 입력 반응, 화면 전환이 모두 들어 있습니다.
- 문제를 고칠 때는 "버튼이 안 된다"보다 "On Click 연결이 비어 있다"처럼 확인 가능한 원인으로 좁힙니다.
- 이 미니 씬이 통과하면 같은 연결 방식을 기존 평가 프로젝트의 HUD와 메뉴에 하나씩 옮겨 적용합니다.
