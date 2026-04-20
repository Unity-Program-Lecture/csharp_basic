# 🚀 [알고리즘 08] 거미줄 네트워크: 그래프(Graph)와 타일 맵 BFS

학습 목표: 노드와 간선으로 이루어진 '그래프'의 개념을 이해하고, 너비 우선 탐색(BFS) 알고리즘을 사용하여 타일 맵 게임에서 캐릭터의 이동 범위를 계산하는 방법을 배웁니다.

---

## 💡 개념 설명 (NCS 알고리즘: 비선형 자료구조 - 그래프)

### 1. 그래프(Graph)란 무엇인가요?
그래프는 '친구 관계'나 '지하철 노선도'처럼 여러 지점(Node, Vertex)들이 선(Edge)으로 연결된 복잡한 네트워크 구조입니다.

- **트리와의 차이**: 트리는 위아래가 명확한 부모-자식 관계가 있지만, 그래프는 누구나 누구와도 연결될 수 있고 순환(Cycle)이 있을 수 있습니다.
- **게임에서의 활용**:
  - **길 찾기(Pathfinding)**: 타일 하나하나가 노드이고, 서로 인접한 타일들이 연결된 선입니다.
  - **스킬 관계**: 특정 스킬을 얻었을 때 다른 스킬들이 해금되는 복합적인 관계도.

### 2. BFS (너비 우선 탐색) 알고리즘
BFS는 시작점에서 가까운 곳부터 "물결이 퍼져나가듯이" 탐색하는 방법입니다.

- **일상 비유**: 어두운 곳에서 손전등을 비추며 내 주변부터 훑어보는 탐색.
- **활용**: 턴제 게임에서 캐릭터가 **"이번 턴에 이동할 수 있는 모든 칸"**을 구할 때 가장 많이 사용됩니다.

---

## 💻 실습 예제

**미션:** 2D 타일 맵 환경에서 BFS 알고리즘을 사용하여 현재 위치에서 특정 거리(Move Range) 이내에 있는 모든 타일을 찾는 코드를 작성하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using System.Collections.Generic;

public class TileRangeFinder : MonoBehaviour
{
    // 타일 위치 정보를 나타내는 간단한 구조체
    public struct Vector2Int
    {
        public int x, y;
        public Vector2Int(int x, int y) { this.x = x; this.y = y; }
    }

    public int moveRange = 3; // 이동 사거리

    public void FindWalkableTiles(Vector2Int startPos)
    {
        // 1. BFS를 위한 큐와 방문 기록(Set/Dictionary) 준비
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, int> visited = new Dictionary<Vector2Int, int>();

        queue.Enqueue(startPos);
        visited[startPos] = 0; // 시작점의 거리는 0

        Debug.Log($"[BFS 시작] 현재 위치: ({startPos.x}, {startPos.y})");

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int currentDistance = visited[current];

            // 최대 사거리에 도달하면 더 이상 탐색하지 않음
            if (currentDistance >= moveRange) continue;

            // 2. 인접한 4방향 타일 체크 (상, 하, 좌, 우)
            Vector2Int[] neighbors = {
                new Vector2Int(current.x, current.y + 1),
                new Vector2Int(current.x, current.y - 1),
                new Vector2Int(current.x - 1, current.y),
                new Vector2Int(current.x + 1, current.y)
            };

            foreach (var next in neighbors)
            {
                // 아직 방문하지 않은 타일만 추가
                if (!visited.ContainsKey(next))
                {
                    visited[next] = currentDistance + 1;
                    queue.Enqueue(next);
                    Debug.Log($"갈 수 있는 타일 발견: ({next.x}, {next.y}), 거리: {visited[next]}");
                }
            }
        }

        Debug.Log($"총 {visited.Count}개의 타일 탐색 완료!");
    }

    void Start()
    {
        FindWalkableTiles(new Vector2Int(0, 0));
    }
}
```

</details>

---

## ✍️ 정리 및 퀴즈

1. **질문**: BFS(너비 우선 탐색)를 구현할 때 가장 핵심이 되는 자료구조는 무엇인가요? (힌트: 먼저 들어온 타일을 먼저 검사해야 합니다.)
2. **질문**: 그래프에서 '순환(Cycle)'이 발생할 경우, 알고리즘이 무한 루프에 빠지지 않게 하려면 어떤 처리가 필요할까요?
3. **질문**: BFS는 물결처럼 퍼져나가는 방식입니다. 만약 시작점에서 도착점까지의 **최단 거리**를 구하고 싶을 때 왜 BFS가 적합한지 설명해 보세요.
