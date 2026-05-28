# 🚀 Day 04: 게임 수학 - 회전과 사원수 (Quaternion & Slerp)

오늘의 목표는 "**오일러 각도의 한계를 극복하는 쿼터니언 (사원수)의 개념을 이해하고, 부동 소수점 오차 없는 부드러운 회전을 구현한다**"입니다.

---

## 1. 오일러 각도 (Euler Angles)와 짐벌락 (Gimbal Lock)
우리가 흔히 쓰는 (x, y, z) 각도 표현은 직관적이지만 치명적인 단점이 있습니다.

- **오일러 각도**: 세 축을 순서대로 회전시키는 방식.
- **짐벌락 현상**: 두 회전축이 겹쳐지면서 한 축의 자유도를 상실하는 현상입니다. (예: 위를 쳐다볼 때 좌우 회전이 꼬이는 경우)

---

## 2. 쿼터니언 (Quaternion): "**4차원 복소수 회전**"
유니티는 내부적으로 회전을 처리할 때 4개의 성분(x, y, z, w)을 가진 쿼터니언을 사용합니다.

### 📍 쿼터니언의 장점
1. **짐벌락이 없습니다.**
2. **회전 보간** (Interpolation)이 매우 부드럽고 정확합니다.
3. 행렬보다 메모리를 적게 사용하고 연산 속도가 빠릅니다.

---

### 💡 실생활 비유로 단박에 이해하는 회전의 차이
![오일러와 쿼터니언 회전 및 짐벌락 비교 다이어그램](Images/rotation_euler_vs_quaternion.svg)

1. **오일러 각도는 "3단 꺾임 스마트폰 거치대" 입니다.**
   - 상하, 좌우, 기울임용 관절 쇠막대가 하나씩 연결되어 있어, 폰이 하늘을 보게 90도로 완전히 꺾어버리는 순간, 원래 좌우로 돌리는 축과 기울임축이 일직선으로 포개져서 좌우 조작이 먹통이 되는 **짐벌락** (Gimbal Lock) 이 발생합니다.
2. **쿼터니언은 "손바닥 위에 얹은 둥근 농구공 (트랙볼)" 입니다.**
   - 애초에 엉킬 관절 자체가 없는 완벽한 원형 구체이므로, 원하는 방향으로 손끝 하나로 쓱 굴려버리면 단 한 번의 움직임으로 꼬임 없이 부드럽게 회전합니다.

### 🎮 실전 FPS 게임의 꼼수: "왜 카메라 상하 각도는 89도에서 멈출까?"
- FPS 게임에서 마우스를 끝까지 올려 하늘을 똑바로 쳐다보는 순간 (Pitch 90도), 카메라 좌우 축과 기울임 축이 겹치는 짐벌락이 일어납니다. 이 상태에서 마우스를 돌리면 화면이 제자리에서 팽이처럼 빙글빙글 돌고 조작이 튀게 됩니다.
- 전 세계 모든 FPS 게임 개발자들은 이 현상을 방지하기 위해, 카메라 상하 각도가 정확히 90도에 도달하기 전인 **`89도 ~ 89.9도`** 에서 강제로 멈추도록 **클램프** (Clamp) 제약을 걸어두는 정교한 우회 전략을 사용합니다!

---

### 🔄 선형 보간 (Lerp) 과 구면 선형 보간 (Slerp) 의 차이
- **Lerp (선형 보간 - Linear Interpolation):** "**두 지점을 잇는 최단 직선 터널**"
  - A에서 B로 갈 때 둥근 곡면을 무시하고 **지구 내부를 칼로 자르듯 뚫고 지나가는 직선**으로 보간합니다.
  - 회전에 잘못 적용하면 회전 도중 사물 크기가 쪼그라들었다 펴지는 왜곡이 생기고, 각속도 (회전 속도) 가 중간에서 빨라졌다 끝에서 느려지는 등 불안정해집니다.
- **Slerp (구면 선형 보간 - Spherical Linear Interpolation):** "**지구본 표면을 따라 날아가는 비행기 항로**"
  - 구의 둥근 표면을 따라 **최단 경로로 미끄러지듯 활공하며** 보간합니다.
  - 회전 반경이 항상 일정하게 유지되어 크기 왜곡이 원천 차단되며, 처음부터 끝까지 "**완벽하게 동일한 속도**" (등속 회전) 로 매끄럽게 회전합니다.

---

## 💻 실습 예제: 입력으로 목표점을 움직이며 Slerp 체감하기
키보드로 노란 목표점을 움직이면 오브젝트가 그 방향을 향해 부드럽게 돌아갑니다. Scene 뷰에서는 **파란 선**이 현재 앞 방향, **노란 점**이 바라봐야 할 목표점, **빨간 선**이 목표 방향입니다.

### 준비
1. 빈 GameObject를 만들고 이름을 `QuaternionPractice`로 바꿉니다.
2. 아래 스크립트를 붙입니다.
3. Unity가 Input System을 사용하도록 설정되어 있어야 합니다. `Project Settings > Player > Active Input Handling`이 `Input System Package (New)` 또는 `Both`인지 확인합니다.
4. Play Mode에서 `W/A/S/D` 또는 방향키로 목표점을 움직이고, `Space`로 목표점을 가운데로 되돌립니다.

```csharp
using UnityEngine;
// UnityEngine.InputSystem은 Unity 6 기준 새 입력 시스템의 키보드, 마우스, 게임패드 입력을 사용하기 위한 네임스페이스입니다.
using UnityEngine.InputSystem;

public class QuaternionInputGizmoPractice : MonoBehaviour
{
    public float rotationSpeed = 4f;
    public float targetMoveSpeed = 3f;
    public float targetDistance = 4f;
    public float targetRange = 3f;

    Vector3 targetOffset = new Vector3(0f, 0f, 4f);

    void Update()
    {
        // Keyboard.current는 Input System에서 현재 키보드 장치를 가져오는 프로퍼티입니다.
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        // Vector2.zero는 (0, 0)을 뜻하는 2D 벡터 기본값입니다.
        Vector2 input = Vector2.zero;

        // aKey, leftArrowKey는 각각 A 키와 왼쪽 방향키를 나타내는 입력 버튼입니다.
        // isPressed는 해당 키가 지금 눌려 있는 동안 true가 되는 프로퍼티입니다.
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            input.x -= 1f;
        }

        // dKey, rightArrowKey는 각각 D 키와 오른쪽 방향키를 나타냅니다.
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            input.x += 1f;
        }

        // sKey, downArrowKey는 각각 S 키와 아래 방향키를 나타냅니다.
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            input.y -= 1f;
        }

        // wKey, upArrowKey는 각각 W 키와 위 방향키를 나타냅니다.
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            input.y += 1f;
        }

        // spaceKey는 스페이스바 입력 버튼입니다.
        // wasPressedThisFrame은 이번 프레임에 막 눌렸을 때만 true가 되는 프로퍼티입니다.
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            targetOffset = new Vector3(0f, 0f, targetDistance);
        }

        // Time.deltaTime은 이전 프레임에서 현재 프레임까지 걸린 시간입니다. 프레임 속도가 달라도 이동 속도를 일정하게 맞출 때 사용합니다.
        targetOffset += new Vector3(input.x, input.y, 0f) * targetMoveSpeed * Time.deltaTime;
        // Mathf.Clamp는 값을 최소값과 최대값 사이로 제한하는 메서드입니다.
        targetOffset.x = Mathf.Clamp(targetOffset.x, -targetRange, targetRange);
        targetOffset.y = Mathf.Clamp(targetOffset.y, -targetRange, targetRange);
        targetOffset.z = targetDistance;

        // normalized는 벡터의 방향은 유지하고 길이만 1로 만든 값을 돌려주는 프로퍼티입니다.
        Vector3 targetDirection = targetOffset.normalized;

        // Quaternion.LookRotation은 지정한 방향을 바라보는 회전값을 만들어 주는 메서드입니다.
        // Vector3.up은 월드 기준 위쪽 방향인 (0, 1, 0)을 뜻합니다.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        // transform.rotation은 현재 오브젝트의 회전값이고, Quaternion.Slerp는 두 회전 사이를 부드럽게 섞습니다.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        // transform.position은 현재 오브젝트의 월드 위치입니다.
        Vector3 origin = transform.position;
        Vector3 targetPosition = origin + targetOffset;
        Vector3 targetDirection = targetOffset.normalized;

        // Gizmos.color는 이후에 그릴 기즈모 도형의 색상을 지정하는 프로퍼티입니다.
        // Color.yellow는 유니티가 미리 제공하는 노란색 값입니다.
        Gizmos.color = Color.yellow;
        // Gizmos.DrawSphere는 Scene 뷰에 구체를 그려 특정 위치를 표시하는 메서드입니다.
        Gizmos.DrawSphere(targetPosition, 0.15f);

        // Color.blue는 유니티가 미리 제공하는 파란색 값입니다.
        Gizmos.color = Color.blue;
        // Gizmos.DrawLine은 Scene 뷰에 두 점을 잇는 선을 그리는 메서드입니다.
        // transform.forward는 현재 오브젝트가 바라보는 앞 방향입니다.
        Gizmos.DrawLine(origin, origin + transform.forward * targetDistance);

        // Color.red는 유니티가 미리 제공하는 빨간색 값입니다.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + targetDirection * targetDistance);
    }
}
```

### 관찰 포인트
- `rotationSpeed`를 낮추면 목표 방향을 천천히 따라가고, 높이면 빠르게 따라갑니다.
- `Slerp`는 현재 회전에서 목표 회전까지 한 번에 꺾지 않고, 둥근 공 표면을 미끄러지듯 부드럽게 이동합니다.
- 목표점이 위아래로 움직여도 `Quaternion.LookRotation(targetDirection, Vector3.up)` 덕분에 앞 방향과 위 방향 기준이 함께 잡혀 회전이 안정적으로 유지됩니다.

---

## 🎯 [심화 미션] 몬스터 사냥 시스템: 짐벌락 없는 정밀한 조준
### [요구 사항]
- 하늘을 나는 비행 몬스터가 지상의 플레이어를 조준할 때, 수직 방향 (Pitch)으로 90도 근처에서도 회전 오류 없이 부드럽게 바라보는 시스템을 구상하세요.
- 단순히 방향만 보는 것이 아니라, 몬스터의 날개 수평 (Roll)을 지면과 평행하게 유지하면서 조준해야 합니다.

### [프로그래밍 힌트]
- `Quaternion.LookRotation`의 두 번째 매개변수 (Up 벡터)를 활용하여 수평을 제어할 수 있습니다.
- `Quaternion.Slerp`를 사용하여 회전 속도를 조절해 보세요.

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 오일러 각도 방식으로 회전할 때 두 축이 겹쳐 회전이 불가능해지는 현상을 무엇이라 합니까?
   - **정답:** 짐벌락 (Gimbal Lock)
2. **문제:** 두 회전값 사이를 최단 경로로 부드럽게 연결해 주는 보간 함수의 이름은?
   - **정답:** Slerp (구면 선형 보간)

---

## 📎 별첨: 사원수의 수학적 원리와 회전 계산 (심화)

사원수 (Quaternion)는 1843년 윌리엄 로언 해밀턴이 발견한 수 체계로, 3차원 공간상의 회전을 표현하는 가장 효율적인 도구입니다.

### 1. 사원수의 정의
사원수는 하나의 실수부와 세 개의 허수부로 구성된 4차원 복소수입니다.
- **수식:** `q = w + xi + yj + zk` (단, `i^2 = j^2 = k^2 = ijk = -1`)
- 유니티에서는 `(x, y, z, w)` 순서로 표시하지만, 수학적으로는 `w`가 회전량과 관련된 실수부이며 `(x, y, z)`는 회전축과 관련된 허수부 벡터입니다.

### 2. 회전의 표현 (Axis-Angle)
임의의 축 `u = (ux, uy, uz)`를 기준으로 `θ`만큼 회전하고자 할 때, 이를 사원수로 변환하는 공식은 다음과 같습니다.
- `w = cos(θ / 2)`
- `x = ux * sin(θ / 2)`
- `y = uy * sin(θ / 2)`
- `z = uz * sin(θ / 2)`
- **특징:** 모든 유효한 회전 사원수의 크기 (Magnitude)는 항상 "**1**" (Unit Quaternion)이어야 합니다.

### 3. 실제 회전 계산 과정 (Hamilton Product)
3차원 상의 한 점 `v`를 사원수 `q`를 이용하여 회전시키는 수학적 과정은 다음과 같습니다.

1. **점의 사원수화:** 3차원 위치 벡터 `v(vx, vy, vz)`를 실수부가 0인 사원수 `p = (vx, vy, vz, 0)`로 변환합니다.
2. **회전 적용:** 사원수 곱셈을 사용하여 다음과 같이 계산합니다.
   - `p' = q * p * q^-1` (여기서 `q^-1`은 `q`의 역사원수입니다.)
3. **결과 추출:** 계산된 `p'`의 허수부 `(x', y', z')`가 바로 회전된 새로운 좌표값이 됩니다.

### 4. 왜 쿼터니언을 쓰는가?
- **연속성:** 오일러 각도는 특정 각도 (90도 등)에서 값이 튀는 현상이 있지만, 쿼터니언은 4차원 구면 위를 매끄럽게 이동하므로 회전이 끊기지 않습니다.
- **보간:** 두 회전 사이의 중간값을 찾을 때, `Slerp`를 이용하면 속도가 일정한 완벽한 구면 회전을 계산할 수 있습니다.

### 5. 유니티에서의 활용과 "**업 벡터 (Up Vector)**"의 중요성
쿼터니언 회전에서 가장 중요한 것은 "**어떤 축을 기준으로 돌릴 것인가**"가 명확하다는 점입니다.
- **LookRotation과 기준면:** 유니티의 `Quaternion.LookRotation(forward, up)` 함수는 앞 (Forward) 방향뿐만 아니라 위 (Up) 방향을 함께 입력받습니다. `up` 벡터를 명시함으로써 회전의 **기준면**을 고정하고, 물체가 의도치 않게 구르는 (Roll) 현상을 방지하여 유일한 회전 해를 찾습니다.
- **오른손 법칙과 방향성:** 명확한 축 (Up)이 세워지면 회전의 방향이 유일하게 결정되므로, 오일러 방식처럼 회전 중에 축이 겹쳐 예측 불가능해지는 짐벌락 현상을 원천적으로 차단합니다.

> 💡 **요약:** 쿼터니언은 "**축 (Axis)**"과 "**각도 (Angle)**" 정보를 4차원 공간에 압축하여 저장하고, 허수 연산을 통해 짐벌락 없이 점을 회전시키는 수학적 마법입니다. 특히 기준이 되는 축 (Up Vector)이 확실할 때 그 주변을 도는 궤적이 단 하나로 결정되어 완벽한 회전 제어가 가능합니다.

### 6. 실무 디버깅 팁: Vector3.Slerp와 Quaternion.Slerp의 숨겨진 차이
실무에서 3D 포물선 이동(예: 유도 미사일, 점프)이나 부드럽게 우회하는 카메라 연출을 할 때 가장 많이 마주하는 치명적인 버그와 수학적 해결법입니다.

* **Vector3.Slerp 의 기하학적 중심은 "월드 원점 `(0, 0, 0)`" 입니다.**
  - `Vector3.Slerp(a, b, t)`는 두 좌표를 원점 `(0, 0, 0)`을 기점으로 하는 위치 벡터로 처리합니다.
  - 따라서, 두 점이 원점과 멀리 떨어져 있다면 이동 궤적이 원점을 기점으로 거대하게 부풀어 올라 하늘 우주 끝으로 솟구치는 궤적 버그가 발생합니다.
  - **해결책 (상대 좌표 변환):** 원하는 중심점 (Center) 을 강제로 임시 원점으로 만들기 위해 빼준 상태에서 Slerp를 돌린 뒤, 결과에 다시 중심점을 더해 월드 좌표로 복구합니다.
    ```csharp
    Vector3 relA = a - center;
    Vector3 relB = b - center;
    // Vector3.Slerp는 두 위치 벡터를 구면 보간하지만, 기준 중심이 월드 원점이라는 점에 주의해야 합니다.
    Vector3 relResult = Vector3.Slerp(relA, relB, t);
    Vector3 finalPos = relResult + center;
    ```
* **Quaternion.Slerp 의 기하학적 중심은 "4차원 초구면의 중심" 입니다.**
  - 쿼터니언은 3D 월드 좌표가 아닌, 추상적인 4차원 회전 상태 공간 안의 원점을 기준으로 보간을 수행합니다.
  - 따라서, 오브젝트가 월드의 어느 엉뚱한 좌표에 가 있든 상관없이, 언제나 **오브젝트 자체의 기준축 (피벗, Pivot)** 을 기준으로 삼아 궤도가 절대 튀지 않고 제자리에서 완벽하고 매끄럽게 회전 보간을 완수합니다.

---

