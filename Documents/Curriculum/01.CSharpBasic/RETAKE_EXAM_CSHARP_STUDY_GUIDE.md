# C# 기초 재시험 대비 학습 요약

이 문서는 재시험 문제를 그대로 보여 주지 않고, 좋은 성적을 받기 위해 반드시 익혀야 할 사고방식과 연습 방법을 정리한 자료입니다.

목표는 문제를 외우는 것이 아니라, 짧은 C# 코드를 보고 직접 실행 흐름을 설명하거나 작은 콘솔 프로그램을 완성할 수 있는 상태가 되는 것입니다.

## 1. 시험 전날 공부 순서

시간이 부족하면 아래 순서대로 공부합니다.

1. 코드 출력 결과 예측
2. 배열, 조건문, 반복문 손으로 따라가기
3. 클래스, 프로퍼티, 생성자 구분하기
4. 상속, 오버라이딩, 인터페이스 말로 설명하기
5. 작은 콘솔 프로그램을 처음부터 끝까지 한 번 작성하기

가장 중요한 습관은 코드 독해 3원칙입니다.

- **위->아래**: 코드는 기본적으로 위에서 아래로 실행됩니다.
- **오->왼**: 대입문은 오른쪽 값을 먼저 계산한 뒤 왼쪽 변수에 넣습니다.
- **안->밖**: 괄호, 메소드 호출, 배열 접근처럼 안쪽 표현식을 먼저 봅니다.

## 2. 코드 출력 결과 예측법

출력 결과 문제는 머릿속으로만 풀면 실수하기 쉽습니다. 반드시 종이에 변수 표를 만들고 한 줄씩 따라갑니다.

예시:

```csharp
int hp = 100;
int damage = 35;

hp = hp - damage;

Console.WriteLine(hp);
```

읽는 순서:

1. `hp`라는 상자에 `100`을 넣습니다.
2. `damage`라는 상자에 `35`를 넣습니다.
3. 오른쪽 `hp - damage`를 먼저 계산합니다.
4. 계산 결과 `65`를 왼쪽 `hp`에 다시 넣습니다.
5. `hp`를 출력합니다.

연습할 때는 아래처럼 표를 씁니다.

| 줄 | hp | damage | 설명 |
| :--- | :--- | :--- | :--- |
| 시작 | - | - | 아직 값이 없음 |
| `int hp = 100;` | 100 | - | hp 상자 생성 |
| `int damage = 35;` | 100 | 35 | damage 상자 생성 |
| `hp = hp - damage;` | 65 | 35 | 오른쪽 계산 후 hp 갱신 |

## 3. 값 복사와 참조 감각

기초 시험에서는 먼저 값 복사를 확실히 잡아야 합니다.

```csharp
int playerLevel = 3;
int savedLevel = playerLevel;

savedLevel = savedLevel + 2;

Console.WriteLine(playerLevel);
Console.WriteLine(savedLevel);
```

`savedLevel = playerLevel;`은 두 변수를 한 몸으로 묶는 것이 아닙니다. 그 순간 `playerLevel` 안에 있던 값을 복사해서 `savedLevel` 상자에 넣는 것입니다. 이후 `savedLevel`을 바꾸어도 `playerLevel`은 자동으로 바뀌지 않습니다.

자주 하는 실수:

- `a = b;`를 "a와 b가 계속 연결된다"로 이해함
- `a = a + 1;`에서 왼쪽 `a`를 먼저 바꾸려고 함
- 출력문이 여러 개일 때 마지막 값만 보고 중간 변화를 놓침

## 4. 배열과 인덱스

배열은 같은 종류의 값을 여러 칸에 나란히 넣은 보관함입니다.

```csharp
int[] prices = { 100, 250, 400 };

Console.WriteLine(prices[0]);
Console.WriteLine(prices[1]);
Console.WriteLine(prices[2]);
```

배열에서 첫 번째 칸은 `0`번입니다.

| 코드 | 의미 | 값 |
| :--- | :--- | :--- |
| `prices[0]` | 첫 번째 칸 | 100 |
| `prices[1]` | 두 번째 칸 | 250 |
| `prices[2]` | 세 번째 칸 | 400 |

시험장에서 꼭 확인할 것:

- 배열 길이가 3이면 마지막 인덱스는 `2`입니다.
- `Length`는 배열의 칸 개수입니다.
- `for`문에서 `i < array.Length`인지 확인합니다.
- `i <= array.Length`는 마지막에 범위를 벗어날 수 있습니다.

## 5. 조건문

조건문은 갈림길입니다. 위에서부터 조건을 검사하고, 처음으로 참인 블록만 실행합니다.

```csharp
int stamina = 45;

if (stamina >= 80)
{
    Console.WriteLine("강공격");
}
else if (stamina >= 40)
{
    Console.WriteLine("일반공격");
}
else
{
    Console.WriteLine("휴식");
}
```

읽는 순서:

1. `stamina >= 80`을 검사합니다.
2. 거짓이면 다음 `else if`로 내려갑니다.
3. `stamina >= 40`이 참이면 해당 블록을 실행합니다.
4. 한 블록이 실행되면 나머지 `else`는 보지 않습니다.

자주 하는 실수:

- 모든 `if`, `else if`, `else`가 실행된다고 생각함
- `>=`, `>`, `<=`, `<`의 경계값을 대충 봄
- 위쪽 조건이 먼저 잡아먹는 범위를 놓침

## 6. 반복문과 누적

반복문은 같은 일을 여러 번 시키는 장치입니다. 누적 문제에서는 `total`, `sum`, `count` 같은 변수가 어떻게 바뀌는지 한 칸씩 추적해야 합니다.

```csharp
int[] rewards = { 5, 10, 15 };
int total = 0;

for (int i = 0; i < rewards.Length; i++)
{
    total = total + rewards[i];
}

Console.WriteLine(total);
```

손으로 풀 때는 이렇게 씁니다.

| 반복 | i | rewards[i] | total 변화 |
| :--- | :--- | :--- | :--- |
| 시작 | - | - | 0 |
| 1회 | 0 | 5 | 0 + 5 = 5 |
| 2회 | 1 | 10 | 5 + 10 = 15 |
| 3회 | 2 | 15 | 15 + 15 = 30 |

`i++`은 반복 블록이 끝난 뒤 `i`를 1 증가시킨다고 생각하면 됩니다.

## 7. 메소드

메소드는 자주 쓰는 행동을 이름 붙여 따로 빼 둔 기능입니다.

```csharp
static int AddScore(int currentScore, int bonus)
{
    return currentScore + bonus;
}

int result = AddScore(70, 15);
Console.WriteLine(result);
```

읽는 순서:

1. `AddScore(70, 15)`가 호출됩니다.
2. `currentScore`에는 `70`, `bonus`에는 `15`가 들어갑니다.
3. `return currentScore + bonus;`가 `85`를 돌려줍니다.
4. `result`에 `85`가 저장됩니다.

기억할 말:

- 매개변수는 메소드가 일을 할 때 받는 재료입니다.
- `return`은 메소드가 계산한 결과를 호출한 곳으로 돌려주는 문입니다.
- `void` 메소드는 돌려주는 결과가 없습니다.

## 8. 클래스, 객체, 프로퍼티

클래스는 설계도이고, 객체는 설계도로 만든 실제 물건입니다.

```csharp
class Item
{
    public string Name { get; set; }
    public int Price { get; set; }
}

Item potion = new Item();
potion.Name = "Potion";
potion.Price = 50;

Console.WriteLine(potion.Name);
Console.WriteLine(potion.Price);
```

구분해서 외우기:

| 용어 | 쉬운 설명 |
| :--- | :--- |
| 클래스 | 어떤 데이터와 기능을 가질지 적어 둔 설계도 |
| 객체 | `new`로 실제로 만든 것 |
| 필드/프로퍼티 | 객체가 가지고 있는 값 |
| `get` | 값을 읽을 수 있음 |
| `set` | 값을 바꿀 수 있음 |

시험에서 프로퍼티가 나오면 "이 객체가 어떤 정보를 저장하고, 그 값을 읽거나 바꾸는 코드가 어디인가?"를 찾으면 됩니다.

## 9. 생성자

생성자는 객체가 만들어지는 순간 자동으로 실행되는 초기 설정 코드입니다.

```csharp
class Skill
{
    public string Name { get; set; }
    public int ManaCost { get; set; }

    public Skill(string name, int manaCost)
    {
        Name = name;
        ManaCost = manaCost;
    }
}

Skill fireball = new Skill("Fireball", 20);
```

읽는 순서:

1. `new Skill("Fireball", 20)`을 만납니다.
2. `Skill(string name, int manaCost)` 생성자가 실행됩니다.
3. `name`에는 `"Fireball"`, `manaCost`에는 `20`이 들어갑니다.
4. 생성자 안에서 프로퍼티 `Name`, `ManaCost`가 초기화됩니다.

생성자의 특징:

- 이름이 클래스 이름과 같습니다.
- 반환형을 쓰지 않습니다.
- 객체를 만들 때 필요한 초기값을 넣기 좋습니다.

## 10. 상속과 오버라이딩

상속은 공통 기능을 부모 클래스에 두고, 자식 클래스가 이어받는 구조입니다.

```csharp
class Character
{
    public virtual void Attack()
    {
        Console.WriteLine("기본 공격");
    }
}

class Archer : Character
{
    public override void Attack()
    {
        Console.WriteLine("화살 공격");
    }
}

Character character = new Archer();
character.Attack();
```

핵심:

- `virtual`: 자식이 바꿀 수 있도록 허용합니다.
- `override`: 부모의 기능을 자식 방식으로 다시 작성합니다.
- 부모 타입 변수에 자식 객체를 담아도, 오버라이딩된 메소드는 실제 객체 기준으로 실행됩니다.

말로 설명할 수 있어야 하는 문장:

> 상속은 공통 규칙을 부모 클래스에 모아 중복을 줄이고, 오버라이딩은 같은 이름의 행동을 자식 클래스에 맞게 다르게 실행하는 방법입니다.

## 11. 구조체

구조체는 작은 데이터 묶음을 표현할 때 사용할 수 있습니다.

```csharp
struct Position
{
    public int X;
    public int Y;
}

Position start;
start.X = 2;
start.Y = 5;

Console.WriteLine(start.X);
Console.WriteLine(start.Y);
```

처음에는 구조체를 "작은 정보 카드"라고 생각하면 됩니다. 예를 들어 좌표, 색상, 간단한 수치 묶음처럼 값 자체가 중요한 데이터를 담을 때 자주 등장합니다.

기초 단계에서 꼭 알 것:

- `struct`도 여러 값을 하나로 묶을 수 있습니다.
- 클래스처럼 멤버를 가질 수 있습니다.
- 단순한 데이터 묶음을 읽고 쓰는 코드를 해석할 수 있어야 합니다.

## 12. 인터페이스

인터페이스는 "이 기능을 반드시 만들겠다"는 약속입니다.

```csharp
interface IUsable
{
    void Use();
}

class HealPotion : IUsable
{
    public void Use()
    {
        Console.WriteLine("체력을 회복합니다.");
    }
}
```

핵심 문장:

> 인터페이스는 구체적인 행동 내용을 직접 정하기보다, 어떤 메소드를 반드시 가져야 하는지 약속하는 역할을 합니다.

구분:

| 코드 | 의미 |
| :--- | :--- |
| `interface IUsable` | 사용 가능한 기능의 약속 |
| `void Use();` | 반드시 만들어야 하는 메소드 이름 |
| `class HealPotion : IUsable` | 이 클래스는 그 약속을 지킴 |
| `public void Use()` | 약속한 기능의 실제 내용 |

## 13. 컬렉션과 제네릭

배열은 크기가 고정되어 있지만, `List<T>`는 값을 추가하거나 제거하기 쉽습니다.

```csharp
List<string> names = new List<string>();

names.Add("Sword");
names.Add("Shield");

Console.WriteLine(names.Count);
Console.WriteLine(names[0]);
```

`<string>`은 이 목록에 문자열만 넣겠다는 뜻입니다. 이것이 제네릭의 기본 감각입니다.

자주 나오는 코드:

```csharp
foreach (string name in names)
{
    Console.WriteLine(name);
}
```

읽는 말:

> names 안에 있는 문자열을 하나씩 꺼내서 name이라는 이름으로 사용한다.

기초 단계에서 꼭 알 것:

- `List<int>`는 정수 목록입니다.
- `List<string>`은 문자열 목록입니다.
- `Add`는 값을 추가합니다.
- `Count`는 현재 들어 있는 개수입니다.
- `foreach`는 처음부터 끝까지 하나씩 꺼낼 때 편합니다.

## 14. 작은 콘솔 프로그램 작성 연습

시험 대비용으로 아래 프로그램을 직접 만들어 봅니다. 실제 시험 문제를 복제한 것이 아니라, 같은 기초 체력을 기르기 위한 연습입니다.

### 연습 과제: 아이템 목록 관리 프로그램

요구 사항:

1. `Item` 클래스를 만듭니다.
2. `Name`, `Price` 프로퍼티를 만듭니다.
3. 생성자로 이름과 가격을 초기화합니다.
4. `List<Item>`에 아이템 3개를 추가합니다.
5. 반복문으로 전체 아이템 이름과 가격을 출력합니다.
6. 전체 가격 합계를 계산해서 출력합니다.
7. 가격이 특정 값 이상인 아이템만 따로 출력합니다.

예시 코드:

```csharp
using System;
using System.Collections.Generic;

class Item
{
    public string Name { get; set; }
    public int Price { get; set; }

    public Item(string name, int price)
    {
        Name = name;
        Price = price;
    }
}

class Program
{
    static void Main()
    {
        List<Item> items = new List<Item>();

        items.Add(new Item("Potion", 50));
        items.Add(new Item("Sword", 300));
        items.Add(new Item("Shield", 200));

        int totalPrice = 0;

        foreach (Item item in items)
        {
            Console.WriteLine(item.Name + " / " + item.Price);
            totalPrice += item.Price;
        }

        Console.WriteLine("Total: " + totalPrice);

        foreach (Item item in items)
        {
            if (item.Price >= 200)
            {
                Console.WriteLine("Expensive: " + item.Name);
            }
        }
    }
}
```

이 예제를 외우지 말고, 다음 질문에 답하면서 이해합니다.

- `Item` 클래스는 어떤 값을 저장하나요?
- 생성자는 언제 실행되나요?
- `items.Add(...)`는 무엇을 추가하나요?
- 첫 번째 `foreach`에서는 무엇을 누적하나요?
- 두 번째 `foreach`의 `if`는 어떤 아이템만 골라내나요?

## 15. 채점에서 점수를 잃지 않는 답안 습관

서술형 답안은 길게 쓰는 것보다 정확하게 쓰는 것이 중요합니다.

좋은 답안의 모양:

- 용어를 한 문장으로 정의합니다.
- 코드에서 실제로 일어나는 일을 한 줄씩 설명합니다.
- 결과만 쓰지 말고, 왜 그렇게 되는지 짧게 붙입니다.
- 클래스, 객체, 생성자, 프로퍼티를 서로 섞어 쓰지 않습니다.

나쁜 답안의 모양:

- "그냥 실행된다"처럼 설명이 너무 넓습니다.
- 출력 결과만 쓰고 중간 계산을 설명하지 않습니다.
- `class`와 `new`의 차이를 구분하지 않습니다.
- 반복문의 시작값, 종료 조건, 증가식을 확인하지 않습니다.

## 16. 최종 점검표

시험장에 들어가기 전 아래 항목에 스스로 체크합니다.

| 항목 | 체크 |
| :--- | :--- |
| 대입문을 오른쪽 먼저 계산한다고 설명할 수 있다. |  |
| 배열 인덱스가 0부터 시작한다는 것을 안다. |  |
| `if`, `else if`, `else` 중 하나의 흐름만 선택되는 상황을 설명할 수 있다. |  |
| `for`문의 시작값, 조건식, 증가식을 보고 반복 횟수를 셀 수 있다. |  |
| 누적 변수의 값 변화를 표로 추적할 수 있다. |  |
| 클래스와 객체의 차이를 설명할 수 있다. |  |
| 프로퍼티의 `get`, `set` 역할을 설명할 수 있다. |  |
| 생성자가 객체 생성 시 초기값을 넣는 코드라는 것을 안다. |  |
| `virtual`과 `override`의 관계를 설명할 수 있다. |  |
| 인터페이스가 기능 구현 약속이라는 것을 설명할 수 있다. |  |
| `List<T>`에 값을 추가하고 반복 출력하는 코드를 작성할 수 있다. |  |

## 17. 마지막 30분 복습법

마지막에는 새 내용을 더 넣기보다 실수를 줄입니다.

1. 출력 결과 예제 3개를 손으로 풉니다.
2. 배열과 반복문 예제 1개를 표로 풉니다.
3. 클래스 예제 1개를 직접 타이핑합니다.
4. 상속, 인터페이스, 제네릭을 각각 한 문장으로 설명해 봅니다.
5. 작은 콘솔 프로그램에서 `using System.Collections.Generic;`, `Main`, 클래스 이름, 중괄호가 빠지지 않았는지 확인합니다.

가장 좋은 답안은 화려한 답안이 아니라, 컴퓨터가 실제로 어떤 순서로 움직이는지 차분하게 따라간 답안입니다.
