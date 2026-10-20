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
2. 기본 실습은 카탈로그를 로컬에 두고 스테이지 번들만 원격에서 읽습니다. Build Remote Catalog는 끕니다. 원격 카탈로그 업데이트는 별도 심화 주제입니다.
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
