# 🚀 Day 09: 게임 물리 심화 (중력과 투사체 운동)

오늘의 목표는 **"중력과 공기 저항(항력)의 물리적 작용을 이해하고, 이를 결합한 운동 방정식을 기반으로 투사체(Projectile)의 실시간 포물선 궤적을 수학적으로 예측하고 구현하는 능력을 배양한다"**입니다.

---

## 1. 💡 이론 (30%): 중력과 공기 저항이 결합된 포물선 운동

### 1) 기본 포물선 운동 (공기 저항이 없을 때)
중력 가속도 $\vec{g} = (0, -g, 0)$만 작용하는 이상적인 환경에서, 초기 속도 $\vec{v}_0 = (v_{0x}, v_{0y}, v_{0z})$로 발사된 투사체의 $t$초 후 위치 $\vec{p}(t)$는 다음과 같습니다.

$$x(t) = x_0 + v_{0x}t$$
$$y(t) = y_0 + v_{0y}t - \frac{1}{2}gt^2$$

- **수평 도달 거리 ($R$)**: 발사 각도가 $\theta$일 때, $R = \frac{v_0^2 \sin(2\theta)}{g}$ 이며, $\theta = 45^\circ$일 때 최대 거리를 가집니다.
- **최고점 높이 ($H$)**: 수직 속도가 0이 되는 지점으로, $H = \frac{v_{0y}^2}{2g}$ 입니다.

---

### 2) 공기 저항(항력, Drag)의 결합
실제 물리 환경 또는 정교한 게임(예: 포트리스, 배틀필드, 저격 시뮬레이션 등)에서는 공기 저항을 고려해야 합니다.
물체에 가해지는 총합력 $\vec{F}$는 다음과 같이 정의됩니다.

$$\vec{F} = \vec{F}_g + \vec{F}_d = m\vec{g} - c\vec{v}$$

여기서 $\vec{F}_d = -c\vec{v}$는 속도에 비례하는 **선형 항력(Linear Drag)** 모델입니다. (유니티의 `Rigidbody.drag`는 이 선형 감쇠 모델을 기본으로 채택하고 있습니다.)

#### 📌 유니티의 Drag 감쇠 공식
유니티 엔진에서 매 프레임 `Rigidbody`의 속도 감쇠는 다음과 같은 수치적 근사(Euler Integration)를 따릅니다.

$$\vec{v}_{new} = \vec{v}_{old} \times (1 - d \times \Delta t) + \vec{g} \times \Delta t$$

- $d$는 `Rigidbody.drag` 계수입니다.
- $\Delta t$는 물리 업데이트 주기(`Time.fixedDeltaTime`)입니다.
- 이 공식에 의해 속도는 지속적으로 지수 감쇠(Exponential Decay)하며, 투사체의 포물선은 비대칭 형태로 찌그러지게 됩니다. (뒤로 갈수록 수평 속도가 급격히 줄어들어 수직으로 뚝 떨어짐)

---

## 2. 💻 실습 (70%): 드래그(Drag)를 반영한 실시간 궤적 예측 시스템

**미션:** 플레이어가 발사할 투사체의 강도와 방향을 조절할 때, **공기 저항(Rigidbody.drag)과 중력**을 정확히 계산하여 궤적을 **LineRenderer**로 실시간 미리 보여주고 발사하는 시스템을 구축하세요.

### 🛠️ 구현 스크립트 (`ProjectilePredictor.cs`)

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class ProjectilePredictor : MonoBehaviour
{
    [Header("발사 설정")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float launchForce = 20f;
    
    [Header("물리 시뮬레이션 예측 설정")]
    [SerializeField] private int maxSteps = 50; // 예측할 점의 개수
    [SerializeField] private float timeStep = 0.05f; // 점 사이의 시간 간격(초)
    [SerializeField] private float projectileDrag = 1.0f; // 발사체 Rigidbody의 drag 값과 일치해야 함
    
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        // 1. 발사 방향 조절 (마우스 커서 방향을 바라보도록 설정)
        AimAtMouse();

        // 2. 마우스를 누르고 있는 동안 실시간 궤적 렌더링
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            lineRenderer.enabled = true;
            PredictTrajectory();
        }
        else
        {
            lineRenderer.enabled = false;
        }

        // 3. 마우스 클릭 해제 시 발사
        if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Fire();
        }
    }

    private void AimAtMouse()
    {
        if (Camera.main == null) return;
        
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetDir = (hit.point - firePoint.position).normalized;
            // 피치(Pitch)와 요(Yaw)를 적용하여 발사대 정렬
            firePoint.forward = targetDir;
        }
    }

    /// <summary>
    /// 공기 저항(Drag)과 중력을 결합한 수치 해석 기반 실시간 궤적 예측
    /// </summary>
    private void PredictTrajectory()
    {
        Vector3 currentPosition = firePoint.position;
        Vector3 currentVelocity = firePoint.forward * launchForce;
        Vector3 gravity = Physics.gravity;
        
        lineRenderer.positionCount = maxSteps;
        lineRenderer.SetPosition(0, currentPosition);

        for (int i = 1; i < maxSteps; i++)
        {
            // 1. 공기 저항에 의한 속도 감쇠 연산 (Unity Rigidbody.drag 모사)
            // v = v * (1 - drag * dt)
            currentVelocity *= (1.0f - projectileDrag * timeStep);

            // 2. 중력에 의한 속도 변화 연산
            // v = v + g * dt
            currentVelocity += gravity * timeStep;

            // 3. 위치 업데이트
            // p = p + v * dt
            currentPosition += currentVelocity * timeStep;

            lineRenderer.SetPosition(i, currentPosition);

            // 지면(Collider)에 부딪치면 예측을 중단하여 선이 땅을 뚫고 들어가지 않게 함
            if (Physics.Raycast(lineRenderer.GetPosition(i - 1), (currentPosition - lineRenderer.GetPosition(i - 1)).normalized, out RaycastHit hit, Vector3.Distance(currentPosition, lineRenderer.GetPosition(i - 1))))
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
                break;
            }
        }
    }

    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            // 예측에 사용한 Drag 값과 반드시 일치시킵니다.
            rb.drag = projectileDrag;
            rb.useGravity = true;
            rb.AddForce(firePoint.forward * launchForce, ForceMode.VelocityChange);
        }
    }
}
```

---


