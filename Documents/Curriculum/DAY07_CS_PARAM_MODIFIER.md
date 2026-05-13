# 🚀 Day 07: 매개변수 한정자 (복사본 vs 원본)

오늘의 목표는 "**함수에 데이터를 줄 때 복사본을 줄지 원본을 줄지 결정하는 법을 배운다**"입니다.

---

## 1. 매개변수 한정자: "시험지 비유"

### 🍎 기본값 (복사본 전달)
- **비유**: 친구에게 내 시험지 **복사본**을 주는 것.
- **특징**: 함수 안에서 값을 바꿔도 밖의 원본은 변하지 않습니다.

```csharp
void AddOne(int n) { n++; }

int score = 10;
AddOne(score);
// score는 여전히 10!
```

### 📝 ref (참조 전달 - 원본 직접 주기)
- **비유**: 내 **원본 시험지**를 직접 주는 것.
- **특징**: 함수 안의 변화가 원본에 그대로 적용됩니다. 초기화가 필수입니다.

```csharp
void RealAddOne(ref int n) { n++; }

int realScore = 10;
RealAddOne(ref realScore);
// realScore는 11이 됨!
```

### 📥 out (출력 전용 - 빈 종이 주기)
- **비유**: 친구에게 **빈 종이**를 주며 답을 적어달라고 하는 것.
- **특징**: 함수 내부에서 반드시 값을 채워야 합니다.

```csharp
void GetPlayerStatus(out int hp, out int mp) 
{
    hp = 100; // 반드시 채워야 함!
    mp = 50;
}

int h, m;
GetPlayerStatus(out h, out m);
```

---

## 💻 실습 예제: 두 변수의 값 바꾸기 (Swap)
```csharp
using UnityEngine;

public class Day07_Practice : MonoBehaviour
{
    void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    void Start()
    {
        int x = 10, y = 20;
        Debug.Log($"바뀌기 전: x={x}, y={y}");
        
        Swap(ref x, ref y); // 원본을 보냅니다.
        
        Debug.Log($"바뀐 후: x={x}, y={y}"); // 결과: x=20, y=10
    }
}
```

---

## ✍️ 핵심 퀴즈
1. 함수 안에서 값을 수정하려고 할 때 에러가 발생하는 '읽기 전용' 한정자는? (힌트: i...)
2. `out` 한정자를 사용할 때, 함수 내부에서 반드시 해야 하는 일은?
3. `ref`와 `out` 중 호출하기 전에 반드시 원본에 값이 있어야 하는 것은?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 6)]
**매개변수 한정자**를 사용하여 플레이어의 경험치 획득과 전리품 획득 로직을 구현합니다.

**[요구 사항]**
1. `ref` 활용: `void LevelUp(ref int exp, ref int level)` 함수를 만듭니다.
   - 경험치가 100 이상이면 레벨을 1 올리고 경험치를 100 뺍니다. 원본 변수가 직접 바뀌어야 합니다.
2. `out` 활용: `bool TryGetLoot(Monster m, out string lootName)` 함수를 만듭니다.
   - 몬스터가 죽어있으면(`IsDead`) 보상 이름을 `out`으로 전달하고 `true`를 반환합니다.
   - 살아있으면 `lootName`을 `"없음"`으로 설정하고 `false`를 반환합니다.
3. 메인 로직에서 몬스터 처치 후 위 두 함수를 호출하여 플레이어의 정보를 업데이트하고 획득한 아이템을 출력하세요.

**[프로그래밍 힌트]**
- `LevelUp(ref myExp, ref myLevel);`과 같이 호출 시에도 `ref` 키워드를 붙여야 합니다.
- `if (TryGetLoot(targetMonster, out string item))` 형식을 사용하면 더 깔끔합니다.

