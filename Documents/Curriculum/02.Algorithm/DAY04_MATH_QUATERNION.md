# 🚀 Day 04: 게임 수학 - 회전과 사원수 (Quaternion & Slerp)

오늘의 목표는 "**오일러 각도의 한계를 극복하는 쿼터니언(사원수)의 개념을 이해하고, 부동 소수점 오차 없는 부드러운 회전을 구현한다**"입니다.

---

## 1. 오일러 각도(Euler Angles)와 짐벌락(Gimbal Lock)
우리가 흔히 쓰는 (x, y, z) 각도 표현은 직관적이지만 치명적인 단점이 있습니다.

- **오일러 각도**: 세 축을 순서대로 회전시키는 방식.
- **짐벌락 현상**: 두 회전축이 겹쳐지면서 한 축의 자유도를 상실하는 현상입니다. (예: 위를 쳐다볼 때 좌우 회전이 꼬이는 경우)

---

## 2. 쿼터니언(Quaternion): "4차원 복소수 회전"
유니티는 내부적으로 회전을 처리할 때 4개의 성분(x, y, z, w)을 가진 쿼터니언을 사용합니다.

### 📍 쿼터니언의 장점
1. **짐벌락이 없습니다.**
2. **회전 보간(Interpolation)이 매우 부드럽고 정확합니다.**
3. 행렬보다 메모리를 적게 사용하고 연산 속도가 빠릅니다.

---

## 3. 부드러운 회전의 핵심: Slerp
단순한 숫자 더하기가 아닌, 구면 위를 따라 최단 거리로 회전하는 기술입니다.

- **Lerp (Linear Interpolation)**: 직선 보간.
- **Slerp (Spherical Linear Interpolation)**: 구면 선형 보간. (회전에서 주로 사용)

---

## 💻 실습 예제: 대상을 향해 부드럽게 고개 돌리기
```csharp
using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 2f;

    void Update()
    {
        if (target == null) return;

        // 1. 목표 방향 계산 (Target - Me)
        Vector3 direction = (target.position - transform.position).normalized;

        // 2. 방향을 쿼터니언 회전값으로 변환
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // 3. 현재 회전에서 목표 회전까지 부드럽게 보간 (Slerp)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
```

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 오일러 각도 방식으로 회전할 때 두 축이 겹쳐 회전이 불가능해지는 현상을 무엇이라 합니까?
   - **정답:** 짐벌락 (Gimbal Lock)
2. **문제:** 두 회전값 사이를 최단 경로로 부드럽게 연결해 주는 보간 함수의 이름은?
   - **정답:** Slerp (구면 선형 보간)
