# DAY 06: Addressables로 스테이지 준비하기

오늘은 DAY 05까지 완성한 게임의 StageMain을 주소로 불러오는 프리팹으로 바꿉니다. 로딩 중에는 시작할 수 없고, 실패하면 다시 로드할 수 있어야 합니다. NCS G2·G7·G8을 다룹니다.

## 1. 핵심 개념: 경기장을 준비한 뒤 문 열기

경기장이 준비되기 전에 경기를 시작하지 않듯, 코인 스테이지가 로드된 뒤 Start 버튼을 활성화합니다. Addressables의 핸들은 요청 상태와 결과를 확인하고 사용한 참조를 반납하는 수령증과 같습니다.

### 이 단어는 무슨 뜻인가요?

- **Address**: 자산을 요청하는 이름표입니다. 이번 주소는 `stage-main`입니다.
- **AsyncOperationHandle**: 비동기 요청의 상태·결과·참조를 관리하는 값입니다.
- **소유자**: 요청한 자산을 언제 해제할지 책임지는 코드입니다. 이 게임에서는 StageLoader입니다.
- **ReleaseInstance**: Addressables로 생성한 인스턴스와 관련 참조를 반납하는 함수입니다.

## 2. 실습: 기존 스테이지를 프리팹으로 이동

먼저 `Window > Package Management > Package Manager`의 Unity Registry에서 Addressables를 설치합니다. 이 과정은 2.11.2와 AssetBundle 빌드 방식을 기준으로 하며 설치한 버전을 기록합니다. Assets/Prefabs 폴더가 없다면 만듭니다. 설정을 생성하면 Assets/AddressableAssetsData가 만들어지며 이 폴더와 meta도 프로젝트에 보관합니다.

**미션:** 같은 게임의 StageMain을 Addressable로 등록합니다.

1. StageMain 아래에는 CoinPickup이 붙은 코인 열 개만 둡니다. Player·GameSession·UI·바닥은 넣지 않습니다. 루트 Position/Rotation은 0, Scale은 1입니다.
2. `Assets/Prefabs/StageMain.prefab`으로 저장합니다. 프리팹의 Coin Id 1~10과 Trigger 설정을 확인합니다.
3. GameSession의 Coins 배열을 비우고 씬의 StageMain을 삭제합니다. 기존 스테이지와 로드한 스테이지가 두 벌 생기지 않게 합니다.
4. `Window > Asset Management > Addressables > Groups`를 열고 처음이라면 Create Addressables Settings를 누릅니다. 기본 Local 그룹 이름을 `CourseStage`로 바꿉니다. Project의 StageMain.prefab을 이 그룹에 끌어 넣고 Address를 `stage-main`으로 지정합니다. 그룹 Inspector에서 Build Path는 LocalBuildPath, Load Path는 LocalLoadPath로 둡니다. 그룹 이름이 Local이어야 로컬 로드가 되는 것은 아닙니다.
5. 오늘은 Play Mode Script를 Use Asset Database로 설정합니다. 실제 번들 기반 로딩은 다음 수업에서 확인합니다.

## 3. 실습: StageLoader 작성

**미션:** `Assets/Scripts/Runtime/StageLoader.cs`를 작성하고 새 StageLoader 오브젝트에 붙입니다. Session에는 기존 GameSession을 연결합니다.

```csharp
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public sealed class StageLoader : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private string address = "stage-main";

    private AsyncOperationHandle<GameObject> handle;
    private bool ownerDestroyed;

    private void Start()
    {
        LoadStage();
    }

    public void LoadStage()
    {
        if (session.IsPlaying || session.IsLoading)
        {
            return;
        }

        // 요청을 시작하기 전에 Start와 중복 로드를 잠그고 이전 코인 참조를 비웁니다.
        session.SetBusy(true);
        session.SetStatus("Loading stage...");
        session.SetStage(null);

        // 이전 스테이지와 요청 참조를 반납한 뒤 새 핸들을 저장합니다.
        ReleaseOwnedOperation();

        // 프리팹을 원점·기본 회전으로 생성 요청하고, 완료 처리는 아래 콜백에 맡깁니다.
        handle = Addressables.InstantiateAsync(address, Vector3.zero, Quaternion.identity);
        // 성공과 실패 모두 완료 콜백에서 확인합니다.
        handle.Completed += OnCompleted;
    }

    private void OnCompleted(AsyncOperationHandle<GameObject> operation)
    {
        // 요청한 오브젝트가 먼저 파괴되었다면, 도착한 결과는 씬에 연결하지 않고 해제합니다.
        if (ownerDestroyed)
        {
            ReleaseOwnedOperation();
            return;
        }

        if (operation.Status != AsyncOperationStatus.Succeeded)
        {
            session.SetBusy(false);
            session.SetStatus("Stage load failed. Press Reload Stage.");
            Debug.LogError(operation.OperationException);
            ReleaseOwnedOperation();
            return;
        }

        // true를 주어 비활성 코인까지 포함합니다. 로드 성공 후에만 Result에 접근합니다.
        CoinPickup[] coins = operation.Result.GetComponentsInChildren<CoinPickup>(true);
        if (coins.Length == 0)
        {
            ReleaseOwnedOperation();
            session.SetBusy(false);
            session.SetStatus("Stage has no coins. Check the prefab.");
            return;
        }

        session.SetStage(coins);
        session.SetBusy(false);
        session.SetStatus("Stage ready. Press Start.");
    }

    private void OnDestroy()
    {
        ownerDestroyed = true;

        // 진행 중인 요청은 여기서 취소되지 않습니다. 완료된 요청만 지금 정리하고,
        // 아직 진행 중이면 위 OnCompleted의 ownerDestroyed 분기에서 나중에 정리합니다.
        if (handle.IsValid() && handle.IsDone)
        {
            ReleaseOwnedOperation();
        }
    }

    private void ReleaseOwnedOperation()
    {
        if (!handle.IsValid())
        {
            return;
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // 성공한 InstantiateAsync 요청은 생성된 인스턴스와 참조를 함께 해제합니다.
            Addressables.ReleaseInstance(handle);
        }
        else
        {
            // 실패한 요청은 인스턴스가 없지만, 요청 핸들이 가진 참조는 반납해야 합니다.
            Addressables.Release(handle);
        }

        // 정리한 핸들을 비워 이후 호출에서 다시 해제하지 않게 합니다.
        handle = default;
    }
}
```

Canvas/SafeAreaRoot/MenuPanel 아래 UI Button으로 ReloadStageButton을 만듭니다. 자식 텍스트는 `RELOAD STAGE`, 크기는 `(340,54)`, Anchor·Pivot은 중앙, 위치는 `(0,10)`으로 둡니다. 버튼 배경의 Raycast Target은 켜고 자식 텍스트는 끕니다. On Click에 `StageLoader.LoadStage()`를 연결합니다. 성공하면 Status가 준비 완료로 바뀌고 Start를 누를 수 있습니다. 실패하면 Start는 계속 잠기고 Reload Stage로 다시 시도합니다. Inspector의 주소를 수정하는 테스트는 Play 종료 후 바꾸고 다시 실행합니다.

GameSession은 그대로 사용합니다. `SetStage`가 새 코인마다 현재 Session과 Player를 연결하므로 프리팹 안에 씬 오브젝트 참조를 저장하지 않습니다. `SetBusy`는 로딩 중 Start를 잠그며, BeginRound는 준비 상태를 다시 검사합니다.

## 4. 실습: 실패와 재시도

- 주소를 잘못 설정해 실패를 재현한 뒤 원복합니다. 실패 시 Start가 잠기고 로그와 안내가 남아야 합니다.
- 메뉴에서 Reload Stage를 두 번 눌러 코인이 스무 개로 늘지 않는지 확인합니다.
- 플레이 중 직접 호출해도 로드를 다시 시작하지 않는지 확인합니다.
- 로드 중 StageLoader를 파괴하는 실험은 복사본에서만 진행합니다. 요청 자체는 취소되지 않으므로 완료 후 해제해야 합니다.
- 비활성화와 파괴는 다릅니다. 이 예제의 정리는 OnDestroy에서 수행합니다.

`LoadAssetAsync`의 자산 참조에는 `Addressables.Release(handle)`, `InstantiateAsync`로 만든 인스턴스에는 `ReleaseInstance`를 사용합니다. 일반 Instantiate/Destroy와 섞지 않습니다.

## 프로젝트 완료 확인

- [ ] 로딩 중 Start가 잠기고 성공 후에만 한 판을 시작합니다.
- [ ] 실패·재로드에서 코인 중복과 참조 누수가 없는지 관찰했습니다.

오늘의 프로젝트를 `Progress/Day06`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. 스테이지 로딩이 끝나기 전에 Start 버튼을 누르지 못하게 하는 이유는 무엇인가요?
2. InstantiateAsync로 만든 스테이지를 Destroy로만 제거하면 어떤 문제가 생길 수 있나요?

## 오늘의 정리

장식 예제 대신 게임에 필요한 코인 스테이지를 비동기로 준비했습니다. 다음 수업에서는 같은 주소를 실제 로컬 콘텐츠 빌드에서 읽습니다.
