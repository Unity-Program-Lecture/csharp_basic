# DAY 01: 비동기 처리와 Task 기초

오늘은 네트워크 응답을 기다리는 동안 프로그램 전체가 멈추지 않게 하는 비동기의 공통 흐름을 콘솔에서 배웁니다.

## 1. 핵심 개념: "주문 번호표를 받고 다른 일을 계속하기"

- **동기 처리 (Synchronous)**: 앞 작업이 끝날 때까지 다음 작업이 기다리는 방식입니다.
- **비동기 처리 (Asynchronous)**: 오래 걸리는 작업을 시작한 뒤, 완료를 기다리는 동안 다른 일을 이어 갈 수 있는 방식입니다.
- **`Task`**: 미래에 끝날 작업과 결과를 표현하는 .NET 객체입니다.
- **`async`**: 메서드 안에서 `await`를 사용할 수 있게 하는 키워드입니다.
- **`await`**: 작업이 끝날 때까지 현재 메서드를 잠시 멈추고, 완료 뒤 이어서 실행하는 키워드입니다.

> `async`/`await`는 자동으로 새 스레드를 만드는 기능이 아닙니다. 네트워크 대기를 읽기 쉽게 표현하는 도구입니다.

## 2. 안내형 실습: 기다리는 동안 다른 메시지 출력하기

**미션:** 2초 걸리는 작업을 기다리기 전과 후의 실행 순서를 확인합니다.

1. `dotnet new console -n AsyncTaskLab`으로 콘솔 프로젝트를 만듭니다.
2. `Program.cs`를 아래 코드로 모두 바꿉니다.

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

## 3. 핵심 개념과 짧은 예시: `Task.WhenAll`

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

### 실행해보면

두 작업의 지연 시간을 더하면 3초지만, 실제 실행은 약 2초 뒤 두 줄이 출력됩니다. 두 작업을 먼저 시작한 뒤 함께 기다렸기 때문입니다.

> 작업 사이에 반드시 순서가 있어야 한다면 `Task.WhenAll`을 쓰지 않습니다. 예를 들어 로그인 성공 뒤에만 인벤토리를 불러와야 한다면 `await LoginAsync()`가 끝난 다음 `await LoadInventoryAsync()`를 호출합니다.

## 응용 실습: 두 작업 함께 기다리기

`LoadProfileAsync`, `LoadInventoryAsync` 두 메서드를 각각 1초와 2초 지연으로 만들고, `Task.WhenAll`로 두 결과를 함께 기다리세요.

## 오늘의 정리

- `Task`는 미래의 완료를, `await`는 완료 뒤 이어질 코드를 표현합니다.
- 다음 DAY에는 취소, 시간 초과, 예외, 여러 작업을 함께 다루는 방법을 배웁니다.
