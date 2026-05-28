# DAY 11: UGUI UI/UX 기초

오늘의 목표는 Unity UI를 "**화면 위에 붙이는 투명한 안내판**"처럼 이해하고, Canvas, RectTransform, Button 이벤트로 간단한 HUD를 만드는 것입니다.

## 1. 핵심 개념: "게임 화면 위의 종이"

UGUI는 게임 화면 위에 체력, 점수, 버튼 같은 정보를 올리는 Unity UI 시스템입니다. Canvas는 UI 종이를 붙이는 판이고, RectTransform은 UI 요소의 위치와 크기를 정합니다. 화면 크기가 달라져도 UI가 무너지지 않게 하려면 Anchor와 Canvas Scaler를 이해해야 합니다.

### 이 단어는 무슨 뜻인가요?

- **Canvas**: UI 요소가 올라가는 화면용 무대입니다.
- **RectTransform**: UI 요소의 위치, 크기, 기준점을 다루는 Transform입니다.
- **Anchor**: UI 요소가 부모의 어느 지점을 기준으로 붙을지 정하는 값입니다.
- **Canvas Scaler**: 해상도가 달라질 때 UI 크기를 조절하는 컴포넌트입니다.
- **Button Event**: 버튼을 눌렀을 때 실행할 함수를 연결하는 기능입니다.

## 실습 예제: 점수 버튼 만들기

**미션:** UGUI Button을 누르면 화면의 점수 Text가 1씩 증가하도록 만듭니다.

1. `GameObject > UI > Canvas`를 만듭니다.
2. Canvas 아래에 `TextMeshPro - Text`와 `Button`을 둡니다.
3. 빈 GameObject에 아래 스크립트를 붙입니다.
4. Button의 `On Click` 이벤트에 `AddScore` 함수를 연결합니다.

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

버튼을 누를 때마다 점수가 증가합니다. Game View 해상도를 바꿔 보면서 UI 위치가 원하는 기준에 붙어 있는지 확인할 수 있습니다.

### 생각해보기

1. 점수 UI는 화면의 어느 Anchor에 붙이는 것이 자연스러울까요?
2. 버튼 클릭 함수를 `public`으로 만들어야 Inspector에서 연결하기 쉬운 이유는 무엇일까요?

## 오늘의 정리

- UGUI는 Unity의 기본 UI 제작 시스템입니다.
- Canvas와 RectTransform은 UI 배치의 핵심입니다.
- 버튼 이벤트는 Inspector에서 함수와 연결해 빠르게 테스트할 수 있습니다.
