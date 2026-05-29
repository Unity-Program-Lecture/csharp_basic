# 🚀 Day 06: 게임 물리 기초 (가속도와 힘)

오늘의 목표는 "**뉴턴의 운동 법칙을 게임에 적용하여, 속도와 가속도의 원리를 이해하고 유니티 물리 엔진 (Rigidbody)을 제어한다**"입니다.

---

## 1. 속도, 가속도, 그리고 힘 (Force)
- **속도** (Velocity): 시간당 위치의 변화량입니다.
- **가속도** (Acceleration): 시간당 속도의 변화량입니다.
- **힘** (Force): 질량을 가진 물체에 가속도를 발생시키는 원인입니다.
  ```text
  힘 = 질량 x 가속도
  F = m * a
  ```
- 물리 연산은 프레임 속도에 독립적이어야 하므로, 유니티에서는 `Update` 대신 일정한 간격으로 호출되는 `FixedUpdate`를 사용합니다.

---

## 2. 유니티 물리 시스템의 핵심 컴포넌트
유니티 물리 엔진(Nvidia PhysX)을 사용하기 위해 반드시 알아야 할 두 가지 도구입니다.

- **Rigidbody** (강체): 물체를 물리 엔진의 통제하에 두는 컴포넌트입니다. 질량(Mass), 마찰력(Drag), 중력 사용 여부 등을 결정하며, 힘을 받아 실제로 "**움직이는 주체**"가 됩니다.
- **Collider** (충돌체): 물체의 물리적인 "**형태**"를 정의합니다. 콜라이더가 없으면 물체는 물리 법칙이 적용되어도 서로 통과해 버립니다. 보이지 않는 물리적 장막이라고 이해하면 쉽습니다.

### 🔎 Rigidbody 인스펙터 주요 항목
`Rigidbody`는 "이 물체가 얼마나 무겁고, 얼마나 쉽게 밀리고, 어떤 방식으로 물리 계산에 참여하는가"를 정하는 컴포넌트입니다.

| 항목 | 의미 | 수업에서 보는 포인트 |
| :--- | :--- | :--- |
| **Mass** | 물체의 질량입니다. 값이 클수록 같은 힘을 받아도 덜 빨라집니다. | 일반 몬스터와 보스 몬스터의 넉백 차이를 만들 때 사용합니다. |
| **Linear Damping** | 이동 속도를 서서히 줄이는 저항입니다. Unity 버전에 따라 **Drag**로 보일 수 있습니다. | 얼음판처럼 미끄러지게 할지, 진흙처럼 금방 멈추게 할지 조절합니다. |
| **Angular Damping** | 회전 속도를 서서히 줄이는 저항입니다. Unity 버전에 따라 **Angular Drag**로 보일 수 있습니다. | 넘어지거나 회전한 물체가 계속 빙글빙글 도는 현상을 줄입니다. |
| **Use Gravity** | 프로젝트의 중력을 받을지 정합니다. | 체크하면 아래로 떨어지고, 끄면 우주 공간처럼 떠 있습니다. |
| **Is Kinematic** | 물리 엔진의 힘으로 움직일지, 코드/애니메이션으로 직접 움직일지 정합니다. | 움직이는 발판처럼 직접 제어하지만 충돌 정보는 필요한 물체에 사용합니다. |
| **Interpolate** | 화면에 보이는 움직임을 부드럽게 보정합니다. | 물리 계산 간격 때문에 물체가 미세하게 끊겨 보일 때 사용합니다. |
| **Collision Detection** | 빠르게 움직이는 물체의 충돌 검사 방식을 정합니다. | 총알처럼 빠른 물체가 벽을 뚫고 지나갈 때 더 정교한 옵션을 고려합니다. |
| **Constraints** | 특정 축의 이동이나 회전을 잠급니다. | 3D 물리 객체를 2D처럼 움직이게 하거나, 넘어지지 않게 고정할 때 사용합니다. |

### 🔎 Collider 인스펙터 주요 항목
`Collider`는 "이 물체의 실제 충돌 모양이 어디까지인가"를 정하는 컴포넌트입니다. 렌더링용 모델과 충돌 모양은 서로 다를 수 있습니다.

| 항목 | 의미 | 수업에서 보는 포인트 |
| :--- | :--- | :--- |
| **Is Trigger** | 물리적으로 밀고 튕기지 않고, 들어왔는지만 감지합니다. | 공격 범위, 아이템 획득 범위, 감지 구역에 사용합니다. |
| **Material** | 마찰력과 튕김 정도를 담은 Physics Material을 연결합니다. | 잘 미끄러지는 바닥, 잘 튕기는 공 같은 느낌을 만듭니다. |
| **Center** | 충돌 모양의 중심 위치입니다. | 모델 중심과 충돌 중심이 어긋날 때 조절합니다. |
| **Size** | `Box Collider`의 가로, 세로, 깊이 크기입니다. | 상자, 벽, 발판처럼 각진 물체에 적합합니다. |
| **Radius** | `Sphere Collider`나 `Capsule Collider`의 반지름입니다. | 공, 감지 범위, 캐릭터 몸통 폭을 정할 때 사용합니다. |
| **Height** | `Capsule Collider`의 전체 높이입니다. | 사람형 캐릭터처럼 둥근 머리와 발을 가진 충돌체에 적합합니다. |
| **Direction** | `Capsule Collider`가 어느 축으로 길게 놓일지 정합니다. | 캐릭터는 보통 Y축 방향 캡슐을 사용합니다. |
| **Convex** | `Mesh Collider`를 물리적으로 움직일 수 있는 단순한 볼록 형태로 처리합니다. | 복잡한 모델을 Rigidbody와 함께 쓸 때 필요할 수 있습니다. |

> 💡 **실습 팁:** Scene 뷰에서 Collider의 초록색 윤곽선을 확인하세요. 모델은 예쁘게 보여도, 실제 충돌은 이 윤곽선 기준으로 일어납니다.

### 💡 꿀팁: Rigidbody와 Collider의 조합
1. **Rigidbody만 있음**: 중력은 받지만 바닥을 뚫고 무한히 추락하는 "**유령 상태**"가 됩니다.
2. **Collider만 있음**: 벽이나 지면처럼 움직이지 않는 "**고정 장애물**" (Static Collider)이 됩니다.
3. **둘 다 있음**: 부딪히고 튕겨 나가는 "**완전한 물리 객체**"가 됩니다.
4. **⚠️ 주의: Rigidbody 없이 Collider만 움직이는 경우**: 물리 엔진에서 가장 피해야 할 행동 중 하나입니다.

### ⚠️ 왜 Rigidbody 없는 콜라이더를 움직이면 안 되나요?
- **성능 저하 (CPU 부하)**: 유니티는 리지드바디가 없는 콜라이더를 "**움직이지 않는 배경**"으로 간주하고 물리 지도를 미리 그려둡니다. 그런데 이 물체가 움직이면 엔진은 매번 "**물리 지도를 통째로 다시 그려야**" 하므로 CPU에 큰 부담을 줍니다.
- **물리 오동작**: 엔진은 이 물체가 순간이동한 것으로 착각할 수 있습니다. 이 과정에서 다른 물체를 밀어내지 못하고 겹쳐버리거나, 충돌 감지가 누락되는 등 "**물리적 버그**"가 발생할 확률이 매우 높습니다.
- **해결책**: 힘에 의해 밀려나지는 않지만 직접 움직여야 하는 물체(예: 움직이는 발판)라면, 반드시 "**Rigidbody**"를 추가하고 "**isKinematic**" 옵션을 체크하여 "**키네매틱 리지드바디**"로 만들어야 합니다.

### 🏗️ 복합 충돌체 (Compound Collider)
복잡한 모양의 물체를 만들 때 아주 중요한 기법입니다.
- **원리**: 부모 오브젝트에 "**Rigidbody**"를 하나만 두고, 자식 오브젝트들에 여러 개의 "**Collider**"를 배치하는 방식입니다.
- **장점**: 유니티는 자식들의 콜라이더를 모두 합쳐 부모 리지드바디에 속한 "**하나의 물리 덩어리**"로 취급합니다. 복잡한 메쉬 콜라이더를 쓰는 것보다 성능이 훨씬 뛰어나며 정교한 충돌 판정이 가능합니다.
- **비유**: 부모는 전체를 움직이는 "**엔진**"이고, 자식들은 각 부위의 "**외장 장갑**" 역할을 한다고 이해하면 쉽습니다.

---

## 3. Rigidbody를 이용한 물리 기반 이동
**미션:** 유니티의 `Rigidbody` 컴포넌트와 `AddForce` 메서드를 사용하여, 질량 (Mass)과 힘 (Force)에 의한 가속도 기반 이동을 구현하세요.

### 📌 AddForce와 ForceMode
`Rigidbody.AddForce(힘의 방향과 크기, ForceMode)`에서 `ForceMode`는 "이 힘을 어떤 방식으로 전달할 것인가"를 정합니다. 같은 숫자를 넣어도 모드에 따라 움직임이 크게 달라집니다.

| ForceMode | 질량 영향 | 적용 방식 | 사용 예시 |
| :--- | :--- | :--- | :--- |
| `ForceMode.Force` | 받음 | 매 `FixedUpdate`마다 계속 미는 힘입니다. | 바람, 엔진 추진, 계속 누르고 있는 이동 입력 |
| `ForceMode.Acceleration` | 받지 않음 | 질량과 상관없이 같은 가속도를 계속 줍니다. | 모든 물체를 같은 속도로 밀고 싶을 때 |
| `ForceMode.Impulse` | 받음 | 한 순간에 강하게 치는 힘입니다. | 점프, 폭발, 타격 넉백 |
| `ForceMode.VelocityChange` | 받지 않음 | 질량과 상관없이 속도를 즉시 바꿉니다. | 대시, 순간 회피, 튜토리얼용 이동 보정 |

이번 예제에서는 키를 누르는 동안 계속 밀어야 하므로 `ForceMode.Force`를 사용합니다. 공격을 맞는 순간의 넉백처럼 "한 번 쾅" 밀어야 할 때는 `ForceMode.Impulse`가 더 자연스럽습니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem; // 최신 인풋 시스템

[RequireComponent(typeof(Rigidbody))]
public class PhysicsMove : MonoBehaviour
{
    private Rigidbody rb;
    public float pushForce = 10f;

    void Start()
    {
        // GetComponent<T>()는 같은 게임 오브젝트에 붙은 T 타입 컴포넌트를 가져오는 메서드입니다.
        rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate는 물리 계산 주기에 맞춰 자동 호출되므로 Rigidbody 이동에 적합합니다.
    void FixedUpdate()
    {
        float h = 0;
        float v = 0;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h = 1;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v = 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v = -1;
        }

        Vector3 forceDir = new Vector3(h, 0, v).normalized;

        // Rigidbody.AddForce는 Rigidbody에 힘을 가해 물리 엔진이 속도를 바꾸게 하는 메서드입니다.
        // ForceMode.Force는 질량을 고려하면서 계속 밀어 주는 방식입니다.
        rb.AddForce(forceDir * pushForce, ForceMode.Force);
    }
}
```

</details>

## 🎯 [심화 미션] 몬스터 사냥 시스템: 힘과 질량에 따른 넉백 구현
### [요구 사항]
- 플레이어의 공격을 받은 몬스터가 뒤로 밀려나는 '넉백' 효과를 `AddForce`를 이용해 기획하세요.
- 일반 몬스터는 크게 밀려나지만, 보스 몬스터는 질량이 크기 때문에 거의 밀려나지 않도록 질량(Mass) 설정을 고려하세요.
- 몬스터가 공중에 떠 있는 상태에서 공격받았을 때와 지면에 있을 때의 저항 차이를 구상해 보세요.

### [프로그래밍 힌트]
- `ForceMode.Impulse`를 사용하면 타격감을 살리기 좋습니다.
- `Rigidbody.drag` 값을 조절하여 넉백 후 멈추는 속도를 제어할 수 있습니다.

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 유니티에서 물리 엔진 연산을 처리할 때 프레임 드랍의 영향을 받지 않기 위해 코드를 작성해야 하는 생명주기 메서드 이름은 무엇인가요?
   - **정답:** `FixedUpdate()`
2. **문제:** 같은 힘으로 일반 몬스터와 보스 몬스터를 밀었을 때, 질량이 더 큰 보스 몬스터가 덜 밀려나는 이유는 무엇인가요?
   - **정답:** 힘이 같다면 질량이 클수록 가속도가 작아지기 때문입니다. (`F = m * a`)
