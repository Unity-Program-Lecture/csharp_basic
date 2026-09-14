# DAY 11: 입력 요청, 서버 판정, 상호작용 동기화

오늘은 Client가 "획득하고 싶다"고 요청하고 Server가 실제 획득 여부를 결정하게 만듭니다.

## 1. 핵심 개념: "선수의 요청과 심판의 판정"

- **Input Action**: 키보드·게임패드 등의 입력을 `Move`, `Interact`처럼 행동 단위로 묶은 Input System 자산입니다.
- **판정 (Validation)**: Server가 거리, 대상 상태, 중복 여부를 검사하는 과정입니다.
- **Spawn/Despawn**: NetworkObject를 네트워크에 생성하거나 제거하는 일입니다.
- **UGUI (Unity User Interface)**: Canvas 기반의 Unity 화면 UI 시스템입니다.

## 2. 안내형 실습: Server가 코인을 제거하기

**미션:** 코인 Trigger에 닿은 Player를 Server가 확인해 코인을 한 번만 제거합니다.

1. `Assets > Create > Input Actions`를 선택해 `NetworkInputActions`를 만듭니다.
2. Asset을 열고 Action Map `Player`, Action `Move`를 만듭니다. Action Type은 `Value`, Control Type은 `Vector2`로 설정합니다.
3. `Move`에 **Add Binding > Add 2D Vector Composite**를 선택하고 Up/W, Down/S, Left/A, Right/D를 연결합니다.
4. Player Prefab에 `NetworkTransform`을 추가합니다. 이 실습은 Server가 Transform을 변경하는 기본 권한을 유지합니다.
5. `NetworkPlayerMove.cs`를 만들고 Player Prefab에 추가합니다.

```csharp
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerMove : NetworkBehaviour
{
    // InputActionReference는 Inspector에서 만든 Move Input Action을 연결합니다.
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float speed = 4f;

    // OnNetworkSpawn은 이 NetworkObject가 네트워크에 생성된 직후 한 번 호출됩니다.
    public override void OnNetworkSpawn()
    {
        // IsOwner인 Player만 자신의 키보드 입력 Action을 켭니다.
        if (IsOwner)
            moveAction.action.Enable();
    }

    public override void OnNetworkDespawn()
    {
        // NetworkObject가 제거될 때 입력 Action도 꺼서 남은 입력을 막습니다.
        if (IsOwner)
            moveAction.action.Disable();
    }

    private void Update()
    {
        // 다른 Client의 Player에는 로컬 입력을 적용하지 않습니다.
        if (!IsOwner)
            return;

        // ReadValue<Vector2>는 Move Action의 가로·세로 입력값을 읽습니다.
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        if (input.sqrMagnitude > 0f)
            MoveRequestRpc(input);
    }

    [Rpc(SendTo.Server)]
    private void MoveRequestRpc(Vector2 input)
    {
        // Server가 받은 입력으로 위치를 바꾸면 NetworkTransform이 결과를 동기화합니다.
        Vector3 movement = new(input.x, 0f, input.y) * speed * Time.deltaTime;
        transform.position += movement;
    }
}
```

6. Player Prefab의 `moveAction` 칸에 `NetworkInputActions > Player > Move`를 끌어 놓습니다.
7. `NetworkPlay` Scene에 Sphere를 만들고 이름을 `Coin`으로 바꿉니다. 이 실습에서는 Scene에 미리 둔 NetworkObject를 사용합니다.
8. Coin에 `NetworkObject`를 추가합니다.
9. Coin의 Sphere Collider에서 **Is Trigger**를 켭니다.
10. Coin에 Rigidbody를 추가하고 **Is Kinematic**을 켭니다.
11. `NetworkCoin.cs`를 만들고 Coin에 추가합니다.

```csharp
using Unity.Netcode;
using UnityEngine;

public class NetworkCoin : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // IsServer로 검사해 Coin 제거와 점수 변경은 Server에서만 실행합니다.
        if (!IsServer)
            return;

        // GetComponent는 충돌한 오브젝트에서 점수 관리 컴포넌트를 찾습니다.
        NetworkScorePlayer player = other.GetComponent<NetworkScorePlayer>();
        if (player == null)
            return;

        // Server가 공유 점수를 변경하면 NetworkVariable이 모든 Client에 반영합니다.
        player.Score.Value += 1;
        // Despawn은 Coin NetworkObject를 모든 참여자에게서 제거합니다.
        NetworkObject.Despawn();
    }
}
```

12. Player Prefab에 Collider와 Rigidbody가 있는지 확인합니다. Rigidbody는 **Is Kinematic**을 켭니다.
13. Host를 시작합니다. Scene에 놓인 Coin NetworkObject가 Host에서 네트워크 오브젝트로 생성되는지 확인합니다.
14. Client를 연결한 뒤 Host 또는 Client Player가 Coin에 닿게 합니다.

### 실행해보면

- Coin은 한 번만 사라집니다.
- 해당 Player의 Score가 Server에서 증가하고 양쪽 창에 동기화됩니다.
- Client가 Coin을 직접 지우는 로그는 없어야 합니다.

### 흔한 오류

| 증상 | 먼저 확인할 것 |
| :--- | :--- |
| Trigger가 호출되지 않음 | Player 또는 Coin에 Rigidbody/Collider 조건이 맞는지 확인합니다. |
| Coin이 한쪽에서만 사라짐 | Coin이 NetworkObject인지, Server에서 `Despawn()`했는지 확인합니다. |
| 점수가 두 번 올라감 | Coin이 제거되기 전 중복 Trigger가 가능한지, Server 조건이 있는지 확인합니다. |

## 응용 실습: 문 열기

NetworkObject인 문을 만들고, Player가 가까울 때만 Server가 문을 열도록 구현하세요. 열린 상태는 모든 Client에서 같아야 합니다.

## 오늘의 정리

- Client는 입력과 요청을 보내고, Server는 상호작용의 유효성과 결과를 판정합니다.
