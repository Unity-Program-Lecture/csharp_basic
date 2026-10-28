# DAY 03: 키보드·게임패드·터치 입력

오늘은 키보드, 게임패드, 화면 방향 버튼으로 같은 이동 기능을 사용하도록 입력을 연결합니다. 추가 실습에서는 화면의 손잡이를 끌어 조작하는 가상 조이스틱도 만듭니다. `Application`을 이용해 게임이 어떤 환경에서 실행 중인지도 확인합니다. 이 활동은 NCS 학습 목표 G5·G7에 해당합니다.

## 1. 핵심 개념: 여러 리모컨, 같은 명령

키보드의 W 키를 누르거나 화면의 위쪽 방향 버튼을 누르면 모두 `Move`라는 같은 명령으로 전달합니다. 장치마다 이동 코드를 새로 만드는 대신, 입력을 공통된 값으로 바꾸어 기존 이동 규칙에 전달합니다.

### 이 단어는 무슨 뜻인가요?

- **Input Action**: Move처럼 장치에서 독립된 입력 의도입니다.
- **Binding**: 실제 키·스틱과 Action을 연결하는 설정입니다.
- **Application**: 실행 중인 앱의 플랫폼·경로·버전·생명주기 등에 접근하는 Unity 클래스입니다.

## 2. 실습: 입력 자산 만들기

**미션:** Input Actions 자산 `PlatformControls`를 만들고 Move를 연결합니다.

1. Assets/Input 폴더를 만들고 그 안에서 `Create > Input Actions`로 PlatformControls 자산을 만든 뒤 편집합니다.
2. Action Map `Gameplay`, Action `Move`를 만듭니다. Action Type은 Value, Control Type은 Vector2로 설정합니다.
3. `2D Vector Composite`를 추가하고 Up=W, Down=S, Left=A, Right=D로 설정합니다.
4. 두 번째 `2D Vector Composite`에는 위·아래·왼쪽·오른쪽 방향키를 연결합니다. 별도 Binding으로 `<Gamepad>/leftStick`도 추가하고 저장합니다. 자산 경로는 `Assets/Input/PlatformControls.inputactions`로 둡니다.
5. 이 예제는 InputActionReference를 직접 사용합니다. PlayerInput 컴포넌트는 추가하지 않습니다. 두 방식의 액션 소유권을 섞지 않습니다.

다음 코드를 `Assets/Scripts/Runtime/PlatformMover.cs`로 작성합니다.

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlatformMover : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private GameSession session;
    [SerializeField] private float speed = 5f;

    private void OnEnable()
    {
        move.action.Enable();
    }

    private void OnDisable()
    {
        move.action.Disable();
    }

    private void Update()
    {
        if (!session.CanMove)
        {
            return;
        }

        // 대각선 입력 (1,1)의 길이는 약 1.41입니다. 최대 길이를 1로 제한해 더 빠르게 움직이지 않게 합니다.
        // 길이가 1보다 작은 스틱 입력은 그대로 유지합니다.
        Vector2 input = Vector2.ClampMagnitude(move.action.ReadValue<Vector2>(), 1f);
        // 입력의 x·y를 바닥의 X·Z 방향으로 옮깁니다.
        // 초당 속도에 이번 프레임의 시간(초)을 곱하면 이번 프레임에 이동할 거리가 됩니다.
        Vector3 next = transform.position + new Vector3(input.x, 0f, input.y) * speed * Time.deltaTime;
        // X·Z 좌표를 각각 -9~9로 제한해 바닥 밖으로 나가지 않게 합니다.
        next.x = Mathf.Clamp(next.x, -9f, 9f);
        next.z = Mathf.Clamp(next.z, -9f, 9f);
        transform.position = next;
    }
}
```

Player에 붙이고 Move 필드의 선택 창에서 PlatformControls의 Gameplay/Move 액션을 선택합니다. 자산 전체가 아니라 Move 액션을 참조해야 합니다. Session 필드에는 Hierarchy의 GameSession 컴포넌트를 연결합니다. Inspector의 Speed를 2와 5로 바꾸어 이동 거리를 비교한 뒤 5로 복원합니다.

## 3. 화면 방향 버튼 구성

Canvas/SafeAreaRoot 아래 빈 UI 오브젝트를 만들고 MobileControls로 이름을 바꿉니다. UI Panel로 만들었다면 Image 컴포넌트를 제거합니다. RectTransform은 아래 크기·Anchor·Pivot·위치로 설정하고, 자식으로 Up·Down·Left·Right 버튼을 만듭니다. 각 버튼에 `OnScreenButton`을 추가하고 다음과 같이 연결합니다. Button의 On Click에 이동 메서드를 연결하지 않습니다. 누르고 있는 상태는 OnScreenButton이 입력 액션에 전달합니다.

| 오브젝트 | 표시 | Control Path | MobileControls 기준 위치 |
| :--- | :--- | :--- | :--- |
| Up | W | `<Keyboard>/w` | (0,65) |
| Down | S | `<Keyboard>/s` | (0,-10) |
| Left | A | `<Keyboard>/a` | (-80,-10) |
| Right | D | `<Keyboard>/d` | (80,-10) |

버튼 크기는 `(70,65)`, Anchor·Pivot은 중앙 `(0.5,0.5)`로 시작합니다. MobileControls 크기는 `(260,220)`이며, 오늘은 Anchor Min·Max `(0.15,0.18)`, Pivot `(0.5,0.5)`, 위치 `(0,0)`을 사용합니다. DAY 15에서 좌하단 모서리 기준으로 배치를 보완합니다.

EventSystem의 InputSystemUIInputModule에 기본 UI Actions가 연결되어 있어야 합니다. HUD 장식 Text/Image의 Raycast Target은 끄고 버튼의 배경 Image는 켭니다. 마우스로 버튼을 누르고 있는 동안 움직이고 놓으면 멈추는지 확인합니다. 이 구성은 가상 Keyboard 입력을 사용하며 실제 키보드와 같은 Move 바인딩을 공유합니다. Android에서 화면 버튼을 사용하려고 물리 키보드를 연결할 필요는 없습니다.

공식 참고: [Input System 1.20 매뉴얼](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/index.html). 설치한 패키지 버전에서 해당 설정을 다시 확인합니다.

## 4. 추가 실습: 가상 조이스틱 만들기

**미션:** 방향 버튼으로 이동하는 것을 확인한 뒤, 같은 프로젝트에 가상 조이스틱을 추가합니다. 기존 PlatformMover와 Move 액션을 그대로 사용합니다.

방향 버튼이 W·A·S·D 키를 대신 누르는 방식이라면, 가상 조이스틱은 게임패드의 왼쪽 스틱을 대신 움직이는 방식입니다. `OnScreenStick`은 화면에서 손잡이를 끈 방향과 크기를 Vector2 입력으로 전달합니다. 실제 게임패드를 연결할 필요는 없습니다.

### UI 오브젝트 구성

Play를 종료하고 기존 `Canvas/SafeAreaRoot/MobileControls` 아래에 다음 오브젝트를 만듭니다. Up·Down·Left·Right는 삭제하지 않습니다.

```text
MobileControls
├── Up / Down / Left / Right
└── JoystickBase (Image)
    └── JoystickHandle (Image, OnScreenStick)
```

1. MobileControls를 선택하고 `UI > Image`로 JoystickBase를 만듭니다.
2. JoystickBase를 선택하고 다시 `UI > Image`로 JoystickHandle을 만듭니다.
3. 두 오브젝트의 RectTransform을 아래 표대로 설정합니다. Scale은 `(1,1,1)`로 둡니다. MobileControls의 기존 배치는 유지합니다.
4. Image의 Color는 바탕을 짙은 회색, 손잡이를 밝은 회색으로 설정합니다. 먼저 기본 Image로 동작을 확인하고, 원형 Sprite가 있다면 Source Image에 넣어 모양을 바꿀 수 있습니다.

| 오브젝트 | 크기 | Anchor Min·Max / Pivot | Anchored Position | Image의 Raycast Target |
| :--- | :--- | :--- | :--- | :--- |
| JoystickBase | `(200,200)` | `(0.5,0.5)` / `(0.5,0.5)` | `(0,0)` | 끔 |
| JoystickHandle | `(64,64)` | `(0.5,0.5)` / `(0.5,0.5)` | `(0,0)` | 켬 |

### OnScreenStick 연결

JoystickHandle에 Add Component로 `On-Screen Stick`을 추가하고 다음과 같이 설정합니다. Button 컴포넌트나 On Click 이벤트, 별도의 이동 스크립트는 추가하지 않습니다.

| 설정 | 값 | 의미 |
| :--- | :--- | :--- |
| Control Path | `<Gamepad>/leftStick` | 가상 게임패드의 왼쪽 스틱으로 입력 전달 |
| Movement Range | `60` | 손잡이의 UI 이동 범위. Player의 월드 이동 거리와는 다름 |
| Behaviour | `Relative Position With Static Origin` | 중앙을 고정하고, 누른 뒤 끈 만큼 손잡이를 이동 |
| Use Isolated Input Actions | 끔 | 기존 EventSystem의 포인터 이벤트로 조작 |

Behaviour의 코드 이름은 `RelativePositionWithStaticOrigin`입니다. 이 설정에서는 **손잡이를 누른 상태로 끌어야** 움직입니다. 바탕의 빈 곳을 누르는 것만으로 이동하는 구성은 아닙니다. EventSystem의 InputSystemUIInputModule과 Canvas의 GraphicRaycaster는 방향 버튼 실습 때의 구성을 사용합니다.

PlatformControls를 열어 Gameplay/Move에 `<Gamepad>/leftStick` 바인딩이 있는지 확인하고 저장합니다. PlatformMover의 Move 필드는 계속 같은 Gameplay/Move를 참조합니다. 조이스틱을 위해 새 Move 액션을 만들지 않습니다.

방향 버튼 네 개는 Inspector 상단의 체크를 꺼 오브젝트를 비활성화하고, JoystickBase와 JoystickHandle은 활성화합니다. **MobileControls 자체는 활성 상태로 둡니다.** 두 입력 UI가 같은 위치에서 겹치지 않도록 한 번에 한 방식만 표시합니다.

### 실행해보면

Play 후 Start를 누르고 Game View에서 손잡이를 마우스로 끌어 봅니다. Android 실기기에서는 같은 위치를 손가락으로 누르고 끕니다.

| 조작 | 확인할 결과 |
| :--- | :--- |
| 위·아래·왼쪽·오른쪽으로 끌기 | 기존 Player가 같은 방향으로 이동 |
| 중앙에서 조금 끌기와 크게 끌기 비교 | 스틱의 데드존을 넘은 뒤에는 입력 크기에 따라 이동 속도가 달라짐 |
| 손잡이에서 손 떼기 | 손잡이가 중앙으로 돌아가고 이동이 멈춤 |
| 대각선으로 끝까지 끌기 | 직선보다 더 빠르게 이동하지 않음 |
| 메뉴·결과 화면에서 끌기 | 기존 session.CanMove 조건에 따라 Player는 이동하지 않음 |

중앙 근처의 작은 입력을 무시하는 범위를 **데드존**이라고 합니다. 기존 PlatformMover는 길이가 1보다 작은 입력을 그대로 사용하므로 스틱 입력 크기에 따른 속도를 유지합니다. `normalized`로 바꾸면 작은 입력도 최대 길이가 되어 이 차이가 사라집니다.

손잡이가 움직이지 않으면 JoystickHandle의 Raycast Target, EventSystem의 UI Actions, UI 위를 덮은 다른 Image를 확인합니다. 손잡이는 움직이는데 Player가 멈춰 있다면 Start 여부, Move의 `<Gamepad>/leftStick` 바인딩, PlatformMover의 Move·Session 참조를 확인합니다.

기본 필수 실습은 방향 버튼 네 개이며 조이스틱은 추가 실습입니다. 사용한 방식과 확인 결과를 기록합니다. 방향 버튼으로 돌아가려면 Play를 종료한 뒤 JoystickBase를 끄고 버튼 네 개를 켭니다. 이후 수업에서도 두 방식 모두 같은 MobileControls 아래에 두어 Safe Area와 플랫폼별 표시 설정을 함께 적용합니다.

공식 참고: [Input System 1.20 OnScreenStick](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/api/UnityEngine.InputSystem.OnScreen.OnScreenStick.html), [스틱 동작 방식](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/api/UnityEngine.InputSystem.OnScreen.OnScreenStick.Behaviour.html).

## 5. 실행 환경 표시

**미션:** `Assets/Scripts/Runtime/PlatformInfo.cs`를 작성하고 기존 GameSession 오브젝트에 붙여 Editor와 PC Player 로그를 비교합니다.

```csharp
using UnityEngine;

public sealed class PlatformInfo : MonoBehaviour
{
    [SerializeField] private GameObject mobileControls;

    private void Start()
    {
        Debug.Log($"Platform={Application.platform}, Editor={Application.isEditor}");
        Debug.Log($"Version={Application.version}, Data={Application.dataPath}");
        Debug.Log($"Save={Application.persistentDataPath}");

        // 모바일과 연습용 Editor에서는 터치 UI를 표시하고, 일반 PC Player에서는 숨깁니다.
        mobileControls.SetActive(Application.isMobilePlatform || Application.isEditor);
    }
}
```

Mobile Controls 필드에 터치 UI의 루트 오브젝트를 연결합니다. Editor에서는 연습할 수 있도록 터치 UI를 표시합니다. Android 프로필을 선택해도 Editor 자체가 Android 기기로 바뀌는 것은 아닙니다. `Application.platform`으로 확인하는 현재 실행 환경과 코드의 컴파일 대상을 구분해서 살펴봅니다.

## 프로젝트 완료 확인

- [ ] WASD·방향키와 화면 방향 버튼으로 같은 Player가 움직입니다. 게임패드는 연결 가능한 환경에서 별도로 확인합니다.
- [ ] 메뉴·결과·중단 상태에서는 이동하지 않고 대각선 속도가 증가하지 않습니다.
- [ ] 조이스틱 추가 실습을 했다면 끌기·손 떼기·입력 크기에 따른 속도를 확인하고, 방향 버튼과 겹치지 않게 표시했습니다.

오늘의 프로젝트를 `Progress/Day03`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. Move를 읽는 코드가 키보드와 터치에서 같은 이유는 무엇인가요?
2. Editor에서 Android 프로필을 선택하는 것과 Android 실기기에서 실행하는 것은 어떻게 다른가요?

## 오늘의 정리

여러 장치의 입력을 하나의 Action으로 연결하면 같은 이동 코드를 재사용할 수 있습니다. 실행 환경에 따른 동작은 해당 Player에서 직접 확인합니다. 다음 수업에서는 화면 크기 대응, 저장, 앱 중단과 복귀를 다룹니다.
