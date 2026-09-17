# 2일차: RDB 설계와 제약 조건 (6교시)

오늘은 RPG 상점의 데이터를 표로 나누고, 표 사이의 관계를 ERD (Entity Relationship Diagram, 개체 관계 다이어그램)로 표현합니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 설계하기
- 주요 학습 내용: 관계형 데이터베이스의 게임 데이터 저장 구조 설계하기, 게임 스키마 작성의 기초

## 1. 핵심 개념: "표와 표를 잇는 지도"

**ERD** (Entity Relationship Diagram)는 어떤 표가 있고 표들이 어떻게 연결되는지 보여 주는 지도입니다. 한 표에는 한 종류의 대상을 넣습니다.

```text
Player 1명 ── 여러 Inventory 기록 ── Item 1종
```

| 표 | 보관하는 대상 | 예시 열 |
| :--- | :--- | :--- |
| `Player` | 플레이어 | `PlayerId`, `Name`, `Gold` |
| `Item` | 아이템 기획 정보 | `ItemId`, `Name`, `Price` |
| `Inventory` | 누가 무엇을 몇 개 가졌는가 | `PlayerId`, `ItemId`, `Quantity` |

### 이 단어는 무슨 뜻인가요?

- **테이블(Table)**: 같은 종류의 정보를 모은 표입니다. `Player`는 플레이어만, `Item`은 아이템만 보관합니다.
- **행(Row, Record)**: 표 안의 데이터 한 건입니다. 플레이어 민지의 정보 한 줄이 Player의 행입니다.
- **열(Column, Field)**: 모든 행이 공통으로 갖는 정보 칸입니다. `Name`, `Gold`가 열입니다.
- **엔터티(Entity)**: 따로 관리할 대상입니다. 예: 플레이어, 아이템. 보통 테이블 하나로 표현합니다.
- **속성(Attribute)**: 엔터티가 가진 정보입니다. 예: 플레이어 이름. 보통 열 하나로 표현합니다.
- **키(Key)**: 행을 찾거나 다른 표와 연결하기 위해 사용하는 값입니다.
- **기본 키(Primary Key, PK)**: 각 행을 유일하게 구별하는 대표 키입니다. 비어 있거나 중복되면 안 됩니다.
- **외래 키(Foreign Key, FK)**: 다른 표의 기본 키를 참조하는 연결 칸입니다.
- **1:N 관계**: 한 플레이어가 여러 인벤토리 기록을 갖는 관계입니다.

## 2. Primary Key는 왜 필요한가요?

기본 키는 학생 번호나 주민등록번호처럼 "이 행이 누구인지"를 확실히 구분하는 값입니다. 이름은 같을 수 있고 나중에 바뀔 수도 있으므로, 보통 이름을 기본 키로 사용하지 않습니다.

| PlayerId (PK) | Name | Gold |
| ---: | :--- | ---: |
| 1 | 민지 | 100 |
| 2 | 민지 | 250 |

두 사람의 이름은 같아도 `PlayerId`가 다르므로 서로 다른 플레이어임을 알 수 있습니다.

### 좋은 기본 키의 조건

1. **유일성**: 다른 행과 값이 겹치지 않습니다.
2. **변하지 않음**: 이름, 닉네임처럼 자주 바뀌는 값을 피합니다.
3. **비어 있지 않음**: 어떤 행인지 모르면 수정·삭제할 때 실수할 수 있습니다.

## 3. Foreign Key는 왜 필요한가요?

인벤토리는 Player와 Item을 이어 주는 표입니다. `Inventory.PlayerId`는 Player 표의 `PlayerId`를, `Inventory.ItemId`는 Item 표의 `ItemId`를 가리킵니다.

```text
Inventory.PlayerId (FK) -> Player.PlayerId (PK)
Inventory.ItemId   (FK) -> Item.ItemId     (PK)
```

외래 키를 선언할 때는 가리킬 표와 열을 `REFERENCES 표이름(열이름)`으로 명시합니다. 예를 들어 아래 문장은 "Inventory의 PlayerId는 Player 표의 PlayerId를 가리킨다"라고 읽습니다.

```sql
FOREIGN KEY (PlayerId) REFERENCES Player(PlayerId)
```

이렇게 참조 대상을 적어야 DB가 "어느 표에 실제로 있는 번호인지" 확인할 수 있습니다. SQLite에서는 연결을 열 때 `PRAGMA foreign_keys = ON;`도 실행해야 이 검사가 실제로 켜집니다. 이 설정의 자세한 이유와 확인 실습은 오늘 후반 교시에서 다룹니다.

| PlayerId (FK) | ItemId (FK) | Quantity |
| ---: | ---: | ---: |
| 1 | 10 | 3 |
| 1 | 20 | 1 |

이 표는 "플레이어 1번이 아이템 10번을 3개, 20번을 1개 가졌다"는 뜻입니다. FK 규칙을 사용하면 Item 표에 없는 아이템 번호를 인벤토리에 넣는 실수를 막을 수 있습니다.

## 4. 설계 순서

1. 기획 문장에서 명사를 찾습니다.
2. "한 개를 여러 번 저장하게 되는가?"를 확인합니다.
3. 반복되는 정보는 별도 표로 분리합니다.
4. 각 표에 기본 키를 정합니다.
5. 연결 표에 외래 키와 수량 같은 관계 정보를 둡니다.

## 5. 안내형 실습: 상점 ERD 그리기

**미션:** `Player`, `Item`, `Inventory` 표를 종이에 그리고 다음 조건을 만족시킵니다.

- Player 한 명은 포션과 검을 모두 가질 수 있습니다.
- 같은 아이템 가격을 플레이어마다 반복해서 쓰지 않습니다.
- `Inventory`에는 `Quantity`가 있습니다.

그린 뒤 아래 질문에 답합니다.

1. `Item.Name`과 `Item.Price`를 `Inventory`에 넣지 않은 이유는 무엇인가요?
2. `Inventory`의 한 행을 유일하게 구별하려면 어떤 키 조합이 필요할까요?

### 키 확인 미니 문제

아래 중 기본 키로 가장 알맞은 것을 고르고 이유를 한 문장으로 적습니다.

| 후보 | 기본 키로 적합한가요? | 이유 |
| :--- | :--- | :--- |
| 플레이어 이름 |  |  |
| 플레이어 닉네임 |  |  |
| PlayerId |  |  |

정답을 고른 뒤, `Inventory` 표에서 `PlayerId`와 `ItemId`가 각각 어느 표를 가리키는지도 화살표로 그립니다.

## 6. 실제 만들기: ERD를 `GameShop.db` 표와 행으로 옮기기

1일차의 `GameShop.db`를 열고, ERD에서 설계한 Player와 Inventory를 실제 표로 만듭니다. 아래 SQL을 `Execute SQL` 탭에서 실행합니다.

```sql
CREATE TABLE IF NOT EXISTS Player (
    PlayerId INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Gold INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS Inventory (
    PlayerId INTEGER NOT NULL,
    ItemId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL CHECK (Quantity >= 0),
    PRIMARY KEY (PlayerId, ItemId),
    FOREIGN KEY (PlayerId) REFERENCES Player(PlayerId),
    FOREIGN KEY (ItemId) REFERENCES Item(itemId)
);

INSERT OR IGNORE INTO Player (PlayerId, Name, Gold)
VALUES (1, '민지', 100);

INSERT OR IGNORE INTO Inventory (PlayerId, ItemId, Quantity)
VALUES (1, 1, 3);

SELECT Player.Name, Item.name, Inventory.Quantity
FROM Inventory
JOIN Player ON Inventory.PlayerId = Player.PlayerId
JOIN Item ON Inventory.ItemId = Item.itemId;
```

### 이 실습에서 처음 만나는 SQL 키워드

| 코드 조각 | 이렇게 읽기 | 사용 목적 |
| :--- | :--- | :--- |
| `CREATE TABLE IF NOT EXISTS Player` | "Player 표가 아직 없을 때만 만든다" | 같은 표를 다시 만들려다 오류가 나는 일을 막음 |
| `INSERT OR IGNORE` | "넣되, 같은 기본 키가 있으면 넘어간다" | 같은 초기 데이터를 다시 실행해도 중복 행이 생기지 않게 함 |
| `JOIN 표이름 ON 조건` | "조건이 맞는 행끼리 표를 이어 읽는다" | 인벤토리의 번호를 플레이어·아이템 이름으로 바꾸어 함께 조회함 |

`INSERT OR IGNORE`는 기존 행을 수정하거나 덮어쓰지 않습니다. 이 교시에서는 "민지와 회복 포션이 아직 없을 때만 준비한다"는 초기 데이터 목적에 맞습니다. 이미 있는 값을 바꿔야 하는 상황의 처리 방법은 5일차에서 배웁니다.

마지막 `SELECT`는 `Inventory`를 기준으로 시작한 뒤, `ON Inventory.PlayerId = Player.PlayerId`처럼 두 번호가 같은 행을 찾아 `Player`를 붙이고, 같은 방식으로 `Item`을 붙입니다. 그래서 번호만 있는 인벤토리에서 플레이어 이름과 아이템 이름까지 함께 볼 수 있습니다.

### 완료 확인

- [ ] `Player`, `Item`, `Inventory`의 역할을 각각 설명할 수 있다.
- [ ] `Inventory`의 `(PlayerId, ItemId)` 조합을 기본 키로 표시했다.
- [ ] 두 외래 키가 각각 어느 표를 가리키는지 화살표로 표시했다.
- [ ] `GameShop.db`에서 Player·Inventory 표와 민지의 포션 보유 행을 확인했다.

## 응용 실습: 장비 강화 재료 설계

강화 재료를 여러 아이템에 사용할 수 있게 `Material`과 연결 표를 추가로 설계하세요. 어떤 표에 가격과 보유 수량을 둘지도 한 문장으로 설명하세요.

## 전반 정리 및 다음 교시

- ERD는 코드를 쓰기 전 데이터의 중복과 빠진 관계를 찾는 도구입니다.
- 후반 교시에는 만든 표에 제약 조건을 적용하고, 잘못된 데이터를 실제로 거절하는지 확인합니다.

---

## 2일차 후반: 스키마, 제약 조건, 정규화

앞 교시에서 설계한 표에 제약 조건을 적용하고, 잘못된 데이터가 실제로 거절되는지 확인합니다. 기본 키·외래 키 관계를 다시 설계하지 않고 `NOT NULL`, `CHECK`, `UNIQUE` 규칙과 오류 확인에 집중합니다.

### 핵심 개념: "장부 작성 규칙"

스키마는 표의 이름, 열의 자료형, 키, 관계, 제한을 정한 규칙입니다. 좋은 규칙은 실수를 코드 밖에서도 막아 줍니다.

| 규칙 | 상점 예시 | 막아 주는 문제 |
| :--- | :--- | :--- |
| `NOT NULL` | 아이템 이름은 비워 둘 수 없음 | 이름 없는 아이템 |
| `CHECK (Gold >= 0)` | 골드는 0 미만 불가 | 음수 골드 |
| `UNIQUE` | 아이템 코드 중복 불가 | 같은 코드의 다른 아이템 |

기본 키와 외래 키의 관계는 앞 교시에서 만든 ERD를 그대로 사용합니다. 이 교시의 목표는 그 관계와 열 값에 잘못된 데이터가 들어갈 때 DB가 어떤 오류로 거절하는지 관찰하는 것입니다.

### 제약 조건을 한 줄씩 읽어 보기

제약 조건은 "프로그램이 조심해서 넣을 것"이라고 부탁하는 것이 아니라, DB가 잘못된 데이터를 거부하게 만드는 규칙입니다.

이제 SQL (Structured Query Language, 구조화 질의 언어)로 표와 규칙을 작성합니다. SQL은 DB에 데이터를 만들고, 찾고, 바꾸도록 요청하는 언어입니다.

```sql
CREATE TABLE Item (
    itemId INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    price INTEGER NOT NULL CHECK (price >= 0),
    itemCode TEXT UNIQUE
);
```

| 코드 조각 | 쉬운 뜻 | 넣으려 하면 어떻게 될까요? |
| :--- | :--- | :--- |
| `PRIMARY KEY` | 각 아이템을 구분하는 고유 번호 | 같은 `itemId`를 두 번 넣을 수 없음 |
| `NOT NULL` | 이 칸은 비워 둘 수 없음 | 이름이나 가격 없이 아이템을 등록할 수 없음 |
| `CHECK (price >= 0)` | 괄호 안 조건을 만족해야 함 | 가격이 `-30`인 아이템 등록 거부 |
| `UNIQUE` | 다른 행과 값이 겹치면 안 됨 | 같은 `itemCode`를 두 번 등록할 수 없음 |
| `FOREIGN KEY` | 다른 표에 있는 값만 참조함 | 없는 플레이어/아이템 번호의 인벤토리 등록 거부 |

#### `REFERENCES`는 어디를 가리키나요?

`REFERENCES 표이름(열이름)`은 외래 키가 **어느 표의 어느 열**을 가리키는지 적는 문법입니다. 아래 Inventory 표는 Player와 Item에 실제로 있는 번호만 사용할 수 있게 만듭니다.

```sql
CREATE TABLE Inventory (
    playerId INTEGER NOT NULL,
    itemId INTEGER NOT NULL,
    quantity INTEGER NOT NULL CHECK (quantity >= 0),
    PRIMARY KEY (playerId, itemId),
    FOREIGN KEY (playerId) REFERENCES Player(playerId),
    FOREIGN KEY (itemId) REFERENCES Item(itemId)
);
```

코드는 위에서 아래로 읽습니다.

1. `playerId`와 `itemId` 열을 만듭니다.
2. `(playerId, itemId)` 조합으로 인벤토리 한 행을 구분합니다.
3. `playerId`는 `Player` 표의 `playerId`를 참조한다고 선언합니다.
4. `itemId`는 `Item` 표의 `itemId`를 참조한다고 선언합니다.

따라서 Player 표에 1번 플레이어가 없는데 `playerId = 1`인 인벤토리를 등록하면 DB가 거부할 수 있습니다. SQLite에서는 이 규칙을 실제로 검사하려면 연결한 뒤 `PRAGMA foreign_keys = ON;`을 실행해야 합니다. `PRAGMA`는 SQLite 연결의 동작 규칙을 설정·확인하는 명령이며, 이 설정은 연결을 새로 열 때마다 다시 적용합니다.

#### `NOT NULL`과 `CHECK`의 차이

- `NOT NULL`은 **값을 적었는가**를 확인합니다. 예: 아이템 이름을 비워 둘 수 없습니다.
- `CHECK`는 **적은 값이 규칙에 맞는가**를 확인합니다. 예: 가격은 0 이상이어야 합니다.

```sql
-- 이름을 비워 두어 NOT NULL 규칙에 걸리는 예
INSERT INTO Item (itemId, name, price)
VALUES (1, NULL, 30);

-- 가격이 음수여서 CHECK 규칙에 걸리는 예
INSERT INTO Item (itemId, name, price)
VALUES (2, '회복 포션', -30);
```

> 위 SQL은 오류를 관찰하기 위한 예입니다. 이미 같은 `itemId`가 있다면 번호를 바꾸거나 테스트 전용 DB에서 실행합니다.

### 정규화: "중복 쪽지를 분리하기"

다음처럼 플레이어 표에 포션 이름과 가격을 반복하면 가격을 바꿀 때 여러 줄을 고쳐야 합니다.

| PlayerName | ItemName | ItemPrice | Quantity |
| :--- | :--- | ---: | ---: |
| 민지 | 포션 | 30 | 3 |
| 준호 | 포션 | 30 | 1 |

`Item` 표에는 아이템의 고정 정보를, `Inventory` 표에는 보유 수량만 둡니다. 이것이 중복을 줄이는 정규화의 출발입니다.

### 안내형 실습: 잘못된 표 고치기

**미션:** 아래 비정규 표를 `Player`, `Item`, `Inventory`, `PurchaseLog`로 나눕니다.

```text
구매번호, 플레이어이름, 골드, 아이템이름, 가격, 구매수량, 구매시각, 오류메시지
```

1. 표마다 기본 키를 적습니다.
2. 다른 표를 참조하는 열에는 `(FK)`를 표시합니다.
3. "아이템 가격 변경" 때 수정할 표를 하나만 고릅니다.

#### 제약 조건 확인 문제

1. 아이템 이름을 반드시 입력하게 하려면 `NOT NULL`과 `CHECK` 중 무엇을 사용해야 할까요?
2. 골드를 음수로 만들 수 없게 하려면 어떤 `CHECK` 조건을 쓸 수 있을까요?
3. 없는 `ItemId`를 Inventory에 넣지 못하게 하려면 어떤 키 규칙이 필요할까요?
4. `REFERENCES Item(itemId)`는 어느 표의 어느 열을 가리키나요?

### 생각해보기

1. 가격을 Inventory에 저장하면 언제 문제가 될까요?
2. 오류 메시지는 모든 구매에 반드시 있어야 할까요? 문서형 NoSQL 로그가 유리할 수 있는 이유는 무엇일까요?

### 실제 만들기: 잘못된 데이터를 DB가 거절하는지 확인하기

앞 교시에서 만든 `GameShop.db`를 열고, 아래 SQL을 **각 블록별로 따로** 실행합니다. 오류 메시지가 보이는 것은 실습 실패가 아니라 제약 조건이 동작했다는 증거입니다.

먼저 없는 ItemId를 인벤토리에 넣어 외래 키 검사를 확인합니다.

```sql
PRAGMA foreign_keys = ON;

INSERT INTO Inventory (PlayerId, ItemId, Quantity)
VALUES (1, 999, 1);
```

다음으로 가격이 음수인 아이템을 거절할 테스트 전용 표를 만듭니다. 1일차의 `Item` 표를 억지로 바꾸지 않고, 제약 조건의 결과만 안전하게 관찰합니다.

```sql
CREATE TABLE IF NOT EXISTS ItemConstraintTest (
    ItemId INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Price INTEGER NOT NULL CHECK (Price >= 0)
);

INSERT INTO ItemConstraintTest (ItemId, Name, Price)
VALUES (1, '오류 확인용 아이템', -30);
```

두 번째 `INSERT`가 거절된 뒤 `SELECT * FROM ItemConstraintTest;`를 실행해 행이 추가되지 않았음을 확인합니다. 여기서 `SELECT`는 조회, `FROM ItemConstraintTest`는 그 표에서 조회한다는 뜻이며, `*`는 "모든 열"을 뜻합니다. 테스트에서는 전체 열을 빠르게 확인할 때 편리하지만, 필요한 결과가 정해진 조회에서는 1일차처럼 열 이름을 직접 적는 편이 좋습니다.

#### 완료 확인

- [ ] 기본 키, 외래 키, `NOT NULL`, `CHECK`가 막는 실수를 각각 말할 수 있다.
- [ ] 비정규 표를 역할이 다른 표로 나누고 각 표의 키를 표시했다.
- [ ] SQLite에서 외래 키 검사를 켜려면 연결마다 `PRAGMA foreign_keys = ON;`이 필요함을 설명할 수 있다.
- [ ] 없는 ItemId와 음수 가격 입력이 각각 거절되는 오류 메시지를 확인했다.

### 응용 실습: 중복 닉네임 규칙 추가

같은 서버 안에서 플레이어 닉네임이 겹치지 않게 하려면 어느 열에 어떤 제약 조건을 추가할지 SQL로 작성하세요. 닉네임 변경 때 생길 수 있는 문제도 한 가지 적으세요.

### 오늘의 정리

- 스키마는 데이터의 설계도, 제약 조건은 장부의 안전장치입니다.
- 다음 시간에는 SQL로 데이터를 조건에 따라 조회·수정·삭제합니다.
