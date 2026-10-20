# DAY 02: 게임 진행 기록과 한 판의 흐름 구현

오늘은 점수·남은 시간·수집한 코인 정보를 설계하고, 같은 규칙을 씬과 UI에 연결합니다. 시작 버튼을 누른 뒤 수집·시간 종료·결과·재도전으로 이어지는 한 판을 만듭니다. NCS G2·G4·G5·G6·G7·G8을 다룹니다.

## 1. 핵심 개념: 게임 진행 기록과 입력 처리 나누기

게임 진행 기록은 어떤 입력 장치로 조작했는지 몰라도 됩니다. `CoinRound`는 점수, 남은 시간, 수집한 코인 ID와 종료 여부를 관리합니다. MonoBehaviour는 입력과 충돌을 게임 규칙에 연결하고, 게임 진행 기록을 화면에 표시합니다.

### 이 단어는 무슨 뜻인가요?

- **List**: 순서대로 자료를 보관하는 줄입니다.
- **Dictionary**: 이름표나 ID로 자료를 찾는 사물함입니다.
- **HashSet**: 같은 항목을 한 번만 보관하는 명단입니다.
- **상태**: 현재 점수·시간·종료 여부 같은 게임의 기억입니다.

## 2. 실습: 게임 진행 기록 클래스 작성

**미션:** `Assets/Scripts/Core/CoinRound.cs`를 만들고 다음 코드를 입력합니다.

Scripts와 Core 폴더가 없다면 먼저 만듭니다. 새 C# 스크립트의 기본 내용을 아래 코드로 모두 바꿉니다. CoinRound는 MonoBehaviour가 아닌 일반 C# 클래스이므로 GameObject에 붙이지 않습니다. 이어지는 실습에서 GameSession이 인스턴스를 생성해 사용합니다.

```csharp
using System;
using System.Collections.Generic;

public sealed class CoinRound
{
    private readonly HashSet<int> collectedIds = new HashSet<int>();

    public int Score { get; private set; }
    public float Remaining { get; private set; }

    public bool IsFinished => Remaining <= 0f;

    public CoinRound(float seconds)
    {
        Reset(seconds);
    }

    public bool TryCollect(int coinId)
    {
        // 안->밖: Add는 새 ID를 넣으면 true, 이미 있는 ID이면 false를 돌려줍니다.
        // !로 뒤집어 중복을 거절하며, 종료 상태이면 ||의 오른쪽 Add는 실행하지 않습니다.
        if (IsFinished || !collectedIds.Add(coinId))
        {
            return false;
        }

        Score += 1;

        return true;
    }

    public void Tick(float deltaTime)
    {
        // NaN은 숫자가 아닌 값입니다. 음수·NaN·무한대는 정상적인 경과 시간으로 계산하지 않습니다.
        if (deltaTime < 0f || float.IsNaN(deltaTime) || float.IsInfinity(deltaTime))
        {
            throw new ArgumentOutOfRangeException(nameof(deltaTime));
        }

        // 오->왼: 남은 시간에서 경과 시간을 빼고, 음수가 되면 0을 저장합니다.
        Remaining = Math.Max(0f, Remaining - deltaTime);
    }

    public void Reset(float seconds)
    {
        if (seconds <= 0f || float.IsNaN(seconds) || float.IsInfinity(seconds))
        {
            throw new ArgumentOutOfRangeException(nameof(seconds));
        }

        // 다음 라운드에서는 같은 코인을 다시 모을 수 있도록 수집 ID를 비웁니다.
        collectedIds.Clear();
        Score = 0;
        Remaining = seconds;
    }
}
```

### 자료 구조 선택

`HashSet`은 같은 코인을 두 번 수집하지 못하도록 기록하는 데 적합합니다. 씬에 배치한 코인 목록은 Inspector 배열로 관리해 순서를 유지합니다. 나중에 ID로 찾아야 할 설정이 많아지면 `Dictionary`를 고려합니다. 지금은 코인 10개를 관리하는 데 필요한 만큼의 구조로 시작합니다.

## 3. 설계 문서 작성

| 자료 | 구조 | 이유 |
| :--- | :--- | :--- |
| 수집한 코인 ID | HashSet<int> | 중복 점수 방지 |
| 씬 코인 참조 | CoinPickup[] | Inspector에서 명확하게 연결 |
| 게임 점수·시간 | CoinRound 속성 | UI·입력 장치와 분리 |
| 최고 점수 | int | 작은 저장 자료 |
| 설정 응답 | roundSeconds 정수 JSON | DAY 13 통신 예제 |

게임 흐름을 `입력 읽기 → 종료 여부 확인 → 이동 → 접촉 시 수집 요청 → 화면 갱신`으로 적습니다. 같은 프레임에 시간이 끝나면 종료 판정 이후의 수집 요청은 거절합니다. 스크립트 실행 순서에 따라 경계 프레임의 이벤트 순서는 달라질 수 있으므로 시간 0에서 수집이 거절되는 규칙 자체를 검증합니다.

### 실행해보면

이 클래스는 `UnityEngine`을 사용하지 않으므로 게임 규칙만 따로 확인할 수 있습니다. 먼저 Unity Console에 컴파일 오류가 없는지 확인합니다. 이어지는 실습에서 GameSession과 UI를 연결해 중복 수집·시간 종료·재시작을 실행하며 확인합니다.

## 4. 핵심 개념: 경기 기록과 진행 요원

`CoinRound`가 경기 기록이라면 `GameSession`은 시작과 종료를 안내하는 진행 요원입니다. 코인은 수집을 요청하고, 진행 요원과 기록 클래스가 진행 상태와 중복 여부를 확인합니다.

### 이 단어는 무슨 뜻인가요?

- **진행 상태**: 메뉴, 플레이, 결과 중 지금 어떤 화면과 동작을 사용할지 나타내는 값입니다.
- **Trigger**: 물리적으로 밀치지 않고 Collider가 겹친 사실을 알려주는 설정입니다.
- **의존성**: 한 클래스가 동작하기 위해 필요한 다른 클래스나 참조입니다.

## 5. 실습: 진행 요원과 코인 작성

### UI와 입력 패키지 준비

1. `Window > Package Management > Package Manager`에서 Input System과 Unity UI를 확인합니다. 없으면 Unity Registry에서 설치합니다. 이 과정의 Input System 참조 버전은 1.20.1이며 설치한 정확한 버전을 기록합니다.
2. `Edit > Project Settings > Player > Other Settings > Active Input Handling`을 Input System Package (New)로 바꾸고 요청되면 Editor를 재시작합니다. 실제 이동 액션은 DAY 03에서 만들지만 오늘 UI 버튼을 누르기 위한 입력 모듈부터 준비합니다.
3. `Window > TextMeshPro > Import TMP Essential Resources`로 기본 폰트와 설정을 가져옵니다. 초기 UI는 영문으로 만들고 한글이 필요하면 글리프가 포함된 폰트를 별도로 준비합니다.
4. Assets/Scripts/Runtime 폴더를 만듭니다. 앞에서 작성한 CoinRound는 Assets/Scripts/Core에 그대로 두고 아래 두 파일만 Runtime에 추가합니다.

**미션:** `Assets/Scripts/Runtime/GameSession.cs`와 `CoinPickup.cs`를 각각 작성합니다. 두 파일을 하나로 합치지 않습니다. 현재 씬에는 스테이지가 직접 배치되어 있고, 네트워크나 Addressables는 아직 사용하지 않습니다.

### GameSession.cs

```csharp
using UnityEngine;
using TMPro;

public sealed class GameSession : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CoinPickup[] coins;
    [SerializeField] private AudioSource collectAudio;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text hud;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private UnityEngine.UI.Button startButton;

    private AudioClip generatedCollectClip;
    private CoinRound round;
    private Vector3 startPosition;
    private float roundSeconds = 30f;
    private bool playing;
    private bool showingResult;
    private bool busy;
    private bool paused;

    public int Score => round.Score;
    public bool IsPlaying => playing;
    public bool CanMove => playing && !paused;
    public bool IsLoading => busy;
    // &&는 왼쪽부터 확인하므로 coins가 null이면 Length에 접근하지 않습니다.
    public bool IsReady => coins != null && coins.Length > 0 && !busy;

    private void Awake()
    {
        round = new CoinRound(roundSeconds);
        startPosition = player.position;

        // 외부 AudioClip이 연결되어 있지 않을 때만 짧은 수집음을 직접 만듭니다.
        if (collectAudio != null && collectAudio.clip == null)
        {
            // 초당 22,050개 표본 중 2,205개를 사용하므로 약 0.1초 길이입니다.
            float[] samples = new float[2205];
            for (int i = 0; i < samples.Length; i++)
            {
                // i / 22050f는 현재 표본의 시간(초), 880f는 1초에 진동하는 횟수입니다.
                // Mathf.PI는 원주율(π)입니다. Sin(2π × 진동수 × 시간)에 남은 길이 비율을 곱해 소리를 서서히 줄입니다.
                // (float)는 정수 나눗셈을 피하기 위한 변환이며, 0.25f는 파형의 크기를 줄이는 계수입니다.
                samples[i] = Mathf.Sin(2f * Mathf.PI * 880f * i / 22050f) * (1f - (float)i / samples.Length) * 0.25f;
            }

            // 표본 수·모노 1채널·초당 표본 수를 지정한 뒤, SetData로 처음 위치 0부터 파형을 채웁니다.
            generatedCollectClip = AudioClip.Create("CollectTone", samples.Length, 1, 22050, false);
            generatedCollectClip.SetData(samples, 0);

            collectAudio.clip = generatedCollectClip;
        }
    }

    private void OnDestroy()
    {
        if (generatedCollectClip != null)
        {
            // 직접 생성한 클립만 정리합니다. Inspector에 연결한 외부 음원은 제거하지 않습니다.
            Destroy(generatedCollectClip);
        }
    }

    private void Start()
    {
        SetStage(coins);
    }

    private void Update()
    {
        if (playing && !paused)
        {
            // 위->아래: 이번 프레임의 시간을 반영한 뒤 종료 여부를 판단합니다.
            round.Tick(Time.deltaTime);

            if (round.IsFinished)
            {
                FinishRound();
            }
        }

        RefreshUi();
    }

    public void BeginRound()
    {
        if (!IsReady || playing)
        {
            return;
        }

        round.Reset(roundSeconds);
        player.position = startPosition;

        foreach (CoinPickup coin in coins)
        {
            coin.gameObject.SetActive(true);
        }

        playing = true;
        showingResult = false;
        paused = false;

        RefreshUi();
    }

    public bool TryCollect(int id)
    {
        // 진행 중이 아니면 수집을 요청하지 않고, 진행 중이어도 최종 승인 여부는 CoinRound가 판단합니다.
        if (!CanMove || !round.TryCollect(id))
        {
            return false;
        }

        if (collectAudio != null)
        {
            collectAudio.Play();
        }

        return true;
    }

    public void ReturnToMenu()
    {
        playing = false;
        showingResult = false;
        paused = false;

        RefreshUi();
    }

    public void SetStage(CoinPickup[] stageCoins)
    {
        // null은 아직 스테이지가 없다는 뜻입니다. 빈 배열로 바꾸어 아래 foreach를 안전하게 건너뜁니다.
        coins = stageCoins ?? new CoinPickup[0];
        foreach (CoinPickup coin in coins)
        {
            coin.Initialize(this, player);
        }

        RefreshUi();
    }

    public void SetBusy(bool value)
    {
        busy = value;

        RefreshUi();
    }

    public void SetStatus(string message)
    {
        statusText.text = message;
    }

    public void SetRoundSeconds(int seconds)
    {
        if (playing)
        {
            return;
        }

        // 다음 라운드의 제한 시간을 허용 범위인 10~120초 안으로 맞춥니다.
        roundSeconds = Mathf.Clamp(seconds, 10, 120);
    }

    private void FinishRound()
    {
        playing = false;
        showingResult = true;
    }

    private void RefreshUi()
    {
        menuPanel.SetActive(!playing && !showingResult);
        hudPanel.SetActive(playing);
        resultPanel.SetActive(showingResult);
        startButton.interactable = IsReady;
        // 0.2초가 남았을 때 0으로 보이지 않도록 올림해서 1초로 표시합니다.
        hud.text = $"Score: {Score}  Time: {Mathf.CeilToInt(round.Remaining)}";
        resultText.text = $"Score: {Score}";
    }
}
```

### CoinPickup.cs

```csharp
using UnityEngine;

public sealed class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinId;

    private GameSession session;
    private Transform player;

    public void Initialize(GameSession owner, Transform target)
    {
        session = owner;
        player = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아직 초기화되지 않았거나, 접촉한 Collider가 지정한 Player의 것이 아니면 무시합니다.
        if (session == null || other.transform != player)
        {
            return;
        }

        // 게임 규칙에서 수집을 승인한 경우에만 코인을 숨깁니다.
        if (session.TryCollect(coinId))
        {
            gameObject.SetActive(false);
        }
    }
}
```

`SetStage`, `SetBusy`, `SetRoundSeconds`는 같은 진행 요원이 사용할 스테이지·준비 상태·제한 시간을 전달받는 창구입니다. 지금은 씬의 코인 배열과 기본 30초를 사용하며, DAY 06·13에서 연결할 기능을 새 게임 코드로 복제하지 않도록 준비합니다.

## 6. 씬과 UI 연결

[DAY 01](DAY01_REQUIREMENTS_ENVIRONMENT.md)에서 만든 바닥과 카메라가 있는 PlatformMain을 엽니다. 앞의 두 스크립트가 컴파일되었는지 확인합니다. Hierarchy의 Create Empty로 GameSession과 StageMain을 만들고, GameSession 오브젝트에 GameSession 컴포넌트를 붙입니다. Player와 코인은 아래 배치 표를 따라 생성합니다.

Canvas는 UI 메뉴에서 만들고 Render Mode를 Screen Space - Overlay로 둡니다. Canvas Scaler는 Scale With Screen Size, Reference Resolution `(1600,900)`, Match 0.5로 설정합니다. Canvas 아래 UI Panel을 만들어 SafeAreaRoot로 이름을 바꾸고 Image 컴포넌트를 제거합니다. RectTransform은 전체 Stretch·offset 0으로 둡니다. 패널은 UI Panel, 텍스트는 TextMeshPro, 버튼은 UI Button으로 직접 생성해 다음 이름으로 바꿉니다. 버튼에 기본 글자가 있다면 그 자식 텍스트를 수정하고, 없다면 자식 TMP 텍스트를 만듭니다. 패널 Image는 짙은 남색, 버튼 Image는 청록색, 텍스트는 흰색으로 지정해 읽기 쉽게 만듭니다. 텍스트와 장식 패널 Image의 Raycast Target은 끄고 버튼 배경 Image는 켭니다. GameSession과 Player는 화면 패널 밖에 둡니다.

```text
PlatformMain
  Ground / Main Camera / Player / GameSession
  StageMain
    Coin01 ... Coin10
  Canvas
    SafeAreaRoot
      MenuPanel: StartButton / StatusText
      HudPanel: HudText
      ResultPanel: ResultText / RestartButton / MenuButton
  EventSystem
```

1. 3D Object > Cylinder로 코인을 만들고 Scale을 `(0.8,0.12,0.8)`로 둡니다. CoinPickup을 붙이고 기본 CapsuleCollider의 Is Trigger를 켭니다. 복제한 Coin01~Coin10의 Coin Id를 1~10으로 다르게 지정합니다.
2. GameSession의 Coins 배열에 열 개를 연결합니다. Player·Collect Audio와 각 패널·텍스트·Start Button 참조도 연결합니다.
3. StartButton과 RestartButton의 On Click에는 `GameSession.BeginRound()`, MenuButton에는 `ReturnToMenu()`를 연결합니다.
4. 첫 실행에서는 메뉴만 보이고 Start가 활성화되는지 확인합니다. Inspector에서 Player를 옮겨 코인 접촉을 시험합니다. 입력 이동은 다음 수업에 연결합니다.
5. Player 오브젝트에는 CapsuleCollider와 Kinematic Rigidbody가 함께 있어야 합니다. 자식 Collider는 사용하지 않습니다.

| 대상 | 설정·필드 | 확인할 결과 |
| :--- | :--- | :--- |
| StageMain | 코인만 자식으로 배치, 루트 위치·회전 0, Scale 1 | 이후 프리팹으로 옮겨도 같은 코인 배치 |
| StartButton | BeginRound | 버튼 조작 후에만 시간 감소 |
| RestartButton | BeginRound | 점수·시간·위치·코인 복원 |
| MenuButton | ReturnToMenu | 결과 화면에서 메뉴로 복귀 |
| Collect Audio | AudioSource, Play On Awake 끄기 | 수집 승인 시에만 효과음 |

Inspector의 Hud·Result Text·Status Text 필드에는 각각 HudText·ResultText·StatusText의 TMP_Text를 연결합니다. 필드 이름과 Hierarchy의 오브젝트 이름을 구분합니다. Title·Hint·ResultTitle 같은 장식 텍스트는 선택 사항입니다. GameSession 오브젝트에 AudioSource를 추가하고 Play On Awake를 끄며 Spatial Blend는 0, Volume은 0.4로 둡니다. 이 AudioSource를 Collect Audio 필드에 연결합니다. AudioClip을 비워 두면 앞 코드가 짧은 수집음을 생성하므로 별도 효과음 파일이 필요하지 않습니다.

3D Object > Capsule로 Player를 만들고 Position을 `(0,0.5,-7)`, Scale을 `(0.8,0.5,0.8)`로 둡니다. 코인은 X를 -6·-3·0·3·6, Z를 -3·3으로 두 줄 배치합니다. 코인 이름과 Coin Id가 일대일로 대응하는지 확인합니다.

Assets/Materials에서 Material 두 개를 만들어 PlayerMaterial·CoinMaterial로 이름을 정합니다. Shader는 Universal Render Pipeline/Lit로 두고 Base Map 색을 각각 초록색·금색으로 지정합니다. PlayerMaterial을 Player에, CoinMaterial을 코인 10개에 연결해 바닥과 구분합니다. 완성된 머티리얼 파일을 가져올 필요는 없습니다.

메뉴·결과 버튼은 크기 `(340,54)`, Anchor·Pivot 중앙으로 시작합니다. MenuPanel 안의 StartButton 위치는 `(0,80)`입니다. ResultPanel의 RestartButton·MenuButton은 `(0,-35)`, `(0,-105)`로 둡니다. 아래 표대로 패널·텍스트·버튼을 배치합니다. 별도 지시가 없으면 Anchor Min·Max와 Pivot은 중앙 `(0.5,0.5)`이고, 위치는 부모 기준 Anchored Position입니다.

| 오브젝트 | 부모 | 크기 / 위치 |
| :--- | :--- | :--- |
| MenuPanel | SafeAreaRoot | 480×600 / (0,0) |
| HudPanel | SafeAreaRoot | 520×70 / (0,370) |
| ResultPanel | SafeAreaRoot | 480×360 / (0,0) |
| StatusText | MenuPanel | 440×80 / (0,-190) |
| HudText | HudPanel | 500×60 / (0,0) |
| ResultText | ResultPanel | 440×60 / (0,45) |

버튼의 자식 텍스트는 START·RESTART·MENU처럼 역할을 알 수 있게 입력합니다. TMP 글자 크기는 24~28로 시작하고 영역 안에 들어오는지 확인합니다. GameSession의 Coins 배열은 Size 10으로 만들고 Coin01~Coin10의 CoinPickup을 하나씩 넣습니다. 참조를 모두 연결한 뒤 Play합니다.

방향 버튼·최고 점수·저장·종료·스테이지 재로딩·설정 읽기 UI는 해당 기능을 배우는 날에 추가합니다. 오늘은 기본 메뉴·HUD·결과 화면만 만듭니다.

EventSystem은 하나만 두고 InputSystemUIInputModule에 기본 UI Actions를 할당합니다. 초기 표시에서는 MenuPanel만 활성화하고 HudPanel·ResultPanel은 비활성화합니다. 저장 기능 연결은 DAY 04에 진행합니다.

참고: [AudioClip.Create](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/AudioClip.Create.html), [AudioClip.SetData](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/AudioClip.SetData.html).

## 7. 경계 조건 확인

코인 두 개를 같은 ID로 바꿔 한 번만 점수가 오르는지 확인한 후 고유 ID로 복원합니다. 시간 종료 후에는 이동·수집이 멈춰야 합니다. 재시작하면 열 개 모두 다시 수집할 수 있어야 합니다. 현재는 HUD를 매 프레임 갱신하는 단순한 구현이며 DAY 14에서 이 게임 코드를 직접 개선합니다.

## 프로젝트 완료 확인

- [ ] CoinRound가 점수·시간·수집 ID를 관리하고, HashSet 선택 이유를 설명합니다.
- [ ] 처음에는 메뉴가 보이고 Start 이후에만 시간이 흐릅니다.
- [ ] 결과·재시작·메뉴 복귀가 동작하고 동일 ID는 두 번 수집되지 않습니다.

오늘의 프로젝트를 `Progress/Day02`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. 수집한 ID를 List로 보관하면 중복 확인은 어떤 절차가 필요한가요?
2. 게임 규칙 클래스가 키보드 종류를 몰라도 되는 이유는 무엇인가요?
3. 종료 여부를 코인 코드와 게임 진행 기록을 관리하는 코드 중 어디에서 최종 판단해야 하나요?
4. 재시작 시 수집 ID를 비우지 않으면 어떤 현상이 생기나요?

## 오늘의 정리

게임 진행 기록을 설계하고 한 판의 시작과 결과를 GameSession에 연결했습니다. 다음 수업은 이 프로젝트의 Player에 입력을 연결합니다.
