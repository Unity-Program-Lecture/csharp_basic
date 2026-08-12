# DAY 06: UI/UX 통합 구현과 체크리스트 검증

오늘의 목표는 지금까지 만든 HUD, 메뉴, 입력, Prefab 구조를 하나의 작은 Unity 6 씬으로 통합하고, 체크리스트를 사용해 UI/UX 품질을 검증하는 것입니다.

## NCS 연결

- 능력단위 요소: 게임 UI/UX 요소 프로그래밍하기, 게임 UI/UX 응용 프로그래밍하기
- 관련 학습 내용: 요소별 상세 체크리스트 작성, 단위 테스트 수행, UI 프레임워크 구현, 설계 문서와 구현 결과 일치성 확인
- Unity 6 재구성: UGUI, Input System, UI Manager, HUD, 메뉴, 알림 UI를 통합하고 플레이 가능한 UI 포트폴리오를 점검합니다.

## 1. 핵심 개념: "만든 UI를 플레이어 입장에서 검사하기"

UI는 만들었다고 끝이 아닙니다. 버튼이 눌리는지, 글자가 읽히는지, 해상도가 바뀌어도 깨지지 않는지, 게임 상태와 화면 표시가 맞는지 확인해야 합니다.

체크리스트는 검사표입니다. 공항에서 비행기 이륙 전 점검표를 보듯이, UI도 출시 전 또는 제출 전 확인 목록을 보고 점검합니다.

### 이 단어는 무슨 뜻인가요?

- **단위 테스트**: UI 요소 하나가 의도대로 동작하는지 확인하는 테스트입니다.
- **통합 테스트**: 여러 UI와 게임 상태가 함께 연결될 때 문제가 없는지 확인하는 테스트입니다.
- **체크리스트**: 빠뜨리기 쉬운 항목을 순서대로 확인하는 목록입니다.
- **접근성**: 글자 크기, 색 대비, 조작 방식처럼 다양한 사용자가 UI를 이해하고 사용할 수 있게 하는 성질입니다.
- **피드백 루프**: 입력 -> 반응 -> 결과 확인이 끊기지 않고 이어지는 흐름입니다.

## 2. 통합 목표

최종 실습 씬에는 다음 기능이 들어갑니다.

| 영역 | 구현 목표 |
| :--- | :--- |
| HUD | 체력, 점수, 남은 시간 표시 |
| 메뉴 | 타이틀, 일시정지, 결과 화면 전환 |
| 입력 | 마우스, 키보드, 게임패드 UI 조작 |
| 피드백 | 버튼 상태 변화, Toast 알림, 비활성화 표시 |
| 프레임워크 | UIManager와 Prefab 기반 알림 또는 팝업 |
| 검증 | 해상도, 입력, 화면 전환, 데이터 연결 체크 |

## 3. UI/UX 체크리스트 작성법

체크리스트는 "좋아 보인다"가 아니라 "확인할 수 있다"로 써야 합니다.

| 나쁜 항목 | 좋은 항목 |
| :--- | :--- |
| UI가 예쁜가? | 버튼 글자와 배경의 대비가 충분해 읽을 수 있는가? |
| 메뉴가 잘 되는가? | Start, Pause, Resume, Retry 버튼이 각각 올바른 화면으로 전환되는가? |
| 입력이 편한가? | 마우스, 키보드, 게임패드 중 2가지 이상으로 메뉴를 조작할 수 있는가? |
| 해상도가 괜찮은가? | 16:9와 4:3 Game View에서 HUD가 화면 밖으로 나가지 않는가? |

## 실습 예제: 제출 전 UI 점검표

**미션:** 자신이 만든 UI 씬을 실행하고 아래 점검표를 직접 확인합니다.

### 화면 구성 점검

| 확인 항목 | 결과 |
| :--- | :--- |
| Canvas Scaler가 `Scale With Screen Size`로 설정되어 있다. |  |
| 기준 해상도가 문서에 적혀 있다. |  |
| HUD, 메뉴, 결과 화면이 각각 Panel로 분리되어 있다. |  |
| 화면 전환 시 여러 Panel이 동시에 겹쳐 보이지 않는다. |  |
| TextMeshPro 글자가 잘리지 않는다. |  |

### 입력 점검

| 확인 항목 | 결과 |
| :--- | :--- |
| 마우스로 모든 주요 버튼을 클릭할 수 있다. |  |
| 키보드 방향키 또는 Tab으로 메뉴를 이동할 수 있다. |  |
| Submit 입력으로 선택된 버튼을 실행할 수 있다. |  |
| Cancel 또는 Esc 입력으로 뒤로 가기나 일시정지가 동작한다. |  |
| EventSystem과 Input System UI Input Module이 올바르게 구성되어 있다. |  |

### 데이터 연결 점검

| 확인 항목 | 결과 |
| :--- | :--- |
| 체력 값이 바뀌면 체력 UI도 바뀐다. |  |
| 점수 값이 바뀌면 점수 Text도 바뀐다. |  |
| 남은 시간이 0 아래로 내려가지 않는다. |  |
| 버튼을 눌렀을 때 즉시 시각적 피드백이 있다. |  |
| 알림 또는 팝업이 필요한 순간에 표시된다. |  |

### 해상도 점검

| Game View | 확인 내용 |
| :--- | :--- |
| `1920 x 1080` | 기준 해상도에서 의도한 위치에 보이는가? |
| `1280 x 720` | 비율이 유지되는가? |
| `1024 x 768` | UI가 겹치거나 화면 밖으로 나가지 않는가? |
| `Free Aspect` | 극단적인 창 크기에서 큰 문제가 없는가? |

### UGUI 최적화 점검

UI 최적화는 "Canvas를 많이 만든다"가 아니라 **자주 바뀌는 영역의 재구성 범위를 작게 만든다**는 뜻입니다. 같은 Canvas 안에서 RectTransform, 계층, Text, Image 등이 자주 바뀌면 그 Canvas의 UI 재구성 비용이 커질 수 있습니다.

아래처럼 변경 주기가 다른 영역만 별도 Canvas로 분리합니다. 이미 있는 `Canvas > UIRoot` 구조는 유지하고, `UIRoot` 아래의 필요한 영역에만 Canvas 컴포넌트를 추가합니다.

```text
Canvas (Root)
└── UIRoot
    ├── StaticHudCanvas       ← 고정 프레임·장식
    ├── DynamicHudCanvas      ← 체력·점수·시간
    ├── ScreenCanvas          ← 타이틀·Pause·결과 화면
    └── EffectCanvas          ← Toast·보상·버튼 강조 연출
```

| 상황 | 권장 처리 | 피할 처리 |
| :--- | :--- | :--- |
| 체력·점수·시간이 바뀜 | 값이 달라진 순간에만 `TextMeshProUGUI`·Slider를 갱신하고 `DynamicHudCanvas`에 둡니다. | `Update()`에서 같은 텍스트를 매 프레임 다시 대입합니다. |
| Toast·보상·버튼 펀치 연출 | `EffectCanvas`에서 재생해 HUD·메뉴의 갱신 범위와 분리합니다. | 고정 HUD와 같은 Canvas에서 계층 이동·크기·텍스트를 계속 바꿉니다. |
| 자동 정렬 메뉴 | Layout Group의 Padding·Spacing을 먼저 정하고, 변경 후 한 번만 갱신합니다. | 중첩된 Layout Group·Content Size Fitter에 자식 크기 변경을 계속 전달합니다. |
| 클릭하지 않는 장식 | Image와 TMP Text의 `Raycast Target`을 끕니다. | 배경·아이콘·설명 글자까지 모두 Raycast 대상으로 둡니다. |
| 긴 Scroll View 목록 | 화면에 보이는 항목만 생성하거나 항목 Prefab을 재사용합니다. | 수백 개의 항목을 처음부터 모두 생성합니다. |
| 이미지·마스크 | Sprite Atlas와 공통 Material을 우선 사용하고, 사각형 잘림은 `RectMask2D`를 검토합니다. | 재질을 제각각 만들거나 큰 반투명 Panel과 Mask를 여러 겹 겹칩니다. |

Canvas는 분리할수록 재구성 범위를 줄일 수 있지만, Canvas마다 배치가 나뉘어 Draw Call이 늘 수 있습니다. 따라서 버튼 하나마다 Canvas를 추가하지 말고, **고정 UI / 자주 갱신되는 HUD / 연출**처럼 실제 변경 주기가 다른 덩어리만 분리합니다.

#### 실습: 분리 전후를 측정해 보기

1. 체력 Bar, 점수 Text, 시간 Text, Toast를 모두 같은 Canvas에 둔 상태에서 Play Mode를 실행합니다.
2. `Window > Analysis > Profiler`와 Frame Debugger에서 UI 갱신과 Draw Call을 확인합니다.
3. 체력·점수·시간을 `DynamicHudCanvas`로, Toast를 `EffectCanvas`로 옮긴 뒤 같은 상황을 다시 실행합니다.
4. 재구성 비용, Draw Call, 화면 결과를 함께 기록합니다. 수치가 항상 좋아져야 하는 것은 아니며, 갱신 범위 감소와 Draw Call 증가 사이의 균형을 설명할 수 있어야 합니다.

| 확인 항목 | 결과 |
| :--- | :--- |
| 고정 UI, 자주 갱신되는 HUD, 연출 UI의 Canvas 분리 이유를 설명할 수 있다. |  |
| 값이 바뀌지 않을 때 TMP Text와 Slider를 반복 갱신하지 않는다. |  |
| 클릭하지 않는 Graphic의 `Raycast Target`을 껐다. |  |
| 분리 전후를 Profiler 또는 Frame Debugger로 확인하고 결과를 기록했다. |  |

#### 여러 Canvas의 렌더링·입력 순서

Canvas를 나누면 한 Canvas 안에서 사용하던 Hierarchy 순서만으로는 화면의 앞뒤를 판단할 수 없습니다. 같은 Canvas 안에서는 Hierarchy에서 **나중에 그려지는 형제**가 위에 보이지만, 별도 Canvas끼리는 `Sorting Layer`와 `Order in Layer` (코드에서는 `sortingOrder`) 규칙을 사용합니다. 중첩 Canvas가 부모와 다른 순서를 가져야 할 때만 `Override Sorting`을 켭니다.

이 과정의 `Screen Space - Overlay` UI는 아래 숫자를 시작 규칙으로 사용합니다. 숫자 사이를 비워 두면 나중에 새 UI를 넣어도 기존 값을 전부 바꿀 필요가 없습니다.

| Canvas 역할 | 권장 Order in Layer | 입력 규칙 |
| :--- | :--- | :--- |
| `StaticHudCanvas` | `0` | 장식은 Raycast Target을 끕니다. |
| `DynamicHudCanvas` | `10` | HUD 자체가 버튼이 아니라면 Raycast Target을 끕니다. |
| `ScreenCanvas` | `100` | 현재 화면의 Button만 입력을 받게 합니다. |
| `ModalCanvas` | `200` | 전체 화면 Dimmer가 뒤 UI 입력을 막고, Popup Button이 가장 앞에서 입력을 받습니다. |
| `EffectCanvas` | `300` | Toast·보상 연출은 위에 보이되, 클릭하지 않는 Graphic은 입력을 막지 않습니다. |

다음도 함께 확인합니다.

- 분리한 Canvas가 서로 다른 `Canvas Scaler` 기준을 사용하면 같은 좌표라도 크기와 위치가 어긋날 수 있습니다. 이 과정에서는 같은 `Scale With Screen Size`, `1920 x 1080`, `Match 0.5`를 기본값으로 맞춥니다.
- 별도 Canvas에 Button·Slider처럼 상호작용 UI가 있다면 해당 Canvas의 `Graphic Raycaster` 구성도 확인합니다. Graphic Raycaster는 Canvas별 Graphic을 대상으로 입력 후보를 찾습니다.
- 보이지 않는 화면은 `CanvasGroup`의 `Blocks Raycasts` 또는 오브젝트 활성 상태까지 함께 끕니다. Alpha만 `0`으로 만들어 두면 투명한 Panel이 뒤의 버튼 클릭을 가로챌 수 있습니다.
- Popup을 열 때는 Popup의 선택 Button을 EventSystem의 첫 선택 대상으로 지정하고, 닫을 때는 이전 화면의 선택 대상으로 돌려놓습니다.
- `Screen Space - Camera`나 `World Space` Canvas는 Overlay와 달리 Camera, Plane Distance, 3D 물체와의 앞뒤 관계도 확인해야 합니다. 이 과정의 기본 씬은 Overlay만 사용합니다.

#### Canvas 내부의 그리기 순서와 입력 우선순위

하나의 Canvas 안에서는 Hierarchy의 형제 순서가 그리기 순서입니다. 먼저 있는 자식이 먼저 그려지고, **나중에 있는 형제일수록 앞에 보입니다.** 따라서 겹치는 UI는 `SetAsLastSibling()`으로 마지막 형제로 보내면 같은 Canvas 안에서 위에 보이게 할 수 있습니다.

```text
ScreenCanvas
├── BackgroundPanel       ← 먼저 그림: 가장 뒤
├── SettingsPanel
└── ConfirmPopup          ← 나중에 그림: 가장 앞
```

`Raycast Target`은 "이 Graphic을 포인터 입력 후보로 검사할지"를 정하는 스위치이며, 순서를 정하는 값이 아닙니다. 같은 `Screen Space - Overlay` Canvas에서 겹친 Image·TMP Text가 모두 Raycast Target이고 CanvasGroup 등으로 막히지 않았다면, 화면상 가장 앞에 그려진 Graphic이 보통 먼저 포인터 입력을 받습니다. 하지만 이는 아래 조건이 충족될 때의 동작입니다.

| 확인 조건 | 어긋났을 때 생기는 현상 |
| :--- | :--- |
| 앞 Canvas에 `Graphic Raycaster`가 활성화되어 있다. | 앞에 보이는 Button·Panel이 입력 후보에 없고 뒤 Canvas가 클릭될 수 있습니다. |
| 장식 Image·TMP Text의 `Raycast Target`이 꺼져 있다. | 투명한 장식이 의도치 않게 앞에서 클릭을 가로챌 수 있습니다. |
| 숨긴 화면은 `CanvasGroup.blocksRaycasts = false` 또는 비활성 상태다. | Alpha가 0인 Popup·Panel이 보이지 않는데도 입력을 막습니다. |
| 여러 Canvas의 Sorting Layer·Order in Layer가 의도대로 설정되어 있다. | 보이는 Popup과 실제 클릭되는 UI의 앞뒤가 어긋납니다. |
| Modal의 Dimmer와 Popup Button만 입력을 받는다. | Modal 뒤의 HUD·메뉴 Button이 함께 눌릴 수 있습니다. |

즉, **표시 순서와 입력 우선순위가 일치하도록 설계해야 하지만, Raycast Target만으로 일치가 보장되지는 않습니다.** 겹치는 UI를 만들었으면 실제 Play Mode에서 앞 Panel, 뒤 Button, 투명 Dimmer를 각각 클릭해 보고 의도한 대상만 반응하는지 확인합니다.

### 테스트 결과와 설계 변경 기록

테스트는 통과 여부만 표시하는 데서 끝나지 않습니다. 문제가 발견됐을 때 원래 설계와 수정한 내용을 남겨야 다음 UI에도 같은 실수를 줄일 수 있습니다.

```text
확인 요소: Pause 메뉴의 ResumeButton
기대 결과: Esc로 메뉴를 열면 ResumeButton이 선택되어 Enter로 바로 계속할 수 있다.
실제 결과: 메뉴는 열리지만 선택 표시가 없어 키보드로 바로 실행할 수 없었다.
원인: 첫 선택 UI가 지정되지 않았다.
수정: Pause 메뉴를 열 때 ResumeButton을 첫 선택 대상으로 지정했다.
다시 확인: 키보드와 게임패드에서 통과.
```

이 기록은 DAY 02의 GUI 디자인 가이드와 연결됩니다. 구현 환경에서 발견한 문제를 보고 레이아웃, 입력 흐름, 피드백 규칙을 바꾸는 것은 실패가 아니라 UI/UX 설계를 완성하는 과정입니다.

## 4. 최종 구현 설명서 작성

포트폴리오 제출 전에는 짧은 구현 설명서를 작성합니다.

```text
프로젝트명:
기준 해상도:
사용한 UI 시스템: UGUI, TextMeshPro, Input System

구현한 화면:
- TitleScreen:
- PlayHud:
- PauseMenu:
- ResultScreen:

지원 입력:
- Mouse:
- Keyboard:
- Gamepad:

재사용 Prefab:
- MenuButton:
- ToastMessage:
- GaugeBar:

테스트 결과:
- 해상도 테스트:
- 입력 테스트:
- 화면 전환 테스트:
- 데이터 연결 테스트:
- UI 최적화 측정:
```

## 5. 자주 발생하는 문제

| 문제 | 원인 후보 | 확인 방법 |
| :--- | :--- | :--- |
| 버튼이 눌리지 않음 | EventSystem 없음, Raycast Target이 가림 | EventSystem과 Canvas Graphic Raycaster 확인 |
| UI가 화면 밖으로 나감 | Anchor 설정 오류 | Game View 해상도 변경 후 위치 확인 |
| 글자가 잘림 | RectTransform 크기 부족 | Text 영역 크기와 Auto Size 확인 |
| Pause 후 게임이 안 움직임 | `Time.timeScale` 복구 누락 | Resume에서 `Time.timeScale = 1f` 확인 |
| 게임패드로 메뉴 이동 안 됨 | Navigation 또는 UI Input Module 설정 오류 | Button Navigation과 EventSystem 확인 |

### 생각해보기

1. 체크리스트 항목은 왜 "확인 가능한 문장"으로 써야 할까요?
2. UI가 기능적으로 맞아도 UX가 나쁠 수 있는 예시는 무엇일까요?
3. 해상도 테스트를 마지막에만 하면 어떤 문제가 생길까요?
4. 구현 설명서가 있으면 평가자나 팀원이 어떤 도움을 받을 수 있을까요?

## 오늘의 정리

- UI/UX 프로그래밍은 구현뿐 아니라 검증과 문서화까지 포함합니다.
- NCS 기준의 체크리스트 제작과 단위 테스트는 Unity 6 프로젝트에서 직접 확인 가능한 항목으로 바꿔야 합니다.
- HUD, 메뉴, 입력, Prefab, 알림을 통합하면 작은 UI 프레임워크 포트폴리오가 됩니다.
- 좋은 UI는 화면에 보이는 모양, 입력 반응, 게임 데이터 연결, 해상도 대응이 함께 맞아야 합니다.
