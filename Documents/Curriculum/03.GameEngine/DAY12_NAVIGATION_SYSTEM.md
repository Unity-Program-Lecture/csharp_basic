# DAY 12: Navigation 시스템과 NavMesh 기초

오늘의 목표는 Unity Navigation을 "**캐릭터가 걸어갈 수 있는 길 지도**"로 이해하고, `NavMeshSurface`와 `NavMeshAgent`를 사용해 오브젝트가 목표 지점까지 이동하는 흐름을 구성하는 것입니다.

## 1. 핵심 개념: "길을 먼저 구워 놓고, Agent가 그 길을 따라간다"

Navigation 시스템은 게임 오브젝트가 어느 공간을 걸어갈 수 있는지 계산해 둔 뒤, 목적지까지 이동 경로를 찾게 해 주는 기능입니다. 이때 걸어갈 수 있는 영역을 `NavMesh`라고 부릅니다.

레벨이 아무리 넓어도 모든 곳을 이동할 수 있는 것은 아닙니다. 벽, 장애물, 낭떠러지, 너무 좁은 통로는 이동할 수 없는 영역입니다. Navigation은 이런 공간 정보를 바탕으로 Agent가 이동 가능한 길을 찾도록 도와줍니다.

### 이 단어는 무슨 뜻인가요?

- **NavMesh**: 이동 가능한 영역을 표시한 길 지도입니다.
- **Bake**: 씬의 지형과 장애물을 분석해 NavMesh를 생성하는 작업입니다.
- **NavMeshSurface**: 어떤 오브젝트를 기준으로 NavMesh를 만들지 정하는 컴포넌트입니다.
- **NavMeshAgent**: NavMesh 위를 따라 이동하는 캐릭터용 컴포넌트입니다.
- **NavMeshObstacle**: Agent가 피하거나 길 계산에서 막힌 영역으로 볼 수 있는 장애물 컴포넌트입니다.
- **Destination**: Agent가 이동할 목표 위치입니다.
- **Area Mask**: Agent가 어떤 이동 영역을 사용할 수 있는지 정하는 설정입니다.

## 2. NavMeshSurface Inspector 주요 프로퍼티

`NavMeshSurface`는 AI Navigation 패키지에서 제공하는 컴포넌트입니다. 프로젝트에 보이지 않는 경우 Package Manager에서 AI Navigation 패키지를 설치해야 합니다.

| 프로퍼티 | 의미 | 수업에서 확인할 것 |
| :--- | :--- | :--- |
| `Agent Type` | 어떤 Agent 크기 기준으로 NavMesh를 만들지 결정 | 사람형 캐릭터인지, 작은 몬스터인지에 따라 달라짐 |
| `Collect Objects` | NavMesh를 만들 때 어떤 오브젝트를 모을지 결정 | 씬 전체, 자식 오브젝트 등 범위 확인 |
| `Include Layers` | 어떤 Layer의 오브젝트를 NavMesh 계산에 포함할지 결정 | 바닥은 포함하고 장식 오브젝트는 제외 가능 |
| `Use Geometry` | 렌더 메시 또는 물리 콜라이더 중 어떤 형태를 기준으로 계산할지 결정 | 충돌 기준과 시각 기준이 다를 수 있음 |
| `Default Area` | 기본 이동 영역 종류 | 보통 `Walkable`로 시작 |
| `Generate Links` | 끊어진 영역 사이 자동 연결 생성 여부 | 초급 실습에서는 꺼 두고 직접 확인하는 편이 안전 |

## 3. NavMeshAgent Inspector 주요 프로퍼티

| 프로퍼티 | 의미 | 수업에서 확인할 것 |
| :--- | :--- | :--- |
| `Agent Type` | 사용할 Agent 크기 기준 | Surface의 Agent Type과 맞는지 확인 |
| `Base Offset` | Agent 기준 위치의 높이 보정 | 모델이 바닥에 뜨거나 묻히면 확인 |
| `Speed` | 이동 속도 | 너무 높으면 코너에서 부자연스러움 |
| `Angular Speed` | 회전 속도 | 낮으면 방향 전환이 느려짐 |
| `Acceleration` | 목표 속도까지 도달하는 가속도 | 낮으면 출발과 정지가 둔해짐 |
| `Stopping Distance` | 목적지 앞에서 멈출 거리 | 상호작용 대상 앞에서 멈출 때 사용 |
| `Auto Braking` | 목적지 근처에서 자동 감속할지 여부 | 꺼 두면 목표 근처에서도 빠르게 지나칠 수 있음 |
| `Radius` | Agent의 폭 | 좁은 통로 통과 가능 여부에 영향 |
| `Height` | Agent의 키 | 낮은 천장이나 공간 판정에 영향 |
| `Obstacle Avoidance` | 다른 Agent나 장애물을 피하는 품질 | Agent가 많을수록 비용도 증가 |
| `Area Mask` | 이동 가능한 영역 종류 | 특정 지역을 못 지나가게 할 때 사용 |

## 4. NavMeshObstacle Inspector 주요 프로퍼티

| 프로퍼티 | 의미 | 수업에서 확인할 것 |
| :--- | :--- | :--- |
| `Shape` | 장애물 모양 | 박스 또는 캡슐 형태 |
| `Center` | 장애물 중심 위치 | 실제 모델과 어긋나지 않게 조정 |
| `Size` / `Radius` / `Height` | 장애물 크기 | Agent가 피해야 할 범위 |
| `Carve` | NavMesh에 구멍을 낼지 여부 | 움직이는 큰 장애물에 사용할 수 있음 |

## 실습 예제: 목표 지점까지 이동하는 Agent 만들기

**미션:** 바닥에 NavMesh를 Bake하고, Agent가 목표 지점으로 이동하게 만듭니다.

1. Package Manager에서 AI Navigation 패키지를 설치합니다.
2. 바닥 역할을 하는 `Plane` 또는 레벨 오브젝트를 만듭니다.
3. 빈 오브젝트 `NavigationSurface`를 만들고 `NavMeshSurface`를 추가합니다.
4. `NavMeshSurface`의 `Collect Objects`와 `Include Layers`를 확인합니다.
5. `Bake`를 실행해 이동 가능한 영역이 파란색으로 표시되는지 확인합니다.
6. 캡슐 오브젝트를 만들고 `NavMeshAgent`를 추가합니다.
7. 목표 지점 오브젝트 `TargetPoint`를 배치합니다.
8. 아래 스크립트를 `SimpleNavMeshMover.cs`로 만들고 Agent 오브젝트에 붙입니다.

```csharp
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleNavMeshMover : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
```

### 실행해보면

Agent가 NavMesh 위에서 목표 지점까지 이동합니다. 목표까지 직선으로 갈 수 없으면 NavMesh 위의 이동 가능한 길을 따라 돌아갑니다. `Stopping Distance`를 조정하면 목표 지점 바로 앞에서 멈추는 거리를 바꿀 수 있습니다.

### 생각해보기

1. 바닥이 보이는데 NavMesh가 생성되지 않는다면 어떤 설정을 확인해야 할까요?
2. Agent의 `Radius`가 너무 크면 좁은 문을 지나갈 수 있을까요?
3. Navigation은 AI 자체일까요, 아니면 이동 가능한 길을 계산하는 시스템일까요?

## 별첨: NavMeshModifier와 NavMeshModifierVolume

`NavMeshSurface`는 전체 NavMesh를 Bake하는 기준을 정합니다. 하지만 실제 레벨에서는 특정 오브젝트만 Bake에서 제외하거나, 특정 구역만 다른 이동 비용을 주고 싶을 때가 있습니다. 이때 사용하는 보조 컴포넌트가 `NavMeshModifier`와 `NavMeshModifierVolume`입니다.

## 별첨 1. NavMeshModifier

`NavMeshModifier`는 특정 GameObject가 NavMesh Bake에 어떻게 반영될지 바꾸는 컴포넌트입니다. 오브젝트 하나 또는 그 자식 오브젝트 묶음에 적용하는 설정이라고 보면 됩니다.

| 프로퍼티 | 의미 | 사용 예 |
| :--- | :--- | :--- |
| `Override Area` | 이 오브젝트의 Area 타입을 직접 지정할지 정함 | 바닥 일부를 `Not Walkable`로 바꾸기 |
| `Area Type` | 적용할 Area 종류 | `Walkable`, `Not Walkable`, `Jump` 등 |
| `Ignore From Build` | 이 오브젝트를 NavMesh Bake 계산에서 제외할지 정함 | 장식용 풀, 작은 돌, 시각 효과 오브젝트 제외 |
| `Apply To Children` | 자식 오브젝트에도 Modifier를 적용할지 정함 | 복잡한 장식물 묶음을 한 번에 제외 |
| `Affected Agents` | 어떤 Agent Type에 적용할지 정함 | 사람은 못 지나가지만 작은 몬스터는 지나가게 구분 |

예를 들어 바닥 위에 장식용 카펫이 있고, 카펫의 Mesh 때문에 NavMesh가 이상하게 갈라진다면 카펫 오브젝트에 `NavMeshModifier`를 붙이고 `Ignore From Build`를 켤 수 있습니다. 그러면 카펫은 화면에는 보이지만 NavMesh Bake 계산에는 들어가지 않습니다.

반대로 특정 다리나 위험 지역을 `Not Walkable`로 만들고 싶다면 `Override Area`를 켜고 `Area Type`을 `Not Walkable`로 지정합니다.

## 별첨 2. NavMeshModifierVolume

`NavMeshModifierVolume`은 오브젝트의 Mesh 모양이 아니라, 지정한 박스 형태의 공간에 Area 타입을 덮어씌우는 컴포넌트입니다. 특정 구역 전체를 느린 땅, 위험 지역, 이동 금지 영역처럼 처리할 때 사용합니다.

| 프로퍼티 | 의미 | 사용 예 |
| :--- | :--- | :--- |
| `Size` | Volume의 박스 크기 | 늪지대나 위험 구역 범위 지정 |
| `Center` | Volume 중심 위치 | 실제 구역과 박스 위치 맞추기 |
| `Area Type` | Volume 안에 적용할 Area 종류 | `Not Walkable` 또는 별도 Area |
| `Affected Agents` | 어떤 Agent Type에 적용할지 정함 | 큰 몬스터만 못 지나가는 영역 만들기 |

`NavMeshModifierVolume`은 보이지 않는 박스를 씬에 놓고, 그 안쪽 NavMesh Area를 바꾸는 방식입니다. 모델의 모양과 상관없이 사각형 구역 단위로 이동 규칙을 바꾸고 싶을 때 적합합니다.

## 별첨 3. 언제 무엇을 쓰나요?

| 상황 | 사용할 컴포넌트 |
| :--- | :--- |
| 특정 오브젝트를 Bake에서 제외하고 싶음 | `NavMeshModifier` |
| 특정 오브젝트와 자식 전체에 같은 Area를 주고 싶음 | `NavMeshModifier` + `Apply To Children` |
| 보이지 않는 박스 구역 전체를 이동 금지로 만들고 싶음 | `NavMeshModifierVolume` |
| 움직이는 장애물이 Agent를 막거나 피하게 만들고 싶음 | `NavMeshObstacle` |
| NavMesh를 생성하고 Bake하고 싶음 | `NavMeshSurface` |

정리하면 `NavMeshSurface`는 전체 지도를 굽는 오븐이고, `NavMeshModifier`는 특정 재료를 빼거나 다른 재료로 표시하는 태그입니다. `NavMeshModifierVolume`은 지도 위에 투명한 박스를 올려 특정 구역의 이동 규칙을 바꾸는 도구입니다.

## 오늘의 정리

- NavMesh는 이동 가능한 영역을 미리 계산해 둔 길 지도입니다.
- `NavMeshSurface`는 NavMesh를 만들 범위와 기준을 정합니다.
- `NavMeshAgent`는 NavMesh 위에서 목적지를 향해 이동합니다.
- `NavMeshObstacle`은 Agent가 피해야 할 장애물이나 길을 막는 대상을 표현합니다.
- `NavMeshModifier`와 `NavMeshModifierVolume`은 Bake 대상과 Area 타입을 세부적으로 조절할 때 사용합니다.
- 엔진 과정에서는 Navigation 시스템의 구조를 이해하고, 클라이언트나 AI 과정에서 추적, 순찰, 상호작용 이동으로 확장합니다.
