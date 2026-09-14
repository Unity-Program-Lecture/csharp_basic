# DAY 02: 취소, 시간 초과, 예외 처리

오늘은 DAY01의 비동기 작업을 사용자가 취소하거나 시간이 초과됐을 때 안전하게 끝내는 방법을 콘솔에서 배웁니다.

## 1. 핵심 개념: "닫힌 창구에서 계속 접수하지 않기"

- **취소 토큰 (CancellationToken)**: "이 작업을 그만해도 된다"는 신호입니다.
- **`CancellationTokenSource`**: 취소 신호를 만들고 `Cancel()` 또는 `CancelAfter()`로 신호를 보내는 객체입니다.
- **시간 초과 (Timeout)**: 정한 시간 안에 끝나지 않은 작업을 실패 또는 취소로 처리하는 기준입니다.
- **예외 (Exception)**: 정상 흐름으로 처리할 수 없는 문제가 발생했음을 알리는 객체입니다.
- **동시 작업 (Concurrent Tasks)**: 두 개 이상의 독립 작업을 겹쳐 진행하고 완료를 함께 기다리는 방식입니다.
- **`IDisposable` 인터페이스**: 파일, 네트워크 연결, 타이머처럼 사용을 마치면 정리해야 하는 자원에 `Dispose()` 정리 약속을 제공하는 C# 인터페이스입니다.

### `using`은 두 가지 뜻이 있습니다

코드 첫 줄의 `using System;`은 **using 지시문**입니다. `System.Console` 대신 `Console`처럼 네임스페이스 이름을 짧게 쓰게 합니다.

반면 아래의 `using CancellationTokenSource cts = new();`은 **using 선언**입니다. `cts`를 포함한 블록 또는 메서드가 끝날 때 `Dispose()`를 자동 호출합니다.

```csharp
using CancellationTokenSource cts = new();
// cts를 사용하는 코드
// Main 메서드가 끝나면 cts.Dispose()가 자동 호출됩니다.
```

가비지 컬렉터 (Garbage Collector)가 언젠가 메모리를 정리해도, 네트워크 연결·파일·타이머 같은 자원은 언제 닫힐지 기다리면 안 됩니다. `IDisposable`과 `using`은 "이 지점에서 사용을 끝낸다"고 명확히 알리는 방법입니다.

### 취소 신호는 어떻게 전달되나요?

`CancellationTokenSource`는 신호를 만드는 리모컨이고, `CancellationToken`은 그 신호를 전달받는 수신기라고 생각하면 됩니다.

```text
cts.CancelAfter(2초)
        ↓
CancellationTokenSource가 취소 신호를 보냄
        ↓
cts.Token을 전달받은 Task.Delay가 신호를 확인함
        ↓
Task는 Canceled 상태가 되고 OperationCanceledException을 전달함
        ↓
await 지점의 catch (OperationCanceledException)이 처리함
```

중요한 점은 `Cancel()`이 실행 중인 모든 코드를 강제로 멈추는 명령이 아니라는 것입니다. **토큰을 전달받고 취소를 지원하는 작업만** 신호를 확인해 취소합니다. 예를 들어 아래 `Task.Delay`는 `cancellationToken`을 받았기 때문에 취소에 반응합니다.

```csharp
await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
```

네트워크 수신, 파일 읽기, 직접 만든 반복문도 취소해야 한다면 같은 토큰을 전달하고 적절한 지점에서 `ThrowIfCancellationRequested()` 또는 `IsCancellationRequested`를 확인해야 합니다.

### 왜 `try`/`catch`가 필요한가요?

취소된 Task를 `await`하면 `OperationCanceledException`이 `await` 위치에서 다시 발생합니다. 사용자가 접속 대기를 취소한 것은 예상 가능한 흐름이므로, 이를 일반 오류처럼 프로그램 끝까지 전파하지 않고 `catch (OperationCanceledException)`에서 "접속 대기가 취소되었습니다"로 처리합니다.

```csharp
try
{
    await waiting;
    Console.WriteLine("접속 완료");
}
catch (OperationCanceledException)
{
    Console.WriteLine("접속 대기가 취소되었습니다.");
}
```

`catch (Exception)` 하나로 모두 처리하면 취소와 실제 접속 오류를 구분하기 어렵습니다. 먼저 `OperationCanceledException`을 따로 처리하고, 필요할 때만 그 밖의 예외를 별도로 기록합니다.

## 2. 안내형 실습: 취소 가능한 접속 대기

**미션:** 5초 대기 중 사용자가 취소하면 완료 대신 취소 결과를 확인합니다.

1. `dotnet new console -n AsyncCancellationLab`으로 콘솔 프로젝트를 만듭니다.
2. `Program.cs`를 아래 코드로 모두 바꿉니다.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // CancellationTokenSource는 취소 신호를 만들고 관리합니다.
        using CancellationTokenSource cts = new();
        // cts.Token을 전달하면 이 Task는 취소 신호를 확인할 수 있습니다.
        Task waiting = WaitForConnectionAsync(cts.Token);
        // CancelAfter는 2초 뒤 cts에 취소 신호를 보냅니다.
        cts.CancelAfter(TimeSpan.FromSeconds(2));

        try
        {
            // 취소된 Task를 await하면 OperationCanceledException이 발생합니다.
            await waiting;
            Console.WriteLine("접속 완료");
        }
        // 예상한 취소는 일반 오류와 구분해 처리합니다.
        catch (OperationCanceledException)
        {
            Console.WriteLine("접속 대기가 취소되었습니다.");
        }
    }

    static async Task WaitForConnectionAsync(CancellationToken cancellationToken)
    {
        // Task.Delay에 토큰을 전달해야 취소 신호에 반응합니다.
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }
}
```

3. 프로젝트 폴더에서 `dotnet run`을 실행합니다.

### 실행해보면

약 2초 뒤 `접속 대기가 취소되었습니다.`가 출력되면 성공입니다.

`cts.CancelAfter`를 지우면 5초 뒤 `접속 완료`이 출력됩니다. 이 차이는 `Task.Delay`가 받은 취소 토큰의 신호를 확인했는지로 결정됩니다.

## 응용 실습: 성공·취소·동시 작업 구분하기

1. `CancelAfter` 시간을 7초로 바꾸고 결과를 기록하세요.
2. 1초와 2초 지연을 가진 두 `Task`를 만들고 `Task.WhenAll`로 기다리세요.
3. "완료", "취소", "예외"가 각각 언제 발생하는지 한 문장씩 적으세요.

## 오늘의 정리

- 비동기 작업에는 완료, 실패, 취소라는 서로 다른 결과가 있습니다.
- `CancellationTokenSource`가 신호를 만들고, `CancellationToken`을 받은 작업이 협조적으로 취소됩니다.
- 취소된 Task를 `await`하면 `OperationCanceledException`이 발생하므로, 예상된 취소는 `try`/`catch`로 정상 흐름으로 처리합니다.
- `IDisposable` 자원은 `using` 선언 또는 `using` 블록으로 작업이 끝나는 시점에 정리합니다.
- 다음 DAY부터는 이 비동기 원리를 바탕으로 네트워크 게임의 서버 구조와 메시지 규약을 설계합니다.
