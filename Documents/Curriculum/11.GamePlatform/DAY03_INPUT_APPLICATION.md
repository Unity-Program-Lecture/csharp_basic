# DAY 03: 키보드·게임패드·터치 입력

오늘은 키보드, 게임패드, 화면 방향 버튼으로 같은 이동 기능을 사용하도록 입력을 연결합니다. `Application`을 이용해 게임이 어떤 환경에서 실행 중인지도 확인합니다. 이 활동은 NCS 학습 목표 G5·G7에 해당합니다.

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

터치 스틱은 선택 실습입니다. 방향 버튼 동작을 확인한 뒤 프로젝트 복사본에서 `OnScreenStick`과 `<Gamepad>/leftStick`을 연결할 수 있습니다. 기본 필수 실습은 방향 버튼 네 개를 사용합니다.

공식 참고: [Input System 1.20 매뉴얼](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/index.html). 설치한 패키지 버전에서 해당 설정을 다시 확인합니다.

## 4. 실행 환경 표시

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

        // 모바일과 연습용 Editor에서는 방향 버튼을 표시하고, 일반 PC Player에서는 숨깁니다.
        mobileControls.SetActive(Application.isMobilePlatform || Application.isEditor);
    }
}
```

Mobile Controls 필드에 터치 UI의 루트 오브젝트를 연결합니다. Editor에서는 연습할 수 있도록 터치 UI를 표시합니다. Android 프로필을 선택해도 Editor 자체가 Android 기기로 바뀌는 것은 아닙니다. `Application.platform`으로 확인하는 현재 실행 환경과 코드의 컴파일 대상을 구분해서 살펴봅니다.

## 프로젝트 완료 확인

- [ ] WASD·방향키와 화면 방향 버튼으로 같은 Player가 움직입니다. 게임패드는 연결 가능한 환경에서 별도로 확인합니다.
- [ ] 메뉴·결과·중단 상태에서는 이동하지 않고 대각선 속도가 증가하지 않습니다.

오늘의 프로젝트를 `Progress/Day03`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. Move를 읽는 코드가 키보드와 터치에서 같은 이유는 무엇인가요?
2. Editor에서 Android 프로필을 선택하는 것과 Android 실기기에서 실행하는 것은 어떻게 다른가요?

## 오늘의 정리

여러 장치의 입력을 하나의 Action으로 연결하면 같은 이동 코드를 재사용할 수 있습니다. 실행 환경에 따른 동작은 해당 Player에서 직접 확인합니다. 다음 수업에서는 화면 크기 대응, 저장, 앱 중단과 복귀를 다룹니다.
