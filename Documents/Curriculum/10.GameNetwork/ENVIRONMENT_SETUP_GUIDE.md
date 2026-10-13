# 게임 네트워크 프로그래밍 환경 설정 안내

이 문서는 DAY00에서 모든 학생이 동일한 시작 상태를 만드는 안내서입니다. DAY01 전에 완료합니다.

## 1. 기준 환경

| 항목 | 기준 | 용도 |
| :--- | :--- | :--- |
| Unity Editor | 수업 지정 Unity 6 버전 | NGO 실습 |
| 템플릿 | 3D Core | 네트워크 플레이 씬 |
| IDE | Visual Studio 2022 이상 또는 Rider | C#·Unity 디버깅 |
| .NET SDK | 수업 지정 .NET SDK | 콘솔 서버·클라이언트 |
| NGO | 2.13.3 — 수업 지정 Unity 6 Editor와의 호환성 확인 | `NetworkManager`, `NetworkObject` |
| Unity Transport | NGO와 호환되는 검증 버전 | 로컬 전송 |
| Multiplayer Play Mode | Unity 6 호환 검증 버전 | 한 PC 다중 플레이어 확인 |
| Input System | Unity 6 호환 검증 버전 | 플레이어 이동과 UI 입력 |
| UniTask | 수업에서 고정한 검증 버전 | Unity 비동기 작업 |

> 패키지 버전은 수업 시작일에 교사가 확정한 값을 사용합니다. 학생이 서로 다른 임의 버전을 설치하지 않습니다.

## 2. Unity 프로젝트 만들기

1. Unity Hub에서 **New project** 를 선택합니다.
2. 수업 지정 Unity 6 Editor와 `3D Core` 템플릿을 선택합니다.
3. 프로젝트 이름을 `GameNetworkLab`으로 입력하고 영문 경로에 생성합니다.
4. 프로젝트를 연 뒤 `Window > General > Console`에서 빨간 Error가 없는지 확인합니다.
5. `Assets/Scenes` 폴더를 만들고 현재 씬을 `NetworkPlay`로 저장합니다.

## 3. 필수 패키지 설치

1. `Window > Package Manager`를 엽니다.
2. 좌측 상단 범위를 `Unity Registry`로 선택합니다.
3. `Netcode for GameObjects`, `Unity Transport`, `Multiplayer Play Mode`, `Input System`을 차례로 찾아 설치합니다.
4. UniTask는 교사가 제공한 고정 UPM Git URL 또는 패키지 파일로 설치합니다.
5. 설치가 끝나면 Console의 Error를 다시 확인합니다.

### 이 단어는 무슨 뜻인가요?

- **NGO (Netcode for GameObjects)**: Unity의 GameObject와 MonoBehaviour에 네트워크 기능을 붙이는 Unity 패키지입니다.
- **Transport (전송 계층)**: 게임 데이터가 Host와 Client 사이를 오가는 통로입니다.
- **UPM (Unity Package Manager)**: Unity 패키지를 설치하고 버전을 관리하는 도구입니다.

## 4. 첫 연결 확인

1. Hierarchy 빈 곳에서 `Create Empty`를 선택하고 이름을 `NetworkManager`로 바꿉니다.
2. Inspector의 **Add Component** 에서 `NetworkManager`와 `Unity Transport`를 차례로 추가합니다.
3. NetworkManager 컴포넌트의 **Network Transport** 칸에 같은 오브젝트의 `Unity Transport` 컴포넌트를 연결합니다.
4. Unity Transport의 접속 설정에서 **Address = `127.0.0.1`**, **Port = `7777`** 을 확인합니다. Server Listen Address가 표시되면 `127.0.0.1`로 설정합니다. 이번 실습은 같은 컴퓨터에서만 연결합니다. 표시 이름은 패키지 버전에 따라 다를 수 있습니다.
5. [DAY09의 플레이어 프리팹과 이동 동기화 만들기](DAY09_NGO_NETWORK_MANAGER_HOST_CLIENT.md#플레이어-프리팹과-이동-동기화-만들기)를 따라 바닥·카메라·Input System을 구성합니다. 플레이어는 Cube가 아니라 캡슐 외형과 CharacterController를 가진 `PlayerNetworkPrefab`입니다.
6. 같은 절차대로 프리팹에 `NetworkObject`, `CharacterController`, 일반 `NetworkTransform`, `NetworkCharacterPlayer`를 추가합니다. NetworkTransform의 **Authority Mode를 Owner** 로 설정합니다. 자식 CapsuleCollider와 씬에 남은 원본 Player는 제거합니다. 이전 OwnerNetworkTransform과 일반 NetworkTransform을 함께 붙이지 않습니다.
7. `NetworkManager` 오브젝트의 **Player Prefab** 칸에 새 캡슐 프리팹을 연결하고 씬을 저장합니다.
8. [DAY09의 안내형 실습](DAY09_NGO_NETWORK_MANAGER_HOST_CLIENT.md#2-안내형-실습-host와-client-시작-버튼)을 미리 따라 Host·Client 버튼과 `NetworkLauncher.cs`를 구성합니다. 제공되지 않은 임시 스크립트에 의존하지 않습니다. 코드도 시작 전에 주소와 포트를 명시합니다.
9. DAY09의 **두 실행 창에서 실제 연결하기** 절차대로 Editor에서 Host, 빌드한 실행 파일에서 Client를 시작합니다. Host의 참여자 수가 2명, Client가 연결 완료인지 확인하고, 두 창을 번갈아 조작하여 이동이 상대 창에도 반영되는지 확인합니다.

**IP (Internet Protocol) 주소** 는 연결할 컴퓨터를 가리키며, `127.0.0.1`은 자기 컴퓨터를 뜻하는 루프백(Loopback) 주소입니다. **포트 (Port)** 는 그 컴퓨터에서 통신 프로그램을 구분하는 번호입니다. Unity Transport는 이 주소·포트로 접속하며, 다른 Host를 자동으로 찾는 것은 아닙니다.

다른 컴퓨터의 Host로 접속하려면 목적지 IP 주소와 서버 수신 설정, 방화벽을 별도로 구성해야 합니다. 이 로컬 실습의 주소를 그대로 사용하면 다른 컴퓨터에는 접속할 수 없습니다.

### 완료 확인

- [ ] Console에 패키지 또는 컴파일 Error가 없다.
- [ ] `NetworkManager`에 `Unity Transport`가 연결되어 있다.
- [ ] Unity Transport의 주소·포트가 `127.0.0.1:7777`이다.
- [ ] Player Prefab에 `NetworkObject`가 있다.
- [ ] Host를 시작하면 Hierarchy에 Player 복제본이 생긴다.
- [ ] 별도 실행 파일의 Client가 연결 완료되고 Host 참여자 수가 2명이 된다.
- [ ] 두 창에서 자신의 캐릭터만 조작되며 상대 창에도 이동이 보인다.

## 5. 시작 전 5분 점검표

| 확인 항목 | 완료 |
| :--- | :---: |
| Unity 6과 .NET SDK가 수업 지정 버전이다. |  |
| NGO, Unity Transport, Multiplayer Play Mode, UniTask 버전이 교사 기준과 같다. |  |
| `NetworkPlay` 씬이 열리고 Console Error가 없다. |  |
| Editor Host와 실행 파일 Client를 연결하고 참여자 2명을 확인했다. |  |
| 캡슐 플레이어의 이동과 바닥·벽 충돌을 두 창에서 확인했다. |  |
| 방화벽 확인이 필요한 포트와 로컬 테스트 포트를 기록했다. |  |

