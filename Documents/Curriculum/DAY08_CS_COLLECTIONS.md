# 🚀 Day 08: 컬렉션과 제네릭 (만능 바구니와 틀)

오늘의 목표는 **"데이터 타입에 얽매이지 않는 만능 코드(제네릭)를 이해하고, 가장 많이 쓰이는 바구니인 List와 Dictionary를 배운다"**입니다.

---

## 1. 제네릭(Generics): "무엇이든 담는 만능 틀"
타입을 미리 정해두지 않고, 나중에 사용할 때 정해서 쓰는 기술입니다.
- **`<T>` (Type)**: "아직 타입을 정하지 않았다!"는 임시 이름표입니다.

---

## 2. List<T>: "늘어나는 배열"
배열은 크기가 고정되어 불편하지만, 리스트는 데이터가 들어오는 대로 크기가 자동으로 늘어납니다.
- **주요 기능**: `Add`(추가), `Remove`(삭제), `Count`(개수 확인), `[]`(인덱서 접근)

---

## 3. Dictionary<K, V>: "이름표가 있는 바구니"
번호(인덱스) 대신 내가 정한 이름표(Key)로 데이터를 찾는 바구니입니다.
- **특징**: "사과"라는 키로 "1000원"이라는 값(Value)을 아주 빠르게 찾아낼 수 있습니다.

---

## 💻 실습 예제: 인벤토리 시스템 기초
**미션:** 리스트를 사용하여 아이템 목록을 관리하고, 딕셔너리를 사용하여 아이템 이름으로 수량을 관리해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;
using System.Collections.Generic; // 컬렉션 필수!

class Program
{
    static void Main()
    {
        // 1. 리스트 (아이템 목록)
        List<string> inventory = new List<string>();
        inventory.Add("빨간 포션");
        inventory.Add("낡은 검");

        // 2. 딕셔너리 (아이템 수량)
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();
        itemCounts["빨간 포션"] = 5;
        itemCounts["낡은 검"] = 1;

        Console.WriteLine($"인벤토리 첫 번째 아이템: {inventory[0]}");
        Console.WriteLine($"포션 개수: {itemCounts["빨간 포션"]}개");
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. 리스트에서 데이터를 지울 때 사용하는 메소드는?
2. 딕셔너리에서 데이터를 찾기 위해 사용하는 '고유한 이름표'를 무엇이라 하나요?
3. 제네릭에서 사용하는 `<T>`는 보통 무엇의 약자인가요?
