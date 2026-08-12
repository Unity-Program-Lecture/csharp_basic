# 게임 UI/UX 프로그래밍 환경 설정 안내

이 문서는 `07.GameUIUX` 과정의 DAY 02~DAY 06 실습을 시작하기 전에 Unity 프로젝트를 같은 기준으로 준비하기 위한 별첨 안내서입니다. 학생마다 UI와 입력 설정이 다르면 같은 코드를 붙여도 Button이 눌리지 않거나 글자가 보이지 않는 문제가 생길 수 있습니다. 수업 시작 전 이 문서의 체크 항목을 한 번씩 확인합니다.

## 1. 기준 환경

| 항목 | 최소 기준 | 권장 기준 | 이유 |
| :--- | :--- | :--- | :--- |
| Unity Editor | Unity 6 `6000.0` 이상 | Unity 6 LTS의 최신 패치 버전 | Unity 6의 Project-wide Input Actions와 현재 UGUI 워크플로를 기준으로 합니다. |
| 프로젝트 템플릿 | 3D Core | 3D Core | HUD·메뉴·입력 실습에 충분하며, 렌더 파이프라인 선택이 UI 실습을 막지 않습니다. |
| Unity UI (UGUI) | `com.unity.ugui` `2.0.0` 이상 | Editor에 맞는 최신 검증 버전 | Canvas, Button, Image, Slider, EventSystem을 사용합니다. |
| TextMeshPro | `com.unity.textmeshpro` `3.2.0` 이상 | Editor에 포함된 최신 검증 버전 | Canvas용 텍스트는 `TextMeshProUGUI`로 만듭니다. |
| Input System | `com.unity.inputsystem` `1.17.0` 이상 | Editor에 포함된 최신 검증 버전 | 키보드·마우스·게임패드 UI 입력을 Input System으로 처리합니다. |
| IDE | Visual Studio 2022 이상, VS Code 또는 Rider | Unity용 C# 워크로드가 설치된 Visual Studio 2022 이상 또는 VS Code + 필수 확장 | C# 스크립트를 작성하고 Console 오류를 확인합니다. |

> **버전 확인 원칙:** Package Manager에서 숫자를 임의로 낮추지 않습니다. 같은 Unity 6 Editor를 사용하는 학생은 그 Editor가 제공하는 최신 검증 패키지 버전을 사용합니다. 위 숫자는 이 과정에서 지원하는 최저선입니다.

## 2. 프로젝트 만들기

1. Unity Hub에서 **New project**를 선택합니다.
2. Editor Version이 Unity 6 `6000.0` 이상인지 확인합니다.
3. 템플릿으로 `3D Core`를 선택합니다.
4. 프로젝트 이름을 `GameUIUXLab`으로 입력합니다.
5. 생성 위치는 영문 경로를 권장합니다. 예: `D:\UnityProjects\GameUIUXLab`
6. 프로젝트를 연 뒤 Console 창에 빨간 Error가 없는지 확인합니다.

`URP` 프로젝트를 이미 사용하고 있다면 그대로 진행해도 됩니다. 이 과정의 UGUI, TextMeshPro, Input System 예제는 3D Core와 URP 모두에서 사용할 수 있습니다.

## 3. IDE와 VS Code 확장 설정

Visual Studio, VS Code, Rider 중 하나를 사용합니다. VS Code를 사용할 때는 단순 텍스트 편집기로 열지 않도록 아래 확장을 설치합니다.

| 도구 | 필수 항목 | 설치 또는 확인 방법 | 역할 |
| :--- | :--- | :--- | :--- |
| Visual Studio 2022 이상 | `Game development with Unity` 워크로드 | Visual Studio Installer에서 Modify 선택 | Unity 프로젝트·C# 자동 완성·디버깅 |
| VS Code | `C#` | Extensions에서 Microsoft 게시자의 `C#` 검색 후 설치 | C# 언어 서비스의 기반 확장 |
| VS Code | `C# Dev Kit` | Extensions에서 Microsoft 게시자의 `C# Dev Kit` 검색 후 설치 | C# 프로젝트 탐색·분석·디버깅 지원 |
| VS Code | `Unity` | Extensions에서 Microsoft 게시자의 `Unity` 검색 후 설치 | Unity C# 코드 편집·디버깅 연동 |
| Unity Editor | `Unity Visual Studio Editor` | Package Manager에서 `com.unity.ide.visualstudio` `2.0.20` 이상 확인 | Visual Studio와 VS Code의 Unity 편집기 연동 |
| Rider | Unity 지원 포함 버전 | Rider에서 Unity 플러그인 활성화 확인 | Unity 프로젝트·C# 자동 완성·디버깅 |

### VS Code를 사용할 때

1. VS Code에서 확장 탭을 열고 Microsoft 게시자의 `C#`, `C# Dev Kit`, `Unity`를 설치합니다.
2. Unity에서 `Edit > Preferences > External Tools`를 엽니다.
3. **External Script Editor**를 `Visual Studio Code`로 설정합니다.
4. Package Manager에서 `Unity Visual Studio Editor` 패키지 `com.unity.ide.visualstudio`가 `2.0.20` 이상인지 확인합니다.
5. `Regenerate project files`를 실행한 뒤, `Assets` 폴더가 아니라 프로젝트 최상위 폴더 `GameUIUXLab`을 VS Code로 엽니다.
6. C# 스크립트를 열어 `using TMPro;`와 `TextMeshProUGUI`에 오류 밑줄이 없는지 확인합니다.

> **주의:** VS Code 확장 이름과 Unity 패키지는 서로 다른 것입니다. VS Code의 `C#`, `C# Dev Kit`, `Unity` 확장과 Unity의 `Unity Visual Studio Editor` 패키지는 편집기 연동용입니다. UGUI·TextMeshPro·Input System은 반드시 Unity Package Manager에서 별도로 확인합니다. `Debugger for Unity` (`Unity.unity-debug`)는 Marketplace에 존재하지만 Unity 공식 지원 확장이 아니므로 이 과정의 필수 목록에 넣지 않습니다.

## 4. 필수 패키지 설치와 확인

메뉴에서 `Window > Package Manager`를 엽니다. 좌측 위 범위를 `Unity Registry`로 선택한 뒤 아래 패키지를 확인합니다.

| 패키지 | Package Manager 표시 이름 | 확인할 것 | 이 과정에서 쓰는 기능 |
| :--- | :--- | :--- | :--- |
| UGUI | `Unity UI` | Installed 상태, `2.0.0` 이상 | Canvas, Image, Button, Slider, Graphic Raycaster |
| TMP | `TextMeshPro` | Installed 상태, `3.2.0` 이상 | `TextMeshProUGUI`, Font Asset, 텍스트 표시 |
| Input System | `Input System` | Installed 상태, `1.17.0` 이상 | Input Action, UI Navigate, Submit, Cancel |

`Unity UI` 또는 `TextMeshPro`가 보이지 않으면 Package Manager 검색창에서 패키지 이름을 검색해 설치합니다. TMP를 처음 사용할 때 **TMP Essential Resources를 가져오겠냐는 창**이 나오면 가져옵니다. 이 리소스에는 기본 Font Asset과 UI용 머티리얼이 포함됩니다.

### Input System 활성화

1. `Edit > Project Settings > Player`를 엽니다.
2. **Other Settings > Active Input Handling**을 찾습니다.
3. 값을 `Input System Package (New)`로 설정합니다.
4. Editor 재시작 안내가 나오면 저장 후 재시작합니다.

기존 Legacy Input Manager 코드를 함께 유지해야 하는 프로젝트만 `Both`를 선택합니다. 이 과정의 새 예제에는 `Input.GetKeyDown`을 사용하지 않습니다.

## 5. UI 입력 설정

UGUI는 Canvas에 그려지는 화면이고, `EventSystem`은 입력을 Button·Slider 같은 UI 요소에 전달하는 접수 창구입니다. Input System을 쓰는 UGUI에는 `Input System UI Input Module`이 반드시 필요합니다.

### Project-wide Input Actions 확인

1. `Edit > Project Settings > Input System Package`를 엽니다.
2. **Project-wide Actions**에 Input Actions Asset이 연결되어 있는지 확인합니다.
3. Asset을 열어 `UI` Action Map이 있는지 확인합니다.
4. `UI` Action Map에 다음 Action이 있는지 확인합니다.

| Action | Type | 최소 바인딩 예시 | 사용 장면 |
| :--- | :--- | :--- | :--- |
| Navigate | Pass Through / Vector2 | 방향키, Gamepad D-pad 또는 Left Stick | 버튼 선택 이동 |
| Submit | Button | Enter, Gamepad South Button | 선택한 Button 실행 |
| Cancel | Button | Escape, Gamepad East Button | 뒤로 가기·일시정지 해제 |
| Point | Pass Through / Vector2 | Mouse Position | 마우스 포인터 위치 |
| Left Click | Pass Through / Button | Mouse Left Button | Button 클릭 |
| ScrollWheel | Pass Through / Vector2 | Mouse Scroll | Scroll View |

### EventSystem 만들기

1. Hierarchy에서 `GameObject > UI > Event System`을 선택합니다.
2. 생성된 `EventSystem` 오브젝트를 선택합니다.
3. `Input System UI Input Module` 컴포넌트가 있는지 확인합니다.
4. `Standalone Input Module`이 남아 있다면 제거하고 `Input System UI Input Module`로 교체합니다.
5. 모듈의 Action 참조가 위의 `UI` Action Map과 연결되는지 확인합니다.

> **주의:** 씬에 활성화된 EventSystem은 한 개만 둡니다. 두 개 이상이면 버튼 선택과 입력 전달이 예측하기 어려워집니다.

## 6. 공통 씬 구성

공통 씬에는 수업 시작에 꼭 필요한 뼈대만 만듭니다. HUD, 화면 Panel, Button, Slider, Toast, UIManager는 미리 만들지 않습니다. DAY 02부터 해당 수업의 목표에 맞춰 하나씩 만들고 연결합니다.

```text
GameUIUXLab
├── Main Camera
├── EventSystem
│   └── Input System UI Input Module
└── Canvas
    └── UIRoot
```

| 공통 오브젝트 | 처음에 필요한 이유 | 이후 DAY에서 추가할 것 |
| :--- | :--- | :--- |
| `Main Camera` | 기본 3D 씬과 Game View를 유지합니다. | UI가 `Screen Space - Overlay`이면 별도 연결하지 않습니다. |
| `EventSystem` | Button, Slider, 키보드·게임패드 UI 입력의 접수 창구입니다. | DAY 03~04에서 Button Navigation과 UI Action Map을 확인합니다. |
| `Canvas` | UGUI 요소가 배치될 화면입니다. | Canvas의 자식 `UIRoot` 아래에 UI를 구성합니다. |
| `UIRoot` | Canvas 안에서 UI 요소와 UI 제어 스크립트를 모아 둘 빈 오브젝트입니다. | DAY 02부터 TMP Text, Slider, Image, 화면 Panel을 자식으로 만들고, DAY 03~05에서 화면 전환·입력·UIManager 스크립트를 붙입니다. |

### Canvas 설정

Canvas를 선택하고 Inspector에서 다음 값을 확인합니다.

| 컴포넌트 | 프로퍼티 | 수업 기본값 | 확인 이유 |
| :--- | :--- | :--- | :--- |
| Canvas | Render Mode | `Screen Space - Overlay` | Camera 설정 없이 화면 UI를 확인합니다. |
| Canvas Scaler | UI Scale Mode | `Scale With Screen Size` | 해상도 변화에 따라 UI를 함께 조절합니다. |
| Canvas Scaler | Reference Resolution | `1920 x 1080` | DAY 02 HUD 배치의 기준 해상도입니다. |
| Canvas Scaler | Screen Match Mode | `Match Width Or Height` | 가로·세로 비율 차이에 대응합니다. |
| Canvas Scaler | Match | `0.5` | 너비와 높이 영향을 절반씩 반영하는 시작값입니다. |
| Graphic Raycaster | Enabled | 켜기 | Button과 Slider가 포인터 입력을 받습니다. |

### UI Transform과 Scale 주의사항

UI의 기본 크기를 `Transform Scale`로 맞추지 않습니다. `Canvas`, `UIRoot`, 화면 Panel, Button, Text, Image의 기본 Scale은 모두 `(1, 1, 1)`로 두고, 크기와 화면 대응은 `RectTransform`, Anchor, Canvas Scaler로 해결합니다. Scale은 상위 오브젝트부터 자식에게 곱해지므로, 처음에는 맞아 보이던 UI도 해상도·부모 구조·레이아웃이 바뀌면 의도와 다르게 커지거나 작아질 수 있습니다.

| Scale로 기본 크기를 조절할 때 생기는 문제 | 이유와 대응 |
| :--- | :--- |
| 해상도별 크기 판단이 어려움 | Canvas Scaler의 화면 보정에 부모·자식 Scale까지 더해집니다. 기준 Scale을 `1`로 유지하고 Canvas Scaler와 Anchor로 화면 대응을 만듭니다. |
| Layout Group 결과가 예상과 다름 | Layout Group과 Content Size Fitter는 `RectTransform`의 크기와 선호 크기를 기준으로 배치합니다. 보이는 크기만 Scale로 키우면 간격·줄바꿈·잘림을 조절하기 어려워집니다. Width/Height, Padding, Spacing을 사용합니다. |
| 이미지·테두리·얇은 글자가 흐리거나 두께가 들쭉날쭉해 보임 | 특히 비정수 Scale에서 래스터 Image와 얇은 선이 선명도를 잃을 수 있습니다. 원본 크기와 RectTransform 크기를 먼저 맞춥니다. TMP는 SDF라 비교적 견디지만, 계층 Scale 문제까지 해결해 주지는 않습니다. |
| 버튼을 크게 보이게 했는데 주변 UI와 겹침 | 클릭 효과나 강조 효과를 부모 Button에 계속 남기면 인접 UI, 마스크, 레이아웃과 충돌하기 쉽습니다. Button의 기본 RectTransform은 `1`로 유지하고, 필요하면 내부 `Visual` 자식만 잠깐 확대했다가 `1`로 되돌립니다. |

| 목적 | 기본 선택 |
| :--- | :--- |
| UI의 가로·세로 크기 변경 | `RectTransform`의 Width/Height 또는 Size Delta |
| 화면 비율·해상도 대응 | Anchor, Pivot, Canvas Scaler |
| 반복 메뉴의 정렬과 간격 | Vertical/Horizontal/Grid Layout Group의 Padding·Spacing |
| 누르는 순간의 튀는 연출 | 내부 `Visual` 자식의 임시 Local Scale 애니메이션 |

시작 전에는 `Canvas`와 `UIRoot`의 Scale이 `(1, 1, 1)`인지, UI를 담는 상위 Panel에도 의도하지 않은 Scale이 없는지 확인합니다.

## 7. TMP와 UGUI 오브젝트 연결 규칙

| 용도 | Hierarchy에서 만들 메뉴 | Inspector에 연결할 타입 | 주의점 |
| :--- | :--- | :--- | :--- |
| Canvas 텍스트 | `GameObject > UI > Text - TextMeshPro` | `TextMeshProUGUI` | 3D용 `TextMeshPro`와 구분합니다. |
| 점수·시간 | `ScoreText`, `TimeText` | `BasicHudView`의 TMP 필드 | `TextMeshProUGUI` 컴포넌트를 끌어 놓습니다. |
| 체력 | `Slider` | `BasicHudView.healthSlider` | 최소값 `0`, 최대값 `100`을 코드와 Inspector에서 확인합니다. |
| 메뉴 버튼 | `GameObject > UI > Button - TextMeshPro` | Button의 `On Click` | `UIScreenFlowController`의 public 함수를 연결합니다. |
| Toast | `ToastMessage` Prefab | `ToastMessage.messageText` | Canvas용 TMP 자식 `MessageText`를 연결합니다. |

## 8. DAY별 시작 전 점검

| DAY | 시작 전에 확인할 것 |
| :--- | :--- |
| DAY 02 | Canvas Scaler, `TextMeshProUGUI` 2개, Slider 1개, 테스트 Button |
| DAY 03 | Screen Panel 4개, Button `On Click`, EventSystem과 Graphic Raycaster |
| DAY 04 | Input System 활성화, UI Action Map, Input System UI Input Module, 키보드·게임패드 바인딩 |
| DAY 05 | `ToastMessage` Prefab, `ToastRoot`, UIRoot의 UIManager |
| DAY 06 | HUD·메뉴·입력·Toast 연결, 4개 해상도 테스트 |

## 9. 수업 시작 전 5분 점검표

| 확인 항목 | 완료 |
| :--- | :--- |
| Unity Editor가 Unity 6 `6000.0` 이상이다. |  |
| 선택한 IDE에서 Unity 프로젝트를 열었고 C# 오류 표시가 정상 동작한다. |  |
| VS Code 사용 시 Microsoft의 `C#`, `C# Dev Kit`, `Unity` 확장을 설치했다. |  |
| VS Code 사용 시 `Unity Visual Studio Editor` 패키지가 `2.0.20` 이상이다. |  |
| UGUI, TextMeshPro, Input System 패키지가 최소 버전 이상으로 설치되어 있다. |  |
| TMP Essential Resources를 가져왔다. |  |
| Active Input Handling이 `Input System Package (New)` 또는 필요한 경우 `Both`다. |  |
| EventSystem에 `Input System UI Input Module`이 있고, 활성 EventSystem은 하나다. |  |
| Canvas에 Graphic Raycaster가 있고 Canvas Scaler 기준값을 설정했다. |  |
| Canvas와 UIRoot의 Scale이 `(1, 1, 1)`이고, 기본 UI 크기를 Scale로 맞추지 않았다. |  |
| `UI` Action Map에 Navigate, Submit, Cancel, Point, Left Click이 있다. |  |
| `TextMeshProUGUI`, Button, Slider를 한 번씩 만들고 Inspector 연결이 가능하다. |  |
| Console에 빨간 Error가 없다. |  |

## 10. 자주 막히는 문제

| 증상 | 먼저 확인할 것 | 해결 방향 |
| :--- | :--- | :--- |
| Button을 눌러도 반응이 없다. | EventSystem, Input System UI Input Module, Graphic Raycaster, Raycast Target | UI 입력 모듈을 교체하고 가리는 Image의 Raycast Target을 확인합니다. |
| TMP 글자가 분홍색이거나 보이지 않는다. | TMP Essential Resources, Font Asset | TMP Essential Resources를 가져오고 Font Asset을 지정합니다. |
| VS Code에서 `TMPro` 또는 Unity 타입이 빨간 줄로 표시된다. | C# Dev Kit, Unity의 External Script Editor, 프로젝트 파일 | 확장을 설치하고 Unity에서 Project Files를 다시 생성한 뒤 프로젝트 최상위 폴더를 다시 엽니다. |
| 게임패드·방향키로 버튼 이동이 안 된다. | UI Action Map, Navigate 바인딩, Button Navigation | UI Action Map을 연결하고 Button Navigation을 Automatic 또는 Explicit으로 설정합니다. |
| HUD가 해상도에서 밀린다. | Anchor, Canvas Scaler, Reference Resolution | Anchor를 다시 잡고 `1920 x 1080`, `1280 x 720`, `1024 x 768`에서 확인합니다. |
| Pause 뒤 Toast가 사라지지 않는다. | `Time.timeScale`, `WaitForSeconds` | 현재 예제는 의도적으로 게임 시간 기준입니다. Pause 중에도 사라져야 할 요구가 생길 때만 `WaitForSecondsRealtime`을 사용합니다. |

## 오늘의 정리

- 이 과정의 공통 스택은 Unity 6, UGUI, TextMeshPro, Input System입니다.
- Canvas는 화면을 그리는 종이, EventSystem은 UI 입력을 전달하는 접수 창구라고 생각하면 됩니다.
- UGUI를 Input System으로 조작하려면 `Input System UI Input Module`을 빠뜨리면 안 됩니다.
- DAY 02 실습 전에 Canvas Scaler, TMP, Slider, EventSystem을 맞춰 두면 이후 수업에서 코드보다 설정 문제로 멈추는 일을 줄일 수 있습니다.
