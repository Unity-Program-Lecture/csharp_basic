# DAY 01: 비동기 처리 - Task, 취소, 동시 작업

오늘은 네트워크 응답을 기다리는 동안 프로그램 전체가 멈추지 않게 하는 비동기의 공통 흐름을 콘솔에서 배웁니다. `Task`를 시작하고, 여러 작업을 함께 기다리고, 필요하면 안전하게 취소하는 데까지 한 번에 연결합니다.

## 1. 핵심 개념: "주문 번호표를 받고 다른 일을 계속하기"

- **동기 처리 (Synchronous)**: 앞 작업이 끝날 때까지 다음 작업이 기다리는 방식입니다.
- **비동기 처리 (Asynchronous)**: 오래 걸리는 작업을 시작한 뒤, 완료를 기다리는 동안 다른 일을 이어 갈 수 있는 방식입니다.
- **`Task`**: 미래에 끝날 작업과 결과를 표현하는 .NET 객체입니다.
- **`async`**: 메서드 안에서 `await`를 사용할 수 있게 하는 키워드입니다.
- **`await`**: 작업이 끝날 때까지 현재 메서드를 잠시 멈추고, 완료 뒤 이어서 실행하는 키워드입니다.

> `async`/`await`는 자동으로 새 스레드를 만드는 기능이 아닙니다. 네트워크 대기를 읽기 쉽게 표현하는 도구입니다.

## 2. 안내형 실습 1: 기다렸다가 접속 결과 받기

**미션:** 2초 걸리는 작업을 `await`로 기다리고 결과를 받습니다.

1. `dotnet new console -n AsyncTaskLab`으로 콘솔 프로젝트를 만듭니다.
2. `AsyncTaskLab/Program.cs`를 아래 코드로 모두 바꿉니다.

```csharp
using System;
using System.Threading.Tasks;

// Program은 콘솔 프로그램의 시작 클래스를 담습니다.
class Program
{
    // async Task Main은 await를 사용할 수 있는 프로그램 시작 메서드입니다.
    static async Task Main()
    {
        Console.WriteLine("접속 요청 시작");

        // await는 ConnectAsync 작업이 끝난 뒤 결과 문자열을 받습니다.
        string result = await ConnectAsync();
        Console.WriteLine($"결과: {result}");
    }

    // Task<string>은 나중에 문자열 결과를 돌려줄 비동기 작업입니다.
    static async Task<string> ConnectAsync()
    {
        // Task.Delay는 지정한 시간 동안 비동기로 기다립니다.
        await Task.Delay(TimeSpan.FromSeconds(2));
        return "접속 완료";
    }
}
```

3. 프로젝트 폴더에서 `dotnet run`을 실행합니다.
4. `접속 요청 시작`과 `결과: 접속 완료` 사이에 약 2초가 있는지 확인합니다.

### 완료 확인

- [ ] `ConnectAsync`의 반환 형식이 `Task<string>`이다.
- [ ] `await` 뒤의 출력은 대기가 끝난 뒤 실행된다.
- [ ] `Task`와 스레드가 같은 뜻이 아니라는 점을 설명할 수 있다.

## 3. 독립 작업은 함께 기다리기: `Task.WhenAll`

**`Task.WhenAll`**은 여러 비동기 작업이 모두 끝날 때까지 함께 기다리는 메서드입니다. 프로필과 인벤토리처럼 서로 기다릴 이유가 없는 작업은 하나가 끝난 뒤 다른 하나를 시작하지 않고, 먼저 둘 다 시작한 다음 `Task.WhenAll`로 결과를 모읍니다.

```csharp
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // 두 Task를 먼저 만들면 두 대기 작업이 겹쳐서 시작됩니다.
        Task<string> profileTask = LoadProfileAsync();
        Task<string> inventoryTask = LoadInventoryAsync();

        // Task.WhenAll은 전달받은 모든 Task가 끝날 때까지 기다립니다.
        string[] results = await Task.WhenAll(profileTask, inventoryTask);
        Console.WriteLine(results[0]);
        Console.WriteLine(results[1]);
    }

    // Task.Delay는 프로필을 불러오는 시간을 간단히 흉내 냅니다.
    static async Task<string> LoadProfileAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return "프로필 로드 완료";
    }

    // 이 작업도 독립적으로 시작되므로 위 Task와 동시에 기다릴 수 있습니다.
    static async Task<string> LoadInventoryAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        return "인벤토리 로드 완료";
    }
}
```

두 작업의 지연 시간을 더하면 3초지만, 실제 실행은 약 2초 뒤 두 줄이 출력됩니다. 두 작업을 먼저 시작한 뒤 함께 기다렸기 때문입니다.

> 작업 사이에 반드시 순서가 있어야 한다면 `Task.WhenAll`을 쓰지 않습니다. 예를 들어 로그인 성공 뒤에만 인벤토리를 불러와야 한다면 `await LoginAsync()`가 끝난 다음 `await LoadInventoryAsync()`를 호출합니다.

## 4. 취소와 자원 정리: "닫힌 창구에서 계속 접수하지 않기"

- **취소 토큰 (CancellationToken)**: "이 작업을 그만해도 된다"는 신호를 전달하는 값입니다.
- **`CancellationTokenSource`**: 취소 신호를 만들고 `Cancel()` 또는 `CancelAfter()`로 보내는 객체입니다.
- **시간 초과 (Timeout)**: 정한 시간 안에 끝나지 않은 작업을 취소 또는 실패로 처리하는 기준입니다.
- **예외 (Exception)**: 정상 흐름으로 처리할 수 없는 문제가 발생했음을 알리는 객체입니다.
- **`IDisposable` 인터페이스**: 파일, 네트워크 연결, 타이머처럼 사용 뒤 정리해야 하는 자원에 `Dispose()` 약속을 제공하는 C# 인터페이스입니다.

### `using`은 두 가지 뜻이 있습니다

코드 첫 줄의 `using System;`은 **using 지시문**입니다. `System.Console` 대신 `Console`처럼 네임스페이스 이름을 짧게 쓰게 합니다.

반면 아래의 `using CancellationTokenSource cts = new();`은 **using 선언**입니다. `cts`를 포함한 메서드가 끝날 때 `Dispose()`를 자동 호출합니다.

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

`Cancel()`은 실행 중인 모든 코드를 강제로 멈추는 명령이 아닙니다. 토큰을 전달받고 취소를 지원하는 작업만 신호를 확인해 취소합니다. 직접 만든 반복문은 `ThrowIfCancellationRequested()` 또는 `IsCancellationRequested`를 확인해야 합니다.

### 왜 `try`/`catch`가 필요한가요?

취소된 Task를 `await`하면 **`OperationCanceledException`** 이 `await` 위치에서 다시 발생합니다. 사용자가 접속 대기를 취소한 것은 예상 가능한 흐름이므로, `catch (OperationCanceledException)`에서 일반 오류와 구분해 처리합니다.

## 5. 안내형 실습 2: 취소 가능한 접속 대기

**미션:** 5초 대기 중 2초가 지나면 취소 결과를 확인합니다.

1. `dotnet new console -n AsyncCancellationLab`으로 콘솔 프로젝트를 만듭니다.
2. `AsyncCancellationLab/Program.cs`를 아래 코드로 모두 바꿉니다.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

// Program은 콘솔 프로그램의 시작 클래스를 담습니다.
class Program
{
    // async Task Main은 await를 사용할 수 있는 시작 메서드입니다.
    static async Task Main()
    {
        // CancellationTokenSource는 취소 신호를 만들고 관리합니다.
        using CancellationTokenSource cts = new();
        // WaitForConnectionAsync는 취소 토큰을 받는 대기 작업입니다.
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

    // CancellationToken은 이 작업이 취소 신호를 확인하게 합니다.
    static async Task WaitForConnectionAsync(
        CancellationToken cancellationToken)
    {
        // Task.Delay에 토큰을 전달해야 취소 신호에 반응합니다.
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }
}
```

3. 프로젝트 폴더에서 `dotnet run`을 실행합니다.
4. 약 2초 뒤 `접속 대기가 취소되었습니다.`가 출력되는지 확인합니다.
5. `CancelAfter`를 7초로 바꾼 뒤, 5초 후 `접속 완료`가 출력되는지도 확인합니다.

## 응용 실습: 접속 준비 작업 함께 기다리기

`LoadProfileAsync`와 `LoadInventoryAsync`를 각각 1초와 2초 지연으로 만들고 `Task.WhenAll`로 기다리세요. 이어서 3초의 취소 시간을 적용해, 완료·취소·예외가 각각 언제 발생하는지 한 문장씩 기록하세요.

## 오늘의 정리

- `Task`는 미래의 완료를, `await`는 완료 뒤 이어질 코드를 표현합니다.
- 독립 작업은 먼저 시작한 뒤 `Task.WhenAll`로 함께 기다릴 수 있습니다.
- `CancellationTokenSource`가 신호를 만들고, `CancellationToken`을 받은 작업이 협조적으로 취소됩니다.
- 취소된 Task를 `await`하면 `OperationCanceledException`이 발생하므로 예상된 취소는 `try`/`catch`로 처리합니다.
- `IDisposable` 자원은 `using` 선언 또는 `using` 블록으로 작업이 끝나는 시점에 정리합니다.
- 다음 DAY에는 이 비동기 원리를 바탕으로 네트워크 게임의 서버 구조와 메시지 규약을 설계합니다.
