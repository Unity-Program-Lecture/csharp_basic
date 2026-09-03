# 참고: NuGet 패키지 사용 가이드

이 문서는 게임 데이터베이스 실습에서 필요한 `Microsoft.Data.Sqlite`, `LiteDB`처럼 **내 프로젝트에 기본으로 들어 있지 않은 기능**을 추가하는 방법을 설명합니다. DAY06과 DAY09 실습 전에 필요할 때 찾아봅니다.

## 1. NuGet 패키지는 무엇인가요?

NuGet은 .NET/C# 프로젝트용 라이브러리를 배포하고 내려받는 패키지 관리자입니다. 패키지는 다른 개발자가 만든 기능 묶음이며, 프로젝트에 추가하면 필요한 라이브러리와 의존 라이브러리를 함께 내려받습니다.

```text
내 C# 프로젝트
    + Microsoft.Data.Sqlite 패키지 -> SQLite를 C#에서 다루는 기능
    + LiteDB 패키지              -> LiteDB 문서 DB를 다루는 기능
```

패키지를 설치했다고 해서 C# 코드가 자동으로 완성되지는 않습니다. 설치 뒤에는 코드 파일에 `using Microsoft.Data.Sqlite;` 또는 `using LiteDB;`를 쓰고, 해당 라이브러리의 클래스를 사용합니다.

### 꼭 구분할 것

| 대상 | 역할 |
| :--- | :--- |
| NuGet 패키지 | 프로젝트에 기능을 추가하는 라이브러리 |
| DB Browser for SQLite | SQLite DB 파일을 눈으로 보고 SQL을 실행하는 별도 프로그램 |
| SQLite DB 파일 | `GameShop.db`처럼 실제 게임 데이터를 저장하는 파일 |
| C# 프로젝트 | 패키지를 이용해 DB 파일을 관리하는 프로그램 |

> 이 문서의 패키지 설치 방법은 DAY06~DAY10의 **.NET 콘솔 프로젝트** 기준입니다. Unity 프로젝트에서 NuGet 패키지를 바로 추가하는 일은 호환성·플랫폼 빌드 확인이 필요하므로, 이 과정에서는 Unity가 DB 결과를 표시하는 역할에 집중합니다.

## 2. 설치 전 확인

1. 설치할 프로젝트 폴더를 엽니다. 폴더 안에 `.csproj` 파일이 있어야 합니다.
2. 터미널 방식이라면 터미널을 그 프로젝트 폴더에서 엽니다.
3. Visual Studio 방식이라면 솔루션 탐색기에서 **설치할 프로젝트**를 선택합니다.
4. NuGet에서 패키지 이름을 검색할 때는 비슷한 이름의 비공식 패키지와 혼동하지 않도록 패키지 ID, 소유자, 설명, 버전을 확인합니다.

## 3. 터미널에서 패키지 설치하기

Windows에서는 PowerShell 또는 Visual Studio의 터미널을 사용할 수 있습니다. 아래 명령은 `.csproj` 파일이 있는 폴더에서 실행합니다.

### 3.1 새 콘솔 프로젝트 만들기

```powershell
dotnet new console -n GameDatabaseLab
cd GameDatabaseLab
```

`dotnet new console`은 콘솔 프로젝트와 `.csproj` 파일을 만듭니다. `cd`는 그 프로젝트 폴더로 이동하는 명령입니다.

### 3.2 SQLite 패키지 추가하기

```powershell
dotnet add package Microsoft.Data.Sqlite
```

이 명령은 `Microsoft.Data.Sqlite` 패키지를 프로젝트에 등록하고 필요한 파일을 내려받습니다. 성공하면 `.csproj`에 아래와 비슷한 줄이 생깁니다. 버전 번호는 수업 당일 선택한 안정 버전에 따라 달라질 수 있습니다.

```xml
<PackageReference Include="Microsoft.Data.Sqlite" Version="버전번호" />
```

### 3.3 LiteDB 패키지 추가하기

```powershell
dotnet add package LiteDB
```

### 3.4 설치 확인과 실행

```powershell
dotnet list package
dotnet run
```

`dotnet list package`는 현재 프로젝트가 참조하는 패키지 목록을 보여 줍니다. `dotnet run`은 패키지를 포함해 프로젝트를 빌드하고 실행합니다.

### 3.5 특정 버전을 지정해야 할 때

수업에서 같은 버전으로 맞춰야 하거나 강사가 버전을 안내한 경우에만 씁니다.

```powershell
dotnet add package LiteDB --version 5.0.21
```

버전 번호를 임의로 오래된 것으로 고정하지 않습니다. 강사가 테스트한 버전 또는 패키지의 안정 버전을 사용합니다.

### 3.6 패키지 삭제와 다시 내려받기

```powershell
dotnet remove package LiteDB
dotnet restore
```

`dotnet remove package`는 프로젝트의 패키지 참조를 제거합니다. `dotnet restore`는 `.csproj`에 적힌 패키지를 다시 내려받고 복원합니다.

## 4. Visual Studio에서 콘솔 프로젝트 만들기

터미널 대신 Visual Studio 화면에서 실습 프로젝트를 만들 수 있습니다.

1. Visual Studio를 실행합니다.
2. 시작 화면에서 `새 프로젝트 만들기(Create a new project)`를 선택합니다.
3. 검색창에 `콘솔 앱` 또는 `Console App`을 입력합니다.
4. 언어가 `C#`인 **콘솔 앱(Console App)** 템플릿을 선택하고 `다음(Next)`을 누릅니다.
5. 프로젝트 이름에 `GameDatabaseLab`을 입력합니다.
6. 위치(Location)는 학생 개인 실습 폴더를 선택합니다. 공개 Git 저장소에서 실습 DB 파일을 관리한다면 `.db` 파일을 커밋하지 않도록 별도 실습 폴더나 `.gitignore`를 사용합니다.
7. `다음(Next)`을 누릅니다.
8. 프레임워크는 강사가 안내한 .NET 버전을 선택합니다. 수업 예제는 Unity 6과 같은 문법 범위로 읽을 수 있도록 C# 9.0 기준으로 작성합니다.
9. `만들기(Create)`를 누릅니다.
10. 솔루션 탐색기에 `GameDatabaseLab` 프로젝트와 `GameDatabaseLab.csproj`, `Program.cs`가 보이는지 확인합니다.
11. `Ctrl+F5`를 눌러 기본 콘솔 프로그램이 실행되는지 확인합니다.

> **Visual Studio의 프로젝트 생성**과 **Unity 프로젝트 생성**은 다른 작업입니다. DAY06~DAY10의 DB 코드는 우선 Visual Studio 콘솔 프로젝트에서 실행합니다.

## 5. Visual Studio 메뉴에서 패키지 설치하기

터미널이 낯설다면 Visual Studio의 NuGet 패키지 관리자 창을 사용해도 됩니다.

1. Visual Studio에서 콘솔 프로젝트 또는 솔루션을 엽니다.
2. 솔루션 탐색기에서 설치할 프로젝트를 선택합니다.
3. 위 메뉴에서 `프로젝트(Project) > NuGet 패키지 관리(Manage NuGet Packages)`를 선택합니다.
4. `찾아보기(Browse)` 탭을 엽니다.
5. 검색창에 `Microsoft.Data.Sqlite` 또는 `LiteDB`를 입력합니다.
6. 패키지 이름, 설명, 소유자 정보를 확인합니다.
   - `Microsoft.Data.Sqlite`: Microsoft가 제공하는 SQLite용 ADO.NET (ActiveX Data Objects .NET, .NET 데이터 접근 기술) 라이브러리
   - `LiteDB`: .NET용 임베디드 문서 데이터베이스 라이브러리
7. `버전(Version)`에서 수업에 지정된 안정 버전을 고릅니다. 별도 지시가 없으면 시험판(Prerelease)은 선택하지 않습니다.
8. `설치(Install)`를 누르고, 라이선스 확인 창이 나오면 내용을 읽고 승인합니다.
9. 설치가 끝나면 `설치됨(Installed)` 탭 또는 솔루션 탐색기의 `종속성(Dependencies) > 패키지(Packages)`에서 패키지를 확인합니다.
10. `빌드(Build) > 솔루션 빌드(Build Solution)` 또는 `Ctrl+Shift+B`로 오류가 없는지 확인합니다.

### Visual Studio에서 삭제·업데이트하기

- 삭제: `설치됨(Installed)` 탭에서 패키지를 선택하고 `제거(Uninstall)`를 누릅니다.
- 업데이트: `업데이트(Updates)` 탭에서 패키지와 버전을 확인한 뒤 업데이트합니다.

> 수업 중에는 학생마다 다른 최신 버전으로 업데이트하지 않습니다. 강사가 확인한 버전을 정해 함께 사용합니다.

## 6. Visual Studio 패키지 관리자 콘솔 사용하기

Visual Studio 메뉴에서 `도구(Tools) > NuGet 패키지 관리자(NuGet Package Manager) > 패키지 관리자 콘솔(Package Manager Console)`을 열 수 있습니다. 이 창은 일반 PowerShell 터미널과 명령어가 다릅니다.

```powershell
Install-Package Microsoft.Data.Sqlite
Install-Package LiteDB
```

특정 버전을 지정할 때는 다음처럼 씁니다.

```powershell
Install-Package LiteDB -Version 5.0.21
```

| 일반 터미널 | Visual Studio 패키지 관리자 콘솔 |
| :--- | :--- |
| `dotnet add package LiteDB` | `Install-Package LiteDB` |
| `dotnet remove package LiteDB` | `Uninstall-Package LiteDB` |
| `dotnet list package` | 설치됨 탭 또는 프로젝트 종속성에서 확인 |

## 7. 설치 뒤 코드에서 사용하기

패키지가 설치된 뒤에도 코드 상단의 `using`과 실제 사용 코드가 필요합니다.

```csharp
using Microsoft.Data.Sqlite;

// SqliteConnection, SqliteCommand 등을 사용할 수 있습니다.
```

```csharp
using LiteDB;

// LiteDatabase, ILiteCollection 등을 사용할 수 있습니다.
```

`using`을 적었는데 빨간 줄이 남는다면, 패키지가 **현재 열어 둔 프로젝트**에 설치됐는지, 설치 후 빌드했는지 확인합니다.

## 8. 자주 발생하는 문제

| 증상 | 먼저 확인할 것 | 해결 방향 |
| :--- | :--- | :--- |
| `dotnet`을 찾을 수 없음 | .NET SDK (Software Development Kit, 소프트웨어 개발 도구 모음) 설치 여부 | Visual Studio Installer 또는 .NET SDK 설치 상태를 강사와 확인 |
| `프로젝트를 찾을 수 없음` | 현재 폴더와 `.csproj` 파일 | `cd`로 프로젝트 폴더 이동 후 다시 실행 |
| 패키지 다운로드 실패 | 인터넷, 학교 방화벽, NuGet 접근 | 오류 메시지를 캡처해 강사에게 알림 |
| `using LiteDB`에 빨간 줄 | 패키지 설치 대상 프로젝트 | 현재 프로젝트의 Dependencies와 빌드 결과 확인 |
| Visual Studio에서 메뉴가 없음 | NuGet 패키지 관리자 구성 요소 | Visual Studio Installer의 .NET 관련 워크로드와 NuGet 구성 요소 확인 |
| Unity 프로젝트에서 바로 오류 | Unity용 호환성·플랫폼 설정 | 콘솔 DB 실습과 Unity UI 실습을 분리하고 강사 지시를 따름 |

## 9. 오늘의 확인

1. NuGet 패키지와 SQLite DB 파일은 어떻게 다른가요?
2. 일반 터미널에서 LiteDB를 추가하는 명령은 무엇인가요?
3. Visual Studio에서 콘솔 프로젝트를 만들 때 어떤 템플릿을 선택하나요?
4. Visual Studio UI에서 설치한 패키지를 어디에서 확인할 수 있나요?
5. `using LiteDB;`에 빨간 줄이 남으면 무엇을 먼저 확인해야 하나요?

## 참고 링크

- [Microsoft Learn - Visual Studio에서 NuGet 패키지 설치 및 관리](https://learn.microsoft.com/ko-kr/nuget/consume-packages/install-use-packages-visual-studio)
- [Microsoft Learn - .NET CLI로 패키지 관리](https://learn.microsoft.com/en-us/nuget/reference/dotnet-commands)
- [NuGet Gallery - Microsoft.Data.Sqlite](https://www.nuget.org/packages/Microsoft.Data.Sqlite)
- [NuGet Gallery - LiteDB](https://www.nuget.org/packages/LiteDB)
