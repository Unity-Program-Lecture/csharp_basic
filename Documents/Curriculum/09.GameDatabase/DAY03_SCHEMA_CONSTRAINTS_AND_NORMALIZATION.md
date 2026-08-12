# DAY 03: 스키마, 제약 조건, 정규화 (4교시)

오늘은 데이터가 잘못 저장되지 않도록 스키마와 제약 조건을 만들고, 반복된 정보를 정리합니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 설계하기
- 주요 학습 내용: 관계형 DB 저장 구조 설계, 게임 스키마 작성의 기초

## 1. 핵심 개념: "장부 작성 규칙"

스키마는 표의 이름, 열의 자료형, 키, 관계, 제한을 정한 규칙입니다. 좋은 규칙은 실수를 코드 밖에서도 막아 줍니다.

| 규칙 | 상점 예시 | 막아 주는 문제 |
| :--- | :--- | :--- |
| 기본 키 | `PlayerId`는 중복 불가 | 같은 플레이어가 두 명 생김 |
| 외래 키 | Inventory의 ItemId는 Item에 존재 | 없는 아이템을 보유함 |
| `NOT NULL` | 아이템 이름은 비워 둘 수 없음 | 이름 없는 아이템 |
| `CHECK (Gold >= 0)` | 골드는 0 미만 불가 | 음수 골드 |
| `UNIQUE` | 아이템 코드 중복 불가 | 같은 코드의 다른 아이템 |

## 2. 제약 조건을 한 줄씩 읽어 보기

제약 조건은 "프로그램이 조심해서 넣을 것"이라고 부탁하는 것이 아니라, DB가 잘못된 데이터를 거부하게 만드는 규칙입니다.

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

### `REFERENCES`는 어디를 가리키나요?

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

따라서 Player 표에 1번 플레이어가 없는데 `playerId = 1`인 인벤토리를 등록하면 DB가 거부할 수 있습니다. SQLite에서는 이 규칙을 실제로 검사하려면 연결한 뒤 `PRAGMA foreign_keys = ON;`을 실행해야 하며, 이 내용은 DAY08에서 실습합니다.

### `NOT NULL`과 `CHECK`의 차이

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

## 3. 정규화: "중복 쪽지를 분리하기"

다음처럼 플레이어 표에 포션 이름과 가격을 반복하면 가격을 바꿀 때 여러 줄을 고쳐야 합니다.

| PlayerName | ItemName | ItemPrice | Quantity |
| :--- | :--- | ---: | ---: |
| 민지 | 포션 | 30 | 3 |
| 준호 | 포션 | 30 | 1 |

`Item` 표에는 아이템의 고정 정보를, `Inventory` 표에는 보유 수량만 둡니다. 이것이 중복을 줄이는 정규화의 출발입니다.

## 4. 실습: 잘못된 표 고치기

**미션:** 아래 비정규 표를 `Player`, `Item`, `Inventory`, `PurchaseLog`로 나눕니다.

```text
구매번호, 플레이어이름, 골드, 아이템이름, 가격, 구매수량, 구매시각, 오류메시지
```

1. 표마다 기본 키를 적습니다.
2. 다른 표를 참조하는 열에는 `(FK)`를 표시합니다.
3. "아이템 가격 변경" 때 수정할 표를 하나만 고릅니다.

### 제약 조건 확인 문제

1. 아이템 이름을 반드시 입력하게 하려면 `NOT NULL`과 `CHECK` 중 무엇을 사용해야 할까요?
2. 골드를 음수로 만들 수 없게 하려면 어떤 `CHECK` 조건을 쓸 수 있을까요?
3. 없는 `ItemId`를 Inventory에 넣지 못하게 하려면 어떤 키 규칙이 필요할까요?
4. `REFERENCES Item(itemId)`는 어느 표의 어느 열을 가리키나요?

## 생각해보기

1. 가격을 Inventory에 저장하면 언제 문제가 될까요?
2. 오류 메시지는 모든 구매에 반드시 있어야 할까요? 문서형 로그가 유리할 수 있는 이유는 무엇일까요?

## 오늘의 정리

- 스키마는 데이터의 설계도, 제약 조건은 장부의 안전장치입니다.
- 다음 시간에는 문서형 DB에 같은 게임 정보를 어떻게 담는지 비교합니다.
