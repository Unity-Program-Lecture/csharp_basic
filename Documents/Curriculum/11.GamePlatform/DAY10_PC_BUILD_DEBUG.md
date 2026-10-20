# DAY 10: PC 빌드, 로그, 배포

오늘은 Windows Player를 프로젝트와 분리된 폴더에서 실행해 봅니다. Mono와 IL2CPP, 개발 빌드와 일반 빌드의 차이도 비교합니다. 이 활동은 NCS 학습 목표 G3·G7·G8에 해당합니다.

## 1. 핵심 개념: 작업대 밖에서도 조립품이 돌아가는가

개발 중에는 프로젝트 폴더의 파일을 읽어 정상 동작하던 코드가, 배포 후에는 그 파일을 찾지 못할 수 있습니다. 이를 확인하기 위해 PC 빌드 결과물 전체를 프로젝트와 떨어진 폴더로 옮겨 실행합니다.

### 이 단어는 무슨 뜻인가요?

- **Scripting Backend**: C# 코드가 실행될 형태를 만드는 방식입니다.
- **Player Log**: Editor 밖에서 실행한 앱의 로그입니다.
- **Script Debugging**: 개발 Player에 디버거를 연결하기 위한 설정입니다.
- **Build Report**: 빌드 시간·크기·포함 자료를 확인하는 결과 정보입니다.

## 2. 실습: 개발용 Windows Player

**미션:** Editor와 분리한 상태에서 기능을 검증합니다.

1. Windows 프로필, Scene List, x86-64 대상, Development Build를 확인합니다.
2. Scripting Backend를 Mono로 설정하고 Groups에서 Default 프로필·CourseStage의 로컬 경로를 확인합니다. Windows용 Addressables 콘텐츠를 먼저 빌드한 뒤 Player를 `Builds/Windows/Development`에 빌드합니다.
3. 출력 폴더 전체를 복사합니다. exe만 옮기지 말고 Data 폴더와 생성된 의존 파일을 포함합니다.
4. Unity 프로젝트를 열지 않은 상태에서 복사한 Player를 실행합니다.
5. 이동·코인 10개·30초 종료·재시작·저장·스테이지 로드·효과음을 확인합니다.
6. 창 크기와 포커스를 바꾸어 HUD·입력·타이머를 기록합니다.

Windows Player 로그의 일반 경로는 `%USERPROFILE%/AppData/LocalLow/<CompanyName>/<ProductName>/Player.log`다. Company/Product 설정이 다르면 폴더도 달라집니다. 여러 실행 결과를 혼동하지 않도록 실행 직후 복사하고 파일명을 붙입니다.

### 실행해보면

`Application.platform`은 Windows Player에서 실행 중임을 나타냅니다. Editor 전용 메뉴는 포함되지 않으며, QuitButton을 누르면 앱이 종료됩니다. 스테이지 프리팹이 보이지 않으면 주소, 콘텐츠 빌드 결과, 카탈로그를 차례로 확인합니다.

## 3. 실습: 디버거와 IL2CPP 비교

개발 빌드에 Script Debugging을 켭니다. 사용하는 C# IDE의 Unity 디버거에서 해당 Player를 선택하고 TryCollect에 중단점을 둡니다. 디버거 연결이 안 되면 포트·방화벽·개발 빌드 여부를 확인합니다. 중단점에서 멈춘 시간은 성능 측정에 포함하지 않습니다.

이후 IL2CPP 모듈과 C++ 도구가 준비된 환경에서 IL2CPP로 빌드합니다. C# 규칙이 같은지 확인하고, 빌드 시간·크기·로그를 비교합니다. 설치되지 않았다면 요구 도구와 실패 로그를 남기고 미검증으로 표시합니다. 문자열 리플렉션·런타임 코드 생성은 AOT/스트리핑에서 다르게 동작할 수 있으므로 실제 사용 기능별로 확인합니다.

## 4. 일반 빌드 재확인

Development Build와 Script Debugging을 끄고 새 폴더에 빌드합니다. 개발 옵션의 비용이 제거된 조건에서 같은 기능을 다시 검증합니다. 성능 비교는 개발 빌드끼리 또는 일반 빌드끼리 맞추고 서로 다른 조건을 섞지 않습니다.

## 프로젝트 완료 확인

- [ ] PC 출력 전체를 다른 폴더에서 실행합니다.
- [ ] 개발·일반 빌드의 같은 기능과 Player 로그를 확인했습니다.

오늘의 프로젝트를 `Progress/Day10`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. exe만 전달했을 때 Addressables 외에도 어떤 자료가 빠질 수 있나요?
2. 디버거를 연결한 채 수집한 프레임 시간은 왜 일반 실행과 구분해야 하나요?

## 오늘의 정리

PC 빌드는 별도 폴더에서 실행하고 로그와 게임 기능까지 확인해야 배포할 준비가 되었는지 판단할 수 있습니다. 다음 수업에서는 같은 게임을 Android 실기기에서 실행하며 차이를 살펴봅니다.
