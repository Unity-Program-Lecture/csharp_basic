# 🚀 Day 09: 게임 물리 심화 (중력과 투사체 운동)

오늘의 목표는 "**중력의 원리를 응용하여 가속도 및 속도 연산을 수행하고, 이를 이용해 포물선 운동을 하는 투사체 (Projectile)를 구현한다**"입니다.

---

## 1. 💡 이론 (30%): 중력과 포물선 운동
- **중력 (Gravity)**: 유니티에서 기본적으로 Y축 방향으로 작용하는 지속적인 힘(가속도)입니다.
- **투사체 (Projectile)**: 포탄, 화살처럼 허공에 던져진 물체를 말합니다.
- **포물선 연산**: 초기 속도(Velocity)를 주면 물체는 앞으로 나아가려 하지만, 중력(Gravity)이 계속 아래로 당기기 때문에 곡선(포물선)을 그리며 떨어지게 됩니다.

---

## 2. 💻 실습 (70%): 대포알 쏘기 (투사체 구현)
**미션:** 유니티의 Rigidbody와 벡터 연산을 활용하여, 마우스 클릭 시 목표 지점을 향해 포물선으로 날아가는 대포알을 발사하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem; // 최신 인풋 시스템

public class Cannon : MonoBehaviour
{
    public GameObject cannonballPrefab;
    public Transform firePoint;
    public float fireForce = 15f;

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 시 발사 (Input System 방식)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Fire();
        }
    }

    void Fire()
    {
        GameObject ball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);
    }
}
```

</details>

---

## 🎯 [심화 미션] 몬스터 사냥 시스템: 곡사 화기 투척
### [요구 사항]
- 멀리 있는 플레이어를 공격하기 위해 몬스터가 '화염병'을 곡사 형태로 투척하는 시스템을 기획하세요.
- 단순히 앞으로 던지는 것이 아니라, 플레이어의 현재 위치까지 도달하기 위한 최적의 발사 각도와 초기 속도를 계산하는 로직을 구상해 보세요.
- 포물선 운동의 수평 거리($R$) 공식과 최고점 높이($H$) 공식을 어떻게 활용할지 생각해 보세요.

### [프로그래밍 힌트]
- `AddForce` 시 `up` 벡터와 `forward` 벡터를 적절히 조합하여 발사 각도를 만들 수 있습니다.
- 공기 저항(`drag`)이 있는 경우와 없는 경우의 궤적 차이를 시뮬레이션해 보세요.

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 대포알과 같이 허공에 던져진 물체가 중력의 영향을 받아 그리는 궤적을 어떤 운동이라고 합니까?
   - **정답:** 포물선 운동
2. **문제:** 유니티 `AddForce`에서 폭발이나 대포 발사처럼 '순간적인 충격'을 가할 때 사용하는 `ForceMode`는 무엇인가요?
   - **정답:** `ForceMode.Impulse`
