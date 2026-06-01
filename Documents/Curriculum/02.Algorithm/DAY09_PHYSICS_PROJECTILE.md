# Day 09: 게임 물리 심화 - 중력과 투사체 운동

오늘의 목표는 "**공을 던졌을 때 앞으로 가면서 아래로 떨어지는 움직임을 이해하고, Unity Scene 뷰에서 투사체 궤적을 직접 눈으로 확인하는 것**"입니다.

투사체 운동은 포탄, 화살, 수류탄, 점프 궤적처럼 "처음에는 앞으로 나아가지만 시간이 지나면 중력 때문에 아래로 휘는 움직임"입니다.

## 1. 핵심 개념: "앞으로 가는 힘과 아래로 당기는 힘"

공을 앞으로 던지면 공은 두 가지 움직임을 동시에 합니다.

- 앞으로 계속 이동한다.
- 중력 때문에 아래로 점점 빨라지며 떨어진다.

그래서 공의 길은 직선이 아니라 휘어진 곡선이 됩니다. 이 곡선을 `Trajectory`라고 합니다.

Unity에서는 보통 XZ 평면이 바닥이고, Y축이 높이입니다. 투사체는 XZ 방향으로 앞으로 나아가면서, Y 방향으로는 `Physics.gravity`의 영향을 받아 아래로 내려갑니다.

### 이 단어는 무슨 뜻인가요?

#### Projectile

던지거나 발사되어 날아가는 물체입니다. 포탄, 화살, 공, 수류탄이 모두 투사체가 될 수 있습니다.

#### Trajectory

투사체가 지나가는 길입니다. 한국어로는 궤적이라고 부릅니다.

#### Initial Velocity

발사 순간의 속도입니다. 방향과 빠르기를 함께 가집니다.

#### Gravity

물체를 아래로 끌어당기는 가속도입니다. Unity에서는 `Physics.gravity`로 현재 프로젝트의 중력 값을 가져올 수 있습니다.

#### Linear Damping

공기 저항처럼 물체의 직선 속도를 줄이는 감쇠입니다. Unity 6에서는 코드에서 `Rigidbody.linearDamping`으로 접근하고, Inspector에는 **Linear Damping**으로 표시됩니다. 오늘 문서에서는 기본 궤적을 먼저 보고, Linear Damping은 선택 개념으로만 다룹니다.

## 2. 투사체 위치 계산

투사체의 위치는 "현재 위치에 속도를 더하고, 속도에는 중력을 더한다"는 방식으로 조금씩 예측할 수 있습니다.

```text
다음 속도 = 현재 속도 + 중력 * 시간 간격
다음 위치 = 현재 위치 + 다음 속도 * 시간 간격
```

이 방식은 작은 시간 간격마다 위치를 한 점씩 찍어보는 방법입니다. 수식 하나로 답을 바로 구하는 대신, 게임 프레임처럼 조금씩 앞으로 나아가며 계산하므로 코드로 이해하기 쉽습니다.

예측이 살펴보는 전체 시간은 대략 `maxSteps * timeStep`입니다. 예를 들어 `maxSteps`가 40이고 `timeStep`이 0.08이면 약 3.2초 뒤까지의 궤적을 미리 보는 셈입니다. 다만 `maxSteps`는 바닥에 반드시 닿게 만들기 위한 값이라기보다, 예측 계산이 끝없이 이어지지 않도록 막는 안전장치입니다.

Linear Damping을 포함하고 싶다면 속도를 업데이트하기 전에 속도를 조금 줄이면 됩니다.

```text
현재 속도 = 현재 속도 * (1 - Linear Damping * 시간 간격)
```

Linear Damping 값이 커질수록 궤적은 멀리 뻗지 못하고 더 빨리 아래로 떨어집니다.

## 실습 예제: OnDrawGizmos로 투사체 궤적 보기

**미션:** 발사 위치, 발사 속도, 발사 각도를 바꿔가며 Scene 뷰에서 투사체 궤적이 어떻게 변하는지 관찰합니다.

### 준비하기

1. Unity 씬에 빈 오브젝트 `ProjectilePreview`를 만듭니다.
2. `ProjectilePreview`에 아래 스크립트를 붙입니다.
3. 바닥 역할을 할 Plane이나 Cube를 만들고 Collider가 켜져 있는지 확인합니다.
4. 실제 발사를 비교하고 싶다면 Rigidbody가 붙은 투사체 프리팹을 만들고 `Projectile Prefab`에 연결합니다.
5. Scene 뷰 오른쪽 위의 `Gizmos` 버튼을 켭니다.
6. Play 모드에서 방향키와 `Q`, `E` 키로 발사 방향과 각도를 조절합니다.
7. `Space` 키를 누르면 연결된 투사체 프리팹이 실제로 발사됩니다.

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileGizmoPreview : MonoBehaviour
{
    [Header("Launch")]
    [Tooltip("Space 키를 눌렀을 때 실제로 생성할 투사체 프리팹입니다. 비워 두면 예측선만 표시합니다.")]
    [SerializeField] private GameObject projectilePrefab;

    [Tooltip("투사체가 발사되는 속도입니다. 값이 클수록 더 멀리 날아갑니다.")]
    [SerializeField] private float launchSpeed = 12f;

    [Tooltip("투사체를 위로 들어 올리는 발사 각도입니다.")]
    [SerializeField] private float launchAngle = 35f;

    [Tooltip("투사체가 좌우로 향하는 방향 각도입니다.")]
    [SerializeField] private float yawAngle = 0f;

    [Header("Prediction")]
    [Tooltip("궤적을 예측할 때 찍을 점의 개수입니다.")]
    [SerializeField] private int maxSteps = 40;

    [Tooltip("예측 점 사이의 시간 간격입니다. 작을수록 더 촘촘하지만 계산이 늘어납니다.")]
    [SerializeField] private float timeStep = 0.08f;

    [Tooltip("Unity 6 Rigidbody의 Linear Damping에 대응하는 예측용 감쇠 값입니다.")]
    [SerializeField] private float linearDamping = 0f;

    private Vector3 hitPoint;
    private Vector3 lastPredictedPoint;
    private bool hasHit;

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        // leftArrowKey와 rightArrowKey는 현재 키보드의 방향키 입력을 읽는 Input System 프로퍼티입니다.
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            yawAngle -= 60f * Time.deltaTime;
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            yawAngle += 60f * Time.deltaTime;
        }

        // Q/E 키로 발사 각도를 낮추거나 높입니다.
        if (Keyboard.current.qKey.isPressed)
        {
            launchAngle -= 40f * Time.deltaTime;
        }

        if (Keyboard.current.eKey.isPressed)
        {
            launchAngle += 40f * Time.deltaTime;
        }

        // Mathf.Clamp는 값을 지정한 최소/최대 범위 안에 가두는 메서드입니다.
        launchAngle = Mathf.Clamp(launchAngle, 5f, 80f);

        // Space 키를 누른 순간 실제 투사체를 발사합니다.
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FireProjectile();
        }
    }

    private Vector3 GetLaunchVelocity()
    {
        // Quaternion.Euler는 각도 값을 회전으로 바꾸는 메서드입니다.
        Quaternion rotation = Quaternion.Euler(-launchAngle, yawAngle, 0f);

        // Vector3.forward는 월드 기준 앞 방향 벡터입니다.
        return rotation * Vector3.forward * launchSpeed;
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null)
        {
            return;
        }

        // Instantiate는 프리팹을 씬에 복제하여 새 GameObject를 만드는 메서드입니다.
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody body = projectile.GetComponent<Rigidbody>();

        if (body == null)
        {
            return;
        }

        // Unity 6에서 linearDamping은 Inspector의 Linear Damping 값과 대응합니다.
        body.linearDamping = linearDamping;
        body.useGravity = true;
        body.linearVelocity = GetLaunchVelocity();
    }

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Vector3 velocity = GetLaunchVelocity();

        hasHit = false;
        lastPredictedPoint = position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, 0.15f);

        for (int i = 0; i < maxSteps; i++)
        {
            Vector3 previousPosition = position;

            // linearDamping이 0보다 크면 속도가 조금씩 줄어듭니다.
            velocity *= 1f - linearDamping * timeStep;

            // Physics.gravity는 현재 프로젝트에 설정된 중력 벡터입니다.
            velocity += Physics.gravity * timeStep;
            position += velocity * timeStep;

            Vector3 move = position - previousPosition;
            float distance = move.magnitude;

            // Physics.Raycast는 이전 점에서 다음 점 방향으로 선을 쏴 Collider와 닿는지 검사합니다.
            if (Physics.Raycast(previousPosition, move.normalized, out RaycastHit hit, distance))
            {
                hasHit = true;
                hitPoint = hit.point;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(previousPosition, hit.point);
                Gizmos.DrawWireSphere(hit.point, 0.25f);
                break;
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(previousPosition, position);
            Gizmos.DrawWireSphere(position, 0.05f);
            lastPredictedPoint = position;
        }

        if (!hasHit)
        {
            // maxSteps 안에서 Collider를 만나지 못했다면 마지막 예측 지점을 표시합니다.
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(lastPredictedPoint, 0.25f);
            return;
        }

        // 착탄 지점을 한 번 더 크게 표시합니다.
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPoint, 0.08f);
    }
}
```

### 실행해보면

Scene 뷰에 하늘색 곡선이 표시됩니다. 이 곡선이 현재 설정으로 발사했을 때 투사체가 지나갈 예상 경로입니다.

`E` 키를 누르면 발사 각도가 높아져 궤적이 위로 솟습니다. `Q` 키를 누르면 발사 각도가 낮아져 궤적이 평평해집니다.

왼쪽/오른쪽 방향키를 누르면 발사 방향이 좌우로 돌아갑니다. 궤적이 Collider에 닿으면 그 지점이 빨간색으로 표시됩니다.

`maxSteps` 안에서 Collider에 닿지 않으면 마지막 예측 지점이 보라색으로 표시됩니다. 이 경우는 충돌이 없는 것이 아니라, 안전장치로 정한 계산 횟수 안에서는 아직 바닥이나 장애물에 닿지 않았다는 뜻입니다.

`launchSpeed`를 키우면 더 멀리 날아가고, `linearDamping`을 키우면 속도가 빨리 줄어들어 궤적이 짧아집니다.

`Projectile Prefab`에 Rigidbody가 붙은 프리팹을 연결하고 `Space` 키를 누르면 실제 투사체가 발사됩니다. 예측선과 실제 투사체가 비슷한 경로로 움직이는지 비교할 수 있습니다.

### 생각해보기

1. 같은 `launchSpeed`일 때 발사 각도를 높이면 항상 더 멀리 날아갈까요?
2. `linearDamping` 값을 키우면 궤적의 어느 부분이 가장 눈에 띄게 달라질까요?
3. `maxSteps`는 왜 바닥에 닿을 때까지 무한히 계산하지 않고 정해진 횟수까지만 반복할까요?
4. 수류탄 예상 궤적과 화살 예상 궤적은 같은 방식으로 보여줘도 될까요?

## 선택 미션: 예측선과 실제 발사 비교하기

Gizmos로 보이는 선은 "예측"입니다. 실제 투사체의 `Rigidbody`가 움직이는 길과 비교하면 예측이 맞는지 확인할 수 있습니다.

비교할 때는 다음 조건을 맞춥니다.

- 예측에 쓰는 `launchSpeed`와 실제 발사 힘을 같은 기준으로 맞춥니다.
- 예측에 쓰는 `linearDamping`과 실제 투사체 `Rigidbody`의 **Linear Damping** 값을 같은 값으로 맞춥니다.
- 실제 투사체의 `Rigidbody.useGravity`가 켜져 있는지 확인합니다.

예측선과 실제 이동 경로가 다르면, 보통 시간 간격(`timeStep`), Linear Damping 값, 발사 방향, Rigidbody 설정 중 하나가 다릅니다.

## 오늘의 정리

- 투사체는 앞으로 이동하면서 중력 때문에 아래로 떨어진다.
- 궤적은 투사체가 지나가는 길이다.
- 작은 시간 간격마다 속도와 위치를 갱신하면 곡선을 예측할 수 있다.
- `OnDrawGizmos`를 사용하면 Console 로그보다 Scene 뷰에서 움직임을 직관적으로 확인할 수 있다.
- Linear Damping을 넣으면 속도가 줄어들어 궤적이 짧고 가파르게 변한다.
