# 🔑 [0803020526_18v4] 게임 알고리즘 단답형 평가 및 실기 과제 정답지

* **과정명**: (게임콘텐츠제작) 유니티(Unity) 프로그래밍 전문가 양성과정
* **교과목명**: 게임 알고리즘
* **평가일**: 2026년 06월 19일 (본평가)
* **훈련교사**: 차호정

---

## Part 1: 단답형 평가 문제 정답 및 채점 기준 (10문항)

각 문항은 **배점 5점씩 총 50점**으로 구성됩니다. 핵심 키워드나 수식이 누락된 경우 부분 점수(2~3점)를 부여할 수 있습니다.

### [게임 수학 영역]

#### Q1. Vector 뺄셈 연산의 기하학적 의미와 공식
* **정답**:
  * 점 $A$에서 점 $B$를 향하는 방향 벡터 식: $B - A$
  * 몬스터(Self)가 플레이어(Target)를 추적하는 최종 방향 벡터 공식: $\text{Target} - \text{Self}$
* **채점 기준**:
  * 뺄셈의 순서가 정확히 "**Target - Self**" 또는 "**목적지 - 출발지**" 형태로 기술되어야 100% 인정합니다. 반대로 적은 경우($\text{Self} - \text{Target}$)는 0점 처리합니다.

---

#### Q2. 벡터의 정규화(Normalization)와 단위 벡터
* **정답**:
  * 크기를 1로 만드는 연산: **정규화 (Normalization)**
  * 유니티 C# 읽기 전용 속성명: `normalized` (또는 `Vector3.normalized`)
* **채점 기준**:
  * '정규화' 용어와 `normalized` 속성명이 대소문자 구분 없이 올바르게 작성되면 정답 인정합니다. (`Normalize()` 함수는 벡터 자체를 수정하는 메서드이므로, '읽기 전용 속성'을 물어본 본 문항에서는 `normalized` 속성을 명시해야 정답입니다.)

---

#### Q3. 3D 공간 변환과 행렬(Matrix) 연산의 순서
* **정답**:
  * $\text{Local Space} \rightarrow \text{World Space} \rightarrow \text{View Space (또는 Camera Space)} \rightarrow \text{Projection Space (또는 Screen/Clip Space)}$
* **채점 기준**:
  * 네 가지 변환 단계가 순서대로 나열되어야 합니다. (행렬 약어인 **L ➡️ W ➡️ V ➡️ P** 또는 **M ➡️ V ➡️ P**의 개념적 흐름을 명확히 인지하고 있으면 정답 인정)

---

#### Q4. 3D 회전 제어의 문제점과 해결책
* **정답**:
  * 회전 자유도를 한 축 잃어버리는 현상: **짐벌락 (Gimbal Lock)**
  * 구면 선형 보간 함수명: `Quaternion.Slerp`
* **채점 기준**:
  * '짐벌락'과 `Slerp` (또는 `Quaternion.Slerp`) 단어가 정확하게 들어가야 인정됩니다. 선형 보간인 `Lerp`와 혼동한 경우 부분점수 2점을 부여합니다.

---

#### Q5. 카메라 시야 범위 기반 렌더링 최적화 기법
* **정답**:
  * **절두체 컬링 (Frustum Culling)**
* **채점 기준**:
  * '절두체 컬링', '뷰 프러스텀 컬링', 또는 영어 명칭 'Frustum Culling'이 기재되면 정답 인정합니다. 단순 '컬링(Culling)'은 오답 또는 1점 감점 처리합니다.

---

### [게임 물리 영역]

#### Q6. 뉴턴의 제2법칙과 가속도 물리 제어
* **정답**:
  * 뉴턴의 운동 제2법칙 공식: $F = ma$ (또는 $a = F/m$)
  * 순간 충격을 가하는 ForceMode 열거형 상수: `ForceMode.Impulse`
* **채점 기준**:
  * 공식 $F=ma$와 `ForceMode.Impulse`가 정확히 매핑되어야 합니다. 지속적인 힘인 `ForceMode.Force`를 적은 경우 2점 감점합니다.

---

#### Q7. 반발 계수(e)에 따른 충돌 유형 구분
* **정답**:
  * 반발 계수 $e = 1$ 충돌 유형: **완전 탄성 충돌 (Elastic Collision)**
  * 반발 계수 $e = 0$ 충돌 유형: **완전 비탄성 충돌 (Perfectly Inelastic Collision)**
* **채점 기준**:
  * 각각 '완전 탄성 충돌'과 '완전 비탄성 충돌'이 정확한 순서대로 기재되어야 합니다. '탄성 충돌'과 '비탄성 충돌'로만 적은 경우 각각 1점씩 감점합니다.

---

#### Q8. 발사체 포물선 운동과 속도 감쇠 요소
* **정답**:
  * 곡선 궤적 운동 명칭: **포물선 운동 (Parabolic Motion)**
  * 속도 감쇠를 일으키는 물리적 용어: **항력 (Drag)** 또는 **공기 저항 (Air Resistance)**
* **채점 기준**:
  * '포물선 운동'과 '항력/공기 저항'이 모두 언급되어야 합니다. `Rigidbody`의 속성명인 `drag`로 적어도 정답 인정합니다.

---

### [자료구조 및 알고리즘 영역]

#### Q9. 딕셔너리(Dictionary)와 리스트(List)의 조회 성능 비교
* **정답**:
  * `Dictionary<Key, Value>` 데이터 조회 시간 복잡도: $O(1)$
  * `List<T>` 순차 탐색 시간 복잡도: $O(N)$
* **채점 기준**:
  * 빅오 표기법 $O(1)$과 $O(N)$이 정확히 대조되어 매핑되어야 합니다. 표기법 괄호 안의 문자나 형태가 다르면 부분 점수 처리합니다.

---

#### Q10. A* 길찾기 알고리즘의 평가 함수 구성
* **정답**:
  * $G$의 의미 (출발지부터 현재 노드까지의 비용): **실제 비용 (또는 누적 비용 / 경과 비용)**
  * $H$의 의미 (현재 노드부터 목적지까지의 예측 비용): **휴리스틱 비용 (또는 예측 비용 / 추정 비용)**
* **채점 기준**:
  * $G$는 실제 축적된 물리적/수학적 이동 비용을 뜻하며, $H$는 휴리스틱(Heuristic) 척도에 근거한 예측 거리를 뜻하므로, 이 두 개념적 용어가 올바르게 매칭되면 정답 인정합니다.

---
---

## Part 2: 평가자 체크리스트 실기 과제 모범 구현 가이드 (50점)

실기 과제는 기획서 해독, 수학/물리 포뮬러의 스크립트 이식성, UGUI 인터페이스 이벤트 상속 무결성, A* 길찾기 AI의 결함 방어를 기준으로 종합 채점합니다.

### 1. Task 1 & 2: 발사체 포물선 궤적 및 물리 시뮬레이션 모범 스크립트
유니티 6 내장 물리 엔진에 의존하지 않고, 오일러 적분(Euler Integration) 기반으로 프레임 단위 중력 및 공기 저항(선형 감쇠) 공식을 구현한 C# 코드입니다.

```csharp
using UnityEngine;

public class ProjectileSimulator : MonoBehaviour
{
    public Vector3 velocity;      // 초기 발사 속도
    public float gravity = 9.81f; // 중력 가속도
    public float drag = 0.1f;     // 공기 저항 계수 (선형 감쇠)
    public float elasticity = 0.7f; // 반발 계수 (e)

    void Update()
    {
        // 1. 공기 저항(항력)에 의한 감쇠력 연산: F_drag = -drag * velocity
        Vector3 dragForce = -drag * velocity;
        
        // 2. 가속도 연산: a = g + (F_drag / m)  [질량 m = 1로 가정]
        Vector3 acceleration = new Vector3(0, -gravity, 0) + dragForce;
        
        // 3. 오일러 적분을 통한 속도 및 위치 갱신
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        
        // 4. 지면 충돌 검사 및 반발 계수 기반 속도 반사 (y=0 지면 기준)
        if (transform.position.y <= 0)
        {
            Vector3 pos = transform.position;
            pos.y = 0;
            transform.position = pos;
            
            // 반발 계수 e를 적용하여 y축 속도 반전 및 감쇠, x/z축 속도 마찰 감쇠
            velocity.y = -velocity.y * elasticity;
            velocity.x *= 0.9f; // 지면 마찰
            velocity.z *= 0.9f;
            
            // 속도가 임계값 이하로 떨어지면 물리 연산 중단 (안정성 가드)
            if (velocity.magnitude < 0.1f)
            {
                velocity = Vector3.zero;
            }
        }
    }
}
```

---

### 2. Task 3: UGUI 커스텀 인벤토리 슬롯 포인터 이벤트 스크립트
UGUI Canvas 환경 하에서 TMP와 `IPointerEnterHandler`, `IPointerExitHandler` 인터페이스 상속을 이용한 호버 이벤트 모범 답안입니다.

```csharp
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private GameObject tooltipWindow;
    [SerializeField] private TextMeshProUGUI tooltipText;
    
    private int itemID;
    private string itemDescription;

    public void SetupSlot(int id, string name, string desc)
    {
        itemID = id;
        itemNameText.text = name;
        itemDescription = desc;
        tooltipWindow.SetActive(false);
    }

    // 마우스 호버 진입 시 호출 (EventSystem 기반 인터랙션)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipWindow != null)
        {
            tooltipText.text = $"[ID: {itemID}] {itemDescription}";
            tooltipWindow.SetActive(true);
        }
    }

    // 마우스 호버 이탈 시 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipWindow != null)
        {
            tooltipWindow.SetActive(false);
        }
    }
}
```

---

### 3. Task 4: A* 알고리즘 탐색 및 무한 루프 예외 차단 디버깅 가이드
A* 루프 동작 시 예기치 못한 막힌 맵이나 가중치 모순으로 인해 연산이 정지하지 않는 결함을 예방하는 `Safety Loop Limit`가 포함된 모범 로직 설계 스니펫입니다.

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    private const int SAFETY_LOOP_LIMIT = 5000; // 무한 루프 방지용 최대 안전 한계치

    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        
        openList.Add(startNode);
        int safetyCounter = 0;

        while (openList.Count > 0)
        {
            safetyCounter++;
            // 런타임 크래시 방어적 예외 처리 (Infinite Loop 차단)
            if (safetyCounter > SAFETY_LOOP_LIMIT)
            {
                Debug.LogError($"[A*] Pathfinding aborted: Loop limit ({SAFETY_LOOP_LIMIT}) exceeded to prevent runtime crash!");
                break;
            }

            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || 
                    (openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // 목적지 도달 성공
            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (neighbor.isWall || closedList.Contains(neighbor)) continue;

                // 타일 이동 가중치(늪지대=3, 일반=1)를 반영하여 G 비용 계산
                float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor) * neighbor.movementPenalty;
                if (newMovementCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode); // Manhattan or Euclidean Distance
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null; // 경로 탐색 실패 시 안전하게 null 반환 (예외 안전 설계)
    }

    private float GetDistance(Node nodeA, Node nodeB)
    {
        // 맨해튼 거리 측정 (Manhattan Distance): 격자형 타일 맵에 최적화
        return Mathf.Abs(nodeA.gridX - nodeB.gridX) + Mathf.Abs(nodeA.gridY - nodeB.gridY);
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // Parent 참조 역추적 시 NullReferenceException 방어 코드
        while (currentNode != startNode && currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    // 예시용 노드 구조체 및 인접 노드 획득용 목업 메서드들
    public class Node {
        public bool isWall;
        public int gridX;
        public int gridY;
        public float gCost;
        public float hCost;
        public float movementPenalty; // 늪지대 등 가중치
        public Node parent;
        public float fCost => gCost + hCost;
    }
    private List<Node> GetNeighbors(Node n) => new List<Node>();
}
```
