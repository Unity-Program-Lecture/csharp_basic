# DAY 15: 네트워크 게임 통합과 최종 점검

오늘은 서버 설계, 콘솔 통신, NGO 동기화, 비동기 UI, 테스트 기록을 하나의 작은 협동 게임으로 연결합니다.

## 1. 핵심 개념: "각 부품이 약속대로 연결된 작은 게임"

- **통합 (Integration)**: 따로 만들던 모듈을 함께 실행해 전체 흐름을 확인하는 작업입니다.
- **회귀 확인 (Regression Check)**: 새 기능을 넣은 뒤 기존에 되던 기능이 다시 망가지지 않았는지 확인하는 작업입니다.
- **완료 조건 (Acceptance Criteria)**: 기능이 완성됐다고 판단하기 위한 관찰 가능한 기준입니다.

## 2. 안내형 실습: 2인 코인 협동 게임 최종 실행

**미션:** 두 Player가 접속해 코인을 수집하고, Server가 결과를 동기화하는 게임을 실행합니다.

1. `NetworkPlay` 씬에서 NetworkManager, Player Prefab, Coin NetworkObject, Canvas, EventSystem을 확인합니다.
2. Player Prefab에 `NetworkObject`, `NetworkScorePlayer`, Collider가 있는지 확인합니다.
3. Scene의 Coin에 `NetworkObject`, Trigger Collider, Rigidbody, `NetworkCoin`이 있는지 확인합니다.
4. Host와 Client를 시작합니다.
5. 두 창에서 Player가 생성됐는지 확인합니다.
6. 한 Player로 Coin을 획득하고, 두 창에서 Coin 제거와 Score 변경을 확인합니다.
7. Client를 종료한 뒤 Host가 계속 실행되는지 확인합니다.
8. 테스트 결과를 DAY14 표에 기록합니다.

### 최종 완료 확인

- [ ] Host/Client가 접속하고 Player NetworkObject가 생성된다.
- [ ] Client 요청에 대해 Server가 점수 또는 코인 획득을 판정한다.
- [ ] NetworkVariable 또는 RPC 결과가 두 창에 동기화된다.
- [ ] 접속 중 UI가 중복 클릭을 막고 상태를 표시한다.
- [ ] 접속 종료·재연결 또는 오류 한 가지를 재현·기록했다.
- [ ] 학생이 `입력 요청 → Server 검증 → 상태 동기화 → UI 표시` 흐름을 설명할 수 있다.

## 응용 실습: 협동 목표 추가

코인 3개를 모으면 `게임 성공`을 표시하는 공유 목표를 추가하세요. 목표 완료가 여러 번 실행되지 않아야 하며, 양쪽 창에서 같은 결과를 보여야 합니다.

## 오늘의 정리

- 네트워크 게임은 연결, 패킷, 권한, 동기화, UI, 테스트가 함께 맞아야 완성됩니다.
- 다음 단계는 측정 결과를 바탕으로 더 많은 참여자와 더 복잡한 서버 구조를 설계하는 일입니다.
