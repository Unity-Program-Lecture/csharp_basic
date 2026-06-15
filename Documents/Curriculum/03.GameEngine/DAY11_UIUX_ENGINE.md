# DAY 11: UGUI UI/UX 기초

오늘의 목표는 Unity UI를 "**화면 위에 붙이는 투명한 안내판**"처럼 이해하고, Canvas, RectTransform, Button 이벤트로 간단한 HUD를 만드는 것입니다.

## 1. 핵심 개념: "게임 화면 위의 종이"

UGUI는 게임 화면 위에 체력, 점수, 버튼 같은 정보를 올리는 Unity UI 시스템입니다. Canvas는 UI 종이를 붙이는 판이고, RectTransform은 UI 요소의 위치와 크기를 정합니다. 화면 크기가 달라져도 UI가 무너지지 않게 하려면 Anchor와 Canvas Scaler를 이해해야 합니다.

### 이 단어는 무슨 뜻인가요?

- **Canvas**: UI 요소가 올라가는 화면용 무대입니다.
- **RectTransform**: UI 요소의 위치, 크기, 기준점을 다루는 Transform입니다.
- **Anchor**: UI 요소가 부모의 어느 지점을 기준으로 붙을지 정하는 값입니다.
- **Pivot**: UI 요소 안에서 회전, 크기 변경, 위치 계산의 중심이 되는 점입니다.
- **Render Mode**: Canvas가 화면에 그려지는 방식을 정하는 옵션입니다.
- **Canvas Scaler**: 해상도가 달라질 때 UI 크기를 조절하는 컴포넌트입니다.
- **Graphic Raycaster**: 마우스 클릭이나 터치가 어떤 UI에 닿았는지 검사하는 컴포넌트입니다.
- **Button Event**: 버튼을 눌렀을 때 실행할 함수를 연결하는 기능입니다.

## 2. Canvas: UI가 올라가는 화면용 종이

Canvas는 UI 요소를 올려놓는 큰 종이입니다. Text, Button, Image 같은 UGUI 요소는 보통 Canvas 아래에 있어야 화면에 보입니다. Canvas를 만들면 `Canvas`, `Canvas Scaler`, `Graphic Raycaster`가 함께 붙고, 버튼을 만들면 클릭을 처리하기 위한 `EventSystem`도 씬에 생깁니다.

Canvas의 `Render Mode`는 UI 종이를 어디에 붙일지 정합니다.

| Render Mode | 쉽게 말하면 | 주로 쓰는 상황 |
| :--- | :--- | :--- |
| `Screen Space - Overlay` | 화면 맨 위에 바로 붙이는 투명 종이 | 체력바, 점수, 메뉴처럼 항상 화면 위에 보여야 하는 UI |
| `Screen Space - Camera` | 특정 카메라 앞에 붙이는 투명 종이 | 카메라 효과, 해상도, UI 거리감을 조금 더 제어하고 싶을 때 |
| `World Space` | 씬 안에 놓는 실제 간판 | 몬스터 머리 위 체력바, 상호작용 안내판처럼 월드에 붙는 UI |

처음 UI를 배울 때는 `Screen Space - Overlay`가 가장 단순합니다. 카메라가 어디를 보든 UI가 화면 위에 바로 그려지기 때문입니다. 반대로 캐릭터 머리 위 이름표처럼 씬 안의 위치를 따라다녀야 하는 UI는 `World Space`가 더 자연스럽습니다.

`Canvas Scaler`는 화면 크기가 바뀔 때 UI 크기를 어떻게 맞출지 정합니다. 실습에서는 `UI Scale Mode`를 `Scale With Screen Size`로 바꾸고, `Reference Resolution`을 `1920 x 1080` 또는 수업 프로젝트 기준 해상도로 맞춰 두면 Game View 해상도를 바꿔도 UI 비율을 비교하기 쉽습니다.

## 3. RectTransform: 사각형 UI의 Transform

일반 GameObject는 `Transform`으로 위치, 회전, 크기를 다룹니다. UI 요소는 사각형이기 때문에 `RectTransform`을 사용합니다. RectTransform에서 가장 중요한 값은 `Anchor`, `Pivot`, `Pos`, `Width`, `Height`입니다.

`Anchor`는 부모 사각형의 어느 지점을 기준으로 붙을지 정합니다. 점수 UI를 왼쪽 위에 붙이고 싶다면 Anchor를 왼쪽 위로 둡니다. 버튼을 오른쪽 아래에 붙이고 싶다면 Anchor를 오른쪽 아래로 둡니다. 이렇게 해야 Game View 크기가 바뀌어도 UI가 의도한 모서리를 따라갑니다.

`Pivot`은 UI 자기 몸 안의 기준점입니다. 종이에 압정을 꽂는 위치라고 생각하면 쉽습니다. Pivot이 가운데면 위치, 회전, 크기 변경이 가운데를 기준으로 일어납니다. Pivot이 왼쪽 위면 왼쪽 위 모서리를 붙잡고 크기가 변합니다.

```text
Anchor = 부모 화면에서 어디에 붙을지
Pivot  = 내 사각형 안에서 어느 점을 잡을지
Pos    = Anchor에서 Pivot까지 얼마나 떨어질지
```

예를 들어 점수 Text를 화면 왼쪽 위에 놓는다면 Anchor와 Pivot을 모두 왼쪽 위로 맞추고, `Pos X`는 `20`, `Pos Y`는 `-20`처럼 여백을 줍니다. 위쪽 방향은 화면 밖이기 때문에 왼쪽 위 기준에서는 Y 값을 음수로 내려 주는 경우가 많습니다.

주의할 점도 있습니다. Anchor를 양쪽으로 벌리면 UI가 부모 크기에 맞춰 늘어나는 모드가 됩니다. 이때 Inspector에는 `Width`, `Height` 대신 `Left`, `Right`, `Top`, `Bottom` 같은 여백 값이 보일 수 있습니다. 배경 패널처럼 늘어나야 하는 UI에는 좋지만, 점수 Text처럼 고정 크기로 둘 UI에는 헷갈릴 수 있습니다.

## 4. 클릭이 동작하려면 필요한 것

Button이 눌리려면 세 가지가 맞아야 합니다.

1. 씬에 `EventSystem`이 있어야 합니다.
2. Canvas에 `Graphic Raycaster`가 있어야 합니다.
3. 클릭할 UI의 `Raycast Target`이 켜져 있어야 합니다.

버튼 위에 투명한 Image가 덮여 있고 그 Image의 `Raycast Target`이 켜져 있으면, 클릭이 버튼까지 도착하지 않을 수 있습니다. UI가 눌리지 않을 때는 "버튼 코드가 틀렸다"라고 바로 판단하지 말고, 클릭을 가로막는 UI가 있는지도 확인해야 합니다.

## 실습 예제: 점수 버튼 만들기

**미션:** UGUI Button을 누르면 화면의 점수 Text가 1씩 증가하도록 만듭니다.

1. `GameObject > UI > Canvas`를 만듭니다.
2. Canvas의 `Render Mode`를 `Screen Space - Overlay`로 둡니다.
3. Canvas Scaler의 `UI Scale Mode`를 `Scale With Screen Size`로 바꿉니다.
4. Canvas 아래에 `TextMeshPro - Text`와 `Button`을 둡니다.
5. 점수 Text는 Anchor와 Pivot을 왼쪽 위로 맞추고, 화면 안쪽으로 약간 여백을 줍니다.
6. Button은 Anchor와 Pivot을 오른쪽 아래로 맞추고, 화면 안쪽으로 약간 여백을 줍니다.
7. 빈 GameObject에 아래 스크립트를 붙입니다.
8. Button의 `On Click` 이벤트에 `AddScore` 함수를 연결합니다.

<details>
<summary>코드 보기</summary>

```csharp
using TMPro;
using UnityEngine;

public class ScoreHudController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private int score;

    void Start()
    {
        RefreshScoreText();
    }

    public void AddScore()
    {
        score++;
        RefreshScoreText();
    }

    private void RefreshScoreText()
    {
        scoreText.text = $"Score: {score}";
    }
}
```

</details>

### 실행해보면

버튼을 누를 때마다 점수가 증가합니다. Game View 해상도를 바꿔 보면서 점수 Text는 왼쪽 위를 따라가고, Button은 오른쪽 아래를 따라가는지 확인합니다. Anchor를 가운데로 바꿔 보면 해상도 변경 시 UI가 의도와 다르게 움직이는 것도 비교할 수 있습니다.

### 생각해보기

1. 점수 UI는 화면의 어느 Anchor에 붙이는 것이 자연스러울까요?
2. Anchor와 Pivot을 둘 다 왼쪽 위로 맞추면 어떤 점이 편해질까요?
3. `Screen Space - Overlay`와 `World Space`는 각각 어떤 UI에 어울릴까요?
4. 버튼 클릭 함수를 `public`으로 만들어야 Inspector에서 연결하기 쉬운 이유는 무엇일까요?

## 오늘의 정리

- UGUI는 Unity의 기본 UI 제작 시스템입니다.
- Canvas는 UI를 어디에 그릴지 정하고, Render Mode에 따라 화면용 UI와 월드용 UI를 나눌 수 있습니다.
- RectTransform은 Anchor, Pivot, Pos를 함께 이해해야 해상도 변화에도 UI가 무너지지 않습니다.
- Canvas Scaler는 해상도 변화에 맞춰 UI 크기를 조절하는 핵심 컴포넌트입니다.
- EventSystem, Graphic Raycaster, Raycast Target이 맞아야 Button 클릭이 정상적으로 전달됩니다.
- 버튼 이벤트는 Inspector에서 함수와 연결해 빠르게 테스트할 수 있습니다.
