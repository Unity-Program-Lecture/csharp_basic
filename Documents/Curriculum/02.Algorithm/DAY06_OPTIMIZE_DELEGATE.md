# 🚀 Day 06: 게임 구조 최적화 (함수의 변수화)

오늘의 목표는 **"대상이나 상태에 따라 실행할 함수를 변경해야 할 때, 함수를 변수처럼 저장하고 실행하는 자료형(대리자)을 배운다"**입니다.

---

## 1. 💡 이론 (30%): 대리자(Delegate)의 원리와 필요성
- 함수를 직접 호출하려면 그 함수의 이름을 미리 알고 있어야 합니다.
- 하지만 **함수 자체를 변수(대리자)에 저장**해두면, 나중에 필요할 때 매우 빠른 속도로 실행할 수 있습니다.
- **장점**: 
  1. 대상의 존재 여부나 개수를 매번 파악할 필요가 없어 효율적입니다.
  2. 상태에 따라 실행할 함수를 실시간으로 변경할 수 있습니다. (예: 평상시 공격 로직 -> 광폭화 시 공격 로직으로 교체)

---

## 2. 💻 실습 (70%): 상태에 따른 공격 로직 변경
**미션:** 대리자(Delegate)를 사용하여, 캐릭터의 현재 무기 상태(검, 활)에 따라 공격 버튼을 눌렀을 때 실행되는 함수가 동적으로 바뀌도록 구현하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // 1. 대리자 선언 (함수를 담을 틀 생성)
    public delegate void AttackAction();
    
    // 2. 대리자 변수 선언
    public AttackAction currentAttack;

    void Start()
    {
        // 초기 상태는 '검 공격'으로 설정
        EquipSword();
    }

    void Update()
    {
        // 대리자에 저장된 함수를 실행 (대상이 뭔지 알 필요 없음!)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentAttack?.Invoke(); 
        }

        // 무기 교체 테스트
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipSword();
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipBow();
    }

    // 3. 상황에 따라 대리자(함수 변수) 교체
    public void EquipSword()
    {
        currentAttack = SwordAttack;
        Debug.Log("검을 장착했습니다.");
    }

    public void EquipBow()
    {
        currentAttack = BowAttack;
        Debug.Log("활을 장착했습니다.");
    }

    // 실제 동작 함수들
    void SwordAttack() { Debug.Log("슉! 검을 휘두릅니다."); }
    void BowAttack() { Debug.Log("피융! 화살을 쏩니다."); }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 함수를 변수의 형태로 저장하는 방법의 일종입니다. 대상을 몰라도 함수만 가지고 있다가 실행할 수 있으며, 상태에 따라 함수를 변경할 때 사용하는 이 자료형의 이름은 무엇입니까?
   - **정답:** 대리자 (Delegate)
2. **문제:** 대리자 변수에 아무 함수도 들어있지 않을 때(Null) 실행하는 것을 방지하기 위해 사용하는 안전한 호출 문법은 무엇인가요?
   - **정답:** `?.Invoke()` (Null 조건부 연산자 활용)
