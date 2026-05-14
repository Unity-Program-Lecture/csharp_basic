# 🚀 Day 07: 게임 물리 - 충돌 감지 기초 (Bounding Box & Sphere)

오늘의 목표는 "**물체 간의 겹침을 수학적으로 판별하는 충돌 알고리즘의 원리를 이해하고, 가장 효율적인 바운딩 볼륨 방식을 구현한다**"입니다.

---

## 1. 충돌 판별의 원리: "경계 영역(Bounding Volume)"
모든 물체의 정교한 표면을 실시간으로 계산하는 것은 매우 무겁습니다. 따라서 단순한 형태의 '상자'나 '구'로 감싸서 먼저 계산합니다.

### 📍 대표적인 충돌 영역 방식
1. **AABB (Axis-Aligned Bounding Box)**: 축에 나란한 사각형. 계산이 가장 빠름.
2. **OBB (Oriented Bounding Box)**: 물체의 회전에 맞춰 기울어진 사각형. 정확하지만 계산이 더 복잡함.
3. **Bounding Sphere**: 구 형태. 중심점 사이의 거리만 재면 되므로 매우 효율적임.

---

## 2. 알고리즘 구현 원리

### 📍 원 충돌 (Circle/Sphere Collision)
- **공식**: 두 구의 중심 사이의 거리 < (반지름 A + 반지름 B) 이면 충돌!
- **최적화 팁**: 루트(`sqrt`) 계산은 무거우므로 **거리의 제곱**과 **반지름 합의 제곱**을 비교합니다.

### 📍 AABB 충돌 (Rectangle Collision)
- 각 축(X, Y, Z)에 대해 범위가 겹치는지 확인합니다.
- `(A.max.x > B.min.x && A.min.x < B.max.x)`와 같이 판별합니다.

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

        // 4. 비교
        if (distanceSq <= radiusSumSq)
        {
            Debug.Log("<color=red>충돌 발생!</color>");
        }
    }
}
```

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 물체의 충돌 계산을 효율적으로 하기 위해 축에 정렬된 사각형 형태로 경계 영역을 잡는 방식을 무엇이라 합니까?
   - **정답:** AABB (Axis-Aligned Bounding Box)
2. **문제:** 원(구) 충돌 판정 시 성능 최적화를 위해 비교하는 두 값의 거리는 어떻게 처리하는 것이 좋습니까?
   - **정답:** 제곱근 계산을 피하기 위해 **거리의 제곱** 값을 사용합니다.
