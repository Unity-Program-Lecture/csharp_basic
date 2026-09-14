# DAY 10: 소유권, NetworkVariable, RPC

오늘은 "누가 조작하고, 누가 결정하며, 무엇을 모두에게 보여 줄지"를 구분합니다.

## 1. 핵심 개념: "내 리모컨, 공동 점수판, 심판 호출"

- **소유권 (Ownership)**: NetworkObject를 조작 요청할 수 있는 참여자 관계입니다. `IsOwner`는 현재 실행 중인 Client가 이 오브젝트의 소유자인지 알려 줍니다.
- **NetworkVariable**: 서버가 바꾼 값을 연결된 참여자에게 동기화하는 네트워크 변수입니다.
- **RPC (Remote Procedure Call)**: 다른 네트워크 참여자에게 함수 실행을 요청하는 방식입니다.
- **Server Authority**: 최종 게임 결과는 Server가 변경하는 원칙입니다.

## 2. 안내형 실습: 서버가 점수를 올리는 버튼

**미션:** Player 소유자가 점수 증가를 요청하고 Server가 점수를 올립니다.

1. Player Prefab에 `NetworkScorePlayer.cs`를 추가합니다.
2. 아래 코드를 `Assets/Scripts/NetworkScorePlayer.cs`에 입력합니다.

```csharp
using Unity.Netcode;
using UnityEngine;

public class NetworkScorePlayer : NetworkBehaviour
{
    // NetworkVariable은 Server가 바꾼 값을 연결된 모든 참여자에게 동기화합니다.
    public NetworkVariable<int> Score = new(0);

    // Button On Click()에서 호출하는 Client의 점수 증가 요청입니다.
    public void RequestAddScore()
    {
        // IsOwner는 현재 실행 중인 Client가 이 Player를 소유하는지 확인합니다.
        if (!IsOwner)
            return;

        // RPC는 이 요청을 Server에서 실행하도록 보냅니다.
        RequestAddScoreRpc();
    }

    // Rpc(SendTo.Server)는 이 메서드 본문을 Server에서 실행하게 합니다.
    [Rpc(SendTo.Server)]
    private void RequestAddScoreRpc()
    {
        // 공유 점수는 Server Authority 원칙에 따라 Server에서만 변경합니다.
        Score.Value += 1;
        Debug.Log($"Server가 점수를 {Score.Value}로 변경했습니다.");
    }
}
```

3. Player Prefab의 자식으로 Canvas와 Button을 만들고 Button 글자를 `점수 요청`으로 바꿉니다.
4. Button의 On Click()에 Player의 `NetworkScorePlayer.RequestAddScore`를 연결합니다.
5. Host와 Client를 연결합니다.
6. Host가 소유한 Player Button을 누르고, 이어서 Client가 소유한 Player Button을 누릅니다.
7. 각 실행 창의 Console에서 Server 로그를 확인합니다.

### 완료 확인

- [ ] 소유자가 아닌 Player의 `RequestAddScore`는 요청하지 않는다.
- [ ] 어느 Client의 요청이든 Server에서만 `Score.Value`가 증가한다.
- [ ] 새 Client가 들어와도 현재 Score 값을 받는다.

## 응용 실습: 점수 표시

`Score.OnValueChanged`를 사용해 각 Player 위의 TMP Text에 현재 점수를 표시하세요. 점수 표시가 Server 값에 따라 양쪽 창에서 함께 바뀌는지 확인하세요.

## 오늘의 정리

- 소유권은 입력 요청 권한을, Server Authority는 결과 변경 권한을 구분합니다.
- RPC는 요청에, NetworkVariable은 공유 상태에 사용합니다.
