# DAY 15: 플랫폼별 성능과 콘텐츠 최적화

오늘은 PC, Android, Web에서 성능을 제한하는 원인을 찾아봅니다. 한 번에 한 가지 조건을 바꾸고 결과를 비교하며 플랫폼에 맞는 개선 방법을 익힙니다. 이 활동은 NCS 학습 목표 G7·G8·G9에 해당합니다.

## 1. 핵심 개념: 시간·공간·전력의 교환

텍스처 크기를 줄이면 메모리는 아낄 수 있지만 화면의 선명도가 낮아질 수 있습니다. 자산을 미리 불러오면 플레이 중 지연은 줄어들어도 처음 기다리는 시간과 메모리 사용량이 늘 수 있습니다. 성능을 개선할 때는 얻는 이점과 함께 달라지는 품질이나 비용도 확인합니다.

### 이 단어는 무슨 뜻인가요?

- **프레임 예산**: 목표 프레임당 사용할 수 있는 시간입니다.
- **텍스처 압축**: 지원 하드웨어의 형식으로 영상 자료를 줄이는 방법입니다.
- **오버드로우**: 같은 화면 영역을 여러 번 그리는 비용입니다.
- **캐시 로드**: 이전에 다운로드한 데이터를 다시 사용하는 방식입니다. 처음 다운로드하는 경우와 구분합니다.

## 2. 실습: 플랫폼별 프레임 정책

**미션:** `Assets/Scripts/Runtime/FramePolicy.cs`를 작성해 기존 GameSession 오브젝트에 붙이고 실행 환경에서 확인합니다.

```csharp
using UnityEngine;

public sealed class FramePolicy : MonoBehaviour
{
    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // 고정 FPS를 요청하지 않고 브라우저의 기본 프레임 스케줄링을 따릅니다.
        Application.targetFrameRate = -1;
#elif UNITY_ANDROID && !UNITY_EDITOR
        // 모바일의 목표 FPS는 targetFrameRate로 정합니다. 수업에서는 30으로 시작해 발열·전력을 비교합니다.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
#else
        // PC와 Editor는 화면 주사율을 따르도록 수직 동기화를 한 주기마다 수행합니다.
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = -1;
#endif
    }
}
```

이 프레임 정책은 수업에서 비교하기 위한 시작값입니다. 실제 동작은 화면 재생률과 플랫폼에 따라 확인해야 합니다. Android에서 30 FPS와 60 FPS를 비교할 때는 목표 프레임을 바꾼 효과와 처리 비용을 줄인 효과를 구분합니다.

## 3. 플랫폼별 개선 실습

### 먼저 화면 비율에 따른 배치 보완하기

DAY 04에서 기록한 겹침·가림을 확인하고 아래 RectTransform을 수정합니다. 크기는 유지하고 Anchor Min·Max와 Pivot을 같은 값으로 설정합니다. 위치는 부모 기준 Anchored Position입니다.

| 오브젝트 | Anchor Min·Max / Pivot | 위치 |
| :--- | :--- | :--- |
| HudPanel | (0.5,1) / (0.5,1) | (0,-28) |
| BestText | (1,1) / (1,1) | (-24,-24) |
| SaveButton | (1,1) / (1,1) | (-24,-86) |
| MobileControls | (0,0) / (0,0) | (30,20) |

BestText는 오른쪽 정렬로 바꿉니다. SafeAreaRoot와 SafeAreaFitter는 유지합니다. 16:9와 가로로 긴 화면에서 HUD·최고 점수·저장·방향 버튼이 보이고 서로 겹치지 않는지 다시 확인합니다. 화면 배치 수정과 CPU·메모리 성능 개선의 결과는 따로 기록합니다.

### 플랫폼별 비용 비교하기

| 환경 | 조사 대상 | 한 번에 바꿀 조건 | 다시 확인할 것 |
| :--- | :--- | :--- | :--- |
| PC | CPU Timeline·GPU·렌더링 | 그림자 거리·실시간 광원 수 | 시각 결과·CPU/GPU 시간 |
| Android | 실기기 프레임·메모리·발열 | 텍스처 Max Size·품질·프레임 정책 | 장시간 프레임·화질·메모리 |
| Web | Network·Performance·Memory | 텍스처 크기·자산 분리·압축 | 최초 다운로드·캐시·실행 메모리 |

Texture Inspector의 플랫폼 Override·Max Size·Format을 확인합니다. 지원 형식과 fallback을 설치 Unity 문서·실기기에서 확인합니다. Android 텍스처 형식을 모든 기기가 동일하게 지원한다고 가정하지 않습니다. Mesh/Texture Read/Write를 끄기 전 런타임 CPU 읽기 기능이 있는지 확인합니다.

URP의 품질 설정에서는 활성 URP Asset과 Quality별 연결을 추적합니다. Render Scale·Shadow Distance를 조정한 경우 사용 중인 프로필과 화면 해상도를 기록합니다. UGUI는 불필요한 문자열 갱신·Raycast Target·겹친 반투명 UI를 조사합니다.

Addressables는 첫 요청·이미 로드한 요청·해제 후 다시 요청을 구분합니다. 중복 자산과 번들 크기를 보고, 너무 큰 묶음을 나누는 대신 요청 수가 늘어나는 비용도 확인합니다. Scene 직접 참조와 Addressable 참조가 중복되지 않는지 조사합니다.

## 4. 선택 실습: Web 압축과 서버 응답

필수 최종본은 DAY 12의 Disabled 설정으로 먼저 완성합니다. 압축을 비교한다면 기존 출력과 ContentServer 소스를 백업하고 다음을 진행합니다. DAY 08의 서버는 .gz·.br 확장자를 아직 제공하지 않으므로 Unity의 압축 설정만 켜면 404나 해제 오류가 납니다.

### ASP.NET Core 서버 수정

Program.cs의 types.Mappings 등록 부분에 다음 세 줄을 추가합니다.

```csharp
            types.Mappings[".gz"] = "application/octet-stream";
            types.Mappings[".br"] = "application/octet-stream";
            // Decompression Fallback 파일은 아래 gzip·br 처리에서 제외하고 Unity의 해제 코드가 읽게 합니다.
            types.Mappings[".unityweb"] = "application/octet-stream";
```

기존 StaticFileOptions 안의 OnPrepareResponse 항목을 다음으로 교체합니다. FileProvider와 ContentTypeProvider는 그대로 둡니다. OnPrepareResponse를 두 개 추가하지 않습니다.

```csharp
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.CacheControl = "no-cache";
                    string extension = Path.GetExtension(context.File.Name);
                    if (extension == ".gz" || extension == ".br")
                    {
                        // 마지막 확장자만 제거합니다. game.wasm.gz라면 game.wasm으로 원래 파일 형식을 찾습니다.
                        string originalName = Path.GetFileNameWithoutExtension(context.File.Name);
                        if (types.TryGetContentType(originalName, out string? contentType))
                        {
                            // 압축된 파일이어도 Content-Type은 원래 데이터 형식(application/wasm 등)으로 보냅니다.
                            context.Context.Response.ContentType = contentType;
                        }

                        // 이미 압축된 파일임을 브라우저에 알려 자동 해제하게 합니다. 서버에서 다시 압축하지 않습니다.
                        context.Context.Response.Headers.ContentEncoding =
                            extension == ".gz"
                                ? "gzip"
                                : "br";
                    }
                }
```

Path는 DAY 08의 using System.IO를 그대로 사용합니다.

### 새 출력으로 비교

1. Unity의 Web 프로필에서 Compression Format을 Gzip, Decompression Fallback을 끄기로 설정합니다. 로컬 Web 콘텐츠가 현재 소스와 일치하는지 확인하고 `Builds/WebGzip`에 Player를 새로 빌드합니다.
2. 기존 서버를 Ctrl+C로 종료하고 ContentServer 폴더에서 아래 명령으로 실행합니다. dotnet run이 수정한 서버를 다시 빌드합니다.

```powershell
dotnet run --no-launch-profile -- --FilesRoot "../PlatformCoinLab/Builds/WebGzip"
```

3. <https://localhost:5443/>을 열고 DevTools의 Disable cache를 켠 뒤 다시 로드합니다. Network에서 .gz 요청의 200 응답·Content-Encoding·Content-Type과 실제 게임 진행을 확인합니다. 정상 페이지가 열리는 것과 실제 라운드 종료까지 동작하는 것을 함께 기록합니다.
4. Brotli도 비교한다면 Compression Format을 바꾸고 `Builds/WebBrotli`에 새로 빌드합니다. 서버의 FilesRoot를 그 폴더로 바꾸고 HTTPS에서 같은 항목을 확인합니다.
5. Decompression Fallback을 켜는 비교는 별도 출력에서 진행합니다. 파일은 .unityweb으로 바뀌고 이 서버는 application/octet-stream으로 제공합니다. Content-Encoding을 추가하지 않아 Unity의 JavaScript 해제기가 처리하게 합니다. 네이티브 해제와 로드 비용이 다름을 기록합니다.

압축 실험을 마치면 필수 최종본의 Compression Format을 Disabled로 복원하고 새 출력으로 다시 확인합니다. 실험 소스·설정·출력과 측정 자료는 따로 보관합니다. [Unity Web 배포와 압축 안내](https://docs.unity3d.com/6000.6/Documentation/Manual/webgl-deploying.html)를 함께 참고합니다.

캐시가 있는 다운로드 크기를 초기 크기 감소라고 표시하지 않습니다. 압축된 전송 크기와 로드 후 메모리는 다른 지표입니다. 크기가 작아져도 파싱·압축 해제 비용 때문에 로드 시간이 길어질 수 있습니다.

## 5. 전후 보고서

### Unity 6.6 기능을 비교할 때

DAY 12의 기본 WebGL 2·WebAssembly32 빌드를 기준으로 WebGPU나 Progressive Asset Loading을 하나씩 바꾸어 비교할 수 있습니다. 브라우저·그래픽 API·아키텍처를 결과에 함께 적고, 전송 크기와 실행 메모리, 첫 화면까지의 시간, 씬 진입 시간을 나누어 측정합니다. WebAssembly64는 큰 메모리가 필요한 경우의 선택지이며 작은 게임의 성능을 자동으로 높이는 옵션은 아닙니다.

DAY 07의 콘텐츠 디렉토리 비교도 같은 원칙으로 진행합니다. 패키지와 콘텐츠 빌드 방식이 달라졌음을 명시하고, 로컬 로드 비교와 원격 다운로드 비교를 구분합니다. 새 기능을 사용했다는 사실보다 게임 동작을 유지하면서 어떤 비용이 달라졌는지 설명합니다.

### 측정 결과 작성

[기록 양식](PRACTICE_REPORT_TEMPLATE.md)을 사용해 같은 장치, 해상도, 부하, 측정 옵션으로 3회 측정합니다. 결과에는 “최적화됨”이라는 표현과 함께 실제로 달라진 지표를 적습니다. 예를 들어 특정 마커 구간의 처리 시간, 메모리 할당량, 로드 시간이 얼마에서 얼마로 바뀌었는지 기록합니다. 아직 측정하지 않은 값은 `미측정`으로 남깁니다.

## 프로젝트 완료 확인

- [ ] 한 번에 한 조건을 바꾸고 실제 측정값을 기록했습니다.
- [ ] 최종 후보 빌드와 알려진 문제를 정리했습니다.

오늘의 프로젝트를 `Progress/Day15`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. 다운로드 파일이 절반이 되면 실행 메모리도 절반이 되나요?
2. Android 60→30 FPS 변경을 알고리즘 개선과 구분해야 하는 이유는 무엇인가요?

## 오늘의 정리

성능 수치는 실행한 플랫폼과 측정 조건을 함께 적어야 비교할 수 있습니다. 다음 수업에서는 기능 테스트와 성능 측정 기록을 정리하고, 문서와 빌드 결과물을 함께 전달합니다.
