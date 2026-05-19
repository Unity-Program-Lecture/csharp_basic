# 🚀 Day 06: 게임 물리 기초 (가속도와 힘)

오늘의 목표는 "**뉴턴의 운동 법칙을 게임에 적용하여, 속도와 가속도의 원리를 이해하고 유니티 물리 엔진 (Rigidbody)을 제어한다**"입니다.

---

## 1. 속도, 가속도, 그리고 힘 (Force)
- **속도** (Velocity): 시간당 위치의 변화량입니다.
- **가속도** (Acceleration): 시간당 속도의 변화량입니다.
- **힘** (Force): 질량을 가진 물체에 가속도를 발생시키는 원인입니다. ($F = ma$)
- 물리 연산은 프레임 속도에 독립적이어야 하므로, 유니티에서는 `Update` 대신 일정한 간격으로 호출되는 `FixedUpdate`를 사용합니다.

---

## 2. 유니티 물리 시스템의 핵심 컴포넌트
유니티 물리 엔진(Nvidia PhysX)을 사용하기 위해 반드시 알아야 할 두 가지 도구입니다.

- **Rigidbody** (강체): 물체를 물리 엔진의 통제하에 두는 컴포넌트입니다. 질량(Mass), 마찰력(Drag), 중력 사용 여부 등을 결정하며, 힘을 받아 실제로 "**움직이는 주체**"가 됩니다.
- **Collider** (충돌체): 물체의 물리적인 "**형태**"를 정의합니다. 콜라이더가 없으면 물체는 물리 법칙이 적용되어도 서로 통과해 버립니다. 보이지 않는 물리적 장막이라고 이해하면 쉽습니다.

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
        rb = GetComponent<Rigidbody>();
    }

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
        
        rb.AddForce(forceDir * pushForce, ForceMode.Force);
    }
}
```

</details>

---

## 4. 등가속도 직선 운동 (Constant Acceleration)
현실적인 점프와 낙하를 구현하기 위해 가장 많이 사용하는 공식입니다.

### 📍 핵심 공식
1. **나중 속도 구하기**: $v = v_0 + at$
   - (현재 속도 = 초기 속도 + 가속도 × 시간)
2. **이동 거리 (변위) 구하기**: $s = v_0t + \frac{1}{2}at^2$
   - (이동 거리 = 초기 속도 × 시간 + 0.5 × 가속도 × 시간의 제곱)

### 🎮 유니티 실무 활용: 낙하 시간 계산
가속도 ($a$) 자리에 중력 가속도 ($g \approx 9.81$)를 대입하면 물체가 바닥에 떨어질 때까지의 시간을 예측할 수 있습니다.

**예시: 높이 10m에서 자유 낙하할 때 걸리는 시간 ($t$)?**
- 공식: $10 = 0 \times t + \frac{1}{2} \times 9.81 \times t^2$
- 결과: $t = \sqrt{20 / 9.81} \approx 1.42$초

> 💡 **Tip**: 유니티 엔진 내부의 `Rigidbody`는 매 프레임 이 공식을 미분하여 계산합니다. 하지만 수류탄의 궤적을 미리 그리거나, 특정 높이까지 점프하기 위한 초기 속도를 계산할 때는 이 공식이 직접적으로 필요합니다.

---

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
2. **문제:** 공기의 저항이 없는 상태에서 속도/가속도가 0인 물체에 Force만큼 힘을 주었을 때, 시간에 따라 속도가 누적되어 점점 빨라지는 이유는 무슨 운동을 하기 때문인가요?
   - **정답:** 등가속도 운동
