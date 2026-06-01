# Day 12: 길찾기 알고리즘 기초 - 그래프와 노드

오늘의 목표는 "**게임 맵을 노드와 간선으로 단순화하고, 어떤 지점들이 서로 연결되어 있는지 Scene 뷰에서 확인하는 것**"입니다.

길찾기를 시작하려면 먼저 맵을 컴퓨터가 이해할 수 있는 형태로 바꿔야 합니다. 복잡한 마을, 던전, 숲 전체를 그대로 계산하기보다, 중요한 위치를 점으로 표시하고 이동 가능한 길을 선으로 연결합니다.

## 1. 핵심 개념: "맵을 점과 선으로 바꾸기"

그래프는 점과 선으로 이루어진 구조입니다.

![Graph Concept Diagram](Images/graph_concept.svg)

- 점은 위치입니다.
- 선은 이동 가능한 길입니다.
- 선에는 이동 비용을 붙일 수 있습니다.

마을 입구, 광장, 상점, 성문을 각각 점으로 보고, 서로 이동할 수 있는 곳만 선으로 연결하면 길찾기용 그래프가 됩니다.

### 이 단어는 무슨 뜻인가요?

#### Graph

노드와 간선으로 이루어진 연결 구조입니다. 길찾기에서는 맵의 이동 가능 관계를 표현합니다.

#### Node

그래프의 점입니다. 게임에서는 타일 한 칸, 웨이포인트, 방, 교차로가 노드가 될 수 있습니다.

#### Edge

노드와 노드를 잇는 선입니다. 두 지점 사이를 이동할 수 있다는 뜻입니다.

#### Weight

간선을 지나갈 때 드는 비용입니다. 가까운 길은 낮은 비용, 늪이나 산길은 높은 비용을 줄 수 있습니다.

#### Adjacency List

각 노드가 어떤 이웃 노드와 연결되어 있는지 목록으로 저장하는 방식입니다.

## 2. 그래프를 고르는 순간

그래프는 "무엇이 무엇과 연결되어 있는가?"가 중요할 때 사용합니다.

| 게임 상황 | 그래프로 볼 수 있는 것 |
| --- | --- |
| 순찰 경로 | 순찰 지점과 이동 가능한 길 |
| 던전 방 연결 | 방과 문 |
| 타일 맵 이동 | 타일과 인접 타일 |
| 대화 선택지 | 선택지와 다음 대화 |

## 실습 예제: Gizmos로 보는 노드와 간선

**미션:** 여러 노드를 Scene 뷰에 점으로 표시하고, 연결된 노드 사이에 선을 그려 그래프 구조를 확인합니다.

### 준비하기

1. Unity 씬에 빈 오브젝트 `GraphVisualizer`를 만듭니다.
2. 아래 스크립트를 붙입니다.
3. Scene 뷰 오른쪽 위의 `Gizmos` 버튼을 켭니다.
4. Inspector에서 `Node Positions`의 좌표를 바꿔 그래프 모양이 어떻게 변하는지 확인합니다.

```csharp
using UnityEngine;

public class GraphGizmoVisualizer : MonoBehaviour
{
    [Header("Nodes")]
    [Tooltip("그래프 노드들의 위치입니다. 각 원소가 Scene 뷰의 점 하나가 됩니다.")]
    [SerializeField] private Vector3[] nodePositions =
    {
        new Vector3(0f, 0f, 0f),
        new Vector3(2f, 0f, 1f),
        new Vector3(4f, 0f, 0f),
        new Vector3(2f, 0f, -2f)
    };

    [Header("Edges")]
    [Tooltip("연결할 노드 번호 쌍입니다. 예: (0, 1)은 0번 노드와 1번 노드를 연결합니다.")]
    [SerializeField] private Vector2Int[] edges =
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 2),
        new Vector2Int(1, 3),
        new Vector2Int(3, 2)
    };

    [Tooltip("노드를 Scene 뷰에 그릴 때 사용할 크기입니다.")]
    [SerializeField] private float nodeRadius = 0.2f;

    private void OnDrawGizmos()
    {
        if (nodePositions == null)
        {
            return;
        }

        DrawEdges();
        DrawNodes();
    }

    private void DrawNodes()
    {
        for (int i = 0; i < nodePositions.Length; i++)
        {
            Vector3 worldPosition = transform.position + nodePositions[i];

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(worldPosition, nodeRadius);

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(worldPosition, nodeRadius + 0.04f);
        }
    }

    private void DrawEdges()
    {
        if (edges == null)
        {
            return;
        }

        Gizmos.color = Color.white;

        foreach (Vector2Int edge in edges)
        {
            if (!IsValidNodeIndex(edge.x) || !IsValidNodeIndex(edge.y))
            {
                continue;
            }

            Vector3 from = transform.position + nodePositions[edge.x];
            Vector3 to = transform.position + nodePositions[edge.y];
            Gizmos.DrawLine(from, to);
        }
    }

    private bool IsValidNodeIndex(int index)
    {
        return index >= 0 && index < nodePositions.Length;
    }
}
```

### 실행해보면

Scene 뷰에 하늘색 점들이 보입니다. 각 점은 노드입니다. 흰색 선은 두 노드가 서로 연결되어 있다는 뜻입니다.

`Node Positions` 값을 바꾸면 점의 위치가 바뀝니다. `Edges` 값을 바꾸면 어떤 점끼리 연결되는지도 바뀝니다.

### 생각해보기

1. 노드가 많아질수록 모든 노드를 모든 노드와 연결하면 어떤 문제가 생길까요?
2. 진흙탕 길처럼 이동하기 힘든 길은 어디에 비용을 붙이면 좋을까요?
3. Day11의 `Chase` 상태에서 몬스터가 플레이어를 쫓으려면 그래프 정보가 어떻게 쓰일 수 있을까요?

## 오늘의 정리

- 그래프는 노드와 간선으로 이루어진 연결 구조이다.
- 노드는 위치나 상태를 나타내는 점이다.
- 간선은 두 노드 사이를 이동할 수 있다는 연결 정보이다.
- 가중치는 길을 지나갈 때 드는 비용이다.
- 길찾기는 그래프 위에서 시작 노드에서 목표 노드까지 이동하는 문제로 바꿔 생각할 수 있다.
