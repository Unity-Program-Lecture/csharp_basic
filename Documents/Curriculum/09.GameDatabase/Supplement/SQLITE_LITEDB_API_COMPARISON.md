# 참고: SQLite와 LiteDB API 비교

SQLite와 LiteDB는 모두 C#에서 파일 기반 DB를 다룰 수 있지만, 코드가 데이터를 표현하는 방식이 다릅니다.

| 목적 | SQLite: SQL을 명령으로 보냄 | LiteDB: C# 객체를 문서로 다룸 |
| :--- | :--- | :--- |
| DB 열기 | `new SqliteConnection(...)` | `new LiteDatabase(...)` |
| 데이터 구조 | `CREATE TABLE` | C# 클래스와 컬렉션 |
| 한 건 구분 | `PRIMARY KEY` | 문서의 `_id` (보통 C#의 `Id`) |
| 새 데이터 | `INSERT` + `ExecuteNonQuery()` | `collection.Insert(...)` |
| 여러 건 조회 | `SELECT` + `ExecuteReader()` | `collection.Find(...)` |
| 한 건 조회 | `SELECT ... WHERE` | `collection.FindOne(...)` |
| 수정 | `UPDATE` | `collection.Update(...)` |
| 삭제 | `DELETE` | `collection.Delete(...)` |
| 인덱스 | `CREATE INDEX` | `EnsureIndex(...)` |
| 거래 처리 | `SqliteTransaction` | 문서 작업의 필요 범위에 맞춰 별도 설계 |

## 같은 "구매 실패 로그"를 두 방식으로 생각하기

SQLite에서는 먼저 로그 표의 열을 정하고 SQL을 작성합니다.

```sql
INSERT INTO PurchaseLog (PlayerId, Message)
VALUES (1, '골드가 부족합니다.');
```

LiteDB에서는 로그 C# 객체를 만들어 컬렉션에 넣습니다.

```csharp
logs.Insert(new GameLog
{
    PlayerId = 1,
    Message = "골드가 부족합니다."
});
```

두 방식 모두 데이터를 파일에 저장합니다. 차이는 SQLite가 표·열·SQL을 중심으로, LiteDB가 C# 객체·문서·컬렉션을 중심으로 생각한다는 점입니다.

LiteDB의 `_id`는 관계형 DB의 기본 키와 비슷하게 한 문서를 고유하게 구분하지만, 관계형 DB의 `FOREIGN KEY`처럼 다른 컬렉션의 ID가 실제 존재하는지 자동으로 검사하는 제약은 없습니다.

## 선택 기준

- 재화, 인벤토리, 구매처럼 여러 데이터를 정확히 함께 바꿔야 한다면 SQLite 실습을 우선합니다.
- 로그, 설정, 퀘스트 스냅샷처럼 한 묶음으로 저장·조회하는 정보는 LiteDB 사례가 이해하기 쉽습니다.
- 온라인 게임의 운영 DB는 이 로컬 실습과 별도입니다. Unity 클라이언트가 직접 DB 파일을 고치는 대신 서버/API가 요청을 검증하는 구조를 사용합니다.

## 더 자세히 보기

- [SQLite API 빠른 참조](SQLITE_API_REFERENCE.md)
- [LiteDB API 빠른 참조](LITEDB_API_REFERENCE.md)
