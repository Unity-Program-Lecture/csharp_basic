# 🏆 1~9일차 통합 실습: 몬스터 아레나 관리 시스템 (Monster Arena)

본 미션은 1일차부터 9일차까지 배운 C#의 핵심 개념들을 하나의 프로그램으로 엮어보는 종합 선물 세트입니다. 각 요구 사항이 어떤 학습 내용과 연결되는지 확인하며 구현해 보세요.

---

## 🎯 미션 목표
**"몬스터 대기열(Queue)에서 몬스터를 꺼내 아레나(List)에 배치하고, 플레이어가 공격하여 전리품(out)을 획득하는 시스템을 구축하라!"**

---

## 📋 [요구 사항 및 연결 개념]

### 1. 데이터의 기초 (Day 01, 05)
- **[개념: 구조체]** 몬스터의 위치를 나타내는 `struct Point`를 만드세요. (필드: `int x, y`)
- **[개념: 변수/타입]** 플레이어의 이름, 공격력, 현재 경험치를 관리할 변수를 선언하세요.

### 2. 설계도와 약속 (Day 03, 06)
- **[개념: 인터페이스]** 공격받을 수 있는 모든 대상의 규칙인 `interface IDamageable`을 만드세요.
  - 프로퍼티: `int HP { get; set; }`, `bool IsDead { get; }`
  - 메소드: `void TakeDamage(int damage)`
- **[개념: 추상 클래스]** `abstract class Entity`를 만들고 `IDamageable`을 구현하세요.
  - 생성자를 통해 이름과 초기 HP를 설정합니다.

### 3. 상속과 개성 (Day 04)
- **[개념: 상속/오버라이딩]** `NormalMonster`와 `BossMonster` 클래스를 만드세요.
  - `BossMonster`는 `TakeDamage`를 오버라이드하여 "보스는 데미지를 50%만 받습니다"라는 특성을 추가하세요.

### 4. 특별한 도구들 (Day 07, 08)
- **[개념: 매개변수 한정자]** 전투 결과 전리품을 돌려주는 함수를 만드세요.
  - `bool TryGetLoot(Monster m, out string item)` : 몬스터가 죽었다면 아이템 이름을 내뱉고 `true`를 반환합니다.
- **[개념: 제네릭/제약 조건]** 몬스터를 생성하고 관리하는 `ArenaManager<T> where T : Entity` 클래스를 만드세요.

### 5. 데이터 묶음 관리 (Day 02, 09)
- **[개념: 컬렉션 - Queue]** `Queue<Monster> spawnQueue`를 만들어 소환 대기 중인 몬스터들을 담으세요.
- **[개념: 컬렉션 - List]** `List<Monster> activeMonsters`를 만들어 현재 아레나에서 싸우고 있는 몬스터들을 관리하세요.
- **[개념: 반복문/조건문]** `while`문을 사용하여 대기열이 빌 때까지 몬스터를 아레나로 옮기고, `foreach`문을 사용하여 아레나의 모든 몬스터를 공격하세요.

---

## 💡 프로기래밍 힌트 (비유)
1. **인터페이스(IDamageable)**: "공격 버튼이 작동하려면 최소한 HP라는 전선이 연결되어 있어야 한다"는 규격서입니다.
2. **제네릭(ArenaManager<T>)**: "몬스터 전용 경기장" 혹은 "용병 전용 경기장"처럼 특정 타입만 받는 전용 경기장 틀입니다.
3. **Queue(소환 대기열)**: 던전 입구에서 차례를 기다리는 몬스터들의 줄입니다.
4. **out(전리품)**: 빈 손으로 들어간 함수가 나올 때 아이템을 쥐어서 나오는 주머니와 같습니다.

---

## 💻 코드 뼈대 (가이드)

```csharp
using System;
using System.Collections.Generic;

// 1. 구조체 (Day 05)
public struct Point { public int x, y; }

// 2. 인터페이스 (Day 06)
public interface IDamageable { ... }

// 3. 추상 클래스 및 상속 (Day 03, 04, 06)
public abstract class Entity : IDamageable { ... }
public class NormalMonster : Entity { ... }
public class BossMonster : Entity { ... }

// 4. 제네릭 관리 클래스 (Day 08, 09)
public class ArenaManager<T> where T : Entity
{
    private List<T> activeMonsters = new List<T>();
    private Queue<T> spawnQueue = new Queue<T>();

    public void AddToQueue(T monster) { ... }
    public void StartBattle() { ... } // 여기서 큐에서 리스트로 옮기고 전투 진행
}

// 5. 메인 실행부 (Day 01, 02, 07)
class Program
{
    static void Main()
    {
        // 아레나 생성, 몬스터 추가, 전투 시작 로직 구현
    }

    // out 키워드 활용 함수 (Day 07)
    static bool TryGetLoot(Entity e, out string loot) { ... }
}
```

---

## 🚩 최종 결과 예시
```text
[시스템] 몬스터 3마리가 대기열에 진입했습니다.
[소환] '슬라임'이 아레나에 입장했습니다! (좌표: 1, 2)
[전투] 플레이어가 '슬라임'을 공격합니다.
[결과] '슬라임' 처치 완료! 전리품: [슬라임 점액]을 획득했습니다.
...
[종료] 아레나에 남은 몬스터가 없습니다. 총 획득 경험치: 500
```
