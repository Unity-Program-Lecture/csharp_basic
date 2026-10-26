# 게임플랫폼 응용 프로그래밍: 하나의 게임을 완성하는 16일 과정

`PlatformCoinLab` 하나를 처음부터 끝까지 이어 만듭니다. 전날 프로젝트를 열어 기능을 추가하고, 매일 시작→플레이→결과→재도전 흐름을 확인합니다. 완성한 게임은 PC·Android·Web에 배포하고 설계·오류 해결·최적화 기록을 함께 남깁니다. iOS는 참고 학습만 합니다.

## 기준 환경과 시간

- Unity 6.6 `6000.6.7f1`, Input System 1.20.1, Addressables 2.11.2, UGUI·TextMeshPro를 기준으로 합니다. 설치한 정확한 버전을 기록합니다.
- 16일 × 6시간 = **96시간**이며 추가 **4시간**은 빌드 오류 보완과 같은 조건의 재측정에 사용합니다. 공식 시간 기준은 [교육 일정 요약](../../COURSE_SCHEDULE_SUMMARY.md)입니다.
- C# 클래스·컬렉션과 Unity Scene·Component·Prefab·기초 UI를 선수학습으로 합니다.
- 2일차에 게임 진행 기록 설계와 한 판의 흐름을 함께 구현합니다. 7일차는 로컬 콘텐츠 빌드, 8일차는 HTTPS 파일 서버, 9일차는 원격 콘텐츠 배포와 오류 복구를 진행합니다.
- 첫날의 다운로드·에뮬레이터 이미지 설치는 사전 준비입니다. 실기기 미확보·설치 지연은 미확인으로 기록하고 보완 시간을 배정합니다.

## 일차별 누적 결과

| 일차 | 학습 문서 | NCS 목표 | 그날 완성할 결과 |
| :--- | :--- | :--- | :--- |
| 01 | [플랫폼 요구 사항과 개발 환경](DAY01_REQUIREMENTS_ENVIRONMENT.md) | G1·G2·G3 | 요구 사항 표·버전 기록·빈 씬과 빌드 프로필 |
| 02 | [게임 진행 기록과 한 판의 흐름 구현](DAY02_GAME_DATA_AND_FLOW.md) | G2·G4·G5·G6·G7·G8 | 게임 진행 기록 설계와 시작→진행→결과→재도전 |
| 03 | [키보드·게임패드·터치 입력](DAY03_INPUT_APPLICATION.md) | G5·G7 | 같은 게임의 키보드·게임패드·터치 조작 |
| 04 | [최고 점수 저장과 화면 대응](DAY04_SCREEN_SAVE_LIFECYCLE.md) | G4·G7·G8 | 최고 점수 표시·저장·Safe Area |
| 05 | [플랫폼 코드와 앱 생명주기](DAY05_EDITOR_PLAYER_CODE.md) | G6·G7·G8 | 종료 버튼·Editor 도구·중단/복귀 |
| 06 | [Addressables로 스테이지 준비하기](DAY06_ADDRESSABLES_RUNTIME.md) | G2·G7·G8 | 스테이지 로드·실패 안내·재시도 |
| 07 | [로컬 콘텐츠 빌드와 Player 연결](DAY07_ADDRESSABLES_LOCAL_BUILD.md) | G3·G7·G8 | 서버 없이 로컬 Player에서 한 판 완료 |
| 08 | [ASP.NET Core HTTPS 파일 서버](DAY08_HTTPS_CONTENT_SERVER.md) | G3·G7·G8 | HTTPS 상태·JSON·파일 응답과 404·연결 실패 구분 |
| 09 | [원격 콘텐츠 배포와 로드 오류 해결](DAY09_ADDRESSABLES_REMOTE_DELIVERY.md) | G3·G7·G8 | 원격 로드 실패 재현·서버 복구·재시도 기록 |
| 10 | [PC 빌드, 로그, 배포](DAY10_PC_BUILD_DEBUG.md) | G3·G7·G8 | 별도 폴더에서 PC 일반 빌드 실행 |
| 11 | [Android 빌드와 실기기 디버깅](DAY11_ANDROID_BUILD_DEBUG.md) | G3·G7·G8 | Android 설치·입력·로그와 장치별 기록 |
| 12 | [Web 빌드와 브라우저 디버깅](DAY12_WEB_BUILD_DEBUG.md) | G3·G7·G8 | HTTP로 실행한 Web과 브라우저 조사 기록 |
| 13 | [설정 파일을 다음 라운드에 적용하기](DAY13_PLATFORM_IO_NETWORK.md) | G4·G6·G7·G8 | 45초 설정 반영·실패 시 30초·시작 잠금 |
| 14 | [실제 게임 UI의 프로파일 분석과 개선](DAY14_PROFILING_ALGORITHM.md) | G6·G8·G9 | 실제 HUD 개선 전후 프로파일·기능 유지 |
| 15 | [플랫폼별 성능과 콘텐츠 최적화](DAY15_PLATFORM_OPTIMIZATION.md) | G7·G8·G9 | 플랫폼별 개선·최종 후보 빌드 |
| 16 | [통합 테스트와 프로젝트 인수인계](DAY16_INTEGRATION_HANDOVER.md) | G1~G9 | 복원 가능한 프로젝트·빌드·G1~G9 증거 |

## 최종 게임의 흐름

`메뉴 → 스테이지 준비 → Start → 이동·코인 수집 → 시간 종료 → 결과·최고 점수 → Restart 또는 메뉴`

코인 10개를 모으는 작은 평면 게임입니다. 제한 시간 기본값은 30초이며, DAY 13부터 메뉴에서 설정을 읽어 다음 라운드의 시간을 바꿀 수 있습니다. 로드 실패 때는 시작을 막고 안내와 재시도를 제공합니다. 결과·명시적 저장·앱 중단 시 최고 점수를 보관합니다. 상점·로그인·온라인 순위표·스토어 출시는 포함하지 않습니다.

## 자료를 사용하는 순서

1. [DAY 01](DAY01_REQUIREMENTS_ENVIRONMENT.md)에서 Editor·모듈을 준비하고 자신의 프로젝트·첫 씬·빌드 프로필을 만듭니다.
2. 매일 문서의 시작 상태를 확인한 뒤 작업 파일을 수정합니다. 새 클래스를 무조건 추가하는 대신 기존 클래스를 수정하는 날도 있습니다.
3. 완료 확인에서 새 기능과 기존 흐름을 함께 시험합니다.
4. [실습 기록](PRACTICE_REPORT_TEMPLATE.md)을 그날 작성하고 복구 지점을 저장합니다. DAY 16에 몰아서 작성하지 않습니다.
5. [프로젝트 진행·복구 안내](PROJECT_PROGRESS_GUIDE.md)로 파일 추가·수정 시점과 연결 상태를 확인합니다.

필요한 설정은 해당 일차에서 진행합니다. DAY 02는 Input System·Unity UI·TextMeshPro, DAY 05는 Editor 실행 설정, DAY 06은 Addressables, DAY 11은 실제 Android 기기의 지원 조건을 확인합니다.

학생이 입력할 코드는 각 DAY 문서에 있습니다. 파일을 복사하거나 별도 프로젝트를 열지 않아도 실습을 진행할 수 있도록 문서의 절차를 따릅니다.

1일차에 Unity Hub에서 빈 Universal 3D 프로젝트를 직접 만들고, 이후에는 자신이 전날까지 만든 프로젝트를 이어 사용합니다. 수업에 필요한 코드·오브젝트·설정·Inspector 연결은 각 DAY 문서에 설명합니다. 완성된 Unity 프로젝트나 교사용 구성 도구를 준비할 필요는 없습니다.

기본 입력은 WASD·방향키·게임패드와 화면 방향 버튼 네 개이며 터치 스틱은 선택 실습입니다. 자신의 소스·씬·설정·실행 결과를 매일 저장하고 [진행·복구 안내](PROJECT_PROGRESS_GUIDE.md)에 따라 복구 지점을 남깁니다.

## 플랫폼과 심화 비교 범위

Windows는 PC 필수 실행 환경입니다. Android 에뮬레이터는 설치·실행 연습에 쓰되 실기기 터치·발열·성능을 대신하지 않습니다. Web은 데스크톱 브라우저를 기준으로 합니다. [iOS 참고 자료](IOS_REFERENCE.md)는 비교용이며 실습·실행·제출 대상에서 제외합니다.

기본 프로젝트는 로컬 AssetBundle 콘텐츠로 복원할 수 있게 유지합니다. DAY 09의 원격 비교는 별도 빌드·설정 기록으로 보관합니다. 콘텐츠 디렉토리, WebGPU, WebAssembly64 비교는 필수 완성 후의 보충 학습이며 최종 게임의 필수 기능으로 늘리지 않습니다.

[NCS 목표 연결표](NCS_ALIGNMENT.md)는 G1~G9가 프로젝트의 어느 부분과 기록으로 연결되는지 안내합니다. [공식 문서와 검증 범위](REFERENCES.md)에서 문서 확인과 실제 Player 검증의 차이를 확인합니다.
