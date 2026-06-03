# DAY 01: 게임 클라이언트 구조와 입력 흐름

오늘의 목표는 게임 클라이언트를 "**플레이어의 손과 게임 세계를 이어 주는 조종석**"처럼 이해하고, 입력이 캐릭터 상태로 전달되는 기본 흐름을 Unity 6에서 만들어 보는 것입니다.

## 1. 핵심 개념: "클라이언트는 플레이어가 만지는 게임"

게임 클라이언트는 플레이어가 직접 실행하고 조작하는 프로그램입니다. 서버가 규칙과 저장소 역할을 맡는다면, 클라이언트는 화면을 보여 주고, 입력을 받고, 캐릭터와 오브젝트가 반응하는 모습을 즉시 보여 줍니다.

입력 흐름은 보통 `입력 장치 -> Input Action -> 플레이어 상태 -> 캐릭터 반응` 순서로 이어집니다. 키보드의 `W`를 눌렀다는 사실보다 중요한 것은 그 입력이 "**앞으로 이동하고 싶다**"라는 의미로 바뀌는 과정입니다.

### 이 단어는 무슨 뜻인가요?

- **Client**: 플레이어의 PC나 기기에서 실행되는 게임 프로그램입니다.
- **Server**: 여러 클라이언트가 공유해야 하는 데이터와 규칙을 관리하는 프로그램입니다.
- **Input Action**: 키보드, 마우스, 게임패드 입력을 `Move`, `Jump`, `Attack` 같은 행동 이름으로 묶은 것입니다.
- **State**: 캐릭터가 현재 어떤 상태인지 나타내는 값입니다. 예: 대기, 이동, 공격 준비.
- **Feedback**: 입력 결과를 화면, 소리, UI 등으로 플레이어에게 알려 주는 반응입니다.

## 실습 예제: 입력 값을 플레이어 상태로 바꾸기

**미션:** Input System으로 이동 입력을 받고, 현재 입력 상태를 화면 디버그 메시지로 확인합니다.

1. 빈 GameObject를 만들고 이름을 `ClientInputTester`로 바꿉니다.
2. Player Input 컴포넌트를 추가하고 `Move` 액션을 `Vector2` 값으로 연결합니다.
3. 아래 스크립트를 붙입니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class ClientInputTester : MonoBehaviour
{
    private Vector2 moveInput;
    private string playerState = "Idle";

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput.sqrMagnitude > 0.01f)
        {
            playerState = "Move";
        }
        else
        {
            playerState = "Idle";
        }
    }

    void Update()
    {
        Debug.Log($"Input: {moveInput}, State: {playerState}");
    }
}
```

</details>

### 실행해보면

이동 키를 누르면 `State`가 `Move`로 바뀌고, 손을 떼면 `Idle`로 돌아옵니다. 아직 캐릭터는 움직이지 않지만, 클라이언트가 입력을 게임 상태로 바꾸는 첫 단계를 확인할 수 있습니다.

### 생각해보기

1. 키보드 입력을 바로 이동 코드에 넣지 않고 `Move` 같은 행동 이름으로 바꾸면 어떤 점이 편할까요?
2. 서버가 없어도 클라이언트에서 먼저 처리해야 하는 반응은 무엇이 있을까요?

## 오늘의 정리

- 게임 클라이언트는 플레이어가 직접 보고 조작하는 실행 프로그램입니다.
- 입력은 장치 값에서 게임 행동으로 바뀐 뒤 캐릭터 상태에 반영됩니다.
- 좋은 클라이언트 코드는 입력, 상태, 반응을 한 덩어리로 섞지 않고 순서대로 연결합니다.
