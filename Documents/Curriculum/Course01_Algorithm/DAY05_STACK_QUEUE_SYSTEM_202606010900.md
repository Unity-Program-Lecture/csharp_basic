# 🚀 [알고리즘 05] 데이터의 질서: 스택(Stack)과 큐(Queue) 시스템

학습 목표: 게임의 UI 시스템과 명령 처리의 핵심인 스택과 큐의 구조를 이해하고, 유니티의 뒤로가기(Back) 기능과 퀘스트/메시지 큐 시스템을 구현해 봅니다.

---

## 💡 개념 설명 (NCS 알고리즘: 선형 자료구조 활용)

### 1. 스택(Stack)이란 무엇인가요?
스택은 '쌓아 올린 접시'와 같습니다. 가장 나중에 쌓은 접시를 가장 먼저 꺼내는 구조입니다. (LIFO: Last-In, First-Out)

- **일상 비유**: 인터넷 브라우저의 '뒤로 가기' 버튼이나 문서 작업의 '되돌리기(Undo)' 기능.
- **게임에서의 활용**: 
  - **UI 창 관리**: 여러 개의 메뉴 창을 띄웠을 때, 'ESC'를 누르면 가장 최근에 연 창부터 닫히는 시스템.
  - **상태 관리**: 캐릭터의 동작 상태(Idle -> Walk -> Run)를 쌓아두고 이전 상태로 복구할 때.

### 2. 큐(Queue)란 무엇인가요?
큐는 '은행의 대기 줄'과 같습니다. 먼저 줄을 선 사람이 먼저 업무를 보는 구조입니다. (FIFO: First-In, First-Out)

- **일상 비유**: 편의점의 선입선출(먼저 들어온 우유를 앞에 진열) 방식.
- **게임에서의 활용**:
  - **서버 메시지 처리**: 플레이어들의 패킷을 들어온 순서대로 처리할 때.
  - **대화 시스템/퀘스트 알림**: 화면 하단에 차례대로 나타났다가 사라지는 알림창 메시지들.
  - **명령 큐**: 전략 게임에서 유닛에게 내린 여러 명령을 순서대로 수행할 때.

---

## 💻 실습 예제

**미션:** 유니티에서 여러 개의 UI 패널을 스택(Stack)으로 관리하여 '뒤로 가기' 기능을 구현하고, 알림 메시지를 큐(Queue)로 관리하여 순차적으로 출력하는 시스템을 만드세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using System.Collections.Generic;

public class GameSystemManager : MonoBehaviour
{
    // 1. UI 스택 (LIFO)
    private Stack<GameObject> uiStack = new Stack<GameObject>();
    
    // 2. 알림 큐 (FIFO)
    private Queue<string> messageQueue = new Queue<string>();

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        uiStack.Push(panel); // 스택에 추가
        AddLog($"패널 오픈: {panel.name}");
    }

    public void CloseLastPanel()
    {
        if (uiStack.Count > 0)
        {
            GameObject lastPanel = uiStack.Pop(); // 가장 최근 패널 꺼내기
            lastPanel.SetActive(false);
            AddLog($"패널 닫음: {lastPanel.name}");
        }
    }

    public void AddMessage(string msg)
    {
        messageQueue.Enqueue(msg); // 큐에 메시지 추가
        ProcessNextMessage();
    }

    private void ProcessNextMessage()
    {
        if (messageQueue.Count > 0)
        {
            string currentMsg = messageQueue.Dequeue(); // 가장 오래된 메시지 꺼내기
            Debug.Log($"[알림창 출력]: {currentMsg}");
        }
    }

    private void AddLog(string log) => Debug.Log($"<color=cyan>{log}</color>");

    void Update()
    {
        // ESC 키를 누르면 마지막에 열린 UI를 닫음
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseLastPanel();
        }
    }
}
```

</details>

---

## ✍️ 정리 및 퀴즈

1. **질문**: RTS 게임에서 유닛에게 '이동' 후 '공격' 명령을 예약했을 때, 어떤 자료구조를 사용하는 것이 가장 자연스러울까요? 그 이유는 무엇인가요?
2. **질문**: 스택의 `Push`와 `Pop`은 각각 어떤 동작을 의미하나요? 
3. **질문**: 왜 UI 시스템에서 `List`보다 `Stack`을 사용하는 것이 구조적으로 더 안전하고 편리할까요?
