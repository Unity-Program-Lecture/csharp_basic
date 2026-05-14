# 🚀 Day 08: 엔진 물리 시스템 (Physics & Rigidbodies)

오늘의 목표는 "**유니티 물리 엔진의 핵심 구성 요소인 Rigidbody와 Collider의 상호작용을 이해하고, 실무적인 물리 감지 로직을 구현한다**"입니다.

---

## 1. 리지드바디 (Rigidbody)
오브젝트가 유니티 물리 엔진의 통제를 받게 만드는 컴포넌트입니다.
- **Mass**: 질량.
- **Drag**: 공기 저항.
- **Use Gravity**: 중력 적용 여부.
- **Is Kinematic**: 체크하면 물리 연산(힘)을 무시하고 스크립트로만 이동시킵니다.

---

## 2. 콜라이더 (Collider): "물리적 피부"
물체의 충돌 범위를 결정합니다.
- **Is Trigger**: 체크하면 물리적인 충돌(튕겨 나감)은 없지만, 겹침 이벤트(`OnTriggerEnter`)를 감지할 수 있습니다. (센서 용도)

---

## 💻 실습 예제: 트리거(Trigger)를 이용한 아이템 획득 시스템
```csharp
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // 콜라이더의 Is Trigger가 체크되어 있어야 호출됨
    private void OnTriggerEnter(Collider other)
    {
        // 1. 충돌한 대상이 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("<color=green>아이템을 획득했습니다!</color>");
            
            // 2. 아이템 오브젝트 파괴 (또는 풀로 반환)
            Destroy(gameObject);
        }
    }
}
```

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 유니티에서 물리적인 반작용(튕겨나감) 없이 물체가 겹쳐진 순간만을 감지하고 싶을 때 Collider에서 설정해야 하는 옵션은?
   - **정답:** Is Trigger
2. **문제:** 오브젝트에 중력이나 마찰력 같은 물리 법칙을 적용하기 위해 필수적으로 추가해야 하는 컴포넌트는?
   - **정답:** 리지드바디 (Rigidbody)
