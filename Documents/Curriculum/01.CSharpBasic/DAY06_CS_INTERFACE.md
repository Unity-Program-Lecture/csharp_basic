# 🚀 Day 06: 추상화와 인터페이스 (강제와 약속)

오늘의 목표는 "**객체를 직접 만들 순 없지만 뼈대가 되는 추상 클래스와, 기능의 약속인 인터페이스의 차이를 이해한다**"입니다.

---

## 1. 추상 클래스(Abstract Class): "미완성 설계도"
공통적인 기능은 미리 만들어두되, 중요한 기능은 자식에게 완성하도록 맡기는 클래스입니다.
- **주의**: 미완성된 설계도이므로 **`new Monster()`와 같이 직접 객체를 만들 수 없습니다.** 반드시 자식 클래스를 통해서만 실체화할 수 있습니다.

```csharp
public abstract class Monster 
{
    public string name;
    public void Move() { Debug.Log($"{name}이 이동합니다."); }

    // 자식마다 공격 방식이 다르니, 내용은 나중에 채워라!
    public abstract void Attack(); 
}

public class Slime : Monster 
{
    public override void Attack() { Debug.Log("점프해서 몸통 박치기!"); }
}
```

---

## 2. 인터페이스(Interface): "기능의 약속"
"이걸 가진 놈이라면 반드시 이 기능을 할 줄 알아야 해!"라고 약속하는 것입니다. 다중 구현이 가능합니다.

```csharp
public interface IItem 
{
    void Use(); // 본문({ })이 없는 것이 특징!
}

public class Potion : IItem 
{
    public void Use() { Debug.Log("체력을 회복합니다."); }
}
```

---

## 💻 실습 예제: 다중 인터페이스 구현
```csharp
using UnityEngine;

interface IMovable { void Move(); }
interface IAttackable { void Attack(); }

public class Player : IMovable, IAttackable 
{
    public void Move() { Debug.Log("플레이어가 걷습니다."); }
    public void Attack() { Debug.Log("플레이어가 칼을 휘두릅니다."); }
}

public class Day06_Practice : MonoBehaviour
{
    void Start()
    {
        Player p = new Player();
        p.Move();
        p.Attack();
    }
}
```

---

## ✍️ 핵심 퀴즈
1. 클래스 상속은 하나만 가능하지만, 인터페이스는 여러 개를 가질 수 있나요?
2. 인터페이스 내부에 `int hp;` 같은 변수를 선언할 수 있나요?
3. 추상 메소드를 정의할 때 사용하는 키워드는?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 5)]
**인터페이스**를 도입하여 '공격받을 수 있는 모든 것'에 대한 규칙을 세웁니다.

**[요구 사항]**
1. `IDamageable` 인터페이스를 만드세요.
   - 메소드: `void TakeDamage(int damage)`
   - 프로퍼티: `bool IsDead { get; }` (읽기 전용)
2. `Monster` 클래스가 `IDamageable`을 구현하도록 수정하세요.
3. 새로운 클래스 `Barricade`(장애물)를 만드세요.
   - 이 클래스도 `IDamageable`을 구현합니다. (HP가 있고 데미지를 받으면 깎임)
   - 하지만 몬스터와 달리 "장애물이 파손되었습니다!"라는 로그를 남깁니다.
4. `IDamageable[] targets` 배열을 만들고 몬스터와 장애물을 섞어서 담으세요.
5. 반복문을 통해 모든 타겟을 공격하고, 인터페이스를 통해 동일한 방식으로 데미지를 입혀보세요.

**[프로그래밍 힌트]**
- 인터페이스는 `new`로 직접 만들 수 없지만, 배열의 타입(`IDamageable[]`)으로는 사용할 수 있습니다.
- `target.TakeDamage(100);`와 같이 인터페이스에 정의된 기능을 호출하면 실제 객체(Monster 또는 Barricade)의 로직이 실행됩니다.

