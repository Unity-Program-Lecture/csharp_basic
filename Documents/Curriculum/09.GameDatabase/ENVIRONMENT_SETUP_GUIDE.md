# 게임 데이터베이스 프로그래밍 환경 준비

1일차 전에 .NET SDK, DB Browser for SQLite, 그리고 C# 코드를 작성·실행할 도구 하나를 준비합니다. C# 도구는 **Visual Studio** 또는 **Visual Studio Code (VS Code)** 중 하나만 선택하면 됩니다. 7일차는 별도 Unity 6 프로젝트와 `com.gilzoide.sqlite-net` 패키지가 필요합니다.

| 선택 | 이런 학습자에게 권장 | 이 과정에서 하는 일 |
| :--- | :--- | :--- |
| Visual Studio | 메뉴와 버튼으로 프로젝트·NuGet 패키지를 관리하고 싶은 학습자 | 4~7일차 콘솔 DB 프로젝트 생성, 패키지 설치, 실행 |
| VS Code | 가벼운 편집기와 터미널 명령을 함께 익히고 싶은 학습자 | 폴더 열기, 코드 편집, `dotnet` 명령으로 패키지 설치·실행 |

> 두 도구를 모두 설치할 필요는 없습니다. 수업 중에는 한 도구를 정해 같은 방식으로 따라갑니다. NuGet 패키지 설치의 자세한 방법은 [NuGet 패키지 사용 가이드](Supplement/NUGET_PACKAGE_GUIDE.md)를 봅니다.

## 1. 공통 준비: .NET SDK와 DB Browser

1. .NET SDK를 설치합니다. SDK는 C# 프로젝트를 만들고 빌드·실행하는 도구 모음입니다. 단순 실행용 Runtime만 설치하면 `dotnet new`, `dotnet run`을 사용할 수 없습니다.
2. 터미널을 새로 열고 `dotnet --version`을 실행합니다.
3. [DB Browser for SQLite](https://sqlitebrowser.org/dl/)를 설치하고 실행합니다.
4. 1일차에 사용할 실습 폴더에 쓰기 권한이 있는지 확인합니다.

## 2. Visual Studio로 준비하기

1. Visual Studio Installer에서 Visual Studio Community 또는 학교에서 안내한 버전을 설치·수정합니다.
2. 워크로드 목록에서 **.NET 데스크톱 개발 (.NET desktop development)** 을 선택합니다. 이 워크로드에는 C# 콘솔 앱을 만들고 빌드하는 데 필요한 도구가 포함됩니다.
3. 설치가 끝나면 Visual Studio를 실행합니다.
4. `새 프로젝트 만들기(Create a new project)`에서 `콘솔 앱(Console App)`을 검색해 임시 프로젝트를 하나 만듭니다.
5. `Ctrl+F5`를 눌러 기본 프로그램이 실행되는지 확인합니다.

설치 뒤 필요한 기능이 빠졌다면 Visual Studio를 닫고 Visual Studio Installer에서 `수정(Modify)`을 선택해 워크로드를 추가할 수 있습니다.

## 3. VS Code로 준비하기

1. [Visual Studio Code](https://code.visualstudio.com/)를 설치합니다.
2. VS Code 왼쪽의 Extensions 보기 (`Ctrl+Shift+X`)를 열고 Microsoft 게시자의 **C# Dev Kit**을 설치합니다.
3. 새 폴더를 만들고 VS Code에서 `File > Open Folder...`로 그 폴더를 엽니다.
4. VS Code의 `Terminal > New Terminal`을 열고 아래 명령을 실행합니다.

```powershell
dotnet new console -n DatabaseEditorCheck
cd DatabaseEditorCheck
dotnet run
```

5. `Hello, World!`가 출력되면 VS Code와 .NET SDK의 기본 연결이 확인된 것입니다.
6. 처음 C# 프로젝트 폴더를 열었을 때 "빌드·디버그에 필요한 파일을 추가할까요?"라는 안내가 나오면 `Yes`를 선택합니다.

> C# Dev Kit은 Visual Studio 구독 로그인이 필요할 수 있습니다. 학교 계정 로그인이 막히거나 확장을 쓸 수 없다면 강사에게 알리고 Visual Studio 또는 수업에서 지정한 C# 편집 도구를 사용합니다.

## 4. 준비 확인

1. 터미널에서 `dotnet --version`을 실행해 .NET SDK 버전이 출력되는지 확인합니다.
2. DB Browser for SQLite를 실행하고 빈 DB를 만들 수 있는지 확인합니다.
3. 1일차에 사용할 실습 폴더에 쓰기 권한이 있는지 확인합니다.
4. Unity 6 프로젝트가 필요한 경우 7일차 전에 Package Manager를 열 수 있는지 확인합니다.

### 완료 확인

- [ ] `dotnet --version`이 오류 없이 출력된다.
- [ ] DB Browser for SQLite에서 `.db` 파일을 열 수 있다.
- [ ] 1일차 실습 폴더에 `GameShop.db` 파일을 저장할 수 있다.
- [ ] Visual Studio 또는 VS Code 중 선택한 도구에서 C# 콘솔 프로젝트를 한 번 실행했다.
- [ ] Unity 실습 대상자는 Package Manager를 열 수 있다.

## 5. 문제가 생기면

| 증상 | 먼저 확인할 것 |
| :--- | :--- |
| `dotnet` 명령을 찾을 수 없음 | .NET SDK 설치와 터미널 재시작 |
| Visual Studio에 콘솔 앱 템플릿이 없음 | Visual Studio Installer에서 `.NET 데스크톱 개발` 워크로드 설치 여부 |
| VS Code에서 C# 자동 완성이 안 됨 | C# Dev Kit 설치, 프로젝트 폴더 전체를 열었는지, .NET SDK 인식 여부 |
| VS Code에서 C# Dev Kit 로그인이 막힘 | 학교 계정·구독 권한을 확인하거나 Visual Studio 방식 사용 |
| DB Browser 실행 실패 | 설치 권한과 운영체제에 맞는 설치 파일 |
| NuGet 패키지 다운로드 실패 | 인터넷, 학교 방화벽, 프로젝트 폴더 쓰기 권한 |
| Unity 패키지 설치 실패 | Console 컴파일 오류와 Git URL 입력값 |

## 공식 참고

- [VS Code에서 C# 시작하기](https://code.visualstudio.com/docs/csharp/get-started)
- [VS Code에서 .NET 사용하기](https://code.visualstudio.com/docs/languages/dotnet)
- [Visual Studio 설치와 워크로드 선택](https://learn.microsoft.com/visualstudio/install/install-visual-studio)
