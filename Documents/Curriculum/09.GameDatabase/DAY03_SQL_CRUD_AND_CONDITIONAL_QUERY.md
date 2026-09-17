# 3일차: SQLite CRUD와 SQL 트랜잭션 (6교시)

오늘은 1일차에 만든 SQLite `GameShop.db`를 다시 열고, SQL로 조건 조회·수정·삭제를 실행합니다. 후반에는 여러 변경을 묶는 `BEGIN`·`COMMIT`·`ROLLBACK`도 SQL로 직접 실행합니다. 1일차에 배운 `CREATE TABLE`, `INSERT INTO`, `SELECT ... FROM`은 필요한 경우에만 짧게 복습합니다.

> 이 문서는 1일차의 표 만들기와 기본 SQL 실습을 완료한 뒤 진행합니다. 오류가 있어 복습이 필요하면 1일차 문서를 확인합니다.

## NCS 연결

- 능력단위 요소: 게임 데이터베이스 프로그래밍하기
- 주요 학습 내용: 설계된 게임 스키마를 관계형 DB로 생성하고 관리하기

## 1. 시작 전 확인

1일차에서 만든 `GameShop.db`를 DB Browser for SQLite로 엽니다. `Browse Data` 탭에서 `Item` 표와 회복 포션 한 건이 보여야 합니다. 파일이나 표가 없다면 1일차의 "SQLite 설치와 첫 아이템 저장" 실습을 먼저 다시 실행합니다.

오늘은 1일차의 SQL 기초를 반복하지 않습니다. 다만 이 문서의 SQL은 SQLite 기준이며, `SELECT`, `INSERT`, `UPDATE`, `DELETE` 같은 기본 문법은 다른 관계형 DBMS에도 비슷하지만 고급 기능의 세부 문법은 제품마다 다를 수 있습니다.

## 오늘의 6교시 흐름

| 교시 | 할 일 | 결과 |
| :--- | :--- | :--- |
| 1교시 | 테스트 아이템 추가, 조건 조회 | 검·방패의 조회 결과 |
| 2교시 | 가격 수정과 대상 확인 | 수정 전후 결과 |
| 3교시 | 테스트 데이터 삭제와 대상 확인 | SQL 실행 기록 |
| 4교시 | SQL 트랜잭션 정상 구매 | 골드·포션이 함께 바뀐 결과 |
| 5교시 | SQL 트랜잭션 실패와 롤백 | 골드·포션이 함께 복원된 결과 |
| 6교시 | 응용 실습과 결과 점검 | 안전한 SQL 실행 기록 |

## 2. 조건을 붙여 조회하기: `SELECT ... WHERE`

1일차의 `INSERT INTO ... VALUES` 형식을 이용해 2번 철 검과 3번 나무 방패를 추가한 뒤, 아래 조건 조회를 실행합니다.

```sql
INSERT INTO Item (itemId, name, price)
VALUES (2, '철 검', 100);

INSERT INTO Item (itemId, name, price)
VALUES (3, '나무 방패', 60);

SELECT name, price
FROM Item
WHERE price >= 50;
```

| 조각 | 뜻 |
| :--- | :--- |
| `SELECT name, price` | 보여 줄 열을 고릅니다. 모든 열을 보고 싶다면 `SELECT *`를 씁니다. |
| `FROM Item` | `Item` 표에서 읽습니다. |
| `WHERE price >= 50` | 가격이 50 이상인 행만 고릅니다. `WHERE`는 선택 사항입니다. |

## 3. 값을 바꾸기: `UPDATE ... SET ... WHERE`

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

## 4. 테스트 행 지우기: `DELETE FROM ... WHERE`

```sql
DELETE FROM Item
WHERE itemId = 1;
```

| 조각 | 뜻 |
| :--- | :--- |
| `DELETE FROM Item` | `Item` 표에서 행을 지웁니다. |
| `WHERE itemId = 1` | 1번 아이템만 지웁니다. |

> `UPDATE`와 `DELETE`에서 `WHERE`를 빼면 표의 모든 행에 적용됩니다. 실행 전에 먼저 같은 조건으로 `SELECT`를 실행해 대상 행을 확인하는 습관을 들입니다.

## 안내형 실습 미션

`Item` 표에 방패와 테스트용 아이템을 더 넣고, 가격이 50 이상인 아이템만 조회하는 SQL을 작성합니다. 이후 철 검의 가격을 한 번 수정하고, 테스트용 아이템 한 개를 삭제합니다.

```sql
SELECT name, price
FROM Item
WHERE price >= 50;
```

### 실행 전 확인 질문

1. `INSERT`와 `UPDATE`의 차이는 무엇인가요?
2. `DELETE FROM Item;`을 실행하면 어떤 일이 일어날까요?
3. 가격이 50 이상인 아이템을 찾을 때 `WHERE` 뒤에는 어떤 조건을 적어야 할까요?
4. `VALUES (2, '철 검', 100)`의 세 값은 각각 어느 열에 들어갈까요?

### 완료 확인

- [ ] 1일차의 `GameShop.db`와 `Item` 표를 열어 회복 포션을 확인했다.
- [ ] `INSERT`, `SELECT`, `UPDATE`, `DELETE`를 각각 한 번 이상 실행했다.
- [ ] `UPDATE` 또는 `DELETE` 전에 같은 조건의 `SELECT`로 대상 행을 확인했다.

## 5. 여러 변경을 묶기: SQL 트랜잭션

구매는 골드 차감과 포션 수량 증가가 **함께** 성공해야 합니다. `BEGIN;`은 변경 묶음을 시작하고, `COMMIT;`은 묶음 전체를 확정하며, `ROLLBACK;`은 `BEGIN;` 뒤의 변경을 모두 취소합니다.

먼저 현재 상태를 확인합니다. 2일차에서 만든 인벤토리 행이 없다면 `PlayerId = 1`, `ItemId = 1` 행을 먼저 준비합니다.

```sql
SELECT PlayerId, Gold
FROM Player
WHERE PlayerId = 1;

SELECT PlayerId, ItemId, Quantity
FROM Inventory
WHERE PlayerId = 1 AND ItemId = 1;
```

### 정상 구매: 함께 확정하기

아래 블록을 순서대로 실행합니다. 마지막 `COMMIT;` 전까지는 변경이 확정되지 않습니다.

```sql
BEGIN;

UPDATE Player
SET Gold = Gold - 30
WHERE PlayerId = 1 AND Gold >= 30;

UPDATE Inventory
SET Quantity = Quantity + 1
WHERE PlayerId = 1 AND ItemId = 1;

COMMIT;
```

실행 뒤 다시 `SELECT`하여 골드가 30 줄고 포션 수량이 1 늘었는지 확인합니다.

### 실패 구매: 직접 롤백하기

이번에는 첫 번째 변경 뒤에 `CHECK (Quantity >= 0)` 제약 조건을 일부러 위반합니다. 두 번째 `UPDATE`에서 오류가 나면 **`COMMIT;`을 실행하지 않고**, 같은 DB Browser 연결에서 `ROLLBACK;`을 실행합니다.

```sql
BEGIN;

UPDATE Player
SET Gold = Gold - 30
WHERE PlayerId = 1 AND Gold >= 30;

UPDATE Inventory
SET Quantity = -1
WHERE PlayerId = 1 AND ItemId = 1;

ROLLBACK;
```

마지막 `ROLLBACK;` 뒤 다시 조회합니다. 첫 번째 `UPDATE`가 실행됐더라도 골드와 포션 수량은 트랜잭션 시작 전 값으로 돌아와야 합니다.

| 상황 | `COMMIT` 전 | `ROLLBACK` 뒤 |
| :--- | :--- | :--- |
| 첫 SQL만 성공 | 골드는 임시로 줄어든 상태 | 골드와 포션 모두 시작 전 상태 |
| 두 번째 SQL이 제약 조건 오류 | 변경을 확정하지 않음 | 첫 SQL의 골드 차감도 취소 |

> `COMMIT`이 성공한 뒤 발견한 업무 오류는 `ROLLBACK`으로 취소할 수 없습니다. 이 경우에는 반대 변경을 새 트랜잭션으로 기록해 보정합니다.

### 완료 확인

- [ ] `BEGIN`·두 `UPDATE`·`COMMIT`으로 정상 구매를 확정했다.
- [ ] 두 번째 `UPDATE`의 제약 조건 오류 뒤 `ROLLBACK;`을 실행했다.
- [ ] 롤백 뒤 첫 번째 `UPDATE`의 골드 차감도 취소됨을 확인했다.

## 응용 실습: 희귀도별 아이템 찾기

`Item` 표에 `rarity` 열을 추가하고, 희귀도가 `Rare` 이상인 아이템만 조회하는 SQL을 작성하세요. 문자열 값에는 왜 작은따옴표가 필요한지도 설명하세요.

## 오늘의 정리

- `WHERE`는 조회·수정·삭제할 대상을 좁히고, 트랜잭션은 여러 변경을 함께 확정하거나 취소합니다.
- 다음 시간에는 C# 프로그램에서 연결·CRUD를 수행하고, 5일차에는 오늘의 트랜잭션을 코드로 자동화합니다.
