# DAY 12: UniTask 연결 UI와 비동기 수명주기

오늘은 접속 준비 중 버튼을 잠그고, 취소·실패·성공을 구분해 화면에 표시합니다.

## 1. 핵심 개념: "닫힌 창구에서 계속 접수하지 않기"

- **UniTask**: Unity PlayerLoop에 맞춘 `async`/`await` 비동기 라이브러리입니다.
- **PlayerLoop**: Unity가 매 프레임 Update, FixedUpdate 등을 실행하는 순서입니다.
- **수명주기 (Lifecycle)**: GameObject가 생성되고, 활성화되고, 파괴되는 과정입니다.
- **`CancellationTokenSource` (CTS)**: 비동기 작업 취소 신호를 만드는 객체입니다.
- **`UniTaskCompletionSource`**: 콜백 기반 완료 신호를 여러 곳에서 기다릴 수 있는 UniTask 형태로 바꾸는 도구입니다.

## 2. 안내형 실습: Host 시작 중 버튼 잠그기

**미션:** 시작 버튼을 연속 클릭할 수 없게 하고, 성공·실패 상태를 표시합니다.

1. DAY09 Canvas에 `ConnectButton`과 `StatusText`가 있는지 확인합니다.
2. `ConnectUiController.cs`를 만들고 아래 코드를 입력합니다.

```csharp
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectUiController : MonoBehaviour
{
    // SerializeField 필드는 Inspector에서 Button과 TMP Text를 연결합니다.
    [SerializeField] private Button connectButton;
    [SerializeField] private TextMeshProUGUI statusText;

    public void StartHostFromButton()
    {
        // Forget은 Button 이벤트에서 기다리지 않는 UniTask의 예외를 UniTask 쪽으로 전달합니다.
        StartHostAsync().Forget();
    }

    // UniTask는 Unity PlayerLoop에 맞춘 비동기 반환 형식입니다.
    private async UniTask StartHostAsync()
    {
        connectButton.interactable = false;
        statusText.text = "Host 시작 중...";

        // StartHost는 NetworkManager의 Host 시작 성공 여부를 반환합니다.
        bool started = NetworkManager.Singleton.StartHost();
        if (!started)
        {
            statusText.text = "Host 시작 실패";
            connectButton.interactable = true;
            return;
        }

        // Yield는 다음 PlayerLoop 시점까지 기다립니다.
        // GetCancellationTokenOnDestroy는 이 GameObject가 파괴되면 대기를 취소합니다.
        await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
        statusText.text = "Host 시작 완료";
        connectButton.interactable = true;
    }
}
```

3. 빈 `ConnectUiController` 오브젝트에 스크립트를 추가합니다.
4. Button과 TMP Text를 Inspector 필드에 끌어 놓습니다.
5. Button On Click()을 `StartHostFromButton`으로 연결합니다.
6. Play Mode에서 버튼을 빠르게 여러 번 눌러 봅니다.

### 완료 확인

- [ ] 시작 중에는 버튼이 비활성화된다.
- [ ] 성공 또는 실패 상태가 Text에 남는다.
- [ ] 오브젝트가 파괴되면 진행 중인 대기가 취소될 수 있다.

> 같은 `UniTask` 인스턴스를 두 번 `await`하지 않습니다. 여러 소비자가 같은 완료 신호를 기다려야 하면 `UniTaskCompletionSource` 같은 다중 대기 가능한 구조를 사용합니다.

## 응용 실습: 재시도 버튼

시작 실패 시 `다시 시도` 버튼을 표시하고, 이미 시작된 Host에는 다시 시작 요청을 보내지 않도록 만드세요.

## 오늘의 정리

- 비동기 작업은 성공만 처리하지 않고 취소·실패·수명주기까지 함께 설계합니다.
