# C# 제네릭 `where` 제약 조건

제네릭에서 `where` 제약 조건은 형식 매개변수(`T`)가 반드시 갖춰야 할 **'최소한의 기능'**을 명시합니다. 이를 통해 컴파일러는 해당 기능을 안전하게 호출할 수 있도록 허용합니다.

---

## 1. `struct` (안심하고 값 형식만 받기)
입력받는 타입이 반드시 **값 형식(Value Type)**이어야 할 때 사용합니다. `null`이 될 수 없음을 보장합니다.

```csharp
public class Calculator<T> where T : struct
{
    public void Reset(ref T value)
    {
        // struct 제약 덕분에 T는 절대 null일 수 없으며 기본값을 가집니다.
        value = default(T); 
    }
}

// 사용 예시
var calc = new Calculator<int>();      // OK
// var calc2 = new Calculator<string>(); // Error (string은 참조 형식)
```

---

## 2. `class` (참조 형식으로 제한하기)
입력받는 타입이 반드시 **참조 형식(Reference Type)**이어야 할 때 사용합니다.

```csharp
public class DataHandler<T> where T : class
{
    public void CheckNull(T item)
    {
        // class 제약이 있어야만 null과 직접 비교하는 것이 의미가 있습니다.
        if (item == null) Console.WriteLine("Data is null");
    }
}

// 사용 예시
var handler = new DataHandler<string>(); // OK
// var handler2 = new DataHandler<int>(); // Error (int는 값 형식)
```

---

## 3. `new()` (내부에서 객체 생성하기)
제네릭 클래스 안에서 `new T()`를 사용하여 **새로운 인스턴스를 만들어야 할 때** 필수입니다. 해당 타입은 반드시 매개변수가 없는 `public` 생성자를 가져야 합니다.

```csharp
public class Factory<T> where T : new()
{
    public T Create()
    {
        // new() 제약이 없으면 컴파일러는 T를 생성할 수 있는지 알 수 없습니다.
        return new T(); 
    }
}

// 사용 예시
var factory = new Factory<List<int>>(); // OK (List는 기본 생성자가 있음)
```

---

## 4. `기반 클래스` (특정 부모를 가진 타입으로 제한)
특정 클래스를 **상속받은 자식 클래스**만 받도록 제한합니다. 이를 통해 부모의 메서드를 직접 호출할 수 있습니다.

```csharp
public class Animal { public virtual void Speak() => Console.WriteLine("..."); }
public class Dog : Animal { public override void Speak() => Console.WriteLine("Woof!"); }

public class Shelter<T> where T : Animal
{
    public void MakeSound(T animal)
    {
        // T가 Animal임이 보장되므로 Speak() 메서드를 호출할 수 있습니다.
        animal.Speak(); 
    }
}
```

---

## 5. `인터페이스` (특정 기능 구현 여부 보장)
가장 많이 쓰이는 제약 조건입니다. 특정 **인터페이스를 구현**한 타입만 허용하여, 해당 인터페이스의 기능을 사용합니다.

```csharp
public class ComparisonTool<T> where T : IComparable<T>
{
    public int Compare(T a, T b)
    {
        // IComparable 제약 덕분에 CompareTo 메서드 사용이 가능합니다.
        return a.CompareTo(b);
    }
}

// 사용 예시
var tool = new ComparisonTool<int>(); // OK (int는 IComparable 구현체)
```

---

## 6. `notnull` (Null 원천 봉쇄)
C# 8.0 이상에서 도입되었으며, 참조/값 형식에 상관없이 **null을 허용하지 않는 타입**만 받습니다.

```csharp
public class SafeLogger<T> where T : notnull
{
    public void Log(T message)
    {
        // T가 절대 null이 아님을 컴파일러가 감시합니다.
        Console.WriteLine(message.ToString());
    }
}

// 사용 예시
var logger = new SafeLogger<string>();   // OK
// var logger2 = new SafeLogger<string?>(); // Warning/Error (Null 허용 타입)
```

---

## 7. 복합 제약 조건 (여러 개를 동시에 사용)
필요에 따라 여러 제약을 조합할 수 있습니다. (순서: class/struct -> 인터페이스 -> new())

```csharp
public class Repository<T> where T : class, IDisposable, new()
{
    public void Process()
    {
        using (T item = new T()) 
        {
            // T는 클래스이고, 생성 가능하며, Dispose 기능이 있음이 보장됨
        }
    }
}
```


### 💡 팁
* **`struct`와 `new()`**: `struct` 제약 조건을 사용하면 자동으로 매개변수 없는 생성자가 포함된 것으로 간주되므로, 둘을 동시에 쓸 필요는 없습니다.
* **인터페이스 제약의 장점**: 인터페이스 제약을 걸면 **박싱(Boxing)** 없이 인터페이스 메서드를 호출할 수 있어 성능상 유리합니다.