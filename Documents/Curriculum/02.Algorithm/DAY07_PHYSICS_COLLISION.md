# 🚀 Day 07: 게임 물리 - 충돌 감지 기초 (Bounding Box & Sphere)

오늘의 목표는 "**물체 간의 겹침을 수학적으로 판별하는 충돌 알고리즘의 원리를 이해하고, 가장 효율적인 바운딩 볼륨 방식을 구현한다**"입니다.

---

## 1. 충돌 판별의 원리: "경계 영역 (Bounding Volume)"
모든 물체의 정교한 표면을 실시간으로 계산하는 것은 매우 무겁습니다. 따라서 단순한 형태의 '상자'나 '구'로 감싸서 먼저 계산합니다.

### 📍 대표적인 충돌 영역 방식

#### AABB (Axis-Aligned Bounding Box)

<p align="center">
  <img src="Images/aabb.svg" width="180" alt="AABB Diagram">
</p>

- **특징**: 세상의 X, Y, Z축에 나란하게 고정된 상자입니다.
- **장점**: 물체가 회전해도 상자는 회전하지 않으므로 계산이 "**가장 빠르고 단순**"합니다.

#### Bounding Sphere (경계 구)

<p align="center">
  <img src="Images/sphere.svg" width="180" alt="Bounding Sphere Diagram">
</p>

- **특징**: 물체를 완전히 감싸는 최소 크기의 구입니다.
- **장점**: "**중심점 사이의 거리**"만 측정하면 되므로 연산 효율이 극대화됩니다.

#### OBB (Oriented Bounding Box)

<p align="center">
  <img src="Images/obb.svg" width="180" alt="OBB Diagram">
</p>

- **특징**: 물체의 "**회전 방향에 맞춰 상자도 함께 회전**"합니다.
- **장점**: 빈 공간이 가장 적어 "**정확한 충돌 범위**"를 제공합니다.
- **비용**: 복잡한 수학 연산이 필요하여 연산 비용이 높습니다.

---

## 2. 알고리즘 구현 원리

### 📍 원 충돌 (Circle/Sphere Collision)

<p align="center">
  <img src="Images/sphere_collision.svg" width="220" alt="Sphere Collision Logic">
</p>

- **공식**: `Distance <= (Radius A + Radius B)`
- **최적화**: 루트 연산을 피하기 위해 "**거리의 제곱**"을 비교합니다.

### 📍 AABB 충돌 (축 정렬 상자 충돌)

<p align="center">
  <img src="Images/aabb_collision.svg" width="220" alt="AABB Collision Logic">
</p>

- **판정 조건**: 모든 축 (X, Y, Z)에서 범위가 겹쳐야 충돌입니다.
- **수식 (X축)**: `A.max.x > B.min.x && A.min.x < B.max.x`

---

## 3. 유니티 물리 이벤트 (Unity Physics Events)
유니티 물리 엔진(PhysX)은 충돌이 감지되었을 때 스크립트로 신호를 보내줍니다.

| 구분 | "Collision" (물리 충돌) | "Trigger" (트리거/센서) |
| :--- | :--- | :--- |
| **반응** | 튕겨 나감 (물리적 실체) | 그냥 통과 (영역 감지) |
| **설정** | `Is Trigger` 체크 해제 | `Is Trigger` 체크 필수 |
| **필수 조건** | \- | 적어도 한쪽은 "**Rigidbody**"가 있어야 함 |

### 📍 주요 이벤트 메서드
```csharp
// 1. 물리적 충돌 발생 시
private void OnCollisionEnter(Collision collision) { /* 충돌 시작 */ }
private void OnCollisionStay(Collision collision) { /* 충돌 중 */ }
private void OnCollisionExit(Collision collision) { /* 충돌 종료 */ }

// 2. 트리거 영역 진입 시 (isTrigger ON)
private void OnTriggerEnter(Collider other) { /* 영역 진입 */ }
private void OnTriggerStay(Collider other) { /* 영역 내부 */ }
private void OnTriggerExit(Collider other) { /* 영역 퇴장 */ }
```

---

## 💻 실습 예제: 거리 제곱을 이용한 최적화된 충돌 체크
```csharp
using UnityEngine;

public class SimpleCollision : MonoBehaviour
{
    public Transform other;
    public float radiusA = 1.0f;
    public float radiusB = 1.0f;

    void Update()
    {
        if (other == null) return;

        // 1. 두 물체 사이의 차이 벡터 구하기
        Vector3 diff = transform.position - other.position;

        // 2. 거리의 제곱 계산 (Magnitude보다 빠름)
        float distanceSq = diff.sqrMagnitude;
        
        // 3. 반지름 합의 제곱 계산
        float radiusSumSq = Mathf.Pow(radiusA + radiusB, 2);

        // 4. 비교 (두 중심점 사이 거리가 반지름 합보다 작으면 충돌)
        if (distanceSq <= radiusSumSq)
        {
            Debug.Log("<color=red>충돌 발생!</color>");
        }
    }
}
```

---

---

## 🎯 [심화 미션] 몬스터 사냥 시스템: 타겟 충돌 감지와 회피 기동
### [요구 사항]
- 몬스터가 플레이어의 공격 범위(Trigger)에 들어왔을 때, 이를 감지하고 즉시 옆으로 회피하는 '긴급 회피' 로직을 설계하세요.
- 충격(Collision)이 발생했을 때만 체력이 깎이도록 구분하고, 트리거는 감지용으로만 사용하도록 설정하세요.
- `OnTriggerEnter`와 `OnCollisionEnter`의 용도 차이를 명확히 구분하여 적용하세요.

### [프로그래밍 힌트]
- 공격 범위는 `Is Trigger`가 체크된 콜라이더를 사용하면 물리적 밀림 없이 감지만 가능합니다.
- `transform.right` 벡터를 활용하여 좌우 회피 방향을 결정할 수 있습니다.

## ✍️ 평가 문항 대비 퀴즈
1. **문제**: 물체의 충돌 계산을 효율적으로 하기 위해 축에 정렬된 사각형 형태로 경계 영역을 잡는 방식을 무엇이라 합니까?
   - **정답**: AABB (Axis-Aligned Bounding Box)
2. **문제**: 원(구) 충돌 판정 시 성능 최적화를 위해 비교하는 두 값의 거리는 어떻게 처리하는 것이 좋습니까?
   - **정답**: 제곱근 계산을 피하기 위해 "**거리의 제곱**" 값을 사용합니다.

---

## 📎 별첨: 운동량 보존 법칙과 반발 계수 (e) 충돌 연산 (심화)

물체끼리 부딪혔을 때 서로 튕겨 나가는 강도는 **반발 계수 (Coefficient of Restitution, $e$)**와 **운동량 보존 법칙**에 의해 물리적으로 정교하게 계산됩니다.

### 1. 반발 계수 ($e$)
반발 계수는 충돌 전후 두 물체의 **상대 속도의 비율**입니다.
$$e = -\frac{v_{2f} - v_{1f}}{v_{2i} - v_{1i}}$$
* $e = 1$ (완전 탄성 충돌): 충돌 후 에너지가 완벽히 보존되어 속도가 그대로 튕겨 나갑니다. (당구공)
* $0 < e < 1$ (비탄성 충돌): 에너지 일부가 열이나 소리로 손실되며 속도가 다소 감쇠되어 튕겨 나갑니다. (현실의 대다수 충돌)
* $e = 0$ (완전 비탄성 충돌): 두 물체가 한 덩어리가 되어 달라붙어 이동합니다. (진흙 더미)

### 2. 1차원 충돌 후의 속도 유도 수식 (질량 $m_1, m_2$)
운동량 보존 법칙($m_1v_{1i} + m_2v_{2i} = m_1v_{1f} + m_2v_{2f}$)과 반발 계수 공식을 연립하면 충돌 후 최종 속도 $v_{1f}$와 $v_{2f}$를 직접 구할 수 있습니다:
$$v_{1f} = \frac{(m_1 - e \cdot m_2)v_{1i} + (1 + e)m_2v_{2i}}{m_1 + m_2}$$
$$v_{2f} = \frac{(m_2 - e \cdot m_1)v_{2i} + (1 + e)m_1v_{1i}}{m_1 + m_2}$$

> 💡 **실무적 팁**: 유니티 6 내부 Physic Material의 Bounciness 값이 바로 이 반발 계수($e$) 역할을 담당하며, 두 물체의 반발 계수가 다를 경우 설정(Average, Multiply 등)에 따라 병합 연산됩니다.

---

## 🎯 NCS 수행준거 평가 가이드

본 7일차 수업 내용은 NCS 게임 알고리즘 능력단위의 **"게임 물리 적용하기"** 및 **"게임 물리를 활용한 충돌 처리 및 물리 계산 수행"** 준거와 직접 연계됩니다.

### 1. 서술형 평가 대비 포인트
* **바운딩 볼륨 유형별 특징 비교**: AABB(축 고정), Bounding Sphere(중심 거리 측정), OBB(오브젝트 회전축 대응)의 연산 속도 및 정밀도 트레이드오프(Trade-off) 관계를 명확히 구술할 수 있어야 합니다.
* **운동량 보존 및 반발 계수**: 탄성과 비탄성 충돌 조건에 따른 속도 변화 및 운동 에너지 보존 법칙의 차이를 공식($e$)에 근거하여 논리적으로 서술해야 합니다.

### 2. 포트폴리오(실기) 평가 포인트
* `Mathf.Pow`나 제곱근 연산이 들어가는 `Vector3.Distance` 대신 연산 부하가 적은 제곱곱셈 벡터 연산(`sqrMagnitude`)을 통해 실시간 충돌 거리를 시뮬레이션하는 최적화 능력을 완벽히 검증합니다. (Unity 6의 Rigidbody/EventSystem 물리 충돌 이벤트 연계 검토)
