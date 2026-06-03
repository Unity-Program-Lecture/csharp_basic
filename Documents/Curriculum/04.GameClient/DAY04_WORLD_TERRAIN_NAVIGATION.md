# DAY 04: 월드, 지형, 이동 가능 영역

오늘의 목표는 게임 월드를 "**캐릭터가 걸어 다닐 수 있는 무대**"처럼 이해하고, 이동 가능한 영역과 막힌 영역을 Unity 씬 안에서 구분하는 방법을 배우는 것입니다.

## 1. 핵심 개념: "지형은 보기 위한 바닥이 아니라 이동 규칙이다"

지형은 단순한 배경이 아닙니다. 캐릭터가 걸을 수 있는 곳, 막히는 곳, 떨어지는 곳, 느려지는 곳을 정하는 게임 규칙의 일부입니다. 그래서 클라이언트 프로그래밍에서는 지형을 만들 때 모양뿐 아니라 이동 가능 여부와 충돌 설정도 함께 생각해야 합니다.

NCS 원문에서는 높이 맵과 지형 내비게이션 알고리즘을 다룹니다. 이 수업에서는 Unity 6 입문 흐름에 맞춰 Terrain, Collider, NavMesh 개념을 먼저 잡고, 작은 씬에서 이동 가능 영역을 눈으로 확인합니다.

### 이 단어는 무슨 뜻인가요?

- **World**: 캐릭터와 오브젝트가 존재하는 게임 공간입니다.
- **Terrain**: Unity에서 지형을 만들 때 사용하는 대표적인 컴포넌트입니다.
- **Collider**: 오브젝트가 막히거나 감지될 수 있는 물리 영역입니다.
- **NavMesh**: 캐릭터나 NPC가 걸어갈 수 있는 표면 정보입니다.
- **Obstacle**: 이동을 막거나 우회하게 만드는 장애물입니다.

## 실습 예제: 이동 가능 영역을 색으로 표시하기

**미션:** 플레이어 주변에 이동 가능한 지점과 막힌 지점을 Gizmos로 표시합니다.

1. Plane을 바닥으로 만들고, Cube 몇 개를 장애물로 배치합니다.
2. 장애물에는 Collider가 있어야 합니다.
3. 빈 GameObject를 만들고 아래 스크립트를 붙입니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class WalkableAreaPreview : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(7, 7);
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float checkRadius = 0.35f;
    [SerializeField] private LayerMask obstacleMask;

    void OnDrawGizmos()
    {
        Vector3 start = transform.position;

        for (int z = 0; z < gridSize.y; z++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector3 offset = new Vector3(
                    (x - gridSize.x / 2) * cellSize,
                    0f,
                    (z - gridSize.y / 2) * cellSize
                );

                Vector3 point = start + offset;
                bool blocked = Physics.CheckSphere(point, checkRadius, obstacleMask);

                Gizmos.color = blocked ? Color.red : Color.green;
                Gizmos.DrawWireSphere(point, checkRadius);
            }
        }
    }
}
```

</details>

### 실행해보면

Scene 뷰에서 초록색 구는 이동 가능한 지점, 빨간색 구는 장애물과 겹치는 지점으로 표시됩니다. 장애물 Layer를 따로 만들고 `obstacleMask`에 연결하면 더 정확하게 확인할 수 있습니다.

### 생각해보기

1. 지형이 예쁘게 보여도 Collider가 없다면 캐릭터 이동에는 어떤 문제가 생길까요?
2. 이동 가능 영역을 먼저 확인하면 NPC 이동이나 퀘스트 배치에 어떤 도움이 될까요?

## 오늘의 정리

- 월드는 캐릭터가 움직이는 무대이면서 이동 규칙을 담는 공간입니다.
- Collider와 Layer를 사용하면 막힌 영역과 이동 가능한 영역을 구분할 수 있습니다.
- `OnDrawGizmos`는 Scene 뷰에서 보이지 않는 규칙을 눈으로 확인하게 도와줍니다.
