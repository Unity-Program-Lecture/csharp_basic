# DAY 11: Android 빌드와 실기기 디버깅

오늘은 Android 기기에 APK를 설치하고 터치로 게임을 조작합니다. 앱을 중단했다가 돌아왔을 때의 동작과 장치 로그도 확인합니다. 이 활동은 NCS 학습 목표 G3·G7·G8에 해당합니다.

## 1. 핵심 개념: 손안의 컴퓨터는 전력도 예산이다

PC에서 무리 없이 처리하던 작업도 Android 기기에서는 메모리를 많이 사용하거나 발열과 배터리 소모를 늘릴 수 있습니다. Game View의 크기를 줄이는 것만으로는 이런 차이를 확인하기 어려우므로 실제 기기에서 테스트합니다.

### 이 단어는 무슨 뜻인가요?

- **APK**: 기기에 설치할 수 있는 Android 패키지입니다.
- **AAB**: 스토어 배포를 위한 앱 번들입니다. APK처럼 기기에 직접 설치하는 파일은 아닙니다.
- **adb**: 개발 PC에서 Android 연결·설치·로그 등을 확인하는 도구입니다.
- **ARM64**: Android 기기에서 널리 사용하는 CPU 아키텍처입니다. 실제 사용할 기기가 지원하는지 확인합니다.

## 2. 실습: 설치와 조작

**미션:** 연결한 기기에서 게임을 실행합니다.

이번 기준 환경은 Android 8.0 (API 26) 이상, OpenGL ES 3.1 이상 또는 Vulkan을 지원하는 기기입니다. Player Settings의 Minimum API Level을 더 높였다면 기기도 그 설정을 충족해야 합니다. SDK·NDK·JDK는 해당 Editor에 Hub로 설치한 도구를 사용합니다. 참조 의존성은 NDK r27c·OpenJDK 17이며, 다른 도구를 섞지 않고 설치한 Editor의 요구 버전을 확인합니다. [Android 지원 조건](https://docs.unity3d.com/6000.6/Documentation/Manual/android-requirements-and-compatibility.html), [지원 도구 버전](https://docs.unity3d.com/6000.6/Documentation/Manual/android-supported-dependency-versions.html)

1. Android 프로필을 활성화하고 Hub의 SDK·NDK·JDK 경로를 확인합니다.
2. Player Settings에서 Company Name·Product Name을 정하고 Application Identifier를 `com.student01.platformcoinlab`처럼 고유한 값으로 지정합니다. 식별자는 한글·공백 없이 영문 이름을 사용합니다. Default Orientation은 Landscape Left, Scripting Backend는 IL2CPP, Target Architectures는 ARM64로 설정하고 기기 지원을 확인합니다. 기존 식별자로 설치한 앱과 다르게 지정하면 저장 데이터도 별개가 된다는 점을 기록합니다.
3. 수업 직접 설치는 APK로 진행합니다. Build App Bundle 옵션을 끕니다. Development Build와 필요 시 Script Debugging을 켭니다.
4. Addressables Android 콘텐츠를 빌드하고 APK를 빌드합니다.
5. 기기의 USB 디버깅 허용을 확인하고 Build And Run으로 설치합니다.
6. 화면 방향 버튼·UI 버튼·코인·사운드·저장·중단/복귀를 검증합니다.

| 기기 테스트 | 확인할 현상 |
| :--- | :--- |
| 방향 버튼 누르기 + 다른 버튼 조작 | 여러 터치가 서로 방해하는지 |
| 화면 비율·Safe Area | 버튼이 잘리거나 카메라 영역이 달라지는지 |
| 홈 이동 후 복귀 | 타이머·입력·저장·사운드 상태 |
| 저사양 장치·장시간 실행 | 프레임·발열 변화 |
| 네트워크 끄기 | 로컬 콘텐츠 유지·원격 로드 실패 안내 |

## 3. 실습: 장치 로그

Unity가 사용하는 SDK의 platform-tools에 있는 adb를 사용합니다. PATH에 등록한 경우 다음 명령을 실행할 수 있습니다.

```powershell
adb devices
adb logcat -s Unity AndroidRuntime
```

기기 상태가 `unauthorized`이면 화면에 표시된 USB 디버깅 허용 창을 확인합니다. `offline`이면 케이블과 기기 연결 상태를 점검합니다. 여러 기기가 연결되어 있다면 `adb -s <serial> ...`로 대상을 지정합니다. 로그 출력을 끝내려면 Ctrl+C를 누릅니다. Android Logcat 패키지로도 같은 로그를 Editor에서 볼 수 있으며, 이 경우 설치한 패키지 버전을 함께 기록합니다.

### 실행해보면

NullReferenceException이 나면 첫 사용자 코드 위치와 Inspector 참조를 확인합니다. Gradle 실패는 게임 런타임 오류와 구분합니다. 설치 실패는 식별자·기존 서명·공간·아키텍처 등을 조사하고, 임의 삭제 전에 원인을 확인합니다.

## 4. 디버깅 환경 비교

PC Player의 로컬 로그와 달리 Android는 USB 또는 네트워크 장치 연결이 필요합니다. Profiler 연결은 Development Player를 대상으로 DAY 14에 다룹니다. Editor에서 잘 되던 경로·파일 읽기는 APK 내부 경로와 달라질 수 있으므로 DAY 13에서 비동기 읽기로 비교합니다.

## 프로젝트 완료 확인

- [ ] 실기기·에뮬레이터 중 실행 환경을 명시했습니다.
- [ ] 터치·Safe Area·저장·중단/복귀 중 확인한 것과 미확인을 구분합니다.

오늘의 프로젝트를 `Progress/Day11`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. AAB를 실습 APK와 구분해야 하는 이유는 무엇인가요?
2. Editor 터치 흉내와 실기기 멀티터치 테스트는 어떤 차이가 있나요?

## 오늘의 정리

Android에서는 설치가 끝난 뒤에도 터치 입력, 장치 로그, 앱 중단과 복귀를 각각 확인해야 합니다. iOS는 같은 모바일 환경이지만 개발 도구와 빌드 과정이 다르므로 참고 자료에서 비교합니다.
