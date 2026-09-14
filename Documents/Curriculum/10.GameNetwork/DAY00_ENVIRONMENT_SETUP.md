# DAY 00: 환경 설정과 첫 연결 확인

이 문서는 90시간 본수업 전에 진행하는 사전 실습입니다. 뒤의 DAY01~DAY15를 시작하기 전에 같은 Unity 6·NGO 환경에서 Host를 실행할 수 있어야 합니다.

## 오늘의 목표

- Unity 6 프로젝트와 콘솔 개발 환경을 준비합니다.
- NGO, Unity Transport, Multiplayer Play Mode, UniTask를 수업 지정 버전으로 설치합니다.
- Host를 시작해 Player NetworkObject가 생성되는지 확인합니다.

## 안내형 실습: 공통 시작 프로젝트 만들기

**미션:** `GameNetworkLab` 프로젝트에서 Host 시작까지 확인합니다.

1. [환경 설정 안내](ENVIRONMENT_SETUP_GUIDE.md)의 **1. 기준 환경**에서 Unity Editor, IDE, .NET SDK 버전을 확인합니다.
2. 같은 문서의 **2. Unity 프로젝트 만들기** 순서대로 `GameNetworkLab`과 `NetworkPlay` 씬을 만듭니다.
3. **3. 필수 패키지 설치** 순서대로 NGO, Unity Transport, Multiplayer Play Mode, UniTask를 설치합니다.
4. **4. 첫 연결 확인** 순서대로 NetworkManager와 Player Prefab을 구성합니다.
5. Play Mode에서 Host를 시작합니다.

### 완료 확인

- [ ] Console에 빨간 Error가 없다.
- [ ] NetworkManager에 Unity Transport가 있다.
- [ ] Player Prefab에 NetworkObject가 있다.
- [ ] Host 시작 뒤 Hierarchy에 Player 복제본이 생성된다.

## 응용 실습: 환경 확인 기록

아래 표를 채워 교사에게 보여 주세요. 값이 비어 있거나 친구와 다르면 DAY01 전에 원인을 확인합니다.

| 항목 | 내 환경 값 | 확인 완료 |
| :--- | :--- | :---: |
| Unity 6 Editor 버전 |  |  |
| .NET SDK 버전 |  |  |
| NGO 버전 |  |  |
| Unity Transport 버전 |  |  |
| UniTask 버전 |  |  |

## 오늘의 정리

- 네트워크 실습은 모든 참여자가 같은 시작 환경을 써야 결과를 비교할 수 있습니다.
- 다음 DAY부터는 네트워크 대기를 이해하기 위한 비동기 처리 기초를 배웁니다.
