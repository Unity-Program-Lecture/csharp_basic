# 🚀 Day 08: 게임 물리 - 심화 연산 (Impulse & Raycast)

오늘의 목표는 "**순간적인 힘(충충격량)의 전달과 물리적 가시성 검사(Raycast)의 원리를 이해하고 게임 실무에 적용한다**"입니다.

---

## 1. 운동량과 충격량 (Momentum & Impulse)
뉴턴 물리에서 물체의 운동 상태 변화를 다루는 핵심 개념입니다.

- **운동량 (P)**: $P = m \times v$ (질량 x 속도). 물체가 가진 운동의 세기입니다.
- **충격량 (Impulse)**: 운동량의 변화량입니다. ($F \times \Delta t$)
- **유니티 적용**: `Rigidbody.AddForce(direction, ForceMode.Impulse)`를 사용하면 폭발이나 타격처럼 한 프레임에 모든 힘을 즉시 전달합니다.

---

## 2. 레이캐스트 (Raycast): "수학적 화살 쏘기"
공간상의 한 점(Origin)에서 특정 방향(Direction)으로 보이지 않는 선을 쏘아 물체와 닿는지 확인하는 기술입니다.

- **원리**: 직선 방정식과 물체의 도형 방정식(상자, 구 등) 사이의 해를 구하는 수학적 과정입니다.
- **용도**: 총기 발사, 바닥 체크, 시야 판별 등.

---

## 💻 실습 예제: 레이캐스트를 이용한 바닥 감지 점프
```csharp
using UnityEngine;
using UnityEngine.InputSystem; // 최신 인풋 시스템

public class RaycastJump : MonoBehaviour
{
    private Rigidbody rb;
    public float jumpForce = 5f;
    public float checkDistance = 0.6f;

    void Start() { rb = GetComponent<Rigidbody>(); }

    void Update()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, checkDistance);

        Debug.DrawRay(transform.position, Vector3.down * checkDistance, isGrounded ? Color.green : Color.red);

        // Input System: 스페이스바를 누른 순간 확인
        if (isGrounded && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
```

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 유니티 `AddForce` 모드 중, 폭발이나 타격처럼 짧은 순간에 급격한 속도 변화를 주고 싶을 때 사용하는 모드는?
   - **정답:** `ForceMode.Impulse`
2. **문제:** 시작점에서 특정 방향으로 선을 쏘아 충돌 여부를 판단하는 물리 기술의 명칭은?
   - **정답:** 레이캐스트 (Raycast)
