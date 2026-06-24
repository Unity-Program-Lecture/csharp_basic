# DAY 02: UGUI 레이아웃과 해상도 대응

오늘의 목표는 Unity 6 UGUI를 사용해 HUD 화면을 만들고, Canvas Scaler와 RectTransform을 이용해 해상도가 달라져도 무너지지 않는 UI 배치를 익히는 것입니다.

## NCS 연결

- 능력단위 요소: 게임 UI/UX 요소 프로그래밍하기
- 관련 학습 내용: GUI 디자인 가이드 이해, 구현 가능성 검토, UI 구현 표준 기반 요소 제작
- Unity 6 재구성: Canvas, RectTransform, Anchor, Pivot, Canvas Scaler, TextMeshPro, Image, Slider를 사용해 HUD를 구현합니다.

## 1. 핵심 개념: "화면 위에 붙이는 투명한 종이"

Canvas는 UI를 올려놓는 큰 종이입니다. Text, Image, Button, Slider 같은 UGUI 요소는 보통 Canvas 아래에 있어야 화면에 보입니다.

UI 레이아웃에서 가장 중요한 질문은 "어느 기준점에 붙일 것인가"입니다. 점수는 왼쪽 위, 버튼은 오른쪽 아래, 체력바는 왼쪽 아래처럼 기준을 정해야 해상도가 바뀌어도 UI가 자연스럽게 따라갑니다.

### 이 단어는 무슨 뜻인가요?

- **Canvas**: UI 요소가 올라가는 화면용 무대입니다.
- **RectTransform**: UI 요소의 위치, 크기, 기준점을 다루는 Transform입니다.
- **Anchor**: 부모 화면의 어느 지점을 기준으로 붙을지 정하는 값입니다.
- **Pivot**: UI 자기 몸 안에서 위치 계산의 중심이 되는 점입니다.
- **Canvas Scaler**: 해상도 변화에 맞춰 UI 크기를 조절하는 컴포넌트입니다.
- **Reference Resolution**: UI를 설계할 때 기준으로 삼는 해상도입니다.

## 2. Canvas 설정 기준

| 컴포넌트 | 권장 설정 | 이유 |
| :--- | :--- | :--- |
| Canvas | `Screen Space - Overlay` | HUD와 메뉴를 가장 단순하게 화면 위에 표시합니다. |
| Canvas Scaler | `Scale With Screen Size` | 해상도가 바뀌어도 UI 비율을 유지합니다. |
| Reference Resolution | `1920 x 1080` | FHD 기준으로 UI를 설계하기 쉽습니다. |
| Screen Match Mode | `Match Width Or Height` | 가로/세로 비율 변화에 대응합니다. |
| Match | `0.5`부터 시작 | 너비와 높이를 절반씩 반영합니다. |

실제 프로젝트에서는 모바일 세로 화면, PC 16:9, 울트라와이드처럼 목표 플랫폼에 따라 기준이 달라질 수 있습니다. 수업에서는 먼저 `1920 x 1080`을 기준으로 익힙니다.

## 3. HUD 배치 원칙

| UI 요소 | 추천 Anchor | 이유 |
| :--- | :--- | :--- |
| 체력바 | 왼쪽 위 또는 왼쪽 아래 | 플레이 중 자주 확인하는 핵심 정보입니다. |
| 점수 | 왼쪽 위 | 빠르게 읽기 쉽습니다. |
| 남은 시간 | 위쪽 가운데 | 전체 목표와 연결됩니다. |
| 스킬 버튼 | 오른쪽 아래 | 마우스/터치 조작에 어울립니다. |
| 목표 안내 | 위쪽 또는 오른쪽 | 플레이 흐름을 방해하지 않게 둡니다. |

UI는 예쁘게 보이는 것도 중요하지만, 플레이 중 시선을 빼앗지 않는 것이 더 중요합니다.

## 실습 예제: 반응형 HUD 만들기

**미션:** 체력바, 점수, 남은 시간을 표시하는 HUD를 만들고 Game View 해상도를 바꿔도 위치가 유지되는지 확인합니다.

### Unity 오브젝트 만들기

1. `GameObject > UI > Canvas`를 만듭니다.
2. Canvas의 `Render Mode`를 `Screen Space - Overlay`로 둡니다.
3. Canvas Scaler의 `UI Scale Mode`를 `Scale With Screen Size`로 바꿉니다.
4. `Reference Resolution`을 `1920 x 1080`으로 설정합니다.
5. Canvas 아래에 `TextMeshPro - Text`, `Slider`, `Image`를 배치합니다.
6. 왼쪽 위에는 점수 Text, 왼쪽 아래에는 체력 Slider, 위쪽 가운데에는 시간 Text를 둡니다.
7. 테스트용 Button을 하나 만들고 `AddScoreAndDamage` 함수에 연결합니다.

### 스크립트 작성

<details>
<summary>코드 보기</summary>

```csharp
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicHudView : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Slider healthSlider;

    private int score;
    private float remainTime = 60f;
    private float health = 100f;

    void Start()
    {
        healthSlider.minValue = 0f;
        healthSlider.maxValue = 100f;
        Refresh();
    }

    void Update()
    {
        remainTime = Mathf.Max(0f, remainTime - Time.deltaTime);
        Refresh();
    }

    public void AddScoreAndDamage()
    {
        score += 10;
        health = Mathf.Max(0f, health - 10f);
        Refresh();
    }

    private void Refresh()
    {
        scoreText.text = $"Score: {score}";
        timeText.text = $"Time: {remainTime:0}";
        healthSlider.value = health;
    }
}
```

</details>

### 코드 읽기

- `위->아래`: 필드에서 어떤 UI를 연결할지 먼저 보고, `Start`, `Update`, `Refresh` 순서로 읽습니다.
- `오->왼`: `scoreText.text = $"Score: {score}"`는 오른쪽 문자열을 만든 뒤 왼쪽 Text에 넣는다는 뜻입니다.
- `안->밖`: `Mathf.Max(0f, health - 10f)`는 먼저 `health - 10f`를 계산하고, 그 값과 `0f` 중 큰 값을 고릅니다.

### 실행해보면

테스트 버튼을 누를 때마다 점수가 오르고 체력이 줄어듭니다. Game View를 `16:9`, `4:3`, `Free Aspect`로 바꿔 보면서 각 UI가 의도한 모서리나 중앙을 따라가는지 확인합니다.

### 생각해보기

1. 체력바의 Anchor가 가운데로 되어 있으면 해상도 변경 시 어떤 문제가 생길까요?
2. TextMeshPro를 사용하는 이유는 무엇일까요?
3. `Refresh` 함수를 따로 만든 이유는 무엇일까요?
4. HUD가 너무 많은 정보를 보여주면 UX에는 어떤 문제가 생길까요?

## 오늘의 정리

- Canvas Scaler는 해상도 대응의 출발점입니다.
- RectTransform의 Anchor와 Pivot은 UI 위치를 안정적으로 잡는 핵심입니다.
- HUD는 자주 보는 정보를 빠르게 읽을 수 있게 배치해야 합니다.
- UI 표시 값은 게임 데이터와 연결되어야 하며, 값이 바뀔 때 화면도 함께 갱신되어야 합니다.
