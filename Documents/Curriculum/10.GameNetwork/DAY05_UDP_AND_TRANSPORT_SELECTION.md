# DAY 05: UDP와 전송 방식 선택

오늘은 빠르지만 도착을 보장하지 않는 UDP를 TCP와 비교합니다.

## 1. 핵심 개념: "확인 전화 없는 빠른 방송"

- **UDP (User Datagram Protocol)**: 연결을 만들지 않고 데이터그램을 빠르게 보내는 통신 방식입니다.
- **데이터그램 (Datagram)**: UDP로 보내는 독립된 데이터 묶음입니다.
- **손실 (Packet Loss)**: 보낸 데이터가 도착하지 않는 현상입니다.
- **지연 (Latency)**: 데이터를 보낸 시점과 받은 시점 사이의 시간입니다.

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

## 3. TCP와 UDP 선택표

| 게임 상황 | 우선 선택 | 이유 |
| :--- | :--- | :--- |
| 로그인, 결제, 채팅 | TCP | 순서와 도착이 중요합니다. |
| 빠르게 계속 바뀌는 위치 | UDP 또는 엔진 전송 기능 | 늦은 옛 위치보다 최신 위치가 더 중요할 수 있습니다. |
| 아이템 획득 결과 | TCP 또는 서버 보장 방식 | 결과가 사라지면 안 됩니다. |

## 응용 실습: 선택 이유 쓰기

`총알 발사`, `친구 요청`, `플레이어 위치`에 대해 TCP 또는 UDP를 고르고 한 줄 이유를 적으세요.

## 오늘의 정리

- TCP와 UDP 중 하나가 항상 좋은 것이 아니라, 데이터의 중요도와 최신성에 따라 선택합니다.
