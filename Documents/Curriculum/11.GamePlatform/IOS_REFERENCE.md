# iOS 참고 자료: 환경과 빌드 흐름

이 자료는 iOS 개발 환경을 읽고 다른 플랫폼과 비교하기 위한 참고 자료입니다. **iOS 실습, Xcode 빌드, iPhone 실행, iOS 제출물은 요구하지 않습니다.** 게임 구현과 실행 확인은 PC, Android, Web에서 진행합니다.

## 1. Android와 같은 모바일이어도 도구는 다르다

Unity의 입력·게임 규칙·UGUI 코드는 상당 부분 공유할 수 있습니다. 그러나 최종 앱을 만드는 도구와 서명·설치 경로는 다릅니다. Android APK를 iPhone에서 실행할 수는 없습니다.

| 항목 | Android 실습 | iOS 참고 |
| :--- | :--- | :--- |
| 주요 개발 도구 | SDK·NDK·JDK·Gradle | macOS·Xcode·Apple SDK |
| Unity 출력 | APK/AAB | Xcode 프로젝트 |
| 최종 앱 구성 | Android 빌드 도구 | Xcode 빌드와 서명 |
| 장치 설치 | USB 디버깅·adb | 개발 팀·서명·프로비저닝·장치 설정 |
| 진단 | adb·Android Logcat·Profiler | Xcode 장치 로그·디버거·Instruments·Profiler |

## 2. 빌드 흐름 이해

일반적인 빌드 흐름은 `iOS Build Support 설치 → iOS Player 설정 → iOS 콘텐츠 빌드 → Xcode 프로젝트 생성 → macOS의 Xcode에서 팀·서명·대상 장치 설정 → 앱 빌드와 설치`다. Windows에서 Xcode 프로젝트를 생성하더라도, 최종 앱을 로컬에서 빌드하고 실행하려면 macOS의 Xcode 환경이 필요합니다.

이번 과정에서는 각 단계가 어떤 역할을 하는지 이해하는 데 집중합니다. 실제로 개발할 때는 Xcode, SDK, 지원 OS, 스토어 제출 조건에 관한 최신 공식 요구 사항을 확인합니다. 개발용 기기에 설치할 때와 App Store에 배포할 때 필요한 계정과 서명 조건도 구분해 살펴봅니다.

## 3. 코드와 동작의 비교 관점

- Input System: Move 액션을 공유하되 터치·UI 상호작용은 실제 장치에서 따로 확인해야 합니다.
- 화면: Safe Area와 방향 변경, 해상도에 따른 카메라/UI 배치를 조사합니다.
- Application: 모바일 pause/focus, 앱 샌드박스 경로, 종료 정책을 고려합니다.
- Addressables: iOS 대상 콘텐츠를 별도로 만듭니다. Android 번들로 대체하지 않습니다.
- AOT·스트리핑: IL2CPP 환경에서 리플렉션·플러그인·사용 코드의 지원을 확인합니다.
- 성능: 모바일 메모리·발열·CPU/GPU 비용을 해당 장치에서 측정해야 합니다.

iOS 앱과 iPhone Safari의 Web 게임도 다른 실행 환경입니다. Safari는 브라우저 정책·Web 지원 범위를 추가로 고려합니다.

## 읽고 설명하기

Android와 iOS의 최종 빌드 도구는 어떻게 다른지, 서명은 왜 필요한지, 네이티브 iOS 앱과 Safari에서 실행하는 Web 게임은 어떤 차이가 있는지 자신의 말로 정리합니다. 실제 장치에서 확인하지 않은 내용에는 조사한 사실인지 예상한 동작인지 표시합니다.

근거: [Unity 6.6 iOS 빌드 과정](https://docs.unity3d.com/6000.6/Documentation/Manual/iphone-BuildProcess.html). 설치 버전과 Xcode 환경에 맞는 공식 문서를 실제 작업 전에 다시 확인합니다.
