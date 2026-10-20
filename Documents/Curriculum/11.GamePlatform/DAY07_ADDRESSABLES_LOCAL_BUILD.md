# DAY 07: 로컬 콘텐츠 빌드와 Player 연결

오늘은 DAY 06의 스테이지 로드를 실제 빌드 콘텐츠에 연결합니다. 로컬 PC Player의 한 판과 재시작을 완성하는 것이 필수 목표입니다. NCS G3·G7·G8을 다룹니다.

## 1. 핵심 개념: 앱은 매장, 콘텐츠는 물류 창고

매장과 물류 창고를 따로 준비하듯, 앱 실행 파일과 자산 번들도 각각 빌드합니다. 카탈로그는 주문 목록처럼 자산의 주소와 실제 위치를 연결합니다. 매장을 열어도 창고에 물건이 준비되지 않으면 배송할 수 없듯, Player만 빌드하고 콘텐츠를 준비하지 않으면 자산을 불러올 수 없습니다.

### 이 단어는 무슨 뜻인가요?

- **AssetBundle**: 플랫폼용으로 빌드된 자산 묶음입니다.
- **Catalog**: 주소와 로드 위치·의존성 정보를 가진 목록입니다.
- **Addressables Profile**: Build Path와 Load Path에 사용하는 변수를 모아 둔 설정입니다. Unity Build Profile과 별개입니다.
- **Content Update**: 이미 배포한 콘텐츠의 변경 사항을 관리하는 작업입니다. 최초 콘텐츠 빌드와 구분합니다.


## 2. 실습: 로컬 콘텐츠 완성

**미션:** PC에서 Asset Database를 사용하지 않고 스테이지를 불러옵니다.

1. Windows 빌드 프로필을 활성화합니다.
2. Groups에서 `stage-main` 주소의 StageMain이 CourseStage 그룹에 있는지 확인합니다. Group의 Content Packing & Loading에서 LocalBuildPath·LocalLoadPath를 확인합니다.
3. Groups의 `Build > New Build > Default Build Script`로 콘텐츠를 만듭니다. 오류와 출력 위치를 기록합니다.
4. Play Mode Script를 `Use Existing Build`로 바꾸고 Play Mode에서 로드합니다.
5. Addressables Settings의 Build Addressables on Player Build 설정을 확인합니다. 이 설정에 따라 Player 빌드 시 콘텐츠를 새로 빌드하거나 기존 콘텐츠를 사용합니다. 이번 실습에서는 Do Not Build Addressables Content on Player Build를 선택해 콘텐츠를 먼저 수동으로 빌드한 뒤 그 결과물을 사용합니다. 대상 플랫폼이나 콘텐츠를 바꾸면 수동 빌드부터 다시 합니다. 설정 이름은 설치한 패키지 버전의 화면과 비교합니다.
6. Windows Player를 빌드·실행하고 StageMain 로드를 확인합니다.
7. PC에서 확인한 순서를 기록합니다. Android·Web의 전체 실행 확인은 DAY 11·12에서 이어서 수행합니다. 준비가 빠른 경우 오늘 해당 콘텐츠 빌드까지만 추가로 확인합니다.

기본 로컬 콘텐츠는 Player 빌드에 포함되도록 구성되어 있습니다. 배포할 파일은 설치한 패키지 버전의 빌드 로그와 실제 Player 출력 폴더에서 확인합니다. `Library`의 내부 캐시 파일을 직접 복사한 것과 정식 빌드 결과물을 혼동하지 않도록 합니다.

### 실행해보면

같은 주소라도 Windows·Android·Web 자산 번들은 대상에 맞게 만들어집니다. Windows에서 만든 번들을 Android용으로 재사용하지 않습니다. `Use Existing Build`는 빌드된 콘텐츠를 쓰는 Editor 모드이고 Play Mode 지원 범위도 대상에 따라 다르므로 해당 Player 실행을 최종 근거로 삼습니다.

검사를 마치면 Play를 종료하고 Groups의 Play Mode Script를 Use Asset Database로 복원합니다. 이후 Editor에서 코드를 고칠 때는 이 모드로 확인하며, 실제 번들을 검사할 때만 Use Existing Build로 전환합니다. Player 빌드에 필요한 콘텐츠 빌드는 별도로 계속 수행합니다.


## 3. 시작부터 재도전까지 회귀 확인

시작 메뉴 → 스테이지 준비 → Start → 수집 → 결과 → Restart → 결과 → 메뉴 → Reload Stage 순서로 실행합니다. 코인이 누적되거나 재시작 후 수집이 막히지 않아야 합니다. 아직은 로컬 콘텐츠를 기준으로 하며 서버 없이 실행되어야 합니다.

### 보충: 콘텐츠 디렉토리

다음 내용은 기본 로컬 빌드를 마친 뒤 비교할 때 사용합니다. 패키지 업그레이드·세 플랫폼 재검증까지 오늘의 필수 작업에 추가하지 않습니다.

#### Unity 6.6의 다른 콘텐츠 빌드 방식

Unity 6.6에는 **콘텐츠 디렉토리 (Content Directories)**가 추가되었습니다. AssetBundle이 물건을 큰 상자로 묶어 관리하는 방식이라면, 콘텐츠 디렉토리는 메시나 텍스처 같은 데이터를 더 세밀하게 불러오고 해제하는 기반입니다. 한 콘텐츠 빌드 안에서 중복 데이터를 줄이고 증분 빌드를 활용할 수 있습니다.

Addressables는 자산을 주소로 요청하는 창구이고, AssetBundle과 콘텐츠 디렉토리는 그 아래에서 데이터를 빌드하고 읽는 방식입니다. 따라서 DAY 06에서 배운 요청 완료, 실패 처리, 소유권과 해제 개념은 계속 필요합니다.

| 비교 항목 | 이 과정의 기본 실습 | 콘텐츠 디렉토리 보충 학습 |
| :--- | :--- | :--- |
| 패키지 | Addressables 2.11.2 | Content Directory 스키마를 제공하는 Addressables 4.x |
| 콘텐츠 구성 | AssetBundle과 카탈로그 | 더 세밀한 데이터와 빌드 매니페스트 |
| 배포 | 로컬·원격 콘텐츠 실습 | 현재 기본 제공 흐름은 Player와 함께 배포하는 로컬 콘텐츠 |
| 수업 목적 | 세 플랫폼의 빌드·원격 로드 흐름 익히기 | 같은 로컬 콘텐츠의 빌드와 로딩 방식 비교 |

Unity 6.6의 Released 목록에는 Addressables 4.1.1도 있습니다. 2.11.2 역시 지원 목록에 있으므로 기본 실습을 진행할 수 있습니다. 비교 실습은 프로젝트 복사본에서 별도로 진행하고, 전환 전후의 패키지 버전과 빌드 설정을 기록합니다. 그룹의 스키마를 바꾸는 작업은 패키지 업그레이드와 함께 공식 전환 절차를 확인해야 합니다.

**비교 실습:** PC 로컬 StageMain 로드를 먼저 완성한 뒤, 복사본을 콘텐츠 디렉토리 구성으로 전환합니다. 같은 자산으로 콘텐츠와 Player를 빌드해 생성·해제 동작, 빌드 시간, 출력 크기, 첫 로드 시간을 비교합니다. 차이가 작거나 측정하지 못한 항목은 그대로 기록합니다. Android와 Web에 확대할 때는 각 플랫폼의 배포·로딩 조건을 따로 확인합니다.

콘텐츠 디렉토리는 현재 기본 원격 배포·콘텐츠 업데이트 흐름을 제공하지 않으므로 DAY 09의 Remote Group 실습을 그대로 대체하지 않습니다. 독립된 콘텐츠 빌드 사이에는 같은 자산이 중복될 수 있고, 순환 참조된 자산은 함께 로드될 수 있습니다. AssetBundle과 혼용하면 공유 의존성이 중복될 수 있다는 점도 확인합니다.

참고: [콘텐츠 디렉토리 소개](https://docs.unity3d.com/6000.6/Documentation/Manual/content-directories-introduction.html), [Addressables 빌드 시스템 선택](https://docs.unity3d.com/Packages/com.unity.addressables@4.1/manual/content-build-systems.html), [Unity 6.6 Addressables 버전 목록](https://docs.unity.com/en-us/engine/6000.6/manual/packages-list/packages-all/pack-safe/com-unity-addressables).


## 프로젝트 완료 확인

- [ ] Use Existing Build와 PC Player에서 stage-main을 읽습니다.
- [ ] 서버 없이 수집·결과·재시작이 유지됩니다.

오늘의 프로젝트를 `Progress/Day07`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. Unity Build Profile과 Addressables Profile은 각각 무엇을 관리하나요?
2. Use Asset Database에서 성공한 로드도 Player에서 다시 확인해야 하는 이유는 무엇인가요?

## 오늘의 정리

같은 게임을 Editor 자산 없이도 실행할 수 있게 만들었습니다. 다음 수업에서는 이 동작하는 로컬 버전을 보관하고, 원격 배포에 사용할 HTTPS 파일 서버를 만듭니다.
