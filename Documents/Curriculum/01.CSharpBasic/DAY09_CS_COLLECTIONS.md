# 🚀 Day 09: 컬렉션과 자료구조 (만능 바구니와 정돈된 데이터)

오늘의 목표는 "**데이터를 효율적으로 관리하는 List, Dictionary와 순서가 정해진 Stack, Queue를 마스터한다**"입니다.

---

## 1. List<T>: "늘어나는 배열"
데이터가 들어오는 대로 크기가 자동으로 늘어나는 가변 배열입니다.

### 📏 주요 도구
- **`Add(T)`**: 데이터 추가 (맨 뒤에 넣기)
- **`Insert(index, T)`**: 중간에 끼워 넣기 (특정 번호에 새치기)
- **`Remove(T)`**: 데이터 삭제 (이름이나 값으로 지우기)
- **`RemoveAt(index)`**: 특정 위치 삭제 (방 번호로 지우기)
- **`Contains(T)`**: 데이터가 들어있는지 확인 (참/거짓)
- **`Clear()`**: 바구니 통째로 비우기
- **`Count`**: 데이터 개수 확인 (프로퍼티)

```csharp
List<string> inventory = new List<string>();

inventory.Add("빨간 포션");      // ["빨간 포션"]
inventory.Insert(0, "황금 검");  // ["황금 검", "빨간 포션"] (0번에 끼워넣기)

if (inventory.Contains("빨간 포션")) // 들어있는지 확인
{
    Debug.Log($"현재 아이템 개수: {inventory.Count}"); // 개수 확인
}

inventory.RemoveAt(1);           // 1번(빨간 포션) 삭제 -> ["황금 검"]
inventory.Clear();               // 모두 비우기 -> []
```

---

## 2. Dictionary<K, V>: "이름표가 있는 바구니"
번호 대신 내가 정한 이름표(Key)로 데이터를 찾는 바구니입니다.

### 📏 주요 도구
- **`Add(K, V)`**: 새로운 이름표와 데이터 추가
- **`ContainsKey(K)`**: 특정 이름표가 있는지 확인 (에러 방지용 필수!)
- **`TryGetValue(K, out V)`**: 안전하게 데이터 꺼내기 (이름표가 없어도 에러 안 남)
- **`Remove(K)`**: 이름표 떼기 (데이터 삭제)
- **`Clear()`**: 목록 초기화
- **`Count`**: 등록된 데이터 개수

```csharp
Dictionary<string, int> itemCounts = new Dictionary<string, int>();

itemCounts.Add("포션", 5);
itemCounts["슬라임 점액"] = 10; // 인덱서로 추가/수정 가능

// 안전하게 꺼내는 방법 (추천!)
if (itemCounts.TryGetValue("포션", out int count)) 
{
    Debug.Log($"포션 개수: {count}");
}

if (itemCounts.ContainsKey("검")) // 키가 있는지 확인
{
    itemCounts.Remove("검");     // 삭제
}

Debug.Log($"등록된 품목 수: {itemCounts.Count}");
itemCounts.Clear(); // 전체 삭제
```

---

## 3. Stack (스택): "접시 쌓기"
나중에 들어온 것이 먼저 나갑니다. (LIFO - Last In First Out)

### 📏 주요 도구
- **`Push(T)`**: 데이터 쌓기 (맨 위에 올리기)
- **`Pop()`**: 데이터 꺼내기 (맨 위에서 가져오고 삭제)
- **`Peek()`**: 맨 위 확인 (보기만 하고 삭제는 안 함)
- **`Clear()`**: 모두 비우기
- **`Count`**: 접시가 몇 장 쌓였나?

```csharp
Stack<string> pages = new Stack<string>();

pages.Push("메인화면");
pages.Push("설정");
pages.Push("그래픽 설정");

Debug.Log($"현재 쌓인 페이지: {pages.Count}"); // 3개
Debug.Log($"맨 위 확인: {pages.Peek()}");     // "그래픽 설정" (보기만 함)

string popPage = pages.Pop();                 // "그래픽 설정" (꺼내기)
Debug.Log($"꺼낸 후 맨 위: {pages.Peek()}");    // "설정"

pages.Clear(); // 스택 비우기
```

---

## 4. Queue (큐): "줄 서기"
먼저 들어온 것이 먼저 나갑니다. (FIFO - First In First Out)

### 📏 주요 도구
- **`Enqueue(T)`**: 줄 서기 (맨 뒤에 추가)
- **`Dequeue()`**: 나가기 (맨 앞사람 내보내고 데이터 반환)
- **`Peek()`**: 맨 앞 확인 (나갈 차례가 누군지 보기만 함)
- **`Clear()`**: 모두 비우기
- **`Count`**: 줄이 얼마나 긴가?

```csharp
Queue<string> waitList = new Queue<string>();

waitList.Enqueue("1번 유저");
waitList.Enqueue("2번 유저");
waitList.Enqueue("3번 유저");

Debug.Log($"대기 인원: {waitList.Count}"); // 3명
Debug.Log($"다음 차례: {waitList.Peek()}"); // "1번 유저" (보기만)

string nextUser = waitList.Dequeue();       // "1번 유저" (나감)
Debug.Log($"현재 대기: {waitList.Peek()}");  // "2번 유저"

waitList.Clear(); // 대기열 초기화
```

---

## 💻 실습 예제: 게임 시스템 통합 관리
```csharp
using UnityEngine;
using System.Collections.Generic;

public class Day09_Practice : MonoBehaviour
{
    void Start()
    {
        // --- 1. List: 인벤토리 정렬 및 관리 ---
        List<string> bag = new List<string> { "목검", "빵", "물" };
        bag.Insert(1, "강화석"); // 1번 위치에 강화석 끼워넣기
        bag.Remove("물");        // 물 삭제
        
        Debug.Log($"[List] 현재 가방 아이템 수: {bag.Count}");
        foreach(string item in bag) Debug.Log($"- 소지품: {item}");


        // --- 2. Dictionary: 몬스터 정보 즉시 찾기 ---
        Dictionary<string, int> monsterHp = new Dictionary<string, int>();
        monsterHp["Slime"] = 50;
        monsterHp["Goblin"] = 120;

        // TryGetValue를 이용한 안전한 접근
        if (monsterHp.TryGetValue("Slime", out int hp))
        {
            Debug.Log($"[Dict] 슬라임의 체력은 {hp}입니다.");
        }


        // --- 3. Queue: 던전 입장 대기열 처리 ---
        Queue<string> entranceQueue = new Queue<string>();
        entranceQueue.Enqueue("전사A");
        entranceQueue.Enqueue("마법사B");

        Debug.Log($"[Queue] 다음 입장 예정자: {entranceQueue.Peek()}");
        Debug.Log($"[Queue] {entranceQueue.Dequeue()} 입장 완료!");


        // --- 4. Stack: UI 메뉴 뒤로가기 시스템 ---
        Stack<string> uiHistory = new Stack<string>();
        uiHistory.Push("메인화면");
        uiHistory.Push("인벤토리");
        uiHistory.Push("상세정보");

        Debug.Log($"[Stack] 현재 화면: {uiHistory.Peek()}");
        Debug.Log($"[Stack] '{uiHistory.Pop()}' 닫음 -> 이전 화면 '{uiHistory.Peek()}'으로 이동");
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
