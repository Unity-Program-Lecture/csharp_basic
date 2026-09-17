# 참고 실습: Unity 클라이언트 - C# 서버 - PostgreSQL 전체 연결

이 참고 실습은 정규 DAY의 필수 범위가 아닙니다. Unity 클라이언트가 PostgreSQL에 직접 접속하지 않고, **Unity → C# API 서버 → PostgreSQL → C# API 서버 → Unity** 순서로 요청과 응답이 흐르는 모습을 실제로 확인합니다.

```text
Unity Button
    -> HTTP POST /scores
ASP.NET Core ScoreServer
    -> 매개 변수를 사용한 SQL UPSERT
PostgreSQL player_score 표
    -> 저장한 PlayerId, Score, UpdatedAt 반환
ScoreServer
    -> JSON 응답
Unity TMP_Text
```

> 이 예제는 같은 PC의 `127.0.0.1`에서만 HTTP로 실행하는 학습용입니다. 실제 서비스는 Unity와 서버 사이에 HTTPS를 사용하고, 게임 서버가 점수를 계산·검증한 뒤 DB에 저장합니다. 클라이언트가 보낸 `score`를 그대로 신뢰하면 안 됩니다.

## 1. 준비물

- .NET 8 SDK
- Unity 6 프로젝트와 TextMeshPro
- PostgreSQL과 pgAdmin

Windows용 PostgreSQL 설치 프로그램에는 PostgreSQL 서버와 pgAdmin 관리 도구가 함께 포함됩니다. 설치 중 설정한 `postgres` 관리자 비밀번호를 잊지 않도록 기록합니다. [PostgreSQL Windows 설치 프로그램](https://www.postgresql.org/download/windows/)

이 저장소에는 다음 파일을 준비해 두었습니다.

| 파일 | 역할 |
| :--- | :--- |
| `Code/GameNetwork/PostgreSqlEndToEnd/ScoreServer/` | Unity 요청을 받는 ASP.NET Core API 서버 |
| `Code/GameNetwork/PostgreSqlEndToEnd/database/01_schema.sql` | `player_score` 표 생성 SQL |
| `Code/GameNetwork/PostgreSqlEndToEnd/UnityClient/ScoreApiClient.cs` | Unity 버튼 요청과 결과 표시 스크립트 |

## 2. Windows에 PostgreSQL 설치하기

1. [PostgreSQL Windows 다운로드 페이지](https://www.postgresql.org/download/windows/)에서 `Download the installer`를 선택합니다.
2. EnterpriseDB 설치 페이지에서 Windows용 설치 파일을 내려받아 실행합니다. 소스 코드를 빌드할 필요는 없습니다.
3. 설치 마법사에서 설치 경로와 데이터 저장 경로는 처음에는 기본값을 유지합니다.
4. 구성 요소 선택 화면에서는 `PostgreSQL Server`, `pgAdmin 4`, `Command Line Tools`를 선택합니다. `Stack Builder`는 이 참고 실습에 필요하지 않으므로 선택을 해제해도 됩니다.
5. `postgres`는 PostgreSQL의 관리자 계정 이름입니다. 이 계정의 비밀번호를 설정합니다. 수업용으로도 추측하기 쉬운 비밀번호는 사용하지 않습니다.
6. 포트는 기본값인 `5432`를 유지합니다. 이미 다른 PostgreSQL이 5432 포트를 쓰고 있다면 사용하지 않는 포트를 정하고, 이후 연결 문자열의 `Port`도 같은 값으로 바꿉니다.
7. Locale은 기본값을 유지하고 설치를 끝냅니다.

설치가 끝나면 Windows 서비스에 `postgresql-x64-...` 형태의 서비스가 실행 중이어야 합니다. 시작 메뉴에서 `서비스`를 열어 상태가 `실행 중`인지 확인합니다. 중지 상태라면 서비스를 시작한 뒤 진행합니다.

### pgAdmin에서 첫 연결 만들기

1. 시작 메뉴에서 `pgAdmin 4`를 실행합니다.
2. 왼쪽 `Servers` 아래에 로컬 PostgreSQL 서버가 이미 보이면 선택하고, 설치 때 정한 `postgres` 비밀번호를 입력합니다.
3. 서버가 보이지 않으면 `Servers`를 마우스 오른쪽 버튼으로 누른 뒤 `Register > Server...`를 선택합니다.
4. `General` 탭의 Name에는 `Local PostgreSQL`처럼 알아보기 쉬운 이름을 입력합니다.
5. `Connection` 탭에는 아래 값을 입력하고 `Save`를 누릅니다.

| 항목 | 값 |
| :--- | :--- |
| Host name/address | `127.0.0.1` |
| Port | `5432` 또는 설치 때 정한 포트 |
| Maintenance database | `postgres` |
| Username | `postgres` |
| Password | 설치 때 설정한 비밀번호 |

> `127.0.0.1`은 현재 PC 자신을 뜻합니다. 이 실습에서는 다른 PC에서 DB에 접속하도록 설정하지 않습니다. 외부 접속에는 PostgreSQL의 접속 주소·인증 규칙과 방화벽 설정이 추가로 필요합니다.

### 설치 확인 SQL

연결한 서버의 `Databases > postgres`를 마우스 오른쪽 버튼으로 누르고 `Query Tool`을 엽니다. 아래 SQL을 실행해 서버·접속 계정·포트를 확인합니다.

```sql
SELECT version();
SELECT current_user;
SHOW port;
```

버전 정보, `postgres`, `5432`이 보이면 설치와 로컬 접속이 준비된 것입니다.

## 3. PostgreSQL에 실습용 DB 만들기

pgAdmin에서 설치 중 만든 `postgres` 계정으로 접속합니다. `postgres` 데이터베이스의 Query Tool에서 아래 두 문장을 **각각 실행**합니다.

```sql
CREATE ROLE game_api LOGIN PASSWORD '수업용_비밀번호';
CREATE DATABASE game_network_lab OWNER game_api;
```

다음으로 pgAdmin에서 `game_network_lab` 데이터베이스를 열고, [01_schema.sql](../../../../Code/GameNetwork/PostgreSqlEndToEnd/database/01_schema.sql)의 내용을 실행합니다.

```sql
CREATE TABLE player_score (
    player_id INTEGER PRIMARY KEY,
    score INTEGER NOT NULL CHECK (score >= 0),
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);
```

`player_id`가 기본 키이므로 한 플레이어의 점수 행은 하나만 존재합니다. 서버의 `ON CONFLICT ... DO UPDATE`는 같은 `player_id` 요청이 다시 오면 새 행을 만들지 않고 점수와 시간을 갱신합니다.

> `postgres` 관리 계정을 애플리케이션 연결에 사용하지 않습니다. 이 실습처럼 목적이 분명한 `game_api` 계정을 별도로 만드는 습관을 들입니다. 수업용 비밀번호와 실제 운영 비밀번호는 Git에 저장하지 않습니다.

## 4. C# 서버 실행하기

PowerShell을 열고, **현재 창에서만** DB 연결 문자열을 설정합니다. 아래의 비밀번호 부분은 앞 단계에서 정한 값으로 바꿉니다.

```powershell
$env:GAME_DB_CONNECTION = "Host=127.0.0.1;Port=5432;Database=game_network_lab;Username=game_api;Password=수업용_비밀번호"
dotnet run --project Code/GameNetwork/PostgreSqlEndToEnd/ScoreServer
```

`GAME_DB_CONNECTION`은 Windows의 `PATH`처럼 운영체제가 프로그램에 전달하는 환경 변수입니다. C# 서버는 `Environment.GetEnvironmentVariable()`로 이 값을 읽습니다. 코드나 Git 파일에 실제 비밀번호를 적지 않는 이유입니다.

아래처럼 표시되면 서버가 실행된 것입니다.

```text
Now listening on: http://127.0.0.1:5080
```

브라우저에서 `http://127.0.0.1:5080/health`를 열어 `{"status":"ok"}`가 보이는지 확인합니다.

### 서버 코드를 읽는 순서

1. `NpgsqlDataSource`가 PostgreSQL 연결과 연결 풀을 관리합니다.
2. `MapPost("/scores", ...)`가 Unity의 `POST /scores` 요청을 `SaveScoreAsync`에 연결합니다.
3. `$1`, `$2` 매개 변수로 PlayerId와 Score를 SQL과 분리해 전달합니다.
4. `INSERT ... ON CONFLICT ... DO UPDATE`가 새 플레이어는 추가하고 기존 플레이어는 점수만 갱신합니다.
5. `RETURNING`으로 DB가 확정한 행을 받아 JSON으로 Unity에 돌려줍니다.

Npgsql는 PostgreSQL용 .NET 데이터 공급자입니다. 데이터 소스 하나를 서버 수명 동안 재사용하면 연결 풀을 함께 관리할 수 있으며, SQL 값은 문자열 결합 대신 매개 변수로 전달해야 합니다. [Npgsql 기본 사용법](https://www.npgsql.org/doc/basic-usage.html)

## 5. Unity 클라이언트 준비하기

1. Unity 프로젝트의 `Assets/Scripts/`에 [ScoreApiClient.cs](../../../../Code/GameNetwork/PostgreSqlEndToEnd/UnityClient/ScoreApiClient.cs)를 복사합니다.
2. Canvas에 TextMeshPro `Text`를 하나 만들고 이름을 `ResultText`로 정합니다.
3. Canvas에 Button을 하나 만들고 글자를 `점수 저장 요청`으로 바꿉니다.
4. 빈 GameObject `ScoreApiClient`를 만들고 스크립트를 붙입니다.
5. Inspector의 `Result Text`에 `ResultText`를 연결합니다.
6. Button의 `On Click()`에 `ScoreApiClient` GameObject를 넣고 `ScoreApiClient.SendDemoScore()`를 선택합니다.
7. C# 서버가 실행 중인 상태에서 Unity Play Mode를 실행하고 버튼을 누릅니다.

성공하면 Unity 화면에 다음처럼 표시됩니다.

```text
저장 완료
PlayerId: 101
Score: 250
UpdatedAt: 2026-...
```

서버 PowerShell에는 `점수 저장: PlayerId=101, Score=250`이, pgAdmin의 `player_score` 표에는 같은 행이 보입니다. 이것이 클라이언트 요청이 DB를 거쳐 다시 클라이언트 응답으로 돌아오는 증거입니다.

## 6. 전체 흐름을 검증하기

아래 세 위치를 모두 확인해야 합니다.

| 확인 위치 | 예상 결과 | 의미 |
| :--- | :--- | :--- |
| Unity Game 화면 | `저장 완료`, PlayerId와 Score | Unity가 HTTP 응답 JSON을 읽음 |
| 서버 PowerShell | `점수 저장: PlayerId=101, Score=250` | 서버가 요청을 검증하고 SQL을 실행함 |
| pgAdmin Query Tool | `SELECT * FROM player_score;` 결과 한 행 | PostgreSQL에 실제 저장됨 |

같은 버튼을 다시 누르면 `player_id = 101` 행은 하나만 남고 `updated_at`만 새 시간으로 바뀝니다. `ScoreApiClient.cs`의 `score = 250`을 다른 값으로 바꾸고 다시 실행하면 UPSERT 결과도 확인할 수 있습니다.

## 7. 자주 발생하는 문제

| 증상 | 확인할 것 |
| :--- | :--- |
| Unity에 `Connection refused` | C# 서버가 먼저 실행 중인지, 주소와 포트가 `127.0.0.1:5080`인지 확인 |
| 서버 시작 시 환경 변수 오류 | 같은 PowerShell 창에서 `$env:GAME_DB_CONNECTION`을 설정했는지 확인 |
| 서버에서 PostgreSQL 인증 오류 | `game_api` 계정·비밀번호·DB 이름이 연결 문자열과 일치하는지 확인 |
| `relation "player_score" does not exist` | `game_network_lab`에 `01_schema.sql`을 실행했는지 확인 |
| pgAdmin에 서버가 보이지 않음 | PostgreSQL 서비스가 실행 중인지, `127.0.0.1`과 설치 포트가 맞는지 확인 |
| `5432` 포트를 사용할 수 없음 | 다른 PostgreSQL·DB 도구가 포트를 쓰는지 확인하고, 설치 포트를 바꿨다면 연결 문자열도 바꿈 |
| Unity WebGL에서 요청 실패 | 브라우저는 CORS 정책을 적용하므로 별도 CORS 설정과 HTTPS가 필요함. 이 참고 실습은 Windows Editor 로컬 실행 범위임 |

## 8. 실무 구조로 확장할 때

이 예제의 `score`는 요청 확인용 고정 값입니다. 실제 게임 서버는 Unity가 보낸 "점수 250"을 그대로 저장하지 않습니다. 서버가 플레이 기록, 보상 규칙, 중복 요청을 검증한 뒤 DB 변경을 결정합니다.

```text
Unity: "코인을 주웠습니다" 요청
서버: 거리·중복·보상 규칙 검증
PostgreSQL: 서버가 확정한 점수 저장
서버: 확정 점수를 Unity에 응답
```

외부 서버 DB를 열 때는 `listen_addresses`, `pg_hba.conf`, 방화벽, TLS, 최소 권한 DB 계정을 함께 설계해야 합니다. 이 설정은 로컬 입문 실습 다음 단계의 운영 주제입니다. [PostgreSQL 연결 설정](https://www.postgresql.org/docs/18/runtime-config-connection.html), [PostgreSQL 클라이언트 인증](https://www.postgresql.org/docs/18/client-authentication.html)
