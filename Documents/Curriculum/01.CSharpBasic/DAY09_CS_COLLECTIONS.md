# 🚀 Day 09: 컬렉션과 자료구조 (만능 바구니와 정돈된 데이터)

오늘의 목표는 "**데이터를 효율적으로 관리하는 List, Dictionary와 순서가 정해진 Stack, Queue를 마스터한다**"입니다.

---

## 1. List<T>: "늘어나는 배열"
데이터가 들어오는 대로 크기가 자동으로 늘어나는 가변 배열입니다.

### 📏 주요 도구
- **`Add(T)`**: 데이터 추가
- **`Remove(T)`**: 데이터 삭제
- **`Count`**: 데이터 개수 확인

```csharp
List<string> inventory = new List<string>();
inventory.Add("빨간 포션");
inventory.Remove("빨간 포션");
```

---

## 2. Dictionary<K, V>: "이름표가 있는 바구니"
번호 대신 내가 정한 이름표(Key)로 데이터를 찾는 바구니입니다.

```csharp
Dictionary<string, int> itemCounts = new Dictionary<string, int>();
itemCounts["포션"] = 5;

if (itemCounts.ContainsKey("포션")) 
{
    Debug.Log($"포션 개수: {itemCounts["포션"]}");
}
```

---

## 3. Stack (스택): "접시 쌓기"
나중에 들어온 것이 먼저 나갑니다. (LIFO)

```csharp
Stack<string> pages = new Stack<string>();
pages.Push("메인화면");
pages.Push("설정");

string back = pages.Pop(); // "설정"이 나옴
```

---

## 4. Queue (큐): "줄 서기"
먼저 들어온 것이 먼저 나갑니다. (FIFO)

```csharp
Queue<string> waitList = new Queue<string>();
waitList.Enqueue("1번 유저");
waitList.Enqueue("2번 유저");

string next = waitList.Dequeue(); // "1번 유저"가 나옴
```

---

## 💻 실습 예제: 인벤토리와 대기열 시스템
```csharp
using UnityEngine;
using System.Collections.Generic;

public class Day09_Practice : MonoBehaviour
{
    void Start()
    {
        // 1. 리스트 순회
        List<int> levels = new List<int> { 1, 5, 10 };
        foreach(int lv in levels) Debug.Log($"레벨: {lv}");

        // 2. 딕셔너리 활용
        Dictionary<string, string> nicknames = new Dictionary<string, string>();
        nicknames["Warrior"] = "전사왕";
        
        // 3. 스택/큐 혼합
        Queue<string> chatQueue = new Queue<string>();
        chatQueue.Enqueue("안녕하세요");
        Debug.Log($"채팅 처리: {chatQueue.Dequeue()}");
    }
}
```

---

## ✍️ 핵심 퀴즈
1. 리스트에서 데이터를 지울 때 사용하는 메소드는?
2. 딕셔너리에서 키가 존재하는지 확인할 때 쓰는 메소드는?
3. "가장 먼저 들어온 데이터가 가장 먼저 나가는" 자료구조의 이름은?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 8)]
**컬렉션**과 **자료구조**를 총동원하여 유연한 몬스터 관리 및 전투 로그 시스템을 구축합니다.

**[요구 사항]**
1. **몬스터 목록 (List):** `List<Monster> monsters`를 만들어 여러 마리의 몬스터를 관리하세요.
2. **이름 검색 (Dictionary):** `Dictionary<string, Monster>`를 사용하여 이름으로 특정 몬스터를 즉시 찾는 기능을 만듭니다.
3. **소환 대기열 (Queue):** `Queue<Monster> spawnQueue`에 몬스터를 넣고 하나씩 꺼내서 `monsters` 리스트에 추가합니다.
4. **전투 로그 (Stack):** 발생한 전투 메시지를 `Stack<string> logs`에 담고, 마지막에 최근 3개의 로그만 출력하세요.

**[프로그래밍 힌트]**
- `foreach`를 사용하여 리스트나 딕셔너리 전체를 훑어볼 수 있습니다.
- `while (spawnQueue.Count > 0)`을 사용하면 대기열이 빌 때까지 자동으로 처리할 수 있습니다.
