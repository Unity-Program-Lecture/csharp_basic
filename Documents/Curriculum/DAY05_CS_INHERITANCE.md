# 🚀 Day 05: 상속과 다형성 (전통과 개성)

오늘의 목표는 "**부모의 기능을 물려받는 상속을 배우고, 자식만의 개성을 더하는 오버라이딩을 마스터한다**"입니다.

---

## 1. 상속(Inheritance): "부모님께 물려받기"
상속은 이미 잘 만들어진 클래스를 그대로 가져와 새로운 기능을 덧붙이는 것입니다.
- **부모 클래스 (Base)**: 기능을 주는 쪽.
- **자식 클래스 (Derived)**: 기능을 받는 쪽.

---

## 2. this vs base (나 vs 부모)
- **`this`**: "지금 이 객체(나)"를 가리킵니다. (내 이름, 내 스킬)
- **`base`**: "나를 만든 부모님"을 가리킵니다. (부모님의 이름, 부모님의 생성자 호출)

---

## 3. 오버라이딩 (virtual & override)
부모에게 물려받은 기능이 마음에 들지 않거나, 나만의 방식으로 바꾸고 싶을 때 사용합니다.
- **`virtual`**: 부모가 "이 기능은 자식이 바꿔도 좋아!"라고 허락하는 것.
- **`override`**: 자식이 "내 방식대로 다시 정의할게요!"라고 선언하는 것.

---

## 💻 실습 예제: 다양한 몬스터 만들기
**미션:** `Monster` 부모 클래스를 만들고, 이를 상속받아 공격 방식이 다른 `Orc`와 `Slime`을 만들어 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

class Monster
{
    public string name;
    public Monster(string name) { this.name = name; }

    public virtual void Attack() 
    { 
        Console.WriteLine($"{name}이(가) 기본 공격을 합니다."); 
    }
}

class Orc : Monster
{
    public Orc() : base("오크") { }
    public override void Attack() 
    { 
        Console.WriteLine("오크가 몽둥이를 크게 휘두릅니다!"); 
    }
}

class Program
{
    static void Main()
    {
        Monster m = new Orc(); // 다형성: 부모의 이름표로 자식을 가리킴
        m.Attack(); // 결과: 오크의 공격이 실행됨
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. 자식 클래스에서 부모 클래스의 생성자를 호출할 때 사용하는 키워드는?
2. 부모의 메소드를 자식이 바꿀 수 있게 허용하려면 부모 쪽에 어떤 키워드를 붙이나요?
3. `Monster m = new Slime();` 처럼 부모 타입으로 자식 객체를 다루는 성질을 무엇이라 하나요?
