# 1일차: 게임 데이터 선택과 SQLite 첫 실습 (3교시)

오늘은 게임의 정보가 어디에 있어야 하는지 판단하고, 관계형 데이터베이스 (**DB**, Database)와 NoSQL DB를 고르는 기준을 익힙니다. 문서형 DB는 NoSQL의 한 종류이며, 이 과정에서는 SQLite와 문서형 NoSQL DB인 LiteDB를 실습합니다. 이어서 SQLite를 설치하고 `GameShop.db` 파일에 첫 아이템을 직접 저장합니다.

## NCS (National Competency Standards, 국가직무능력표준) 연결

- 능력단위: `0803020532_18v4 게임 데이터베이스 프로그래밍`
- 능력단위 요소: 게임 데이터베이스 설계하기
- 주요 학습 내용: 게임 기획에 따라 데이터 구조를 결정하고 데이터베이스를 선정하기

## 1. 핵심 개념: "게임의 기억을 맡기는 장부"

게임 실행 중의 변수는 손에 든 메모처럼 게임을 끄면 사라질 수 있습니다. 데이터베이스는 여러 프로그램이 함께 보고 오래 보관할 정보를 관리하는 장부입니다.

| 저장 장소 | 예시 | 역할 |
| :--- | :--- | :--- |
| Unity 변수 | 현재 이동 속도, 임시 공격력 | 실행 중 계산 |
| ScriptableObject | 포션 이름, 아이콘, 기본 가격 | 개발자가 정한 기획 원본 |
| 데이터베이스 | 플레이어 골드, 인벤토리, 구매 기록 | 플레이에 따라 바뀌고 보관할 정보 |

### 이 단어는 무슨 뜻인가요?

- **DB** (**Database**, 데이터베이스): 데이터를 체계적으로 보관하는 장소입니다.
- **DBMS** (**Database Management System**, 데이터베이스 관리 시스템): DB를 만들고, 찾고, 바꾸고, 보호하는 프로그램입니다.
- **스키마**: DB에 어떤 표나 문서가 있고 어떤 규칙으로 연결되는지 적은 설계도입니다.
- **관계형 DB (RDB, Relational Database, 관계형 데이터베이스)**: 행과 열로 된 표를 관계로 연결하는 DB입니다. SQLite, MySQL, PostgreSQL, SQL Server가 대표적인 RDB입니다.
- **NoSQL** (**Not Only SQL**, 비관계형 DB 방식을 포괄하는 이름): 관계형 표와 SQL만을 중심으로 설계하지 않는 여러 DB 방식입니다.
- **문서형 NoSQL DB**: **JSON** (JavaScript Object Notation, 자바스크립트 객체 표기법)처럼 한 덩어리의 문서에 관련 정보를 묶어 보관하는 NoSQL DB입니다.
- **SQL** (Structured Query Language, 구조화 질의 언어): 관계형 DB에 표를 만들고 데이터를 넣거나 찾도록 요청하는 언어입니다.

### 관계형 DB와 NoSQL의 대표 예시

| 상위 분류 | 하위 유형 또는 방식 | 대표 제품 | 이 과정의 실습 |
| :--- | :--- | :--- | :--- |
| 관계형 DB (RDB) | 표·행·열·관계 | SQLite, MySQL, PostgreSQL, SQL Server | SQLite |
| NoSQL | 문서형 | LiteDB, MongoDB | LiteDB |
| NoSQL | 키-값형 | Redis | 개념만 확인 |
| NoSQL | 컬럼형 | Apache Cassandra, HBase | 개념만 확인 |
| NoSQL | 그래프형 | Neo4j | 개념만 확인 |

> NoSQL은 문서형 DB와 같은 뜻이 아닙니다. 문서형은 NoSQL 안에 있는 여러 유형 중 하나입니다. 이 과정에서는 입문자가 실제로 완성할 수 있는 범위로 문서형 NoSQL인 LiteDB만 구현합니다.

## 2. 무엇을 어디에 저장할까요?

| 게임 정보 | 추천 장소 | 이유 |
| :--- | :--- | :--- |
| 아이템 이름과 기본 가격 | ScriptableObject 또는 Item 표 | 기획 원본을 한 곳에서 관리 |
| 계정, 재화, 인벤토리 | 관계형 DB | 정확한 관계와 거래 처리가 중요 |
| 접속 기록, 오류 기록 | 문서형 NoSQL DB | 기록마다 내용이 조금 달라도 됨 |
| 화면 해상도, 사운드 설정 | 문서형 NoSQL DB 또는 로컬 설정 파일 | 한 사용자 설정을 묶어 저장 |

## 3. SQL 첫 문장 읽기

SQL은 영어 단어를 조합한 명령문이지만, 영어 문장을 완벽히 번역하려 하기보다 **명령 → 대상 표 → 열과 값 → 조건** 순서로 읽으면 쉽습니다. 키워드는 관례로 대문자로 쓰고, 표·열 이름은 `Item`, `price`처럼 씁니다. 대소문자 자체보다 일관성이 더 중요합니다.

| 문법 약속 | 뜻 | 예 |
| :--- | :--- | :--- |
| 키워드 | DB에 시키는 일을 나타내는 예약어 | `SELECT`, `INSERT INTO` |
| 표·열 이름 | 데이터를 넣거나 찾을 장소와 칸 | `Item`, `price` |
| 괄호와 쉼표 | 여러 열·값을 묶고 구분함 | `(itemId, name, price)` |
| 작은따옴표 | 글자 값을 감쌈 | `'회복 포션'` |
| 줄바꿈·들여쓰기 | SQL의 뜻은 바꾸지 않고 읽기 쉽게 나눔 | `SELECT ...` 다음 줄에 `FROM ...` |
| 세미콜론 `;` | SQL 한 문장의 끝을 표시하고 여러 문장을 구분함 | `SELECT * FROM Item;` |

SQL에서 줄바꿈과 들여쓰기는 Python처럼 문법으로 강제되지 않습니다. 아래 두 문장은 같은 뜻이지만, 수업에서는 키워드마다 줄을 나누어 읽기 쉽게 씁니다.

```sql
SELECT name, price FROM Item WHERE price >= 50;

SELECT name, price
FROM Item
WHERE price >= 50;
```

세미콜론도 API에 SQL 문장 하나만 전달할 때는 없이 실행되는 경우가 있지만, DB Browser·콘솔에서는 문장이 끝났음을 알려 주고 여러 문장을 구분하는 데 필요할 수 있습니다. 따라서 이 과정의 모든 SQL 문장 끝에는 세미콜론을 붙입니다.

### 1) `CREATE TABLE`: 표 만들기

```sql
CREATE TABLE Item (
    itemId INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    price INTEGER NOT NULL
);
```

`CREATE TABLE Item`은 "Item이라는 표를 만든다"라고 읽습니다. 괄호 안에는 표의 열과 규칙을 적습니다. `INTEGER`는 정수, `TEXT`는 글자 자료형입니다. `PRIMARY KEY`는 행을 구별하는 번호이고, `NOT NULL`은 값을 비워 둘 수 없다는 규칙입니다.

`NULL`은 `0`이나 빈 문자열 `''`과 다르게 "값이 아직 없거나 알 수 없음"을 뜻하는 특별한 값입니다. `NOT NULL`이 붙은 `name`, `price` 열에는 `NULL`을 넣을 수 없지만, 규칙이 없는 열에는 넣을 수 있습니다.

```sql
CREATE TABLE Memo (
    title TEXT NOT NULL,
    description TEXT
);

INSERT INTO Memo (title, description)
VALUES ('점검 메모', NULL);  -- description은 NULL 허용
```

위 예제에서 `title`에 `NULL`을 넣으면 오류가 나지만, `description`에는 `NOT NULL`이 없으므로 `NULL`을 넣을 수 있습니다.

### 표 만들 때 자주 붙이는 규칙

| 키워드 | 쉬운 뜻 | 게임 데이터 예시 |
| :--- | :--- | :--- |
| `PRIMARY KEY` | 한 행을 중복 없이 구별하는 대표 번호 | `itemId`로 각 아이템을 구별 |
| `NOT NULL` | `NULL`을 허용하지 않아 반드시 값을 받음 | 이름·가격을 비워 두지 않음 |
| `UNIQUE` | 다른 행과 값이 겹치면 안 됨 | 아이템 코드나 서버 안의 닉네임 중복 방지 |
| `CHECK (조건)` | 괄호 안 규칙을 만족해야 함 | `CHECK (price >= 0)`으로 음수 가격 방지 |
| `DEFAULT 값` | 값을 생략하면 미리 정한 값을 넣음 | 수량을 생략했을 때 `DEFAULT 0` 적용 |
| `REFERENCES 표(열)` | 다른 표에 있는 번호를 가리킴 | 인벤토리의 아이템 번호가 `Item` 표를 가리킴 |

```sql
CREATE TABLE ItemExample (
    itemId INTEGER PRIMARY KEY,
    itemCode TEXT NOT NULL UNIQUE,
    price INTEGER NOT NULL CHECK (price >= 0),
    quantity INTEGER NOT NULL DEFAULT 0
);
```

이 예제는 아이템 번호를 고유하게 정하고, 코드 중복과 음수 가격을 막으며, 수량을 생략하면 `0`으로 시작하게 합니다. `REFERENCES`로 표와 표를 연결하는 자세한 방법과 제약 조건이 잘못된 데이터를 거절하는 실습은 2일차에서 진행합니다.

### 2) `INSERT INTO ... VALUES`: 새 행 넣기

```sql
INSERT INTO Item (itemId, name, price)
VALUES (1, '회복 포션', 30);
```

`INSERT INTO Item`은 "Item 표 안에 넣는다"이고, 첫 괄호는 **어느 열에**, `VALUES` 뒤 괄호는 **그 열 순서에 맞춰 어떤 값을** 넣는지 나타냅니다. 따라서 위 문장은 "Item 표의 번호·이름·가격 칸에 1·회복 포션·30을 넣는다"라고 읽습니다.

### 3) `SELECT ... FROM`: 필요한 열 조회하기

```sql
SELECT itemId, name, price
FROM Item;
```

`SELECT`는 "고른다", `FROM Item`은 "Item 표에서"입니다. 즉 "Item 표에서 번호, 이름, 가격을 골라 보여 준다"라는 뜻입니다. `SELECT * FROM Item;`처럼 `*`를 쓰면 모든 열을 조회하지만, 처음에는 필요한 열을 직접 적는 편이 결과를 읽기 쉽습니다.

## 4. 안내형 실습: SQLite 설치와 첫 아이템 저장

1일차 전에 [환경 준비](ENVIRONMENT_SETUP_GUIDE.md)의 .NET SDK와 DB Browser 실행 확인을 마쳤다면, 아래 순서로 SQLite 파일을 만듭니다.

1. [DB Browser for SQLite 공식 다운로드](https://sqlitebrowser.org/dl/) 페이지에서 Windows용 `Standard installer`를 내려받아 설치합니다.
2. 설치 뒤 시작 메뉴에서 `DB Browser for SQLite`를 실행합니다. 실행할 수 없다면 학교 PC의 설치 권한을 강사에게 알립니다.
3. `File > New Database`를 누르고 실습 폴더에 `GameShop.db`로 저장합니다.
4. `Execute SQL` 탭에서 아래 SQL을 위에서 아래로 실행합니다. 방금 읽은 `CREATE TABLE`, `INSERT INTO ... VALUES`, `SELECT ... FROM`을 한 번씩 직접 사용합니다.

```sql
CREATE TABLE Item (
    itemId INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    price INTEGER NOT NULL
);

INSERT INTO Item (itemId, name, price)
VALUES (1, '회복 포션', 30);

SELECT itemId, name, price
FROM Item;
```

5. 실행 결과에 회복 포션 한 행이 보이는지 확인하고, `Browse Data` 탭에서도 같은 한 건이 보이는지 확인합니다.

> DB Browser for SQLite는 SQLite 파일을 눈으로 확인하는 도구입니다. 학생 프로그램에 DB 엔진을 별도로 설치하는 것은 아닙니다.

## 5. 응용 실습: RPG (Role-Playing Game, 역할 수행 게임) 상점 기획문에서 데이터 찾기

**미션:** 아래 문장에서 저장할 정보에 밑줄을 긋고, 저장 장소를 정합니다.

> 플레이어는 골드로 포션과 검을 구매한다. 아이템마다 이름, 가격, 아이콘이 있다. 플레이어는 여러 아이템을 여러 개 보유할 수 있다. 구매 실패와 접속 오류는 기록으로 남긴다.

1. `기획 원본`, `관계형 DB`, `문서형 NoSQL DB` 세 칸을 만듭니다.
2. 문장의 명사를 각 칸에 배치합니다.
3. "왜 그곳에 저장하는가"를 한 문장으로 적습니다.

## 생각해보기

1. 플레이어 골드를 ScriptableObject에 저장하면 어떤 문제가 생길까요?
2. 구매 기록마다 오류 메시지의 항목 수가 다르다면 표와 문서 중 어느 쪽이 편할까요?

### 완료 확인

- [ ] DB Browser for SQLite를 실행하고 `GameShop.db`를 만들었다.
- [ ] `Item` 표에 회복 포션을 저장하고 `SELECT` 결과를 확인했다.
- [ ] `CREATE TABLE`, `INSERT INTO ... VALUES`, `SELECT ... FROM`을 각각 어떤 순서로 읽는지 설명할 수 있다.
- [ ] 기획 원본, 관계형 DB, 문서형 NoSQL DB에 저장할 정보를 구분했다.
- [ ] RDB가 관계형 DB를 뜻함을 설명하고, 문서형 DB가 NoSQL의 한 종류임을 설명할 수 있다.
- [ ] 각 선택의 이유를 데이터 특성과 연결해 설명했다.

## 추가 응용 실습: 친구 목록 저장 위치 고르기

친구 목록, 친구 요청 기록, 접속 오류 기록을 각각 어디에 저장할지 정하고 이유를 한 문장씩 적으세요.

## 오늘의 정리

- DB 선택은 유행이 아니라 "어떤 데이터를, 어떤 규칙으로, 얼마나 안전하게 다룰 것인가"의 문제입니다.
- 다음 시간에는 오늘 만든 `Item` 표를 출발점으로 플레이어, 아이템, 인벤토리의 관계를 ERD (Entity Relationship Diagram, 개체 관계 다이어그램)로 그립니다.
