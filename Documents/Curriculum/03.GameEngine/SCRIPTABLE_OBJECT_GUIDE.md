# 보충: ScriptableObject 데이터 관리

이 문서의 목표는 ScriptableObject를 "**게임 안에서 여러 오브젝트가 함께 읽는 데이터 카드**"로 이해하고, Prefab과 무엇이 다른지, 어떤 상황에서 쓰면 좋은지, 실무에서 어떤 실수를 조심해야 하는지 정리하는 것입니다.

`DAY08_UNITY_RESOURCE.md`에서 Prefab을 "**원본 설계도**"로 배웠다면, ScriptableObject는 설계도 안에 들어가는 숫자와 설명을 따로 보관하는 데이터 파일에 가깝습니다.

## 1. 핵심 개념: "물건 설계도와 데이터 카드 나누기"

게임 아이템을 예로 들어 보겠습니다.

검 Prefab은 씬에 놓이거나 생성될 수 있는 실제 GameObject 묶음입니다. Mesh, Collider, AudioSource, 공격 판정 스크립트처럼 "**장면 안에서 움직이는 부품**"을 가집니다.

검 ScriptableObject는 검의 이름, 공격력, 가격, 아이콘, 설명처럼 "**여러 곳에서 읽어야 하는 데이터**"를 가집니다. 인벤토리 UI, 상점 UI, 드롭 테이블, 전투 계산 코드가 같은 데이터를 함께 읽을 수 있습니다.

### 이 단어는 무슨 뜻인가요?

- **ScriptableObject**: GameObject에 붙지 않고 Project 창에 에셋 파일로 저장되는 데이터 객체입니다.
- **데이터 에셋**: 코드가 읽을 수 있도록 프로젝트 파일로 저장한 값 묶음입니다.
- **공유 데이터**: 여러 오브젝트나 시스템이 같은 원본을 참조해서 읽는 값입니다.
- **런타임 상태**: 게임 실행 중 계속 바뀌는 현재 체력, 현재 탄약, 현재 위치 같은 값입니다.
- **템플릿 데이터**: 아이템 기본 공격력, 몬스터 기본 이동 속도처럼 원본 규칙으로 쓰는 값입니다.

## 2. Prefab과 ScriptableObject 비교

| 구분 | Prefab | ScriptableObject |
| :--- | :--- | :--- |
| 비유 | 물건을 찍어내는 설계도 | 여러 곳에서 보는 데이터 카드 |
| 저장 위치 | Project 창의 Prefab 에셋 | Project 창의 `.asset` 데이터 에셋 |
| 씬 배치 | 씬에 인스턴스로 배치 가능 | 단독으로 씬에 배치하지 않음 |
| 주 내용 | GameObject, Transform, Component 구성 | 이름, 숫자, 아이콘, 참조, 설정값 |
| 코드 사용 | `Instantiate`로 복사본 생성 | Inspector에 연결해 값 읽기 |
| 자주 쓰는 곳 | 몬스터, 총알, UI 패널, 아이템 오브젝트 | 아이템 데이터, 스킬 데이터, 몬스터 밸런스, 사운드 목록 |

중요한 차이는 "**Prefab은 장면에 나타나는 물체를 만들고, ScriptableObject는 그 물체가 읽을 데이터를 보관한다**"는 점입니다.

예를 들어 `Slime` Prefab은 슬라임 모델, Collider, Animator, 이동 스크립트를 가질 수 있습니다. `SlimeData` ScriptableObject는 이름, 최대 체력, 이동 속도, 경험치, 드롭 아이템 목록을 가질 수 있습니다.

## 3. 어떤 용도로 많이 사용하나요?

ScriptableObject는 특히 "**같은 형식의 데이터를 많이 만들고, 여러 시스템이 함께 읽어야 할 때**" 편합니다.

| 용도 | 예시 |
| :--- | :--- |
| 아이템 데이터 | 이름, 아이콘, 가격, 설명, 회복량 |
| 스킬 데이터 | 쿨타임, 소비 MP, 사거리, 이펙트 Prefab |
| 몬스터 데이터 | 최대 체력, 공격력, 이동 속도, 경험치 |
| 무기 데이터 | 공격력, 공격 속도, 탄환 Prefab, 사운드 |
| 레벨 설정 | 제한 시간, 목표 점수, 등장 몬스터 목록 |
| 사운드/VFX 목록 | 행동 이름별 AudioClip, Particle Prefab |

실무에서는 코드 안에 숫자를 직접 박아 넣는 대신, ScriptableObject로 빼서 기획자나 개발자가 Inspector에서 조정할 수 있게 만드는 경우가 많습니다.

나쁜 예:

```csharp
damage = 35;
cooldown = 1.2f;
```

좋은 방향:

```csharp
damage = skillData.Damage;
cooldown = skillData.Cooldown;
```

숫자가 코드 안에 흩어져 있으면 밸런스를 고칠 때 파일 여러 개를 뒤져야 합니다. ScriptableObject로 모으면 Project 창에서 `Fireball`, `IceArrow`, `Heal` 같은 데이터 에셋을 따로 만들고 비교할 수 있습니다.

## 4. 실습 예제: 아이템 데이터 만들기

**미션:** 아이템의 이름, 가격, 설명을 ScriptableObject로 만들고, 씬의 오브젝트가 그 데이터를 읽어 Console에 출력하게 합니다.

### 1단계: ItemData 스크립트 만들기

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Game Data/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private int price;
    [TextArea]
    [SerializeField] private string description;

    public string ItemName => itemName;
    public int Price => price;
    public string Description => description;
}
```

</details>

`CreateAssetMenu`는 Project 창에서 새 데이터 에셋을 만들 수 있게 해 주는 표시판입니다. 스크립트를 저장한 뒤 Unity로 돌아가면 `Assets > Create > Game Data > Item Data` 메뉴를 사용할 수 있습니다.

코드를 `위->아래`로 읽으면, 먼저 `ItemData`가 `ScriptableObject`를 상속받는다는 점을 확인합니다. 그 다음 `itemName`, `price`, `description`은 Inspector에서 입력할 값이고, 아래의 `ItemName`, `Price`, `Description`은 다른 스크립트가 읽는 통로입니다.

### 2단계: 데이터를 읽는 스크립트 만들기

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class ItemDataPrinter : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    void Start()
    {
        if (itemData == null)
        {
            Debug.LogWarning("ItemData가 연결되지 않았습니다.");
            return;
        }

        Debug.Log($"아이템 이름: {itemData.ItemName}");
        Debug.Log($"가격: {itemData.Price}");
        Debug.Log($"설명: {itemData.Description}");
    }
}
```

</details>

### 3단계: Unity에서 연결하기

1. `ItemData.cs`와 `ItemDataPrinter.cs`를 `Assets/_Project/Scripts`에 만듭니다.
2. Project 창에서 `Create > Game Data > Item Data`를 선택합니다.
3. 새 에셋 이름을 `PotionItemData`로 바꿉니다.
4. Inspector에서 이름, 가격, 설명을 입력합니다.
5. 빈 GameObject를 만들고 `ItemDataPrinter`를 붙입니다.
6. `ItemData` 칸에 `PotionItemData` 에셋을 끌어 넣습니다.
7. Play를 눌러 Console 출력을 확인합니다.

### 실행해보면

씬의 GameObject는 직접 숫자를 가지고 있지 않습니다. 대신 Project 창에 있는 `PotionItemData` 에셋을 참조하고, 실행할 때 그 값을 읽어 출력합니다.

같은 `PotionItemData`를 인벤토리 UI, 상점 UI, 아이템 드롭 오브젝트가 함께 참조하면 한 곳에서 수정한 이름과 가격이 여러 기능에 같이 반영됩니다.

## 5. Prefab과 함께 쓰는 실무 흐름

ScriptableObject는 Prefab을 대체하는 기능이 아닙니다. 둘을 함께 쓰면 역할이 분명해집니다.

```text
ItemData.asset
  - 이름: 회복 물약
  - 가격: 50
  - 회복량: 30
  - 아이콘: potion_icon
  - 월드 Prefab: PotionPickup.prefab

PotionPickup.prefab
  - Mesh 또는 Sprite
  - Collider
  - PickupItem 스크립트
  - 연결된 ItemData: 회복 물약
```

이 구조에서 Prefab은 "**플레이어가 실제로 줍는 물체**"이고, ScriptableObject는 "**그 물체가 어떤 아이템인지 알려 주는 정보 카드**"입니다.

실무에서는 다음처럼 나누는 경우가 많습니다.

| 데이터 | Prefab에 두기 좋은가요? | ScriptableObject에 두기 좋은가요? |
| :--- | :--- | :--- |
| Mesh, Collider, Animator | 좋음 | 부적합 |
| 현재 위치, 현재 회전 | 좋음 | 부적합 |
| 아이템 이름, 설명, 가격 | 가능하지만 반복됨 | 좋음 |
| 기본 공격력, 기본 체력 | 가능하지만 관리가 어려움 | 좋음 |
| 현재 체력, 현재 탄약 | 좋을 때도 있지만 인스턴스 상태로 관리 | 주의 필요 |
| 이펙트 Prefab 참조 | 가능 | 설정 데이터로 좋음 |

## 6. 사용상 주의할 점

### 1. 실행 중 바뀌는 값을 원본 에셋에 저장하지 않기

ScriptableObject는 여러 오브젝트가 함께 보는 원본 데이터입니다. 그래서 실행 중에 값을 바꾸면 같은 에셋을 참조하는 다른 오브젝트에도 영향이 갈 수 있습니다.

특히 Editor에서 Play 중 ScriptableObject 값을 바꾸면 실수로 원본 에셋 값이 바뀐 것처럼 보일 수 있습니다. 플레이어의 현재 체력, 현재 경험치, 현재 장착 아이템처럼 저장 파일에 가까운 값은 ScriptableObject 원본에 직접 기록하지 않는 편이 안전합니다.

권장 흐름:

```text
MonsterData.asset: 최대 체력 100
MonsterRuntimeState: 현재 체력 100 -> 70 -> 0
```

`MonsterData`는 기본값이고, `MonsterRuntimeState`나 `MonsterHealth` 같은 별도 클래스가 현재 값을 들고 있게 만듭니다.

### 2. 데이터 에셋과 저장 데이터를 구분하기

ScriptableObject는 프로젝트에 포함되는 개발용 에셋입니다. 플레이어가 게임을 하며 만든 세이브 데이터와는 다릅니다.

- 아이템 기본 정보: ScriptableObject에 적합
- 플레이어가 실제로 가진 아이템 개수: 저장 데이터에 적합
- 스킬의 기본 쿨타임: ScriptableObject에 적합
- 현재 남은 쿨타임: 런타임 상태에 적합

간단히 말해, "**기본 규칙은 ScriptableObject, 지금 상태는 런타임 객체나 저장 파일**"로 나눕니다.

### 3. 참조가 빠졌을 때를 대비하기

ScriptableObject를 Inspector에 연결하는 방식은 편리하지만, 연결을 잊으면 `null` 문제가 생깁니다. 그래서 실행 시 중요한 참조는 확인하는 코드를 둡니다.

```csharp
if (itemData == null)
{
    Debug.LogWarning("ItemData가 연결되지 않았습니다.");
    return;
}
```

경고 메시지는 누가 봐도 어떤 칸이 비었는지 알 수 있게 씁니다. 팀 작업에서는 친절한 경고 하나가 디버깅 시간을 크게 줄입니다.

### 4. 폴더와 이름 규칙 정하기

ScriptableObject가 많아지면 Project 창이 금방 복잡해집니다. 처음부터 폴더를 나누는 습관이 좋습니다.

```text
Assets/
  _Project/
    Data/
      Items/
      Skills/
      Monsters/
    Prefabs/
      Items/
      Monsters/
    Scripts/
```

이름도 `PotionItemData`, `FireballSkillData`, `SlimeMonsterData`처럼 용도와 종류가 보이게 붙입니다. `Data1`, `NewItemData`, `Test` 같은 이름은 나중에 찾기 어렵습니다.

### 5. 데이터가 너무 많은 일을 하지 않게 하기

ScriptableObject에 함수도 만들 수 있지만, 처음에는 "**값을 담는 역할**"에 집중하는 편이 좋습니다. 데이터 에셋이 전투 계산, UI 표시, 저장 처리까지 모두 직접 하게 만들면 역할이 섞입니다.

좋은 기준은 다음과 같습니다.

- ScriptableObject: 기본 데이터 제공
- MonoBehaviour: 씬에서 실행되는 동작 처리
- Manager 또는 System 클래스: 여러 데이터를 모아 규칙 처리
- SaveData 클래스: 플레이어 진행 상태 저장

## 7. 실무 체크리스트

ScriptableObject를 만들기 전에 아래 질문을 확인합니다.

| 질문 | 예라고 답하면 |
| :--- | :--- |
| 같은 형식의 데이터를 여러 개 만들 예정인가요? | ScriptableObject 후보입니다. |
| 여러 씬, UI, 시스템이 같은 값을 읽어야 하나요? | ScriptableObject가 유용합니다. |
| 실행 중 계속 바뀌는 현재 상태인가요? | 원본 ScriptableObject에 직접 저장하지 않습니다. |
| 이 데이터가 없으면 기능이 동작하지 않나요? | `null` 검사와 경고 메시지를 둡니다. |
| 팀원이 Inspector에서 조정해야 하나요? | 필드 이름과 Tooltip을 친절하게 둡니다. |
| 에셋 수가 많아질 예정인가요? | `Data/Items`, `Data/Skills`처럼 폴더를 먼저 나눕니다. |

## 생각해보기

1. 몬스터 Prefab과 몬스터 ScriptableObject에는 각각 어떤 정보를 넣는 것이 좋을까요?
2. 플레이어의 현재 체력을 ScriptableObject 원본에 직접 저장하면 어떤 문제가 생길 수 있을까요?
3. 아이템 이름과 가격을 코드 안에 직접 쓰는 방식보다 ScriptableObject로 분리하면 어떤 작업이 쉬워질까요?
4. 같은 `ItemData`를 상점 UI와 인벤토리 UI가 함께 참조하면 어떤 장점이 있을까요?

## 오늘의 정리

- Prefab은 장면에 배치하거나 생성할 GameObject 설계도입니다.
- ScriptableObject는 Project 창에 저장되는 공유 데이터 에셋입니다.
- 아이템, 스킬, 몬스터, 레벨 설정처럼 같은 형식의 데이터를 많이 만들 때 유용합니다.
- 기본 규칙과 템플릿 데이터는 ScriptableObject에 두고, 현재 체력이나 저장 진행도 같은 런타임 상태는 별도로 관리합니다.
- ScriptableObject와 Prefab은 경쟁 관계가 아니라, 실제 오브젝트와 데이터 카드를 나누는 협력 관계입니다.
