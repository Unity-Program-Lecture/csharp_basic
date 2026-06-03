# DAY 02: 플레이어 캐릭터 이동과 반응

오늘의 목표는 플레이어 캐릭터를 "**입력에 맞춰 자세를 바꾸는 배우**"처럼 이해하고, 이동 방향과 바라보는 방향을 함께 처리하는 기본 캐릭터 컨트롤러를 만드는 것입니다.

## 1. 핵심 개념: "입력은 방향이고, 캐릭터는 반응이다"

플레이어가 이동 키를 누르면 클라이언트는 먼저 이동 방향을 계산합니다. 그다음 캐릭터를 움직이고, 방향을 돌리고, 필요하다면 애니메이션 상태를 바꿉니다. 즉, 캐릭터 제어는 단순히 위치만 바꾸는 일이 아니라 플레이어의 의도를 게임 속 몸짓으로 바꾸는 일입니다.

이번 단계에서는 복잡한 애니메이션 대신 `Rigidbody` 이동과 회전만 다룹니다. NCS 원문의 캐릭터 반응 설계는 Unity 6 수업 흐름에 맞춰 "입력에 따른 이동, 회전, 상태 갱신"으로 작게 나누어 실습합니다.

### 이 단어는 무슨 뜻인가요?

- **Character Controller**: 플레이어 캐릭터의 이동, 회전, 상태를 관리하는 코드입니다.
- **Move Direction**: 입력으로 만들어진 이동 방향입니다.
- **Look Direction**: 캐릭터가 바라볼 방향입니다.
- **Rigidbody**: Unity 물리 계산으로 오브젝트를 움직이게 하는 컴포넌트입니다.
- **linearVelocity**: Rigidbody가 현재 어느 방향으로 얼마나 빠르게 움직이는지 나타내는 속도 값입니다.

## 실습 예제: 입력 방향으로 이동하고 바라보기

**미션:** 이동 입력을 받아 캐릭터가 움직이는 방향을 바라보게 만듭니다.

1. Capsule 또는 Cube 오브젝트를 만들고 `Rigidbody`를 붙입니다.
2. Player Input의 `Move` 액션을 `Vector2`로 연결합니다.
3. 아래 스크립트를 플레이어 오브젝트에 붙입니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ClientPlayerMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 12f;

    private Rigidbody body;
    private Vector2 moveInput;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);

        if (direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        body.linearVelocity = new Vector3(
            direction.x * moveSpeed,
            body.linearVelocity.y,
            direction.z * moveSpeed
        );

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }
    }
}
```

</details>

### 실행해보면

플레이어가 입력 방향으로 이동하고, 이동 중에는 그 방향을 바라봅니다. 정지하면 마지막으로 바라본 방향을 유지합니다.

### 생각해보기

1. 이동은 되는데 회전이 없다면 플레이어가 조작감을 어떻게 느낄까요?
2. `moveSpeed`와 `turnSpeed`를 Inspector에서 조절하면 캐릭터 느낌이 어떻게 달라질까요?

## 오늘의 정리

- 플레이어 캐릭터 제어는 입력 방향을 이동과 회전 반응으로 바꾸는 과정입니다.
- 물리 기반 이동은 `FixedUpdate`에서 처리하면 계산 타이밍이 안정적입니다.
- 캐릭터가 바라보는 방향은 조작감과 피드백에 큰 영향을 줍니다.
