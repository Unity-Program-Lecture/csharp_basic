# 🚀 2일차: 컴퓨터에게 똑똑하게 일 시키기 (제어 흐름 마스터)

오늘 수업의 목표는 **"컴퓨터의 계산 방식을 이해하고, 반복문과 조건문을 내 마음대로 조종한다"**입니다.

---

## 0. 기본 연산자: "컴퓨터의 계산법"
컴퓨터가 데이터를 처리할 때 사용하는 기호들입니다. 수학 시간과 비슷하지만 조금 다른 부분도 있어요!

### 💡 이 기호들은 무슨 뜻인가요?
- **산술 연산자**: `+`, `-`, `*`(곱하기), `/`(나누기), **`%`(나머지)**
    - **`%`**: "나누고 남은 찌꺼기"를 구합니다. `5 % 2`는 1입니다. (짝수/홀수 판별에 필수!)
- **비교 연산자**: `==`(같니?), `!=`(다르니?), `>`, `<`, `>=`, `<=`
    - **`==`**: "같다"가 아니라 **"양쪽이 똑같니?"**라고 물어보는 기호입니다. (결과는 True/False)
- **논리 연산자**: `&&`(그리고), `||`(또는), `!`(반대)
    - **`&&`**: "둘 다 맞아야 정답!" (ID와 PW가 모두 맞아야 로그인 성공)
    - **`||`**: "하나라도 맞으면 정답!" (사과 또는 배 중 하나만 있어도 OK)

### 💻 실습 예제: 간단한 계산기 만들기
```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int num1 = 10;
            int num2 = 3;

            Console.WriteLine("=== 기본 연산 실습 ===");
            Console.WriteLine("10 더하기 3은? " + (num1 + num2));
            Console.WriteLine("10 나누기 3의 몫은? " + (num1 / num2));
            Console.WriteLine("10 나누기 3의 나머지는? " + (num1 % num2)); // 1 출력

            Console.WriteLine("\n=== 비교 연산 실습 ===");
            Console.WriteLine("10은 3보다 큰가요? " + (num1 > num2));     // True
            Console.WriteLine("10은 3과 똑같나요? " + (num1 == num2));   // False

            Console.WriteLine("\n=== 논리 연산 실습 ===");
            bool isApple = true;
            bool isBanana = false;
            Console.WriteLine("사과와 바나나가 둘 다 있나요? " + (isApple && isBanana)); // False
        }
    }
}
```

---

## 1. break와 continue: "내 마음대로 흐름 바꾸기"
반복문(`for`, `while`) 안에서 사용하는 특수 명령입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`break`**: "당장 멈춰!" (반복문 즉시 탈출)
- **`continue`**: "이번 판은 패스!" (아래 코드 무시하고 다음 숫자로 점프)

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
                if (i % 2 == 0) continue; // 짝수는 "패스!" (찌꺼기가 0이면 짝수)

                Console.WriteLine("홀수 확인: " + i);

                if (i == 7)
                {
                    Console.WriteLine("🎯 7을 찾았다! 종료!");
                    break; // 찾았으니까 "당장 멈춰!"
                }
            }
        }
    }
}
```

---

## 2. switch, case, default: "깔끔한 메뉴 선택"
### 💡 이 단어는 무슨 뜻인가요?
- **`switch`**: "어디로 연결할까?"라고 메뉴판을 펼치는 명령입니다.
- **`case`**: "~인 경우"라는 뜻입니다.
- **`default`**: "나머지 전부"라는 뜻입니다.

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

            switch (input)
            {
                case "1":
                    Console.WriteLine("⚔️ 검을 선택했습니다.");
                    break;
                case "2":
                    Console.WriteLine("🏹 활을 선택했습니다.");
                    break;
                default:
                    Console.WriteLine("❓ 다시 골라주세요.");
                    break;
            }
        }
    }
}
```

---

## 3. while과 do: "조건부 무한 반복"
### 💡 이 단어는 무슨 뜻인가요?
- **`while`**: "~하는 동안에는 계속해!" (참이면 무한 반복)
- **`do`**: "일단 해!" (무조건 한 번은 실행)

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
            while (hp > 0)
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
### 💡 이 단어는 무슨 뜻인가요?
- **`Parse`**: "글자를 해석해서 숫자로 변신시켜줘!"라는 주문입니다.

### 💻 실습 예제: 내년 나이 구하기
```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("나이를 입력하세요: ");
            string input = Console.ReadLine(); 

            int age = int.Parse(input); // "글자" -> "숫자" 변신!
            Console.WriteLine("내년에는 " + (age + 1) + "살이 되시네요!");
        }
    }
}
```

---

## 5. 실전 미션: "Up & Down 게임 완성하기"
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

            while (true)
            {
                Console.Write("숫자 입력: ");
                guess = int.Parse(Console.ReadLine());

                if (guess == target)
                {
                    Console.WriteLine("🎉 정답!");
                    break; 
                }
                else if (guess < target) Console.WriteLine("UP!");
                else Console.WriteLine("DOWN!");
            }
        }
    }
}
```
