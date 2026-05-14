# 🚀 Day 14: 게임 알고리즘 - 탐색과 길찾기 (BFS & DFS)

오늘의 목표는 "**그래프 공간에서 목표 지점을 찾기 위한 기초 알고리즘인 너비 우선 탐색(BFS)과 깊이 우선 탐색(DFS)의 원리를 이해한다**"입니다.

---

## 1. 그래프 순회 (Graph Traversal)
지하철 노선도와 같은 그래프 구조에서 모든 정점(Node)을 한 번씩 방문하는 방법입니다.

### 📍 BFS (Breadth-First Search, 너비 우선 탐색)
- **비유**: 퍼져나가는 물결. 시작점에서 가까운 이웃부터 차례대로 방문합니다.
- **도구**: **큐(Queue)** 자료구조를 사용합니다.
- **특징**: **최단 경로**를 보장합니다.

### 📍 DFS (Depth-First Search, 깊이 우선 탐색)
- **비유**: 미로 찾기. 한 방향으로 갈 수 있는 데까지 끝까지 갔다가 막히면 되돌아옵니다.
- **도구**: **스택(Stack)**이나 **재귀 함수**를 사용합니다.
- **특징**: 모든 경로를 끝까지 훑어야 할 때 유리하지만, 최단 경로를 보장하지는 않습니다.

---

## 2. 게임 실무에서의 활용
- **BFS**: 타일 기반 게임(SRPG)에서 이동 가능한 범위 표시, 최단 거리 길찾기.
- **DFS**: 미로 생성 알고리즘, 복잡한 퍼즐의 해답 찾기.

---

## 💻 실습 예제: 큐(Queue)를 이용한 BFS 탐색 로직 (슈도 코드)
실제 길찾기 엔진이 어떻게 이웃 타일을 방문하는지 로직의 흐름을 이해해 봅니다.

```csharp
// [BFS 탐색 흐름]
void BFS_Example(Node startNode)
{
    Queue<Node> queue = new Queue<Node>();
    List<Node> visited = new List<Node>();

    queue.Enqueue(startNode);
    visited.Add(startNode);

    while (queue.Count > 0)
    {
        Node current = queue.Dequeue();
        Debug.Log($"현재 위치 방문: {current.name}");

        // 연결된 이웃들 확인
        foreach (Node neighbor in current.neighbors)
        {
            if (!visited.Contains(neighbor))
            {
                visited.Add(neighbor);
                queue.Enqueue(neighbor);
            }
        }
    }
}
```

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 시작점에서 가장 가까운 노드들부터 순차적으로 탐색하며, 최단 경로를 찾기에 적합한 알고리즘은?
   - **정답:** BFS (너비 우선 탐색)
2. **문제:** BFS(너비 우선 탐색)를 구현할 때 방문할 노드 목록을 관리하기 위해 사용하는 자료구조는?
   - **정답:** 큐 (Queue)
