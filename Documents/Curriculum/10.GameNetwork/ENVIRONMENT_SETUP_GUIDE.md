# 게임 네트워크 프로그래밍 환경 설정 안내

이 문서는 DAY00에서 모든 학생이 동일한 시작 상태를 만드는 안내서입니다. DAY01 전에 완료합니다.

## 1. 기준 환경

| 항목 | 기준 | 용도 |
| :--- | :--- | :--- |
| Unity Editor | 수업 지정 Unity 6 버전 | NGO 실습 |
| 템플릿 | 3D Core | 네트워크 플레이 씬 |
| IDE | Visual Studio 2022 이상 또는 Rider | C#·Unity 디버깅 |
| .NET SDK | 수업 지정 .NET SDK | 콘솔 서버·클라이언트 |
| NGO | Unity 6 Editor와 호환되는 검증 버전 | `NetworkManager`, `NetworkObject` |
| Unity Transport | NGO와 호환되는 검증 버전 | 로컬 전송 |
| Multiplayer Play Mode | Unity 6 호환 검증 버전 | 한 PC 다중 플레이어 확인 |
| UniTask | 수업에서 고정한 검증 버전 | Unity 비동기 작업 |

> 패키지 버전은 수업 시작일에 교사가 확정한 값을 사용합니다. 학생이 서로 다른 임의 버전을 설치하지 않습니다.

## 2. Unity 프로젝트 만들기

1. Unity Hub에서 **New project**를 선택합니다.
2. 수업 지정 Unity 6 Editor와 `3D Core` 템플릿을 선택합니다.
3. 프로젝트 이름을 `GameNetworkLab`으로 입력하고 영문 경로에 생성합니다.
4. 프로젝트를 연 뒤 `Window > General > Console`에서 빨간 Error가 없는지 확인합니다.
5. `Assets/Scenes` 폴더를 만들고 현재 씬을 `NetworkPlay`로 저장합니다.

## 3. 필수 패키지 설치

1. `Window > Package Manager`를 엽니다.
2. 좌측 상단 범위를 `Unity Registry`로 선택합니다.
3. `Netcode for GameObjects`, `Unity Transport`, `Multiplayer Play Mode`를 차례로 찾아 설치합니다.
4. UniTask는 교사가 제공한 고정 UPM Git URL 또는 패키지 파일로 설치합니다.
5. 설치가 끝나면 Console의 Error를 다시 확인합니다.

### 이 단어는 무슨 뜻인가요?

- **NGO (Netcode for GameObjects)**: Unity의 GameObject와 MonoBehaviour에 네트워크 기능을 붙이는 Unity 패키지입니다.
- **Transport (전송 계층)**: 게임 데이터가 Host와 Client 사이를 오가는 통로입니다.
- **UPM (Unity Package Manager)**: Unity 패키지를 설치하고 버전을 관리하는 도구입니다.

## 4. 첫 연결 확인

1. Hierarchy 빈 곳에서 `Create Empty`를 선택하고 이름을 `NetworkManager`로 바꿉니다.
2. Inspector의 **Add Component**에서 `NetworkManager`와 `Unity Transport`를 차례로 추가합니다.
3. `Assets/Prefabs` 폴더를 만들고 Cube를 끌어 넣어 `PlayerNetworkPrefab`으로 저장합니다.
4. Prefab을 열어 `NetworkObject` 컴포넌트를 추가합니다.
5. `NetworkManager` 오브젝트의 **Player Prefab** 칸에 `PlayerNetworkPrefab`을 끌어 놓습니다.
6. Play Mode에서 Host를 시작할 수 있는 임시 버튼 또는 교사가 제공한 시작 스크립트를 사용합니다.

### 완료 확인

- [ ] Console에 패키지 또는 컴파일 Error가 없다.
- [ ] `NetworkManager`에 `Unity Transport`가 연결되어 있다.
- [ ] Player Prefab에 `NetworkObject`가 있다.
- [ ] Host를 시작하면 Hierarchy에 Player 복제본이 생긴다.

## 5. 시작 전 5분 점검표

| 확인 항목 | 완료 |
| :--- | :---: |
| Unity 6과 .NET SDK가 수업 지정 버전이다. |  |
| NGO, Unity Transport, Multiplayer Play Mode, UniTask 버전이 교사 기준과 같다. |  |
| `NetworkPlay` 씬이 열리고 Console Error가 없다. |  |
| Host를 시작해 Player Prefab 생성까지 확인했다. |  |
| 방화벽 확인이 필요한 포트와 로컬 테스트 포트를 기록했다. |  |

