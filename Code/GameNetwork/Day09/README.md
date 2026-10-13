# 9일차 Unity 네트워크 캐릭터 실습 스크립트

이 폴더는 콘솔 프로젝트가 아니라 Unity 프로젝트에 복사할 C# 스크립트 모음입니다. 실제 씬·프리팹 생성, Input System 설정, 컴포넌트 등록, 두 실행 창 연결 절차는 [9일차 학습 문서](../../../Documents/Curriculum/10.GameNetwork/DAY09_NGO_NETWORK_MANAGER_HOST_CLIENT.md)를 따릅니다.

두 파일을 Unity 프로젝트의 `Assets/Scripts`에 복사합니다.

- `NetworkLauncher.cs`: 로컬 주소·포트를 지정하고 Host·Client를 시작하며 연결 상태를 표시합니다.
- `NetworkCharacterPlayer.cs`: 자기 캐릭터에서만 Input System 입력과 CharacterController 이동·중력을 처리합니다.

Unity 6, Netcode for GameObjects 2.13.3, 수업 지정 Unity Transport·Input System 패키지, TextMesh Pro가 필요합니다. Player Prefab에는 일반 `NetworkTransform` 하나를 추가하고 **Authority Mode = Owner** 로 설정합니다. 별도 OwnerNetworkTransform 클래스는 필요하지 않습니다. 기존 씬에서 해당 컴포넌트를 사용했다면 일반 NetworkTransform으로 교체합니다.

`NetworkCharacterPlayer`는 별도의 입력 에셋이나 PlayerInput 컴포넌트 없이 코드에서 이동 액션을 만듭니다.

대상은 같은 컴퓨터의 Host 한 명과 Client 한 명입니다. 소유자 권한 이동을 보여 주는 학습용 예제이며 서버의 이동 검증, 플레이어 간 충돌, 점프, 애니메이션은 포함하지 않습니다.

문서 코드와 파일 내용의 일치 및 공식 API를 확인했습니다. [NGO 검증 프로젝트](../NGO/README.md)에 같은 코드를 적용하여 Unity 6000.6.5f1·NGO 2.13.3에서 컴파일, 실제 두 피어 연결, 가상 키보드 입력을 통한 이동 동기화와 벽 충돌을 확인했습니다. 실제 키보드·마우스로 직접 조작하는 절차와 검증 범위는 [검증 기록](../NGO/VERIFICATION.md)을 참고합니다.
