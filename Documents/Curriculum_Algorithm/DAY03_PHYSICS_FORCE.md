# 🚀 Day 03: 게임 물리 기초 (가속도와 힘)

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

[RequireComponent(typeof(Rigidbody))]
public class PhysicsMove : MonoBehaviour
{
    private Rigidbody rb;
    public float pushForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // 물리 연산은 FixedUpdate에서 처리
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forceDir = new Vector3(h, 0, v);
        
        // 질량과 물리 법칙에 기반한 힘 가하기
        // ForceMode.Force: 연속적인 힘 적용 (질량 영향 받음)
        rb.AddForce(forceDir * pushForce, ForceMode.Force);
    }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 유니티에서 물리 엔진 연산을 처리할 때 프레임 드랍의 영향을 받지 않기 위해 코드를 작성해야 하는 생명주기 메서드 이름은 무엇인가요?
   - **정답:** `FixedUpdate()`
2. **문제:** 공기의 저항이 없는 상태에서 속도/가속도가 0인 물체에 Force만큼 힘을 주었을 때, 시간에 따라 속도가 누적되어 점점 빨라지는 이유는 무슨 운동을 하기 때문인가요?
   - **정답:** 등가속도 운동
