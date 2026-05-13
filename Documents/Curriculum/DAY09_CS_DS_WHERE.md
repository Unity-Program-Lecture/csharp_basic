# 🚀 Day 09: 자료구조와 제약 조건 (순서와 문지기)

오늘의 목표는 "**데이터를 넣고 빼는 순서가 정해진 Stack, Queue를 배우고, 제네릭에 문지기를 세우는 where 절을 마스터한다**"입니다.

---

## 1. Stack (스택): "접시 쌓기"
나중에 들어온 것이 먼저 나갑니다. (LIFO)

```csharp
Stack<string> pages = new Stack<string>();
pages.Push("네이버");
pages.Push("구글");

string lastPage = pages.Pop(); // "구글"이 나옴
```

---

## 2. Queue (큐): "줄 서기"
먼저 들어온 것이 먼저 나갑니다. (FIFO)

```csharp
Queue<string> waitList = new Queue<string>();
waitList.Enqueue("1번 손님");
waitList.Enqueue("2번 손님");

string next = waitList.Dequeue(); // "1번 손님"이 나옴
```

---

## 3. 제네릭 제약 조건 (where): "만능 틀의 문지기"
"이런 특징을 가진 놈만 들어와!"라고 제한하는 문구입니다.

```csharp
// T는 반드시 클래스여야 함
class Box<T> where T : class { }

// T는 반드시 특정 인터페이스를 구현해야 함
void Attack<T>(T target) where T : IDamageable { }
```

---

## 💻 실습 예제: 값 형식만 받는 출력기
```csharp
using UnityEngine;

public class Day09_Practice : MonoBehaviour
{
    // 문지기: T는 반드시 값 형식(struct)이어야 한다!
    void PrintValue<T>(T data) where T : struct
    {
        Debug.Log($"값 형식 데이터: {data}");
    }

    void Start()
    {
        PrintValue(100);    // 성공! (int는 struct)
        PrintValue(true);   // 성공! (bool은 struct)
        
        // PrintValue("안녕"); // 에러 발생 (string은 class)
    }
}
```

---

## ✍️ 핵심 퀴즈
1. "가장 먼저 들어온 데이터가 가장 먼저 나가는" 자료구조의 이름은?
2. 스택(Stack)에서 데이터를 뺄 때 사용하는 메소드 이름은?
3. 제네릭에서 T가 반드시 클래스여야 한다고 제한할 때 사용하는 코드는?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 8)]
**Stack, Queue** 자료구조와 **제네릭 제약 조건**을 활용하여 전투 로그와 소환 시스템을 구축합니다.

**[요구 사항]**
1. **소환 대기열 (Queue):** `Queue<Monster> spawnQueue`를 만드세요.
   - 3마리의 몬스터를 순서대로 `Enqueue` 하고, 하나씩 `Dequeue` 하여 필드에 소환(출력)합니다.
2. **전투 로그 (Stack):** `Stack<string> battleLogs`를 만드세요.
   - 전투 중 발생하는 일(데미지 입음, 처치됨 등)을 `Push` 합니다.
   - 모든 전투가 종료된 후, 최근 발생한 로그부터 5개만 `Pop` 하여 출력하세요.
3. **제약 조건 활용:** `void PrintTargetInfo<T>(T target) where T : IDamageable` 함수를 만듭니다.
   - `IDamageable` 인터페이스를 가진 대상만 인자로 받아 그 상태를 출력하는 기능을 담당합니다.

**[프로그래밍 힌트]**
- `Queue`는 "다음 나올 몬스터"를 미리 준비할 때 유용합니다.
- `Stack`은 "가장 최근의 사건"을 되짚어볼 때(Ctrl+Z 처럼) 주로 쓰입니다.
- `while (spawnQueue.Count > 0)`을 사용하면 큐가 빌 때까지 안전하게 데이터를 처리할 수 있습니다.

