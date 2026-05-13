# 🚀 Day 08: 제네릭과 제약 조건 (만능 틀과 문지기)

오늘의 목표는 "**데이터 타입에 얽매이지 않는 만능 코드(제네릭)를 이해하고, 제네릭에 문지기를 세우는 where 절을 마스터한다**"입니다.

---

## 1. 제네릭(Generics): "무엇이든 담는 만능 틀"
타입을 미리 정해두지 않고, 나중에 사용할 때 정해서 쓰는 기술입니다. (`<T>`)

### 1-1. 제네릭 메소드 (Generic Method)
매개변수 타입이 달라도 로직이 같다면 하나로 묶을 수 있습니다.
```csharp
void PrintData<T>(T data)
{
    Debug.Log($"데이터 출력: {data}");
}

// 사용 예시
PrintData<int>(10);
PrintData<string>("Hello");
```

### 1-2. 제네릭 클래스 & 구조체 (Generic Class/Struct)
데이터를 보관하는 틀 자체를 제네릭으로 만듭니다.
```csharp
public class ItemBox<T>
{
    public T item;
    
    public void SetItem(T newItem)
    {
        item = newItem;
    }

    public T GetItem()
    {
        return item;
    }
}

public struct Pair<T>
{
    public T first;
    public T second;
}

// 사용 예시
ItemBox<int> intBox = new ItemBox<int>();
intBox.SetItem(100);

Pair<string> namePair = new Pair<string> { first = "Kim", second = "Lee" };
```

### 1-3. 제네릭 인터페이스 (Generic Interface)
특정 타입에 의존하지 않는 기능의 약속을 정의합니다.
```csharp
public interface IRepository<T>
{
    void Save(T data);
    T Load();
}

public class ItemRepository<T> : IRepository<T>
{
    private T savedData;

    public void Save(T data)
    {
        savedData = data;
    }

    public T Load()
    {
        return savedData;
    }
}
```

---

## 2. 제네릭 제약 조건 (where): "만능 틀의 문지기"
"이런 특징을 가진 놈만 들어와!"라고 제한하는 문구입니다.

```csharp
// T는 반드시 클래스여야 함
class Box<T> where T : class { }

// T는 반드시 특정 인터페이스를 구현해야 함
void Attack<T>(T target) where T : IDamageable { }

// T는 반드시 값 형식(struct)이어야 함
void PrintValue<T>(T data) where T : struct { }
```

---

## 💻 실습 예제: 제약 조건이 있는 제네릭 시스템
```csharp
using UnityEngine;

public interface IDamageable { void TakeDamage(int amount); }

public class Day08_Practice : MonoBehaviour
{
    // 문지기: IDamageable 인터페이스를 구현한 대상만 공격 가능!
    void GenericAttack<T>(T target, int damage) where T : IDamageable
    {
        Debug.Log("제네릭 공격 시스템 가동");
        target.TakeDamage(damage);
    }

    void Start()
    {
        // 1. 제네릭 클래스 사용
        ItemBox<string> nameBox = new ItemBox<string>();
        nameBox.SetItem("엑스칼리버");
        
        // 2. 제약 조건 테스트 (IDamageable 구현체가 필요)
        // GenericAttack("나무", 10); // 에러 발생: string은 IDamageable이 아님
    }
}
```

---

## ✍️ 핵심 퀴즈
1. 제네릭에서 사용하는 `<T>`는 보통 무엇의 약자인가요?
2. 제네릭에서 T가 반드시 클래스여야 한다고 제한할 때 사용하는 코드는?
3. 인터페이스를 제약 조건으로 걸었을 때의 장점은 무엇인가요?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 7)]
**제네릭**과 **제약 조건**을 활용하여 어떤 대상이든 안전하게 생성하고 관리하는 시스템을 구축합니다.

**[요구 사항]**
1. 제네릭 메소드 `void Spawn<T>(T entity) where T : IDamageable`를 만듭니다.
   - `IDamageable` 인터페이스를 가진 대상만 인자로 받아 소환 로그를 남깁니다.
2. 제네릭 클래스 `Storage<T>`를 만듭니다.
   - `T`는 반드시 `class`여야 합니다.
   - 객체를 저장하고 꺼내는 기능을 가집니다.
3. 위 시스템을 사용하여 몬스터를 생성하고 저장소에 보관하는 로직을 작성하세요.

**[프로그래밍 힌트]**
- `where T : class, IDamageable`과 같이 여러 제약 조건을 동시에 걸 수도 있습니다.
- 제약 조건을 활용하면 제네릭 함수 내부에서 `target.TakeDamage()`와 같은 인터페이스 기능을 바로 호출할 수 있어 편리합니다.
