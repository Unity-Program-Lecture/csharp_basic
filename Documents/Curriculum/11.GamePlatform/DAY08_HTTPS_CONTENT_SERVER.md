# DAY 08: ASP.NET Core HTTPS 파일 서버

오늘은 네트워크 과정에서 배운 ASP.NET Core로 HTTPS 파일 서버를 만듭니다. DAY 07의 로컬 게임은 그대로 보관하고, 상태 API와 JSON·정적 파일 응답을 확인합니다. 스테이지 번들을 서버에 연결하는 작업은 다음 수업에서 진행합니다. NCS G3·G7·G8을 다룹니다.

## 1. 핵심 개념: 준비한 파일을 전달하는 창구

로그인 서버가 요청을 처리해 결과를 만든다면, 파일 서버는 지정한 폴더의 파일을 찾아 전달합니다. 파일이 있는 폴더와 요청 URL, 응답 형식을 먼저 확인해야 다음 수업에서 게임의 콘텐츠 요청도 조사할 수 있습니다.

### 이 단어는 무슨 뜻인가요?

- **정적 파일**: 요청할 때 새로 계산하지 않고 저장된 내용을 그대로 전달하는 파일입니다.
- **HTTPS**: 인증서로 서버의 신원을 확인하고 통신을 암호화하는 HTTP 연결입니다.
- **MIME 형식**: 응답이 JSON·JavaScript·번들 중 어떤 데이터인지 알리는 Content-Type 값입니다.
- **FilesRoot**: 이번 서버에서 외부에 제공할 파일 폴더를 지정하는 실행 인수입니다.

## 2. 실습: ASP.NET Core HTTPS 파일 서버 만들기

**미션:** [네트워크 DAY 03](../10.GameNetwork/DAY03_HTTP_HTTPS_GAME_AUTH.md)의 WebApplication과 개발용 인증서를 다시 사용합니다. Python이나 별도의 서버 도구는 설치하지 않습니다.

1. Unity 프로젝트의 상위 작업 폴더에서 터미널을 엽니다. 네트워크 과정에서 사용한 .NET 10 SDK와 ASP.NET 및 웹 개발 도구를 사용합니다. 아래 명령으로 별도 서버를 만듭니다. Visual Studio의 ASP.NET Core Empty 템플릿으로 같은 이름의 프로젝트를 만들어도 됩니다.
2. 생성한 ContentServer.csproj와 Program.cs를 아래 코드로 바꿉니다. 인증 서버의 로그인·토큰 코드는 사용하지 않습니다.
3. ContentServer 폴더 아래 Content 폴더를 만들고 platform-config.json에 아래 JSON을 입력합니다.
4. 네트워크 수업의 개발 인증서는 `dotnet dev-certs https --check --trust`로 확인합니다. 준비하지 않았다면 `dotnet dev-certs https --trust`를 실행하고 Windows의 신뢰 확인을 마칩니다.

```powershell
dotnet new web -n ContentServer --framework net10.0
Set-Location ContentServer
```

작업 폴더는 다음과 같습니다. Unity Hub에서 선택한 프로젝트 위치가 다르다면 FilesRoot 경로를 자신의 위치로 바꿉니다. 서버 코드는 Assets 안에 넣지 않습니다.

```text
자신의 작업 폴더/
  PlatformCoinLab/           1일차부터 만든 Unity 프로젝트
  ContentServer/
    ContentServer.csproj
    Program.cs
    Content/
      platform-config.json
```

### ContentServer.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>disable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

### Program.cs

```csharp
using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace ContentServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // 제공할 파일 폴더 하나를 정합니다. Unity 프로젝트 전체를 지정하지 않습니다.
            // --FilesRoot 인수가 없으면 ??의 오른쪽인 Content 폴더를 사용합니다.
            string folder = builder.Configuration["FilesRoot"] ?? "Content";
            // 상대 경로라면 서버 프로젝트의 콘텐츠 루트를 기준으로 절대 경로를 구합니다.
            string root = Path.GetFullPath(folder, builder.Environment.ContentRootPath);
            if (!Directory.Exists(root))
            {
                throw new DirectoryNotFoundException("제공할 폴더가 없습니다: " + root);
            }

            // using 선언으로 Main이 끝날 때 파일 제공자가 사용한 자원을 정리합니다.
            using PhysicalFileProvider files = new PhysicalFileProvider(root);

            // 수업에서 쓰는 형식만 제공합니다. .cs, .csproj 등은 제공하지 않습니다.
            FileExtensionContentTypeProvider types = new FileExtensionContentTypeProvider();
            types.Mappings.Clear();
            // application은 프로그램이 처리할 데이터, octet-stream은 8비트 단위로 이어진 일반 바이너리 데이터를 뜻합니다.
            types.Mappings[".bundle"] = "application/octet-stream";
            types.Mappings[".hash"] = "text/plain";
            types.Mappings[".json"] = "application/json";
            types.Mappings[".html"] = "text/html";
            types.Mappings[".js"] = "application/javascript";
            types.Mappings[".css"] = "text/css";
            types.Mappings[".wasm"] = "application/wasm";
            types.Mappings[".data"] = "application/octet-stream";
            types.Mappings[".png"] = "image/png";
            types.Mappings[".ico"] = "image/x-icon";
            types.Mappings[".svg"] = "image/svg+xml";

            WebApplication app = builder.Build();
            // Web 빌드의 index.html도 같은 서버로 열 수 있게 합니다.
            app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = files });
            // 기본 문서 경로를 먼저 고른 뒤, 실제 파일과 확장자에 맞는 응답 형식을 제공합니다.
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = files,
                ContentTypeProvider = types,
                // no-cache는 받은 응답을 재사용하기 전에 서버에 확인하라는 뜻입니다. Unity 번들 캐시 삭제는 아닙니다.
                OnPrepareResponse = context =>
                    context.Context.Response.Headers.CacheControl = "no-cache"
            });

            app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

            Console.WriteLine("파일 제공 폴더: " + root);
            // 네트워크 과정에서 준비한 localhost 개발 인증서를 사용합니다.
            app.Run("https://localhost:5443");
        }
    }
}
```

### Content/platform-config.json

```json
{"roundSeconds":45}
```

ContentServer 폴더의 터미널에서 실행합니다. 이 터미널은 서버를 사용하는 동안 열어 둡니다. Ctrl+C로 종료합니다.

```powershell
dotnet run --no-launch-profile
```

브라우저에서 `https://localhost:5443/health`를 열어 상태 JSON, `https://localhost:5443/platform-config.json`으로 45초 설정을 확인합니다. 아직 원격 번들을 만들지 않았어도 이 단계까지는 시험할 수 있습니다. FilesRoot 인수를 생략하면 서버 프로젝트 아래 Content 폴더를 사용합니다.

### 네트워크 과정과 연결

| 코드 | 네트워크 과정에서 이어지는 역할 |
| :--- | :--- |
| WebApplication.CreateBuilder / Build | 요청을 받을 서버 준비 |
| PhysicalFileProvider | 요청 URL에 대응하는 파일 폴더 지정 |
| UseStaticFiles | 폴더의 파일을 GET·HEAD 응답으로 전달 |
| FileExtensionContentTypeProvider | .bundle은 application/octet-stream으로 전달 |
| MapGet("/health", ...) | 서버 상태 JSON을 반환하는 기존 API 방식 |
| Run("https://localhost:5443") | 개발 인증서로 같은 PC의 HTTPS 요청 받기 |

로그인 서버와 파일 서버는 응답을 만드는 방식이 다릅니다. 인증·업로드·폴더 목록은 이번 서버에 추가하지 않습니다. 오늘은 예제 JSON을 둔 Content 폴더를 제공하고, 다음 수업에서는 ServerData처럼 배포할 빌드 출력 폴더를 지정합니다. Unity 프로젝트 전체를 지정하지 않습니다. 등록하지 않은 확장자는 제공하지 않습니다.

인증서 오류가 나면 서버 실행 여부·localhost 이름·인증서 신뢰부터 확인합니다. 인증서 검증을 무조건 통과시키는 CertificateHandler나 브라우저 경고 무시를 해결 방법으로 사용하지 않습니다. .NET 서버와 Unity의 인증서 확인 결과가 다르다면 각각의 오류를 기록하고 신뢰 설정을 확인합니다.

## 3. 실습: 정상 응답과 실패 구분

1. `https://localhost:5443/health`에서 상태 JSON이 나오고 `/platform-config.json`에서 `roundSeconds`가 45인지 확인합니다.
2. 브라우저 개발자 도구의 Network에서 요청 URL·상태 코드·Content-Type을 기록합니다. JSON 파일 응답의 형식도 확인합니다.
3. `/missing-config.json`처럼 존재하지 않는 경로를 요청해 404 응답을 확인합니다. 서버를 Ctrl+C로 종료한 경우의 연결 실패와 구분합니다.
4. 같은 명령으로 서버를 다시 실행하고 정상 JSON을 다시 받는지 확인합니다. 인증서 오류는 파일 경로 오류와 구분해 기록합니다.
5. 오늘은 Content 폴더만 제공합니다. Unity 프로젝트 전체를 FilesRoot로 지정하지 않습니다. DAY 09에서 실제 ServerData 폴더로 전환합니다.

서버를 종료한 뒤 DAY 07의 Windows 로컬 Player를 실행해 서버 없이도 한 판이 끝나는지 확인합니다. 오늘의 서버 작업으로 로컬 콘텐츠의 경로와 Unity 코드를 바꾸지는 않습니다.

## 프로젝트 완료 확인

- [ ] HTTPS 상태 API와 45초 JSON을 확인하고 요청 URL·상태 코드·Content-Type을 기록했습니다.
- [ ] 404·서버 중지·인증서 오류를 구분하고, 재실행 후 정상 응답을 확인했습니다.
- [ ] ContentServer 소스와 로컬 Unity 프로젝트를 각각 보관했습니다.

오늘의 프로젝트를 `Progress/Day08`에 기록합니다. 소스 관리 커밋 또는 Assets·Packages·ProjectSettings 백업을 사용하고, 씬 저장·Inspector 참조·빌드 조건을 [실습 기록 양식](PRACTICE_REPORT_TEMPLATE.md)에 남깁니다. 다음 수업에서 새 Unity 프로젝트를 만들지 않습니다.

### 생각해보기

1. `/health`가 정상 응답하면 스테이지 번들 다운로드도 확인한 것일까요?
2. 파일 경로가 잘못된 경우와 HTTPS 인증서를 신뢰하지 못하는 경우는 어떻게 구분하나요?

## 오늘의 정리

네트워크 과정의 요청·응답 구조를 파일 제공에 적용했습니다. 다음 수업에서는 이 서버에 실제 스테이지 번들을 배포하고 Unity Player의 요청과 실패·복구를 확인합니다.

공식 참고: [ASP.NET Core 정적 파일 제공](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-10.0), [개발용 HTTPS 인증서](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs).
