# DAY 04: TCP 비동기 서버와 클라이언트

오늘은 TCP로 한 명의 클라이언트가 서버에 연결하고 메시지를 주고받게 만듭니다.

## 1. 핵심 개념: "받았다는 확인이 있는 등기 배송"

- **TCP (Transmission Control Protocol)**: 데이터의 도착과 순서를 확인하는 통신 방식입니다.
- **소켓 (Socket)**: 프로그램이 네트워크로 데이터를 보내고 받는 접점입니다.
- **포트 (Port)**: 한 컴퓨터 안에서 어떤 프로그램과 통신할지 구분하는 번호입니다.
- **`TcpListener`**: TCP 연결 요청을 기다리는 서버 객체입니다.
- **`TcpClient`**: TCP 서버에 연결하는 클라이언트 객체입니다.

## 2. 안내형 실습: 한 줄 채팅

**미션:** 서버는 받은 한 줄을 `서버 수신:`으로 출력하고, 클라이언트는 응답을 받습니다.

1. `TcpServerLab`, `TcpClientLab` 콘솔 프로젝트 두 개를 만듭니다.
2. 서버 `Program.cs`를 아래 코드로 모두 바꿉니다.

```csharp
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // TcpListener는 지정한 IP 주소와 포트에서 TCP 연결을 기다리는 서버입니다.
        TcpListener listener = new(IPAddress.Loopback, 7777);
        listener.Start();
        Console.WriteLine("클라이언트 연결 대기");

        // AcceptTcpClientAsync는 Client가 연결될 때까지 비동기로 기다립니다.
        using TcpClient client = await listener.AcceptTcpClientAsync();
        // NetworkStream은 연결된 Client와 바이트를 주고받는 통로입니다.
        using NetworkStream stream = client.GetStream();
        // StreamReader와 StreamWriter는 스트림을 한 줄 문자열로 읽고 씁니다.
        using StreamReader reader = new(stream);
        using StreamWriter writer = new(stream) { AutoFlush = true };

        // ReadLineAsync는 줄바꿈이 포함된 메시지 한 줄을 비동기로 받습니다.
        string? message = await reader.ReadLineAsync();
        Console.WriteLine($"서버 수신: {message}");
        // WriteLineAsync는 메시지 끝에 줄바꿈을 붙여 전송합니다.
        await writer.WriteLineAsync("PONG");
        listener.Stop();
    }
}
```

3. 클라이언트 `Program.cs`를 아래 코드로 모두 바꿉니다.

```csharp
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // TcpClient는 TCP 서버에 연결하는 클라이언트 객체입니다.
        using TcpClient client = new();
        // ConnectAsync는 IP 주소와 포트를 가진 서버에 비동기로 연결합니다.
        await client.ConnectAsync("127.0.0.1", 7777);
        using NetworkStream stream = client.GetStream();
        using StreamReader reader = new(stream);
        using StreamWriter writer = new(stream) { AutoFlush = true };

        await writer.WriteLineAsync("PING");
        string? response = await reader.ReadLineAsync();
        Console.WriteLine($"서버 응답: {response}");
    }
}
```

4. 첫 PowerShell 창에서 `dotnet run --project TcpServerLab`을 실행합니다.
5. 두 번째 PowerShell 창에서 `dotnet run --project TcpClientLab`을 실행합니다.

### 코드에서 `using`을 확인하세요

`TcpClient`, `NetworkStream`, `StreamReader`, `StreamWriter`는 통신이 끝난 뒤 닫아야 하는 `IDisposable` 자원입니다. 코드의 `using TcpClient client = ...;` 선언은 `Main` 메서드가 끝날 때 `Dispose()`를 호출해 연결과 스트림을 정리합니다. Server와 Client 양쪽에서 이 선언을 빼지 않은 이유를 확인하세요.

### 실행해보면

- 서버: `클라이언트 연결 대기` 다음 `서버 수신: PING`
- 클라이언트: `서버 응답: PONG`

### 흔한 오류

| 증상 | 확인할 것 |
| :--- | :--- |
| 연결 거부 | 서버를 먼저 실행했는지, 양쪽 포트가 `7777`인지 확인합니다. |
| 대기 상태로 멈춤 | 클라이언트가 줄바꿈을 포함해 메시지를 보냈는지 확인합니다. |
| 주소 사용 불가 | 이전 서버가 종료됐는지, 같은 포트를 다른 프로그램이 쓰지 않는지 확인합니다. |

## 응용 실습: 두 번째 메시지

클라이언트가 `PING` 뒤 `HELLO`를 한 번 더 보내고, 서버가 각각 응답하도록 확장하세요. 이때 Server와 Client 모두 `ReadLineAsync()`를 두 번 호출하거나 반복문으로 바꿔야 합니다.

## 오늘의 정리

- TCP는 연결을 먼저 만들고 순서 있는 데이터를 교환합니다.
- `await`로 연결·수신을 기다리면 대기 중인 코드를 명확하게 읽을 수 있습니다.
