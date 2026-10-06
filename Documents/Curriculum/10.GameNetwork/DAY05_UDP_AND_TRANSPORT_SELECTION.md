# DAY 05: UDP와 전송 방식 선택

오늘은 빠르지만 도착을 보장하지 않는 UDP를 TCP와 비교합니다.

## 1. 핵심 개념: "확인 전화 없는 빠른 방송"

- **UDP (User Datagram Protocol)**: 연결을 만들지 않고 데이터그램을 빠르게 보내는 통신 방식입니다.
- **데이터그램 (Datagram)**: UDP로 보내는 독립된 데이터 묶음입니다.
- **손실 (Packet Loss)**: 보낸 데이터가 도착하지 않는 현상입니다.
- **지연 (Latency)**: 데이터를 보낸 시점과 받은 시점 사이의 시간입니다.
- **TCP (Transmission Control Protocol)**: 연결 위에서 데이터의 순서와 전달을 관리하는 전송 규약입니다.
- **IP (Internet Protocol)**: 컴퓨터 사이에서 데이터를 보낼 주소와 전달 방식을 정하는 규약입니다.
- **UTF-8 (Unicode Transformation Format - 8-bit)**: 한글 등을 포함한 문자를 바이트로 표현하는 인코딩 방식입니다.

## 2. 안내형 실습: UDP 위치 메시지

**미션:** 송신자는 위치 문자열을 보내고, 수신자는 도착한 메시지를 출력합니다.

1. `UdpReceiverLab`, `UdpSenderLab` 콘솔 프로젝트를 만듭니다.
2. 수신자 `Program.cs`를 아래 코드로 모두 바꿉니다.

```csharp
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // UdpClient는 UDP 데이터그램을 보내거나 받는 객체입니다.
        using UdpClient receiver = new(7778);
        Console.WriteLine("UDP 수신 대기");
        // ReceiveAsync는 UDP 데이터그램 하나가 도착할 때까지 기다립니다.
        UdpReceiveResult result = await receiver.ReceiveAsync();
        // UTF8.GetString은 받은 바이트 배열을 사람이 읽는 문자열로 바꿉니다.
        string message = Encoding.UTF8.GetString(result.Buffer);
        Console.WriteLine($"수신: {message}");
    }
}
```

3. 송신자 `Program.cs`를 아래 코드로 모두 바꿉니다.

```csharp
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using UdpClient sender = new();
        // UTF8.GetBytes는 전송할 문자열을 네트워크 바이트 배열로 바꿉니다.
        byte[] bytes = Encoding.UTF8.GetBytes("POSITION:3,5");
        // IPEndPoint는 보낼 대상의 IP 주소와 포트를 함께 나타냅니다.
        await sender.SendAsync(bytes, bytes.Length, new IPEndPoint(IPAddress.Loopback, 7778));
        Console.WriteLine("전송: POSITION:3,5");
    }
}
```

4. 첫 PowerShell 창에서 `dotnet run --project UdpReceiverLab`을 실행합니다.
5. 두 번째 PowerShell 창에서 `dotnet run --project UdpSenderLab`을 실행합니다.

### 실행해보면

수신자 콘솔에 `POSITION:3,5`가 표시되면 성공입니다.

## 3. 안내형 실습: 서로 보내고 받는 UDP 피어

**미션:** 같은 프로그램을 두 번 실행하여 A와 B가 각각 메시지를 입력하고 상대 메시지를 받습니다.

앞 실습은 송신 역할과 수신 역할을 따로 배웠습니다. UDP가 송신 전용 또는 수신 전용인 것은 아닙니다. 하나의 UdpClient로 양쪽 역할을 할 수 있으며, TCP처럼 연결을 수락하는 과정은 없습니다. 그렇다고 상대가 실행 중인지, 메시지가 도착했는지 자동으로 확인해 주는 것은 아닙니다.

### 이 단어는 무슨 뜻인가요?

- **피어 (Peer)**: 서로 통신하는 참여자입니다. 이번 실습에서는 A와 B 모두 송신자이자 수신자입니다.
- **포트 (Port)**: 컴퓨터 안에서 데이터를 받을 통로의 번호입니다. A는 7790번, B는 7791번에서 받습니다.
- **엔드포인트 (Endpoint)**: 통신 주소인 IP 주소와 포트를 묶은 것입니다. 예를 들어 `127.0.0.1:7790`입니다.
- **루프백 (Loopback)**: 같은 컴퓨터로 데이터를 보내는 주소입니다. 여기서는 `127.0.0.1`을 사용합니다.

### 환경 확인과 프로젝트 준비

1. 실습용 빈 폴더를 VS Code로 열고 내부 터미널에서 `dotnet --version`으로 .NET SDK가 설치되어 있는지 확인합니다. 이 예제는 .NET 8 콘솔 프로젝트를 기준으로 합니다.
2. 다음 명령으로 콘솔 프로젝트 하나를 만듭니다. 별도 NuGet 패키지는 필요 없습니다.

```powershell
dotnet new console -n UdpPeerLab --framework net8.0
```

3. 생성된 `UdpPeerLab/Program.cs`를 아래 코드로 모두 바꿉니다. 서버·클라이언트 프로젝트를 따로 만들 필요는 없습니다.

<details>
<summary>코드 보기: 입력·송신과 수신을 함께 실행하는 UDP 피어</summary>

```csharp
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // args는 실행 명령에서 -- 뒤에 전달한 문자열 배열입니다.
        if (args.Length != 3
            || !int.TryParse(args[0], out int localPort)
            || !int.TryParse(args[1], out int remotePort)
            || localPort < 1 || localPort > 65535
            || remotePort < 1 || remotePort > 65535)
        {
            Console.WriteLine("사용법: dotnet run -- <내 포트> <상대 포트> <이름>");
            Console.WriteLine("포트는 1~65535 사이의 정수를 사용하세요.");
            return;
        }

        string name = args[2];
        // Loopback은 같은 컴퓨터를 가리킵니다. 지정한 로컬 주소와 포트에서 받습니다.
        // using은 범위를 벗어날 때 Dispose를 호출하여 소켓 자원을 정리합니다.
        using (UdpClient peer = new UdpClient(new IPEndPoint(IPAddress.Loopback, localPort)))
        {
            IPEndPoint destination = new IPEndPoint(IPAddress.Loopback, remotePort);
            Console.WriteLine($"{name}: 내 포트 {localPort}, 상대 포트 {remotePort}");
            Console.WriteLine("메시지를 입력하세요. /exit으로 종료합니다.");

            // 수신과 입력·송신을 함께 진행합니다.
            Task receiveTask = ReceiveMessagesAsync(peer);
            // Console.ReadLine은 동기 입력이므로 스레드 풀에서 입력·송신을 실행합니다.
            Task sendTask = Task.Run(() => SendMessagesAsync(peer, destination, name));
            // WhenAll은 이미 시작한 두 작업이 모두 끝날 때까지 기다립니다.
            await Task.WhenAll(sendTask, receiveTask);
        }
    }

    private static async Task SendMessagesAsync(
        UdpClient peer, IPEndPoint destination, string name)
    {
        try
        {
            while (true)
            {
                string? message = Console.ReadLine();
                if (message is null || message == "/exit")
                {
                    return;
                }
                if (!string.IsNullOrWhiteSpace(message))
                {
                    // GetBytes는 이름과 메시지를 UTF-8 바이트 배열로 바꿉니다.
                    byte[] bytes = Encoding.UTF8.GetBytes($"[{name}] {message}");
                    // SendAsync는 지정한 상대에게 데이터그램 하나를 보냅니다.
                    await peer.SendAsync(bytes, bytes.Length, destination);
                    Console.WriteLine($"전송: {message}");
                }
            }
        }
        catch (SocketException exception)
        {
            Console.WriteLine($"송신 실패: {exception.Message}");
        }
        finally
        {
            // 입력이 끝나면 소켓을 닫아 대기 중인 ReceiveAsync도 종료시킵니다.
            peer.Close();
        }
    }

    private static async Task ReceiveMessagesAsync(UdpClient peer)
    {
        try
        {
            while (true)
            {
                // UdpReceiveResult는 받은 바이트와 실제 송신자의 주소·포트를 담습니다.
                UdpReceiveResult result = await peer.ReceiveAsync();
                string message = Encoding.UTF8.GetString(result.Buffer);
                Console.WriteLine($"수신 {result.RemoteEndPoint}: {message}");
            }
        }
        catch (ObjectDisposedException)
        {
            // /exit으로 소켓을 닫으면 수신 대기가 이 예외로 끝날 수 있습니다.
        }
        catch (SocketException exception)
        {
            // Windows에서는 소켓을 닫을 때도 SocketException이 발생할 수 있습니다.
            // 다른 네트워크 오류일 수도 있으므로 메시지를 출력합니다.
            Console.WriteLine($"수신 종료: {exception.Message}");
        }
    }
}
```

</details>

### 실행 순서

프로젝트를 만든 실습 폴더에서 터미널 두 개를 엽니다.

1. 첫 번째 터미널에서 A를 실행합니다.

```powershell
dotnet run --project UdpPeerLab -- 7790 7791 A
```

2. 두 번째 터미널에서 B를 실행합니다. 내 포트와 상대 포트가 반대입니다.

```powershell
dotnet run --project UdpPeerLab -- 7791 7790 B
```

3. 두 프로그램이 실행된 뒤 A에서 `안녕 B`를 입력하고 Enter를 누릅니다.
4. B에서 `안녕 A`를 입력하고 Enter를 누릅니다.
5. 종료할 때는 **각 터미널에서** `/exit`을 입력합니다. A의 종료가 B에 자동으로 통지되지는 않습니다.

명령의 `--`는 dotnet 옵션과 프로그램에 전달할 인수를 구분합니다. 이후 세 값은 순서대로 내 수신 포트, 상대 수신 포트, 표시 이름입니다.

### 실행해보면

A의 수신 출력:

```text
수신 127.0.0.1:7791: [B] 안녕 A
```

B의 수신 출력:

```text
수신 127.0.0.1:7790: [A] 안녕 B
```

전체 출력 순서는 입력 시점에 따라 달라질 수 있습니다. 상대가 실행되기 전에 보낸 메시지는 나중에 상대를 켜도 다시 전달되지 않습니다. `전송:` 출력도 상대가 받았다는 확인이 아니라 로컬 전송 작업이 끝났다는 뜻입니다.

코드는 위에서 아래로 읽습니다. Main에서 주소를 만들고 수신·송신 작업을 시작한 뒤 Task.WhenAll로 두 작업의 종료를 기다립니다. 송신 메서드는 입력을 문자열에서 바이트로 바꾸어 보내고, 수신 메서드는 받은 바이트를 문자열로 바꾸어 출력합니다.

이 실습에서는 상대 주소를 고정했지만 ReceiveAsync는 내 포트에 도착한 다른 송신자의 데이터그램도 받을 수 있습니다. 출력 이름은 송신자가 넣은 문자열이지 인증된 사용자 ID가 아닙니다.

### 문제가 생기면

- **포트가 이미 사용 중:** 이전 A·B 프로그램을 종료하고 다시 실행합니다. 두 프로그램을 같은 내 포트로 실행하지 않습니다.
- **메시지가 안 보임:** 두 프로그램을 모두 실행했는지, 내 포트와 상대 포트를 반대로 지정했는지 확인합니다.
- **상대를 종료했는데 내 프로그램은 남아 있음:** UDP는 상대 종료를 자동 통지하지 않습니다. 내 터미널에서도 /exit을 입력합니다.
- **수신 종료 오류가 보임:** /exit 직후라면 소켓을 닫으며 발생할 수 있습니다. 실행 중 오류라면 오류 메시지와 포트를 확인합니다. 수신 작업만 끝난 경우 입력 대기는 남아 있으므로 /exit으로 종료합니다.

### 응용 실습: 위치 메시지 주고받기

과정 설명 없이 다음 조건을 만족하도록 확장하세요.

- A와 B가 각각 `POSITION:3,5` 같은 위치 메시지를 보낼 수 있습니다.
- 상대에게서 받은 이름과 위치를 출력합니다.
- 전송한 메시지 개수와 받은 메시지 개수를 각각 표시합니다.
- 상대를 종료한 뒤 계속 보냈을 때 전송 성공과 수신 확인이 어떻게 다른지 한 줄로 설명합니다.

## 4. TCP와 UDP 선택표

| 게임 상황 | 우선 선택 | 이유 |
| :--- | :--- | :--- |
| 로그인, 결제, 채팅 | TCP | 순서와 도착이 중요합니다. |
| 빠르게 계속 바뀌는 위치 | UDP 또는 엔진 전송 기능 | 늦은 옛 위치보다 최신 위치가 더 중요할 수 있습니다. |
| 아이템 획득 결과 | TCP 또는 서버 보장 방식 | 결과가 사라지면 안 됩니다. |

## 응용 실습: 선택 이유 쓰기

`총알 발사`, `친구 요청`, `플레이어 위치`에 대해 TCP 또는 UDP를 고르고 한 줄 이유를 적으세요.

## 오늘의 정리

- TCP와 UDP 중 하나가 항상 좋은 것이 아니라, 데이터의 중요도와 최신성에 따라 선택합니다.
- UDP의 송신과 수신은 한 프로그램에서 함께 할 수 있지만, 상대의 접속·종료나 메시지 전달을 자동 확인하지는 않습니다.
