# 🚀 2일차: 컴퓨터에게 똑똑하게 일 시키기 (제어 흐름 마스터)

오늘 수업의 목표는 **"내가 만든 반복문과 조건문의 흐름을 내 마음대로 조종한다"**입니다. 어제 배운 `for`문을 떠올리며 첫 번째 예제부터 시작해봅시다!

---

## 1. break와 continue: "내 마음대로 흐름 바꾸기"
반복문(`for`, `while`) 안에서 특별한 규칙을 정할 때 사용합니다. 어제 배운 `for`문에 날개를 달아주는 기능입니다.

### 💻 실습 예제: 럭키 세븐 찾기 (어제 배운 for문 활용)
```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 1부터 10까지 숫자 중 7 찾기 ===");

            for (int i = 1; i <= 10; i++)
            {
                // 1. 짝수는 건너뛰기
                if (i % 2 == 0) 
                {
                    continue; // 아래 코드를 무시하고 다음 숫자로 넘어감 (건너뛰기)
                }

                Console.WriteLine("현재 홀수 확인 중: " + i);

                // 2. 7을 찾으면 반복 종료
                if (i == 7)
                {
                    Console.WriteLine("🎯 럭키 세븐을 찾았습니다! 반복을 멈춥니다.");
                    break; // 반복문(for)을 즉시 탈출 (그만하기)
                }
            }
        }
    }
}
```
**🔍 해석**: `continue`는 "이번 숫자는 패스!", `break`는 "이제 그만! 집에 가자!"라는 뜻입니다.

---

## 2. switch문: "메뉴판에서 고르기"
`if`문이 "맞다/틀리다"를 따진다면, `switch`는 값이 무엇인지에 따라 바로 해당 칸으로 이동합니다.

### 💻 실습 예제: RPG 캐릭터 무기 선택
```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 무기 상점에 오신 것을 환영합니다 ===");
            Console.WriteLine("원하는 번호를 입력하세요: (1.검 / 2.지팡이 / 3.활)");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.WriteLine("⚔️ 낡은 검을 획득했습니다. 공격력이 5 상승합니다.");
                    break;
                case "2":
                    Console.WriteLine("🪄 나무 지팡이를 획득했습니다. 마력이 10 상승합니다.");
                    break;
                case "3":
                    Console.WriteLine("🏹 짧은 활을 획득했습니다. 사거리가 늘어납니다.");
                    break;
                default:
                    Console.WriteLine("❓ 그런 무기는 없습니다. 주먹으로 싸우시겠습니까?");
                    break;
            }
        }
    }
}
```
**🔍 해석**: 여기서 `break`는 "무기를 골랐으니 상점을 나가겠다"는 뜻입니다. `switch`문에서도 탈출용으로 꼭 써야 합니다!

---

## 3. while문: "조건이 맞을 때까지 무한 반복"
반복 횟수가 정해지지 않았을 때, "상태"가 변할 때까지 계속 시킵니다.

### 💻 실습 예제: 몬스터 사냥하기
```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int monsterHP = 50;
            int attackDamage = 12;

            Console.WriteLine("🐉 몬스터를 발견했습니다! 전투 시작!");

            while (monsterHP > 0)
            {
                Console.WriteLine("내 공격! 몬스터에게 {0}의 데미지를 입혔습니다.", attackDamage);
                monsterHP -= attackDamage; // HP 깎기
                Console.WriteLine("몬스터의 남은 체력: {0}", monsterHP);
            }

            Console.WriteLine("🎉 몬스터를 처치했습니다! 경험치를 얻습니다.");
        }
    }
}
```
**🔍 해석**: `monsterHP > 0`이 "참(True)"인 동안에만 중괄호 `{ }` 안의 코드가 계속 실행됩니다.

---

## 4. do-while문: "일단 한 번은 하고 나서 검사"
`while`은 시작부터 조건을 따지지만, `do-while`은 최소 한 번은 무조건 실행합니다.

### 💻 실습 예제: 비밀번호 맞추기
```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string password = "sbs";
            string input;

            do
            {
                Console.Write("비밀번호를 입력하세요 (힌트: s_s): ");
                input = Console.ReadLine();

                if (input != password)
                {
                    Console.WriteLine("❌ 틀렸습니다. 다시 시도하세요.");
                }

            } while (input != password);

            Console.WriteLine("🔓 접속 성공! 환영합니다.");
        }
    }
}
```
**🔍 해석**: "일단 입력받고(`do`), 그 다음에 비밀번호가 맞는지 검사(`while`)하자!"는 흐름입니다.

---

## 5. 실전 미션: "Up & Down 게임 완성하기"
지금까지 배운 걸 다 섞어서 하나의 프로그램을 완성해봅시다.

```csharp
using System;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int target = 42; // 정답
            int guess = 0;
            int count = 0;

            Console.WriteLine("=== 1~100 사이의 숫자를 맞춰보세요 ===");

            while (true) // 일단 무한 반복
            {
                Console.Write("예상 숫자 입력: ");
                guess = int.Parse(Console.ReadLine());
                count++; // 시도 횟수 증가

                if (guess == target)
                {
                    Console.WriteLine("🎊 정답입니다! {0}번 만에 맞추셨네요.", count);
                    break; // 정답을 맞췄으니 무한 반복 탈출!
                }
                else if (guess < target)
                {
                    Console.WriteLine("더 큰 숫자입니다! (UP)");
                }
                else
                {
                    Console.WriteLine("더 작은 숫자입니다! (DOWN)");
                }
            }
        }
    }
}
```

### 💡 오늘의 약속
1. **중괄호 `{ }` 세트 확인**: 열린 만큼 반드시 닫아야 합니다. 네임스페이스, 클래스, 메서드의 중괄호를 꼭 확인하세요!
2. **들여쓰기(Tab)**: `namespace` 안에 `class`, `class` 안에 `Main`이 있다는 것을 들여쓰기로 표현해야 코드가 예쁘고 읽기 쉽습니다.
3. **해석하기**: 코드를 치면서 속으로 "지금 체력을 깎고 있네", "지금 숫자를 비교하고 있네"라고 중얼거려 보세요!
