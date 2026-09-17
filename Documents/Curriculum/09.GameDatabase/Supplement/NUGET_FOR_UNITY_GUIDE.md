# 참고: NuGetForUnity로 Unity 프로젝트에 패키지 추가하기

이 문서는 Unity 6 프로젝트에서 NuGet 패키지를 설치하는 방법을 설명합니다. 7일차의 기본 SQLite 실습은 [Unity용 SQLite-net 사용 가이드](UNITY_SQLITE_NET_GUIDE.md)를 사용합니다. 이 문서는 LiteDB처럼 NuGet으로 배포되는 라이브러리를 Unity에서 별도로 시험할 때 사용합니다.

> NuGetForUnity는 Unity Editor 안에서 NuGet 패키지를 설치·관리하는 도구입니다. Visual Studio의 NuGet 관리자와 역할은 비슷하지만, 설치 위치와 Unity 호환성 확인 방법은 다릅니다.

## 1. 왜 별도 도구가 필요한가요?

Unity Package Manager는 Unity 패키지용이고, `LiteDB`, `Microsoft.Data.Sqlite` 같은 .NET 라이브러리는 NuGet 패키지로 배포됩니다. NuGetForUnity는 Unity Editor에서 NuGet 패키지를 검색·설치하는 창을 제공합니다.

```text
Unity Package Manager
    -> NuGetForUnity 설치
        -> Unity 메뉴의 NuGet 창
            -> LiteDB 설치
                -> Unity C# 스크립트에서 using LiteDB;
```

## 2. 시작 전 준비

- Unity 6 프로젝트를 먼저 백업하거나 Git 커밋합니다.
- 인터넷 연결과 Git 실행 가능 여부를 확인합니다. Git URL 방식은 Unity가 저장소를 내려받는 과정이 필요합니다.
- 이 문서의 실습은 **Windows Editor에서 LiteDB 파일을 읽는 학습용**입니다.
- 실제 온라인 게임은 Unity 클라이언트가 DB 파일을 직접 관리하지 않고 `Unity -> 서버/API -> DB` 구조를 사용합니다.

## 3. NuGetForUnity 설치하기

1. Unity 6 프로젝트를 엽니다.
2. 위 메뉴에서 `Window > Package Manager`를 엽니다.
3. 왼쪽 위의 `+` 버튼을 누릅니다.
4. `Add package from git URL...`을 선택합니다.
5. 아래 주소를 붙여 넣습니다.

```text
https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity
```

6. `Add`를 누르고 설치가 끝날 때까지 기다립니다.
7. 상단 메뉴에 `NuGet` 항목이 생기는지 확인합니다.

> 특정 버전으로 고정해야 할 때만 Git URL 끝에 `#v버전번호`를 붙입니다. 수업에서는 강사가 한 번 검증한 버전을 모든 학생이 같이 사용합니다.

## 4. LiteDB 설치하기

1. Unity 메뉴에서 `NuGet > Manage NuGet Packages`를 엽니다.
2. 검색창에 `LiteDB`를 입력합니다.
3. 패키지 ID가 정확히 `LiteDB`인지, 안정 버전인지 확인합니다. `Prerelease`는 선택하지 않습니다.
4. `Install`을 누르고, 의존성 설치 및 Unity 재컴파일이 끝날 때까지 기다립니다.
5. Unity Console에 컴파일 오류가 없는지 확인합니다.
6. Project 창에서 NuGetForUnity가 관리하는 패키지 폴더와 `packages.config`가 생성·갱신되었는지 확인합니다.

NuGetForUnity는 기본적으로 프로젝트 안에 NuGet 패키지 정보를 기록하고, 패키지 파일을 프로젝트의 설정 경로에 둡니다. 이 경로와 버전 관리 포함 여부는 NuGetForUnity 설정과 팀 규칙에 맞춰 강사가 먼저 정합니다.

## 5. 7일차에서 이 도구를 기본 경로로 쓰지 않는 이유

`Microsoft.Data.Sqlite`는 기본적으로 SQLitePCLRaw와 네이티브 SQLite 번들을 함께 사용합니다. .NET 콘솔 프로젝트에서는 편리하지만, Unity에서는 Editor·Windows 빌드·Android·iOS·IL2CPP (Intermediate Language To C++, 중간 언어를 C++로 변환하는 Unity 스크립팅 백엔드)마다 네이티브 라이브러리와 플랫폼 설정을 별도로 검증해야 할 수 있습니다.

따라서 7일차의 기본 실습은 **UPM용 `com.gilzoide.sqlite-net`**으로 합니다. 해당 패키지는 Unity 플랫폼용 네이티브 라이브러리와 WebGL용 저장 처리를 포함합니다. LiteDB는 이 문서에서 다루는 NuGet 연습 및 문서형 NoSQL DB 확장 실습으로 유지합니다.

| 목적 | 권장 선택 | 이유 |
| :--- | :--- | :--- |
| 1~5일차 관계형 DB·SQL·트랜잭션 학습 | .NET 콘솔 + Microsoft.Data.Sqlite | SQL과 관계형 설계를 분명히 학습 |
| 6~7일차 문서형 NoSQL DB 학습 | .NET 콘솔 + LiteDB | 문서 CRUD 학습 |
| 7일차 Unity에서 실제 SQLite 파일 읽기 | Unity + `com.gilzoide.sqlite-net` | UPM으로 설치하며 주요 Unity 플랫폼과 WebGL 지원을 안내함 |
| Unity에서 LiteDB를 실제로 시험 | Unity + NuGetForUnity + LiteDB | 문서형 NoSQL DB를 Unity에서 읽는 확장 실습 |

## 6. 7일차 LiteDB 확장 실습 실패 확인

`GameLogs.db` 복사, `GameLogReader` 스크립트, 화면 표시 절차는 [7일차](../DAY07_LITEDB_AND_UNITY_CONNECTION.md)의 "선택 확장 실습"에서 진행합니다. 이 가이드는 NuGetForUnity 설치와 설치 문제 해결을 위한 참고 자료로 유지합니다.

| 증상 | 먼저 확인할 것 |
| :--- | :--- |
| Git URL 설치 실패 | Git 설치·인터넷·URL 전체 입력 여부 |
| `NuGet` 메뉴가 안 보임 | Package Manager 설치 완료와 Console 오류 여부 |
| `using LiteDB` 빨간 줄 | NuGet 창에서 LiteDB 설치 완료, Unity 재컴파일 여부 |
| `GameLogs.db`를 찾지 못함 | `Assets/StreamingAssets/GameLogs.db` 경로와 파일 이름 |
| 로그가 없다고 출력됨 | 6일차에서 `logs` 컬렉션에 문서를 넣었는지 |
| DB 잠금 또는 쓰기 오류 | DB Browser와 콘솔 앱이 파일을 열고 있지 않은지, `persistentDataPath` 복사본을 여는지 |

## 공식 참고

- [NuGetForUnity 공식 설치·사용 안내](https://github.com/GlitchEnzo/NuGetForUnity)
- [Unity Package Manager 창](https://docs.unity3d.com/Manual/upm-ui.html)
- [Microsoft.Data.Sqlite의 SQLitePCLRaw 번들](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/custom-versions)
