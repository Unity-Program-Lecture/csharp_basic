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
**미션:** 매개변수를 받아 인사말을 출력하는 메소드와 두 정수의 합을 반환하는 메소드를 각각 정의하고 호출해 보세요.

<details>
<summary>코드 보기</summary>

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

</details>

---

## 2. 클래스(Class): "객체를 만드는 설계도"
변수(상태)와 메소드(행동)를 하나로 묶어놓은 **'주머니'**이자 **'설계도'**입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`class` (클래스)**: 붕어빵을 찍어내는 **'틀'**이나 자동차의 **'설계도'**와 같습니다.
- **`new` (뉴)**: 설계도를 보고 **'진짜 물건'**을 하나 만들어내라는 특수 명령어입니다.
- **인스턴스 (Instance)**: 설계도(`new`)를 통해 만들어진 **'진짜 물건(객체)'**을 말합니다.

### 💻 실습 예제: 슬라임 몬스터 설계도
**미션:** 이름과 체력을 속성으로 가지고, 데미지를 입는 기능을 가진 슬라임 클래스를 설계하고 인스턴스를 생성해 보세요.

<details>
<summary>코드 보기</summary>

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

</details>

---

## 3. 접근 제한자: "내 것과 남의 것"
클래스 안의 소중한 정보를 아무나 바꾸지 못하게 막거나 허용하는 보안 설정입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`public` (퍼블릭)**: "공공의"라는 뜻입니다. **외부 어디서든** 이 정보를 보거나 바꿀 수 있습니다.
- **`private` (프라이빗)**: "개인적인"이라는 뜻입니다. **클래스 안에서만** 볼 수 있고, 외부에서는 절대 건드릴 수 없습니다.

### 💻 실습 예제: 소중한 지갑 관리
**미션:** private 접근 제한자를 사용하여 외부에서 직접 수정할 수 없는 잔액 변수를 만들고, 메소드를 통해서만 값을 변경하도록 구현해 보세요.

<details>
<summary>코드 보기</summary>

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

</details>

---

## 4. 3일차 종합 미션: "RPG 캐릭터 만들기"
오늘 배운 클래스와 메소드를 활용해 나만의 캐릭터 설계도를 완성해보세요.

**미션:** 사용자로부터 이름을 입력받아 캐릭터 객체를 생성하고, 현재 상태를 출력하는 기능을 포함한 간단한 RPG 캐릭터 시스템을 완성해 보세요.

<details>
<summary>코드 보기</summary>

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

</details>

---
## 5. 3일차 심화 미션: "몬스터 사냥 시스템" 구현하기

**[미션 목표]**
`Character` 클래스와 `Monster` 클래스를 각각 설계하고, 플레이어가 몬스터를 공격하거나 물약을 마셔 체력을 회복하는 로직을 메서드로 구현합니다. 이를 통해 객체 간의 데이터 전달과 상호작용의 흐름을 학습합니다.

---

### 1) 요구 사항

#### 1. 클래스 설계
* **Player 클래스**: 이름(`Name`), 공격력(`Atk`), 현재 체력(`Hp`) 필드를 가집니다.
* **Monster 클래스**: 이름(`Name`), 현재 체력(`Hp`) 필드를 가집니다.

#### 2. 메서드 구현
* **Attack(Monster target)**: 플레이어가 특정 몬스터를 공격하여 몬스터의 HP를 자신의 공격력만큼 감소시킵니다.
* **TakeDamage(int damage)**: 공격받았을 때 HP가 감소하고, 현재 남은 체력을 출력합니다.
* **Heal()**: 물약을 사용하여 플레이어의 HP를 일정량(예: 20) 회복합니다.

#### 3. 생성자(Constructor)
* 객체를 생성할 때 이름과 초기 능력치를 자유롭게 설정할 수 있도록 생성자를 정의합니다.

---

### 2) 프로그래밍 힌트
* `Attack` 메서드의 매개변수로 `Monster` 타입의 객체를 전달받아, 그 객체의 `TakeDamage` 메서드를 호출하는 방식으로 설계해 보세요.
* `Math.Max(0, Hp)`를 사용하면 체력이 음수로 표시되는 것을 방지할 수 있습니다.
* 객체지향의 핵심인 **'메시지 전달'**에 집중하여, 플레이어가 몬스터에게 "데미지를 입어라"라고 명령하는 구조를 만듭니다.


**[심화 과제 (선택 사항)]**
- 반환 타입 활용: Attack 메서드가 공격 성공 여부나 실제 가한 데미지 값을 int로 반환하도록 수정해 보세요.
- 몬스터 반격: 플레이어가 공격한 후, 몬스터도 플레이어를 공격하는 Monster.Attack(Player target) 메서드를 추가하여 턴제 전투의 기초를 만들어 보세요.
- Static 변수 활용: 프로그램 전체에서 생성된 총 몬스터의 숫자를 기록하는 static int monsterCount 변수를 추가하고 관리해 보세요.

---
## ✍️ 3일차 핵심 퀴즈
1. 클래스 안에 선언된 변수와 메소드 중, 외부에서 절대 건드리지 못하게 하려면 어떤 키워드를 붙여야 하나요?
2. `static`이 붙은 메소드와 그렇지 않은 메소드의 가장 큰 차이는 무엇인가요?
