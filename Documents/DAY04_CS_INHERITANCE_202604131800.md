# 🚀 4일차: 부모의 능력을 물려받자! (생성자와 상속)

오늘의 목표는 **"객체가 태어날 때 초기 설정을 하고, 공통된 기능을 효율적으로 관리하는 방법(상속)을 배운다"**입니다.

---

## 1. 생성자(Constructor): "태어날 때 정해지는 운명"
객체를 `new`로 만들 때 딱 한 번 실행되는 특수한 메소드입니다. 주로 캐릭터의 이름이나 초기 체력을 설정할 때 사용합니다.

### 💡 이 단어는 무슨 뜻인가요?
- **생성자 (Constructor)**: 클래스 이름과 똑같이 생긴 메소드입니다. 반환 타입(`void` 등)을 쓰지 않습니다.
- **오버로딩 (Overloading)**: "과적"이라는 뜻으로, **이름은 같지만 재료(매개변수)가 다른** 여러 버전의 생성자를 만드는 것을 말합니다.

### 💻 실습 예제: 이름 없는 캐릭터는 없다!
**미션:** 객체 생성 시 초기화 작업을 수행하는 생성자를 정의하고, 매개변수에 따라 다르게 동작하는 생성자 오버로딩을 실습해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

namespace Day04
{
    class Hero
    {
        public string name;
        public int hp;

        // 1. 기본 생성자 (재료가 없을 때)
        public Hero()
        {
            name = "이름없음";
            hp = 100;
            Console.WriteLine("기본 히어로가 생성되었습니다.");
        }

        // 2. 이름만 받는 생성자 (오버로딩)
        public Hero(string name)
        {
            this.name = name; // 'this.name'은 클래스의 변수, 'name'은 받은 재료입니다.
            this.hp = 100;
            Console.WriteLine("{0} 히어로가 생성되었습니다.", name);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Hero h1 = new Hero(); // 기본 생성자 실행
            Hero h2 = new Hero("슈퍼맨"); // 이름 있는 생성자 실행

            Console.WriteLine("h1 이름: " + h1.name);
            Console.WriteLine("h2 이름: " + h2.name);
        }
    }
}
```

</details>

---

## 2. 상속(Inheritance): "자식은 부모를 닮는다"
여러 클래스에 겹치는 코드가 많을 때, 공통된 부분(부모)을 만들고 나머지가 이를 이어받는 기술입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **부모 클래스 (Base/Parent)**: 공통 기능을 가진 원조 클래스입니다.
- **자식 클래스 (Derived/Child)**: 부모의 능력을 물려받고 자신만의 기능을 추가한 클래스입니다.
- **`:` (콜론)**: "누구로부터 물려받는다"는 뜻의 기호입니다. (예: `class Warrior : Job`)

### 💻 실습 예제: RPG 직업 나누기
**미션:** 공통 기능을 가진 부모 클래스를 상속받아 새로운 자식 클래스를 만들고, 부모의 기능을 그대로 사용하는 방법을 익혀 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

namespace Day04
{
    // 1. 모든 직업의 공통 부모 (Job)
    class Job
    {
        public string jobName;
        public void Attack()
        {
            Console.WriteLine("{0}이(가) 일반 공격을 합니다!", jobName);
        }
    }

    // 2. 전사 (Warrior)는 Job을 상속받습니다.
    class Warrior : Job
    {
        public void Bash() // 전사만의 특별한 기술
        {
            Console.WriteLine("전사가 강력한 배쉬 공격을 합니다!");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Warrior myWarrior = new Warrior();
            myWarrior.jobName = "용맹한 전사";
            
            myWarrior.Attack(); // 부모(Job)로부터 물려받은 기능
            myWarrior.Bash();   // 자신(Warrior)만의 기능
        }
    }
}
```

</details>

---

## 3. 오버라이딩(Overriding): "물려받은 기술 재창조"
부모의 기능이 마음에 들지 않거나, 자식마다 다르게 행동해야 할 때 기능을 **'덮어쓰기'**하는 것입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`virtual` (버추얼)**: "이 기능은 자식이 **바꿀 수 있게 허용**하겠다"는 뜻입니다. (부모 쪽에 작성)
- **`override` (오버라이드)**: "부모가 준 기능을 **내가 다시 정의**하겠다"는 뜻입니다. (자식 쪽에 작성)

### 💻 실습 예제: 몬스터마다 다른 울음소리
**미션:** 부모 클래스의 가상 메소드(virtual)를 자식 클래스에서 재정의(override)하여 각 객체마다 고유한 동작을 수행하도록 구현해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

namespace Day04
{
    class Monster
    {
        public virtual void Cry() // 나중에 바꿀 수 있도록 virtual!
        {
            Console.WriteLine("몬스터가 소리를 냅니다.");
        }
    }

    class Slime : Monster
    {
        public override void Cry() // 몬스터의 소리를 슬라임 버전으로 덮어쓰기!
        {
            Console.WriteLine("슬라임: 푸슉푸슉~");
        }
    }

    class Dragon : Monster
    {
        public override void Cry()
        {
            Console.WriteLine("드래곤: 크워어어어어!");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Monster s = new Slime();
            Monster d = new Dragon();

            s.Cry(); // "푸슉푸슉~" 출력
            d.Cry(); // "크워어어어어!" 출력
        }
    }
}
```

</details>

---

## 4. 4일차 종합 미션: "동물원 관리 프로그램"
다음 조건에 맞는 클래스 구조를 설계해보세요.

1. `Animal` 부모 클래스를 만듭니다.
   - `name` 변수를 가집니다.
   - `Eat()` 메소드를 가집니다. (가상 메소드로 만드세요)
2. `Dog`, `Cat` 클래스를 만들어 `Animal`을 상속받습니다.
   - 각자에게 맞는 `Eat()` 내용을 오버라이딩 하세요. (예: "강아지가 사료를 먹습니다.")
3. `Main`에서 강아지와 고양이를 한 마리씩 생성하고 밥을 먹여보세요.

---

**Tip**: 자식 클래스에서 부모 클래스의 생성자를 부르고 싶을 때는 `public Dog(string name) : base(name) { }` 처럼 `: base()`를 사용합니다!

---

## 5. 4일차 심화 미션: "몬스터 도감 확장"

**[미션 목표]**
3일차에서 만든 `Monster` 클래스를 부모 클래스로 삼아, 다양한 특징을 가진 자식 몬스터(Slime, Orc, Dragon)를 설계합니다. 상속과 오버라이딩을 통해 코드의 재사용성을 높이고 각 객체만의 고유한 행동을 정의하는 법을 학습합니다.

---

### 1) 요구 사항

#### 1. 부모 클래스 고도화 (`Monster`)
* **공통 필드**: 이름(`Name`), 체력(`Hp`), 공격력(`Atk`).
* **가상 메서드 (`virtual`)**: 
    * `Attack(Player target)`: 기본 공격 로직을 정의합니다.
    * `TakeDamage(int damage)`: 기본 데미지 입기 로직을 정의합니다.

#### 2. 자식 클래스 설계 (`Inheritance`)
* **Slime (슬라임)**: 체력은 낮지만, 데미지를 입을 때마다 "말랑말랑해서 데미지가 경감되었습니다!"라는 메시지와 함께 받는 데미지를 소폭 줄여서 계산합니다. (`TakeDamage` 오버라이드)
* **Orc (오크)**: 체력이 일정 수준 이하가 되면 "광폭화!" 상태가 되어 공격력이 상승합니다. (`Attack` 오버라이드)
* **Dragon (드래곤)**: 보스급 몬스터로, 공격 시 일정한 확률로 특수 기술인 "브레스"를 뿜어 강력한 데미지를 입힙니다. (`Attack` 오버라이드)

#### 3. 생성자 활용
* 각 자식 클래스는 `base()` 키워드를 사용하여 부모 클래스의 생성자를 호출하고, 각 몬스터의 특징에 맞는 초기 능력치를 설정해야 합니다.

---

### 2) 프로그래밍 힌트
* `base.TakeDamage(damage)`를 호출하면 부모 클래스에 정의된 기본 로직을 사용하면서 추가적인 연산(데미지 경감 등)을 덧붙일 수 있습니다.
* 드래곤의 특수 기술 확률은 `Random` 클래스를 사용하여 구현할 수 있습니다.
* 부모 타입의 변수로 자식 객체를 다루는 **다형성**을 의식하며 설계해 보세요. (예: `Monster m = new Slime();`)


**[심화 과제 (선택 사항)]**
- **추상 클래스(Abstract) 적용**: `Monster` 자체는 실체가 없는 개념이므로 `abstract` 클래스로 선언하고, 모든 몬스터가 반드시 구현해야 할 메서드를 정의해 보세요.
- **다형성 테스트**: 부모 타입인 `Monster` 변수에 서로 다른 자식 객체들을 번갈아 담으며 동일한 메서드를 호출해 보세요.

---
## ✍️ 4일차 핵심 퀴즈
1. 부모 클래스의 메소드를 자식이 마음대로 바꾸려면, 부모 쪽에는 어떤 키워드를 붙여야 하나요?
2. 자식 클래스에서 부모 클래스의 생성자를 강제로 호출할 때 사용하는 키워드는 무엇인가요?
