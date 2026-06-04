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
    // 격자의 가로 칸 개수입니다. x 좌표는 0부터 width - 1까지 사용할 수 있습니다.
    [Tooltip("가로 칸 수입니다.")]
    [SerializeField] private int width = 6;

    // 격자의 세로 칸 개수입니다. y 좌표는 0부터 height - 1까지 사용할 수 있습니다.
    [Tooltip("세로 칸 수입니다.")]
    [SerializeField] private int height = 4;

    // Scene 뷰에 보이는 한 칸의 실제 크기입니다. 값이 커질수록 칸 사이 간격도 커집니다.
    [Tooltip("Scene 뷰에 그릴 칸 크기입니다.")]
    [SerializeField] private float cellSize = 1f;

    [Header("Path")]
    // 탐색을 시작하는 칸입니다. 초록색으로 표시됩니다.
    [Tooltip("경로 탐색을 시작할 칸입니다.")]
    [SerializeField] private Vector2Int start = new Vector2Int(0, 0);

    // 도착해야 하는 목표 칸입니다. 빨간색으로 표시됩니다.
    [Tooltip("경로 탐색의 목표 칸입니다.")]
    [SerializeField] private Vector2Int goal = new Vector2Int(5, 3);

    // 지나갈 수 없는 칸 목록입니다. 검은색으로 표시됩니다.
    [Tooltip("이동할 수 없는 벽 칸 목록입니다.")]
    [SerializeField] private Vector2Int[] walls =
    {
        new Vector2Int(2, 0),
        new Vector2Int(2, 1),
        new Vector2Int(2, 2)
    };

    // 실수로 너무 오래 탐색하지 않도록 막는 안전장치입니다.
    [Tooltip("탐색이 끝없이 반복되지 않도록 막는 최대 반복 횟수입니다.")]
    [SerializeField] private int maxIterations = 100;

    // 최종으로 찾은 경로를 저장합니다. 시작점부터 목표점 순서로 들어갑니다.
    private readonly List<Vector2Int> path = new List<Vector2Int>();

    // 이미 확인이 끝난 칸을 저장합니다. 다시 검사하지 않기 위한 목록입니다.
    private readonly HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

    // 경로를 찾았는지 여부입니다. 흰색 경로 선을 그릴지 결정할 때 사용합니다.
    private bool pathFound;

    // maxIterations에 걸려서 탐색이 중단되었는지 여부입니다.
    private bool stoppedBySafetyLimit;

    // OnValidate는 Unity Editor에서 Inspector 값이 바뀌거나 스크립트가 다시 로드될 때 호출됩니다.
    // 게임을 실행하지 않아도 Scene 뷰에서 경로 변화를 바로 확인하기 위해 사용합니다.
    private void OnValidate()
    {
        // Inspector에서 값을 바꾸는 즉시 경로를 다시 계산합니다.
        RebuildPath();
    }

    private void Awake()
    {
        // 게임이 실행될 때도 현재 설정으로 경로를 한 번 계산합니다.
        RebuildPath();
    }

    private void RebuildPath()
    {
        // 이전 계산 결과가 남아 있으면 새 결과와 섞이므로 먼저 비웁니다.
        path.Clear();
        closedSet.Clear();
        pathFound = false;
        stoppedBySafetyLimit = false;

        // 시작점이나 목표점이 격자 밖에 있거나 벽이면 탐색을 시작할 수 없습니다.
        if (!IsInsideGrid(start) || !IsInsideGrid(goal) || IsWall(start) || IsWall(goal))
        {
            return;
        }

        // openSet은 "아직 후보로 남아 있는 칸"입니다.
        List<Vector2Int> openSet = new List<Vector2Int>();

        // cameFrom은 각 칸에 도착하기 직전에 있었던 칸을 저장합니다.
        // 나중에 목표점에서 시작점까지 거꾸로 따라가며 경로를 복원합니다.
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        // gCost는 시작점에서 해당 칸까지 실제로 이동한 비용입니다.
        Dictionary<Vector2Int, int> gCost = new Dictionary<Vector2Int, int>();

        // 탐색은 시작점 하나를 후보 목록에 넣는 것에서 출발합니다.
        openSet.Add(start);
        gCost[start] = 0;

        int iterationCount = 0;

        // 후보 칸이 남아 있는 동안 계속 가장 좋아 보이는 칸을 하나씩 꺼내 확인합니다.
        while (openSet.Count > 0)
        {
            iterationCount++;

            // 경로를 못 찾은 채 너무 오래 반복되면 안전하게 중단합니다.
            if (iterationCount > maxIterations)
            {
                stoppedBySafetyLimit = true;
                return;
            }

            // F 비용이 가장 낮은 칸을 이번에 확인할 칸으로 고릅니다.
            Vector2Int current = GetLowestFCostNode(openSet, gCost);

            // 목표에 도착했다면 cameFrom을 따라 최종 경로를 만들고 탐색을 끝냅니다.
            if (current == goal)
            {
                BuildPath(cameFrom, current);
                pathFound = true;
                return;
            }

            // current는 이제 확인이 끝났으므로 후보 목록에서 빼고 closedSet에 넣습니다.
            openSet.Remove(current);
            closedSet.Add(current);

            // current의 위, 오른쪽, 아래, 왼쪽 칸을 차례로 확인합니다.
            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                // 이미 확인했거나 벽인 칸은 지나갈 수 없으므로 건너뜁니다.
                if (closedSet.Contains(neighbor) || IsWall(neighbor))
                {
                    continue;
                }

                // 이 예제에서는 한 칸 이동 비용을 10으로 고정합니다.
                int newGCost = gCost[current] + 10;

                // 처음 발견한 칸이거나, 더 싼 비용으로 도착할 수 있으면 정보를 갱신합니다.
                if (!gCost.ContainsKey(neighbor) || newGCost < gCost[neighbor])
                {
                    // neighbor로 오기 직전 칸은 current였다고 기록합니다.
                    cameFrom[neighbor] = current;
                    gCost[neighbor] = newGCost;

                    // 아직 후보 목록에 없다면 다음 탐색 후보로 등록합니다.
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
        // 일단 첫 번째 후보를 가장 좋은 칸이라고 가정하고 시작합니다.
        Vector2Int bestNode = openSet[0];
        int bestCost = GetFCost(bestNode, gCost);

        // 나머지 후보와 비교하면서 F 비용이 더 낮은 칸을 찾습니다.
        for (int i = 1; i < openSet.Count; i++)
        {
            int cost = GetFCost(openSet[i], gCost);

            // F 비용이 낮다는 것은 "지금까지 비용도 낮고 목표에도 가까워 보인다"는 뜻입니다.
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
        // G 비용: 시작점에서 이 칸까지 실제로 이동한 비용입니다.
        int g = gCost.ContainsKey(node) ? gCost[node] : 9999;

        // H 비용: 이 칸에서 목표까지 남았다고 예상하는 비용입니다.
        // 대각선 이동이 없으므로 x 차이 + y 차이를 사용하는 맨해튼 거리를 씁니다.
        int h = GetManhattanDistance(node, goal) * 10;

        // F 비용은 A*가 다음 후보를 고를 때 사용하는 점수입니다.
        return g + h;
    }

    private int GetManhattanDistance(Vector2Int a, Vector2Int b)
    {
        // 두 칸 사이를 가로/세로 이동만으로 간다고 생각했을 때 필요한 최소 칸 수입니다.
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // 이 예제에서는 대각선 이동을 허용하지 않고 4방향만 확인합니다.
        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        foreach (Vector2Int direction in directions)
        {
            // 현재 칸에 방향 값을 더하면 그 방향의 이웃 칸 좌표가 됩니다.
            Vector2Int next = node + direction;

            // 격자 밖으로 나간 칸은 후보에 넣지 않습니다.
            if (IsInsideGrid(next))
            {
                neighbors.Add(next);
            }
        }

        return neighbors;
    }

    private void BuildPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        // current는 목표점입니다. 목표점부터 거꾸로 경로를 모읍니다.
        path.Clear();
        path.Add(current);

        // cameFrom에 기록된 이전 칸을 계속 따라가면 시작점까지 되돌아갈 수 있습니다.
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        // 지금은 목표점 -> 시작점 순서이므로 뒤집어서 시작점 -> 목표점 순서로 만듭니다.
        path.Reverse();
    }

    private bool IsInsideGrid(Vector2Int node)
    {
        // x와 y가 각각 허용 범위 안에 들어와야 격자 안의 칸입니다.
        return node.x >= 0 && node.x < width && node.y >= 0 && node.y < height;
    }

    private bool IsWall(Vector2Int node)
    {
        // walls 배열에 같은 좌표가 하나라도 있으면 벽으로 판단합니다.
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
        // Scene 뷰가 갱신될 때마다 최신 Inspector 값으로 경로를 다시 계산합니다.
        RebuildPath();

        // y와 x를 돌며 모든 격자 칸을 하나씩 그립니다.
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int node = new Vector2Int(x, y);

                // 2D 격자 좌표를 Unity 월드 좌표로 바꿉니다. y 높이는 0으로 고정합니다.
                Vector3 position = transform.position + new Vector3(x * cellSize, 0f, y * cellSize);

                // 칸의 상태에 따라 색을 고르고, 색칠된 정육면체를 그립니다.
                Gizmos.color = GetNodeColor(node);
                Gizmos.DrawCube(position, Vector3.one * (cellSize * 0.85f));

                // 칸 경계를 검은 선으로 한 번 더 그려서 격자 구조가 잘 보이게 합니다.
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(position, Vector3.one * (cellSize * 0.85f));
            }
        }

        // 경로를 찾았다면 칸 중심을 잇는 흰색 선을 추가로 그립니다.
        DrawPathLines();
    }

    private Color GetNodeColor(Vector2Int node)
    {
        // 시작점은 가장 먼저 눈에 띄도록 초록색으로 표시합니다.
        if (node == start)
        {
            return Color.green;
        }

        // 목표점은 빨간색으로 표시합니다.
        if (node == goal)
        {
            return Color.red;
        }

        // 벽은 지나갈 수 없는 칸이므로 검은색으로 표시합니다.
        if (IsWall(node))
        {
            return Color.black;
        }

        // 최종 경로에 포함된 칸은 노란색으로 표시합니다.
        if (path.Contains(node))
        {
            return Color.yellow;
        }

        // 확인이 끝난 칸은 하늘색으로 표시합니다.
        // 안전장치에 걸려 중단된 경우에는 보라색으로 표시해 실패 원인을 구분합니다.
        if (closedSet.Contains(node))
        {
            return stoppedBySafetyLimit ? Color.magenta : Color.cyan;
        }

        // 아직 탐색에서 특별한 의미가 없는 기본 칸입니다.
        return Color.gray;
    }

    private void DrawPathLines()
    {
        // 경로를 찾지 못했거나 선을 이을 만큼 칸이 부족하면 아무것도 그리지 않습니다.
        if (!pathFound || path.Count < 2)
        {
            return;
        }

        Gizmos.color = Color.white;

        // path[0] -> path[1], path[1] -> path[2]처럼 이웃한 경로 칸끼리 선을 잇습니다.
        for (int i = 1; i < path.Count; i++)
        {
            // 칸 중심보다 살짝 높은 위치에 선을 그려서 큐브 안에 묻히지 않게 합니다.
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
