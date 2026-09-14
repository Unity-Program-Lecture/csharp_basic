# DAY 09: NGO NetworkManager, Host, Client

오늘부터 콘솔에서 확인한 역할을 Unity 6 GameObject에 적용합니다.

## 1. 핵심 개념: "경기장 관리실과 참가자"

- **NetworkManager**: NGO의 연결, 참여자, 오브젝트 생성을 관리하는 중심 컴포넌트입니다.
- **Host**: 서버 역할과 로컬 클라이언트 역할을 함께 수행하는 참여자입니다.
- **Client**: Host 또는 Server에 연결해 게임에 참여하는 프로그램입니다.
- **NetworkObject**: NGO가 네트워크에서 식별하고 생성·제거할 수 있는 GameObject입니다.

## 2. 안내형 실습: Host와 Client 시작 버튼

**미션:** 한 버튼은 Host를 시작하고, 다른 버튼은 Client를 시작합니다.

1. DAY00의 `NetworkPlay` 씬을 엽니다.
2. Canvas를 만들고 자식으로 `StartHostButton`, `StartClientButton`, `StatusText`를 만듭니다.
3. `Assets/Scripts` 폴더에 `NetworkLauncher.cs`를 만들고 아래 코드를 모두 입력합니다.

```csharp
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkLauncher : MonoBehaviour
{
    // SerializeField는 private 필드를 Inspector에서 연결할 수 있게 합니다.
    [SerializeField] private TextMeshProUGUI statusText;

    // Button On Click()에서 호출하는 Host 시작 메서드입니다.
    public void StartHost()
    {
        // NetworkManager.Singleton은 씬의 유일한 NetworkManager에 접근합니다.
        // StartHost는 서버와 로컬 Client를 함께 시작하고 성공 여부를 반환합니다.
        bool started = NetworkManager.Singleton.StartHost();
        statusText.text = started ? "Host 시작" : "Host 시작 실패";
    }

    // Button On Click()에서 호출하는 Client 연결 시작 메서드입니다.
    public void StartClient()
    {
        // StartClient는 이미 시작된 Host 또는 Server로 연결을 시도합니다.
        bool started = NetworkManager.Singleton.StartClient();
        statusText.text = started ? "Client 연결 시도" : "Client 시작 실패";
    }
}
```

4. Hierarchy에서 빈 오브젝트 `NetworkLauncher`를 만들고 스크립트를 추가합니다.
5. `StatusText`를 `statusText` 칸에 끌어 놓습니다.
6. `StartHostButton`의 Button 컴포넌트 **On Click()**에서 `NetworkLauncher > NetworkLauncher.StartHost`를 연결합니다.
7. `StartClientButton`도 같은 방식으로 `StartClient`를 연결합니다.
8. Player Prefab에 `NetworkObject`가 있는지, NetworkManager의 Player Prefab 칸이 연결됐는지 확인합니다.
9. Multiplayer Play Mode 또는 별도 실행 창에서 한 창은 Host, 다른 창은 Client를 시작합니다.

### 완료 확인

- [ ] Host 창 Hierarchy에 Host와 Client의 Player 복제본이 모두 생긴다.
- [ ] Client 창에도 두 Player가 보인다.
- [ ] Console에 연결 Error가 없다.

### 흔한 오류

| 증상 | 원인 후보 | 해결 |
| :--- | :--- | :--- |
| 버튼이 아무 반응이 없다 | On Click 연결 또는 EventSystem 누락 | Button의 연결과 EventSystem을 확인합니다. |
| Player가 생성되지 않는다 | Player Prefab 또는 NetworkObject 누락 | NetworkManager의 Player Prefab과 Prefab의 NetworkObject를 확인합니다. |
| Client가 연결되지 않는다 | Host를 먼저 시작하지 않음, Transport 설정 불일치 | Host 시작 뒤 Client를 누르고 Unity Transport를 확인합니다. |

## 응용 실습: 연결 상태 표시

`NetworkManager.Singleton.IsHost`, `IsClient`, `IsConnectedClient` 중 필요한 값을 사용해 `Host`, `Client`, `연결 안 됨` 상태를 Text로 표시하세요.

## 오늘의 정리

- NetworkManager가 연결을 시작하고 NetworkObject가 참여자별 게임 오브젝트를 만듭니다.
