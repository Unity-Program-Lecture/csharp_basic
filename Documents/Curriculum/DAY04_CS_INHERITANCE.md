# 🚀 Day 04: 상속과 다형성 (전통과 개성)

오늘의 목표는 "**부모의 기능을 물려받는 상속을 배우고, 자식만의 개성을 더하는 오버라이딩을 마스터한다**"입니다.

---

## 1. 상속(Inheritance): "부모님께 물려받기"
이미 만들어진 클래스의 기능을 그대로 가져와 새로운 기능을 덧붙이는 것입니다.

```csharp
public class Animal 
{
    public void Eat() { Debug.Log("냠냠 먹습니다."); }
}

// Animal의 기능을 물려받음
public class Dog : Animal 
{
    public void Bark() { Debug.Log("멍멍!"); }
}
```

---

## 2. this vs base (나 vs 부모)
- **`this`**: "지금 이 객체(나)"를 가리킵니다.
- **`base`**: "나를 만든 부모님"을 가리킵니다.

```csharp
public class Parent
{
    public Parent(string msg) { Debug.Log($"부모 생성자: {msg}"); }
}

public class Child : Parent
{
    // base()를 통해 부모 생성자에 데이터를 전달
    public Child() : base("안녕!") 
    {
        Debug.Log("자식 생성자 호출");
    }
}
```

### 💡 왜 자식에서 부모 생성자를 호출해야 할까? (중요!)
1. **부모가 먼저 태어나야 합니다**: 자식 객체가 메모리에 만들어질 때, 사실 그 안에는 부모의 영역이 먼저 만들어집니다. 즉, 부모의 변수들을 먼저 초기화해야 자식의 변수들도 안전하게 사용할 수 있습니다.
2. **부모의 "기본 생성자"가 없을 때**: 부모 클래스에 매개변수가 있는 생성자를 하나라도 직접 만들면, 컴파일러는 더 이상 **기본 생성자(매개변수 없는 것)**를 자동으로 만들어주지 않습니다.
3. **명시적 약속**: 자식 입장에서는 부모의 영역을 어떻게 초기화할지 선택해야 합니다. 부모에게 기본 생성자가 없다면, 자식은 반드시 `base(값)`을 통해 "부모님, 이 데이터로 초기화해주세요!"라고 명시적으로 알려줘야만 합니다.

---

## 3. 오버라이딩 (virtual & override)
부모의 기능을 자식의 방식대로 바꾸고 싶을 때 사용합니다.
- **`virtual`**: 부모가 수정을 허락함.
- **`override`**: 자식이 재정의함.

```csharp
public class Player
{
    public virtual void Move() { Debug.Log("천천히 걷습니다."); }
}

public class Warrior : Player
{
    public override void Move() { Debug.Log("빠르게 달려갑니다!"); }
}
```

---

## 💻 실습 예제: 다양한 몬스터 만들기
```csharp
using UnityEngine;

public class Monster
{
    public string name;
    public Monster(string name) { this.name = name; }

    public virtual void Attack() 
    { 
        Debug.Log($"{name}이(가) 기본 공격을 합니다."); 
    }
}

public class Orc : Monster
{
    public Orc() : base("오크") { }
    public override void Attack() 
    { 
        Debug.Log("오크가 몽둥이를 크게 휘두릅니다!"); 
    }
}

public class Day04_Practice : MonoBehaviour
{
    void Start()
    {
        Monster m = new Orc(); // 다형성: 부모 타입으로 자식을 다룸
        m.Attack(); // 결과: 오크의 공격이 실행됨
    }
}
```

---

## ✍️ 핵심 퀴즈
1. 자식 클래스에서 부모 클래스의 생성자를 호출할 때 사용하는 키워드는?
2. 부모의 메소드를 자식이 바꿀 수 있게 허용하려면 부모 쪽에 어떤 키워드를 붙이나요?
3. `Monster m = new Slime();` 처럼 부모 타입으로 자식 객체를 다루는 성질을 무엇이라 하나요?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 3)]
**상속**과 **다형성**을 활용하여 일반 몬스터와 보스 몬스터를 구분하고 다른 행동을 하도록 만듭니다.

**[요구 사항]**
1. `Monster` 부모 클래스에 `virtual void OnDead()` 메소드를 만듭니다. (기본: "몬스터가 사라졌습니다.")
2. `NormalMonster` 자식 클래스: `OnDead`를 오버라이드하여 "아이템을 떨어뜨렸습니다."를 출력합니다.
3. `BossMonster` 자식 클래스:
   - `private int shield;` 필드를 추가합니다.
   - `TakeDamage`를 오버라이드하여 쉴드가 있으면 쉴드부터 깎고, 쉴드가 0일 때만 HP를 깎습니다.
   - `OnDead`를 오버라이드하여 "화려한 이펙트와 함께 보스 처치!"를 출력합니다.
4. `Monster[] monsters` 배열에 일반 몬스터와 보스 몬스터를 섞어서 담고, 반복문을 통해 모두 공격하여 각각 어떤 `OnDead`가 실행되는지 확인하세요.

**[프로그래밍 힌트]**
- `Monster m = new BossMonster();`와 같이 부모 타입 배열에 자식 객체를 담을 수 있습니다.
- `base.TakeDamage(damage);`를 사용해 부모의 로직을 재사용할 수 있는지 고민해 보세요.

