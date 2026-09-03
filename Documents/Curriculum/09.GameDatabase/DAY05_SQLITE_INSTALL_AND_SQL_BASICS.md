# DAY 05: SQLite 설치와 첫 SQL (4교시)

오늘은 SQLite DB 파일을 만들고, DB Browser for SQLite로 SQL의 기본 문법을 직접 실행합니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 프로그래밍하기
- 주요 학습 내용: 설계된 게임 스키마를 관계형 DB로 생성하고 관리하기

## 1. SQLite는 무엇인가요?

SQLite는 별도 서버 없이 `.db` 파일 하나에 SQL 관계형 DB를 저장하는 도구입니다. 오늘은 **GUI** (Graphical User Interface, 그래픽 사용자 인터페이스) 도구로 파일을 보고, 다음 시간에는 C# 코드로 같은 일을 합니다.

### SQL은 무엇인가요?

**SQL** (Structured Query Language)은 데이터베이스에 "표를 만들어 주세요", "이 데이터를 넣어 주세요", "조건에 맞는 데이터를 보여 주세요"라고 요청하는 언어입니다. C#처럼 게임 전체의 동작을 만드는 범용 프로그래밍 언어라기보다, **데이터를 만들고·찾고·바꾸고·지우는 데 특화된 언어**라고 이해하면 됩니다.

SQL은 SQLite, MySQL, PostgreSQL, SQL Server 같은 **관계형 DBMS의 공통 기반 언어**입니다. 하지만 모든 DB가 SQL을 주된 방식으로 쓰는 것은 아닙니다. LiteDB나 MongoDB 같은 문서형 DB는 보통 C# **API** (Application Programming Interface, 응용 프로그래밍 인터페이스) 또는 제품별 문서 질의 방식을 사용합니다. 또 관계형 DB끼리도 `SELECT`, `INSERT`, `UPDATE`, `DELETE` 같은 기본 문법은 비슷하지만, 자료형·날짜 함수·자동 번호·고급 기능에는 제품별 차이(방언)가 있습니다.

```text
학생이 SQL 문장을 작성
        ↓
DB Browser 또는 C# 프로그램이 SQLite에 전달
        ↓
SQLite가 GameShop.db 파일의 데이터를 처리
        ↓
조회 결과 또는 성공/오류 메시지를 돌려줌
```

| 단어 | 역할 | 상점 비유 |
| :--- | :--- | :--- |
| DB | 데이터를 보관하는 곳 | 상점 장부 |
| DBMS | DB를 관리하고 SQL을 처리하는 프로그램 | 장부를 읽고 규칙을 확인하는 관리자 |
| SQLite | 파일 하나로 동작하는 관계형 DBMS | `GameShop.db`를 관리하는 관리자 |
| SQL | DBMS에 보내는 요청 문장 | "포션을 장부에 적어 주세요"라는 요청서 |

다음 SQL은 "Item 표에서 이름과 가격을 읽어 주세요"라는 요청입니다.

```sql
SELECT name, price
FROM Item;
```

SQL이 실행된다고 해서 C# 코드가 없어지는 것은 아닙니다. C#은 버튼을 눌렀을 때 어떤 SQL을 보낼지 결정하고 결과를 Unity 화면이나 콘솔에 표시합니다. SQL은 그중 DB를 다루는 부분을 담당합니다.

## 오늘의 4교시 흐름

| 교시 | 할 일 | 결과 |
| :--- | :--- | :--- |
| 1교시 | DB Browser for SQLite 다운로드·설치 | 도구 실행 확인 |
| 2교시 | DB 파일과 `Item` 표 생성 | `GameShop.db`와 표 1개 |
| 3교시 | SQL 기본 문법 실행 | 등록·조회·수정·삭제 결과 |
| 4교시 | 조건 조회와 미니 실습 | SQL 파일 또는 실행 기록 |

## 2. DB Browser for SQLite 다운로드와 설치 (Windows)

1. 브라우저에서 [DB Browser for SQLite 공식 다운로드](https://sqlitebrowser.org/dl/) 페이지를 엽니다.
2. Windows 항목에서 자신의 PC에 맞는 `Standard installer`를 고릅니다. 대부분의 PC는 `win64`입니다. ARM (Advanced RISC Machine, RISC는 Reduced Instruction Set Computer의 약자) 기반 PC는 ARM64 설치 파일을 고릅니다.
3. 내려받은 `.msi` 파일을 두 번 클릭합니다.
4. 설치 화면에서 기본 설치 위치를 유지하고 `Install`을 누릅니다.
5. 설치가 끝나면 시작 메뉴에서 `DB Browser for SQLite`를 실행합니다.
6. 실행이 되지 않으면 다운로드 페이지에서 `zip (no installer)` 대신 표준 설치 파일을 받았는지 확인하고, 학교 PC의 설치 권한은 강사에게 알립니다.

> 이 프로그램은 SQLite DB를 눈으로 확인하는 도구입니다. 학생 프로그램에 포함되는 DB 엔진 설치가 아닙니다.

## 3. 첫 DB 파일 만들기

1. `File > New Database`를 누릅니다.
2. 실습 폴더에 `GameShop.db`로 저장합니다.
3. `Create Table`에서 표 이름을 `Item`으로 입력합니다.
4. 아래 열을 추가한 뒤 `itemId`에 `PK`를 체크합니다.

| 열 이름 | 자료형 | 규칙 |
| :--- | :--- | :--- |
| `itemId` | INTEGER | Primary Key |
| `name` | TEXT | Not Null |
| `price` | INTEGER | Not Null |

5. `Write Changes`를 눌러 파일에 실제로 저장합니다.

## 4. SQL 기본 문법: "DB에 요청하는 짧은 문장"

SQL은 보통 명령어로 시작하고, 마지막에 세미콜론(`;`)을 붙입니다. SQL 명령어는 대문자로 쓰는 관례가 있지만, 소문자로 써도 됩니다. 수업에서는 읽기 쉽게 대문자로 통일합니다.

| 명령 | 하는 일 | 상점 예시 |
| :--- | :--- | :--- |
| `CREATE TABLE` | 새 표 만들기 | `Item` 표 생성 |
| `INSERT INTO` | 새 행 넣기 | 포션 등록 |
| `SELECT` | 데이터 조회 | 가격이 50 이상인 아이템 찾기 |
| `UPDATE` | 기존 값 수정 | 포션 가격 변경 |
| `DELETE FROM` | 행 삭제 | 테스트 아이템 삭제 |

### 표를 SQL로 만들기

DB Browser의 표 생성 화면으로도 만들 수 있지만, 아래 SQL을 실행하면 같은 `Item` 표를 만들 수 있습니다. SQL의 줄은 위에서 아래로 읽습니다. 먼저 표 이름을 정하고, 괄호 안에 열 이름과 규칙을 적습니다.

| SQL 조각 | 뜻 |
| :--- | :--- |
| `itemId` | 아이템을 구분하는 열 이름 |
| `INTEGER` | 정수 자료형 |
| `PRIMARY KEY` | 중복되지 않는 대표 번호. 한 행을 정확히 찾을 때 사용 |
| `TEXT` | 글자 자료형 |
| `NOT NULL` | 빈 값을 허용하지 않는 규칙 |

```sql
CREATE TABLE Item (
    itemId INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    price INTEGER NOT NULL
);
```

`CREATE TABLE`은 표를 한 번만 만들어야 합니다. 이미 GUI에서 `Item` 표를 만들었다면 이 SQL은 다시 실행하지 않습니다. "table already exists" 오류는 표가 이미 있다는 뜻입니다.

### 데이터를 등록하고 조회하기

`Execute SQL` 탭에 아래 SQL을 입력하고 실행합니다.

```sql
INSERT INTO Item (itemId, name, price)
VALUES (1, '회복 포션', 30);

SELECT itemId, name, price
FROM Item;
```

결과가 보이면 `Browse Data` 탭에서도 `Item` 표를 선택해 확인합니다.

## 5. SQL 문장을 조각으로 읽기

SQL은 영어 문장처럼 보이지만, 각 자리에 역할이 정해져 있습니다. 처음에는 아래 네 가지 틀을 통째로 읽고, 빈칸을 자신의 표·열·값으로 바꿔 씁니다.

### 새 데이터를 넣는 문장: `INSERT INTO ... VALUES`

```sql
INSERT INTO Item (itemId, name, price)
VALUES (1, '회복 포션', 30);
```

| 조각 | 뜻 |
| :--- | :--- |
| `INSERT INTO Item` | `Item` 표에 새 행을 넣습니다. |
| `(itemId, name, price)` | 값을 넣을 열의 순서를 적습니다. |
| `VALUES` | 이제 실제 값을 적겠다는 뜻입니다. |
| `(1, '회복 포션', 30)` | 앞의 열 순서에 맞춰 넣을 값입니다. 숫자는 그대로, 글자는 작은따옴표로 감쌉니다. |

열과 값은 같은 순서·같은 개수여야 합니다. 예를 들어 `name`에 넣을 `'회복 포션'`을 `price` 자리에 쓰면 데이터 의미가 틀어집니다.

### 데이터를 읽는 문장: `SELECT ... FROM ... WHERE`

```sql
SELECT name, price
FROM Item
WHERE price >= 50;
```

| 조각 | 뜻 |
| :--- | :--- |
| `SELECT name, price` | 보여 줄 열을 고릅니다. 모든 열을 보고 싶다면 `SELECT *`를 씁니다. |
| `FROM Item` | `Item` 표에서 읽습니다. |
| `WHERE price >= 50` | 가격이 50 이상인 행만 고릅니다. `WHERE`는 선택 사항입니다. |

### 값을 바꾸는 문장: `UPDATE ... SET ... WHERE`

```sql
UPDATE Item
SET price = 35
WHERE itemId = 1;
```

| 조각 | 뜻 |
| :--- | :--- |
| `UPDATE Item` | `Item` 표의 기존 행을 바꿉니다. |
| `SET price = 35` | `price` 열을 35로 바꿉니다. |
| `WHERE itemId = 1` | 1번 아이템만 바꿉니다. |

### 데이터를 지우는 문장: `DELETE FROM ... WHERE`

```sql
DELETE FROM Item
WHERE itemId = 1;
```

| 조각 | 뜻 |
| :--- | :--- |
| `DELETE FROM Item` | `Item` 표에서 행을 지웁니다. |
| `WHERE itemId = 1` | 1번 아이템만 지웁니다. |

> `UPDATE`와 `DELETE`에서 `WHERE`를 빼면 표의 모든 행에 적용됩니다. 실행 전에 먼저 같은 조건으로 `SELECT`를 실행해 대상 행을 확인하는 습관을 들입니다.

### 데이터를 수정하고 삭제하기

```sql
UPDATE Item
SET price = 35
WHERE itemId = 1;

DELETE FROM Item
WHERE itemId = 1;
```

`WHERE`는 "어느 행을 바꿀지 또는 지울지" 정하는 조건입니다.

### 이 단어는 무슨 뜻인가요?

- **SQL**: DB에 "만들기, 넣기, 찾기, 바꾸기, 지우기"를 요청하는 언어입니다.
- **INSERT**: 새 행을 넣습니다.
- **SELECT**: 조건에 맞는 행을 읽습니다.
- **UPDATE**: 조건에 맞는 행의 값을 바꿉니다.
- **DELETE**: 조건에 맞는 행을 지웁니다.
- **WHERE**: 명령을 적용할 행의 조건을 적습니다.
- **Primary Key**: 같은 행을 중복 없이 구별합니다.

## 실습 미션

`Item` 표에 검과 방패를 더 넣고, 가격이 50 이상인 아이템만 조회하는 SQL을 작성합니다. 이후 검의 가격을 한 번 수정하고, 테스트용 아이템 한 개를 삭제합니다.

```sql
SELECT name, price
FROM Item
WHERE price >= 50;
```

### 실행 전 확인 질문

1. `INSERT`와 `UPDATE`의 차이는 무엇인가요?
2. `DELETE FROM Item;`을 실행하면 어떤 일이 일어날까요?
3. 가격이 50 이상인 아이템을 찾을 때 `WHERE` 뒤에는 어떤 조건을 적어야 할까요?
4. `VALUES (1, '회복 포션', 30)`의 세 값은 각각 어느 열에 들어갈까요?

## 오늘의 정리

- SQLite는 파일 하나로 관계형 DB와 SQL을 연습할 수 있습니다.
- `CREATE`, `INSERT`, `SELECT`, `UPDATE`, `DELETE`는 다음 C# DB 프로그램에서도 그대로 만납니다.
- 다음 시간에는 C# 프로그램이 `GameShop.db`를 만들고 표를 생성하게 합니다.
