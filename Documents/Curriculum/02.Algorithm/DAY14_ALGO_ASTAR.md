# Day 14: 고급 길찾기 - A* 알고리즘

오늘의 목표는 "**A*가 G 비용과 H 비용을 더해 다음에 볼 칸을 고르는 방식을 이해하고, 작은 그리드에서 경로와 안전장치를 눈으로 확인하는 것**"입니다.

Day11에서는 몬스터의 상태를 나눴고, Day12에서는 맵을 그래프로 바꿨고, Day13에서는 그래프를 탐색하는 순서를 봤습니다. Day14에서는 목표 방향으로 더 똑똑하게 탐색하는 A*를 다룹니다.

## 1. 핵심 개념: "지금까지 든 비용과 앞으로 남은 예상 비용"

A*는 모든 길을 무작정 확인하지 않습니다. 현재까지 실제로 이동한 비용과 목표까지 남은 예상 비용을 함께 봅니다.

```text
F = G + H
```

- `G`: 시작점에서 현재 칸까지 실제로 이동한 비용
- `H`: 현재 칸에서 목표까지 남았다고 예상하는 비용
- `F`: 다음에 어떤 칸을 먼저 볼지 고르는 기준

`F`가 낮은 칸은 "지금까지도 적게 들었고, 목표까지도 가까워 보이는 칸"입니다.

### 이 단어는 무슨 뜻인가요?

#### A*

그래프에서 시작점부터 목표점까지 좋은 경로를 찾는 알고리즘입니다. 보통 "에이 스타"라고 읽습니다.

#### G Cost

시작점부터 현재 노드까지 실제로 이동한 비용입니다.

#### H Cost

현재 노드에서 목표까지 남았다고 예상하는 비용입니다. 장애물을 완벽하게 계산하지 않고 대략적인 거리로 추정합니다.

#### F Cost

`G + H`입니다. A*는 보통 F 비용이 가장 낮은 노드를 먼저 확인합니다.

#### Open Set

앞으로 확인할 후보 노드 모음입니다.

#### Closed Set

이미 확인을 끝낸 노드 모음입니다.

## 2. 안전장치가 필요한 이유

목표가 벽으로 막혀 있거나, 코드에 실수가 있으면 탐색이 너무 오래 계속될 수 있습니다. 그래서 실제 구현에서는 최대 반복 횟수 같은 안전장치를 둡니다.

`maxIterations`는 경로를 반드시 찾게 만드는 값이 아닙니다. 탐색이 끝없이 이어지지 않도록 막는 상한선입니다.

## 실습 예제: Gizmos로 보는 A* 비용과 경로

**미션:** 작은 그리드에서 시작점, 목표점, 장애물, 경로를 색으로 확인합니다.

### 준비하기

1. Unity 씬에 빈 오브젝트 `AStarVisualizer`를 만듭니다.
2. 아래 스크립트를 붙입니다.
3. Scene 뷰 오른쪽 위의 `Gizmos` 버튼을 켭니다.
4. Inspector에서 `Start`, `Goal`, `Walls` 값을 바꿔 경로가 어떻게 달라지는지 확인합니다.

```csharp
using System.Collections.Generic;
using UnityEngine;

public class AStarGizmoVisualizer : MonoBehaviour
{
    [Header("Grid")]
    [Tooltip("가로 칸 수입니다.")]
    [SerializeField] private int width = 6;

    [Tooltip("세로 칸 수입니다.")]
    [SerializeField] private int height = 4;

    [Tooltip("Scene 뷰에 그릴 칸 크기입니다.")]
    [SerializeField] private float cellSize = 1f;

    [Header("Path")]
    [Tooltip("경로 탐색을 시작할 칸입니다.")]
    [SerializeField] private Vector2Int start = new Vector2Int(0, 0);

    [Tooltip("경로 탐색의 목표 칸입니다.")]
    [SerializeField] private Vector2Int goal = new Vector2Int(5, 3);

    [Tooltip("이동할 수 없는 벽 칸 목록입니다.")]
    [SerializeField] private Vector2Int[] walls =
    {
        new Vector2Int(2, 0),
        new Vector2Int(2, 1),
        new Vector2Int(2, 2)
    };

    [Tooltip("탐색이 끝없이 반복되지 않도록 막는 최대 반복 횟수입니다.")]
    [SerializeField] private int maxIterations = 100;

    private readonly List<Vector2Int> path = new List<Vector2Int>();
    private readonly HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
    private bool pathFound;
    private bool stoppedBySafetyLimit;

    private void OnValidate()
    {
        RebuildPath();
    }

    private void Awake()
    {
        RebuildPath();
    }

    private void RebuildPath()
    {
        path.Clear();
        closedSet.Clear();
        pathFound = false;
        stoppedBySafetyLimit = false;

        if (!IsInsideGrid(start) || !IsInsideGrid(goal) || IsWall(start) || IsWall(goal))
        {
            return;
        }

        List<Vector2Int> openSet = new List<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> gCost = new Dictionary<Vector2Int, int>();

        openSet.Add(start);
        gCost[start] = 0;

        int iterationCount = 0;

        while (openSet.Count > 0)
        {
            iterationCount++;

            if (iterationCount > maxIterations)
            {
                stoppedBySafetyLimit = true;
                return;
            }

            Vector2Int current = GetLowestFCostNode(openSet, gCost);

            if (current == goal)
            {
                BuildPath(cameFrom, current);
                pathFound = true;
                return;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || IsWall(neighbor))
                {
                    continue;
                }

                int newGCost = gCost[current] + 10;

                if (!gCost.ContainsKey(neighbor) || newGCost < gCost[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gCost[neighbor] = newGCost;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    private Vector2Int GetLowestFCostNode(List<Vector2Int> openSet, Dictionary<Vector2Int, int> gCost)
    {
        Vector2Int bestNode = openSet[0];
        int bestCost = GetFCost(bestNode, gCost);

        for (int i = 1; i < openSet.Count; i++)
        {
            int cost = GetFCost(openSet[i], gCost);

            if (cost < bestCost)
            {
                bestNode = openSet[i];
                bestCost = cost;
            }
        }

        return bestNode;
    }

    private int GetFCost(Vector2Int node, Dictionary<Vector2Int, int> gCost)
    {
        int g = gCost.ContainsKey(node) ? gCost[node] : 9999;
        int h = GetManhattanDistance(node, goal) * 10;
        return g + h;
    }

    private int GetManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
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

            if (IsInsideGrid(next))
            {
                yield return next;
            }
        }
    }

    private void BuildPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        path.Clear();
        path.Add(current);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
    }

    private bool IsInsideGrid(Vector2Int node)
    {
        return node.x >= 0 && node.x < width && node.y >= 0 && node.y < height;
    }

    private bool IsWall(Vector2Int node)
    {
        foreach (Vector2Int wall in walls)
        {
            if (wall == node)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        RebuildPath();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int node = new Vector2Int(x, y);
                Vector3 position = transform.position + new Vector3(x * cellSize, 0f, y * cellSize);

                Gizmos.color = GetNodeColor(node);
                Gizmos.DrawCube(position, Vector3.one * (cellSize * 0.85f));

                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(position, Vector3.one * (cellSize * 0.85f));
            }
        }

        DrawPathLines();
    }

    private Color GetNodeColor(Vector2Int node)
    {
        if (node == start)
        {
            return Color.green;
        }

        if (node == goal)
        {
            return Color.red;
        }

        if (IsWall(node))
        {
            return Color.black;
        }

        if (path.Contains(node))
        {
            return Color.yellow;
        }

        if (closedSet.Contains(node))
        {
            return stoppedBySafetyLimit ? Color.magenta : Color.cyan;
        }

        return Color.gray;
    }

    private void DrawPathLines()
    {
        if (!pathFound || path.Count < 2)
        {
            return;
        }

        Gizmos.color = Color.white;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 from = transform.position + new Vector3(path[i - 1].x * cellSize, 0.55f, path[i - 1].y * cellSize);
            Vector3 to = transform.position + new Vector3(path[i].x * cellSize, 0.55f, path[i].y * cellSize);
            Gizmos.DrawLine(from, to);
        }
    }
}
```

### 실행해보면

초록색 칸은 시작점, 빨간색 칸은 목표점입니다. 검은색 칸은 지나갈 수 없는 벽입니다. 노란색 칸은 A*가 찾은 경로이고, 하늘색 칸은 탐색 중 확인했던 칸입니다.

`Walls` 값을 바꾸면 경로가 우회됩니다. `maxIterations`를 너무 작게 줄이면 탐색이 안전장치에 걸려 하늘색 확인 칸이 보라색 계열로 표시됩니다.

### 생각해보기

1. `G` 비용만 사용하면 목표 방향을 고려하지 못하는 이유는 무엇일까요?
2. `H` 비용이 너무 부정확하면 어떤 경로를 먼저 보게 될까요?
3. `maxIterations`는 경로를 찾기 위한 값이 아니라 무엇을 막기 위한 값일까요?

## 오늘의 정리

- A*는 `F = G + H`로 다음에 확인할 노드를 고른다.
- `G`는 실제로 이동한 비용이다.
- `H`는 목표까지 남았다고 예상하는 비용이다.
- Open Set은 앞으로 확인할 후보이고, Closed Set은 이미 확인한 노드이다.
- `maxIterations` 같은 안전장치는 무한 반복이나 과도한 계산을 막기 위한 상한선이다.
