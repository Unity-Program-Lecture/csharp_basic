# 🚀 3일차: 기능은 쪼개고, 데이터는 묶고! (메소드와 클래스)

오늘의 목표는 **"반복되는 코드를 깔끔하게 정리하고, 나만의 설계도(클래스)를 만들어 본다"**입니다.

---

## 1. 메소드(Method): "자주 쓰는 기능 상자"
`Main` 안에 모든 코드를 다 쓰면 너무 길어지고 복잡해집니다. 자주 쓰는 기능을 떼어내서 이름을 붙인 것이 메소드입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`static` (스테틱)**: "프로그램이 시작될 때 **항상 그 자리에 있다**"는 뜻입니다. 지금은 `Main`에서 바로 쓰기 위해 붙여줍니다.
- **`void` (보이드)**: "비어 있다"는 뜻입니다. 일을 시켰을 때 **결과물을 돌려주지 않고** 일만 끝내는 메소드에 붙입니다.
- **`return` (리턴)**: "결과를 돌려주다"라는 뜻입니다. 계산기처럼 결과값이 필요할 때 사용합니다.
- **매개변수 (Parameter)**: 메소드 상자 안에 집어넣는 **"재료"**입니다.

### 💻 실습 예제: 인사하는 기계와 더하기 계산기
```csharp
using System;

namespace Day03
{
    internal class Program
    {
        // 1. 결과물이 없는 인사 메소드 (void)
        static void SayHello(string name) // 'name'이라는 재료를 받음
        {
            Console.WriteLine("안녕! 나는 {0}라고 해.", name);
        }

        // 2. 결과물을 돌려주는 계산 메소드 (int)
        static int Add(int a, int b)
        {
            return a + b; // 두 숫자를 더해서 밖으로 던져줌!
        }

        static void Main(string[] args)
        {
            // 메소드 호출 (상자 사용하기)
            SayHello("SBS봇"); 
            
            int result = Add(10, 20);
            Console.WriteLine("10 + 20의 결과는: " + result);
        }
    }
}
```

---

## 2. 클래스(Class): "객체를 만드는 설계도"
변수(상태)와 메소드(행동)를 하나로 묶어놓은 **'주머니'**이자 **'설계도'**입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`class` (클래스)**: 붕어빵을 찍어내는 **'틀'**이나 자동차의 **'설계도'**와 같습니다.
- **`new` (뉴)**: 설계도를 보고 **'진짜 물건'**을 하나 만들어내라는 특수 명령어입니다.
- **인스턴스 (Instance)**: 설계도(`new`)를 통해 만들어진 **'진짜 물건(객체)'**을 말합니다.

### 💻 실습 예제: 슬라임 몬스터 설계도
```csharp
using System;

namespace Day03
{
    // 1. 슬라임의 설계도 (클래스)를 만듭니다.
    class Slime
    {
        public string name; // 슬라임의 이름 (변수)
        public int hp;      // 슬라임의 체력 (변수)

        public void TakeDamage(int damage) // 데미지를 입는 행동 (메소드)
        {
            hp -= damage;
            Console.WriteLine("{0}이(가) {1}의 데미지를 입었습니다! (남은 HP: {2})", name, damage, hp);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 2. 설계도를 보고 진짜 슬라임 두 마리를 만듭니다.
            Slime s1 = new Slime();
            s1.name = "파란 슬라임";
            s1.hp = 30;

            Slime s2 = new Slime();
            s2.name = "황금 슬라임";
            s2.hp = 100;

            // 3. 각자 행동하게 시킵니다.
            s1.TakeDamage(10);
            s2.TakeDamage(50);
        }
    }
}
```

---

## 3. 접근 제한자: "내 것과 남의 것"
클래스 안의 소중한 정보를 아무나 바꾸지 못하게 막거나 허용하는 보안 설정입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`public` (퍼블릭)**: "공공의"라는 뜻입니다. **외부 어디서든** 이 정보를 보거나 바꿀 수 있습니다.
- **`private` (프라이빗)**: "개인적인"이라는 뜻입니다. **클래스 안에서만** 볼 수 있고, 외부에서는 절대 건드릴 수 없습니다.

### 💻 실습 예제: 소중한 지갑 관리
```csharp
using System;

namespace Day03
{
    class Wallet
    {
        private int money = 0; // 돈은 비밀! (외부에서 직접 수정 불가)

        public void AddMoney(int amount) // 돈을 넣는 통로
        {
            money += amount;
            Console.WriteLine("{0}원이 입금되었습니다. 현재 잔액: {1}원", amount, money);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Wallet myWallet = new Wallet();
            
            // myWallet.money = 1000000; // 에러 발생! (private이라 직접 못 건드림)
            
            myWallet.AddMoney(5000); // 정해진 방법(메소드)으로만 접근 가능!
        }
    }
}
```

---

## 4. 3일차 종합 미션: "RPG 캐릭터 만들기"
오늘 배운 클래스와 메소드를 활용해 나만의 캐릭터 설계도를 완성해보세요.

```csharp
using System;

namespace Day03
{
    class Player
    {
        public string name;
        public int level = 1;

        public void ShowStatus()
        {
            Console.WriteLine("--- 캐릭터 정보 ---");
            Console.WriteLine("이름: " + name);
            Console.WriteLine("레벨: " + level);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("캐릭터 이름을 정해주세요: ");
            string inputName = Console.ReadLine();

            Player myHero = new Player();
            myHero.name = inputName;

            myHero.ShowStatus();
            Console.WriteLine("모험을 시작합니다!");
        }
    }
}
```
