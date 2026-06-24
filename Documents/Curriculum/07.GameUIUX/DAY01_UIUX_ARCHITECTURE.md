# DAY 01: 게임 UI/UX 분석과 아키텍처 설계

오늘의 목표는 게임 UI/UX를 "**플레이어와 게임이 대화하는 창구**"로 이해하고, 기획 문서에서 필요한 UI 요소를 뽑아 Unity 6에서 구현할 화면 구조로 정리하는 것입니다.

## NCS 연결

- 능력단위: `0803020529_18v4 게임 UIUX 프로그래밍`
- 능력단위 요소: 게임 UI/UX 설계하기
- 관련 학습 내용: UI/UX 분석, 계획 수립, 리소스 설계, 콘셉트 아이디어 도출, UI/UX 아키텍처 설계
- Unity 6 재구성: 기획 문서를 바탕으로 UGUI Canvas, 화면 흐름, 입력 방식, UI Prefab 구조를 설계합니다.

## 1. 핵심 개념: "게임과 플레이어 사이의 통역사"

UI는 플레이어가 게임을 조작하고 정보를 확인하는 화면 장치입니다. UX는 플레이어가 그 UI를 사용하면서 느끼는 편리함, 답답함, 명확함, 몰입감까지 포함합니다.

예를 들어 체력바는 UI입니다. 그런데 체력이 줄어들 때 색이 바뀌고, 위험할 때 화면이 살짝 흔들리며, 회복하면 초록색 효과가 뜬다면 그것은 UX까지 고려한 설계입니다.

### 이 단어는 무슨 뜻인가요?

- **UI**: 버튼, 체력바, 점수판, 메뉴처럼 플레이어가 보고 조작하는 화면 요소입니다.
- **UX**: 플레이어가 UI를 사용하면서 느끼는 전체 경험입니다.
- **HUD**: 플레이 중 항상 보이는 체력, 점수, 탄약, 미니맵 같은 정보 영역입니다.
- **인터페이스 아키텍처**: UI 화면, 입력, 데이터, 전환 흐름을 어떻게 나눌지 정리한 설계도입니다.
- **와이어프레임**: 색과 장식을 빼고 UI 위치와 구조만 그린 초안입니다.
- **피드백**: 플레이어 행동에 대해 게임이 보여주는 반응입니다. 버튼 색 변화, 효과음, 팝업 메시지가 모두 피드백입니다.

## 2. UI/UX 요소 추출하기

NCS 교재는 게임 기획 문서에서 UI/UX를 추출하고 분석한 뒤 작업 순서를 정하도록 요구합니다. Unity 수업에서는 다음 네 가지로 나누어 정리합니다.

| 구분 | 질문 | Unity 6 예시 |
| :--- | :--- | :--- |
| 입력 인터페이스 | 플레이어가 무엇으로 조작하나요? | 키보드, 마우스, 게임패드, 터치, Input System Action |
| 게임 화면 인터페이스 | 플레이 중 무엇을 보여주나요? | 체력바, 점수, 쿨타임, 목표 안내, 미니맵 |
| 컨트롤 인터페이스 | 플레이어가 누르는 조작 UI는 무엇인가요? | 시작 버튼, 스킬 버튼, 인벤토리 버튼, 설정 슬라이더 |
| 커스텀 인터페이스 | 게임 장르에 맞는 특별한 UI는 무엇인가요? | 리듬 판정선, 카드 선택 UI, 대화 선택지, 퀘스트 트래커 |

## 3. Unity 6 UI 아키텍처 기본 구조

처음부터 모든 UI를 한 스크립트에 넣으면 나중에 고치기 어렵습니다. 그래서 화면, 데이터, 입력, 전환을 분리해서 생각합니다.

```text
Game State       -> PlayerStatus, Inventory, QuestState
UI Controller    -> HudController, MenuController
UGUI View        -> TextMeshPro, Image, Button, Slider
Input System     -> Submit, Cancel, Navigate, Pointer
Scene Flow       -> Play, Pause, Result
```

쉽게 말하면 데이터는 "**실제 체력**"이고, UI는 "**체력계 눈금**"입니다. 체력계가 예쁘게 보인다고 실제 체력이 바뀌는 것은 아닙니다. 실제 체력이 바뀌면 UI가 그 결과를 보여주도록 연결해야 합니다.

## 4. 설계 문서에 포함할 내용

오늘 작성할 설계 문서는 코드를 짜기 전의 지도입니다.

| 항목 | 작성 내용 |
| :--- | :--- |
| 화면 목록 | HUD, 일시정지 메뉴, 설정 메뉴, 결과 화면 등 |
| 화면 전환 | 어떤 버튼이나 조건으로 화면이 바뀌는지 |
| 표시 정보 | 체력, 점수, 시간, 목표, 아이템 수량 등 |
| 입력 방식 | 키보드, 마우스, 게임패드, 터치 중 무엇을 지원하는지 |
| UI Prefab 후보 | 반복해서 쓸 버튼, 슬롯, 팝업, 알림 |
| 해상도 기준 | 기본 기준 해상도와 Canvas Scaler 설정 |
| 피드백 방식 | 색 변화, 애니메이션, 사운드, 비활성화 표시 |

## 실습 예제: 미니 액션 게임 UI 설계서 만들기

**미션:** "몬스터를 잡고 점수를 얻는 미니 액션 게임"의 UI/UX 설계 초안을 작성합니다.

### 1단계: 화면 목록

```text
1. TitleScreen
   - 시작 버튼
   - 설정 버튼
   - 종료 버튼

2. PlayHud
   - 플레이어 체력바
   - 점수 Text
   - 남은 시간 Text
   - 목표 안내 Text

3. PauseMenu
   - 계속하기 버튼
   - 설정 버튼
   - 타이틀로 돌아가기 버튼

4. ResultScreen
   - 최종 점수
   - 처치한 몬스터 수
   - 다시하기 버튼
```

### 2단계: 입력 인터페이스

```text
Move      : WASD / Left Stick
Attack    : Mouse Left Button / Gamepad South Button
Pause     : Esc / Start Button
Submit    : Enter / Gamepad South Button
Cancel    : Esc / Gamepad East Button
Navigate  : Arrow Keys / D-Pad / Left Stick
```

### 3단계: UI Prefab 후보

```text
HealthBar
ScoreText
MenuButton
OptionSlider
InventorySlot
ToastMessage
```

### 4단계: Unity 오브젝트 구조 초안

```text
Canvas
├── Screen_Title
├── Screen_PlayHud
│   ├── HealthBar
│   ├── ScoreText
│   ├── TimeText
│   └── ObjectiveText
├── Screen_Pause
└── Screen_Result

EventSystem
UIRoot
└── UIFlowController
```

### 실행해보면

아직 Unity에서 동작하는 화면은 만들지 않습니다. 대신 "무엇을 만들지"가 분명해집니다. 다음 수업부터 이 설계서를 보고 Canvas, TextMeshPro, Button, Slider를 실제로 배치합니다.

### 생각해보기

1. 체력바와 점수 Text는 같은 HUD에 있어도 데이터 출처가 서로 다를까요?
2. 버튼을 눌렀을 때 색이 바뀌지 않으면 플레이어는 어떤 불편을 느낄까요?
3. 게임패드까지 지원하려면 마우스 클릭 UI와 무엇이 달라져야 할까요?
4. 자주 반복해서 쓰는 UI를 Prefab으로 만들면 어떤 점이 편할까요?

## 오늘의 정리

- UI는 플레이어가 보고 조작하는 화면 요소이고, UX는 그 요소를 사용하며 느끼는 전체 경험입니다.
- NCS 기준으로 UI/UX 설계는 입력, 화면, 컨트롤, 커스텀 인터페이스를 추출하는 일에서 시작합니다.
- Unity 6에서는 UGUI Canvas, TextMeshPro, Button, Slider, Input System을 기준으로 설계를 구현합니다.
- 코드를 만들기 전에 화면 목록, 입력 방식, 전환 흐름, Prefab 후보를 정리하면 구현 중 길을 잃지 않습니다.
