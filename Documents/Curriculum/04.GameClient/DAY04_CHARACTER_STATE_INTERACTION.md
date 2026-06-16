# DAY 04: 캐릭터 상태와 상호작용

오늘의 목표는 캐릭터 상태를 "**캐릭터가 지금 들고 있는 상황표**"처럼 이해하고, 가까운 오브젝트와 상호작용할 수 있는 기본 구조를 만드는 것입니다.

## 1. 핵심 개념: "상호작용은 조건을 확인한 뒤 실행된다"

게임에서 플레이어가 상자를 열거나 NPC와 대화하거나 아이템을 줍는 일은 모두 상호작용입니다. 하지만 아무 때나 실행되면 안 됩니다. 플레이어가 충분히 가까운지, 상호작용 가능한 대상인지, 지금 캐릭터가 행동할 수 있는 상태인지 먼저 확인해야 합니다.

상태는 캐릭터의 현재 상황을 정리한 표시판입니다. `Idle`, `Move`, `Interact`처럼 상태를 나누면 코드가 "지금 무엇을 할 수 있는가"를 판단하기 쉬워집니다.

### 이 단어는 무슨 뜻인가요?

- **Interaction**: 플레이어가 오브젝트, NPC, 아이템 등과 주고받는 행동입니다.
- **Interactable**: 상호작용할 수 있는 대상입니다.
- **Trigger**: 물리 충돌처럼 밀어내지는 않지만, 들어오고 나간 사실을 감지하는 Collider 설정입니다.
- **Interface**: 여러 클래스가 같은 이름의 기능을 제공하게 만드는 약속입니다.
- **Character State**: 캐릭터가 현재 할 수 있는 행동을 판단하기 위한 상태 값입니다.

## 실습 예제: 가까운 상자와 상호작용하기

**미션:** 플레이어가 Trigger 범위 안의 오브젝트를 기억하고, `Interact` 입력으로 기능을 실행합니다.

1. 플레이어에 Collider와 Rigidbody를 붙입니다.
2. 상자 오브젝트에 Collider를 붙이고 `Is Trigger`를 켭니다.
3. Player Input에 `Interact` 액션을 Button으로 추가합니다.
4. 아래 스크립트들을 각각 파일로 만든 뒤 플레이어와 상자에 붙입니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void Interact();
}

public class PlayerInteractor : MonoBehaviour
{
    private IInteractable currentTarget;
    private string characterState = "Idle";

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed || currentTarget == null)
        {
            return;
        }

        characterState = "Interact";
        currentTarget.Interact();
        Debug.Log($"State: {characterState}");
        characterState = "Idle";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentTarget = interactable;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable == currentTarget)
        {
            currentTarget = null;
        }
    }
}
```

</details>

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class TreasureBox : MonoBehaviour, IInteractable
{
    private bool isOpen;

    public void Interact()
    {
        if (isOpen)
        {
            Debug.Log("상자는 이미 열려 있습니다.");
            return;
        }

        isOpen = true;
        Debug.Log("상자를 열고 보상을 확인했습니다.");
    }
}
```

</details>

### 실행해보면

플레이어가 상자 Trigger 안에 들어간 뒤 상호작용 입력을 누르면 상자가 열립니다. 한 번 열린 상자는 다시 열리지 않고 안내 메시지를 출력합니다.

### 생각해보기

1. `currentTarget`을 저장하지 않으면 상호작용 입력을 눌렀을 때 어떤 대상을 실행해야 할지 어떻게 알 수 있을까요?
2. 상자, NPC, 문이 모두 `IInteractable`을 사용하면 어떤 점이 편해질까요?

## 오늘의 정리

- 상호작용은 가까운 대상 확인, 입력 확인, 실행 순서로 처리합니다.
- 상태 값은 캐릭터가 지금 어떤 행동 중인지 정리해 줍니다.
- 인터페이스를 사용하면 서로 다른 오브젝트를 같은 방식으로 다룰 수 있습니다.
