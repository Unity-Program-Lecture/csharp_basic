# 공식 문서와 검증 범위

## 기준

NCS 학습 범위는 저장소의 `LM0803020528_18v4,+LM0803020529_18v4_게임플랫폼과+UIUX+프로그래밍.pdf` 중 게임플랫폼 영역을 기준으로 합니다. 9개 학습 목표와 일차별 수업의 관계는 [연결표](NCS_ALIGNMENT.md)에서 확인할 수 있습니다.

교육 일정은 `Documents/COURSE_SCHEDULE_SUMMARY.md`의 100시간을 기준으로 합니다. 16일 자료는 하루 6시간 총 96시간이며 추가 4시간을 별도로 안내합니다.

초기 자료는 저장소의 `Code/MonsterHunt/ProjectSettings/ProjectVersion.txt`에서 확인한 `6000.4.7f1`을 참고해 작성했습니다. 2026년 10월 19일 외부 링크를 점검하면서 공식 매뉴얼의 참조 버전을 지원 중인 Unity 6.6으로 변경했습니다. 기존 MonsterHunt 프로젝트는 버전 확인에 참고했으며, 이 과정의 완성된 실습 프로젝트로 제공하는 것은 아닙니다.

Unity 6.4는 [공식 출시 공지](https://discussions.unity.com/t/unity-6-4-is-now-available/1713245)에서 지원 종료가 안내되어 있습니다. 문서가 열리는 것과 해당 에디터 버전이 지원 중인 것은 다릅니다. Unity 6.6의 지원 상태는 [공식 출시 공지](https://discussions.unity.com/t/unity-6-6-is-now-available/1735357)에서 확인할 수 있습니다.

## 참고한 공식 자료

| 주제 | 근거와 확인 내용 |
| :--- | :--- |
| Android | [Unity 6.6 환경 구성](https://docs.unity3d.com/6000.6/Documentation/Manual/android-sdksetup.html): Build Support·SDK·NDK·JDK |
| iOS | [Unity 6.6 빌드 흐름](https://docs.unity3d.com/6000.6/Documentation/Manual/iphone-BuildProcess.html): Unity 출력과 Xcode의 최종 빌드 구분 |
| Web 브라우저 | [Unity 6.6 브라우저 호환성](https://docs.unity3d.com/6000.6/Documentation/Manual/webgl-browsercompatibility.html): 브라우저별 지원 확인 |
| Web 서버 | [Unity 6.6 Web 배포](https://docs.unity3d.com/6000.6/Documentation/Manual/webgl-deploying.html): 압축·응답 헤더·서버 구성 |
| Application | [Unity 6.6 API](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Application.html): 플랫폼·경로·실행 정보 |
| 조건부 컴파일 | [Unity 6.6 플랫폼별 컴파일](https://docs.unity3d.com/6000.6/Documentation/Manual/platform-dependent-compilation.html): Editor·플랫폼 심벌 |
| 프로파일링 | [Unity 6.6 앱 프로파일링](https://docs.unity3d.com/6000.6/Documentation/Manual/profiler-profiling-applications.html): 실제 Player 측정 |
| 가상 입력 | [Input System 1.20 매뉴얼](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/index.html): 입력 시스템의 공식 진입점. 가상 입력은 패키지 문서의 On-screen controls 항목 참고 |
| Addressables 설치 | [Addressables 2.11 설정](https://docs.unity3d.com/Packages/com.unity.addressables@2.11/manual/AddressableAssetsGettingStarted.html): 패키지와 설정 시작점 |
| 콘텐츠 빌드 | [Addressables 2.11 전체 빌드](https://docs.unity3d.com/Packages/com.unity.addressables@2.11/manual/builds-full-build.html): 경로·프로필·콘텐츠 빌드 |
| Player와 콘텐츠 | [Addressables 2.11 Player 빌드 연계](https://docs.unity3d.com/Packages/com.unity.addressables@2.11/manual/build-player-builds.html): 함께 빌드할지 별도 빌드할지 선택 |

패키지 참조는 Unity 6.6의 Released 목록에 있는 [Input System 1.20.1](https://docs.unity.com/en-us/engine/6000.6/manual/packages-list/packages-all/pack-safe/com-unity-inputsystem)과 [Addressables 2.11.2](https://docs.unity.com/en-us/engine/6000.6/manual/packages-list/packages-all/pack-safe/com-unity-addressables)을 기준으로 합니다. 기존 AssetBundle 실습에 맞춰 Addressables 2.x 문서를 선택했습니다. Input System 1.20의 기존 OnScreen.html 주소는 점검 도구에서 접근을 확인하지 못해, 접근을 확인한 같은 버전의 매뉴얼 첫 페이지로 연결했습니다.

이번 점검은 링크의 접근 여부, 문서 주제, 공식 버전 지원 정보를 확인한 것입니다. 프로젝트나 패키지를 실제로 업그레이드하거나 Unity 6.6에서 예제의 빌드·실행을 검증한 것은 아닙니다. 수업에서는 설치한 정확한 버전을 기록하고 설정 이름과 동작을 다시 확인합니다. 실습용 `http://localhost:8000` 주소는 외부 문서 링크가 아니므로 그대로 유지합니다.

## 자료 작성 시 확인한 범위

### Unity 6.6 기준 문서 보완

2026년 10월 26일 수업 기준 Editor를 [Unity 6.6 `6000.6.7f1`](https://unity.com/releases/editor/whats-new/6000.6.7f1)로 변경했습니다. 다음 내용은 2026년 10월 19일 [Unity 6.6 변경 사항](https://docs.unity.com/en-us/engine/6000.6/manual/whats-new/unity66)을 기준으로 보완한 내용입니다. 기준 버전 변경은 `6000.6.7f1`에서의 빌드·실행 검증을 완료했다는 의미는 아닙니다.

- 이전 DAY 14 예제에서 사용 중단된 정렬 인자 오버로드를 제거했습니다. 누적형 개편에서는 별도 탐색 예제를 실제 GameSession UI 갱신 개선으로 대체했습니다. [공식 API](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Object.FindObjectsByType.html)
- 환경 설정: Package Management 메뉴 경로, Android 최소 지원 조건과 도구 버전을 안내합니다.
- DAY 05: Domain Reload 기본 비활성화와 정적 상태 초기화를 설명합니다.
- DAY 07: 콘텐츠 디렉토리와 Addressables 4.x의 비교 학습을 추가합니다. 기본 원격 실습은 AssetBundle 구성을 유지합니다.
- DAY 12·15: 기본 Web 설정을 명시하고 WebGPU, WebAssembly64, Progressive Asset Loading의 용도와 비교 방법을 설명합니다.

위 항목은 공식 문서와 예제 소스를 비교해 수정한 내용입니다. 아래의 기존 검증 기록과 별개이며, Unity 6.6 Player에서 실행 검증을 완료했다는 의미는 아닙니다.

### 기존 작성 단계의 확인 기록

- NCS 목표 9개, 본문 평가 준거와 평가 방법, 16일의 주제 연결.
- 일차별 문서·상대 링크·코드 블록의 구조.
- DAY 02의 순수 C# CoinRound와 동일한 코드로 중복 수집·종료·재시작·잘못된 값 처리를 .NET에서 실행 검증.

## 실제 수업 환경에서 확인해야 할 것

MonoBehaviour의 컴파일, Inspector 연결, 씬 동작은 수업용 Unity 프로젝트에서 확인해야 합니다. PC, Android, Web의 콘텐츠와 Player 빌드, 장치 로그, 사운드, UI, 프로파일 측정도 실제 환경에서 확인할 항목입니다. 자료 작성 단계에서는 세 플랫폼의 빌드와 실행 검증을 완료하지 않았으며, iOS는 참고 학습 범위로 다룹니다.

이 자료는 학생이 코드를 직접 입력하고 씬을 구성하며 따라 하는 실습 안내서입니다. 완성된 Unity 프로젝트를 복사하지 않고 일차별 코드를 직접 입력하고 필요한 오브젝트를 구성합니다.

## 누적형 과정 개편과 확인 범위

게임 진행 기록 설계와 한 판의 흐름 구현을 2일차에 함께 다룹니다. 7일차 로컬 빌드, 8일차 HTTPS 파일 서버, 9일차 원격 배포로 나누어 총 16일을 유지합니다. 시작·결과·재시작, 최고 점수, Addressables 스테이지 준비, 다음 라운드 설정을 하나의 GameSession에 연결했습니다. DAY 14는 그 게임의 UI 비용을 측정합니다.

Code/GamePlatform의 참고 소스는 작성한 코드의 누락을 비교하는 선택 자료입니다. 실습에 필요한 코드는 각 DAY 문서에 제시하며, 폴더 복사나 완성본 열기를 수행 조건으로 삼지 않습니다. 각 일차의 씬·Inspector·빌드 상태는 학생이 직접 구성하고 저장합니다.

문서와 참고 소스의 코드 일치, 상대 링크, 일차 번호, NCS 연결과 비공개 해설 대응을 확인합니다. 기존 순수 C# 검증 결과와 새 통합 흐름의 Unity 검증은 구분하며, 실제 Editor 컴파일·씬·Addressables 콘텐츠 빌드·Player 동작은 별도 확인 대상으로 남깁니다.

이번 개편 후 CoinRound의 순수 C# 8개 시나리오가 통과했습니다. 별도 .NET 로직 하네스에서는 DAY 02 GameSession 10개, DAY 04와 최종 GameSession 각각 13개, 총 36개의 시작 잠금·중복 수집·종료·재시작·저장·설정 적용 확인이 통과했습니다. 하네스는 Unity·TMP·UI 대체 타입과 직접 호출한 생명주기 메서드를 사용했습니다. 실제 Unity 어셈블리 컴파일, 물리 이벤트, 로더의 비동기 콜백, Inspector 연결, 파일 저장, 실제 프로파일 성능을 검증한 결과는 아닙니다.

## 학습과 검증의 구분

학생은 DAY 01부터 자신의 프로젝트를 만들고 문서의 코드·설정·완료 확인을 수행합니다. 교사의 참고 프로젝트나 과거 검증 기록을 열어 볼 필요는 없습니다. 문서와 참고 소스의 정적 일치·로직 검사·서버 응답 검사는 실제 Unity Player·Android 장치·Web 브라우저 실행의 증거와 구분합니다. 제출할 실행·오류 해결·성능 측정은 자신의 환경에서 직접 기록합니다.
