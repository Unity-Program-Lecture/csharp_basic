# DAY 03: HTTP/HTTPS 로그인과 게임 인증 흐름

오늘은 게임 서버에 접속하기 전, 계정을 확인하고 인증 결과를 받는 HTTP/HTTPS 요청을 콘솔 프로젝트로 실습합니다. 비밀번호를 TCP 게임 메시지로 보내지 않고, HTTPS 로그인 뒤 발급받은 토큰으로 게임 서버 접속을 증명하는 흐름을 이해합니다.

## 1. HTTP와 HTTPS: "봉투가 열린 편지와 잠긴 편지"

- **HTTP (Hypertext Transfer Protocol)**: 클라이언트가 요청을 보내고 서버가 응답을 돌려주는 통신 규칙입니다.
- **HTTPS (Hypertext Transfer Protocol Secure)**: HTTP 통신에 TLS 보안을 적용한 방식입니다.
- **TLS (Transport Layer Security)**: 전송 중인 내용을 암호화하고, 접속한 서버가 맞는지 확인하는 보안 규약입니다.
- **요청 (Request)**: 클라이언트가 서버에 처리해 달라고 보내는 메시지입니다.
- **응답 (Response)**: 서버가 요청을 처리한 뒤 돌려주는 결과입니다.
- **API (Application Programming Interface)**: 프로그램끼리 기능을 요청하고 결과를 주고받기 위한 약속입니다.
- **JSON (JavaScript Object Notation)**: 데이터를 키와 값이 있는 텍스트 형태로 표현하는 형식입니다.
- **POST 요청 (POST request)**: 서버에 새 데이터나 처리할 데이터를 전달하는 HTTP 요청 방식입니다.
- **ASP.NET Core**: .NET으로 HTTP/HTTPS 서버와 API를 만드는 웹 개발 프레임워크입니다.

HTTP는 내용이 암호화되지 않은 편지와 같고, HTTPS는 TLS로 잠근 편지와 같습니다. 로그인 비밀번호, 인증 토큰, 결제 정보처럼 노출되면 안 되는 값은 HTTPS로 보냅니다.

| 구분 | HTTP | HTTPS |
| :--- | :--- | :--- |
| 전송 내용 | 암호화되지 않음 | TLS로 암호화됨 |
| 서버 확인 | 서버가 맞는지 검증하지 않음 | 인증서를 통해 서버를 확인함 |
| 게임 사용 | 로컬 단순 테스트 외에는 권장하지 않음 | 로그인, 계정, 상점, 공지, 원격 설정 API |

HTTPS는 로그인·계정 생성·점검 공지·상점·결제·게임 설정처럼 "요청 하나에 응답 하나"를 받는 기능에 주로 사용합니다. 매 프레임 이동 상태처럼 매우 자주 바뀌는 데이터는 이후 DAY04의 TCP 또는 DAY05의 UDP와 같은 전송 방식을 검토합니다.

## 2. 게임의 로그인과 TCP 접속은 다른 단계입니다

**액세스 토큰 (access token)** 은 인증 서버가 로그인 성공 후 발급하는, 짧은 시간 동안 유효한 사용자 확인표입니다. **PlayerId (Player Identifier)** 는 게임 안에서 플레이어를 구분하는 고유 번호입니다.

```text
콘솔 클라이언트 → HTTPS /login → 인증 서버
콘솔 클라이언트 ← accessToken, PlayerId ← 인증 서버
        ↓
이후 TCP 게임 서버에 연결
        ↓
첫 메시지 AUTH|accessToken 전송
        ↓
게임 서버가 토큰을 확인한 뒤 PlayerId와 연결을 등록
```

`AUTH`는 이 과정의 예시에서 "인증 요청"을 뜻하도록 정한 메시지 타입입니다. 오늘 실습은 HTTPS 로그인과 토큰 발급까지만 구현하며, DAY04에서 이 토큰이 TCP 첫 메시지로 쓰이는 이유를 연결합니다.

## 3. 환경 준비: 개발용 HTTPS 인증서

**개발용 인증서 (development certificate)** 는 내 컴퓨터의 `localhost`에서 HTTPS를 시험하기 위해 사용하는 인증서입니다. **`localhost`** 는 현재 실행 중인 내 컴퓨터를 가리키는 이름입니다. 인증서는 서버의 신원을 확인하고 TLS 암호화를 시작할 때 사용하는 디지털 증명서이며, 이 실습의 개발용 인증서는 실제 서비스용 인증서가 아닙니다.

PowerShell에서 다음 명령을 한 번 실행합니다.

```powershell
dotnet dev-certs https --trust
```

인증서가 유효하고 신뢰되는지 확인할 때는 다음 명령을 사용합니다.

```powershell
dotnet dev-certs https --check --trust
```

> 이 실습은 같은 컴퓨터에서 실행합니다. 휴대폰이나 다른 PC의 `localhost`는 서버 PC가 아니라 각 기기 자신을 뜻합니다.

### Visual Studio 2022 또는 이후 버전에서 프로젝트 만들기

Visual Studio 2022와 이후 버전 (예: Visual Studio 2026)은 화면의 세부 배치가 달라도 아래 절차로 같은 프로젝트를 만들 수 있습니다.

1. Visual Studio Installer에서 **ASP.NET 및 웹 개발 (ASP.NET and web development)** 워크로드가 설치되어 있는지 확인합니다. 템플릿이 보이지 않으면 Visual Studio에서 `도구` → `도구 및 기능 가져오기`를 선택해 워크로드를 추가합니다.
2. Visual Studio에서 `새 프로젝트 만들기`를 선택하고, 검색창에 `ASP.NET Core Empty`를 입력합니다.
3. **ASP.NET Core Empty** 템플릿을 선택하고 프로젝트 이름을 `AuthServerLab`으로 지정합니다.
4. 추가 정보에서 설치된 **LTS (Long Term Support, 장기 지원)** .NET 버전을 선택하고, 인증 방식은 `없음 (None)`으로 둡니다. HTTPS 설정 항목이 보이면 활성화합니다.
5. 프로젝트를 만든 뒤 `Program.cs`를 아래 서버 코드로 바꿉니다. 실행은 상단의 `https` 실행 프로필을 선택한 뒤 `F5` 또는 `Ctrl+F5`를 누릅니다.

클라이언트는 같은 방법으로 `콘솔 앱 (Console App)` 템플릿을 선택해 `AuthClientLab`이라는 별도 프로젝트로 만듭니다. 서버와 클라이언트를 동시에 실행해야 하므로, 서버는 한 Visual Studio 창에서 `Ctrl+F5`로 실행한 상태로 두고 클라이언트는 두 번째 Visual Studio 창 또는 PowerShell에서 실행합니다.

## 4. 안내형 실습 1: HTTPS 인증 서버 만들기

**미션:** `player01`/`1234` 로그인에 성공하면 `accessToken`과 `PlayerId`를 JSON으로 돌려주는 HTTPS 서버를 실행합니다.

1. PowerShell에서는 `dotnet new web -n AuthServerLab`으로, Visual Studio에서는 위의 **ASP.NET Core Empty** 절차로 웹 서버 프로젝트를 만듭니다.
2. `AuthServerLab/Program.cs`를 아래 코드로 모두 바꿉니다.

```csharp
using System;
using System.Collections.Concurrent;

// Program은 HTTPS 인증 서버를 시작하는 클래스입니다.
public class Program
{
    // ConcurrentDictionary는 여러 요청이 동시에 와도 안전하게 값을 보관하는 컬렉션입니다.
    private static readonly ConcurrentDictionary<string, int> IssuedTokens =
        new ConcurrentDictionary<string, int>();

    // Main은 서버 프로그램의 시작 메서드입니다.
    public static void Main(string[] args)
    {
        // WebApplicationBuilder는 ASP.NET Core 웹 서버 설정을 만드는 객체입니다.
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        // WebApplication은 HTTP/HTTPS 요청을 받는 서버 객체입니다.
        WebApplication app = builder.Build();

        // MapPost는 POST 요청 주소와 처리 메서드를 연결합니다.
        app.MapPost("/login", Login);
        app.MapPost("/validate", Validate);

        // Run은 localhost 5001번 포트에서 HTTPS 서버를 실행합니다.
        app.Run("https://localhost:5001");
    }

    // Login은 /login 요청의 계정 정보를 확인하고 토큰을 발급합니다.
    private static IResult Login(LoginRequest request)
    {
        // 학습용 고정 계정입니다. 실제 서비스는 DB와 비밀번호 해시를 사용합니다.
        if (request.Id != "player01" || request.Password != "1234")
        {
            return Results.Unauthorized();
        }

        // Guid는 중복 가능성이 매우 낮은 식별자를 만드는 타입입니다.
        string accessToken = Guid.NewGuid().ToString("N");
        IssuedTokens[accessToken] = 101;

        LoginResponse response = new LoginResponse();
        response.AccessToken = accessToken;
        response.PlayerId = 101;

        return Results.Ok(response);
    }

    // Validate는 게임 서버가 토큰을 확인하는 상황을 단순화한 API입니다.
    private static IResult Validate(TokenRequest request)
    {
        int playerId;

        if (!IssuedTokens.TryGetValue(request.AccessToken, out playerId))
        {
            return Results.Unauthorized();
        }

        ValidateResponse response = new ValidateResponse();
        response.PlayerId = playerId;

        return Results.Ok(response);
    }
}

// LoginRequest는 클라이언트가 보내는 로그인 JSON 구조입니다.
public class LoginRequest
{
    public string Id { get; set; }
    public string Password { get; set; }
}

// LoginResponse는 서버가 돌려주는 로그인 JSON 구조입니다.
public class LoginResponse
{
    public string AccessToken { get; set; }
    public int PlayerId { get; set; }
}

// TokenRequest는 토큰 검증 요청의 JSON 구조입니다.
public class TokenRequest
{
    public string AccessToken { get; set; }
}

// ValidateResponse는 토큰 검증 성공 뒤 돌려주는 JSON 구조입니다.
public class ValidateResponse
{
    public int PlayerId { get; set; }
}
```

3. 첫 PowerShell 창에서 `dotnet run --project AuthServerLab`을 실행합니다.
4. `https://localhost:5001`에서 서버가 실행 중이라는 메시지를 확인합니다.

## 5. 안내형 실습 2: 콘솔 클라이언트로 HTTPS 로그인하기

**미션:** 콘솔 클라이언트가 JSON 로그인 요청을 보내고 응답의 `PlayerId`와 `accessToken`을 출력합니다.

1. 두 번째 PowerShell 창에서는 `dotnet new console -n AuthClientLab`으로, Visual Studio에서는 위에서 설명한 **콘솔 앱** 절차로 콘솔 프로젝트를 만듭니다.
2. `AuthClientLab/Program.cs`를 아래 코드로 모두 바꿉니다.

```csharp
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// Program은 HTTPS 로그인 콘솔 클라이언트의 시작 클래스입니다.
public class Program
{
    private const string ServerUrl = "https://localhost:5001";

    // Main은 일반적인 콘솔 시작 형식으로 비동기 작업을 끝까지 기다립니다.
    public static void Main(string[] args)
    {
        RunAsync().GetAwaiter().GetResult();
    }

    // RunAsync는 로그인 입력, HTTPS 요청, 응답 출력을 순서대로 처리합니다.
    private static async Task RunAsync()
    {
        Console.Write("아이디: ");
        string id = Console.ReadLine();
        Console.Write("비밀번호: ");
        string password = Console.ReadLine();

        LoginRequest loginRequest = new LoginRequest();
        loginRequest.Id = id;
        loginRequest.Password = password;

        // JsonSerializer는 C# 객체를 JSON 문자열로 바꾸는 도구입니다.
        string requestJson = JsonSerializer.Serialize(loginRequest);

        // HttpClient는 HTTP/HTTPS 요청을 보내는 객체입니다.
        using (HttpClient client = new HttpClient())
        // StringContent는 문자열을 HTTP 요청 본문으로 담는 객체입니다.
        using (StringContent content = new StringContent(
            requestJson,
            Encoding.UTF8,
            "application/json"))
        {
            // PostAsync는 /login 주소로 JSON POST 요청을 비동기로 보냅니다.
            HttpResponseMessage response = await client.PostAsync(
                ServerUrl + "/login",
                content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("로그인 실패: " + response.StatusCode);
                return;
            }

            // ReadAsStringAsync는 응답 본문의 JSON 텍스트를 비동기로 읽습니다.
            string responseJson = await response.Content.ReadAsStringAsync();
            // Deserialize는 JSON 텍스트를 LoginResponse 객체로 읽습니다.
            LoginResponse loginResponse = JsonSerializer.Deserialize<LoginResponse>(
                responseJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            Console.WriteLine("로그인 성공");
            Console.WriteLine("PlayerId: " + loginResponse.PlayerId);
            Console.WriteLine("AccessToken: " + loginResponse.AccessToken);
        }
    }
}

// LoginRequest는 서버에 보낼 로그인 JSON 구조입니다.
public class LoginRequest
{
    public string Id { get; set; }
    public string Password { get; set; }
}

// LoginResponse는 서버가 보낸 로그인 JSON 구조입니다.
public class LoginResponse
{
    public string AccessToken { get; set; }
    public int PlayerId { get; set; }
}
```

3. 두 번째 창에서 `dotnet run --project AuthClientLab`을 실행합니다.
4. 아이디에 `player01`, 비밀번호에 `1234`를 입력합니다.

### 실행해보면

다음과 비슷한 결과가 출력됩니다. 토큰 문자열은 매번 달라집니다.

```text
로그인 성공
PlayerId: 101
AccessToken: 0d3c... (매번 다른 값)
```

### 흔한 오류

| 증상 | 확인할 것 |
| :--- | :--- |
| HTTPS 연결 실패 | 서버가 먼저 실행 중인지, `dotnet dev-certs https --trust`를 실행했는지 확인합니다. |
| 로그인 실패: Unauthorized | 아이디가 `player01`, 비밀번호가 `1234`인지 확인합니다. |
| 주소 사용 불가 | 5001번 포트를 다른 프로그램이 사용 중인지 확인합니다. |

> 이 코드는 학습용입니다. 실제 서비스에서는 고정 계정·메모리 토큰 대신 데이터베이스, 비밀번호 해시, 만료 시간, 안전한 토큰 검증을 사용합니다. 토큰도 로그에 노출하지 않습니다.

## 응용 실습: 로그인 실패와 토큰 검증

1. 비밀번호를 일부러 틀리게 입력해 `Unauthorized` 결과를 확인하세요.
2. 서버의 `IssuedTokens`에 없는 임의 토큰으로 `/validate` 요청을 보내면 어떤 상태 코드가 나올지 적으세요.
3. HTTPS 로그인 뒤 TCP 게임 서버에 연결한다면 첫 메시지를 어떤 형식으로 보낼지 `AUTH|...` 형식으로 작성하세요.

## 오늘의 정리

- HTTP는 요청·응답 방식이며, HTTPS는 HTTP에 TLS 보안을 적용한 방식입니다.
- 게임에서는 로그인, 계정, 상점, 공지, 원격 설정처럼 요청·응답이 필요한 기능에 HTTPS를 주로 사용합니다.
- 비밀번호는 TCP 게임 메시지로 보내지 않고 HTTPS 로그인으로 확인합니다.
- 로그인 성공 뒤 받은 accessToken은 이후 게임 서버 연결을 인증하는 데 사용할 수 있습니다.
- 프로토콜별 선택 기준은 [게임에서 사용하는 네트워크 프로토콜 참고 안내](Supplement/GAME_NETWORK_PROTOCOL_REFERENCE.md)에서 다시 확인할 수 있습니다.
