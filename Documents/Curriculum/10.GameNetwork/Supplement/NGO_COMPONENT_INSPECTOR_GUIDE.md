# NGO 핵심 컴포넌트 Inspector 설정 안내

이 문서는 **NGO (Netcode for GameObjects) 2.13.3** 과 Unity 6을 기준으로, 핵심 구성 요소의 Editor 설정을 설명하는 보충 자료입니다. [NGO 기본 구조와 컨셉](NGO_ARCHITECTURE_AND_CONCEPTS.md)으로 역할을 이해한 뒤, [DAY09 실습](../DAY09_NGO_NETWORK_MANAGER_HOST_CLIENT.md)과 함께 사용합니다. 새로운 수업 일차를 추가하지 않습니다.

설명 기준은 Host 한 개와 별도 Client가 같은 컴퓨터에서 접속하는 클라이언트–서버 구성입니다. **Host** 는 서버와 로컬 클라이언트 역할을 함께 맡고, **Client** 는 서버에 접속해 참가합니다. **Server** 는 접속과 게임 진행을 관리하는 서버 역할입니다.

**Inspector (인스펙터)** 는 선택한 오브젝트·에셋의 컴포넌트와 설정값을 확인하고 수정하는 Editor 창입니다. 각 표는 **항목의 역할 → 실습 설정 → 변경 시 영향** 순서로 읽습니다. 전체 옵션을 외우는 것보다 연결·생성·이동 중 어느 단계에 영향을 주는지 구분하는 것이 목표입니다.

## 1. 먼저 어떤 오브젝트를 선택할지 확인하기

**Hierarchy** 는 현재 씬의 오브젝트 목록이고, **Project** 는 프로젝트의 에셋 목록입니다. **에셋 (Asset)** 은 프리팹·스크립트·재질 등 프로젝트에 저장하는 자료입니다. **프리팹 (Prefab)** 은 같은 구성의 객체를 만들기 위한 설계도 에셋입니다.

| 구성 요소 | 확인할 위치 | 핵심 책임 |
| --- | --- | --- |
| NetworkManager | Hierarchy의 NetworkManager 오브젝트 | 연결·참여자·객체 생성 관리 |
| Unity Transport | 같은 NetworkManager 오브젝트 | 주소·포트와 실제 데이터 전송 |
| NetworkObject | Project에서 연 PlayerNetworkPrefab의 루트 | 네트워크 식별·생성·소유권 관리 |
| NetworkTransform | 같은 플레이어 프리팹 루트 | 위치·회전·크기 동기화 |
| NetworkBehaviour 기반 스크립트 | 같은 프리팹의 NetworkCharacterPlayer | 개발자가 작성한 입력·이동·네트워크 게임 로직 |

**NetworkBehaviour 자체를 직접 추가하는 것은 아닙니다.** 이를 상속한 스크립트의 Inspector를 확인하며, 구체적인 설명은 6절에서 다룹니다. NetworkTransform도 NetworkBehaviour를 상속한 컴포넌트입니다.

**Play Mode (플레이 모드)** 는 Editor에서 게임을 실행하는 상태입니다. 설정을 저장할 때는 먼저 Play Mode를 종료합니다. 씬의 NetworkManager와 NetworkLauncher는 Hierarchy에서 선택하고, 플레이어는 Project 창의 `PlayerNetworkPrefab.prefab`을 열어 설정합니다. 실행 중 생긴 복제본만 수정하면 다음 실행이나 다른 프로그램에 같은 설정이 적용되지 않습니다.

**설정을 바꾼 뒤에는 씬·프리팹을 저장하고 실행 파일도 다시 빌드합니다.** 컴포넌트의 접힌 설정 묶음을 펼쳐 확인합니다. 표시 이름·배치는 Editor와 패키지 버전에 따라 조금 다를 수 있습니다.

**API (Application Programming Interface)** 는 프로그램이 다른 기능을 사용하는 접점입니다. 아래 공식 API 링크는 설정에 대응하는 필드·메서드의 의미를 확인하는 참고 자료입니다.

## 2. NetworkManager: 연결과 객체 생성의 기준

씬의 `NetworkManager` 오브젝트를 선택합니다. 실제 통신은 Unity Transport, 접속 후 플레이어 생성은 Player Prefab 설정으로 연결합니다. **PlayerObject** 는 특정 Client의 대표 플레이어로 연결한 네트워크 객체입니다.

| 주요 항목 | 역할 | DAY09 설정·확인 | 변경 시 영향 |
| --- | --- | --- | --- |
| Network Transport | NGO가 사용할 전송 컴포넌트 참조 | 같은 오브젝트의 Unity Transport 연결 | 비어 있거나 다른 구현을 가리키면 의도한 통신을 시작할 수 없음 |
| Player Prefab | 자동 PlayerObject 생성에 사용할 프리팹 | 새 캡슐 PlayerNetworkPrefab 연결 | 비어 있으면 이 구성의 자동 플레이어 생성이 이루어지지 않음 |
| Network Prefabs / Network Prefabs Lists | 네트워크 생성에 사용할 프리팹 목록 | 사용하는 목록에 플레이어 프리팹이 포함되는지 확인 | 상대 프로그램에 대응 프리팹이 없으면 생성 정보를 받아도 올바르게 생성할 수 없음 |
| Enable Scene Management | NGO의 씬 관리와 접속 시 씬 동기화 사용 | 켬 | 끄면 현재 실습과 다른 씬·초기 동기화 구성을 직접 준비해야 함 |
| Connection Approval | 개발자가 작성한 접속 승인 검사 사용 | 이번 실습에서는 끔 | 켜려면 승인 콜백도 구현해야 하며, 체크만으로 로그인·인증이 완성되지 않음 |
| Tick Rate | 초당 네트워크 갱신 주기 | 양쪽에서 수업 기준값 유지 | 높이면 갱신 기회가 늘지만 처리·전송 비용도 늘어날 수 있음 |
| Force Same Prefabs | 접속 시 등록 프리팹 구성의 일치 확인 | 기본값 유지하고 양쪽 빌드를 맞춤 | 끄더라도 누락된 프리팹이 자동 제공되는 것은 아님 |
| Protocol Version | 호환 여부를 구분할 게임 통신 버전 | 양쪽에서 동일하게 유지 | 호환되지 않는 빌드를 구분하는 용도이며 NGO 패키지 버전 번호와는 다름 |

**NetworkPrefabsList** 는 네트워크 프리팹 목록을 담는 에셋입니다. Inspector가 목록 에셋의 참조로 표시되면 그 에셋도 선택해 내용을 확인합니다. Player Prefab이 자동 포함되는 구성에서는 같은 프리팹을 다시 중복 등록하지 않습니다.

**틱 (Tick)** 은 네트워크 갱신의 한 주기입니다. Tick Rate는 화면의 프레임 수나 이동 속도 설정이 아닙니다. **Connection Approval (연결 승인)** 은 서버가 참가 요청을 검사해 접속 허용 여부를 결정하는 절차입니다. **콜백 (Callback)** 은 특정 사건이 발생했을 때 등록한 처리를 호출하는 방식입니다. Connection Approval을 켜면 접속 승인 콜백에서 허용 여부 등을 결정하도록 구성합니다.

[공식 NetworkManager Inspector 설명](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/manual/components/core/networkmanager.html#networkmanager-inspector-properties)

## 3. Unity Transport: 목적지와 수신 지점

같은 `NetworkManager` 오브젝트의 Unity Transport 컴포넌트를 확인합니다. **IP (Internet Protocol) 주소** 는 네트워크의 통신 대상을 가리키는 주소이고, **포트 (Port)** 는 그 대상 안에서 통신 프로그램의 수신 지점을 구분하는 번호입니다. 127.0.0.1은 자기 컴퓨터를 가리키는 **루프백 (Loopback)** 주소입니다. **접속할 주소와 서버가 기다릴 주소는 목적이 다릅니다.**

| 주요 항목 | 역할 | DAY09 설정·확인 | 변경 시 영향 |
| --- | --- | --- | --- |
| Address | Client가 접속할 서버 주소 | 127.0.0.1 | 다른 컴퓨터에 접속하려면 그 Host의 주소가 필요 |
| Port | 목적지 서버의 수신 포트 | 7777 | Host와 Client가 다른 목적지 포트를 사용하면 연결되지 않음 |
| Server Listen Address | 서버가 접속을 받을 로컬 수신 주소 | 127.0.0.1 | 루프백 수신이면 다른 컴퓨터의 접속은 받지 않음 |
| Use WebSockets | WebSocket 전송 사용 여부 | 끔 | 지원 환경에 맞춰 전송 방식이 달라지며 상대 구성도 호환되어야 함 |
| Use Encryption | 전송 암호화 사용 여부 | 기본값 유지 — 이번 실습에서는 별도 보안 연결을 구성하지 않음 | 켜는 것만으로 로그인·게임 요청 검증까지 제공되는 것은 아니며 필요한 보안 설정도 갖춰야 함 |

기본 전송인 **UDP (User Datagram Protocol)** 는 개별 데이터그램 단위로 보내는 프로토콜이며 자체적으로 도착·순서를 보장하지 않습니다. **WebSocket** 은 **TCP (Transmission Control Protocol)** 기반의 양방향 메시지 통신 프로토콜입니다. TCP는 순서대로 바이트를 전달하는 신뢰성 있는 스트림 프로토콜입니다. 자세한 전송 방식과 신뢰성의 관계는 [NGO 기본 구조와 컨셉](NGO_ARCHITECTURE_AND_CONCEPTS.md)의 전송 방식 설명에서 확인합니다.

DAY09의 NetworkLauncher는 시작 전에 `SetConnectionData`를 호출합니다. **실제 시작 시 Address·Port·Server Listen Address는 코드에서 지정한 값이 적용됩니다.** Inspector에서만 주소를 바꾸고 기존 코드를 그대로 두면 그 값으로 접속하지 않을 수 있습니다. `HostAddress`와 `HostPort`는 상수이므로 Inspector 입력 칸이 아니라 소스 코드에서 확인합니다.

[공식 UnityTransport API](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/api/Unity.Netcode.Transports.UTP.UnityTransport.html)

## 4. NetworkObject: 생성과 관찰의 기준

`PlayerNetworkPrefab`의 루트를 선택합니다. NetworkObject는 루트에 하나만 두고 자식 Body에 중복 추가하지 않습니다. **스폰 (Spawn)** 은 NGO에 네트워크 객체로 생성·등록하는 작업입니다. 로컬에서 객체를 만드는 것과 네트워크에 생성하는 것은 구분합니다.

| 주요 항목 | 역할 | DAY09 설정·확인 | 변경 시 영향 |
| --- | --- | --- | --- |
| Synchronize Transform | 네트워크 생성·초기 동기화 때 Transform 전달 | 켬 | 초기 위치 등 전달 여부에 영향을 주며 지속적인 이동 동기화와는 별개 |
| Spawn With Observers | 생성할 때 관찰 대상에게 객체를 공개 | 켬 | 끄면 처음부터 다른 Client에 공개되지 않아 별도의 공개 처리가 필요 |
| Auto Object Parent Sync | 네트워크 부모 관계 변경을 동기화 | 기본값인 켬 유지 | 부모 변경을 사용하는 기능의 전달 방식에 영향을 주며 현재 실습에서는 부모를 변경하지 않음 |

**관찰자 (Observer)** 는 해당 네트워크 객체의 복제본과 갱신을 전달받는 참여자입니다. 카메라에 캐릭터가 보이는지와 NGO에서 객체를 관찰하는지는 다른 개념입니다.

특히 **Synchronize Transform을 켜는 것만으로 이후 이동이 계속 전달되지는 않습니다.** 계속 변하는 위치·회전은 NetworkTransform이 담당합니다.

**ID (Identifier)** 는 식별자입니다. ClientId는 접속 참여자, NetworkObjectId는 네트워크 객체, OwnerClientId는 그 객체의 소유자를 식별하는 번호입니다. 이 번호들은 실행 중 NGO가 정합니다. 프리팹 Inspector에 같은 번호를 직접 입력해 두 창의 객체를 연결하는 방식이 아닙니다. 소유권도 임의의 Inspector 번호가 아니라 생성·소유권 변경 API로 관리합니다.

[공식 NetworkObject API](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/api/Unity.Netcode.NetworkObject.html)

## 5. NetworkTransform: 누가 어떤 이동 값을 보낼 것인가?

같은 프리팹 루트에 일반 NetworkTransform을 하나만 추가합니다. **소유권 (Ownership)** 은 누구에게 객체가 배정되어 있는지 나타내고, **권한 (Authority)** 은 특정 상태를 누가 결정·전송하는지 나타냅니다. Authority Mode의 Server는 서버, Owner는 객체 소유자가 Transform의 상태를 결정·전송하는 방식입니다.

| 주요 항목 | 역할 | DAY09 설정 | 변경 시 영향 |
| --- | --- | --- | --- |
| Authority Mode | Transform 상태를 결정·전송할 주체 | Owner | Server로 바꾸면 기존 Client의 직접 이동 코드와 권한 구성이 맞지 않음 |
| Position X·Y·Z | 위치 동기화 축 | 모두 켬 | 끈 축의 이동은 이 컴포넌트로 지속 동기화하지 않음 |
| Rotation X·Y·Z | 회전 동기화 축 | Y만 켬 | 이번 이동 코드의 좌우 방향 회전만 전달 |
| Scale X·Y·Z | 크기 동기화 축 | 모두 끔 | 이번 실습은 크기를 바꾸지 않으므로 전달하지 않음 |
| Interpolate | 수신한 상태를 보간하여 표시 | 켬 | 상대 이동을 부드럽게 표시하며, 권한 없는 복제본의 표시 처리에 사용 |
| In Local Space | 부모 기준 로컬 좌표 사용 | 끔 | 켜면 월드 좌표가 아닌 부모 기준으로 해석하므로 부모 구성도 고려해야 함 |
| 위치·회전·크기 Threshold | 갱신을 보낼 변경량의 기준 | 기본값 유지 | 너무 크면 작은 변화 전달이 줄고, 너무 작으면 전송 빈도가 늘 수 있음 |

**보간 (Interpolation)** 은 수신한 상태들 사이를 이어 움직임을 부드럽게 표시하는 처리입니다. 네트워크 지연을 없애거나 게임의 충돌 규칙을 계산하는 기능은 아닙니다.

**월드 좌표 (World Space)** 는 씬 전체 기준의 좌표이고, **로컬 좌표 (Local Space)** 는 부모 기준의 좌표입니다. 현재 플레이어 루트는 네트워크 부모 변경 없이 월드 좌표로 이동합니다.

**Threshold (임계값)** 는 갱신을 보낼지 판단하는 변화량의 기준입니다. 위치·크기·회전은 각 기준이 있으며, 회전 기준은 각도입니다. 임계값을 0으로 낮추더라도 입력이나 화면의 모든 프레임마다 반드시 패킷 하나를 보낸다는 뜻은 아닙니다. Tick Rate 등 네트워크 갱신 구조도 함께 작용합니다.

Authority Mode는 플레이어 소유자를 바꾸는 설정이 아니며, 체력·점수 변수의 권한도 자동 변경하지 않습니다. OwnerNetworkTransform과 일반 NetworkTransform을 함께 붙여 해결하려 하지 않습니다.

[공식 NetworkTransform API](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/api/Unity.Netcode.Components.NetworkTransform.html)

## 6. NetworkBehaviour: 직접 만든 네트워크 스크립트의 Inspector

NetworkBehaviour는 네트워크 기능을 사용하는 스크립트의 **추상 기반 클래스** 입니다. 추상 클래스는 그대로 객체를 만들기보다 상속해서 필요한 동작을 구현하는 기반입니다. Add Component에서 NetworkBehaviour 자체를 추가하는 것이 아니라, 이를 상속한 `NetworkCharacterPlayer` 같은 스크립트를 추가합니다.

따라서 모든 NetworkBehaviour에 공통으로 표시되는 Move Speed·Gravity 설정이 있는 것은 아닙니다. **Inspector에 보이는 사용자 설정은 해당 스크립트가 어떤 필드를 선언했는지에 따라 달라집니다.**

### Inspector에 보이는 설정과 코드에서 결정하는 값

**필드 (Field)** 는 클래스 안에 선언한 데이터 저장 공간입니다. **SerializeField** 는 private 필드를 Unity가 저장하고 Inspector에 표시하도록 지정하는 특성입니다. **특성 (Attribute)** 은 코드 요소에 부가 정보를 붙이는 문법입니다.

**직렬화 (Serialization)** 는 데이터를 저장·전송 가능한 표현으로 바꾸는 작업입니다. 여기서는 Unity가 씬·프리팹 설정을 저장하는 직렬화를 말합니다. Inspector에 값이 저장된다는 것과 네트워크로 그 값이 자동 전달된다는 것은 다릅니다.

| 구분 | Inspector와의 관계 | 현재 예제 |
| --- | --- | --- |
| Unity 직렬화를 지원하는 public 필드 | 일반적으로 저장·표시 대상이며 숨김 등 별도 설정의 영향을 받음 | 사용자 스크립트가 공개한 설정 |
| SerializeField가 붙은 지원 형식의 private 필드 | private이어도 설정을 저장하고 표시 | moveSpeed·gravity |
| 일반 프로퍼티·상수·직렬화하지 않은 private 필드 | 기본 Inspector의 저장용 입력 칸이 아님 | IsOwner, HostAddress, moveAction |
| NetworkVariable의 권한·RPC의 호출 대상 | 스크립트의 선언·설정으로 결정 | Inspector의 Authority Mode와 별개 |

**NetworkVariable** 은 읽기·쓰기 권한에 따라 상태를 공유하는 네트워크 변수입니다. **RPC (Remote Procedure Call)** 는 다른 참여자에서 함수 실행을 요청하는 방식입니다. 실행 중 값을 표시하는 전용 Inspector가 있더라도, 현재 예제의 네트워크 권한을 일반 입력 칸으로 설정하는 것은 아닙니다.

### NetworkCharacterPlayer에서 실제로 확인하기

Project에서 PlayerNetworkPrefab을 열고 루트의 NetworkCharacterPlayer를 확인합니다.

| 주요 항목 | 역할 | DAY09 설정 | 변경 시 영향 |
| --- | --- | --- | --- |
| Move Speed | 초당 이동 속도 | 4 | 높이면 소유자의 실제 이동이 빨라짐 |
| Gravity | 아래 방향 가속도 | -20 | 음수 방향으로 수직 속도를 계산하며 이동 코드에서 적용 |

이 두 값은 이동 코드의 설정입니다. 그 결과인 위치·회전을 다른 창에 전달하는 것은 NetworkTransform의 책임입니다. Move Speed를 Inspector에서 바꾸었다고 다른 Client의 프리팹 설정까지 자동 수정되는 것은 아닙니다.

IsOwner·IsServer·IsClient·IsSpawned는 실행 중 역할·소유권·네트워크 생성 상태를 확인하는 값입니다. Inspector에서 IsOwner를 체크해 자기 플레이어로 만드는 방식이 아닙니다. OnNetworkSpawn도 설정 칸이 아니라 NGO가 네트워크 생성 때 호출하는 메서드입니다.

NetworkObject는 같은 객체 또는 부모에 있어야 합니다. 현재 실습에서는 **NetworkObject → NetworkTransform → NetworkCharacterPlayer** 순서로 프리팹 루트에 배치합니다. NetworkObject를 NetworkBehaviour 계열보다 위에 두고, 대응 프로그램에서도 같은 네트워크 스크립트 구성을 사용합니다. 여러 스크립트의 네트워크 초기화 순서도 Inspector의 컴포넌트 배치와 관련이 있으므로 순서를 임의로 다르게 만들지 않습니다.

[공식 NetworkBehaviour 설명](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/manual/components/core/networkbehaviour.html), [Unity 6 SerializeField 설명](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/SerializeField.html)

## 7. 실습 보조 컴포넌트

### CharacterController: 실제로 움직이고 충돌하는 방법

NetworkTransform이 “이동 결과 전달”이라면 CharacterController는 “이동 수행”입니다. 루트의 캡슐 충돌 크기가 자식 Body의 외형과 맞는지 확인합니다.

| 주요 항목 | 역할 | DAY09 설정 | 변경 시 영향 |
| --- | --- | --- | --- |
| Height | 충돌 캡슐의 전체 높이 | 2 | 외형보다 너무 작거나 크면 바닥·벽 접촉이 어색해짐 |
| Radius | 충돌 캡슐의 반지름 | 0.5 | 벽과 유지하는 간격과 좁은 통로 통과 여부에 영향 |
| Center | 루트 기준 충돌 캡슐 중심 | (0, 1, 0) | 높이 2인 캡슐의 발밑이 루트 위치에 맞음 |
| Skin Width | 충돌 경계에서 허용하는 작은 겹침 여유 | 0.08 | 지나치게 작으면 경계에서 떨리거나 걸릴 수 있음 |
| Step Offset | 자동으로 올라갈 수 있는 턱 높이 | 0.3 | 낮은 턱을 넘는 동작에 영향 |
| Slope Limit | 올라갈 수 있는 경사의 각도 제한 | 기본값 유지 | 경사면에서 올라갈 수 있는 범위에 영향 |
| Min Move Distance | 이동 처리를 할 최소 거리 | 0 | 작은 이동을 무시하지 않도록 설정 |

부모 Scale = (1, 1, 1)을 기준으로 설정합니다. **Collider (콜라이더)** 는 충돌 판정용 형태입니다. **Rigidbody** 는 질량·힘 등을 물리 엔진으로 계산하는 컴포넌트입니다. 현재 예제는 CharacterController로 이동하므로 Body의 CapsuleCollider는 제거하고 Rigidbody도 추가하지 않습니다.

Inspector의 컴포넌트 활성 여부와 실행 중 활성 상태도 구분합니다. DAY09 코드는 Awake에서 컨트롤러를 끄고, OnNetworkSpawn에서 소유자의 컨트롤러만 켭니다. 상대 복제본의 컨트롤러가 꺼져 있는 것은 의도한 동작입니다. 중력은 CharacterController의 별도 입력 칸이 아니라 이동 코드에서 계산합니다.

[Unity 6 CharacterController 설정 설명](https://docs.unity3d.com/6000.0/Documentation/Manual/class-CharacterController.html)

### 실습 스크립트와 화면·입력 연결

다음은 NGO 핵심 구성 외에 DAY09의 이동·화면·버튼 입력을 지원하는 컴포넌트 설정입니다. NetworkLauncher는 네트워크 객체의 소유권을 다루는 플레이어 코드가 아니라, 시작 버튼과 연결 상태 표시를 담당하는 실습 스크립트입니다.

| 컴포넌트·항목 | 역할 | DAY09 설정·확인 |
| --- | --- | --- |
| NetworkCharacterPlayer / Move Speed | 이동 속도 | 4 — 값을 높이면 소유자의 실제 이동이 빨라짐 |
| NetworkCharacterPlayer / Gravity | 아래 방향 가속도 | -20 — 음수이며 중력 처리는 코드가 수행 |
| NetworkLauncher / Status Text | 연결 상태를 표시할 텍스트 참조 | 씬의 StatusText에 있는 TextMeshProUGUI 연결 |
| Host 버튼 / Button / On Click() | 버튼 클릭 시 실행할 메서드 | 씬의 NetworkLauncher를 연결하고 StartHost() 선택 |
| Client 버튼 / Button / On Click() | 버튼 클릭 시 실행할 메서드 | 같은 NetworkLauncher의 StartClient() 선택 |
| EventSystem / InputSystemUIInputModule | Input System으로 UI 입력 처리 | 기본 입력 액션이 비면 Assign Default Actions 실행 |
| Main Camera / Projection·Size | 플레이어를 함께 볼 화면 구성 | Orthographic·8 — 카메라는 이동 권한이나 네트워크 관찰자를 결정하지 않음 |
| Body / Renderer | 캡슐 외형 표시 | 활성 상태 유지 — 외형 표시와 네트워크 상태 전달은 별개 |

**UI (User Interface)** 는 버튼·텍스트 등 사용자가 프로그램과 상호작용하는 화면 요소입니다. **TextMeshProUGUI** 는 Canvas에 텍스트를 표시하는 컴포넌트입니다. **InputSystemUIInputModule** 은 EventSystem에서 Input System 입력을 UI 동작에 연결합니다. **Orthographic (직교 투영)** 은 거리에 따른 원근 축소 없이 화면을 보여 주는 카메라 투영 방식입니다.

**InputAction** 은 키·버튼 등의 입력을 이동 같은 게임 동작으로 묶는 Input System의 기능입니다. 이동 입력은 NetworkCharacterPlayer가 코드에서 InputAction으로 구성합니다. 별도 PlayerInput 컴포넌트나 입력 에셋을 연결할 칸은 없습니다. IsOwner도 Inspector에서 체크하는 옵션이 아니라 실행 중 소유권으로 판단하는 값입니다.

NetworkVariable의 읽기·쓰기 권한과 RPC 호출 대상은 이 예제에서 위 컴포넌트의 체크박스로 정하는 것이 아니라 스크립트에서 선언·설정합니다. Inspector의 Authority Mode 하나로 모든 통신 권한을 정한다고 생각하지 않습니다.

## 8. 설정을 확인하는 순서

1. 씬의 NetworkManager에서 Transport 참조와 Player Prefab을 확인합니다.
2. Unity Transport의 주소·포트를 확인하고, 시작 코드가 값을 다시 지정하는지도 확인합니다.
3. Player Prefab의 NetworkObject에서 생성·관찰 관련 설정을 확인합니다.
4. NetworkTransform에서 Owner 권한과 동기화 축을 확인합니다.
5. CharacterController와 NetworkCharacterPlayer에서 충돌 크기·이동 값을 확인합니다.
6. Status Text, 버튼 이벤트, EventSystem의 입력 연결을 확인합니다.
7. 저장·재빌드한 뒤 두 창을 실행해 접속·생성·이동을 각각 확인합니다.

Inspector 설정은 게임 코드를 대신하는 것이 아니라 코드가 사용할 조건과 참조를 제공하는 것입니다. 실제 프로젝트·씬을 만드는 순서는 [DAY09 안내형 실습](../DAY09_NGO_NETWORK_MANAGER_HOST_CLIENT.md)을 따릅니다.

## 생각해보기

1. NetworkObject의 Synchronize Transform과 NetworkTransform은 어떤 차이가 있나요?
2. Inspector에서 주소만 변경해도 현재 NetworkLauncher의 접속 목적지가 바뀌나요?
3. IsOwner가 Move Speed처럼 Inspector에서 입력하는 설정값이 아닌 이유는 무엇인가요?
4. NetworkTransform의 Authority Mode를 Owner로 바꾸면 점수 변수의 쓰기 권한도 바뀌나요?

## 관련 문서

- [NGO 기본 구조와 컨셉](NGO_ARCHITECTURE_AND_CONCEPTS.md)
- [DAY09: Host·Client 연결과 캐릭터 이동](../DAY09_NGO_NETWORK_MANAGER_HOST_CLIENT.md)
- [DAY10: 소유권, NetworkVariable, RPC](../DAY10_NGO_OWNERSHIP_NETWORKVARIABLE_RPC.md)

> 사용 버전의 공식 문서와 수업 스크립트를 바탕으로 정리한 설정 안내입니다. 대상 Unity 프로젝트에서 Inspector 화면과 두 실행 창의 동작을 직접 검증한 결과 보고서는 아닙니다.
