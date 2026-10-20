# DAY 04: 최고 점수 저장과 화면 대응

오늘은 DAY 03의 게임에 최고 점수 표시와 저장을 연결하고 화면 비율이 바뀌어도 UI를 사용할 수 있도록 구성합니다. NCS G4·G7·G8을 다룹니다.

## 1. 핵심 개념: 게임을 꺼도 남는 기록

한 판의 진행 기록은 새로 시작할 때 비우지만, 최고 점수는 다음 실행에도 남깁니다. 결과 화면에서 확인한 값과 다시 실행해 읽은 값을 비교해 보겠습니다.

### 이 단어는 무슨 뜻인가요?

- **PlayerPrefs**: 정수·실수·문자열 같은 작은 값을 저장하는 기능입니다. 암호화나 점수 위변조 방지 수단은 아닙니다.
- **Safe Area**: 기기 테두리나 카메라 영역을 고려해 UI를 배치할 영역입니다.
- **Canvas Scaler**: 기준 해상도에 맞춰 화면 크기에 따른 UI 크기를 조정합니다.

## 2. 실습: 기존 GameSession에 저장 연결

**미션:** DAY 02에 작성한 GameSession.cs를 열어 아래 필드와 메서드를 추가·교체합니다. 기존 이동·수집·시작·결과 코드는 유지하며 같은 이름의 클래스를 새 파일에 추가하지 않습니다.

필드에 다음 세 줄을 추가합니다.

```csharp
    [SerializeField] private TMP_Text bestText;

    private const string BestKey = "PlatformCoinLab.Best";
    private int best;
```

`Awake`의 첫 줄에 아래 코드를 추가합니다. 나머지 초기화는 유지합니다.

```csharp
    // 저장한 최고 점수가 없으면 기본값 0으로 시작합니다.
    best = PlayerPrefs.GetInt(BestKey, 0);
```

`FinishRound`를 교체하고 `SaveBest`, `OnApplicationPause`를 추가합니다.

```csharp
    private void FinishRound()
    {
        playing = false;
        showingResult = true;
        SaveBest();
    }

    public void SaveBest()
    {
        // 기존 최고 점수와 이번 점수 중 큰 값만 남겨 기록이 낮아지지 않게 합니다.
        best = Mathf.Max(best, Score);
        PlayerPrefs.SetInt(BestKey, best);
        // 메모리에서 바꾼 저장 값을 실제 저장소에 반영합니다.
        PlayerPrefs.Save();
    }

    private void OnApplicationPause(bool value)
    {
        // 중단이면 true, 복귀이면 false입니다. 같은 값을 이동·시간 처리의 잠금에도 사용합니다.
        paused = value;
        if (value)
        {
            SaveBest();
        }
    }
```

`RefreshUi`의 마지막 줄에 최고 점수 표시를 추가합니다.

```csharp
    bestText.text = $"Best: {best}";
```

1. Canvas/SafeAreaRoot 아래 UI > Text - TextMeshPro로 BestText를 만듭니다. 표시를 `Best: 0`, 글자 크기를 24~28로 두고 Raycast Target을 끕니다. RectTransform의 크기는 `(300,50)`, Anchor·Pivot은 중앙, 위치는 `(590,375)`로 정합니다. GameSession의 Best Text에 연결해 메뉴와 결과에서도 계속 보이게 합니다.
2. 같은 SafeAreaRoot 아래 UI Button으로 SaveButton을 만듭니다. 자식 텍스트는 `SAVE BEST`, 크기는 `(220,50)`, Anchor·Pivot은 중앙, 위치는 `(590,310)`으로 둡니다. 버튼 배경의 Raycast Target은 켜고 자식 텍스트는 끕니다. On Click에 `GameSession.SaveBest()`를 연결합니다.
3. 결과 화면에서 자동 저장되는지 확인한 뒤 앱을 종료하고 다시 실행해 Best가 유지되는지 확인합니다.
4. Web에서는 사용자 버튼 조작과 결과 확정 시점에 저장합니다. 사이트 데이터 삭제·비공개 모드에서는 보존 조건이 달라질 수 있습니다.
5. Pause 코드는 저장 시점을 놓치지 않기 위해 함께 추가했습니다. 중단과 복귀 테스트는 DAY 05에서 수행합니다. `Time.timeScale` 대신 이 게임의 `paused` 상태로 진행과 이동을 멈춥니다.

## 3. 실습: Safe Area 연결

**미션:** `SafeAreaFitter.cs`를 작성해 기존 Canvas 바로 아래 SafeAreaRoot에 붙입니다.

```csharp
using UnityEngine;

public sealed class SafeAreaFitter : MonoBehaviour
{
    private RectTransform target;
    private Rect previous;
    private Vector2Int previousSize;

    private void Awake()
    {
        target = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Rect safe = Screen.safeArea;
        Vector2Int size = new Vector2Int(Screen.width, Screen.height);
        // 화면 크기가 0이면 나눗셈을 하지 않습니다.
        // 안전 영역과 화면 크기가 모두 그대로이면 같은 배치를 다시 계산하지 않습니다.
        if (size.x <= 0 || size.y <= 0 || (safe == previous && size == previousSize))
        {
            return;
        }

        previous = safe;
        previousSize = size;

        // 픽셀 좌표를 화면 크기로 나누어 Anchor가 사용하는 0~1 비율로 바꿉니다.
        // 예: 화면 너비 1000에서 안전 영역의 왼쪽이 50이면 X 최소 Anchor는 0.05입니다.
        // anchorMin은 왼쪽 아래, anchorMax는 오른쪽 위 모서리입니다.
        target.anchorMin = new Vector2(safe.xMin / size.x, safe.yMin / size.y);
        target.anchorMax = new Vector2(safe.xMax / size.x, safe.yMax / size.y);
        // 추가 픽셀 여백을 없애 RectTransform이 위에서 정한 Anchor 영역을 그대로 채우게 합니다.
        target.offsetMin = Vector2.zero;
        target.offsetMax = Vector2.zero;
    }
}
```

SafeAreaRoot는 화면 전체 Stretch, offset 0으로 시작합니다. SafeAreaFitter는 이 오브젝트에 붙입니다. BestText·SaveButton을 활성화하고, GameSession의 Best Text 필드에는 BestText를, SaveButton의 On Click에는 `GameSession.SaveBest()`를 연결합니다.

DAY 02에서 만든 패널과 오늘 추가한 UI는 아래 배치를 사용합니다. Anchor·Pivot은 아직 중앙 기준이며 위치는 부모 기준 Anchored Position입니다. DAY 15에서 상단·우측·좌하단 Anchor로 바꾸어 화면 비율 대응을 보완합니다.

| 요소 | Anchor·Pivot | 시작 크기·배치 |
| :--- | :--- | :--- |
| MenuPanel | 중앙 | 480×600, 위치 (0,0) |
| HudPanel / HudText | 중앙 | 패널 520×70, 위치 (0,370), 텍스트 500×60, 위치 (0,0) |
| ResultPanel / ResultText | 중앙 | 패널 480×360, 위치 (0,0), 텍스트 440×60, 위치 (0,45) |
| BestText | 중앙 | 300×50, 위치 (590,375) |
| SaveButton | 중앙 | 220×50, 위치 (590,310) |
| StatusText | MenuPanel 중앙 | 440×80, 위치 (0,-190) |

Canvas는 Screen Space - Overlay, Canvas Scaler는 Scale With Screen Size, Reference Resolution `(1600,900)`, Match 0.5를 사용합니다. 폰트는 우선 TMP 기본 영문 폰트를 쓰고 한글을 표시할 때만 한글 글리프를 준비합니다. 텍스트와 장식 Image의 Raycast Target은 끄고 버튼의 입력 영역만 켭니다.

먼저 16:9에서 배치를 확인하고 다른 화면 비율과 Safe Area에서도 살펴봅니다. 요소가 가려지거나 겹치면 크기·화면 비율·오브젝트를 기록합니다. SafeAreaFitter는 안전 영역을 정할 뿐, 자식 UI의 고정 위치까지 자동으로 다시 배치하지는 않습니다. 이 문제는 DAY 15의 Anchor 보완에서 이어서 해결합니다.

## 프로젝트 완료 확인

- [ ] 결과의 최고 점수가 다음 실행에도 표시됩니다.
- [ ] SafeAreaRoot가 안전 영역에 맞게 변하고, 자식 UI가 가려지는 화면 비율은 기록했습니다.

오늘의 프로젝트를 `Progress/Day04`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. 게임 진행 기록과 최고 점수는 왜 서로 다른 시점에 초기화하나요?
2. Safe Area와 Canvas Scaler는 각각 무엇을 조정하나요?

## 오늘의 정리

점수 저장 결과가 실제 화면과 다음 실행에 연결되었습니다. 다음 수업에서는 같은 게임의 중단·복귀와 플랫폼별 종료 동작을 확인합니다.
