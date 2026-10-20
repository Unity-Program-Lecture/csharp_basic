# PlatformCoinLab 프로젝트 진행과 복구 안내

## 매일 자신이 만든 프로젝트 이어가기

DAY 01에 Unity Hub에서 Universal 3D 템플릿으로 PlatformCoinLab을 만듭니다. DAY 02부터는 이 프로젝트를 열어 전날까지 작성한 코드와 저장한 씬에 기능을 추가합니다. 완성본 다운로드·교사 브랜치 전환·자동 씬 구성 명령은 수업의 준비 조건이 아닙니다.

각 DAY의 시작 상태를 보고 전날 결과가 동작하는지 확인한 뒤 새 작업을 시작합니다. 빠진 부분이 있으면 전날 문서의 코드와 연결 표를 따라 고칩니다. 첫날에는 C# 스크립트를 추가하지 않고 환경·배경·카메라·빌드 프로필을 준비합니다. 첫 수업 코드는 DAY 02의 CoinRound입니다.

| 일차 | 전날에서 바뀌는 부분 | 복구 후 확인할 결과 |
| :--- | :--- | :--- |
| [01](DAY01_REQUIREMENTS_ENVIRONMENT.md) | 플랫폼 요구 사항과 개발 환경 | 요구 사항 표·버전 기록·빈 씬과 빌드 프로필 |
| [02](DAY02_GAME_DATA_AND_FLOW.md) | 게임 진행 기록과 한 판의 흐름 구현 | 게임 진행 기록 설계와 시작→진행→결과→재도전 |
| [03](DAY03_INPUT_APPLICATION.md) | 키보드·게임패드·터치 입력 | 같은 게임의 키보드·게임패드·터치 조작 |
| [04](DAY04_SCREEN_SAVE_LIFECYCLE.md) | 최고 점수 저장과 화면 대응 | 최고 점수 표시·저장·Safe Area |
| [05](DAY05_EDITOR_PLAYER_CODE.md) | 플랫폼 코드와 앱 생명주기 | 종료 버튼·Editor 도구·중단/복귀 |
| [06](DAY06_ADDRESSABLES_RUNTIME.md) | Addressables로 스테이지 준비하기 | 스테이지 로드·실패 안내·재시도 |
| [07](DAY07_ADDRESSABLES_LOCAL_BUILD.md) | 로컬 콘텐츠 빌드와 Player 연결 | 서버 없이 로컬 Player에서 한 판 완료 |
| [08](DAY08_HTTPS_CONTENT_SERVER.md) | ASP.NET Core HTTPS 파일 서버 | HTTPS 상태·JSON·파일 응답과 404·연결 실패 구분 |
| [09](DAY09_ADDRESSABLES_REMOTE_DELIVERY.md) | 원격 콘텐츠 배포와 로드 오류 해결 | 원격 로드 실패 재현·서버 복구·재시도 기록 |
| [10](DAY10_PC_BUILD_DEBUG.md) | PC 빌드, 로그, 배포 | 별도 폴더에서 PC 일반 빌드 실행 |
| [11](DAY11_ANDROID_BUILD_DEBUG.md) | Android 빌드와 실기기 디버깅 | Android 설치·입력·로그와 장치별 기록 |
| [12](DAY12_WEB_BUILD_DEBUG.md) | Web 빌드와 브라우저 디버깅 | HTTP로 실행한 Web과 브라우저 조사 기록 |
| [13](DAY13_PLATFORM_IO_NETWORK.md) | 설정 파일을 다음 라운드에 적용하기 | 45초 설정 반영·실패 시 30초·시작 잠금 |
| [14](DAY14_PROFILING_ALGORITHM.md) | 실제 게임 UI의 프로파일 분석과 개선 | 실제 HUD 개선 전후 프로파일·기능 유지 |
| [15](DAY15_PLATFORM_OPTIMIZATION.md) | 플랫폼별 성능과 콘텐츠 최적화 | 플랫폼별 개선·최종 후보 빌드 |
| [16](DAY16_INTEGRATION_HANDOVER.md) | 통합 테스트와 프로젝트 인수인계 | 복원 가능한 프로젝트·빌드·G1~G9 증거 |

## 파일 추가와 기존 코드 수정

- DAY 02: CoinRound·GameSession·CoinPickup을 작성하고 Player·코인·기본 메뉴·HUD·결과 UI를 직접 만듭니다.
- DAY 03: PlatformMover·PlatformInfo와 입력 자산·MobileControls·화면 방향 버튼을 만듭니다.
- DAY 04: 같은 GameSession에 저장 기능을 추가하고 BestText·SaveButton과 SafeAreaFitter를 만듭니다.
- DAY 05: QuitButton·ExitGame과 Editor 전용 PlatformDiagnostics를 만듭니다. 이 날 처음 Editor용 코드를 추가합니다.
- DAY 06: 코인을 StageMain 프리팹으로 옮기고 StageLoader·ReloadStageButton을 만듭니다.
- DAY 07: 로컬 콘텐츠와 Windows Player를 빌드합니다.
- DAY 08: Unity 밖에서 ContentServer를 만들고 HTTPS 상태·JSON·파일 응답을 확인합니다.
- DAY 09: 원격 콘텐츠를 빌드해 같은 서버에 배포하고 실패·복구를 확인합니다.
- DAY 10~12: 플랫폼별 빌드 프로필·실행·디버깅을 확인합니다.
- DAY 13: JSON 설정 파일·PlatformConfigLoader·LoadSettingsButton을 만듭니다.
- DAY 14: 기존 GameSession.RefreshUi에 마커와 표시 값 캐시를 추가합니다.
- DAY 15: FramePolicy를 작성하고 화면 Anchor와 플랫폼별 설정을 보완합니다.
- DAY 16: 자신의 최종 버전으로 빌드·복원·인수인계를 확인합니다.

같은 GameSession 클래스를 새 파일로 계속 추가하지 않습니다. 문서에서 지정한 필드·메서드를 기존 파일에 추가하거나 교체합니다. 작업 중 Inspector 참조와 버튼 이벤트도 직접 확인합니다.

## 저장할 복구 지점

문서의 Progress/Day01~Day16은 실습 기록의 구분 이름입니다. Unity가 자동 생성하는 폴더가 아닙니다. 그날의 수정 파일·씬·설정·실행 환경·알려진 문제와 커밋 해시 또는 백업 위치를 기록합니다.

Git을 사용한다면 자신의 저장소에 커밋합니다. Git을 사용하지 않는다면 Unity를 닫은 뒤 Assets와 meta·Packages·ProjectSettings를 함께 백업합니다. Library·Temp·생성 번들·빌드 출력은 원본과 구분하며, 서명 키·비밀번호·개인 장치 식별 정보는 제출하지 않습니다.

## 최종 흐름과 연결

| 책임 | 클래스·설정 |
| :--- | :--- |
| 진행 기록·중복 수집·시간 | CoinRound |
| 메뉴·플레이·결과·최고 점수·시작 잠금 | GameSession |
| 입력·이동 | PlatformMover·PlatformControls |
| 코인 접촉 요청 | CoinPickup |
| 스테이지 준비·재시도·해제 | StageLoader |
| 다음 라운드 설정·실패 시 기본값 | PlatformConfigLoader |
| 화면·플랫폼 표시·종료·프레임 정책 | SafeAreaFitter·PlatformInfo·ExitGame·FramePolicy |
| HTTPS 파일 응답 | 별도 ASP.NET Core ContentServer |

GameSession과 로더는 숨기는 화면 패널 밖에 둡니다. StageMain에는 코인만 포함하며 Session과 Player는 로드 후 전달받습니다. 기본 완성본은 로컬 콘텐츠로 서버 없이 실행합니다. 원격 실습은 설정과 결과를 기록한 뒤 로컬 경로로 복원합니다.

## 매일 확인할 흐름

DAY 02부터 메뉴 → Start → 수집 → 시간 종료 → 결과 → Restart → 메뉴를 확인합니다. 새 기능을 추가한 날에는 최고 점수·로딩 실패·설정 실패도 확인합니다. 조작할 수 없는 기기나 실행하지 않은 항목은 미확인으로 남깁니다.

DAY 10~12에는 플랫폼별 빌드·로그를 기록하고 DAY 14~15에는 같은 조건의 실제 Player 측정을 남깁니다. DAY 16에는 다른 폴더에 자신의 프로젝트를 복원해 추가 설명 없이 실행·수정할 수 있는지 확인합니다.
