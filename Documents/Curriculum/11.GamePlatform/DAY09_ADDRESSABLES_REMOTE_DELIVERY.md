# DAY 09: 원격 콘텐츠 배포와 로드 오류 해결

오늘은 [DAY 08](DAY08_HTTPS_CONTENT_SERVER.md)에 만든 HTTPS 파일 서버에 DAY 07의 스테이지 번들을 배포합니다. 서버가 응답하지 않을 때도 게임이 멈추지 않고 실패 안내와 재시도를 제공해야 합니다. NCS G3·G7·G8을 다룹니다.

## 1. 핵심 개념: 창고가 멀어지면 배송 경로도 확인하기

같은 `stage-main` 주소를 사용해도 콘텐츠가 웹 서버에 있으면 다운로드와 캐시, 서버 응답을 확인해야 합니다. 게임 코드의 요청 창구는 그대로 두고 배포 위치를 바꾸어 보겠습니다.

### 이 단어는 무슨 뜻인가요?

- **RemoteLoadPath**: 실행 앱이 원격 콘텐츠를 요청하는 URL입니다.
- **카탈로그**: 자산 주소와 실제 위치·의존성을 연결하는 목록입니다.
- **CORS**: 브라우저가 다른 출처의 자산을 읽을 수 있는지 확인하는 규칙입니다.
- **콘텐츠 업데이트**: 이미 배포한 콘텐츠와 Player의 호환성을 고려해 변경을 전달하는 작업입니다.

## 2. 실습: 원격 스테이지 배포

DAY 07에 만든 로컬 프로젝트와 빌드를 보관합니다. 먼저 DAY 08의 ContentServer를 실행해 `/health`와 `/platform-config.json`이 정상 응답하는지 확인합니다. 서버 소스나 인증서를 새로 만들지 않습니다. 자신이 만든 CourseStage 그룹을 그대로 두고 RemotePractice 프로필과 그룹 경로만 바꿉니다. 원격용 별도 그룹을 만드는 것은 선택 비교로 남깁니다.

1. Addressables Groups의 Profile 메뉴에서 Profiles 창을 엽니다. Default를 복제해 RemotePractice를 만들고 활성 프로필로 선택합니다. RemoteBuildPath는 `ServerData/[BuildTarget]`, RemoteLoadPath는 `https://localhost:5443/[BuildTarget]`으로 정합니다. CourseStage의 Build Path·Load Path를 각각 RemoteBuildPath·RemoteLoadPath로 바꿉니다.
2. 기본 실습은 카탈로그를 로컬에 두고 스테이지 번들만 원격에서 읽습니다. Window > Asset Management > Addressables > Settings를 선택하고 Inspector의 Catalog에서 Build Remote Catalog를 끕니다. 원격 카탈로그 업데이트는 문서 마지막의 선택 실습에서 진행합니다.
3. Groups의 Build > New Build > Default Build Script로 콘텐츠를 빌드합니다. 상태 확인에 사용한 서버를 Ctrl+C로 종료하고 ContentServer 폴더에서 아래 명령으로 ServerData를 제공합니다. FilesRoot는 자신의 Unity 프로젝트 경로로 바꿉니다. File > Build Profiles에서 Windows 프로필과 PlatformMain Scene List를 확인하고 `Builds/WindowsRemote`에 Player를 빌드합니다. 이전 주소로 만든 카탈로그·Player는 서버만 바꿔도 주소가 바뀌지 않습니다.
4. 필수 비교는 같은 PC의 Windows Player로 진행합니다. Android의 localhost는 기기 자신이며, LAN으로 확장하려면 서버 접근 주소·인증서 이름·장치의 인증서 신뢰를 따로 구성해야 합니다. localhost 개발 인증서를 IP 주소에 그대로 사용하는 것으로 해결되지 않습니다.
5. Web은 동일 출처 제공을 먼저 확인하고, 다른 출처에서는 CORS를 확인합니다. HTTP 페이지/HTTPS 페이지와 혼합 콘텐츠 정책도 함께 봅니다.
6. 서버를 중지해 로드 실패를 확인하고, 재실행·캐시 삭제·캐시 유지 상황을 나눠 기록합니다.

```powershell
dotnet run --no-launch-profile -- --FilesRoot "../PlatformCoinLab/ServerData"
```

실제 파일은 `ServerData/StandaloneWindows64/실제파일명.bundle`, 요청 URL은 `https://localhost:5443/StandaloneWindows64/실제파일명.bundle`입니다. 서버의 시작 메시지와 UnityWebRequest 오류에 나타난 주소를 함께 비교합니다. 서버의 no-cache 응답은 HTTP 재검증 정책이며 Unity의 번들 캐시를 지우는 기능이 아닙니다.

원격 실습을 완료하지 못했다면 정상 동작하는 로컬 콘텐츠 구성을 최종본으로 유지합니다. 원격 로드에서 어디까지 확인했고 어떤 오류가 남았는지 기록하면 이후에 실습을 이어갈 수 있습니다.

Editor에서 원격 번들을 검사하려면 Groups에서 RemotePractice 프로필과 CourseStage의 원격 경로를 선택합니다. 콘텐츠를 빌드한 뒤 Play Mode Script를 Use Existing Build로 바꾸고 실행합니다. 서버 로그의 번들 요청과 Unity의 로딩 결과를 함께 확인합니다.

실습을 마치면 Default 프로필과 CourseStage의 로컬 경로를 복원하고 콘텐츠를 다시 빌드합니다. Play Mode Script는 Use Asset Database로 복원합니다. 기본 완료본에는 로컬 설정을 저장하고, 원격 설정·서버 소스·실패와 복구 결과는 실습 기록으로 보관합니다.

## 3. 업데이트와 중복 자산 조사

이미 직접 참조된 자산을 Addressable로도 넣으면 중복 포함될 수 있습니다. Analyze와 빌드 보고서로 의존성을 조사합니다. 콘텐츠 업데이트는 배포 당시 content state를 보관하고 Update a Previous Build 흐름을 검토합니다. 카탈로그만 바꾸면 모든 Player 버전이 자동으로 호환된다고 가정하지 않습니다.


## 4. 실패 후 복구까지 확인

StageLoader 코드를 새로 만들지 않습니다. 로드 실패 시 Status와 Start 잠금, Reload Stage 재시도는 DAY 06 코드의 동작을 검증합니다. 서버를 다시 켠 뒤 메뉴에서 Reload Stage를 눌러 실제로 시작 가능한지 확인합니다.

| 상황 | 관찰할 것 |
| :--- | :--- |
| 정상 서버·캐시 없음 | 요청 URL과 다운로드 후 게임 시작 |
| 같은 콘텐츠·캐시 있음 | 요청·전송량 차이와 정상 수집 |
| 서버 중지·캐시 없음 | 로드 실패 안내·Start 잠금 |
| 서버 복구 후 Reload Stage | 메뉴에서 재시도 성공 |
| Web 404 또는 CORS | Network 응답과 Console의 차이 |

처음 만든 원격 번들은 Editor나 Player에서 내려받기 전에 서버를 Ctrl+C로 중지하고 Player를 실행합니다. 실패 안내와 Start 잠금을 확인한 뒤 서버를 다시 켜고 Reload Stage를 누르면 실제 다운로드와 복구를 이어 확인할 수 있습니다. 이때 플레이 중인 Player를 다시 빌드하지 않습니다.

이미 내려받아 캐시가 생겼다면 테스트 복사본에서 StageMain 프리팹의 코인 위치를 조금 바꾸고 원격 콘텐츠를 새로 빌드합니다. 새 번들 이름이 만들어졌는지 확인하고 Player도 새 카탈로그로 다시 빌드한 뒤, 새 번들을 받기 전에 위 절차를 반복합니다. 캐시 덕분에 성공한 실행을 서버 연결 성공으로 기록하지 않습니다. 최종 기본 제출본은 로컬 구성을 유지해 네트워크 없이 복원할 수 있도록 합니다.

원격 번들 배포와 실패·복구가 필수 실습이고, 실제 콘텐츠 업데이트 배포는 확장입니다. 두 경우 모두 같은 로컬 복구 지점으로 돌아갈 수 있어야 합니다.

## 프로젝트 완료 확인

- [ ] 404·서버 중지·캐시 조건을 구분해 기록했습니다.
- [ ] ASP.NET Core HTTPS 서버에서 실제 번들을 내려받았고, 인증서·URL·MIME을 확인했습니다.
- [ ] 서버 복구 후 Reload Stage로 같은 게임을 다시 시작합니다.

오늘의 프로젝트를 `Progress/Day09`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. Web 원격 로드에서 404 오류와 CORS 오류를 어떻게 구분하나요?
2. 서버를 껐는데도 스테이지가 로드되면 어떤 조건을 확인해야 하나요?

## 오늘의 정리

같은 스테이지를 원격 번들로 제공하고 서버·경로·캐시 조건에 따른 실패와 복구를 확인했습니다. 다음 수업부터 같은 게임을 플랫폼별로 배포하며 오늘의 로그 조사 절차를 이어 사용합니다.

공식 참고: [ASP.NET Core 정적 파일 제공](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-10.0), [개발용 HTTPS 인증서](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs).

## 5. 선택 실습: 실행 파일을 바꾸지 않고 스테이지 업데이트하기

**미션:** 원격 카탈로그를 사용하는 Windows Player를 한 번 만든 뒤, 실행 파일을 다시 빌드하지 않고 서버의 콘텐츠만 업데이트해 코인 배치가 바뀌는지 확인합니다.

DAY 07~09를 같은 수업에서 진행한다면 로컬 빌드 → HTTPS 서버 → 원격 번들의 실패·복구를 먼저 확인하고 이 실습을 이어갑니다. 기존 StageLoader와 서버를 그대로 사용하며 새 C# 스크립트나 업데이트 버튼은 만들지 않습니다. 기본 실습을 마친 상태를 보관하고 별도 프로젝트 복사본에서 진행합니다.

### 원격 카탈로그가 추가하는 역할

카탈로그는 `stage-main` 주소가 어느 번들을 가리키는지 알려 주는 목록입니다. 앞의 기본 실습에서는 이 목록이 Player에 포함되어 있어 목록을 바꾸려면 Player도 다시 빌드했습니다. 이번에는 서버의 갱신된 목록을 받아 같은 주소로 바뀐 스테이지를 찾습니다.

| 구분 | 기본 원격 실습 | 이번 업데이트 실습 |
| :--- | :--- | :--- |
| 카탈로그 | Player에 포함된 목록 사용 | 최초 로컬 목록과 서버의 원격 카탈로그 사용 |
| 서버 제공 파일 | 원격 번들 | 원격 번들·카탈로그·카탈로그 해시 |
| 변경 확인 | 새 카탈로그로 Player도 다시 빌드 | 최초 Player를 그대로 재실행 |

### 1단계: 첫 배포 설정

1. Play와 실행 중인 Player를 종료하고 프로젝트 복사본을 엽니다. 빌드 대상은 Windows로 유지합니다.
2. Addressables Groups에서 RemotePractice 프로필을 선택합니다. CourseStage의 원격 Build Path·Load Path는 앞의 실습과 같은 값으로 둡니다.
3. CourseStage를 선택하고 Inspector의 Content Update Restriction에서 **Prevent Updates를 끕니다.** 이전 표시 방식에서는 `Can Change Post Release`에 해당합니다. 이 그룹은 코인 배치처럼 배포 후 바뀔 콘텐츠를 담습니다. 로컬 그룹의 제한 설정은 유지합니다.
4. Window > Asset Management > Addressables > Settings를 선택하고 Inspector에서 아래 설정을 적용합니다.

| 위치 | 설정 | 값 |
| :--- | :--- | :--- |
| Catalog | Build Remote Catalog | 켬 |
| Catalog | Build & Load Paths | Remote. 직접 지정하려면 `<custom>` 선택 후 아래 두 경로 사용 |
| Catalog | Build Path / Load Path | RemoteBuildPath / RemoteLoadPath |
| Catalog | Enable Json Catalog | 켬 |
| Catalog | Only update catalogs manually | 끔 |
| Build | Build Addressables on Player Build | Do not Build Addressables content on Player Build |

Build Path·Load Path는 Build & Load Paths를 `<custom>`으로 지정했을 때 개별 표시됩니다. 카탈로그와 CourseStage 모두 `ServerData/[BuildTarget]`에 빌드하고 `https://localhost:5443/[BuildTarget]`에서 읽도록 맞춥니다. Player 빌드 시 자동 콘텐츠 빌드를 끄는 것은 직접 만든 첫 콘텐츠와 배포 기준이 다시 생성되는 일을 막기 위해서입니다.

이번에는 JSON 카탈로그를 사용하므로 DAY 08 서버가 이미 제공하는 `.json`·`.hash`·`.bundle` 형식으로 실습할 수 있습니다. 바이너리 카탈로그를 선택하면 서버에 `.bin` 형식 허용도 추가해야 하므로 이 실습에서는 설정을 바꾸지 않습니다. `.hash`는 카탈로그 변경 여부를 확인하는 값이며 번들 전체를 다시 받기 전에 목록 변경을 판단하는 데 사용합니다.

### 2단계: V1 콘텐츠와 Player 만들기

1. StageMain 프리팹의 현재 코인 배치를 캡처해 V1으로 기록하고 저장합니다. `stage-main` 주소와 코인 ID는 유지합니다.
2. Groups의 Build > New Build > Default Build Script로 **최초 전체 콘텐츠를 빌드**합니다. ServerData/StandaloneWindows64에 생성된 카탈로그 `.json`·`.hash`와 번들을 확인합니다. 파일 이름은 실제 생성된 이름을 사용합니다.
3. Settings의 Update a Previous Build > Content State Build Path를 확인합니다. 기본 위치는 `Assets/AddressableAssetsData/<대상 플랫폼>/addressables_content_state.bin`이며, `Windows`처럼 실제 생성된 대상 폴더에서 찾습니다. 경로를 변경했다면 설정된 출력 위치를 사용합니다. 이 상태 파일과 V1 서버 출력 전체를 Assets·서버 제공 폴더 밖의 별도 백업 폴더에 복사합니다.
4. ContentServer의 FilesRoot를 **이번 프로젝트 복사본의 ServerData 폴더**로 지정해 실행합니다. `/health`와 실제 카탈로그 `.json`·`.hash`의 HTTPS 주소가 응답하는지 확인합니다. 카탈로그 URL에는 `/StandaloneWindows64/`를 포함합니다.
5. File > Build Profiles에서 Windows 프로필과 PlatformMain 씬을 확인하고 Player를 `Builds/WindowsRemoteCatalogV1`에 빌드합니다. 이 실행 폴더 전체를 보관합니다.
6. 서버를 켠 상태에서 V1 실행 파일로 게임을 시작해 기존 배치를 확인하고 **Player를 완전히 종료**합니다.

앞의 Build Remote Catalog가 꺼진 상태로 만든 WindowsRemote 실행 파일을 재사용하지 않습니다. 이번 V1은 원격 카탈로그 확인 설정을 포함해 한 번 새로 만든 실행 파일입니다. `addressables_content_state.bin`은 Editor의 업데이트 빌드 기준 자료이며, Player가 다운로드할 카탈로그가 아니므로 서버 제공 폴더에 넣지 않습니다.

### 3단계: 콘텐츠만 V2로 업데이트

1. 같은 프로젝트에서 StageMain 프리팹을 열고 코인 두세 개의 위치를 눈에 띄게 바꿉니다. 바닥 범위 안에 배치하고 프리팹을 저장합니다. 코인 수·ID·주소·스크립트는 유지합니다.
2. 배포 때의 프로필·경로·JSON 형식·그룹 업데이트 제한 설정을 유지합니다. 첫 배포 이후에는 이 설정을 바꾸지 않습니다.
3. Groups의 Build > **Update a Previous Build**를 선택합니다. 기본 설정에서는 현재 Content State Build Path의 상태 파일을 사용합니다. 파일 선택을 요구하면 보관한 V1의 `addressables_content_state.bin`을 선택합니다. 자동으로 사용했다면 해당 경로의 파일이 V1 기준인지 확인합니다.
4. 이 단계에서는 **New Build를 실행하거나 Player를 다시 빌드하지 않습니다.** 잘못 전체 빌드를 했다면 변경된 상태 파일을 기준으로 계속하지 말고 V1 백업과 설정을 복원합니다.
5. 갱신된 출력 폴더의 새 번들과 카탈로그·해시 파일을 서버에서 제공합니다. FilesRoot가 빌드 출력 폴더를 직접 가리키면 별도 업로드 없이 파일이 반영됩니다. 별도 제공 폴더를 사용한다면 새 번들을 먼저 복사하고 카탈로그·해시는 그 뒤에 교체합니다. V1 번들은 삭제하지 않습니다.
6. **2단계에서 보관한 WindowsRemoteCatalogV1 실행 파일**을 다시 실행합니다. 처음 Addressables가 초기화될 때 카탈로그 변경을 확인하고, 같은 `stage-main` 주소로 V2 배치를 읽는지 확인합니다.

업데이트 빌드는 기존 상태 파일을 변경하지 않습니다. 같은 V1 Player를 위한 다음 업데이트에도 V1의 상태 파일을 사용합니다. 코인 위치 같은 자산 데이터만 바꾸는 실습이며, 새로운 C# 클래스·메서드나 게임 코드를 기존 실행 파일에 다운로드하는 실습은 아닙니다.

### 4단계: 변경 결과와 실패 조건 확인

| 확인 | 기대 결과 |
| :--- | :--- |
| V1 첫 실행 | 원래 코인 배치로 한 판 진행 |
| V2 배포 후 같은 V1 실행 파일 재실행 | 변경한 코인 배치가 나타나며 수집·종료·재시작 유지 |
| 서버 요청 기록 | 카탈로그 해시·필요한 카탈로그와 새 번들 요청 확인. 캐시된 파일은 다시 받지 않을 수 있음 |
| 새 번들을 받기 전 서버에서 그 파일을 잠시 다른 곳에 보관 | 카탈로그는 갱신돼도 번들 요청은 실패하며 Start 잠금 |
| 번들을 원래 위치에 복구하고 Reload Stage | 다운로드 후 변경된 스테이지로 시작 가능 |

번들 누락 실험은 새 V2 번들을 한 번도 내려받지 않은 상태에서만 진행합니다. 이미 캐시했다면 시간상 생략하고 `캐시로 인해 실패 조건 미확인`으로 기록합니다. 카탈로그 실패와 번들 실패는 요청 URL로 구분합니다. 서버를 끈 결과만으로 항상 시작 실패나 항상 오프라인 성공을 기대하지 않고, 로컬 카탈로그·저장된 카탈로그·번들 캐시의 영향을 확인합니다.

여기서는 앱을 종료한 뒤 재실행하여 초기화 시점에 업데이트합니다. 실행 중인 앱의 Reload Stage는 스테이지를 다시 로드할 뿐, 그 버튼 자체가 원격 카탈로그를 갱신하는 기능은 아닙니다. 실행 중 업데이트는 이후 `CheckForCatalogUpdates`·`UpdateCatalogs`와 기존 자산의 해제 순서를 함께 다루는 별도 확장으로 남깁니다.

### 실습 기록과 다음 수업 준비

- [ ] 같은 V1 실행 파일로 V1·V2 배치를 확인하고 전후 화면을 기록했습니다.
- [ ] V1 상태 파일, 카탈로그·해시·번들의 역할을 구분했습니다.
- [ ] 변경 뒤에도 한 판의 기본 기능이 유지되는지 확인했습니다.

위 결과를 기존 DAY 09 실습 기록에 덧붙입니다. 별도 보고서는 만들지 않습니다. 원격 실습의 V1 실행 파일·상태 파일·서버 출력과 변경 기록은 보관하고, 다음 수업은 원래의 로컬 프로젝트에서 이어갑니다. 복사본을 계속 사용할 경우 Build Remote Catalog와 JSON 등 변경 설정을 원래 값으로 되돌리고 Default 프로필·CourseStage 로컬 경로·Use Asset Database를 복원한 뒤 로컬 콘텐츠를 다시 빌드합니다.

공식 참고: [Addressables 설정과 카탈로그](https://docs.unity3d.com/Packages/com.unity.addressables@2.11/manual/AddressableAssetSettings.html), [업데이트 빌드](https://docs.unity3d.com/Packages/com.unity.addressables@2.11/manual/builds-update-build.html), [그룹 업데이트 제한](https://docs.unity3d.com/Packages/com.unity.addressables@2.11/manual/content-update-build-settings.html).
