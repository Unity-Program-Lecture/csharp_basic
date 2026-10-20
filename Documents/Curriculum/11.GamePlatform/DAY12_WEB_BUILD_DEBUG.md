# DAY 12: Web 빌드와 브라우저 디버깅

오늘은 HTTP 서버를 통해 Web Player를 실행합니다. 브라우저 개발자 도구의 Console과 Network를 살펴보며 오류 원인을 찾는 방법을 배웁니다. 이 활동은 NCS 학습 목표 G3·G7·G8에 해당합니다.

## 1. 핵심 개념: 브라우저는 또 하나의 운영 환경

Web 게임은 브라우저가 서버에서 Wasm 코드와 데이터를 받아 실행합니다. 따라서 게임 코드뿐 아니라 웹 서버의 설정, 브라우저 정책, 네트워크 상태도 실행 결과에 영향을 줍니다.

### 이 단어는 무슨 뜻인가요?

- **Wasm**: 브라우저가 실행하는 코드 형식입니다.
- **MIME**: 서버가 파일 종류를 알려주는 응답 정보입니다.
- **Content-Encoding**: gzip·br 등 압축 형식을 알리는 응답 정보입니다.
- **CORS**: 다른 출처에서 리소스를 읽을 수 있는지 서버와 브라우저가 확인하는 규칙입니다.

## 2. 실습: 압축 없는 첫 실행

**미션:** 서버 설정과 게임 오류를 섞지 않도록 첫 로컬 빌드는 압축을 끕니다.

첫 빌드는 그래픽 API를 WebGL 2, 아키텍처를 WebAssembly32로 두고 Progressive Asset Loading과 멀티스레드 옵션을 끕니다. DAY 08의 서버는 별도의 교차 출처 격리 헤더를 구성하지 않았으므로 멀티스레드 빌드를 기본 서버에 바로 올리지 않습니다. WebGPU 등 선택 기능은 기본 실행을 확인한 복사본에서 비교합니다.

1. Web 프로필을 활성화합니다. Development Build로 둡니다.
2. Player Settings의 Web Publishing Settings에서 Compression Format을 Disabled로 둡니다. 첫 실습용이며 최종 배포 정책은 DAY 15에서 비교합니다.
3. Web용 Addressables 콘텐츠를 빌드합니다.
4. `Builds/Web`에 Build And Run으로 빌드해 서버가 제공하는 페이지를 엽니다. 이전 압축 형식의 파일을 섞지 않도록 새 출력 폴더를 사용합니다.
5. 이동·재시작·저장·스테이지·사운드·창 크기 변경을 테스트합니다.
6. 다른 데스크톱 브라우저에서도 같은 URL을 실행하고 이름·버전을 기록합니다.

수동 서버가 필요하면 DAY 08에 만든 ASP.NET Core HTTPS 서버를 재사용합니다. 이전 서버를 Ctrl+C로 종료한 뒤 자신이 만든 ContentServer 폴더의 터미널에서 다음을 실행합니다. 경로는 자신의 Web 빌드 출력으로 바꾸며 첫 실습용 압축 없는 출력만 사용합니다.

```powershell
dotnet run --no-launch-profile -- --FilesRoot "../PlatformCoinLab/Builds/Web"
```

서버를 실행한 뒤 브라우저에서 `https://localhost:5443/`을 엽니다. UseDefaultFiles가 index.html을 연결하고 UseStaticFiles가 JS·Wasm·data를 제공합니다. HTML 파일을 더블클릭하면 `file://`로 열리므로 서버를 통한 실행과 조건이 달라집니다. 개발 인증서는 DAY 08과 같은 설정을 사용합니다.

## 3. 실습: Console·Network 조사

| 현상 | 먼저 확인 | 다음 확인 |
| :--- | :--- | :--- |
| 빈 화면 | 첫 Console 오류 | JS·Wasm·data 응답 상태 |
| 자산 404 | 요청 URL | 서버 파일·대소문자·카탈로그 |
| CORS 오류 | 출처와 응답 헤더 | 동일 출처 또는 서버 허용 설정 |
| 압축 오류 | Content-Encoding | 실제 압축·확장자·fallback |
| 소리가 안 남 | 최초 사용자 클릭 | 오디오 정책·Clip·AudioSource |
| 키보드가 안 됨 | Canvas 포커스 | 입력 Action·페이지 입력 충돌 |

Network에서 요청 상태·전송 크기·로드 시간을 기록합니다. 캐시를 유지한 재접속과 Disable cache를 사용한 첫 로드를 나눠 측정합니다. 제공된 ContentServer는 압축 파일의 Content-Encoding을 처리하지 않습니다. Gzip·Brotli 비교는 DAY 15에서 서버 설정까지 추가한 뒤 진행합니다.

### 실행해보면

첫 사용자 클릭 전에는 브라우저가 오디오를 제한할 수 있습니다. 코인 수집 전에 버튼을 눌러 오디오를 시작하는 흐름을 확인합니다. 탭을 배경으로 보내면 업데이트·렌더링이 제한될 수 있고, 브라우저를 앱의 종료 버튼으로 강제 종료하는 방식을 사용하지 않습니다.

## 4. Web 제약 비교

동기 파일 읽기·네이티브 플러그인·스레드·네트워크 기능은 설치 Unity 버전과 브라우저 지원 범위를 확인합니다. PC의 `System.Net.Sockets` 코드를 그대로 이식하지 않습니다. 브라우저 저장 영역도 일반 OS 디렉터리와 다릅니다. WebGL과 WebGPU 지원 범위를 구분하고 본 과정은 폭넓은 호환성을 위한 기본 Web 설정으로 진행합니다.

### Unity 6.6의 Web 설정 선택

Unity 6.6에서는 WebGPU가 실험 단계를 벗어났습니다. WebAssembly64와 Progressive Asset Loading도 추가되었습니다. 모든 기능을 한 번에 켜기보다 실행할 브라우저와 다운로드 목표에 맞춰 선택합니다.

| 항목 | 기본 실습 설정 | 선택 이유와 확장 방향 |
| :--- | :--- | :--- |
| 그래픽 API | WebGL 2를 사용하는 구성 | 브라우저 간 기본 동작을 먼저 확인합니다. WebGPU는 별도 프로필이나 프로젝트 복사본에서 비교하고 지원·대체 동작을 확인합니다. |
| WebAssembly 아키텍처 | WebAssembly32 | 작은 코인 수집 게임의 시작점으로 사용합니다. WebAssembly64는 더 큰 메모리를 다룰 수 있지만 Safari 미지원 등 브라우저 제약이 있으며, 초기 로드 시간에도 영향을 줄 수 있습니다. |
| Progressive Asset Loading | 첫 비교에서는 끄기 | 씬 단위로 자산을 나누어 읽는 기능입니다. 기본 실행을 확인한 뒤 켜서 최초 다운로드와 씬 진입 시간을 비교합니다. Addressables 원격 배포와는 별도 기능입니다. |
| 멀티스레드 | 첫 실습에서는 끄기 | 서버의 교차 출처 격리 등 추가 조건이 필요하므로 기본 ContentServer 실습과 분리합니다. |

자동으로 선택된 기본값에 의존하지 말고, 설치한 Editor의 Web Player Settings에서 실제 설정을 확인해 기록합니다. 멀티스레드 등 추가 기능을 켜면 서버 헤더 조건도 달라질 수 있으므로 첫 실습과 같은 조건으로 섞어 비교하지 않습니다. [Unity 6.6 Web 변경 사항](https://docs.unity.com/en-us/engine/6000.6/manual/whats-new/unity66)

## 프로젝트 완료 확인

- [ ] HTTP 페이지에서 시작부터 결과까지 한 판을 완료합니다.
- [ ] Console·Network·오디오·포커스·저장을 확인했습니다.

오늘의 프로젝트를 `Progress/Day12`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. 빌드 성공 후 브라우저 빈 화면이 뜬다면 게임 C#만 조사하면 충분할까요?
2. 캐시가 있는 로드와 첫 다운로드를 왜 나눠야 하나요?

## 오늘의 정리

Web에서 오류가 발생하면 게임 코드, 브라우저, 서버를 함께 살펴봅니다. 다음 수업에서는 PC, Android, Web의 파일 경로와 데이터를 요청하는 흐름을 비교합니다.
