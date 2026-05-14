# 🚀 Day 06: 게임 물리 기초 (가속도와 힘)

오늘의 목표는 "**뉴턴의 운동 법칙을 게임에 적용하여, 속도와 가속도의 원리를 이해하고 유니티 물리 엔진(Rigidbody)을 제어한다**"입니다.

---

## 1. 속도, 가속도, 그리고 힘(Force)
- **속도 (Velocity)**: 시간당 위치의 변화량입니다.
- **가속도 (Acceleration)**: 시간당 속도의 변화량입니다.
- **힘 (Force)**: 질량을 가진 물체에 가속도를 발생시키는 원인입니다. ($F = ma$)
- 물리 연산은 프레임 속도에 독립적이어야 하므로, 유니티에서는 `Update` 대신 일정한 간격으로 호출되는 `FixedUpdate`를 사용합니다.

---

## 2. Rigidbody를 이용한 물리 기반 이동
**미션:** 유니티의 `Rigidbody` 컴포넌트와 `AddForce` 메서드를 사용하여, 질량(Mass)과 힘(Force)에 의한 가속도 기반 이동을 구현하세요.

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

## 3. 등가속도 직선 운동 (Constant Acceleration)
현실적인 점프와 낙하를 구현하기 위해 가장 많이 사용하는 공식입니다.

### 📍 핵심 공식
1. **나중 속도 구하기**: $v = v_0 + at$
   - (현재 속도 = 초기 속도 + 가속도 × 시간)
2. **이동 거리(변위) 구하기**: $s = v_0t + \frac{1}{2}at^2$
   - (이동 거리 = 초기 속도 × 시간 + 0.5 × 가속도 × 시간의 제곱)

### 🎮 유니티 실무 활용: 낙하 시간 계산
가속도($a$) 자리에 중력 가속도($g \approx 9.81$)를 대입하면 물체가 바닥에 떨어질 때까지의 시간을 예측할 수 있습니다.

**예시: 높이 10m에서 자유 낙하할 때 걸리는 시간($t$)?**
- 공식: $10 = 0 \times t + \frac{1}{2} \times 9.81 \times t^2$
- 결과: $t = \sqrt{20 / 9.81} \approx 1.42$초

> 💡 **Tip**: 유니티 엔진 내부의 `Rigidbody`는 매 프레임 이 공식을 미분하여 계산합니다. 하지만 수류탄의 궤적을 미리 그리거나, 특정 높이까지 점프하기 위한 초기 속도를 계산할 때는 이 공식이 직접적으로 필요합니다.

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 유니티에서 물리 엔진 연산을 처리할 때 프레임 드랍의 영향을 받지 않기 위해 코드를 작성해야 하는 생명주기 메서드 이름은 무엇인가요?
   - **정답:** `FixedUpdate()`
2. **문제:** 공기의 저항이 없는 상태에서 속도/가속도가 0인 물체에 Force만큼 힘을 주었을 때, 시간에 따라 속도가 누적되어 점점 빨라지는 이유는 무슨 운동을 하기 때문인가요?
   - **정답:** 등가속도 운동
