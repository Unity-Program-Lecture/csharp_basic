using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class NetworkCharacterPlayer : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private InputAction moveAction;
    private float verticalSpeed;

    private void Awake()
    {
        // CharacterController는 벽·바닥 충돌을 고려하여 캐릭터를 이동시킵니다.
        controller = GetComponent<CharacterController>();
        // 생성 전과 원격 복제본에서는 물리 이동을 하지 않습니다.
        controller.enabled = false;
    }

    // OnNetworkSpawn은 이 오브젝트가 네트워크에 생성될 때 호출됩니다.
    public override void OnNetworkSpawn()
    {
        // IsOwner는 현재 실행 창이 이 Player를 소유하는지 확인합니다.
        if (!IsOwner)
        {
            return;
        }

        // OwnerClientId는 이 Player의 소유자 번호입니다.
        // Host와 별도 Client가 겹치지 않도록 두 사람의 시작 위치를 나눕니다.
        bool isHostPlayer = OwnerClientId == Unity.Netcode.NetworkManager.ServerClientId;
        transform.position = new Vector3(isHostPlayer ? -2f : 2f, 0f, 0f);
        controller.enabled = true;

        // InputAction은 입력을 게임 동작으로 묶습니다. 별도 입력 에셋은 필요 없습니다.
        moveAction = new InputAction("Move", InputActionType.Value);
        // 2DVector는 네 방향 키를 Vector2 값으로 합치는 복합 입력입니다.
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
        // Enable은 이 액션이 입력을 읽기 시작하도록 합니다.
        moveAction.Enable();
    }

    private void Update()
    {
        // IsSpawned는 네트워크 생성 완료 여부입니다. 내 캐릭터만 입력을 처리합니다.
        if (!IsSpawned || !IsOwner || moveAction == null)
        {
            return;
        }

        // ReadValue<Vector2>는 좌우(x), 앞뒤(y) 입력 값을 읽습니다.
        Vector2 input = moveAction.ReadValue<Vector2>();
        // ClampMagnitude는 대각선 입력도 길이 1 이하로 제한합니다.
        Vector3 direction = Vector3.ClampMagnitude(new Vector3(input.x, 0f, input.y), 1f);

        // isGrounded는 직전 Move에서 바닥에 닿았는지 알려 줍니다.
        if (controller.isGrounded && verticalSpeed < 0f)
        {
            verticalSpeed = -2f;
        }
        // Move는 중력을 자동 적용하지 않으므로 수직 속도를 직접 계산합니다.
        verticalSpeed += gravity * Time.deltaTime;
        Vector3 velocity = direction * moveSpeed;
        velocity.y = verticalSpeed;
        // Move에는 속도가 아니라 이번 프레임의 이동 거리를 전달합니다.
        controller.Move(velocity * Time.deltaTime);

        if (direction.sqrMagnitude > 0f)
        {
            // LookRotation은 이동 방향을 바라보는 회전을 만듭니다.
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // OnNetworkDespawn은 네트워크에서 제거될 때 호출됩니다.
    public override void OnNetworkDespawn()
    {
        if (moveAction != null)
        {
            moveAction.Disable();
            // Dispose는 액션이 가진 자원을 정리합니다.
            moveAction.Dispose();
            moveAction = null;
        }
        controller.enabled = false;
    }
}
