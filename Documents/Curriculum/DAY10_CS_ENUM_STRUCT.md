# 🚀 Day 10: 값 형식의 깊이 (Enum, Struct, Boxing)

오늘의 목표는 "**가독성을 높이는 열거형(Enum)을 익히고, 값 형식과 참조 형식을 넘나드는 Boxing/Unboxing의 위험성을 이해한다**"입니다.

---

## 1. 열거형(Enum): "숫자에 붙인 이름표"
컴퓨터가 좋아하는 숫자 대신 사람이 좋아하는 이름으로 상태를 관리합니다.
- **비유**: 리모컨의 11번 채널 대신 'MBC'라고 부르는 것.
- **장점**: 오타를 방지하고 코드가 읽기 편해집니다.

---

## 2. 구조체(Struct): "가볍고 빠른 상자"
클래스와 비슷하지만 스택(Stack) 메모리를 사용하는 값 형식입니다.
- **특징**: 상속이 불가능합니다. 크기가 작고 자주 쓰이는 데이터(좌표, 색상 등)에 적합합니다.

---

## 3. Boxing & Unboxing: "선물 포장"
- **Boxing**: 사과(값 형식)를 상자에 담아 창고(힙)로 보내는 것. (`int` -> `object`)
- **Unboxing**: 창고의 상자를 가져와 포장을 뜯고 사과를 꺼내는 것. (`object` -> `int`)
- **주의**: 포장을 뜯고 싸는 데는 비용(시간)이 듭니다. **너무 많이 쓰면 프로그램이 느려집니다!**

---

## 💻 실습 예제: 게임 상태 관리와 구조체
**미션:** 캐릭터의 상태를 `Enum`으로 관리하고, 위치 좌표를 `Struct`로 만들어 서로 대입했을 때 값이 어떻게 복사되는지 확인해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

enum GameState { Start, Play, End }

struct Position
{
    public int x, y;
}

class Program
{
    static void Main()
    {
        GameState state = GameState.Start;
        Console.WriteLine($"현재 상태: {state}");

        Position p1 = new Position { x = 10, y = 20 };
        Position p2 = p1; // 값 복사 (별개의 상자 생성)
        p2.x = 99;

        Console.WriteLine($"p1.x: {p1.x}, p2.x: {p2.x}"); // 결과: 10, 99 (원본 유지)
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. `int` 같은 값 형식을 `object` 타입으로 바꾸는 과정을 무엇이라 하나요?
2. 구조체(Struct)는 상속이 가능한가요?
3. `enum`의 각 항목은 내부적으로 어떤 데이터 타입으로 저장되나요? (기본값)
