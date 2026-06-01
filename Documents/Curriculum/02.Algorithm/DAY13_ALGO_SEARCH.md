# Day 13: 그래프 탐색 - BFS와 DFS

오늘의 목표는 "**그래프에서 시작점부터 목표점까지 노드를 어떤 순서로 방문하는지 이해하고, BFS와 DFS의 차이를 Scene 뷰에서 확인하는 것**"입니다.

Day12에서 맵을 노드와 간선으로 바꾸는 방법을 봤습니다. 이제는 그 그래프 위를 어떻게 탐색할지 정해야 합니다.

## 1. 핵심 개념: "가까운 곳부터 볼까, 한 길로 깊게 들어갈까?"

탐색은 아직 모르는 길을 하나씩 확인하는 과정입니다.

BFS는 시작점 주변부터 넓게 퍼져나갑니다. 물결이 퍼지는 느낌입니다. 가까운 노드를 먼저 보므로, 모든 간선 비용이 같다면 가장 적은 단계의 경로를 찾기 좋습니다.

DFS는 한 방향으로 갈 수 있는 데까지 깊게 들어갑니다. 막히면 되돌아와 다른 길을 봅니다. 미로를 한 길씩 끝까지 파보는 느낌입니다.

### 이 단어는 무슨 뜻인가요?

#### BFS

`Breadth-First Search`의 줄임말입니다. 너비 우선 탐색이라고 부릅니다. 시작점에서 가까운 노드부터 차례대로 방문합니다.

#### DFS

`Depth-First Search`의 줄임말입니다. 깊이 우선 탐색이라고 부릅니다. 한 방향으로 깊게 들어갔다가 막히면 되돌아옵니다.

#### Visited

이미 방문한 노드를 뜻합니다. 같은 노드를 계속 다시 방문하면 탐색이 끝나지 않을 수 있으므로 방문 기록이 필요합니다.

#### Queue

BFS에서 자주 사용하는 줄서기 구조입니다. 먼저 들어온 노드를 먼저 꺼냅니다.

#### Stack

DFS에서 자주 사용하는 쌓기 구조입니다. 나중에 들어온 노드를 먼저 꺼냅니다.

## 2. BFS와 DFS 비교

| 구분 | BFS | DFS |
| --- | --- | --- |
| 방문 느낌 | 가까운 곳부터 넓게 퍼진다 | 한 길을 깊게 파고든다 |
| 주로 쓰는 자료구조 | `Queue<T>` | `Stack<T>` 또는 재귀 |
| 잘 어울리는 상황 | 이동 가능 범위, 최단 단계 찾기 | 미로 탐색, 모든 경우 확인 |
| 주의할 점 | 넓게 퍼져 메모리를 많이 쓸 수 있다 | 깊게 들어가 돌아오는 흐름을 놓치기 쉽다 |

## 실습 예제: Gizmos로 보는 BFS 방문 순서

**미션:** 3x3 그리드에서 BFS가 어떤 순서로 칸을 방문하는지 색으로 확인합니다.

### 준비하기

1. Unity 씬에 빈 오브젝트 `BfsVisualizer`를 만듭니다.
2. 아래 스크립트를 붙입니다.
3. Scene 뷰 오른쪽 위의 `Gizmos` 버튼을 켭니다.
4. Play 모드에서 `Space` 키를 눌러 한 단계씩 방문 순서를 진행합니다.
5. `R` 키를 누르면 탐색을 처음부터 다시 시작합니다.

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BfsGizmoVisualizer : MonoBehaviour
{
    [Header("Grid")]
    [Tooltip("가로 칸 수입니다.")]
    [SerializeField] private int width = 3;

    [Tooltip("세로 칸 수입니다.")]
    [SerializeField] private int height = 3;

    [Tooltip("Scene 뷰에 그릴 칸 크기입니다.")]
    [SerializeField] private float cellSize = 1f;

    private readonly Queue<Vector2Int> frontier = new Queue<Vector2Int>();
    private readonly HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
    private Vector2Int currentNode;

    private void Start()
    {
        ResetSearch();
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StepSearch();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetSearch();
        }
    }

    private void ResetSearch()
    {
        frontier.Clear();
        visited.Clear();

        currentNode = new Vector2Int(0, 0);
        frontier.Enqueue(currentNode);
        visited.Add(currentNode);
    }

    private void StepSearch()
    {
        if (frontier.Count == 0)
        {
            return;
        }

        currentNode = frontier.Dequeue();

        foreach (Vector2Int neighbor in GetNeighbors(currentNode))
        {
            if (visited.Contains(neighbor))
            {
                continue;
            }

            visited.Add(neighbor);
            frontier.Enqueue(neighbor);
        }
    }

    private IEnumerable<Vector2Int> GetNeighbors(Vector2Int node)
    {
        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int next = node + direction;

            if (next.x < 0 || next.x >= width || next.y < 0 || next.y >= height)
            {
                continue;
            }

            yield return next;
        }
    }

    private void OnDrawGizmos()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int node = new Vector2Int(x, y);
                Vector3 position = transform.position + new Vector3(x * cellSize, 0f, y * cellSize);

                Gizmos.color = GetNodeColor(node);
                Gizmos.DrawCube(position, Vector3.one * (cellSize * 0.8f));

                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(position, Vector3.one * (cellSize * 0.8f));
            }
        }
    }

    private Color GetNodeColor(Vector2Int node)
    {
        if (Application.isPlaying && node == currentNode)
        {
            return Color.yellow;
        }

        if (Application.isPlaying && visited.Contains(node))
        {
            return Color.cyan;
        }

        return Color.gray;
    }
}
```

### 실행해보면

`Space`를 누를 때마다 노란색 현재 칸이 이동하고, 방문한 칸은 하늘색으로 남습니다. BFS는 시작점 주변의 가까운 칸부터 넓게 퍼져나가는 모습을 보입니다.

`R` 키를 누르면 방문 기록이 지워지고 다시 시작됩니다.

### 생각해보기

1. BFS가 가까운 칸부터 방문하는 이유는 Queue의 어떤 특징 때문일까요?
2. DFS로 바꾸려면 Queue 대신 어떤 자료구조를 사용하면 좋을까요?
3. 이동 비용이 모두 같을 때 BFS가 짧은 단계의 경로를 찾기 쉬운 이유는 무엇일까요?

## 오늘의 정리

- BFS는 가까운 노드부터 넓게 탐색한다.
- DFS는 한 방향으로 깊게 탐색한다.
- BFS는 Queue와 잘 어울린다.
- DFS는 Stack 또는 재귀와 잘 어울린다.
- 방문 기록이 없으면 같은 노드를 반복해서 방문할 수 있다.
