# 🚀 [알고리즘 03] 데이터 정리의 미학: 정렬(Sorting)

학습 목표: 다양한 정렬 알고리즘의 원리를 이해하고, 이를 유니티의 인벤토리 시스템에 적용하여 아이템을 이름이나 등급순으로 정리하는 방법을 배웁니다.

---

## 💡 개념 설명 (NCS 알고리즘: 정렬 알고리즘)

### 1. 정렬(Sorting)이란?
뒤섞여 있는 데이터들을 일정한 규칙(오름차순, 내림차순)에 따라 순서대로 나열하는 것을 말합니다.

- **버블 정렬(Bubble Sort)**: 옆에 있는 데이터와 하나씩 비교하며 자리를 바꿉니다. 구현은 쉽지만 아주 느립니다 (O(N²)).
- **퀵 정렬(Quick Sort)**: 기준점(Pivot)을 정하고 데이터를 반으로 쪼개며 정렬합니다. 매우 빠릅니다 (O(N log N)).

### 2. 게임 개발에서의 활용: 인벤토리
플레이어가 획득한 수많은 아이템을 '공격력 순', '획득 날짜 순', '이름 순'으로 정렬할 때 정렬 알고리즘이 사용됩니다. C#에서는 기본적으로 최적화된 정렬 함수(`Sort`)를 제공하므로, 우리는 '어떤 기준'으로 정렬할지만 정해주면 됩니다.

---

## 💻 실습 예제

**미션:** 아이템 객체들을 담은 리스트를 '아이템 등급(Grade)'이 높은 순서대로 정렬하는 인벤토리 기능을 구현하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using System.Collections.Generic;
using System.Linq; // LINQ를 사용한 정렬이 편리합니다.

[System.Serializable]
public class Item
{
    public string name;
    public int grade; // 1: 일반, 2: 희귀, 3: 전설
}

public class InventorySort : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    void Start()
    {
        // 샘플 데이터 추가
        items.Add(new Item { name = "철검", grade = 1 });
        items.Add(new Item { name = "전설의 방패", grade = 3 });
        items.Add(new Item { name = "은반지", grade = 2 });

        Debug.Log("정렬 전:");
        PrintItems();

        // 실습 미션: 아이템 등급 기준 내림차순 정렬
        // C#의 List.Sort와 람다식을 사용한 정렬 알고리즘 적용
        items.Sort((itemA, itemB) => itemB.grade.CompareTo(itemA.grade));

        Debug.Log("등급순 정렬 후:");
        PrintItems();
    }

    void PrintItems()
    {
        foreach (var item in items)
        {
            Debug.Log($"이름: {item.name}, 등급: {item.grade}");
        }
    }
}
```

</details>

---

## ✍️ 정리 및 퀴즈

1. **질문**: 버블 정렬은 왜 대규모 데이터 정렬에 부적합할까요? 시간 복잡도 관점에서 설명해 보세요.
2. **질문**: 만약 등급이 같을 경우 '이름' 순으로 2차 정렬을 하고 싶다면 코드를 어떻게 수정해야 할까요?
