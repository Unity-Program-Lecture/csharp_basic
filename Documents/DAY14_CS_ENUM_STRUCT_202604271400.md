# 🚀 14일차: 이름 붙인 숫자와 가벼운 상자 (Enum & Struct)

오늘의 목표는 **"가독성을 높여주는 열거형(Enum)을 배우고, 구조체(Struct)와 함께 값 형식(Value Type)의 특징을 마스터한다"**입니다.

---

## 1. 열거형(Enum): "숫자에 이름표 붙이기"
컴퓨터는 숫자(0, 1, 2...)를 좋아하지만, 사람은 이름(전사, 마법사, 궁수...)을 좋아합니다. 숫자에 이름을 붙여서 실수를 방지하는 기술입니다.

### 💡 왜 열거형을 쓰나요?
- **가독성**: `if (job == 0)`보다 `if (job == Job.Warrior)`가 훨씬 이해하기 쉽습니다.
- **안전성**: 엉뚱한 숫자(예: 999번 직업)가 들어오는 것을 막아줍니다.
- **비유**: 리모컨의 채널 번호 대신 'MBC', 'KBS'라고 부르는 것과 같습니다.

### 💻 실습 예제: 게임 캐릭터 상태 관리
**미션:** 캐릭터의 상태(대기, 이동, 공격, 사망)를 열거형으로 정의하고 출력해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

namespace Day14
{
    // 열거형 정의 (클래스 밖이나 안 모두 가능)
    enum State
    {
        Idle,   // 0
        Move,   // 1
        Attack, // 2
        Die     // 3
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            State myState = State.Idle;

            Console.WriteLine($"현재 상태: {myState}"); // 결과: Idle
            Console.WriteLine($"상태 번호: {(int)myState}"); // 결과: 0 (형변환 가능)

            if (myState == State.Idle)
            {
                Console.WriteLine("캐릭터가 쉬고 있습니다.");
            }
        }
    }
}
```

</details>

---

## 2. 구조체(Struct): "가벼운 데이터 꾸러미"
클래스와 비슷하지만 메모리 스택(Stack)에 직접 저장되는 작고 빠른 상자입니다.

### 💡 클래스 vs 구조체 핵심 요약
- **구조체**: **값 형식(Value Type)**. 복사할 때 데이터 자체가 복제됨. (시험지 복사본)
- **클래스**: **참조 형식(Reference Type)**. 복사할 때 주소만 복제됨. (집 주소 공유)

---

## 3. 사실 우리가 쓰던 것들의 비밀
- **구조체인 것들**: `int`, `float`, `bool`, `char`, `Vector3`, `Color`
- **클래스인 것들**: `string`, `Array`, `List`, `Monster`, `Player`

---

## 4. 심화: 제네릭의 문지기, `where`
오늘 배운 지식을 활용하면 제네릭 틀에 "값 형식(구조체)만 들어와!"라고 제한을 걸 수 있습니다.

- **`where T : struct`**: 구조체/열거형만 허용
- **`where T : class`**: 클래스만 허용

---

## ✍️ 14일차 핵심 퀴즈
1. 열거형(Enum)을 사용했을 때 얻을 수 있는 가장 큰 장점은 무엇인가요?
2. `int`는 구조체인가요, 클래스인가요?
3. `string`이 구조체가 아닌 클래스로 만들어진 이유는 무엇일까요? (비유: 크기가 변할 수 있어서)
4. 제네릭 문지기(`where`)에게 `int`만 통과시키고 싶다면 어떤 조건을 걸어야 할까요?
