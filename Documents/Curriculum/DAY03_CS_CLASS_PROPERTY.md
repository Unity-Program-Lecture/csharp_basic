# 🚀 Day 03: 클래스와 프로퍼티 (설계도와 안전 창구)

오늘의 목표는 "**객체지향의 핵심인 클래스를 이해하고, 데이터를 안전하게 보호하는 프로퍼티를 마스터한다**"입니다.

---

## 1. 클래스(Class): "객체 제조 설계도"
클래스는 변수(상태)와 메소드(기능)를 하나로 묶은 설계도입니다. 이 설계도로 만든 실체를 **객체(Object)** 또는 **인스턴스**라고 부릅니다.
- **비유**: 붕어빵 틀(클래스)과 붕어빵(객체).

```csharp
// 1. 설계도(클래스) 만들기
public class Monster
{
    public string name = "슬라임";
    public void Move() { Debug.Log($"{name}이 이동합니다."); }
}

// 2. 설계도로 실체(객체) 만들기
Monster m = new Monster();
m.Move();
```

---

## 2. 생성자 (Constructor): "객체의 탄생과 초기화"
객체가 생성될 때(`new`) 자동으로 호출되는 특별한 함수입니다. 주로 변수의 초기값을 설정할 때 사용합니다.

### 🐣 기본 생성자 (Default Constructor)
매개변수가 없는 생성자입니다. 클래스에 생성자를 하나도 만들지 않으면 컴파일러가 자동으로 빈 기본 생성자를 만들어줍니다.

```csharp
public class Player
{
    public string name;
    // 기본 생성자
    public Player() 
    {
        name = "이름 없음";
        Debug.Log("플레이어가 탄생했습니다.");
    }
}
```

### ⚡ 생성자 오버로딩 (Constructor Overloading)
매개변수의 개수나 타입을 다르게 하여, 여러 가지 방식으로 객체를 초기화할 수 있게 하는 기술입니다.

```csharp
public class Monster
{
    public string name;
    public int hp;

    // 1. 기본 생성자
    public Monster() 
    {
        name = "평범한 몬스터";
        hp = 100;
    }

    // 2. 오버로딩된 생성자 (이름만 받기)
    public Monster(string name)
    {
        this.name = name;
        this.hp = 100;
    }

    // 3. 오버로딩된 생성자 (이름과 체력 모두 받기)
    public Monster(string name, int hp)
    {
        this.name = name;
        this.hp = hp;
    }
}
```

---

## 3. 접근 한정자 (Access Modifiers): "문지기"
- **public**: 누구나 접근 가능 (광장)
- **private**: 나만 접근 가능 (비밀 일기장). 클래스 내부에서만 보입니다.
- **protected**: 자식 클래스에게만 공개합니다.

---

## 4. 프로퍼티 (Property): "안전한 데이터 창구"
변수를 `public`으로 두면 외부에서 마음대로 값을 바꿀 수 있어 위험합니다. 프로퍼티는 데이터를 보호하면서도 외부와 소통하게 해줍니다.

---

## 💻 실습 예제: 체력 보호 시스템
**미션:** 플레이어의 HP가 0 미만으로 설정되려 하면 자동으로 0이 되게 하는 프로퍼티를 구현해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class Player
{
    private int hp = 100;

    public int HP
    {
        get { return hp; }
        set 
        { 
            // Mathf.Max를 사용하면 더 깔끔하게 보호할 수 있습니다!
            hp = Mathf.Max(0, value); 
        }
    }
}

public class Day03_Practice : MonoBehaviour
{
    void Start()
    {
        Player p = new Player();
        p.HP = -50; // 음수를 넣어봅니다.
        Debug.Log($"현재 HP: {p.HP}"); // 결과: 0 (보호 성공!)
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. 클래스의 기본 접근 한정자(아무것도 안 적었을 때)는 무엇인가요?
2. 프로퍼티 `set` 안에서 전달받은 새로운 값을 나타내는 키워드는?
3. `new` 키워드를 통해 클래스로부터 실제 객체를 만드는 과정을 무엇이라고 하나요? (ㅇㅅㅌㅅㅎ)

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 2)]
2일차에서 다룬 '배열 기반 사냥 시스템'을 **클래스**와 **프로퍼티**를 사용해 리팩토링(Refactoring)합니다.

**[요구 사항]**
1. `Monster` 클래스를 만드세요.
   - 필드: `private string name;`, `private int hp;`
   - 프로퍼티: `Name` (읽기 전용), `HP` (쓰기 시 0 미만이 되면 0으로 고정)
   - 생성자: 이름과 초기 체력을 받아 설정합니다.
   - 메소드: `TakeDamage(int damage)` - 데미지를 받으면 HP 프로퍼티를 통해 체력을 깎고 로그를 남깁니다.
2. `Monster[] monsters` 배열을 만들고 3마리의 몬스터 객체를 생성하여 담으세요.
3. 플레이어의 공격력을 설정하고, `for` 반복문을 돌며 모든 몬스터에게 `TakeDamage`를 호출합니다.
4. 반복문이 끝난 후, 살아남은 몬스터의 이름만 골라서 출력해 보세요.

**[프로그래밍 힌트]**
- `Monster[] monsters = new Monster[3];`으로 공간을 만들고 `new Monster(...)`로 각 칸을 채워야 합니다.
- `if (monster.HP > 0)` 조건을 통해 생존 여부를 확인할 수 있습니다.

