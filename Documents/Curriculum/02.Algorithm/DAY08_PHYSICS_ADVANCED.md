# 🚀 Day 08: 게임 물리 - 심화 연산 (Impulse & Raycast)

오늘의 목표는 "**순간적인 힘 (충격량)의 전달과 물리적 가시성 검사 (Raycast)의 원리를 이해하고 게임 실무에 적용한다**"입니다.

---

## 1. 운동량과 충격량 (Momentum & Impulse)
뉴턴 물리에서 물체의 운동 상태 변화를 다루는 핵심 개념입니다.

- **운동량 (P)**: $P = m \times v$ (질량 x 속도). 물체가 가진 운동의 세기입니다.
- **충격량 (Impulse)**: 운동량의 변화량입니다. ($F \times \Delta t$)
- **유니티 적용**: `Rigidbody.AddForce(direction, ForceMode.Impulse)`를 사용하면 폭발이나 타격처럼 한 프레임에 모든 힘을 즉시 전달합니다.

### ForceMode에 따른 힘 전달 방식
`Rigidbody.AddForce(direction, mode)`에서 `direction`은 힘을 줄 방향과 크기이고, `mode`는 그 힘을 물리 엔진이 어떻게 해석할지 정하는 옵션입니다. 같은 `direction` 값을 넣어도 `ForceMode`에 따라 "계속 밀기"가 될 수도 있고, "순간적으로 튕기기"가 될 수도 있습니다.

| ForceMode | 힘이 전달되는 방식 | 질량 영향 | 자주 쓰는 호출 패턴 | 사용 예시 |
| --- | --- | --- | --- | --- |
| `ForceMode.Force` | 매 물리 프레임마다 힘을 계속 누적해서 전달합니다. | 받음 | `FixedUpdate`에서 일정 기간 반복 호출 | 로켓 추진, 바람, 계속 미는 힘 |
| `ForceMode.Acceleration` | 질량을 무시하고 일정한 가속도를 계속 전달합니다. | 받지 않음 | `FixedUpdate`에서 일정 기간 반복 호출 | 캐릭터 이동 보정, 동일한 가속도 적용 |
| `ForceMode.Impulse` | 짧은 순간에 충격량을 한 번 전달합니다. | 받음 | 점프/피격/폭발 이벤트 순간에 단발 호출 | 점프, 폭발, 피격 넉백 |
| `ForceMode.VelocityChange` | 질량을 무시하고 속도 변화량을 즉시 더합니다. | 받지 않음 | 대시/보정 이벤트 순간에 단발 호출 | 대시, 순간 이동 보정, 일정한 점프감 |

초보자 관점에서는 이렇게 구분하면 쉽습니다.

- `Force`와 `Acceleration`은 손으로 계속 밀고 있는 느낌입니다.
- `Impulse`와 `VelocityChange`는 한 번 툭 치는 느낌입니다.
- `Force`와 `Impulse`는 질량이 큰 물체일수록 덜 움직입니다.
- `Acceleration`과 `VelocityChange`는 질량이 달라도 같은 정도로 움직입니다.
- 일반적으로 지속적인 힘은 `FixedUpdate`에서 여러 번 호출하고, 순간적인 힘은 입력이나 충돌 같은 이벤트가 발생한 프레임에 한 번만 호출합니다.

따라서 `rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse)`는 "위쪽으로 한 번 강하게 밀어 올리는 충격"입니다. 캐릭터의 `Rigidbody.mass`가 커지면 같은 `jumpForce`라도 더 낮게 점프합니다.

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
        // Physics.Raycast는 시작점에서 방향으로 보이지 않는 선을 쏴 충돌 여부를 검사하는 메서드입니다.
        // Vector3.down은 월드 기준 아래 방향인 (0, -1, 0) 벡터입니다.
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, checkDistance);

        // Debug.DrawRay는 씬 뷰에 디버그용 광선을 그려 Raycast 방향을 눈으로 확인하게 해 줍니다.
        Debug.DrawRay(transform.position, Vector3.down * checkDistance, isGrounded ? Color.green : Color.red);

        // Input System: 스페이스바를 누른 순간 확인
        // wasPressedThisFrame은 해당 키가 이번 프레임에 막 눌렸을 때만 true가 되는 프로퍼티입니다.
        if (isGrounded && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Vector3.up은 월드 기준 위 방향인 (0, 1, 0) 벡터입니다.
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
```

---

## 🎯 [심화 미션] 몬스터 사냥 시스템: 정밀 저격과 부위 파괴
### [요구 사항]
- 플레이어가 몬스터의 특정 부위(머리, 다리 등)를 조준하여 사격했을 때, 부딪힌 위치를 정확히 판별하는 시스템을 기획하세요.
- 머리에 맞으면 데미지가 2배, 다리에 맞으면 몬스터의 이동 속도가 느려지도록 로직을 구성해 보세요.
- `LayerMask`를 활용하여 몬스터만 조준하고 장애물은 무시하는 최적화 방법을 구상하세요.

### [프로그래밍 힌트]
- `RaycastHit.collider.tag` 또는 레이어 정보를 사용하여 부위별 판정을 할 수 있습니다.
- 몬스터의 자식 오브젝트마다 서로 다른 태그를 가진 콜라이더를 배치해 보세요.

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 유니티 `AddForce` 모드 중, 폭발이나 타격처럼 짧은 순간에 급격한 속도 변화를 주고 싶을 때 사용하는 모드는?
   - **정답:** `ForceMode.Impulse`
2. **문제:** 시작점에서 특정 방향으로 선을 쏘아 충돌 여부를 판단하는 물리 기술의 명칭은?
   - **정답:** 레이캐스트 (Raycast)
