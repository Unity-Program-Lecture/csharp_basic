# 🚀 2일차: 컴퓨터에게 똑똑하게 일 시키기 (제어 흐름 마스터)

오늘 수업의 목표는 **"내 코드에 들어있는 영어 단어(키워드)들이 어떤 명령을 내리는지 정확히 이해한다"**입니다.

---

## 1. break와 continue: "반복문의 흐름 제어"
어제 배운 `for`문이나 오늘 배울 `while`문 안에서 사용하는 "특수 명령"입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`break` (브레이크)**: 자동차 브레이크처럼 **"지금 당장 멈춰!"**라는 뜻입니다. 반복문이 몇 번 남았든 상관없이 그 자리에서 즉시 탈출합니다.
- **`continue` (컨티뉴)**: **"이번 판은 패스! 다음 판으로 넘어가자!"**라는 뜻입니다. 아래 코드는 무시하고 다음 반복 회차로 바로 점프합니다.

### 💻 실습 예제: 럭키 세븐 찾기
```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i <= 10; i++)
            {
                if (i % 2 == 0) 
                {
                    continue; // 짝수는 "패스!" (아래 출력문을 실행 안 함)
                }

                Console.WriteLine("홀수 확인: " + i);

                if (i == 7)
                {
                    Console.WriteLine("🎯 7을 찾았다! 종료!");
                    break; // 찾았으니까 "당장 멈춰!" (반복문 탈출)
                }
            }
        }
    }
}
```

---

## 2. switch, case, default: "깔끔한 메뉴 선택"
여러 갈래 길 중에서 하나를 고를 때 `if-else`보다 읽기 편하게 만드는 도구입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`switch` (스위치)**: 전등 스위치처럼 **"어디로 연결할까?"**라고 메뉴판을 펼치는 명령입니다.
- **`case` (케이스)**: **"~인 경우"**라는 뜻입니다. `case "1":`은 입력값이 "1"인 경우를 말합니다.
- **`default` (디폴트)**: **"나머지 전부"**라는 뜻입니다. 준비한 `case`들 중에 맞는 게 없을 때 마지막으로 찾아가는 곳입니다.

### 💻 실습 예제: 무기 상점
```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("무기를 선택하세요 (1.검 / 2.활): ");
            string input = Console.ReadLine();

            switch (input) // "입력값에 따라 스위치를 올린다!"
            {
                case "1": // "그 값이 1인 경우"
                    Console.WriteLine("⚔️ 검을 선택했습니다.");
                    break;
                case "2": // "그 값이 2인 경우"
                    Console.WriteLine("🏹 활을 선택했습니다.");
                    break;
                default: // "1도 아니고 2도 아닌 나머지 전부"
                    Console.WriteLine("❓ 다시 골라주세요.");
                    break;
            }
        }
    }
}
```

---

## 3. while과 do: "조건부 무한 반복"
정해진 횟수가 아니라, **"특정 상황이 끝날 때까지"** 계속 시키고 싶을 때 씁니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`while` (와일)**: **"~하는 동안에는 계속해!"**라는 뜻입니다. 괄호 `()` 안의 조건이 참(True)이면 무한히 반복합니다.
- **`do` (두)**: **"일단 해!"**라는 뜻입니다. 조건을 따지기 전에 무조건 한 번은 실행하라는 강력한 명령입니다.

### 💻 실습 예제: 몬스터 HP 깎기 (while)
```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int hp = 30;
            while (hp > 0) // "HP가 0보다 큰 동안에는 계속 때려!"
            {
                Console.WriteLine("공격! 남은 HP: " + hp);
                hp -= 10;
            }
            Console.WriteLine("🐉 몬스터 처치!");
        }
    }
}
```

---

## 4. int.Parse: "변신의 마법"
컴퓨터는 우리가 키보드로 치는 모든 것을 **'글자(string)'**로만 인식합니다. 숫자로 계산하려면 '변신'이 필요합니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`int` (인트)**: 정수 상자를 뜻합니다.
- **`Parse` (파스)**: **"해석해서 변환하다"**라는 뜻입니다. 
- **`int.Parse("123")`**: "따옴표가 붙은 글자 '123'을 진짜 숫자 123으로 해석해서 바꿔줘!"라는 주문입니다.

### 💻 실습 예제: 나이 더하기
```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("나이를 입력하세요: ");
            string input = Console.ReadLine(); // 예: "25" 입력

            int age = int.Parse(input); // "글자 25"를 "숫자 25"로 변신!
            Console.WriteLine("내년에는 " + (age + 1) + "살이 되시네요!");
        }
    }
}
```

---

## 5. 실전 미션: "Up & Down 게임 완성하기"
오늘 배운 키워드(`while`, `break`, `int.Parse`)를 모두 사용해서 게임을 만들어봅시다.

```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int target = 42; 
            int guess = 0;

            Console.WriteLine("=== 숫자 맞추기 게임 ===");

            while (true) // "중단 명령(break)이 나올 때까지 무한 반복해!"
            {
                Console.Write("숫자 입력: ");
                guess = int.Parse(Console.ReadLine());

                if (guess == target)
                {
                    Console.WriteLine("🎉 정답!");
                    break; // "정답 맞췄으니 무한 반복 당장 멈춰!"
                }
                else if (guess < target) Console.WriteLine("UP!");
                else Console.WriteLine("DOWN!");
            }
        }
    }
}
```
