# DAY 05: 인벤토리와 아이템 이벤트

오늘의 목표는 인벤토리를 "**캐릭터가 들고 다니는 작은 창고**"처럼 이해하고, 아이템 획득과 사용 이벤트가 캐릭터 상태에 반영되는 흐름을 만드는 것입니다.

## 1. 핵심 개념: "아이템은 데이터이고, 사용은 이벤트다"

아이템은 이름, 종류, 효과 값을 가진 데이터입니다. 플레이어가 아이템을 줍는 순간에는 인벤토리에 데이터가 추가되고, 아이템을 사용하는 순간에는 체력 회복, 속도 증가, 문 열기 같은 이벤트가 실행됩니다.

처음부터 장비 교체, 중첩, 저장, 서버 동기화를 모두 만들면 너무 커집니다. 이번 수업에서는 "아이템을 얻는다 -> 목록에 넣는다 -> 사용한다 -> 효과가 적용된다" 흐름만 작게 구현합니다.

### 이 단어는 무슨 뜻인가요?

- **Inventory**: 캐릭터가 가진 아이템 목록입니다.
- **Item Data**: 아이템의 이름, 종류, 효과 값 같은 정보입니다.
- **Consumable**: 사용하면 사라지는 소비 아이템입니다.
- **Event**: 어떤 일이 발생했을 때 실행되는 처리입니다.
- **Effect**: 아이템 사용 결과로 캐릭터나 월드에 적용되는 변화입니다.

## 실습 예제: 회복 아이템 줍고 사용하기

**미션:** 아이템 데이터를 인벤토리에 추가하고, 첫 번째 아이템을 사용해 체력을 회복합니다.

1. 빈 GameObject를 만들고 `SimpleInventory` 스크립트를 붙입니다.
2. Input System 패키지가 설치되어 있는지 확인합니다.
3. `Space` 키로 예시 아이템을 추가하고, `Enter` 키로 첫 아이템을 사용합니다.

<details>
<summary>코드 보기</summary>

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleInventory : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 50;

    private readonly List<ItemData> items = new List<ItemData>();

    void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AddItem(new ItemData("Small Potion", 20));
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            UseFirstItem();
        }
    }

    private void AddItem(ItemData item)
    {
        items.Add(item);
        Debug.Log($"{item.Name}을(를) 인벤토리에 넣었습니다.");
    }

    private void UseFirstItem()
    {
        if (items.Count == 0)
        {
            Debug.Log("사용할 아이템이 없습니다.");
            return;
        }

        ItemData item = items[0];
        items.RemoveAt(0);

        currentHealth = Mathf.Min(currentHealth + item.HealAmount, maxHealth);
        Debug.Log($"{item.Name} 사용. 현재 체력: {currentHealth}/{maxHealth}");
    }
}

public class ItemData
{
    public string Name { get; }
    public int HealAmount { get; }

    public ItemData(string name, int healAmount)
    {
        Name = name;
        HealAmount = healAmount;
    }
}
```

</details>

### 실행해보면

`Space`를 누르면 회복 아이템이 목록에 추가됩니다. `Enter`를 누르면 첫 번째 아이템이 사라지고 체력이 회복됩니다.

### 생각해보기

1. 아이템을 문자열 하나로만 저장하면 나중에 어떤 정보가 부족해질까요?
2. 회복 아이템과 열쇠 아이템은 같은 방식으로 사용할 수 있을까요, 아니면 구분이 필요할까요?

## 오늘의 정리

- 인벤토리는 아이템 데이터를 보관하는 목록입니다.
- 아이템 사용은 캐릭터나 월드에 변화를 일으키는 이벤트입니다.
- 큰 시스템도 획득, 보관, 사용, 효과 적용 순서로 나누면 작게 만들 수 있습니다.
