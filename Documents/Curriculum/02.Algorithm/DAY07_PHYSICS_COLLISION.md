# Day 07: 게임 물리 - 충돌 감지 기초

오늘의 목표는 "**물체를 단순한 경계 영역으로 감싸고, 두 물체가 겹쳤는지 수학적으로 판별하는 방법을 이해하는 것**"입니다.

충돌 판정은 게임에서 계속 일어나는 질문입니다.

> "플레이어가 아이템에 닿았는가?"
> "공격 범위 안에 몬스터가 들어왔는가?"
> "두 물체가 실제로 부딪혔는가?"

이 질문을 매번 물체의 복잡한 표면 전체로 계산하면 너무 무겁습니다. 그래서 게임은 먼저 물체를 단순한 상자나 구로 감싸고, 그 단순한 모양끼리 겹치는지 확인합니다.

## 1. 핵심 개념: "복잡한 물체를 쉬운 모양으로 감싸기"

충돌 판정에서 가장 중요한 생각은 "**정확도와 속도 사이의 균형**"입니다.

캐릭터의 갑옷, 무기, 팔, 다리 모양을 전부 계산하면 정확하지만 느립니다. 반대로 캐릭터 전체를 하나의 상자나 구로 감싸면 조금 덜 정확해도 훨씬 빠르게 계산할 수 있습니다.

이렇게 충돌 계산을 위해 물체를 감싸는 단순한 영역을 `Bounding Volume`이라고 합니다.

### 이 단어는 무슨 뜻인가요?

#### Bounding Volume

충돌 계산을 빠르게 하기 위해 물체를 감싸는 단순한 영역입니다. 상자, 구, 캡슐 같은 모양을 자주 씁니다.

#### AABB

`Axis-Aligned Bounding Box`의 줄임말입니다. 월드의 X, Y, Z축에 나란한 상자입니다.

![AABB Diagram](Images/aabb.svg)

- 물체가 회전해도 상자는 월드 축에 맞춰 유지됩니다.
- 계산이 단순해서 넓은 범위의 1차 충돌 검사에 자주 사용됩니다.

#### Bounding Sphere

물체를 하나의 구로 감싼 충돌 영역입니다.

![Bounding Sphere Diagram](Images/sphere.svg)

- 중심점 사이의 거리와 반지름만 비교하면 됩니다.
- 회전에 영향을 받지 않아 계산이 빠릅니다.

#### OBB

`Oriented Bounding Box`의 줄임말입니다. 물체의 회전 방향을 따라 함께 회전하는 상자입니다.

![OBB Diagram](Images/obb.svg)

- 물체 모양에 더 가깝게 맞출 수 있습니다.
- AABB보다 계산이 복잡합니다.

## 2. 충돌 판정 원리

### 구 충돌 판정

두 구가 충돌했는지는 중심점 사이의 거리와 두 반지름의 합을 비교해서 판단합니다.

![Sphere Collision Logic](Images/sphere_collision.svg)

```text
두 중심점 사이의 거리 <= 반지름 A + 반지름 B
```

실제 코드에서는 제곱근 계산을 피하기 위해 거리 자체가 아니라 "**거리의 제곱**"을 비교하는 경우가 많습니다.

```text
거리의 제곱 <= (반지름 A + 반지름 B)의 제곱
```

### AABB 충돌 판정

AABB는 모든 축에서 범위가 겹쳐야 충돌입니다.

![AABB Collision Logic](Images/aabb_collision.svg)

```text
X축 겹침 && Y축 겹침 && Z축 겹침
```

X축만 보면 다음과 같이 비교할 수 있습니다.

```text
A.max.x > B.min.x && A.min.x < B.max.x
```

## 3. Unity 물리 이벤트

Unity 물리 엔진은 충돌이 감지되었을 때 스크립트의 특정 메서드를 자동으로 호출합니다.

| 구분 | Collision | Trigger |
| :--- | :--- | :--- |
| 의미 | 실제로 부딪히는 물리 충돌 | 지나갈 수 있는 감지 영역 |
| 반응 | 튕겨 나가거나 막힘 | 통과하면서 감지만 함 |
| 설정 | `Is Trigger` 체크 해제 | `Is Trigger` 체크 |

`OnCollision...`과 `OnTrigger...` 이벤트는 Collider만 두 개 있다고 항상 호출되지 않습니다. 충돌하는 두 오브젝트 중 적어도 한쪽에는 `Rigidbody`가 있어야 Unity가 물리 엔진에서 움직이는 물체로 추적합니다.

```csharp
// OnCollisionEnter는 실제 물리 충돌이 시작될 때 호출됩니다.
private void OnCollisionEnter(Collision collision)
{
    // 실제 물리 충돌 처리 코드가 들어갈 자리입니다.
}

// OnTriggerEnter는 트리거 영역에 다른 Collider가 들어왔을 때 호출됩니다.
private void OnTriggerEnter(Collider other)
{
    // 트리거 감지 처리 코드가 들어갈 자리입니다.
}
```

## 실습 예제 1: Trigger와 Collision 이벤트 확인하기

**미션:** `Trigger`와 `Collision`이 언제 시작되고 끝나는지 Scene 뷰에서 확인합니다.

Unity에서는 영역에서 나가는 순간을 `Leave`가 아니라 `Exit`라는 이름으로 표현합니다. 그래서 메서드 이름도 `OnTriggerExit`, `OnCollisionExit`입니다.

### 준비하기

1. Unity 씬에 `Player` 오브젝트를 만들고 `Rigidbody`와 `Collider`를 붙입니다.
2. `Wall` 오브젝트를 만들고 `Collider`를 붙입니다. 이 Collider는 `Is Trigger`를 끕니다.
3. `SensorZone` 오브젝트를 만들고 `Collider`를 붙입니다. 이 Collider는 `Is Trigger`를 켭니다.
4. `Player`에 아래 스크립트를 붙입니다.
5. Scene 뷰 오른쪽 위의 `Gizmos` 버튼이 켜져 있는지 확인합니다.

```csharp
using UnityEngine;

public class PhysicsEventViewer : MonoBehaviour
{
    public string currentState = "대기 중";
    private Color gizmoColor = Color.gray;

    private void OnCollisionEnter(Collision collision)
    {
        // 실제로 부딪히는 충돌이 시작될 때 호출됩니다.
        currentState = "Collision Enter: " + collision.gameObject.name;
        gizmoColor = Color.red;
    }

    private void OnCollisionExit(Collision collision)
    {
        // 실제 물리 충돌이 끝나서 떨어졌을 때 호출됩니다.
        currentState = "Collision Exit: " + collision.gameObject.name;
        gizmoColor = Color.yellow;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 통과 가능한 감지 영역에 들어왔을 때 호출됩니다.
        currentState = "Trigger Enter: " + other.gameObject.name;
        gizmoColor = Color.cyan;
    }

    private void OnTriggerExit(Collider other)
    {
        // 통과 가능한 감지 영역에서 나갔을 때 호출됩니다.
        currentState = "Trigger Exit: " + other.gameObject.name;
        gizmoColor = Color.blue;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, 1.2f);
    }
}
```

### 실행해보면

`Player`가 `Wall`에 부딪히면 와이어 구체가 빨간색으로 바뀝니다. `Wall`에서 떨어지면 노란색으로 바뀝니다.

`Player`가 `SensorZone` 안으로 들어가면 하늘색으로 바뀝니다. `SensorZone` 밖으로 나가면 파란색으로 바뀝니다.

이 예제에서 `currentState`는 Inspector의 Debug 모드나 디버거로 확인할 수 있는 상태 값입니다. 화면에 꼭 보이지 않아도, 어떤 이벤트가 마지막으로 호출됐는지 코드 흐름을 남기기 위한 변수입니다.

## 실습 예제 2: 거리 제곱으로 구 충돌 판정하기

**미션:** 두 오브젝트의 중심점 사이 거리를 이용해 구 충돌을 직접 판정합니다.

### 준비하기

1. Unity 씬에 빈 오브젝트 `CollisionTester`를 만듭니다.
2. 비교할 대상 오브젝트를 하나 더 만들고 이름을 `Target`으로 바꿉니다.
3. `CollisionTester`에 아래 스크립트를 붙입니다.
4. Inspector에서 `Other` 칸에 `Target`을 연결합니다.
5. Scene 뷰 오른쪽 위의 `Gizmos` 버튼이 켜져 있는지 확인합니다.

```csharp
using UnityEngine;

public class SimpleCollision : MonoBehaviour
{
    public Transform other;
    public float radiusA = 1.0f;
    public float radiusB = 1.0f;

    private bool IsOverlapping()
    {
        if (other == null)
        {
            return false;
        }

        // 두 물체의 위치 차이를 벡터로 구합니다.
        Vector3 diff = transform.position - other.position;

        // sqrMagnitude는 벡터 길이의 제곱입니다.
        // 제곱근 계산 없이 거리 비교를 할 수 있습니다.
        float distanceSq = diff.sqrMagnitude;

        // 두 반지름을 더한 뒤 제곱합니다.
        float radiusSum = radiusA + radiusB;
        float radiusSumSq = radiusSum * radiusSum;

        // 거리의 제곱이 반지름 합의 제곱보다 작거나 같으면 두 구가 겹친 것입니다.
        return distanceSq <= radiusSumSq;
    }

    private void OnDrawGizmos()
    {
        // OnDrawGizmos는 Scene 뷰에 개발용 시각 표시를 그릴 때 사용하는 Unity 메시지 메서드입니다.
        if (other == null)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, radiusA);
            return;
        }

        bool isOverlapping = IsOverlapping();

        // 충돌하지 않으면 초록색, 충돌하면 빨간색으로 범위를 표시합니다.
        Gizmos.color = isOverlapping ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusA);
        Gizmos.DrawWireSphere(other.position, radiusB);

        // 두 중심점 사이의 거리를 눈으로 확인할 수 있도록 선을 그립니다.
        Gizmos.DrawLine(transform.position, other.position);
    }
}
```

### 실행해보면

Scene 뷰에 두 오브젝트의 충돌 범위가 원으로 표시됩니다.

`Target`을 `CollisionTester` 가까이 옮겨 두 원이 겹치면 Gizmos 색이 빨간색으로 바뀝니다. 겹치지 않으면 초록색으로 보입니다.

`radiusA`나 `radiusB` 값을 키우면 원이 커지고, 더 멀리 떨어져 있어도 충돌로 판정됩니다. 반대로 값을 줄이면 더 가까이 붙어야 충돌로 판정됩니다.

### 생각해보기

1. AABB는 왜 회전하지 않는 상자로 충돌 범위를 잡을까요?
2. 구 충돌 판정에서 거리 대신 거리의 제곱을 비교하면 어떤 계산을 줄일 수 있을까요?
3. 아이템 획득 영역은 `Collision`과 `Trigger` 중 어느 쪽이 더 어울릴까요?

## 선택 미션: Trigger와 Collision 구분하기

공격 범위처럼 물리적으로 밀어내지 않고 감지만 필요한 영역은 `Trigger`로 만드는 것이 자연스럽습니다. 벽이나 바닥처럼 실제로 막혀야 하는 물체는 `Collision`으로 처리하는 것이 자연스럽습니다.

작은 테스트 씬을 만들어 다음 두 상황을 비교해봅니다.

1. `Is Trigger`가 꺼진 Collider끼리 부딪혔을 때
2. 한쪽 Collider의 `Is Trigger`가 켜져 있을 때

비교할 때는 두 오브젝트 중 적어도 한쪽에 `Rigidbody`가 있어야 이벤트가 호출된다는 점을 함께 확인합니다.

## 오늘의 정리

- 충돌 판정은 복잡한 물체를 단순한 경계 영역으로 감싸서 빠르게 처리한다.
- AABB는 월드 축에 맞춘 상자라 계산이 빠르다.
- Bounding Sphere는 중심점 거리와 반지름으로 충돌을 판정한다.
- 구 충돌 판정에서는 제곱근 계산을 피하기 위해 거리의 제곱을 비교할 수 있다.
- Unity의 `Collision`은 실제 물리 충돌, `Trigger`는 통과 가능한 감지 영역에 어울린다.
