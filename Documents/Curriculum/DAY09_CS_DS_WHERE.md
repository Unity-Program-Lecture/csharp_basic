# 🚀 Day 09: 자료구조와 제약 조건 (순서와 문지기)

오늘의 목표는 "**데이터를 넣고 빼는 순서가 정해진 Stack, Queue를 배우고, 제네릭에 문지기를 세우는 where 절을 마스터한다**"입니다.

---

## 1. Stack (스택): "접시 쌓기"
- **특징**: 나중에 들어온 것이 먼저 나갑니다. (**LIFO**: Last-In First-Out)
- **메소드**: `Push`(넣기), `Pop`(빼기)
- **비유**: 웹 브라우저의 '뒤로 가기' 버튼, 상자에 쌓인 카드 더미.

---

## 2. Queue (큐): "줄 서기"
- **특징**: 먼저 들어온 것이 먼저 나갑니다. (**FIFO**: First-In First-Out)
- **메소드**: `Enqueue`(넣기), `Dequeue`(빼기)
- **비유**: 식당 대기 줄, 프린터 인쇄 대기 목록.

---

## 3. 제네릭 제약 조건 (where): "만능 틀의 문지기"
제네릭 `<T>`는 무엇이든 올 수 있지만, 때로는 "이런 특징을 가진 놈만 들어와!"라고 제한해야 할 때가 있습니다.

- **`where T : struct`**: T는 반드시 **값 형식(숫자, 구조체 등)**이어야 함.
- **`where T : class`**: T는 반드시 **참조 형식(클래스 등)**이어야 함.
- **`where T : 부모클래스/인터페이스`**: 특정 부모를 가졌거나 약속을 지킨 놈만 가능.

---

## 💻 실습 예제: 값 형식만 받는 출력기
**미션:** `where T : struct` 제약 조건을 사용하여, 숫자나 구조체 같은 값 형식 데이터만 출력하는 메소드를 만들어 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

class Program
{
    // 문지기: T는 반드시 값 형식(struct)이어야 한다!
    static void PrintValue<T>(T data) where T : struct
    {
        Console.WriteLine($"값 형식 데이터: {data}");
    }

    static void Main()
    {
        PrintValue(100);    // 성공! (int는 struct)
        PrintValue(true);   // 성공! (bool은 struct)
        
        // PrintValue("안녕"); // 에러! (string은 class라 문지기가 막음)
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. "가장 먼저 들어온 데이터가 가장 먼저 나가는" 자료구조의 이름은?
2. 스택(Stack)에서 데이터를 뺄 때 사용하는 메소드 이름은?
3. 제네릭에서 T가 반드시 클래스여야 한다고 제한할 때 사용하는 코드는?
