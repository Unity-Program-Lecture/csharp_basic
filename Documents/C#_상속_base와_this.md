# C# 상속: base와 this 핵심 정리

상속을 이해할 때 `this`와 `base`는 "**나**"(자식)와 "**부모**"를 구분하는 가장 중요한 이정표입니다.

---

## 1. this 키워드: "지금 이 객체(나)"

`this`는 현재 인스턴스 자신을 가리킵니다.

*   **이름 중복 해결:** 필드와 매개변수의 이름이 같을 때 구분합니다. (`this.name = name;`)
*   **생성자 연결 (this()):** 클래스 내에서 매개변수가 다른 생성자를 호출하여 코드 중복을 줄입니다.

## 2. base 키워드: "나를 만든 부모"

`base`는 자식 클래스에서 부모 클래스의 멤버에 접근할 때 사용합니다.

*   **부모 생성자 호출 (base()):** 자식 클래스의 인스턴스를 만들 때, 부모 클래스의 생성자를 먼저 실행하도록 강제합니다.
*   **부모 멤버 호출:** 재정의(Override)된 메서드가 아닌, 부모의 원본 기능을 호출할 때 사용합니다.

---

## 3. 상세 예제: 캐릭터 시스템 구현

기반 클래스(`Unit`)와 이를 상속받는 파생 클래스(`Warrior`)를 통해 실전 활용법을 알아봅니다.

```csharp
using System;

// [기반 클래스: 부모]
class Unit
{
    public string Name;
    public int Hp;

    // 기본 생성자
    public Unit(string name)
    {
        this.Name = name; // this로 매개변수와 필드 구분
        this.Hp = 100;
        Console.WriteLine($"[Unit] {Name} 생성 (부모 생성자 호출)");
    }

    public virtual void Move()
    {
        Console.WriteLine($"{Name}이(가) 이동합니다.");
    }
}

// [파생 클래스: 자식]
class Warrior : Unit
{
    public int Strength;

    // base(name)을 통해 부모 클래스의 생성자에 매개변수를 전달하며 호출
    public Warrior(string name, int strength) : base(name)
    {
        this.Strength = strength; // 자신만의 필드 초기화
        Console.WriteLine($"[Warrior] 힘 {Strength} 설정 (자식 생성자 호출)");
    }

    // 부모의 생성자 중 하나를 선택해 연결 (this() 활용 예시)
    public Warrior(string name) : this(name, 10) // 힘 기본값 10으로 설정하여 위 생성자 호출
    {
    }

    public override void Move()
    {
        base.Move(); // 부모의 Move() 로직 실행
        Console.WriteLine($"{Name}이(가) 전사답게 묵직하게 뛰어갑니다.");
    }
}

class Program
{
    static void Main()
    {
        Warrior warrior = new Warrior("아라곤", 50);
        warrior.Move();
    }
}
```

### 💡 실행 결과 및 흐름 설명
1.  **부모 먼저:** `new Warrior("아라곤", 50)` 호출 시, `base(name)`에 의해 `Unit` 생성자가 먼저 실행됩니다.
2.  **자식 다음:** 부모의 초기화가 끝나면 `Warrior` 생성자의 나머지 본문이 실행됩니다.
3.  **메서드 확장:** `base.Move()`를 호출함으로써 부모의 기능을 버리지 않고 그 위에 자식만의 기능을 덧붙일 수 있습니다.

---

## 4. 상속에서 함께 알아야 할 핵심 키워드

상속의 메커니즘을 완성하기 위해 아래 키워드들을 함께 학습하는 것이 좋습니다.

### ① virtual & override (다형성)

* **virtual:** 부모 클래스에서 "자식이 재정의할 수 있음"을 명시합니다.  
* **override:** 부모의 `virtual` 메서드를 자식 클래스에서 새롭게 구현합니다.

### ② abstract (추상화)

* **abstract class:** 인스턴스를 직접 생성할 수 없는 '설계도' 전용 클래스입니다.  
* **abstract method:** 구현부 없이 선언만 하며, 자식 클래스에서 **반드시** 구현하도록 강제합니다.

### ③ sealed (상속 제한)

* 더 이상 상속을 허용하지 않거나, 특정 메서드의 재정의를 막을 때 사용합니다. 클래스의 보안과 설계를 견고하게 만듭니다.

---

## 4. 학습 팁

"상속은 단순히 기능을 물려받는 것을 넘어, '**나(this)만의 개성**'과 '**부모(base)로부터의 전통**'을 어떻게 조화시키느냐의 문제입니다."  
