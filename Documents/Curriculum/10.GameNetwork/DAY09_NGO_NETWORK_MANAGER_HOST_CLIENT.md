# DAY 09: NGO NetworkManager, Host, Client

오늘부터 콘솔에서 확인한 역할을 Unity 6 GameObject에 적용합니다. NGO 2.13.3을 사용해 Host와 Client를 실제 연결하고, CharacterController 플레이어의 이동이 양쪽 창에 반영되는지 확인합니다.

실습 전에 [NGO 기본 구조와 컨셉](Supplement/NGO_ARCHITECTURE_AND_CONCEPTS.md)을 읽습니다. NGO 2.13.3의 일반 NetworkTransform 권한 설정과 각 창의 복제본·소유권을 구분하는 보충 자료입니다.

설정 칸의 의미는 [NGO 핵심 컴포넌트 Inspector 설정 안내](Supplement/NGO_COMPONENT_INSPECTOR_GUIDE.md)를 함께 참고합니다. 주요 항목의 역할·실습 값·변경 시 영향을 설명합니다.

교사가 완성된 구성과 결과를 먼저 확인하려면 [Unity 6.6.5 NGO 검증 프로젝트](../../../Code/GameNetwork/NGO/README.md)를 사용합니다. 실제 접속·이동·충돌 검증 기록이 포함되어 있습니다. 이 프로젝트는 DAY09의 기준 구성이며 이후 차시의 기능은 별도로 확장합니다.

## 1. 핵심 개념: "경기장 관리실과 참가자"

- **NetworkManager**: NGO의 연결, 참여자, 오브젝트 생성을 관리하는 중심 컴포넌트입니다.
- **Host**: 서버 역할과 로컬 클라이언트 역할을 함께 수행하는 참여자입니다.
- **Client**: Host 또는 Server에 연결해 게임에 참여하는 프로그램입니다.
- **NetworkObject**: NGO가 네트워크에서 식별하고 생성·제거할 수 있는 GameObject입니다.
- **NGO (Netcode for GameObjects)**: GameObject의 생성과 상태를 네트워크로 관리하는 Unity 패키지입니다.
- **IP (Internet Protocol) 주소**: 연결할 컴퓨터를 가리키는 주소입니다. `127.0.0.1`은 다른 컴퓨터가 아니라 자기 컴퓨터를 가리키는 루프백(Loopback) 주소입니다.
- **포트 (Port)**: 같은 컴퓨터에서 통신 프로그램을 구분하는 번호입니다. 이번 실습은 `7777`을 사용합니다.
- **Unity Transport**: 실제 데이터 전송과 접속 주소·포트를 담당하는 컴포넌트입니다. 다른 Host를 자동으로 찾아주는 기능은 아닙니다.

## 2. 안내형 실습: Host와 Client 시작 버튼

**미션:** 한 버튼은 Host를 시작하고, 다른 버튼은 Client를 시작합니다. 각 창에서 자신의 플레이어를 움직여 상대 창에서도 같은 이동을 확인합니다.

1. DAY00의 `NetworkPlay` 씬을 엽니다.
2. `NetworkManager`를 선택합니다. NetworkManager 컴포넌트의 **Network Transport** 칸에 같은 오브젝트의 `Unity Transport` 컴포넌트를 연결합니다. Unity Transport의 접속 설정에서 **Address = `127.0.0.1`**, **Port = `7777`** 을 확인합니다. Server Listen Address가 표시되면 `127.0.0.1`로 설정합니다. 버전에 따라 표시 이름이 다르므로 아래 코드도 시작 전에 주소와 포트를 명시적으로 지정합니다.
3. Canvas를 만들고 자식으로 UI의 Button - TextMeshPro 두 개와 Text - TextMeshPro 하나를 생성합니다. 이름을 `StartHostButton`, `StartClientButton`, `StatusText`로 바꾸고, 버튼 글자는 `Host 시작`, `Client 시작`으로 설정합니다. 서로 겹치지 않도록 위아래로 배치합니다. TMP (TextMesh Pro, 텍스트 표시 패키지) 리소스 가져오기 창이 나타나면 **Import TMP Essentials** 를 실행합니다. 버튼 입력을 받을 `EventSystem`도 Hierarchy에 있어야 합니다.
4. `Assets/Scripts` 폴더에 `NetworkLauncher.cs`를 만들고 아래 코드를 모두 입력합니다.

```csharp
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkLauncher : MonoBehaviour
{
    // SerializeField는 private 필드를 Inspector에서 연결할 수 있게 합니다.
    [SerializeField] private TextMeshProUGUI statusText;

    // 로컬 실습에서는 두 실행 창 모두 같은 컴퓨터의 같은 포트를 사용합니다.
    private const string HostAddress = "127.0.0.1";
    private const ushort HostPort = 7777;

    // Button On Click()에서 호출하는 Host 시작 메서드입니다.
    public void StartHost()
    {
        // NetworkManager.Singleton은 씬의 유일한 NetworkManager에 접근합니다.
        NetworkManager manager = NetworkManager.Singleton;
        // IsListening은 이미 서버 또는 클라이언트가 실행 중인지 확인합니다.
        if (manager.IsListening)
        {
            return;
        }
        // GetComponent는 같은 오브젝트의 UnityTransport 컴포넌트를 가져옵니다.
        UnityTransport transport = manager.GetComponent<UnityTransport>();
        // SetConnectionData(접속 주소, 포트, 서버 수신 주소)로 직접 연결 설정을 지정합니다.
        // 세 번째 인자도 루프백으로 지정하므로 다른 컴퓨터의 접속은 받지 않습니다.
        transport.SetConnectionData(HostAddress, HostPort, HostAddress);
        // StartHost는 서버와 로컬 Client를 함께 시작하고 성공 여부를 반환합니다.
        bool started = manager.StartHost();
        statusText.text = started ? "Host 시작" : "Host 시작 실패";
    }

    // Button On Click()에서 호출하는 Client 연결 시작 메서드입니다.
    public void StartClient()
    {
        NetworkManager manager = NetworkManager.Singleton;
        if (manager.IsListening)
        {
            return;
        }
        UnityTransport transport = manager.GetComponent<UnityTransport>();
        // 클라이언트의 목적지는 같은 컴퓨터의 127.0.0.1:7777입니다.
        transport.SetConnectionData(HostAddress, HostPort);
        // StartClient는 이미 시작된 Host 또는 Server로 연결을 시도합니다.
        // true는 시작 성공이지 접속 완료가 아닙니다. 완료 여부는 Update에서 확인합니다.
        bool started = manager.StartClient();
        statusText.text = started ? "Client 연결 시도" : "Client 시작 실패";
    }

    // Update는 매 프레임 실행됩니다. 버튼을 누른 뒤 실제 연결 상태를 표시합니다.
    private void Update()
    {
        NetworkManager manager = NetworkManager.Singleton;
        if (manager == null)
        {
            return;
        }
        if (manager.IsHost)
        {
            // ConnectedClientsIds는 서버에 연결된 참여자 ID(Identifier, 식별자) 목록입니다.
            // Host 자신의 로컬 Client도 포함되므로 별도 Client가 붙으면 2명이 됩니다.
            statusText.text = $"Host 실행 / 참여자 {manager.ConnectedClientsIds.Count}명";
        }
        else if (manager.IsConnectedClient)
        {
            // IsConnectedClient는 연결 승인과 초기 동기화까지 완료됐는지 확인합니다.
            statusText.text = "Client 연결 완료";
        }
    }
}
```

5. Hierarchy에서 빈 오브젝트 `NetworkLauncher`를 만들고 스크립트를 추가합니다.
6. `StatusText`를 `statusText` 칸에 끌어 놓습니다.
7. `StartHostButton`의 Button 컴포넌트 **On Click()** 에 `NetworkLauncher` 오브젝트를 추가하고 `NetworkLauncher > StartHost()`를 선택합니다.
8. `StartClientButton`도 같은 방식으로 `StartClient()`를 연결합니다.
9. Player Prefab에 `NetworkObject`가 있는지, NetworkManager의 Player Prefab 칸이 연결됐는지 확인합니다. NetworkManager의 **Network Transport** 연결도 다시 확인하고 씬을 저장합니다.

### 플레이어 프리팹과 이동 동기화 만들기

**미션:** Cube 대신 CharacterController를 가진 캡슐 캐릭터를 만들고, 내 창의 입력으로 이동한 결과가 상대 창에도 보이게 합니다. CharacterController는 이동 기능이며 사람 모양의 모델을 제공하는 컴포넌트는 아닙니다. 이번 실습은 외부 모델·애니메이션 없이 캡슐을 캐릭터의 외형으로 사용합니다.

#### 먼저 바닥·카메라·입력 준비

1. Package Manager에서 **Input System** 패키지를 설치합니다. `Edit > Project Settings > Player > Other Settings > Active Input Handling`을 **Input System Package (New)** 또는 **Both** 로 설정합니다. 재시작 안내가 나오면 씬을 저장하고 Editor를 재시작합니다.
2. EventSystem을 선택합니다. 기존 `StandaloneInputModule`이 있으면 제거하고 `InputSystemUIInputModule`을 추가합니다. 기본 UI (User Interface, 사용자 인터페이스) 입력 액션이 비어 있으면 Inspector의 **Assign Default Actions** 를 누릅니다. 시작 버튼 클릭이 동작해야 합니다.
3. `GameObject > 3D Object > Plane`을 만들고 이름을 `Ground`로 바꿉니다. Position은 `(0, 0, 0)`, Scale은 `(2, 1, 2)`로 설정합니다. Collider는 유지합니다.
4. Main Camera의 Position은 `(0, 10, -10)`, Rotation은 `(45, 0, 0)`으로 설정합니다. Projection은 **Orthographic**, Size는 **8** 로 설정합니다. 위에서 비스듬히 두 플레이어를 함께 보는 고정 카메라입니다.
5. Cube 하나를 장애물 `Wall`로 만들고 Position을 `(0, 1, 4)`, Scale을 `(6, 2, 1)`로 설정합니다. BoxCollider는 유지합니다. 이 Cube는 플레이어가 아니라 벽입니다.

#### 프리팹 생성·등록

1. Hierarchy에서 빈 오브젝트 `PlayerNetworkPrefab`을 만듭니다. Position과 Rotation은 `(0, 0, 0)`, Scale은 `(1, 1, 1)`로 설정합니다.
2. 자식으로 `3D Object > Capsule`을 만들고 이름을 `Body`로 바꿉니다. **Local Position** 은 `(0, 1, 0)`, Local Rotation은 `(0, 0, 0)`, Local Scale은 `(1, 1, 1)`입니다.
3. Body의 **CapsuleCollider는 제거** 합니다. 충돌은 부모의 CharacterController 하나로 처리합니다. Rigidbody도 추가하지 않습니다.
4. 부모 `PlayerNetworkPrefab`에 `NetworkObject`와 `CharacterController`를 추가합니다. NetworkObject는 부모에 하나만 두고 Body에는 추가하지 않습니다.
5. 아래 이동 스크립트를 `Assets/Scripts/NetworkCharacterPlayer.cs`에 입력하고 컴파일 오류가 없는지 확인합니다.
6. 부모에 일반 `NetworkTransform`과 `NetworkCharacterPlayer`를 추가합니다. NGO 2.13.3에서는 별도 OwnerNetworkTransform 스크립트 없이 Inspector로 권한을 선택합니다. 이전 실습의 OwnerNetworkTransform이 붙어 있다면 제거하고 일반 NetworkTransform 하나만 사용합니다.
7. 아래 Inspector 설정표대로 컴포넌트를 설정합니다.
8. `Assets/Prefabs` 폴더에 부모를 끌어 넣어 `PlayerNetworkPrefab.prefab`으로 저장합니다. DAY00의 Cube 프리팹이 같은 이름으로 남아 있다면 기존 Cube 프리팹은 이름을 바꿔 보관하고 새 프리팹을 만듭니다. 참조가 새 프리팹을 가리키는지 다시 확인합니다.
9. 씬에 남은 원본 Player 오브젝트와 이전 플레이어 Cube는 제거합니다. **프리팹 에셋은 유지** 하고 네트워크 접속 시 생성되는 복제본만 사용합니다.
10. NetworkManager의 **Player Prefab** 칸에 새 캡슐 프리팹을 연결합니다. Network Prefabs 목록이 노출되어 있으면 새 프리팹이 포함되는지도 확인합니다. 같은 프리팹을 중복 등록하지 않습니다. Enable Scene Management는 켜 둡니다.
11. Player Prefab은 참여자별로 NGO가 자동 생성합니다. 씬에 Player를 배치하거나 직접 Instantiate할 필요가 없습니다. 씬을 저장하고 **실행 파일을 다시 빌드** 합니다.

#### Inspector 설정표

| 컴포넌트·항목 | 값 | 실행 결과 |
| --- | --- | --- |
| CharacterController / Height | 2 | 캡슐 충돌 높이 |
| CharacterController / Radius | 0.5 | 캡슐 충돌 반지름 |
| CharacterController / Center | (0, 1, 0) | 루트 위치가 발밑을 가리킴 |
| CharacterController / Skin Width | 0.08 | 충돌 경계에서 걸림을 완화 |
| CharacterController / Step Offset | 0.3 | 낮은 턱을 오를 수 있는 높이 |
| CharacterController / Min Move Distance | 0 | 작은 이동도 처리 |
| NetworkCharacterPlayer / Move Speed | 4 | 초당 이동 거리 |
| NetworkCharacterPlayer / Gravity | -20 | 아래로 가속하는 값 |
| NetworkTransform / Authority Mode | Owner | 소유자가 이동 결과를 결정·전송 |
| NetworkTransform / Position X·Y·Z | 켬 | 위치 세 축을 전달 |
| NetworkTransform / Rotation Y | 켬 | 바라보는 방향을 전달 |
| NetworkTransform / Rotation X·Z | 끔 | 이번 실습에서 사용하지 않는 회전 |
| NetworkTransform / Scale X·Y·Z | 끔 | 크기는 모든 창에서 고정 |
| NetworkTransform / Interpolate | 켬 | 상대 캐릭터의 이동을 부드럽게 표시 |

**Authority Mode는 반드시 Owner로 설정합니다.** 기본값인 Server를 그대로 사용하면 별도 Client가 직접 움직인 결과는 서버의 위치 결정으로 인정되지 않습니다. 위치·회전 축과 보간 항목도 위 표대로 설정합니다. 보간 (Interpolation)은 받은 상태 사이를 이어 상대 이동을 부드럽게 표시하는 처리입니다.

#### NetworkTransform의 권한 설정

**소유권 (Ownership)** 은 어떤 참여자에게 이 오브젝트가 배정되었는지를 나타냅니다. **권한 (Authority)** 은 동기화할 상태를 누가 결정하는지를 나타냅니다. 이번 실습은 내 리모컨으로 내 캐릭터를 움직이는 **소유자 권한** 방식입니다.

일반 NetworkTransform의 Authority Mode를 Owner로 선택하면 됩니다. 소유자만 입력을 읽도록 다음 스크립트에서 IsOwner도 확인합니다. 상대 복제본에도 같은 NetworkTransform이 필요하지만, 상대 입력을 실행하지는 않습니다. Owner 모드도 현재 Host를 통한 연결 구조를 바꾸지 않습니다.

#### NetworkCharacterPlayer.cs

NetworkBehaviour는 MonoBehaviour에 네트워크 생성·소유권 정보를 더한 기반 클래스입니다. 코드를 **위 → 아래** 로 읽고, Update에서는 **입력 → 속도 계산 → Move → 회전** 을 확인합니다. InputAction은 코드에서 만들기 때문에 별도의 Input Actions 에셋이나 PlayerInput 컴포넌트는 필요 없습니다.

<details>
<summary>코드 보기</summary>

```csharp
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class NetworkCharacterPlayer : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private InputAction moveAction;
    private float verticalSpeed;

    private void Awake()
    {
        // CharacterController는 벽·바닥 충돌을 고려하여 캐릭터를 이동시킵니다.
        controller = GetComponent<CharacterController>();
        // 생성 전과 원격 복제본에서는 물리 이동을 하지 않습니다.
        controller.enabled = false;
    }

    // OnNetworkSpawn은 이 오브젝트가 네트워크에 생성될 때 호출됩니다.
    public override void OnNetworkSpawn()
    {
        // IsOwner는 현재 실행 창이 이 Player를 소유하는지 확인합니다.
        if (!IsOwner)
        {
            return;
        }

        // OwnerClientId는 이 Player의 소유자 번호입니다.
        // Host와 별도 Client가 겹치지 않도록 두 사람의 시작 위치를 나눕니다.
        bool isHostPlayer = OwnerClientId == Unity.Netcode.NetworkManager.ServerClientId;
        transform.position = new Vector3(isHostPlayer ? -2f : 2f, 0f, 0f);
        controller.enabled = true;

        // InputAction은 입력을 게임 동작으로 묶습니다. 별도 입력 에셋은 필요 없습니다.
        moveAction = new InputAction("Move", InputActionType.Value);
        // 2DVector는 네 방향 키를 Vector2 값으로 합치는 복합 입력입니다.
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
        // Enable은 이 액션이 입력을 읽기 시작하도록 합니다.
        moveAction.Enable();
    }

    private void Update()
    {
        // IsSpawned는 네트워크 생성 완료 여부입니다. 내 캐릭터만 입력을 처리합니다.
        if (!IsSpawned || !IsOwner || moveAction == null)
        {
            return;
        }

        // ReadValue<Vector2>는 좌우(x), 앞뒤(y) 입력 값을 읽습니다.
        Vector2 input = moveAction.ReadValue<Vector2>();
        // ClampMagnitude는 대각선 입력도 길이 1 이하로 제한합니다.
        Vector3 direction = Vector3.ClampMagnitude(new Vector3(input.x, 0f, input.y), 1f);

        // isGrounded는 직전 Move에서 바닥에 닿았는지 알려 줍니다.
        if (controller.isGrounded && verticalSpeed < 0f)
        {
            verticalSpeed = -2f;
        }
        // Move는 중력을 자동 적용하지 않으므로 수직 속도를 직접 계산합니다.
        verticalSpeed += gravity * Time.deltaTime;
        Vector3 velocity = direction * moveSpeed;
        velocity.y = verticalSpeed;
        // Move에는 속도가 아니라 이번 프레임의 이동 거리를 전달합니다.
        controller.Move(velocity * Time.deltaTime);

        if (direction.sqrMagnitude > 0f)
        {
            // LookRotation은 이동 방향을 바라보는 회전을 만듭니다.
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // OnNetworkDespawn은 네트워크에서 제거될 때 호출됩니다.
    public override void OnNetworkDespawn()
    {
        if (moveAction != null)
        {
            moveAction.Disable();
            // Dispose는 액션이 가진 자원을 정리합니다.
            moveAction.Dispose();
            moveAction = null;
        }
        controller.enabled = false;
    }
}
```

</details>

CharacterController는 **내 플레이어에서만 활성화** 합니다. 상대 복제본은 입력·중력 계산을 하지 않고 NetworkTransform이 전달한 위치를 표시합니다. 이번 실습은 바닥·벽 충돌만 확인하며 플레이어끼리의 충돌, 점프, 애니메이션은 다루지 않습니다.

Host의 캐릭터는 `(-2, 0, 0)`, 별도 Client의 캐릭터는 `(2, 0, 0)`에서 시작합니다. 정확히 두 참여자를 위한 배치이므로 세 번째 Client를 연결하면 오른쪽 위치가 겹칠 수 있습니다.


### 두 실행 창에서 실제 연결하기

기본 경로는 **Unity Editor 한 창 + 빌드한 실행 파일 한 창** 입니다. 같은 실행 창에서 두 버튼을 모두 누르는 실습이 아닙니다.

1. Editor의 Play Mode를 끈 상태에서 씬을 저장합니다.
2. Unity 6의 `File > Build Profiles`를 열고 Windows 플랫폼을 선택합니다. Windows 빌드 지원이 없으면 Unity Hub에서 해당 Editor의 Windows Build Support 모듈을 설치합니다.
3. **Scene List** 에서 **Add Open Scenes** 로 `NetworkPlay`를 추가합니다. 해당 씬을 포함하도록 체크하고, 첫 실행 씬이 되도록 목록 맨 위에 놓습니다. 기존 실습과 무관한 씬은 이번 빌드에서 제외합니다.
4. **Build** 를 눌러 프로젝트 안의 `Builds/NetworkLab` 같은 별도 폴더에 빌드합니다. 생성된 실행 파일을 엽니다.
5. Editor에서 Play Mode를 시작하고 **Host 시작** 을 누릅니다. 상태가 `Host 실행 / 참여자 1명`인지 확인합니다.
6. 빌드한 실행 파일 창에서 **Client 시작** 을 누릅니다. 같은 창에서는 Host 버튼을 누르지 않습니다.
7. Client 창이 `Client 연결 완료`, Editor가 `Host 실행 / 참여자 2명`으로 바뀌는지 확인합니다. Editor Hierarchy에는 Player 복제본이 두 개 있어야 합니다.
8. 확인 후 실행 파일을 닫고 Editor의 Play Mode를 종료합니다. 다시 실습할 때도 Host부터 시작합니다.

Multiplayer Play Mode를 이미 설정했다면 두 플레이어 실행 창을 사용하는 것도 가능합니다. 그 경우에도 한 창만 Host를 시작하고 다른 창만 Client를 시작합니다. 처음 실습은 위의 빌드 경로로 진행하면 가상 플레이어 설정에 의존하지 않습니다.

### 움직여서 동기화 결과 확인하기

1. 연결 후 양쪽 창에 왼쪽 Host 캐릭터와 오른쪽 Client 캐릭터가 보이는지 확인합니다.
2. Editor의 **Game 뷰를 클릭** 해 입력 초점을 주고 W/A/S/D 또는 방향키를 누릅니다. 왼쪽 Host 캐릭터만 움직이며 실행 파일 창에서도 같은 캐릭터의 이동이 보여야 합니다.
3. 실행 파일 창을 클릭하고 같은 키를 누릅니다. 이번에는 오른쪽 Client 캐릭터만 움직이며 Editor에서도 그 이동이 보여야 합니다.
4. W 또는 위쪽 방향키로 벽 쪽으로 이동해 캐릭터가 벽을 통과하지 않는지 확인합니다. 두 창에서 같은 위치에 멈추는지 봅니다.
5. 한 창만 조작할 때 두 캐릭터가 동시에 움직이면 IsOwner 검사를 확인합니다. 실행 파일이 이전 코드라면 새로 빌드합니다.
6. 카메라 밖이나 바닥 끝까지 이동하지 말고 가운데 영역에서 시험합니다. 씬을 처음 상태로 다시 확인하려면 두 실행을 종료하고 Host부터 시작합니다.

**연결만 되는 상태와 이동이 동기화되는 상태는 다릅니다.** NetworkObject는 네트워크 식별·생성을, CharacterController는 이동·충돌을, Owner로 설정한 NetworkTransform은 위치·회전 전송을 담당합니다.

이 예제는 소유자의 위치를 신뢰하는 입문용 구성입니다. 서버가 이동 속도·충돌을 검증하는 보안 구조나 물리 시뮬레이션 전체를 동기화하는 구성은 아닙니다. 서버 권한 입력·검증은 DAY11에서 별도로 학습합니다.

### 다른 컴퓨터 접속과는 구분하기

위 코드는 같은 컴퓨터 전용입니다. 다른 컴퓨터와 통신하려면 클라이언트의 목적지를 Host 컴퓨터의 IP 주소로 바꾸고, Host의 수신 주소도 외부 접속을 받도록 바꿔야 합니다. 방화벽의 UDP (User Datagram Protocol, 데이터그램 전송 프로토콜) 포트 허용도 필요합니다. 인터넷 접속에서는 공유기와 Relay 설정 등 추가 조건이 있으므로 주소만 바꾸면 무조건 연결되는 것은 아닙니다.

### 완료 확인

- [ ] Host 창 Hierarchy에 Host와 Client의 Player 복제본이 모두 생긴다.
- [ ] Host 표시가 `참여자 2명`, Client 표시가 `Client 연결 완료`이다.
- [ ] 양쪽 실행 창의 주소·포트가 `127.0.0.1:7777`로 일치한다.
- [ ] Editor에서 왼쪽 캐릭터를 움직이면 Client 창에서도 움직인다.
- [ ] Client 창에서 오른쪽 캐릭터를 움직이면 Editor에서도 움직인다.
- [ ] 자신의 캐릭터만 조작되고 바닥·벽 충돌이 적용된다.
- [ ] Console에 연결 Error가 없다.

### 흔한 오류

| 증상 | 원인 후보 | 해결 |
| :--- | :--- | :--- |
| 버튼이 아무 반응이 없다 | On Click 연결 또는 EventSystem 누락 | Button의 연결과 EventSystem을 확인합니다. |
| Player가 생성되지 않는다 | Player Prefab 또는 NetworkObject 누락 | NetworkManager의 Player Prefab과 Prefab의 NetworkObject를 확인합니다. |
| Client가 연결되지 않는다 | Host 미실행, 목적지 주소·포트 불일치 | Editor에서 Host를 먼저 시작하고, 양쪽이 `127.0.0.1:7777`인지 확인합니다. 코드 변경 후 실행 파일도 다시 빌드합니다. |
| Host 시작 실패 또는 포트 사용 오류 | 다른 실행 창이나 이전 프로세스가 7777을 사용 중 | 이전 실습 실행 파일을 닫고 Play Mode를 종료한 뒤 Host 한 개만 시작합니다. |
| NullReferenceException이 발생한다 | NetworkManager, Unity Transport 또는 statusText 연결 누락 | 씬의 컴포넌트와 Inspector 참조를 다시 확인합니다. |
| 빌드한 창에 버튼이 없다 | 저장 전 빌드 또는 다른 씬으로 시작 | 씬을 저장하고 Scene List의 첫 씬을 확인한 뒤 다시 빌드합니다. |
| Client 캐릭터 이동이 되돌아간다 | NetworkTransform의 Authority Mode가 Server | 일반 NetworkTransform 하나만 두고 Authority Mode를 Owner로 설정합니다. 프리팹·씬을 저장하고 실행 파일도 다시 빌드합니다. |
| 한 창에서 두 캐릭터가 움직인다 | 소유권 검사 누락 | Update의 IsOwner 검사와 Player Prefab 자동 생성을 확인합니다. |
| 입력이 안 되거나 UI 버튼이 안 눌린다 | 창 초점 또는 Input System 설정 누락 | Game 뷰·실행 창을 클릭하고 Active Input Handling 및 EventSystem의 InputSystemUIInputModule을 확인합니다. |
| 캐릭터가 바닥을 통과한다 | 바닥 Collider 또는 컨트롤러 설정 누락 | Ground의 Collider, CharacterController의 Center·Height를 확인합니다. |

## 응용 실습: 연결 상태 표시

- Host 종료 후 Client에 `연결 끊김`을 표시합니다.
- 자신의 캐릭터와 상대 캐릭터의 색을 구분합니다.
- 장애물을 추가하고 두 창에서 이동·충돌 결과를 비교합니다.

## 오늘의 정리

- NetworkManager가 연결을 시작하고 NetworkObject가 참여자별 게임 오브젝트를 만듭니다.
- StartClient는 Unity Transport의 목적지 주소·포트로 접속합니다. 시작 성공과 실제 연결 완료는 다릅니다.

### 생각해보기

1. CharacterController만 있고 NetworkTransform이 없으면 상대 창에서는 무엇이 보일까요?
2. IsOwner 검사를 제거하면 두 실행 창의 입력이 어떻게 충돌할까요?
3. 소유자의 이동 결과를 그대로 신뢰하면 서버 검증 방식과 어떤 차이가 있나요?

## 참고 문서

- [Unity: NetworkManager와 접속 주소·포트 설정](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/manual/components/core/networkmanager.html)
- [Unity: UnityTransport.SetConnectionData](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/api/Unity.Netcode.Transports.UTP.UnityTransport.html)
- [Unity 6: 빌드에 포함할 씬 관리](https://docs.unity3d.com/kr/6000.0/Manual/build-profile-scene-list.html)
- [Unity: NetworkTransform의 Authority Mode](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/api/Unity.Netcode.Components.NetworkTransform.html)
- [Unity: CharacterController.Move와 중력 처리](https://docs.unity3d.com/kr/2022.3/ScriptReference/CharacterController.Move.html)
