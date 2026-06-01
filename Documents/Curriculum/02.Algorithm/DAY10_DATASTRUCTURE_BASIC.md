# Day 10: 게임 자료구조 - 데이터를 담는 상자 고르기

오늘의 목표는 "**게임 데이터의 성격에 맞게 List, Queue, Stack, Dictionary를 구분하고, 인벤토리 예제를 통해 순서 저장과 빠른 검색의 차이를 눈으로 확인하는 것**"입니다.

게임은 계속 데이터를 다룹니다.

> "아이템을 몇 번째 슬롯에 넣을까?"
> "대기 중인 이벤트를 어떤 순서로 처리할까?"
> "방금 연 메뉴로 되돌아가려면 무엇을 기억해야 할까?"
> "아이템 ID로 정보를 바로 찾으려면 어떻게 해야 할까?"

자료구조는 이런 데이터를 담는 상자입니다. 모든 상자가 같은 모양이 아니듯, 모든 데이터에 같은 자료구조를 쓰면 불편해집니다.

## 1. 핵심 개념: "상황에 맞는 상자 선택하기"

인벤토리는 순서가 중요합니다. 0번 슬롯, 1번 슬롯처럼 위치가 있고, 학생이 화면에서 바로 확인할 수 있습니다. 이런 경우에는 `List<T>`처럼 순서대로 담는 구조가 자연스럽습니다.

반대로 아이템 ID가 `1003`인 아이템을 바로 찾아야 한다면, 처음부터 끝까지 하나씩 뒤지는 방식은 느릴 수 있습니다. 이때는 이름표를 붙여 찾는 `Dictionary<TKey, TValue>`가 어울립니다.

### 이 단어는 무슨 뜻인가요?

#### 자료구조

데이터를 저장하고 꺼내기 위한 방법입니다. 같은 데이터라도 어떻게 담느냐에 따라 추가, 삭제, 검색의 편의성이 달라집니다.

#### List

순서대로 데이터를 담는 상자입니다. 인벤토리 슬롯, 스테이지 목록, 대화 문장 목록처럼 순서가 중요한 데이터에 어울립니다.

#### Queue

먼저 들어온 데이터가 먼저 나가는 줄서기 구조입니다. 매칭 대기열, 이벤트 처리 순서, 알림 표시 순서에 어울립니다.

#### Stack

나중에 들어온 데이터가 먼저 나가는 접시 쌓기 구조입니다. 뒤로 가기, 실행 취소, 메뉴 열기 기록에 어울립니다.

#### Dictionary

Key로 Value를 찾는 이름표 구조입니다. 아이템 ID로 아이템 정보 찾기, 몬스터 이름으로 능력치 찾기처럼 빠른 검색이 중요할 때 사용합니다.

## 2. 어떤 상황에 무엇을 쓸까?

| 상황 | 어울리는 자료구조 | 이유 |
| --- | --- | --- |
| 인벤토리 슬롯을 순서대로 보여준다 | `List<T>` | 몇 번째 칸인지가 중요하다 |
| 먼저 들어온 알림을 먼저 보여준다 | `Queue<T>` | 먼저 온 것이 먼저 처리된다 |
| 가장 최근 행동부터 되돌린다 | `Stack<T>` | 마지막 행동을 먼저 꺼낸다 |
| 아이템 ID로 정보를 바로 찾는다 | `Dictionary<TKey, TValue>` | Key를 이용해 빠르게 찾는다 |

자료구조를 고를 때는 "무엇을 자주 하는가?"를 먼저 봅니다.

- 순서대로 보여주는 일이 많으면 `List`
- 들어온 순서대로 처리해야 하면 `Queue`
- 마지막 작업부터 되돌려야 하면 `Stack`
- 고유한 이름표로 빨리 찾아야 하면 `Dictionary`

## 실습 예제: Gizmos로 보는 인벤토리 List와 Dictionary

**미션:** `List`로 인벤토리 슬롯 순서를 만들고, `Dictionary`로 아이템 ID를 빠르게 찾아 강조 표시합니다.

### 준비하기

1. Unity 씬에 빈 오브젝트 `InventoryVisualizer`를 만듭니다.
2. 아래 스크립트를 붙입니다.
3. Scene 뷰 오른쪽 위의 `Gizmos` 버튼을 켭니다.
4. Play 모드에서 `A`, `Backspace`, `F`, 왼쪽/오른쪽 방향키를 눌러 인벤토리 변화를 확인합니다.

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryStructureVisualizer : MonoBehaviour
{
    private class InventoryItem
    {
        public int ItemId { get; }
        public string ItemName { get; }
        public Color SlotColor { get; }

        public InventoryItem(int itemId, string itemName, Color slotColor)
        {
            ItemId = itemId;
            ItemName = itemName;
            SlotColor = slotColor;
        }
    }

    [Header("Inventory")]
    [Tooltip("인벤토리에 들어갈 수 있는 최대 슬롯 수입니다.")]
    [SerializeField] private int maxSlotCount = 8;

    [Tooltip("Scene 뷰에 그릴 슬롯 한 칸의 크기입니다.")]
    [SerializeField] private float slotSize = 0.8f;

    [Tooltip("슬롯 사이의 간격입니다.")]
    [SerializeField] private float slotGap = 0.15f;

    // List는 슬롯 순서를 그대로 보관합니다. 0번, 1번, 2번처럼 인덱스로 접근할 수 있습니다.
    private readonly List<InventoryItem> inventory = new List<InventoryItem>();

    // Dictionary는 ItemId를 Key로 사용해 해당 아이템이 몇 번째 슬롯에 있는지 빠르게 찾습니다.
    private readonly Dictionary<int, int> slotIndexByItemId = new Dictionary<int, int>();

    private int nextItemId = 1000;
    private int selectedSlotIndex;
    private int highlightedItemId = -1;

    private void Update()
    {
        // Keyboard.current는 현재 연결된 키보드 장치를 가져오는 Input System 프로퍼티입니다.
        if (Keyboard.current == null)
        {
            return;
        }

        // wasPressedThisFrame은 이번 프레임에 막 눌린 순간에만 true가 됩니다.
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            AddItem();
        }

        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            RemoveSelectedItem();
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            selectedSlotIndex--;
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            selectedSlotIndex++;
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            HighlightNewestItemByDictionary();
        }

        // Mathf.Clamp는 선택 번호가 0보다 작거나 마지막 슬롯을 넘지 않도록 막습니다.
        selectedSlotIndex = Mathf.Clamp(selectedSlotIndex, 0, Mathf.Max(0, inventory.Count - 1));
    }

    private void AddItem()
    {
        if (inventory.Count >= maxSlotCount)
        {
            return;
        }

        int itemId = nextItemId;
        nextItemId++;

        string itemName = "Item " + itemId;
        Color slotColor = GetColorBySlot(inventory.Count);
        InventoryItem item = new InventoryItem(itemId, itemName, slotColor);

        // Add는 List의 맨 뒤에 새 데이터를 추가합니다.
        inventory.Add(item);
        slotIndexByItemId[item.ItemId] = inventory.Count - 1;

        selectedSlotIndex = inventory.Count - 1;
        highlightedItemId = item.ItemId;
    }

    private void RemoveSelectedItem()
    {
        if (inventory.Count == 0)
        {
            return;
        }

        InventoryItem removedItem = inventory[selectedSlotIndex];
        // RemoveAt은 지정한 인덱스의 데이터를 제거하고, 뒤쪽 데이터를 앞으로 당깁니다.
        inventory.RemoveAt(selectedSlotIndex);
        slotIndexByItemId.Remove(removedItem.ItemId);

        RebuildDictionary();
        selectedSlotIndex = Mathf.Clamp(selectedSlotIndex, 0, Mathf.Max(0, inventory.Count - 1));
        highlightedItemId = -1;
    }

    private void HighlightNewestItemByDictionary()
    {
        int newestItemId = nextItemId - 1;

        // Dictionary는 Key가 있는지 빠르게 확인하고, 있으면 슬롯 번호를 바로 가져올 수 있습니다.
        // TryGetValue는 Key가 있으면 true를 반환하고, 찾은 Value를 out 변수에 넣어 줍니다.
        if (slotIndexByItemId.TryGetValue(newestItemId, out int slotIndex))
        {
            selectedSlotIndex = slotIndex;
            highlightedItemId = newestItemId;
        }
    }

    private void RebuildDictionary()
    {
        // Clear는 Dictionary 안에 들어 있던 Key-Value 쌍을 모두 지웁니다.
        slotIndexByItemId.Clear();

        for (int i = 0; i < inventory.Count; i++)
        {
            // List의 현재 순서를 기준으로 Dictionary의 슬롯 번호를 다시 맞춥니다.
            slotIndexByItemId[inventory[i].ItemId] = i;
        }
    }

    private Color GetColorBySlot(int index)
    {
        // 같은 색만 반복되면 슬롯 구분이 어려우므로 몇 가지 색을 번갈아 사용합니다.
        Color[] colors =
        {
            new Color(0.2f, 0.7f, 1f),
            new Color(0.3f, 0.9f, 0.45f),
            new Color(1f, 0.75f, 0.25f),
            new Color(0.9f, 0.45f, 1f)
        };

        return colors[index % colors.Length];
    }

    private void OnDrawGizmos()
    {
        // OnDrawGizmos는 Scene 뷰에 개발용 시각 표시를 그릴 때 사용하는 Unity 메시지 메서드입니다.
        for (int i = 0; i < maxSlotCount; i++)
        {
            Vector3 slotPosition = transform.position + Vector3.right * i * (slotSize + slotGap);

            // Application.isPlaying은 현재 Play 모드인지 확인하는 프로퍼티입니다.
            bool hasItem = Application.isPlaying && i < inventory.Count;

            // Gizmos.DrawCube는 Scene 뷰에 색이 채워진 정육면체를 그립니다.
            Gizmos.color = hasItem ? inventory[i].SlotColor : Color.gray;
            Gizmos.DrawCube(slotPosition, Vector3.one * slotSize);

            // DrawWireCube는 채워지지 않은 테두리 상자를 그립니다.
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(slotPosition, Vector3.one * slotSize);

            if (!Application.isPlaying || !hasItem)
            {
                continue;
            }

            if (i == selectedSlotIndex)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(slotPosition, Vector3.one * (slotSize + 0.18f));
            }

            if (inventory[i].ItemId == highlightedItemId)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(slotPosition + Vector3.up * 0.65f, 0.18f);
            }
        }
    }
}
```

### 실행해보면

`A` 키를 누를 때마다 인벤토리 슬롯에 색이 채워집니다. 이 슬롯 순서가 `List<InventoryItem>`의 순서입니다.

왼쪽/오른쪽 방향키를 누르면 흰색 테두리로 선택 슬롯이 바뀝니다. `Backspace`를 누르면 선택한 슬롯의 아이템이 제거되고, 뒤쪽 아이템들이 앞으로 당겨집니다.

`F` 키를 누르면 `Dictionary`가 가장 최근에 추가한 아이템 ID를 찾아 노란색 표시를 띄웁니다. 이때 `Dictionary`는 아이템을 처음부터 하나씩 찾는 대신, `ItemId`라는 Key로 슬롯 번호를 바로 가져옵니다.

### 생각해보기

1. 인벤토리처럼 순서가 보이는 데이터에는 왜 `List`가 잘 어울릴까요?
2. 선택한 아이템을 제거하면 왜 `Dictionary`를 다시 정리해야 할까요?
3. 알림 메시지를 들어온 순서대로 보여주려면 `List`, `Queue`, `Stack` 중 무엇이 가장 자연스러울까요?
4. 실행 취소 기능은 왜 `Stack`과 잘 어울릴까요?

## 별첨: 기획 문장에서 자료구조 찾기

기획 문장을 읽을 때는 명사와 동사를 나눠보면 자료구조 선택이 쉬워집니다.

> 플레이어는 최대 20개의 아이템을 보관할 수 있는 가방을 가진다. 아이템은 획득 순서대로 들어오지만, 중간에 있는 아이템을 버릴 수 있다.

- 명사: 플레이어, 아이템, 가방, 슬롯
- 동사: 보관한다, 들어온다, 버린다
- 어울리는 자료구조: 슬롯 순서가 있으므로 `List`

> 보스는 포효, 지진, 휩쓸기 순서로 스킬을 예약하고 순서대로 실행한다.

- 명사: 보스, 스킬, 예약 목록
- 동사: 예약한다, 순서대로 실행한다
- 어울리는 자료구조: 먼저 예약한 스킬을 먼저 실행한다면 `Queue`

> 플레이어는 방금 연 메뉴부터 닫아야 한다.

- 명사: 메뉴, 열기 기록
- 동사: 연다, 닫는다, 되돌린다
- 어울리는 자료구조: 마지막에 연 메뉴를 먼저 닫으므로 `Stack`

> 아이템 ID를 입력하면 아이템 정보를 바로 찾는다.

- 명사: 아이템 ID, 아이템 정보
- 동사: 찾는다
- 어울리는 자료구조: Key로 Value를 찾으므로 `Dictionary`

## 오늘의 정리

- 자료구조는 데이터를 담고 꺼내는 방법이다.
- `List`는 순서가 중요한 데이터에 어울린다.
- `Queue`는 먼저 들어온 것을 먼저 처리할 때 어울린다.
- `Stack`은 마지막에 들어온 것을 먼저 꺼낼 때 어울린다.
- `Dictionary`는 Key로 Value를 빠르게 찾을 때 어울린다.
- 게임 기능을 읽을 때는 명사와 동사를 나누면 필요한 자료구조가 보이기 시작한다.
