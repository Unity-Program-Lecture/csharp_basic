# 🚀 Day 08: 컬렉션과 제네릭 (만능 바구니와 틀)

오늘의 목표는 "**데이터 타입에 얽매이지 않는 만능 코드(제네릭)를 이해하고, 가장 많이 쓰이는 바구니인 List와 Dictionary를 배운다**"입니다.

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
```

### 1-3. 제네릭 인터페이스 (Generic Interface)
특정 타입에 의존하지 않는 기능의 약속을 정의합니다.
```csharp
public interface IRepository<T>
{
    void Save(T data);
    T Load();
}

// 인터페이스 구현 예시
public class StringItemRepository : IRepository<string>
{
    private string savedData;

    public void Save(string data)
    {
        savedData = data;
    }

    public string Load()
    {
        return savedData;
    }
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

## 2. List<T>: "늘어나는 배열"
데이터가 들어오는 대로 크기가 자동으로 늘어나는 가변 배열입니다.

### 📏 주요 도구 (Property & Method)
- **`Count`**: "지금 몇 개 들어있지?" - 현재 들어있는 데이터의 개수를 알려줍니다. (배열의 Length와 같음)
- **`Add(T)`**: "새 물건 추가!" - 리스트의 맨 마지막 칸에 데이터를 넣습니다.
- **`Remove(T)`**: "이거 버려줘" - 특정 데이터를 찾아서 삭제합니다. (성공하면 true, 없으면 false)
- **`RemoveAt(index)`**: "몇 번 칸 비워줘" - 번호(인덱스)를 지정해서 삭제합니다.
- **`Contains(T)`**: "이거 안에 있어?" - 특정 데이터가 들어있는지 확인합니다. (true/false)
- **`Clear()`**: "다 비워!" - 리스트의 모든 데이터를 한꺼번에 삭제합니다.

```csharp
// List 사용 예시
List<string> inventory = new List<string>();

inventory.Add("빨간 포션");
inventory.Add("낡은 검");
inventory.Remove("낡은 검");

if (inventory.Contains("빨간 포션"))
{
    Debug.Log($"보유 중! 개수: {inventory.Count}");
}
```

---

## 3. Dictionary<K, V>: "이름표가 있는 바구니"
번호 대신 내가 정한 이름표(Key)로 데이터를 찾는 바구니입니다.

### 🏷️ 주요 도구 (Property & Method)
- **`Count`**: "이름표가 총 몇 개지?" - 저장된 데이터(Key-Value 쌍)의 개수를 알려줍니다.
- **`Add(K, V)`**: "새 이름표와 물건 등록!" - 새로운 키와 값을 추가합니다. (이미 있는 키라면 에러 발생)
- **`Remove(K)`**: "이 이름표 떼줘" - 키를 지정해서 데이터를 삭제합니다.
- **`ContainsKey(K)`**: "이 이름표가 이미 있나?" - 특정 키가 존재하는지 확인합니다. (조회 전 필수 체크!)
- **`KeyValuePair<K, V>`**: "이름표와 물건 세트" - 딕셔너리에서 데이터를 하나 꺼낼 때 '키'와 '값'을 묶어서 담는 전용 상자입니다.

```csharp
// Dictionary 사용 및 순회 예시
Dictionary<string, int> itemCounts = new Dictionary<string, int>();

itemCounts["빨간 포션"] = 5;
itemCounts["파란 포션"] = 10;
itemCounts["낡은 검"] = 1;

// foreach를 사용한 딕셔너리 전체 순회
// kvp 상자 안에는 .Key와 .Value가 들어있습니다.
foreach (KeyValuePair<string, int> kvp in itemCounts)
{
    Debug.Log($"아이템: {kvp.Key}, 개수: {kvp.Value}개");
}
```

---

## 💻 종합 실습 예제: 제네릭 시스템 구축
**미션:** 제네릭을 사용하여 아이템을 저장하는 상자와 출력 시스템을 만들어 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using System.Collections.Generic;

// 1. 제네릭 인터페이스: "무언가를 담는 기능"의 약속
public interface IBox<T>
{
    void SetContent(T content);
    T GetContent();
}

// 2. 제네릭 클래스: 실제 상자 구현
public class ItemBox<T> : IBox<T>
{
    private T item;
    public void SetContent(T content) { item = content; }
    public T GetContent() { return item; }
}

// 3. 제네릭 구조체: 위치 정보를 제네릭으로 (int 또는 float)
public struct Point<T>
{
    public T x, y;
}

public class Day08_Practice : MonoBehaviour
{
    // 4. 제네릭 메소드: 데이터 타입을 가리지 않는 출력기
    void Logger<T>(string label, T value)
    {
        Debug.Log($"[{label}] : {value}");
    }

    void Start()
    {
        // 제네릭 클래스 사용
        ItemBox<string> nameBox = new ItemBox<string>();
        nameBox.SetContent("드래곤 슬레이어");
        Logger("무기 이름", nameBox.GetContent());

        ItemBox<int> countBox = new ItemBox<int>();
        countBox.SetContent(99);
        Logger("소지 개수", countBox.GetContent());

        // 제네릭 구조체 사용
        Point<float> fPos = new Point<float> { x = 10.5f, y = 20.2f };
        Logger("부드러운 좌표", $"({fPos.x}, {fPos.y})");

        // 기존 컬렉션 활용
        List<string> inventory = new List<string> { "포션", "검", "방패" };
        Logger("인벤토리 첫 칸", inventory[0]);
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. 리스트에서 데이터를 지울 때 사용하는 메소드는?
2. 딕셔너리에서 데이터를 찾기 위해 사용하는 '고유한 이름표'를 무엇이라 하나요?
3. 제네릭에서 사용하는 `<T>`는 보통 무엇의 약자인가요?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 7)]
고정된 크기의 배열 대신 **컬렉션(List, Dictionary)**을 사용하여 유연한 몬스터 관리 시스템을 만듭니다.

**[요구 사항]**
1. `List<Monster> monsterList`를 생성하고 몬스터를 자유롭게 추가/삭제해 봅니다.
   - `monsterList.RemoveAt(0);` 또는 `monsterList.Remove(target);`을 활용해 처치된 몬스터를 목록에서 제거하세요.
2. `Dictionary<string, Monster> monsterDict`를 생성합니다.
   - 키(Key)는 몬스터의 이름, 값(Value)은 몬스터 객체로 설정합니다.
   - `monsterDict["네임드 보스"].TakeDamage(500);`와 같이 이름표로 특정 몬스터를 즉시 찾아 공격하는 기능을 만드세요.
3. (도전) 제네릭 메소드 `void Spawn<T>(T entity) where T : IDamageable`를 만들어, 몬스터나 장애물을 생성하고 리스트에 추가하는 기능을 구현해 보세요.

**[프로그래밍 힌트]**
- `foreach`를 사용해 `monsterList` 전체를 순회하며 상태를 체크할 수 있습니다.
- `Dictionary`에 접근하기 전에는 `ContainsKey()`로 이름표가 존재하는지 확인하는 습관이 중요합니다.

