# DAY 14: 실제 게임 UI의 프로파일 분석과 개선

오늘은 DAY 13까지 연결한 게임에서 매 프레임 UI 문자열을 만드는 비용을 측정합니다. 게임과 별개인 탐색 예제를 추가하지 않고 기존 GameSession을 개선합니다. NCS G6·G8·G9를 다룹니다.

## 1. 핵심 개념: 전광판 숫자가 바뀔 때만 다시 쓰기

초 단위로 보이는 타이머를 매 프레임 새 문자열로 만들 필요는 없습니다. 먼저 비용을 측정하고, 점수나 표시 초가 바뀔 때만 UI를 갱신하도록 바꿉니다.

### 이 단어는 무슨 뜻인가요?

- **ProfilerMarker**: 코드 구간에 이름을 붙여 측정하는 도구입니다.
- **GC Alloc**: 관리 메모리가 새로 할당되는 양입니다. GC 수집 시간과 다릅니다.
- **캐시**: 마지막으로 표시한 값을 기억해 같은 일을 반복하지 않게 하는 방법입니다.
- **기준값**: 수정 전 같은 조건에서 측정해 둔 비교 출발점입니다.

## 2. 실습: 현재 코드의 기준값 측정

**미션:** 기존 GameSession의 RefreshUi를 측정하고 Player에서 수정 전 결과를 보관합니다.

1. 파일 위에 `using Unity.Profiling;`을 추가합니다.
2. 클래스 필드에 다음 마커를 추가합니다.

```csharp
    private static readonly ProfilerMarker UiMarker = new ProfilerMarker("CoinLab.PresentUi");
```

3. 기존 `RefreshUi` 본문 전체를 `using (UiMarker.Auto()) { ... }`로 감쌉니다. 이 단계에서는 호출 빈도와 문자열 갱신을 바꾸지 않습니다.
4. StreamingAssets/platform-config.json의 roundSeconds를 측정용 30으로 바꿉니다. Windows 프로필의 로컬 콘텐츠를 빌드하고 Development Build·Autoconnect Profiler를 켜 Player를 별도 폴더에 빌드합니다. Window > Analysis > Profiler를 열고 연결 대상이 Editor가 아니라 실행한 Player인지 확인합니다.
5. Player 메뉴에서 Load Settings로 30초 설정을 읽은 뒤 Start → 시간 종료 → Restart를 같은 순서로 수행합니다. 워밍업 후 라운드 구간을 30초씩 3회 측정하고 CPU 시간·GC Alloc·느린 프레임을 보관합니다. 수정 후에도 같은 JSON과 빌드 옵션으로 재측정합니다.
6. GPU처럼 도구에서 제공하지 않는 값은 미지원으로 표시합니다. Deep Profile은 원인 조사 때만 쓰고 전후 비교에서는 같은 옵션을 유지합니다.

## 3. 실습: 표시 값이 바뀔 때만 갱신

**미션:** 마커는 유지하고 다음 필드를 추가한 뒤 RefreshUi만 아래 코드로 교체합니다. DAY 04까지 추가한 저장 코드와 DAY 13의 설정 로더 연결은 유지합니다.


```csharp
    // 처음 표시할 0점과 구분하려고 -1로 시작합니다. 첫 RefreshUi에서는 반드시 갱신됩니다.
    private int shownScore = -1;
    private int shownSeconds = -1;
    private int shownBest = -1;
    private bool shownPlaying;
    private bool shownResult;
    private bool shownReady;
```


```csharp
    private void RefreshUi()
    {
        // using 블록을 벗어날 때 측정 구간이 끝납니다. 중간에 return해도 측정이 종료됩니다.
        using (UiMarker.Auto())
        {
            // 실제 남은 시간 대신 화면에 보이는 올림한 초를 비교하므로, 매 프레임 타이머 문자열을 만들지 않습니다.
            int seconds = Mathf.CeilToInt(round.Remaining);
            bool ready = IsReady;
            // 점수·시간뿐 아니라 화면 상태와 시작 가능 여부까지 모두 같을 때만 갱신을 건너뜁니다.
            if (shownScore == Score
                && shownSeconds == seconds
                && shownBest == best
                && shownPlaying == playing
                && shownResult == showingResult
                && shownReady == ready)
            {
                return;
            }

            // 이번에 표시할 값을 기억해 두고 다음 호출에서 비교합니다.
            shownScore = Score;
            shownSeconds = seconds;
            shownBest = best;
            shownPlaying = playing;
            shownResult = showingResult;
            shownReady = ready;
            menuPanel.SetActive(!playing && !showingResult);
            hudPanel.SetActive(playing);
            resultPanel.SetActive(showingResult);
            startButton.interactable = ready;
            hud.text = $"Score: {Score}  Time: {seconds}";
            resultText.text = $"Score: {Score}";
            bestText.text = $"Best: {best}";
        }
    }
```

Status 텍스트는 SetStatus에서 메시지가 바뀔 때만 작성하므로 이 캐시와 별도로 관리합니다.

## 4. 같은 게임인지 다시 확인

처음 메뉴, Start, 수집, 시간 종료, 결과, Restart, 메뉴 복귀, Reload Stage, 설정 읽기, 최고 점수 표시를 모두 확인합니다. 그 뒤 수정 전과 같은 장치·씬·해상도·라운드 시간·빌드 옵션으로 다시 3회 측정합니다.

효과가 작다면 작은 효과라고 기록합니다. 더 큰 차이를 보여 주려고 최종 게임에 불필요한 반복 작업을 추가하지 않습니다. FPS가 같아도 할당과 CPU 시간이 달라질 수 있으며, 반대로 할당이 줄어도 전체 병목은 렌더링일 수 있습니다.

전후 측정을 모두 마치면 platform-config.json을 45초로 복원하고 메뉴에서 다시 읽어 다음 라운드가 45초인지 확인합니다. 최종 배포본은 복원한 JSON을 포함해 다시 빌드합니다. 측정용 30초 출력과 최종 출력은 별도 폴더로 보관합니다.

## 프로젝트 완료 확인

- [ ] 동일 조건 수정 전후 프로파일을 각 3회 기록했습니다.
- [ ] 메뉴·로딩·수집·결과·저장·설정 변경 동작을 다시 확인했습니다.

오늘의 프로젝트를 `Progress/Day14`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. FPS가 같아도 UI 문자열 할당을 줄인 의미가 있을 수 있는 이유는 무엇인가요?
2. 점수만 캐시하고 로딩 상태와 화면 상태를 비교하지 않으면 어떤 문제가 생기나요?

## 오늘의 정리

실제 프로젝트의 UI를 개선하고 기능과 측정 결과를 함께 확인했습니다. 다음 수업에서는 플랫폼별 렌더링·콘텐츠 비용을 개선해 최종 빌드를 준비합니다.
