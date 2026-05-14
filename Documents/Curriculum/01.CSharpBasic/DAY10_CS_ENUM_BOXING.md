# 🚀 Day 10: 열거형과 박싱 (상태 관리와 포장)

오늘의 목표는 "**가독성을 높이는 열거형(Enum)을 익히고, 값 형식과 참조 형식을 넘나드는 Boxing/Unboxing의 개념을 이해한다**"입니다.

---

## 1. 열거형(Enum): "숫자에 붙인 이름표"
숫자 대신 사람이 이해하기 쉬운 이름으로 상태를 관리합니다.

```csharp
public enum ItemType 
{
    Weapon,   // 0
    Armor,    // 1
    Potion    // 2
}

ItemType myItem = ItemType.Weapon;
```

---

## 2. Boxing & Unboxing: "선물 포장"
- **Boxing**: 값 형식을 상자에 담아 힙(Heap)으로 보내는 것.
- **Unboxing**: 상자에서 데이터를 꺼내는 것.
- **주의**: 자주 발생하면 성능이 떨어집니다.

```csharp
int n = 10;
object obj = n;      // Boxing (값 -> 참조)
int m = (int)obj;    // Unboxing (참조 -> 값)
```

---

## 💻 실습 예제: 게임 상태 관리
```csharp
using UnityEngine;

public enum GameState { Start, Play, End }

public class Day10_Practice : MonoBehaviour
{
    void Start()
    {
        GameState state = GameState.Start;
        Debug.Log($"현재 상태: {state}");

        // 박싱 예시
        int num = 123;
        object box = num; // Boxing
        
        int unboxed = (int)box; // Unboxing
        Debug.Log($"박싱 후 꺼낸 값: {unboxed}");
    }
}
```

---

## ✍️ 핵심 퀴즈
1. `int` 같은 값 형식을 `object` 타입으로 바꾸는 과정을 무엇이라 하나요?
2. `enum`의 각 항목은 내부적으로 어떤 데이터 타입으로 저장되나요? (기본값)
3. 박싱과 언박싱이 성능에 안 좋은 영향을 미치는 주된 이유는?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 9)]
**열거형(Enum)**을 도입하여 몬스터의 등급과 현재 인공지능 상태를 체계적으로 관리합니다.

**[요구 사항]**
1. `MonsterRank` 열거형을 만듭니다. (`Common`, `Elite`, `Boss`)
2. `MonsterState` 열거형을 만듭니다. (`Idle`, `Chase`, `Attack`, `Dead`)
3. `Monster` 클래스에 위 두 타입을 멤버 변수로 추가합니다.
4. `switch`문을 활용하여 몬스터의 현재 상태(`MonsterState`)에 따라 다른 로그를 출력하는 `UpdateAI()` 메소드를 만드세요.
   - `Idle`: "주변을 배회합니다."
   - `Attack`: "플레이어를 공격합니다!"
5. 등급(`MonsterRank`)에 따라 공격력 배율을 다르게 적용해 보세요. (예: Boss는 공격력 3배)

**[프로그래밍 힌트]**
- `if (state == MonsterState.Dead)` 처럼 가독성 좋게 코드를 짤 수 있습니다.
- `(int)monsterRank`와 같이 형변환(Casting)을 통해 열거형의 숫자 값을 가져올 수도 있습니다.

