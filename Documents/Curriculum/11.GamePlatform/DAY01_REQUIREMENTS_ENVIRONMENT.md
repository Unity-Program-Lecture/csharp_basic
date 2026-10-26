# DAY 01: 플랫폼 요구 사항과 개발 환경

오늘은 플랫폼별 요구 사항과 도구를 정하고, 앞으로 계속 사용할 `PlatformCoinLab` 프로젝트를 만듭니다. 빈 씬을 빌드해 개발 환경을 확인합니다. NCS G1·G2·G3을 다룹니다.

## 1. 핵심 개념: 같은 음식, 다른 도시락통

게임 규칙은 음식이고 플랫폼은 도시락통입니다. 음식이 같아도 통 크기와 먹는 도구가 다르면 담는 방법을 바꿔야 합니다. PC 키보드 입력을 스마트폰에 그대로 옮기면 게임은 보여도 조작할 수 없습니다.

### 이 단어는 무슨 뜻인가요?

- **개발 플랫폼**: Editor·컴파일러·SDK가 실행되는 작업 환경입니다.
- **실행 플랫폼**: 완성된 게임이 실제로 실행되는 OS·하드웨어·브라우저 환경입니다.
- **라이브러리**: 입력이나 자산 로드처럼 반복되는 기능을 제공하는 도구 묶음입니다.
- **요구 사항**: 게임이 반드시 제공해야 할 기능과 지켜야 할 조건입니다.

## 2. 게임 기획을 플랫폼 조건으로 바꾸기

공통 실습에서는 평면 위를 이동하며 30초 동안 코인 10개를 수집하는 게임을 만듭니다. 코인 하나를 모으면 1점을 얻고, 시간이 끝나면 이동과 수집이 멈춥니다. 재시작 버튼을 누르면 처음 상태로 돌아갑니다.

| 구분 | PC (Windows) | Android 앱 | Web (데스크톱) | iOS 참고 |
| :--- | :--- | :--- | :--- | :--- |
| 입력 | WASD·방향키·게임패드 | 화면 방향 버튼 | 키보드·포인터·브라우저 포커스 | 터치·제스처 |
| 화면 | 창 크기 변경 | 가로 화면·Safe Area | HTML Canvas 크기 | Safe Area·기기 방향 |
| 개발 도구 | Hub·Editor·C# 도구 | SDK·NDK·JDK·adb | Web 모듈·HTTP 서버·DevTools | Mac·Xcode·서명 |
| 저장 | 쓰기 가능한 앱 경로 | 앱별 저장 공간 | 브라우저 저장 정책 | 앱 샌드박스 |
| 제약 | GPU·CPU·창 포커스 | 메모리·배터리·발열 | 다운로드·메모리·사용자 오디오 입력 | OS 생명주기·서명 |

60 FPS를 목표로 한다면 한 프레임을 처리할 수 있는 시간은 약 16.7ms입니다. 이를 프레임 예산이라고 합니다. 실제 기기에서 이 목표를 유지할 수 있는지 측정하고, 어렵다면 화면 품질이나 목표 프레임을 조정한 이유를 기록합니다.

## 3. 실습: 도구를 이유와 함께 선택하기

**미션:** 기능에 맞는 도구를 고르고, 다른 도구와 비교했을 때 이 도구를 선택한 이유를 한 문장씩 씁니다.

| 기능 | 수업 선택 | 선택 이유 |
| :--- | :--- | :--- |
| 이동·터치 | Input System | 같은 Move 액션에 여러 장치를 연결 |
| 버튼·HUD | UGUI·TextMeshPro | 기존 UI 선수학습을 활용하고 화면 대응 가능 |
| 콘텐츠 로드 | Addressables | 주소 기반 비동기 로드와 콘텐츠 빌드 관계 학습 |
| 설정 JSON 읽기 | UnityWebRequest | 로컬 URL·Android 패키지·HTTP 읽기를 비교 |
| 단순 최고 점수 | PlayerPrefs | 작은 숫자 저장을 시작점으로 활용 |

1. 개발 PC 사양과 사용할 Android 모델을 조사합니다.
2. PC/Android/Web 각각의 입력·화면·성능·저장·사운드 요구를 작성합니다.
3. 네트워크 없는 환경에서도 기본 게임이 가능한지 정합니다.
4. 기본 게임은 로컬 콘텐츠로 실행하고, 서버는 DAY 09의 원격 번들 비교와 DAY 13의 선택 HTTP 설정 읽기에 사용한다고 표시합니다.
5. iOS는 참고 범위임을 기록합니다.

### 실행해보면

오늘 정리한 요구 사항은 나중에 게임을 테스트할 때 확인 기준이 됩니다. 예를 들어 “모바일 지원”이라고만 적기보다 “가로 화면에서 터치로 이동할 수 있고, 버튼이 Safe Area 안에 보인다”처럼 직접 확인할 수 있는 문장으로 작성합니다.

## 4. 핵심 개념: 공장과 배송지는 다르다

Editor는 물건을 조립하는 공장이고 Player는 배송된 완제품입니다. Play Mode는 공장 안의 시운전입니다. 완제품을 실제 배송지에서 열어 봐야 실행 환경을 확인할 수 있습니다.

### 이 단어는 무슨 뜻인가요?

- **빌드 모듈**: 특정 플랫폼용 Player를 만들기 위한 부품입니다.
- **Android SDK** (Android Software Development Kit, 안드로이드 소프트웨어 개발 도구 모음): Android 앱 개발에 필요한 API와 빌드 도구 등을 제공합니다. SDK는 개발 도구 모음을 뜻하는 일반 용어이며, 이 문서에서 SDK는 Android SDK를 가리킵니다.
- **NDK** (Native Development Kit, 네이티브 개발 도구 모음): C/C++ 같은 네이티브 코드를 Android 기기용으로 빌드하는 데 사용합니다.
- **JDK** (Java Development Kit, 자바 개발 도구 모음): Java 코드를 컴파일하고 Android 빌드 도구를 실행하는 데 사용합니다.
- **Build Profile**: 플랫폼과 빌드 설정을 묶어서 관리하는 설정입니다.
- **Development Build**: 로그·프로파일링을 활용하기 위한 개발용 빌드입니다.

### Unity의 Android 빌드에서는 어디에 사용하나요?

Unity에서 Android 빌드를 누르면 여러 도구가 역할을 나누어 앱을 만듭니다. 조립 공장에 서로 다른 작업 도구가 필요하듯 SDK, NDK, JDK도 각각 담당하는 일이 있습니다.

| 도구 | Unity 빌드에서 맡는 역할 | 실습에서 만나는 예 |
| :--- | :--- | :--- |
| **Android SDK** | 대상 Android API에 맞춰 앱을 빌드하고, 리소스를 처리해 APK·AAB 패키지를 만드는 데 필요한 도구를 제공합니다. | Target API Level에 맞는 Android SDK가 필요합니다. Android SDK에 포함된 `adb`는 빌드한 APK를 기기에 설치하거나 로그를 확인할 때도 사용합니다. |
| **NDK** | C/C++ 코드를 Android CPU에서 실행할 수 있는 네이티브 라이브러리로 컴파일하고 연결합니다. | IL2CPP를 사용하면 Unity가 C# 코드를 C++로 변환하고, NDK의 도구로 이를 ARM64 같은 대상 아키텍처에 맞게 빌드합니다. |
| **JDK** | Android 빌드를 진행하는 Gradle을 실행할 Java 환경과 Java 컴파일 도구를 제공합니다. | 직접 Java 코드를 작성하지 않아도 Unity가 생성한 Android 프로젝트를 Gradle로 빌드할 때 필요합니다. Unity는 기본적으로 OpenJDK를 사용합니다. |

**Gradle**은 필요한 코드와 자산을 모으고 컴파일과 패키징 작업을 조율하는 빌드 도구입니다. JDK가 Gradle을 실행할 환경을 제공하고, 빌드 과정에서는 필요한 SDK·NDK 도구를 사용합니다. NDK는 C#을 직접 컴파일하는 도구가 아니라, IL2CPP가 변환한 C++ 코드를 빌드하는 데 사용한다는 점을 기억합니다.

이 설명은 Android 빌드를 기준으로 합니다. Windows나 Web 빌드에서는 대상에 맞는 별도의 도구를 사용합니다.

참고: [Unity의 Android 빌드 과정](https://docs.unity3d.com/6000.6/Documentation/Manual/how-unity-builds-android-applications.html), [Android 개발 환경 설정](https://docs.unity3d.com/6000.6/Documentation/Manual/android-sdksetup.html).

## 5. 실습: 개발 도구와 프로젝트 준비

**미션:** Unity Hub에서 자신의 PlatformCoinLab을 만들고 배경과 카메라만 있는 첫 씬을 저장합니다. 오늘은 C# 스크립트를 추가하지 않습니다.

### Editor와 플랫폼 모듈 설치

이 과정의 기준은 **Unity 6.6 `6000.6.7f1`** 입니다. 2026년 10월 26일에 수업 기준 버전으로 지정했으며, 설치한 전체 Editor 버전과 운영체제도 실습 기록에 적습니다. 같은 조립 설명서라도 부품이 다르면 과정이 달라지듯, Unity도 Editor·패키지·SDK에 따라 설정 이름과 결과가 달라질 수 있습니다.

1. Unity Hub의 Installs에서 기준 Editor를 설치하거나 설치 상태를 확인합니다.
2. 해당 Editor의 Add modules에서 Android Build Support와 하위 Android SDK & NDK Tools·OpenJDK, Web Build Support를 설치합니다.
3. Windows IL2CPP 비교를 위해 해당 빌드 모듈과 Visual Studio의 C++ 개발 도구도 준비합니다. 첫 Windows 빌드는 Mono로 확인하며 IL2CPP 비교는 DAY 10에서 진행합니다.
4. `Edit > Preferences > External Tools`에서 Android SDK·NDK·JDK가 해당 Editor에 함께 설치된 도구를 사용하는지 확인합니다. 다른 Editor나 별도 Android Studio의 도구로 바꾸지 않습니다.

참고: [Android 개발 환경](https://docs.unity3d.com/6000.6/Documentation/Manual/android-sdksetup.html).

### 새 프로젝트 만들기와 버전 기록

1. Hub의 New project에서 Universal 3D 템플릿을 선택하고 이름을 PlatformCoinLab으로 정합니다.
2. 자신의 작업 폴더를 위치로 선택합니다. 이후에는 같은 프로젝트를 이어 사용합니다.
3. Project 창에서 Assets/Scenes와 Assets/Materials 폴더를 만듭니다. 템플릿에 같은 폴더가 있다면 그대로 사용합니다.
4. `ProjectSettings/ProjectVersion.txt`의 Editor 버전과 `Window > Package Management > Package Manager`의 설치 패키지 버전을 기록합니다. Packages/manifest.json·packages-lock.json·ProjectSettings는 프로젝트와 함께 보관합니다.
5. `Edit > Project Settings > Editor`의 Enter Play Mode 설정을 기록합니다. Domain Reload와 반복 Play의 차이는 DAY 05에서 다룹니다.

Input System·Unity UI·TextMeshPro 준비는 UI를 만드는 DAY 02, Addressables 설치는 스테이지 로딩을 만드는 DAY 06에서 진행합니다. 아직 이후 일차의 스크립트나 빈 기능 오브젝트를 만들지 않습니다.

### 첫 씬의 바닥과 카메라

1. 현재 씬을 `Assets/Scenes/PlatformMain.unity`로 저장합니다.
2. Hierarchy에서 3D Object > Cube를 만들고 이름을 Ground로 바꿉니다. Position은 `(0,-0.3,0)`, Scale은 `(20,0.5,20)`으로 둡니다.
3. Assets/Materials에서 Create > Material로 GroundMaterial을 만듭니다. Shader는 Universal Render Pipeline/Lit, Base Map 색은 짙은 남색으로 정한 뒤 Ground에 끌어 놓습니다.
4. 템플릿의 Main Camera를 Position `(0,23,-15)`, Rotation `(57,0,0)`으로 둡니다. Projection은 Orthographic, Size는 13으로 시작해 바닥이 보이는지 확인합니다.
5. 템플릿의 Directional Light는 유지합니다. 씬을 저장하고 Play에서 카메라 화면을 확인한 뒤 종료합니다.

오늘은 배경과 카메라가 있는 상태입니다. Player·코인·GameSession·Canvas·EventSystem은 DAY 02에 직접 추가합니다. 격자와 후처리는 기본 게임 완성 후 선택 사항입니다.

### 빌드 프로필 만들기

1. `File > Build Profiles`에서 Windows·Android·Web 프로필을 각각 추가합니다. 대상 프로필을 추가할 수 없다면 Hub의 해당 모듈 설치부터 확인합니다.
2. Scene List에 PlatformMain을 등록하고 첫 실행 씬인지 확인합니다.
3. Windows 프로필의 Player Settings에서 Scripting Backend를 Mono로 둡니다. 첫날은 로그를 확인할 수 있도록 Development Build를 사용합니다.
4. Android 프로필에서는 Minimum API Level을 Android 8.0 (API 26) 이상, Scripting Backend를 IL2CPP, Target Architectures를 ARM64로 설정합니다. 사용할 기기가 이 조건을 지원하는지 확인합니다. Build App Bundle과 Export Project는 끄고 SDK·NDK·JDK는 Hub 제공 도구를 유지합니다.
5. 출력 폴더는 Assets 밖의 `Builds/Windows`, `Builds/Android`, `Builds/Web`으로 나눕니다. Switch Profile 뒤에는 컴파일과 자산 재임포트가 끝날 때까지 기다립니다.

## 6. 실습: 빈 프로젝트로 세 환경 확인

**미션:** 방금 저장한 PlatformMain을 각 프로필로 빌드하고, 도구 준비·빌드·실제 실행 중 어디까지 확인했는지 기록합니다.

1. 모듈 설치 후 프로젝트를 열고 패키지 버전을 기록합니다.
2. 공통 씬을 저장하고 Scene List에 등록합니다.
3. Windows 프로필로 전환하고 `Build And Run`을 실행합니다. 창이 열리는지 확인합니다.
4. Android 기기가 있다면 개발자 옵션·USB 디버깅을 켜고 데이터 케이블로 연결합니다. 기기의 디버깅 허용 창을 확인한 뒤 Run Device에서 기기를 선택하고 APK 빌드·설치를 확인합니다. 기기가 없다면 아래의 ‘Android 기기 없이 빌드 확인하기’를 진행합니다.
5. Web 프로필로 전환한 뒤 Build And Run으로 브라우저를 엽니다. 파일을 더블클릭하는 방식과 구분합니다.
6. 실패하면 Console의 마지막 줄만 보지 말고 빌드 로그의 첫 오류를 기록합니다.

| 점검 | 실패 시 확인 |
| :--- | :--- |
| 프로필을 추가할 수 없음 | 해당 Editor의 모듈 설치 상태 |
| Android 도구 경로 오류 | External Tools의 Hub 제공 SDK/NDK/JDK |
| 기기가 목록에 없음 | 케이블·USB 허용·드라이버·adb 연결 |
| 엉뚱한 씬 시작 | Scene List와 씬 순서 |
| Web 빈 화면 | 브라우저 Console·Network·서버 응답 |

### Android 기기 없이 빌드 확인하기

**APK 파일을 만드는 데는 Android 실기기가 필요하지 않습니다.** 먼저 APK를 생성하고, 실기기가 없다면 아래 Android Studio 에뮬레이터 실습으로 설치와 실행도 시도해 봅니다.

1. Android 빌드 프로필을 활성화하고 스크립트 컴파일과 자산 재임포트가 끝날 때까지 기다립니다.
2. Scene List에 실습 씬이 포함되어 있는지 확인합니다.
3. APK를 만들도록 Build App Bundle을 끕니다. APK 생성까지 확인하려면 Export Project도 끕니다. Export Project는 최종 APK 대신 Android용 프로젝트를 내보내는 옵션입니다.
4. `Build And Run` 대신 **Build**를 선택하고 `Builds/Android/PlatformCoinLab.apk`처럼 프로젝트의 Assets 폴더 밖에 저장합니다.
5. 빌드 완료 메시지와 APK 파일의 생성 여부, 수정 시각, 크기를 확인합니다. 오류가 발생하면 빌드 로그에서 첫 오류를 찾아 기록합니다.
6. APK 생성까지 확인했다면 결과를 `Android APK 빌드 성공 / 설치·실행 미확인`으로 적습니다. 이어서 에뮬레이터에서 실행한 결과는 따로 기록합니다.

| 확인 방법 | 기기 없이 확인할 수 있는 것 | 이후에 확인할 것 |
| :--- | :--- | :--- |
| Unity에서 APK 빌드 | Android 대상 컴파일과 패키징 완료 여부 | 실제 설치와 실행 |
| Android Studio의 Android Emulator | 가상 기기에 APK 설치와 실행, 기본 화면과 조작 | 실기기의 입력·호환성·성능 |
| Android Studio의 APK Analyzer (선택) | APK 내부 파일, 최종 AndroidManifest.xml, 파일별 크기 | 앱의 실제 동작 |
| Unity의 Device Simulator (선택) | Editor에서 화면 비율과 Safe Area에 따른 UI 배치 | Android Player의 실행, 실제 입력과 성능 |

APK 내부를 더 살펴보고 싶다면 Android Studio의 `Build > Analyze APK`에서 생성한 파일을 엽니다. Android Studio는 에뮬레이터 실행과 APK 분석에 사용하며, APK 빌드 자체는 Unity Hub로 준비한 도구만으로 진행할 수 있습니다. [APK Analyzer 공식 안내](https://developer.android.com/studio/debug/apk-analyzer)

### Android Studio 에뮬레이터에서 APK 실행하기

**미션:** Windows PC에 Android 가상 기기를 만들고, Unity에서 빌드한 APK를 설치해 첫 화면을 확인합니다.

**AVD** (Android Virtual Device, 안드로이드 가상 기기)는 에뮬레이터에서 사용할 기기 모양과 Android 버전 등을 정한 설정입니다. 아래에서는 ARM64 바이너리를 실행할 수 있는 Android 11의 x86_64 시스템 이미지를 사용해 봅니다. 모든 x86_64 이미지가 ARM64 앱을 실행하는 것은 아니므로 버전과 이미지 종류를 함께 확인합니다.

#### 1. 가상 기기 만들기

1. [Android Studio 공식 사이트](https://developer.android.com/studio)에서 Windows용 Android Studio를 설치합니다. 초기 설정에서 Android SDK와 Android Emulator를 설치합니다. 에뮬레이터용 SDK를 추가하더라도 Unity의 External Tools 경로는 기존 Hub 제공 도구를 유지합니다.
2. 시작 화면에서 `More Actions > Virtual Device Manager`를 엽니다. 프로젝트를 연 상태라면 `Tools > Device Manager`에서 엽니다. 메뉴 이름은 설치 버전에 따라 조금 다를 수 있습니다.
3. `Create Device`에서 Pixel 계열의 Phone 기기를 선택합니다.
4. 시스템 이미지 목록에서 **Android 11 (API 30) / Google APIs / x86_64** 조합을 찾아 다운로드합니다. 필요한 경우 다른 이미지나 이전 버전을 표시하는 옵션을 사용합니다.
5. 설정을 마치고 가상 기기를 생성한 뒤 목록의 **▶ 버튼**을 누릅니다. Android 홈 화면이 나올 때까지 기다립니다.

에뮬레이터가 가상화 관련 오류로 시작하지 않으면 PC의 CPU 가상화 설정과 Windows 하이퍼바이저 구성을 확인합니다. [가상 기기 생성 안내](https://developer.android.com/studio/run/managing-avds), [에뮬레이터 가속 설정](https://developer.android.com/studio/run/emulator-acceleration)

#### 2. Unity에서 실행용 APK 만들기

1. Unity의 Android 빌드 프로필을 활성화합니다.
2. `Player Settings > Other Settings`에서 아래 항목을 설정합니다.

| 항목 | 실습 설정 | 이유 |
| :--- | :--- | :--- |
| Scripting Backend | IL2CPP | C# 코드를 C++로 변환해 네이티브 코드로 빌드합니다. |
| Target Architectures | ARM64 | 선택한 이미지의 ARM64 실행 기능을 이용합니다. |
| Minimum API Level | API 30 이하의 지원 값 | 가상 기기의 Android 11보다 높은 최소 버전을 요구하지 않도록 합니다. Target API Level과는 다른 설정입니다. |

3. Scene List에 실습 씬이 있는지 확인합니다.
4. Build App Bundle과 Export Project를 끄고 **Build**를 선택합니다.
5. `Builds/Android/PlatformCoinLab.apk`로 저장합니다. 이번에는 APK를 직접 설치하므로 Build And Run을 사용할 필요가 없습니다.

#### 3. APK 설치와 실행 확인하기

1. 생성한 APK 파일을 실행 중인 에뮬레이터 화면으로 드래그합니다.
2. 설치가 끝나면 Android의 앱 목록을 열고 Unity에서 설정한 Product Name의 앱을 찾습니다.
3. 앱 아이콘을 눌러 실행합니다. 1일차의 빈 실습 씬이 열리는지 확인합니다. 아직 구현하지 않은 이동·수집 기능은 이후 수업에서 확인합니다.
4. 결과를 `APK 빌드 성공 / Android 11 에뮬레이터 설치·실행 성공 / 실기기 미확인`처럼 기록합니다. 설치나 실행에 실패했다면 실패한 단계와 오류 메시지를 남깁니다.

| 문제 | 먼저 확인할 내용 |
| :--- | :--- |
| `INSTALL_FAILED_NO_MATCHING_ABIS` | APK에 ARM64 코드가 포함되어 있는지, 이미지가 ARM64 실행을 지원하는지 확인합니다. |
| Android 버전이 낮다는 설치 오류 | Unity의 Minimum API Level이 가상 기기의 API 30보다 높지 않은지 확인합니다. |
| 설치 후 검은 화면 또는 즉시 종료 | Android Studio의 Logcat에서 해당 앱의 오류를 확인합니다. 그래픽 API·네이티브 라이브러리·초기화 오류를 구분합니다. |
| 에뮬레이터가 시작되지 않음 | 가상화·하이퍼바이저 설정과 에뮬레이터 오류 메시지를 확인합니다. |

Device Simulator는 Editor에서 화면 배치를 확인하는 도구이며 APK를 실행하지 않습니다. APK 실행에는 위의 Android Emulator를 사용합니다. Unity는 Android 에뮬레이터를 공식 지원하지 않으므로 이 절차의 실행 성공을 보장하지는 않습니다. 에뮬레이터에서 확인한 결과와 실기기에서 확인한 결과를 구분하며, 실제 터치 입력·발열·성능은 실기기에서 다시 확인합니다.

참고: [APK 설치 방법](https://developer.android.com/studio/run/emulator-install-add-files), [Android 11 이미지의 ARM 바이너리 지원](https://developer.android.com/studio/releases/emulator#3000_february_19_2020), [Unity의 Android 호환성 안내](https://docs.unity3d.com/6000.6/Documentation/Manual/android-requirements-and-compatibility.html).

### 실행해보면

빌드가 끝나면 PC에서는 실행 파일과 데이터 폴더가, Android에서는 APK가, Web에서는 HTML과 Build 관련 파일이 만들어집니다. 파일 구성과 함께 각 결과물을 어디에서 실행하는지도 적어 봅니다. 예를 들어 Web Player는 브라우저에서 실행합니다.

## 7. 확장 실습: 설정 한 개씩 바꾸기

Product Name을 바꿔 창·앱 이름이 어떻게 달라지는지 확인합니다. Android Application Identifier를 `com.example.platformcoinlab` 형태로 정합니다. 수업용 예시 식별자이며 실제 출시 식별자는 별도로 결정합니다.

Development Build 설정을 바꿔 다시 빌드하고, 파일 크기와 로그, 실행 결과를 비교합니다. 빌드 대상을 바꾸면 자산을 다시 임포트하는 데도 시간이 걸립니다. 전환을 기다리는 동안 다음에 확인할 항목과 절차를 정리합니다.

## 프로젝트 완료 확인

- [ ] 요구 사항과 도구 선택 이유를 기록했습니다.
- [ ] PlatformMain을 Windows에서 실행하고 Web·Android의 빌드·실행 상태를 구분해 기록했습니다.

오늘의 프로젝트를 `Progress/Day01`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. Windows에서 Editor가 실행된다고 Android 게임 실행도 확인한 것일까요?
2. Input System과 Addressables는 각각 어떤 요구 사항을 해결하나요?
3. Build 성공과 Build And Run 성공은 어떤 점이 다른가요?
4. Android 도구를 다른 Editor 설치 폴더와 섞으면 어떤 문제가 생길 수 있나요?

## 오늘의 정리

요구 사항에 맞춰 도구를 선택하고 같은 프로젝트의 첫 씬과 빌드 프로필을 준비했습니다. 다음 수업에서는 이 프로젝트에 게임 진행 기록 클래스를 추가합니다.
