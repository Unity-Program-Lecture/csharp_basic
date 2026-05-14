# 🚀 Day 05: 게임 물리 - 운동의 기초 (Speed & Acceleration)

오늘의 목표는 "**위치, 속도, 가속도의 수학적 관계를 이해하고, 미분/적분의 개념을 게임 코드에 적용하여 등가속도 운동을 구현한다**"입니다.

---

## 1. 위치, 속도, 가속도의 관계 (미분과 적분)
게임 엔진은 매 프레임 아주 짧은 시간(`DeltaTime`) 동안의 변화를 계산하여 물체를 이동시킵니다.

- **속도 (Velocity)**: 위치의 변화율. (위치를 시간에 대해 미분)
- **가속도 (Acceleration)**: 속도의 변화율. (속도를 시간에 대해 미분)
- **적분 (Integration)**: 반대로 가속도를 시간에 따라 쌓으면 속도가 되고, 속도를 쌓으면 위치가 됩니다.

---

## 2. 등가속도 직선 운동 (Constant Acceleration)
가장 대표적인 예시는 **중력**입니다. 시간에 따라 속도가 일정하게 증가하는 운동입니다.

### 📍 핵심 공식
1. **속도 공식**: $v = v_0 + at$
2. **변위(위치 변화) 공식**: $s = v_0t + \frac{1}{2}at^2$

---

## 💻 실습 예제: 중력 직접 구현하기 (Rigidbody 없이)
유니티의 `Update` 문에서 수학 공식을 직접 코드로 옮겨 중력을 시뮬레이션해 봅니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class ManualGravity : MonoBehaviour
{
    public float gravity = -9.81f;
    private float currentVelocityY = 0f;

    void Update()
    {
        // 1. 가속도를 속도에 적분 (v = v0 + at)
        currentVelocityY += gravity * Time.deltaTime;

        // 2. 속도를 위치에 적분 (y = y0 + vt)
        // 실제로는 공식 s = vt + 0.5at^2를 쓰지만, 매 프레임 계산 시에는 미분 형태로 적용함
        Vector3 pos = transform.position;
        pos.y += currentVelocityY * Time.deltaTime;

        // 3. 지면 충돌 시 정지 (임시 로직)
        if (pos.y < 0)
        {
            pos.y = 0;
            currentVelocityY = 0;
        }

        transform.position = pos;
    }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 시간당 속도의 변화량을 나타내는 물리적 개념은 무엇입니까?
   - **정답:** 가속도 (Acceleration)
2. **문제:** 유니티 엔진에서 프레임 속도와 상관없이 일정한 물리 계산을 하기 위해 사용하는 시간 값 변수는?
   - **정답:** `Time.deltaTime`
