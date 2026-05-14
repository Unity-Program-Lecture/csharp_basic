# 🚀 Day 10: 게임 자료구조 - 선형 구조 (List, Stack, Queue)

오늘의 목표는 "**데이터를 줄지어 보관하는 선형 자료구조의 특징을 이해하고, 게임의 각 상황에 맞는 최적의 보관함을 선택한다**"입니다.

---

## 1. 배열(Array) vs 연결 리스트(Linked List)
가장 기본이 되는 두 저장 방식의 차이를 아는 것이 중요합니다.

- **배열 (Array)**: 
  - 메모리가 **연속적**입니다. 
  - **인덱스 번호**로 즉시 접근이 가능해 검색이 매우 빠릅니다. 
  - 중간에 데이터를 끼워 넣거나 삭제할 때 뒤의 데이터들을 모두 밀어야 하므로 느립니다.
- **연결 리스트 (Linked List)**:
  - 메모리가 흩어져 있고 각 노드가 다음 노드의 **주소**를 가집니다.
  - 삽입/삭제 시 주소만 바꿔주면 되므로 매우 빠릅니다.
  - 특정 데이터를 찾으려면 처음부터 줄을 따라가야 하므로 검색이 느립니다.

---

## 2. 스택(Stack)과 큐(Queue)

### 📍 스택 (Stack): "접시 쌓기"
- **특징**: LIFO (Last-In, First-Out). 나중에 넣은 게 먼저 나옵니다.
- **용도**: 게임 메뉴 UI (뒤로 가기), 스킬 캔슬 히스토리.

### 📍 큐 (Queue): "줄 서기"
- **특징**: FIFO (First-In, First-Out). 먼저 넣은 게 먼저 나옵니다.
- **용도**: 로딩 대기열, 메시지 로그 알림창, 오브젝트 풀링.

---

## 💻 실습 예제: 큐(Queue)를 이용한 알림 메시지 시스템
```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 최신 인풋 시스템

public class MessageSystem : MonoBehaviour
{
    private Queue<string> messageQueue = new Queue<string>();

    void Start()
    {
        messageQueue.Enqueue("퀘스트를 수락했습니다.");
        messageQueue.Enqueue("경험치 100 획득!");
        messageQueue.Enqueue("레벨업!!");
    }

    void Update()
    {
        // 마우스 클릭 시 하나씩 꺼내기 (Input System 방식)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (messageQueue.Count > 0)
            {
                string msg = messageQueue.Dequeue();
                Debug.Log($"<color=cyan>[알림]</color> {msg}");
            }
            else
            {
                Debug.Log("더 이상 표시할 메시지가 없습니다.");
            }
        }
    }
}
```

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 먼저 들어간 데이터가 가장 먼저 나오는(FIFO) 자료구조의 명칭은?
   - **정답:** 큐 (Queue)
2. **문제:** 메모리 주소가 연속적으로 배치되어 인덱스를 통한 직접 접근이 가능한 자료구조는?
   - **정답:** 배열 (Array)
