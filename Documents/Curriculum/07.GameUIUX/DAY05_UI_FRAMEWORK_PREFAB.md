# DAY 05: UI 프레임워크와 Prefab 구조

오늘의 목표는 반복해서 쓰는 UI를 Prefab으로 만들고, UI Manager를 통해 화면 열기, 닫기, 갱신을 관리하는 작은 UI 프레임워크를 구현하는 것입니다.

## NCS 연결

- 능력단위 요소: 게임 UI/UX 응용 프로그래밍하기
- 관련 학습 내용: UI 프레임워크 아키텍처 설계, UI 프레임워크 구현, 커스텀 인터페이스 구현, 프로그램 인터페이스 문서화
- Unity 6 재구성: UGUI Prefab, Panel Controller, UI Manager, 이벤트 기반 갱신 구조를 사용해 재사용 가능한 UI 구조를 만듭니다.

## 1. 핵심 개념: "매번 새로 만드는 대신 조립식 부품 만들기"

버튼, 슬롯, 팝업, 알림 메시지는 게임 안에서 여러 번 반복됩니다. 매번 새로 만들면 모양과 코드가 조금씩 달라져서 관리가 어려워집니다.

Prefab은 조립식 부품입니다. 한 번 잘 만들어 두면 여러 화면에서 같은 규격으로 사용할 수 있습니다.

### 이 단어는 무슨 뜻인가요?

- **Prefab**: 미리 만들어 저장해 둔 GameObject 설계도입니다.
- **UI Manager**: UI 화면 열기, 닫기, 갱신을 관리하는 중심 스크립트입니다.
- **View**: 실제 화면에 보이는 Text, Image, Button 묶음입니다.
- **Controller**: View에 값을 넣고 버튼 동작을 연결하는 스크립트입니다.
- **Popup**: 화면 위에 잠시 띄우는 확인창, 경고창, 보상창입니다.
- **Toast**: 몇 초 후 자동으로 사라지는 짧은 알림 메시지입니다.

## 2. 작은 UI 프레임워크 구조

```text
Canvas
└── UIRoot
    ├── HudView
    ├── MenuView
    ├── PopupRoot
    │   └── ConfirmPopup
    └── ToastRoot
        └── ToastMessage

Scripts
├── UIManager
├── HudView
├── MenuView
├── ConfirmPopup
└── ToastMessage
```

큰 프로젝트의 UI 프레임워크는 복잡할 수 있지만, 수업에서는 다음 세 가지 기능만 목표로 삼습니다.

1. 화면을 열고 닫을 수 있다.
2. 게임 상태를 UI에 반영할 수 있다.
3. 반복 UI를 Prefab으로 재사용할 수 있다.

## 3. UI Prefab 제작 기준

| Prefab | 포함 요소 | 재사용 위치 |
| :--- | :--- | :--- |
| `MenuButton` | Button, TextMeshPro Text | 타이틀, 일시정지, 결과 화면 |
| `ItemSlot` | Icon Image, Count Text, Selected Border | 인벤토리, 상점, 보상 화면 |
| `ConfirmPopup` | Message Text, OK Button, Cancel Button | 종료 확인, 구매 확인 |
| `ToastMessage` | Message Text, Background Image | 아이템 획득, 저장 완료 |
| `GaugeBar` | Fill Image, Label Text | 체력, 마나, 경험치 |

## 실습 예제: Toast 알림 프레임워크 만들기

**미션:** 버튼을 누르면 화면 오른쪽 위에 짧은 알림 메시지가 나타났다가 사라지게 만듭니다.

### ToastMessage Prefab 구조

```text
ToastMessage
├── Background
└── MessageText
```

### ToastMessage 스크립트

<details>
<summary>코드 보기</summary>

```csharp
using System.Collections;
using TMPro;
using UnityEngine;

public class ToastMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float lifeTime = 2f;

    public void Show(string message)
    {
        messageText.text = message;
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        // WaitForSeconds는 Time.timeScale의 영향을 받습니다.
        // Pause 중에도 Toast가 사라져야 할 때만 WaitForSecondsRealtime으로 바꿉니다.
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
```

</details>

### UIManager 스크립트

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform toastRoot;
    [SerializeField] private ToastMessage toastPrefab;

    public void ShowToast(string message)
    {
        ToastMessage toast = Instantiate(toastPrefab, toastRoot);
        toast.Show(message);
    }

    public void ShowItemToast()
    {
        ShowToast("아이템을 획득했습니다.");
    }
}
```

</details>

### Unity 연결

1. Canvas 아래의 `UIRoot`에 `ToastRoot`를 만들고 Anchor를 오른쪽 위로 설정합니다.
2. `ToastMessage` UI를 만든 뒤 Prefab으로 저장합니다.
3. 씬의 `UIManager`에 `toastRoot`와 `toastPrefab`을 연결합니다.
4. 테스트 Button의 `On Click`에 `UIManager.ShowItemToast`를 연결합니다.

### 실행해보면

버튼을 누르면 `ToastMessage` Prefab이 생성되고, 메시지를 표시한 뒤 2초 후 사라집니다. 같은 Prefab을 여러 곳에서 재사용할 수 있습니다.

## 4. UI 문서화

NCS에서는 기능별 동작과 프로그램 인터페이스를 문서화하는 것도 중요하게 봅니다. 수업에서는 다음 형식으로 간단히 작성합니다.

```text
UI 이름: ToastMessage
목적: 짧은 알림 메시지를 화면에 표시한다.
사용 위치: 아이템 획득, 저장 완료, 오류 안내
입력: string message
동작: 생성 -> 메시지 표시 -> lifeTime 후 삭제
테스트: 버튼 클릭 시 메시지가 보이고 자동으로 사라지는지 확인
```

### 생각해보기

1. ToastMessage를 Prefab으로 만들면 어떤 점이 편할까요?
2. UIManager가 모든 UI 세부 동작을 직접 처리하면 어떤 문제가 생길까요?
3. `Instantiate(toastPrefab, toastRoot)`에서 오른쪽 `toastRoot`는 어떤 역할을 할까요?
4. 팝업과 토스트는 UX 관점에서 어떤 차이가 있을까요?

## 오늘의 정리

- UI 프레임워크는 UI를 반복해서 안정적으로 만들기 위한 기본 구조입니다.
- Prefab은 버튼, 슬롯, 팝업, 알림처럼 반복되는 UI를 재사용하게 해 줍니다.
- UIManager는 화면 열기, 닫기, 생성 같은 큰 흐름을 맡고, 각 View는 자기 표시 책임을 갖는 것이 좋습니다.
- 구현한 UI는 입력, 동작, 테스트 방법을 짧게 문서화해야 나중에 유지보수하기 쉽습니다.
