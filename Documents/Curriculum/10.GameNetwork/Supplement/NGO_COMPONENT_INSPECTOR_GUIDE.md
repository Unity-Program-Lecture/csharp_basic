# NGO 핵심 컴포넌트 Inspector 설정 안내

이 문서는 **NGO (Netcode for GameObjects) 2.13.3**, **Unity 6000.6.5f1**, **Unity Transport 패키지 6.6.0** 을 기준으로 기본 Inspector의 설정·조건부 항목·실행 중 상태를 설명합니다. [NGO 기본 구조와 컨셉](NGO_ARCHITECTURE_AND_CONCEPTS.md)을 읽고 [DAY09 실습](../DAY09_NGO_NETWORK_MANAGER_HOST_CLIENT.md)과 함께 사용합니다. 새로운 수업 일차를 추가하지 않습니다.

기준 프로젝트는 [NGO 실습 검증 프로젝트](../../../../Code/GameNetwork/NGO/README.md)입니다. 표의 **기본값** 은 새 컴포넌트의 패키지 초기값, **DAY09 값** 은 수업에서 사용할 값입니다. 이미 저장한 씬·프리팹의 값은 초기값과 다를 수 있습니다. 고급 옵션도 설명하되 이번 실습에 필요하지 않으면 기본값 유지를 안내합니다.

설명 기준은 같은 컴퓨터에서 Host 하나와 별도 Client가 접속하는 클라이언트–서버 구성입니다. **Host** 는 서버와 로컬 클라이언트 역할을 함께 맡고, **Server** 는 서버 역할만, **Client** 는 서버에 접속하는 역할을 맡습니다.

## 1. 선택 위치와 문서 읽는 방법

**Inspector (인스펙터)** 는 선택한 오브젝트·에셋의 설정을 확인하고 수정하는 Editor 창입니다. **Hierarchy** 는 현재 씬의 오브젝트 목록, **Project** 는 저장된 에셋 목록입니다. **에셋 (Asset)** 은 프로젝트에 저장하는 자료이고, **프리팹 (Prefab)** 은 같은 구성의 객체를 만들기 위한 설계도입니다. **루트 (Root)** 는 해당 프리팹 구성의 최상위 객체입니다.

| 구성 요소 | 확인할 위치 | 핵심 책임 |
| --- | --- | --- |
| NetworkManager | 씬의 NetworkManager | 연결·참여자·객체 생성 관리 |
| Unity Transport | 같은 NetworkManager | 주소·포트와 실제 데이터 전송 |
| NetworkObject | PlayerNetworkPrefab 루트 | 네트워크 식별·생성·소유권 관리 |
| NetworkTransform | 같은 플레이어 프리팹 루트 | 위치·회전·크기 동기화 |
| NetworkBehaviour 기반 스크립트 | 같은 프리팹의 NetworkCharacterPlayer | 개발자가 작성한 입력·이동·네트워크 게임 로직 |

**Play Mode (플레이 모드)** 는 Editor에서 게임을 실행하는 상태입니다. 저장할 때는 먼저 Play Mode를 종료합니다. 플레이어는 실행 중 생긴 복제본만 바꾸지 말고 Project의 프리팹을 열어 수정합니다. **씬·프리팹을 저장한 뒤 실행 파일도 다시 빌드합니다.**

**접기/펼치기 (Foldout)** 는 설정 묶음을 열고 닫는 기능입니다. 상위 옵션·패키지·네트워크 모드에 따라 하위 항목이 나타나거나 사라집니다. 회색 칸은 현재 수정할 수 없는 항목입니다. **Debug Inspector** 는 내부 직렬화 필드도 보여 주는 진단용 화면이므로 기본 화면에 없는 내부 값을 임의로 수정하지 않습니다.

**API (Application Programming Interface)** 는 프로그램이 다른 기능을 사용하는 접점입니다. 코드의 필드 이름과 화면 표시 이름이 다르면 필요한 곳에 함께 적었습니다. 다음 절은 실제 전용 Inspector의 묶음·표시 순서로 읽습니다. 일부 전용 NGO Inspector는 스크립트 참조 칸을 의도적으로 숨깁니다.

## 2. NetworkManager: 연결과 객체 생성의 기준

씬의 NetworkManager를 선택합니다. **PlayerObject** 는 특정 Client의 대표 플레이어로 연결된 네트워크 객체입니다. **ID (Identifier)** 는 식별자입니다. ClientId는 참여자, NetworkObjectId는 생성된 객체, OwnerClientId는 그 객체의 소유자 번호입니다.

### 2.1 상단: 실행과 로그

**로그 (Log)** 는 상태·진단 기록입니다. **백그라운드 (Background)** 는 다른 창이 선택되어 게임 창에 입력 초점이 없는 상태입니다.

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향 |
| --- | --- | --- | --- |
| Run In Background | 켬 | 켬 | 창이 선택되지 않아도 게임 실행을 계속하도록 합니다. 두 창 실습에서 한쪽 실행이 멈추는 문제를 줄입니다. 입력 초점까지 자동으로 주는 옵션은 아닙니다. |
| Log Level | Normal | Normal | Developer는 자세한 진단, Normal은 일반 진단, Error는 오류 위주, Nothing은 NGO 로그 억제입니다. Nothing이 Unity의 모든 로그까지 없애지는 않습니다. |

### 2.2 Network Settings: 시작과 메시지 처리

**토폴로지 (Topology)** 는 참여자의 역할과 연결 구조입니다. **소유권 (Ownership)** 은 객체가 누구에게 배정되는지, **권한 (Authority)** 은 상태를 누가 결정·전송하는지를 말합니다. **세션 (Session)** 은 함께 게임에 참여하는 실행 그룹입니다. **분산 권한 (Distributed Authority)** 은 참여자들이 객체별 권한을 분담하는 별도 구성입니다. Owner 권한 NetworkTransform을 쓰는 클라이언트–서버 실습과 같은 뜻이 아닙니다.

**틱 (Tick)** 은 네트워크 갱신의 한 주기입니다. **핸드셰이크 (Handshake)** 는 연결 준비 정보를 교환하는 절차입니다. **콜백 (Callback)** 은 사건이 발생했을 때 등록한 처리를 호출하는 방식입니다. **NetworkVariable** 은 권한에 따라 상태를 공유하는 네트워크 변수입니다.

**RPC (Remote Procedure Call)** 는 다른 참여자에서 함수 실행을 요청하는 방식입니다. **해시 (Hash)** 는 데이터를 계산해 만든 식별값이며 다른 입력이 같은 값이 되는 충돌이 있을 수 있습니다. **프로파일링 (Profiling)** 은 실행 시간·처리량 등을 측정하는 작업, **메트릭 (Metric)** 은 측정 수치입니다. **대역폭 (Bandwidth)** 은 일정 시간에 전송할 수 있는 데이터 양입니다.

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향·표시 조건 |
| --- | --- | --- | --- |
| Network Topology | ClientServer | ClientServer | Client Server 또는 Distributed Authority를 선택합니다. Multiplayer Services SDK (Software Development Kit, 소프트웨어 개발 도구 모음)가 설치된 조건에서 표시됩니다. 항목이 없어도 일반 Host·Client 실습은 가능합니다. 분산 권한에는 별도의 세션·서비스 구성이 필요합니다. |
| Protocol Version | 0 | 0, 양쪽 동일 | 게임 통신 규격의 호환성을 구분합니다. NGO 패키지 버전이나 전송 프로토콜 버전이 아니며, 서로 다른 통신 버전의 빌드를 구분·차단하는 용도입니다. |
| Network Transport | 미연결 | 같은 오브젝트의 Unity Transport | 사용할 전송 구현 참조입니다. 컴포넌트가 존재하는 것과 이 칸에 연결된 것은 별도로 확인합니다. 비어 있으면 경고·전송 구현 선택 화면이 표시될 수 있습니다. |
| Tick Rate | 30 | 30 | 초당 네트워크 갱신 주기입니다. 높이면 갱신 기회와 처리·전송 비용이 늘 수 있습니다. 화면 프레임 수·물리 갱신 주기·이동 속도와는 별개입니다. |
| Spawn Timeout | 10초 | 10초 | 메시지의 대상 NetworkObject가 아직 없을 때 메시지를 보류하는 제한 시간입니다. 객체가 끝내 생성되지 않으면 보류 메시지가 버려질 수 있습니다. 전체 접속 대기 시간이나 의도적인 생성 지연이 아닙니다. |
| Connection Approval | 끔 | 끔 | 서버의 접속 승인 검사입니다. 켜면 ConnectionApprovalCallback을 구현해야 합니다. 체크만으로 로그인·인증이 완성되지 않습니다. 분산 권한에서 같은 서버 승인 방식으로 작동하지 않습니다. |
| Client Connection Buffer Timeout | 10초 | 기본값 유지 | 서버가 접속 승인 핸드셰이크 완료를 기다리는 시간입니다. Connection Approval을 켰을 때 표시되며 초과하면 해당 접속을 종료합니다. Transport 재시도 간격과 구분합니다. |
| NetworkVariable Length Safety | 끔 | 끔 | NetworkVariable 직렬화 길이의 경계를 검사합니다. CPU (Central Processing Unit, 중앙 처리 장치)·전송 비용이 늘 수 있으며 모든 악성 입력·부정행위를 막는 옵션은 아닙니다. 필드 이름은 EnsureNetworkVariableLengthSafety입니다. |
| Recycle Network Ids | 켬 | 켬 | 제거된 객체의 NetworkObjectId를 일정 시간 후 재사용합니다. ClientId를 초기화하는 옵션이 아닙니다. |
| Network Id Recycle Delay | 120초 | 120초 | 객체 번호가 미사용 상태가 된 뒤 재사용할 때까지 기다리는 시간입니다. Recycle Network Ids를 켰을 때 표시됩니다. 같은 번호를 본다고 이전과 같은 게임 객체라고 단정하지 않습니다. |
| Rpc Hash Size | VarIntFourBytes | 기본값 유지 | RPC 식별 해시의 범위입니다. VarIntFourBytes는 32비트, VarIntEightBytes는 64비트 범위이며 큰 범위는 충돌 가능성을 줄이는 데 씁니다. 가변 길이 인코딩이므로 매번 정확히 4·8바이트를 쓴다는 뜻은 아닙니다. |
| Network Profiling Metrics | 켬 | 켬 | 개발 빌드의 메시지 처리 프로파일링 정보를 수집합니다. 처리 비용이 있으며 일반 릴리스 빌드에서는 비활성화됩니다. 필드 이름은 NetworkProfilingMetrics입니다. |
| Network Message Metrics | 켬 | 설치된 경우 기본값 유지 | 메시지 통계 수집입니다. Multiplayer Tools가 있을 때 표시되며 Realtime Network Stats Monitor 등에서 사용합니다. 끄면 수집 비용과 진단 정보가 줄어듭니다. |

**가변 길이 인코딩** 은 값에 따라 표현에 필요한 바이트 수가 달라지는 방식입니다. **직렬화 (Serialization)** 는 데이터를 저장·전송 가능한 표현으로 바꾸는 작업입니다. Inspector 설정 저장과 네트워크 전송의 직렬화는 사용 목적이 다릅니다.

### 2.3 Prefab Settings: 어떤 설계도로 생성할 것인가?

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향·표시 조건 |
| --- | --- | --- | --- |
| Force Same Prefabs | 켬 | 켬 | 등록 프리팹 구성의 일치를 검사합니다. 끄더라도 상대에게 없는 프리팹이 자동 다운로드되지는 않습니다. 대응 스크립트 구성도 맞아야 합니다. |
| Auto Spawn Player Prefab | 켬 | 해당 없음 | 분산 권한에서 승인·동기화 후 Client가 기본 Player Prefab을 자동 생성합니다. Multiplayer Services SDK가 있고 Distributed Authority를 선택했을 때 표시됩니다. 현재 클라이언트–서버 실습의 자동 플레이어 생성 스위치와 혼동하지 않습니다. |
| Default Player Prefab | 미연결 | PlayerNetworkPrefab | 자동 PlayerObject 생성용입니다. 기존 안내의 Player Prefab과 같은 역할이며 이 버전 표시 이름은 Default Player Prefab입니다. 비우면 현재 기본 흐름의 자동 플레이어 생성이 이루어지지 않습니다. |
| Network Prefabs Lists / 목록 참조 | 자동 목록 구성에 따라 다름 | PracticeNetworkPrefabs, 플레이어 등록 | 사용할 NetworkPrefabsList 에셋들입니다. 참조가 없거나 모두 빈 참조면 경고가 나올 수 있습니다. 목록 에셋을 열어 프리팹까지 확인합니다. |
| Migrate Prefab List | 버튼 | 사용하지 않음 | 이전 버전 목록 형식을 감지했을 때만 나오는 변환 버튼입니다. 새 구성에서는 없는 것이 정상입니다. 변환 후 목록 에셋·참조를 확인합니다. |

**NetworkPrefabsList** 는 네트워크 프리팹 목록 에셋입니다. Player Prefab이 자동 포함되는 구성에서는 같은 프리팹을 불필요하게 중복 등록하지 않습니다. **Override (재정의)** 는 요청된 설계도 대신 다른 설계도를 쓰도록 지정하는 기능입니다.

목록 에셋의 Inspector도 확인합니다. +/− 버튼은 항목 추가·제거, Size·개수는 목록 길이입니다.

| 목록 에셋 항목 | 의미 | DAY09 설정·주의 |
| --- | --- | --- |
| Is Default | 기본 목록인지 표시하는 읽기 전용 상태 | 소유권 설정이 아닙니다. 실습용 사용자 목록과 자동 생성 기본 목록을 구분합니다. |
| Prefab | 생성에 사용할 프리팹 | PlayerNetworkPrefab 연결, 루트의 NetworkObject 확인 |
| Override | 대체 사용 여부 | 끔 / None |
| Prefab / Hash 선택 | Override를 켰을 때 원본을 참조 또는 해시로 찾는 방식 | 서로 다른 프로젝트·에셋 구성 등의 고급 용도입니다. |
| Source Prefab To Override | 대체할 원본 프리팹 | Prefab 방식에서 표시됩니다. |
| Source Hash To Override | 대체할 원본 프리팹의 해시 | Hash 방식에서 표시됩니다. 실행 중 객체 번호와 다릅니다. |
| Overriding Target Prefab | 원본 대신 만들 프리팹 | Override 사용 시 필요하며 대응 NetworkBehaviour 구성이 호환되어야 합니다. |

### 2.4 Scene Management Settings: 씬을 함께 준비하기

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향·표시 조건 |
| --- | --- | --- | --- |
| Enable Scene Management | 켬 | 켬 | NGO의 씬 관리, 접속 시 씬 맞추기, 씬에 배치한 NetworkObject 동기화, 씬 이벤트 진행 추적을 사용합니다. 범위는 아래 설명을 읽습니다. |
| Load Scene Time Out | 120초 | 120초 | 씬 로드·언로드 이벤트 완료를 기다리는 제한 시간입니다. Enable Scene Management를 켰을 때 표시됩니다. 너무 짧으면 로딩이 느린 Client를 실패로 판정할 수 있습니다. |

**씬 동기화** 는 “서버와 Client가 같은 게임 공간을 준비하게 하는 것”입니다. 게임 도중 새로 접속한 Client도 필요한 씬과 생성된 네트워크 객체에 동기화합니다. 각 Client의 로드·언로드·접속 동기화 진행 상황은 **씬 이벤트 (Scene Event)** 로 확인합니다.

체크했다고 모든 Unity 씬 변경 호출이 전파되는 것은 아닙니다. 현재 구성에서 함께 이동할 씬은 **네트워크가 시작된 Host 또는 Server가 NGO의 `NetworkManager.Singleton.SceneManager.LoadScene`으로 로드** 합니다. 일반 Unity의 `UnityEngine.SceneManagement.SceneManager.LoadScene`으로 서버만 씬을 바꾸면 이미 연결된 Client의 씬 전환은 자동 동기화되지 않습니다. 로딩은 비동기 작업이므로 시작 성공과 모든 Client의 완료도 다릅니다.

이 기능은 씬 파일을 다운로드해 주지 않습니다. 각 빌드가 해당 씬을 가지고 있어야 하며 기본 LoadScene 경로에서는 Build Profiles의 Scene List에 등록해야 합니다. 모든 Client가 물리적으로 같은 순간에 로드를 끝낸다는 뜻도 아닙니다.

끄더라도 NGO 통신 전체가 꺼지는 것은 아닙니다. 동적 네트워크 프리팹 생성 등을 사용할 수 있지만 씬 로드 순서·준비 완료 확인·씬 객체의 초기 동기화 정책은 별도 설계가 필요합니다. NetworkObject의 Active Scene Synchronization·Scene Migration Synchronization과 함께 읽습니다.

### 2.5 실행 버튼과 실행 중 상태

| 표시 항목 | 조건 | 의미·주의 |
| --- | --- | --- |
| Start Host | Play Mode, 아직 시작하지 않은 Client Server 구성 | 서버와 로컬 Client를 함께 시작합니다. |
| Start Server | 같은 조건 | 서버만 시작합니다. Host와 달리 자기 로컬 플레이어가 자동 참가하는 구성이 아닙니다. |
| Start Client | 같은 조건, 분산 권한에서는 Client 시작 버튼 구성 | 접속을 시도합니다. 시작 성공과 접속 완료는 다릅니다. |
| Stop Host / Server / Client | 해당 역할로 실행 중 | Shutdown으로 네트워크를 종료합니다. Play Mode 자체를 끄는 버튼은 아닙니다. |
| NetworkConfig 수정 불가 안내 | 실행 중 | 먼저 네트워크를 종료해야 수정할 수 있습니다. 편집 상태와 화면이 다른 이유입니다. |
| Multiplayer Tools 설치 안내 | 해당 패키지가 없을 때 | 추가 진단 도구 안내이며 기본 접속을 막는 오류가 아닙니다. |

DAY09에서는 Inspector 버튼 대신 씬에 만든 Host·Client 버튼을 씁니다. Inspector 시작 버튼은 NetworkLauncher의 주소 지정 코드를 통과하지 않으므로 Transport Inspector 설정을 먼저 확인해야 합니다.

### 2.6 API에는 있지만 기본 Inspector에는 없는 값

NetworkConfig의 EnableTimeResync·TimeResyncInterval·EnableNetworkLogs·ConnectionData 같은 필드가 API에 있다고 이 버전 기본 Inspector에 모두 입력 칸이 생기는 것은 아닙니다. 앞의 표는 전용 Inspector가 실제로 그리는 항목 기준입니다. 내부 필드나 추가 Editor 확장의 화면을 기본 수업 설정과 섞지 않습니다.

[공식 NetworkManager Inspector 설명](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/manual/components/core/networkmanager.html#networkmanager-inspector-properties)

## 3. Unity Transport: 목적지와 수신 지점

같은 NetworkManager의 Unity Transport를 확인합니다. **IP (Internet Protocol) 주소** 는 통신 대상을 가리키고 **포트 (Port)** 는 그 컴퓨터의 통신 수신 지점을 구분합니다. `127.0.0.1`은 자기 컴퓨터인 **루프백 (Loopback)** 주소입니다.

**UDP (User Datagram Protocol)** 는 개별 데이터그램으로 보내며 자체적으로 도착·순서를 보장하지 않습니다. **TCP (Transmission Control Protocol)** 는 순서대로 바이트를 전달하는 신뢰성 있는 스트림 프로토콜입니다. **WebSocket** 은 TCP 기반 양방향 메시지 통신입니다. **TLS (Transport Layer Security)** 는 전송 암호화 기술, **DTLS (Datagram Transport Layer Security)** 는 데이터그램 전송에 맞춘 보안 기술입니다.

### 3.1 전송 모드·대기열·시간 제한

**패킷 (Packet)** 은 네트워크로 전달하는 데이터 단위, **Payload (페이로드)** 는 실제로 운반할 데이터입니다. **큐 (Queue, 대기열)** 는 처리할 데이터를 순서대로 보관합니다. **Heartbeat (하트비트)** 는 다른 데이터가 없을 때 연결이 살아 있음을 확인하는 통신입니다. 설정 이름의 **MS (Milliseconds)** 는 밀리초이며 1000ms = 1초입니다.

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향 |
| --- | --- | --- | --- |
| Protocol Type | UnityTransport | UnityTransport | 직접 연결과 RelayUnityTransport 모드를 구분합니다. Relay는 중계 서비스를 통하는 연결이며 별도 접속 정보가 필요합니다. UDP·TCP 선택 칸이나 실패 시 자동 Relay 전환 옵션이 아닙니다. |
| Use WebSockets | 끔 | 끔 | 켜면 WebSocket 전송을 사용합니다. 브라우저 플랫폼 등 제약에 맞춰 선택하며 상대 구성도 호환되어야 합니다. |
| Use Encryption | 끔 | 끔 | UDP에서는 DTLS, WebSocket에서는 TLS 보안 연결을 사용합니다. 직접 연결은 인증서·비밀 정보 설정도 필요합니다. Relay는 접속 데이터와 연결 형식도 맞춰야 합니다. 체크만으로 로그인·전투 검증이 제공되지는 않습니다. |
| Max Packet Queue Size | 128 | 128 | 내부 송수신 큐가 다룰 패킷 수의 한도입니다. 너무 작으면 몰린 패킷 처리가 막힐 수 있습니다. 패킷 바이트 크기·동접 인원·초당 전송량 한도가 아닙니다. |
| Max Payload Size | 6144바이트 | 6144 | Transport가 다룰 비신뢰성 Payload의 최대 크기입니다. 높여도 인터넷에서 그 크기가 그대로 안전하게 전달된다는 뜻은 아닙니다. 실제 전송에서는 분할 등도 고려합니다. |
| Heartbeat Timeout MS | 500ms | 500 | 통신이 없을 때 Heartbeat를 보내기까지 기다리는 시간입니다. 끊김 판정 시간과 다릅니다. |
| Connect Timeout MS | 1000ms | 1000 | 새 접속 시도를 다시 보내기 전 기다리는 간격입니다. 전체 접속 대기 시간이 1초라는 뜻은 아닙니다. |
| Max Connect Attempts | 60회 | 60 | 접속 시도를 반복할 최대 횟수입니다. 초과하면 실패 처리합니다. 같은 대상을 재시도하며 Relay로 자동 전환하지 않습니다. |
| Disconnect Timeout MS | 30000ms | 30000 | 연결된 상대에게서 통신을 받지 못했을 때 끊김으로 판단하는 제한 시간입니다. Heartbeat도 통신에 포함되므로 게임 메시지가 없다는 이유만으로 끊기지는 않습니다. |

기본 조합의 직접 접속 실패 대기는 대략 `1000ms × 60회 = 60초` 규모입니다. 처리 시점·환경에 따라 달라지며 정확히 60초를 보장하는 계산은 아닙니다. 접속 재시도·승인 대기·씬 로드는 서로 다른 단계입니다. 모든 값을 같은 시간으로 맞추거나 모두 줄이는 방식으로 해결하지 않습니다.

### 3.2 접속 주소와 서버 바인딩

**바인딩 (Binding)** 은 소켓을 자기 컴퓨터의 수신 주소·포트에 묶는 작업입니다. **소켓 (Socket)** 은 프로그램이 네트워크로 데이터를 보내고 받는 통신 접점입니다. **IPv4 (Internet Protocol version 4)** 와 **IPv6 (Internet Protocol version 6)** 는 IP 주소 체계입니다. IPv4 루프백은 127.0.0.1, IPv6 루프백은 ::1입니다. **URL (Uniform Resource Locator)** 은 통신 자원의 위치를 표현하는 주소입니다.

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향·표시 조건 |
| --- | --- | --- | --- |
| Address | 127.0.0.1 | 127.0.0.1 | Client의 목적지 서버입니다. 다른 컴퓨터에 접속하려면 Host의 주소가 필요합니다. 지원 환경에서는 호스트 이름도 쓸 수 있습니다. |
| Port | 7777 | 7777 | 목적지 서버의 수신 포트입니다. Host·Client의 목적지 포트가 다르면 접속하지 못합니다. Client의 로컬 포트와 같아야 한다는 뜻은 아닙니다. |
| Web Socket Path / WebSocket Path | / | 해당 없음 | Use WebSockets를 켰을 때 표시됩니다. URL 중 접속·수신할 경로이며 IP 주소를 넣는 칸이 아닙니다. 필드 이름은 WebSocketPath입니다. |
| Allow Remote Connections? | 루프백 수신, 끔 | 끔 | 외부 컴퓨터의 접속을 받을 수신 구성을 허용합니다. 켜고 별도 IP를 쓰지 않으면 IPv4는 0.0.0.0, IPv6는 ::로 모든 해당 로컬 인터페이스에서 수신합니다. 방화벽·공유기를 자동 설정하지 않습니다. |
| Override Bind IP (optional) | 빈 칸 | 빈 칸 | Remote Connections가 켜졌을 때 수정 가능합니다. 서버가 수신할 특정 로컬 IP입니다. Client가 찾아갈 목적지나 공유기의 공인 IP를 무조건 입력하는 칸이 아닙니다. |

ServerListenAddress는 이 버전 기본 Inspector에서 독립 칸 대신 마지막 두 항목으로 제어합니다. Remote Connections를 끄면 루프백, 켜고 Override를 비우면 전체 인터페이스 수신으로 설정됩니다. 0.0.0.0은 서버 수신 범위이며 Client 목적지로 쓰지 않습니다. 방화벽·공유기 통과 조건은 별도입니다.

DAY09의 NetworkLauncher는 시작 직전에 SetConnectionData로 Address·Port·ServerListenAddress를 지정합니다. **Inspector에서 주소만 바꿔도 코드가 다시 덮어쓸 수 있습니다.** HostAddress·HostPort는 상수이므로 소스에서 바꾸고 실행 파일도 함께 맞춥니다. 다른 컴퓨터 접속은 지금의 루프백 실습과 구분합니다.

### 3.3 표시되지 않는 API·이전 버전 항목

| 항목 | 현재 처리 | 의미·주의 |
| --- | --- | --- |
| Client Bind Port (`ConnectionData.ClientBindPort`) | 기본 화면에서 숨긴 ConnectionData 안의 API 설정 | 기본 0이며 운영체제가 Client 로컬 포트를 정합니다. 목적지 Port=7777과 별개입니다. |
| Max Send Queue Size (`MaxSendQueueSize`) | 일반 입력 칸이 아닌 API 설정 | 송신 큐 크기는 필요에 따라 동적으로 정해집니다. 특수한 메모리 제한 목적이 아니면 제한하지 않습니다. 너무 작으면 큰 트래픽에서 끊김을 유발할 수 있습니다. |
| Debug Simulator / Packet Delay·Jitter·Drop | 숨김·지원 중단, 설정해도 효과 없음 | 옛 화면의 항목을 현재 누락으로 오해하지 않습니다. 테스트에는 Multiplayer Tools의 Network Simulator를 사용합니다. |

**지터 (Jitter)** 는 지연 시간의 들쭉날쭉한 변화, **패킷 손실 (Packet Loss)** 은 일부 패킷이 전달되지 않는 현상입니다. 직접 연결 실패 뒤 Relay로 재접속하려면 연결 관리 코드를 따로 구현해야 하며 Host도 같은 Relay 할당으로 준비되어 있어야 합니다.

[공식 UnityTransport API](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/api/Unity.Netcode.Transports.UTP.UnityTransport.html), [Unity Transport 6.6 시간 제한 안내](https://docs.unity3d.com/Packages/com.unity.transport@6.6/manual/faq.html)

## 4. NetworkObject: 생성·소유권·부모·씬의 기준

PlayerNetworkPrefab 루트를 선택합니다. **스폰 (Spawn)** 은 NGO에 네트워크 객체로 생성·등록하는 작업입니다. 로컬 Instantiate만 실행하는 것과 다릅니다. 현재 실습은 루트에 NetworkObject 하나를 두고 Body에 중복 추가하지 않습니다.

### 4.1 편집 상태의 설정

**관찰자 (Observer)** 는 그 객체의 복제본·갱신을 전달받는 참여자입니다. 카메라에 보이는지와 네트워크 관찰은 다릅니다. **부모 관계 (Parenting)** 는 어떤 객체의 하위에 속하는지를 말합니다.

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향 |
| --- | --- | --- | --- |
| Ownership | Distributable | 해당 없음 / 기본값 유지 | Multiplayer Services SDK 조건에서 표시되는 분산 권한용 정책입니다. 다음 표를 읽습니다. 클라이언트–서버에서 OwnerClientId를 입력하는 칸이 아닙니다. |
| Always Replicate As Root | 끔 | 끔 | 상대 복제본에서 부모를 무시하고 루트 객체로 취급합니다. 부모 관계를 유지하려는 구성에서는 주의합니다. |
| Synchronize Transform | 켬 | 켬 | 생성·접속 초기 동기화 때 Transform 정보를 전달합니다. 계속 움직이는 위치를 매번 전송하는 기능이 아닙니다. |
| Active Scene Synchronization | 끔 | 끔 | 활성 씬이 바뀌면 동적으로 생성된 객체를 새 활성 씬으로 옮기고 동기화합니다. 여러 씬을 사용하는 구성에서 객체 소속을 결정합니다. |
| Scene Migration Synchronization | 켬 | 켬 | 서버가 동적으로 생성된 객체를 다른 씬으로 이동했을 때 Client의 대응 객체도 같은 씬으로 이동시킵니다. 활성 씬 변경 없이 특정 객체를 옮기는 경우도 포함합니다. |
| Spawn With Observers | 켬 | 켬 | 생성 시 관찰 대상에게 공개합니다. 끄면 서버 외의 관찰자가 없는 상태로 시작하며 NetworkShow 등으로 필요한 Client에 공개해야 합니다. |
| Dont Destroy With Owner | 끔 | 끔 | 켜면 소유 Client가 끊겼을 때 객체가 함께 제거되지 않도록 합니다. 클라이언트–서버에서는 남은 객체의 서버 소유·관리도 고려합니다. 게임 상태 복구가 자동 구현되지는 않습니다. |
| Auto Object Parent Sync | 켬 | 켬 | 지원되는 부모 관계 변경을 자동 동기화합니다. 끄면 별도 정책이 필요합니다. |
| Sync Owner Transform When Parented | 켬 | 켬 | 부모 변경 때 서버·Host가 정한 Transform 상태를 소유자 복제본에도 적용합니다. Owner 권한 이동에서 전환을 조정할 때 검토하며, 꺼도 비소유 복제본의 동기화까지 모두 끄지는 않습니다. |
| Allow Owner To Parent | 끔 | 끔 | 클라이언트–서버에서 소유자가 별도 서버 요청 RPC 없이 부모 변경을 시작하도록 허용합니다. 분산 권한은 소유자가 권한자이므로 같은 스위치가 필요하지 않습니다. 일반 NGO 부모 조건까지 없애지는 않습니다. |

Active Scene Synchronization과 Scene Migration Synchronization은 **동적으로 생성한 NetworkObject와 통합 NetworkSceneManager** 를 사용하는 조건에서 읽습니다. Enable Scene Management를 켜야 합니다. 전자는 활성 씬 변경에 따른 이동, 후자는 특정 객체의 씬 이동 결과 전달입니다. 둘을 함께 켤 수 있지만 후자만 켰다고 모든 객체가 항상 활성 씬을 따라가지는 않습니다.

부모 변경은 생성된 객체·대응 부모 등 NGO 조건을 지켜야 합니다. 실습에서는 부모를 바꾸지 않습니다. 옵션을 켰다고 프리팹에 중첩 NetworkObject를 추가해도 된다는 뜻은 아닙니다.

### 4.2 Ownership의 조건부 선택지

앞에서 설명한 소유권·권한을 구분해서 읽습니다. **Session Owner** 는 분산 권한 세션의 조정 역할을 맡는 참여자입니다. 현재 Host·Client 실습의 Server 권한과 같은 뜻이 아닙니다.

| 선택지 | 분산 권한에서의 의미 | 주의 |
| --- | --- | --- |
| None | 자동 분배·즉시 획득·요청에 의한 변경을 허용하지 않는 정적 정책 | 소유자 번호를 지정한다는 뜻이 아닙니다. |
| Distributable | 참여자가 들어오거나 나갈 때 분배 대상 | 잠금·진행 중 요청 같은 조건도 영향을 줍니다. 기본 정책입니다. |
| Transferable | 잠금 등 조건이 허용하면 비소유자가 즉시 획득 | 전투 결과를 신뢰해도 된다는 보안 보장이 아닙니다. |
| Request Required | 현재 소유자에게 요청해 소유권 획득 | 요청·응답 흐름을 코드로 다룹니다. |
| Session Owner | 세션 소유자만 이 객체를 소유 | Player Prefab의 소유자 배정과 구분합니다. |
| All | 분산·이전·요청 플래그를 묶어 선택하는 Editor 편의 항목 | 모두가 동시에 같은 객체의 소유자가 되는 뜻이 아닙니다. Session Owner와 임의로 함께 조합하지 않습니다. |

### 4.3 실행 중 읽기 전용 표시와 진단 버튼

Play Mode의 생성된 복제본을 선택하면 편집용 표 대신 상태가 표시됩니다. 번호·체크는 NGO가 정한 상태이지 직접 입력할 설정이 아닙니다.

| 항목 | 의미·확인할 점 |
| --- | --- |
| GlobalObjectIdHash | 원본 프리팹·씬 객체를 식별하는 해시입니다. NetworkObjectId와 다릅니다. 편집 상태에서도 읽기 전용으로 표시됩니다. |
| NetworkObjectId | 이번 실행에서 생성된 객체 번호입니다. 대응 복제본은 같은 번호를 씁니다. |
| OwnerClientId | 현재 소유자 번호입니다. |
| IsSpawned | 네트워크에 생성·등록된 상태입니다. |
| IsLocalPlayer | 현재 실행 창의 대표 PlayerObject인지 확인합니다. |
| IsOwner | 현재 실행 창이 이 객체의 소유자인지 확인합니다. |
| IsOwnedByServer | 서버 소유인지 확인합니다. Host의 자기 플레이어와 별도 Client의 플레이어를 비교합니다. |
| IsPlayerObject | 특정 Client의 대표 PlayerObject인지 확인합니다. 소유자가 있다고 모두 PlayerObject는 아닙니다. |
| IsSceneObject | 씬에 미리 배치됐는지 표시하는 기존 화면 이름이며 2.13 코드의 InScenePlaced 상태와 연결됩니다. |
| DestroyWithScene | 속한 씬이 제거될 때 객체도 제거되는지 표시합니다. 생성 방식·Spawn 옵션 등의 런타임 정책이지 편집 체크박스가 아닙니다. |
| NetworkManager | 관리하는 NetworkManager 참조입니다. 편집 상태에서도 읽기 전용으로 표시됩니다. |
| Observers | 서버에서 펼쳐 볼 수 있는 관찰 Client 목록이며 전체 접속자 목록과 항상 같지는 않습니다. |
| Ownership | 분산 권한 실행에서 현재 정책을 표시합니다. |
| Spawn 버튼 | Play Mode의 서버에서 아직 생성되지 않은 해당 객체에 표시됩니다. NetworkObject.Spawn을 호출하는 진단 버튼이며 평소 생성 로직을 대신하지 않습니다. |

### 4.4 NetworkObject와 NetworkTransform의 관계

NetworkObject가 “누구의 어떤 객체인가?”를 정한다면 NetworkTransform은 “그 객체가 어디에 있는가?”를 전달합니다. **NetworkObject가 전혀 없으면 NetworkTransform의 네트워크 동기화는 동작하지 않습니다.** 같은 객체 또는 부모에 NetworkObject가 있어야 하며 실제 동기화에는 네트워크 생성도 완료되어야 합니다.

같은 루트에 둘을 둘 수도, 부모에 NetworkObject 하나를 두고 자식에 NetworkTransform을 둘 수도 있습니다. 자식마다 NetworkObject를 중복 추가할 필요는 없습니다. 반대로 NetworkObject만 있다고 이동이 계속 전달되는 것도 아닙니다. **Synchronize Transform의 초기 전달과 NetworkTransform의 지속 갱신은 별개** 입니다.

[공식 NetworkObject API](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/api/Unity.Netcode.NetworkObject.html), [공식 구성·부모 조건](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/manual/components/core/networkobject.html#using-networkobjects)

## 5. NetworkTransform: 누가 어떤 이동 값을 보낼 것인가?

프리팹 루트의 일반 NetworkTransform을 확인합니다. 순서는 Axis to Synchronize → Authority → Thresholds → Delivery → Configurations입니다. 상속한 컴포넌트에서는 NetworkTransform Properties로 접혀 보일 수도 있습니다. 기본 설정과 파생 클래스가 추가한 설정을 구분합니다.

### 5.1 Axis to Synchronize와 Authority

**축 (Axis)** 은 좌표·회전의 X·Y·Z 방향입니다. Unity 3D 실습에서 X는 좌우, Y는 높이, Z는 앞뒤이고 Y축 회전은 캐릭터가 좌우 방향을 바라보는 회전입니다. **Quaternion (쿼터니언)** 은 3차원 회전을 표현하는 네 성분의 수, **Euler Angle (오일러 각)** 은 축별 각도 표현입니다. 쿼터니언 동기화는 일부 축을 따로 보내는 대신 회전 전체를 다룹니다.

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향 |
| --- | --- | --- | --- |
| Position X / Y / Z | 모두 켬 | 모두 켬 | 위치를 전달할 축입니다. 끈 축은 지속 갱신에서 제외됩니다. |
| Rotation X / Y / Z | 모두 켬 | Y만 켬 | 회전을 전달할 축입니다. Quaternion 동기화를 켜면 선택이 숨겨지고 모두 켜집니다. 다시 끈 뒤 원하는 축을 재확인합니다. |
| Scale X / Y / Z | 모두 켬 | 모두 끔 | 크기를 전달할 축입니다. 실습은 크기가 변하지 않아 제외합니다. |
| Authority Mode | Server | Owner | Server는 서버, Owner는 현재 소유자의 Transform 변경을 전송합니다. 분산 권한에서는 소유자가 상태 권한자라는 모드 제약도 고려합니다. |

Authority Mode는 소유자를 바꾸거나 체력·점수의 쓰기 권한을 바꾸는 설정이 아닙니다. Server로 바꾸면 Client가 로컬 Transform을 직접 이동시키는 현재 코드와 맞지 않습니다. 그때는 Client가 입력을 요청하고 서버가 이동하도록 코드를 바꿉니다. OwnerNetworkTransform과 일반 NetworkTransform을 함께 추가하지 않습니다.

### 5.2 Thresholds: 얼마나 달라지면 갱신할 것인가?

**Threshold (임계값)** 는 갱신을 보낼 변화량의 기준입니다. 해당 종류의 동기화 축을 모두 끄면 그 임계값 칸도 숨겨집니다.

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향 |
| --- | --- | --- | --- |
| Position Threshold | 0.001 | 기본값 유지 | 위치 변경량 기준입니다. 높이면 작은 이동 전달이 줄 수 있고 낮추면 갱신량이 늘 수 있습니다. |
| Rot Angle Threshold | 0.01도 | 기본값 유지 | 회전 각도 변경량 기준입니다. 코드 이름은 RotAngleThreshold이고 Quaternion 방식에서도 갱신 판단에 사용합니다. |
| Scale Threshold | 0.01 | 기본값 유지 | 크기 변경량 기준입니다. 실습은 Scale 축을 껐으므로 칸이 숨겨집니다. |

위치·크기 임계값을 0으로 두면 변화가 없어도 매 네트워크 틱 갱신을 유발할 수 있습니다. 화면의 모든 프레임마다 패킷 하나를 보낸다는 뜻은 아닙니다. Tick Rate·전송 묶음 등도 작용합니다. 상대 복제본의 Inspector에서만 바꿔 권한자의 전송 정책을 바꿀 수는 없습니다.

### 5.3 Delivery: 전달 방식

**델타 (Delta)** 는 이전 값과 비교한 변화입니다. **신뢰성 전달 (Reliable Delivery)** 은 손실 복구·순서 보장을 포함하고, **비신뢰성 전달 (Unreliable Delivery)** 은 해당 전달에서 도착·순서를 모두 보장하지 않는 방식입니다. 최신 상태를 자주 보내는 이동 갱신에 활용할 수 있습니다.

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향 |
| --- | --- | --- | --- |
| Tick Sync Children | 끔 | 끔 | 부모가 갱신을 보낼 때 자식의 갱신도 맞추도록 합니다. 자식들은 자기 변화량 기준으로도 갱신하며 자식마다 같은 체크가 필요한 것은 아닙니다. 계층 이동의 시점 차이를 줄입니다. |
| Use Unreliable Deltas | 끔 | 끔 | 일반 변화 갱신에 비신뢰성 전달을 사용해 손실 환경에서 오래된 상태 대기를 줄이는 데 활용합니다. 끄면 신뢰성 있는 분할·순서 전달을 씁니다. 자동 폴백이나 전체 NGO 메시지 모드 스위치는 아닙니다. |

Use Unreliable Deltas는 모든 상태를 무조건 버려도 된다는 뜻이 아닙니다. 초기·전체 상태 전달과 손실을 보완하는 주기적 축 상태 동기화 같은 처리가 함께 있습니다. 아래 Switch Transform Space When Parented와 동시에 사용할 수 없습니다.

### 5.4 Configurations: 좌표계와 보간

**월드 좌표 (World Space)** 는 씬 전체 기준, **로컬 좌표 (Local Space)** 는 부모 기준입니다. **보간 (Interpolation)** 은 수신 상태 사이를 이어 부드럽게 표시합니다. 지연·충돌·권한 검증을 없애는 기능은 아닙니다.

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향·표시 조건 |
| --- | --- | --- | --- |
| Switch Transform Space When Parented | 끔 | 끔 | 부모가 있으면 로컬, 없으면 월드 좌표로 자동 전환하며 부모 변경 표시를 처리합니다. 켜면 In Local Space가 숨겨지고 Tick Sync Children이 켜지며 Use Unreliable Deltas가 꺼집니다. |
| In Local Space | 끔 | 끔 | 로컬 좌표 동기화를 직접 선택합니다. 자동 전환을 끈 경우에만 표시됩니다. 부모의 구성·이동도 고려해야 합니다. |
| Interpolate | 켬 | 켬 | 권한 없는 복제본의 표시를 보간합니다. 끄면 수신 상태를 바로 적용해 이동이 계단처럼 보일 수 있습니다. 파생 컴포넌트가 별도로 숨기는 경우도 있습니다. |

Interpolate를 켜고 해당 종류의 축이 하나 이상 켜져 있을 때 하위 항목이 나타납니다. DAY09에서는 Position·Rotation만 보이며 Scale 보간 설정은 숨겨집니다.

| 항목 | 기본값 | 의미·DAY09 확인 |
| --- | --- | --- |
| Position Interpolation Type (`PositionInterpolationType`) | LegacyLerp | 위치 보간 알고리즘입니다. 기본값 유지. |
| Rotation Interpolation Type (`RotationInterpolationType`) | LegacyLerp | 회전 보간 알고리즘입니다. 기본값 유지. |
| Scale Interpolation Type (`ScaleInterpolationType`) | LegacyLerp | 크기 보간 알고리즘이며 Scale 동기화가 필요합니다. |
| Slerp Position | 끔 | 위치를 구면 보간으로 목표에 접근시킵니다. 직선 이동과 궤적이 달라질 수 있습니다. Position의 SmoothDampening에서는 숨겨집니다. |
| Position Lerp Smoothing (`PositionLerpSmoothing`) | 켬 | 위치의 추가 평활화입니다. 기본값 유지. |
| Rotation Lerp Smoothing (`RotationLerpSmoothing`) | 켬 | 회전의 추가 평활화입니다. 기본값 유지. |
| Scale Lerp Smoothing (`ScaleLerpSmoothing`) | 켬 | 크기의 추가 평활화이며 Scale 동기화가 필요합니다. |
| Position Max Interpolation Time (`PositionMaxInterpolationTime`) | 0.1초 | Position Lerp Smoothing을 켰을 때 표시됩니다. 커지면 부드러운 대신 빠른 변화가 둔해질 수 있고 작으면 정밀한 대신 떨림이 드러날 수 있습니다. |
| Rotation Max Interpolation Time (`RotationMaxInterpolationTime`) | 0.1초 | Rotation Lerp Smoothing을 켰을 때 표시되는 회전 평활화 시간 기준입니다. |
| Scale Max Interpolation Time (`ScaleMaxInterpolationTime`) | 0.1초 | Scale Lerp Smoothing을 켰을 때 표시되는 크기 평활화 시간 기준입니다. |

**Lerp (Linear Interpolation)** 는 선형 보간, **Slerp (Spherical Linear Interpolation)** 는 구면 선형 보간입니다. **평활화 (Smoothing)** 는 급격한 변화가 보이지 않게 추가로 완만하게 표시하는 처리입니다.

Interpolation Type의 LegacyLerp는 기존 방식, Lerp는 지연에 따른 보간 버퍼 깊이 등을 반영하는 선형 방식, SmoothDampening은 변화율에 맞춰 부드럽게 접근하는 감쇠 방식입니다. **버퍼 (Buffer)** 는 나중에 처리할 데이터를 잠시 보관하는 공간입니다. LegacyLerp의 추가 평활화는 프레임 속도에 의존하므로 새 방식과 같은 값을 써도 결과가 같다고 볼 수 없습니다. 실행 중 알고리즘 변경은 순간적 흔들림을 만들 수 있어 수업은 기본값을 유지합니다.

### 5.5 Configurations 마지막: 회전 표현과 압축

앞에서 설명한 쿼터니언·오일러 각은 회전 표현의 선택이고, 다음 옵션은 그 값을 어떤 정밀도·크기로 전송할지 정합니다.

**Float (부동소수점 수)** 는 소수 값을 표현하며 일반 float는 32비트입니다. **Half Float** 는 16비트 표현으로 데이터 양을 줄이는 대신 정밀도·범위가 줄어듭니다. 1바이트는 8비트입니다. **압축 (Compression)** 은 표현 크기를 줄이는 처리입니다.

| 항목 | 기본값 | DAY09 값 | 의미·변경 시 영향·표시 조건 |
| --- | --- | --- | --- |
| Use Quaternion Synchronization | 끔 | 끔 | 회전 전체를 쿼터니언으로 전달합니다. 켜면 Rotation X·Y·Z 선택이 숨겨지고 모두 켜집니다. 복잡한 회전·부모 계층에서 검토하지만 Y축 회전 실습에는 필요하지 않습니다. |
| Use Quaternion Compression | 끔 | 끔 | 쿼터니언 동기화가 켜졌을 때 표시됩니다. 회전 표현을 16바이트에서 4바이트로 줄이는 대신 정밀도 손실이 있습니다. 전체 패킷 길이는 아닙니다. 쿼터니언 동기화를 끄면 이 설정도 꺼집니다. |
| Use Half Float Precision | 끔 | 끔 | 위치·회전·크기 표현의 정밀도를 줄입니다. 쿼터니언 회전은 압축이 꺼졌을 때 16바이트에서 8바이트 표현을 쓸 수 있습니다. Quaternion Compression이 켜지면 회전에서는 그 압축이 우선합니다. |

빠른 이동·큰 좌표·복잡한 회전에서 압축을 켜기 전에는 오차와 결과를 시험합니다. 모두 켜는 것이 항상 최선은 아닙니다. Rigidbody·Rigidbody2D가 있는데 대응 NetworkRigidbody·NetworkRigidbody2D가 없으면 Inspector가 경고를 표시할 수 있습니다. 현재 실습에는 Rigidbody를 추가하지 않습니다.

[공식 NetworkTransform API](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/api/Unity.Netcode.Components.NetworkTransform.html)

## 6. NetworkBehaviour: 직접 만든 스크립트의 Inspector

NetworkBehaviour는 네트워크 스크립트의 **추상 기반 클래스** 입니다. 추상 클래스는 그대로 객체를 만들기보다 상속해 동작을 구현하는 기반입니다. NetworkBehaviour 자체 대신 NetworkCharacterPlayer 같은 파생 스크립트를 추가합니다. NetworkTransform도 NetworkBehaviour를 상속합니다.

모든 파생 스크립트에 Move Speed·Gravity가 있는 것은 아닙니다. **보이는 설정은 스크립트의 직렬화 필드와 전용 Editor에 따라 다릅니다.**

### 6.1 일반 필드·네트워크 상태·코드 선언 구분

**필드 (Field)** 는 클래스의 데이터 저장 공간입니다. **SerializeField** 는 private 필드를 Unity가 저장·표시하도록 지정하는 **특성 (Attribute)** 입니다. 특성은 코드 요소에 부가 정보를 붙이는 문법입니다. **NetworkList** 는 목록 변경을 공유하는 네트워크 자료형입니다.

| 종류·표시 | Inspector와의 관계 |
| --- | --- |
| Script | 소스 참조이며 기본 NetworkBehaviour Editor에서는 읽기 전용입니다. 일부 전용 NGO Editor는 숨깁니다. |
| Unity 직렬화를 지원하는 public 필드 | 일반 저장·표시 대상이며 HideInInspector 등 숨김 설정의 영향을 받습니다. |
| SerializeField가 붙은 지원 형식의 private 필드 | private이어도 저장·표시됩니다. 현재 moveSpeed·gravity입니다. |
| public NetworkVariable·NetworkList | 지원 형식은 NGO 전용 Editor가 별도로 표시할 수 있습니다. 일반 Unity 필드처럼 저장되는 설정과 다르며 형식별 표시 제한이 있습니다. |
| IsOwner·IsServer·IsClient·IsSpawned | 실행 중 상태를 읽는 프로퍼티이지 소유권·역할을 지정하는 일반 입력 칸이 아닙니다. |
| NetworkVariable 읽기·쓰기 권한, RPC 대상 | 스크립트 선언·API로 정합니다. NetworkTransform Authority Mode 하나로 모두 결정하지 않습니다. |
| OnNetworkSpawn | 네트워크 생성 때 NGO가 호출하는 메서드이지 설정 칸이 아닙니다. |
| HostAddress·HostPort·moveAction | 상수 또는 직렬화하지 않은 private 필드이며 일반 입력 칸이 아닙니다. |

전용 Editor에서 NetworkVariable 값이 보여도 영구 프리팹 설정 저장을 뜻하지 않습니다. 실행 중 변경은 쓰기 권한을 지켜야 하며 권한 없는 변경은 실패·오류가 될 수 있습니다. Inspector에 저장된 값이 상대에게 자동 전송되는 것과도 구분합니다.

### 6.2 NetworkCharacterPlayer의 실제 설정

| 항목 | 코드 초기값·DAY09 값 | 의미·변경 시 영향 |
| --- | --- | --- |
| Move Speed | 4 | 소유자의 초당 이동 속도이며 높이면 이동이 빨라집니다. |
| Gravity | -20 | 아래 방향 가속도이며 코드가 수직 속도에 적용합니다. |

이동은 자기 플레이어에서만 계산하고 결과를 NetworkTransform이 전송합니다. Move Speed를 바꿔도 상대 프리팹 설정 파일이 자동 수정되지는 않습니다.

NetworkObject는 같은 객체 또는 부모에 있어야 합니다. 현재 루트는 **NetworkObject → NetworkTransform → NetworkCharacterPlayer** 순서입니다. NetworkObject를 NetworkBehaviour 계열보다 위에 두고 양쪽 스크립트 구성·순서를 맞춥니다. OnNetworkSpawn 호출 순서도 배치와 관련이 있습니다.

[공식 NetworkBehaviour 설명](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.13/manual/components/core/networkbehaviour.html), [Unity 6 SerializeField 설명](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/SerializeField.html)

## 7. 실습 보조 컴포넌트

이 절은 DAY09에 필요한 보조 설정입니다. NGO 핵심 Inspector 안내와 달리 일반 UI·카메라의 모든 옵션을 열거하는 문서는 아닙니다.

### 7.1 CharacterController: 이동과 충돌

NetworkTransform이 이동 결과 전달이라면 CharacterController는 이동 수행입니다. 부모 Scale=(1, 1, 1) 기준으로 외형과 충돌 크기를 맞춥니다. 아래는 DAY09에 저장할 값입니다.

| 항목 | DAY09 값 | 의미·변경 시 영향 |
| --- | --- | --- |
| Height | 2 | 충돌 캡슐의 전체 높이입니다. 외형과 다르면 접촉이 어색해집니다. |
| Radius | 0.5 | 반지름이며 좁은 통로·벽과의 간격에 영향을 줍니다. |
| Center | (0, 1, 0) | 루트 기준 중심으로 높이 2인 캡슐 발밑을 루트 위치에 맞춥니다. |
| Skin Width | 0.08 | 경계의 작은 겹침 여유이며 너무 작으면 떨림·걸림이 생길 수 있습니다. |
| Step Offset | 0.3 | 자동으로 올라갈 수 있는 턱 높이입니다. |
| Slope Limit | 기본값 유지 | 올라갈 수 있는 경사 각도 제한입니다. |
| Min Move Distance | 0 | 작은 이동을 무시하는 기준이며 실습에서는 작은 이동도 처리합니다. |

**Collider (콜라이더)** 는 충돌 판정용 형태, **Rigidbody** 는 질량·힘 등을 물리 엔진으로 계산하는 컴포넌트입니다. Body의 CapsuleCollider는 제거하고 Rigidbody도 추가하지 않습니다. CharacterController.Move는 중력을 자동 적용하지 않아 코드가 계산합니다.

Inspector에서 켜 둔 상태와 실행 상태도 구분합니다. 코드는 Awake에서 컨트롤러를 끄고 OnNetworkSpawn에서 소유자의 컨트롤러만 켭니다. 상대 복제본이 꺼진 것은 정상입니다.

[Unity 6 CharacterController 설정 설명](https://docs.unity3d.com/6000.0/Documentation/Manual/class-CharacterController.html)

### 7.2 화면·입력·버튼 연결

**UI (User Interface)** 는 사용자와 상호작용하는 화면입니다. **TextMeshProUGUI** 는 Canvas에 텍스트를 표시합니다. **InputSystemUIInputModule** 은 Input System 입력을 UI 동작에 연결하고, **InputAction** 은 입력을 게임 동작으로 묶습니다. **Orthographic (직교 투영)** 은 원근에 따른 축소 없이 보여 주는 방식입니다.

| 컴포넌트·항목 | DAY09 설정 | 의미 |
| --- | --- | --- |
| NetworkLauncher / Status Text | StatusText의 TextMeshProUGUI | 연결 상태 표시입니다. Launcher는 플레이어 소유권이 아니라 시작·표시 스크립트입니다. |
| Host 버튼 / Button / On Click() | NetworkLauncher.StartHost() | 같은 씬의 Launcher를 연결합니다. |
| Client 버튼 / Button / On Click() | NetworkLauncher.StartClient() | 같은 Launcher의 Client 시작을 연결합니다. |
| EventSystem / InputSystemUIInputModule | 기본 액션 연결 | 비면 Assign Default Actions를 실행합니다. 기존 StandaloneInputModule과 섞지 않습니다. |
| Main Camera / Projection·Size | Orthographic·8 | 두 캐릭터를 함께 보는 카메라이며 관찰자·소유권을 정하지 않습니다. |
| Body / Renderer | 활성화 | 외형 표시이며 네트워크 전송과 별개입니다. |

이동 코드는 InputAction을 코드에서 만듭니다. 별도 PlayerInput·입력 에셋을 연결할 칸은 없습니다. 실제 씬·프리팹 제작·카메라 배치는 DAY09를 따릅니다.

## 8. 서로 비슷해 보이는 설정 비교

| 헷갈리는 설정 | 구분할 질문 |
| --- | --- |
| Connect Timeout / Client Connection Buffer Timeout / Load Scene Time Out / Spawn Timeout | 재시도 간격인가, 승인을 기다리는가, 씬을 기다리는가, 메시지의 대상 객체를 기다리는가? |
| NetworkObject Synchronize Transform / NetworkTransform | 초기 상태 전달인가, 지속적인 이동 전달인가? |
| Ownership / Authority Mode | 누구에게 배정하는가, Transform을 누가 전송하는가? |
| Enable Scene Management / Active Scene Synchronization / Scene Migration Synchronization | 참여자의 씬 준비인가, 객체가 활성 씬을 따라가는가, 특정 객체의 씬 이동을 전파하는가? |
| Auto Object Parent Sync / In Local Space / Switch Transform Space When Parented | 부모 관계를 전달하는가, 좌표 기준을 정하는가, 부모 변경에 따라 자동 전환하는가? |
| Observer / 카메라 / IsOwner | 데이터를 받는가, 화면에 보이는가, 조작할 소유자인가? |

## 9. 설정 확인 순서와 완료 기준

1. NetworkManager의 Client Server 구성, Transport 참조, Default Player Prefab·목록을 확인합니다.
2. Transport의 모드·주소·포트·루프백 수신과 시작 코드의 덮어쓰기를 확인합니다.
3. NetworkObject의 생성·관찰·소유자 이탈·부모·씬 정책을 확인합니다.
4. NetworkTransform의 Owner 권한, 위치 전체·회전 Y·크기 끔, 기본 보간·압축 설정을 확인합니다.
5. 컨트롤러 크기·이동 속도·중력·화면·버튼·입력 참조를 확인합니다.
6. 저장·재빌드한 뒤 참여자 2명·객체 생성·이동·벽 충돌을 확인합니다.
7. 복제본의 NetworkObjectId·OwnerClientId·IsOwner를 비교하고 편집 설정과 구분합니다.

설정은 코드가 사용할 조건·참조를 준비하는 것이지 게임 코드를 대신하지 않습니다. [DAY09 안내형 실습](../DAY09_NGO_NETWORK_MANAGER_HOST_CLIENT.md)의 제작 순서와 함께 사용합니다.

### 생각해보기

1. NetworkObject만 있을 때 처음 위치와 이후 이동은 어떻게 다를까요?
2. Use Unreliable Deltas와 Switch Transform Space When Parented의 배타 조건은 어떻게 확인할까요?
3. 서버가 일반 Unity SceneManager로만 씬을 바꾸면 이미 접속한 Client는 어떻게 될까요?
4. 접속 실패가 재시도·승인 대기·씬 로드 중 어느 단계인지 어떻게 구분할까요?
5. Authority Mode를 Owner로 바꾸면 체력 변수의 쓰기 권한도 바뀔까요?

## 10. 확인 근거와 범위

설치된 NGO 2.13.3의 NetworkManagerEditor·NetworkObjectEditor·NetworkTransformEditor·NetworkBehaviourEditor·UnityTransportEditor와 대응 런타임 필드, Unity Transport 6.6.0 시간 상수를 대조했습니다. 표시 조건은 전용 Editor, 기본값은 런타임 선언 기준입니다. 오래된 항목을 현재 기본 화면에 있는 것처럼 섞지 않습니다.

실제 접속·이동·충돌 검증은 [별도 기록](../../../../Code/GameNetwork/NGO/VERIFICATION.md)에 있습니다. **이번 보강에서 모든 고급 옵션 조합을 실행 시험한 것은 아닙니다.** 분산 권한·Relay·보안·압축·씬 전환 확장까지 실행 검증했다고 읽지 않습니다. 프로젝트 파일이나 기존 패키지 설정은 바꾸지 않았습니다.

## 관련 문서

- [NGO 기본 구조와 컨셉](NGO_ARCHITECTURE_AND_CONCEPTS.md)
- [DAY09: Host·Client 연결과 캐릭터 이동](../DAY09_NGO_NETWORK_MANAGER_HOST_CLIENT.md)
- [DAY10: 소유권, NetworkVariable, RPC](../DAY10_NGO_OWNERSHIP_NETWORKVARIABLE_RPC.md)
- [NGO 검증 프로젝트와 실행 방법](../../../../Code/GameNetwork/NGO/README.md)
