# DAY 09: Unity 물리 엔진 기초

오늘의 목표는 Unity 물리를 "**게임 세상 안의 간단한 운동 법칙**"으로 이해하고, Collider와 Rigidbody가 충돌과 중력을 어떻게 담당하는지 확인하는 것입니다.

## 1. 핵심 개념: "보이지 않는 충돌 상자"

화면에 보이는 모델과 실제 충돌 판정은 다를 수 있습니다. Collider는 오브젝트 주변에 놓인 보이지 않는 충돌 상자이고, Rigidbody는 물체가 힘과 중력에 반응하게 만드는 부품입니다. 둘을 함께 쓰면 바닥에 떨어지고, 부딪히고, 밀리는 동작을 만들 수 있습니다.

### 이 단어는 무슨 뜻인가요?

- **Collider**: 충돌 범위를 나타내는 컴포넌트입니다.
- **Rigidbody**: 중력, 힘, 속도 같은 물리 계산을 받는 컴포넌트입니다.
- **Is Trigger**: 물리적으로 막지는 않고 겹침 이벤트만 받는 Collider 옵션입니다.
- **Collision**: 서로 막고 튕기는 충돌입니다.
- **Trigger**: 통과는 가능하지만 들어옴과 나감을 감지하는 충돌입니다.

## 2. Rigidbody와 Collider Inspector 주요 프로퍼티

| 컴포넌트 | 프로퍼티 | 의미 |
| :--- | :--- | :--- |
| `Rigidbody` | `Mass` | 물체의 질량입니다. 충돌과 힘 반응에 영향을 줍니다. |
| `Rigidbody` | `Drag` | 이동 속도가 줄어드는 공기 저항입니다. |
| `Rigidbody` | `Angular Drag` | 회전 속도가 줄어드는 저항입니다. |
| `Rigidbody` | `Use Gravity` | 중력을 받을지 정합니다. |
| `Rigidbody` | `Is Kinematic` | 물리 힘 대신 스크립트나 애니메이션으로 움직일지 정합니다. |
| `Rigidbody` | `Interpolate` | 화면에 보이는 움직임을 부드럽게 보정합니다. |
| `Collider` | `Is Trigger` | 물리적으로 막지 않고 겹침 이벤트만 받을지 정합니다. |
| `Collider` | `Material` | 마찰과 튕김 정도를 정하는 Physics Material입니다. |
| `Collider` | `Center` / `Size` | 충돌 범위의 위치와 크기입니다. |

## 실습 예제: Trigger로 문 열기

**미션:** 플레이어가 특정 구역에 들어오면 문 오브젝트가 위로 올라가도록 만듭니다.

1. 플레이어 오브젝트에 `Rigidbody`와 `Collider`를 붙입니다.
2. 구역용 큐브를 만들고 `Collider`의 `Is Trigger`를 켭니다.
3. 문 역할의 큐브를 하나 만들고, 구역용 큐브에 아래 스크립트를 붙입니다.
4. Inspector에서 `door`에 문 큐브를 연결합니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class TriggerDoorOpener : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private float openHeight = 3f;

    private Vector3 closedPosition;

    void Awake()
    {
        closedPosition = door.position;
    }

    void OnTriggerEnter(Collider other)
    {
        door.position = closedPosition + Vector3.up * openHeight;
    }
}
```

</details>

### 실행해보면

플레이어가 Trigger 구역에 들어가는 순간 문 큐브가 위로 올라갑니다. Trigger 구역은 플레이어를 물리적으로 막지 않고, 들어왔다는 사건만 스크립트에 알려 줍니다.

### 생각해보기

1. 문 앞 감지 구역에는 Collision과 Trigger 중 무엇이 더 어울릴까요?
2. Rigidbody가 없는 오브젝트끼리는 물리 이벤트가 왜 잘 발생하지 않을까요?

## 오늘의 정리

- Collider는 충돌 범위, Rigidbody는 물리 반응을 담당합니다.
- Trigger는 겹침 감지용 구역을 만들 때 유용합니다.
- 물리 이벤트를 안정적으로 받으려면 충돌하는 쪽 중 적어도 한쪽에 Rigidbody가 필요합니다.
