# 🚀 Day 08: 게임 알고리즘 기초 (유한 상태 머신, FSM)

오늘의 목표는 **"NPC나 몬스터의 인공지능을 설계할 때 가장 기본이 되는 알고리즘인 '유한 상태 머신(FSM)'의 개념을 이해하고 구현한다"**입니다.

---

## 1. 💡 이론 (30%): FSM (Finite State Machine)
- **개념**: 기계나 캐릭터가 '유한한 개수의 상태(State)' 중 단 하나의 상태만 가질 수 있으며, 특정 조건(Event)에 따라 다른 상태로 전환(Transition)되는 수학적 모델입니다.
- **예시 (몬스터 AI)**:
  - **상태**: `Idle`(대기), `Trace`(추적), `Attack`(공격), `Die`(사망)
  - **전환 조건**: 플레이어가 시야에 들어옴(Idle -> Trace), 거리가 공격 사거리 이내임(Trace -> Attack).
- **장점**: 행동을 상태별로 쪼개어 관리하므로 코드가 스파게티처럼 꼬이는 것을 막아줍니다. `switch-case`문이나 다형성(추상 클래스/인터페이스)을 이용해 구현합니다.

---

## 2. 💻 실습 (70%): 몬스터 상태 전이 구현
**미션:** 열거형(Enum)과 `switch-case`문을 사용하여, 플레이어와의 거리에 따라 상태가 변하는 몬스터의 기초 AI 알고리즘을 작성하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class MonsterFSM : MonoBehaviour
{
    // 1. 상태 열거형 정의
    public enum State { Idle, Trace, Attack }
    public State currentState = State.Idle;

    public Transform player;
    public float traceDist = 10f; // 추적 시작 거리
    public float attackDist = 2f; // 공격 시작 거리

    void Update()
    {
        // 2. 상태에 따른 행동 분기
        switch (currentState)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Trace:
                UpdateTrace();
                break;
            case State.Attack:
                UpdateAttack();
                break;
        }
    }

    void UpdateIdle()
    {
        Debug.Log("주위를 두리번 거립니다.");
        float dist = Vector3.Distance(transform.position, player.position);
        
        // 전이 조건: 플레이어가 10m 이내로 들어오면 추적 시작
        if (dist <= traceDist)
        {
            currentState = State.Trace;
        }
    }

    void UpdateTrace()
    {
        Debug.Log("플레이어를 향해 뛰어갑니다!");
        float dist = Vector3.Distance(transform.position, player.position);

        // 전이 조건 1: 사거리 내로 들어오면 공격
        if (dist <= attackDist) currentState = State.Attack;
        // 전이 조건 2: 너무 멀어지면 다시 대기
        else if (dist > traceDist) currentState = State.Idle;
    }

    void UpdateAttack()
    {
        Debug.Log("플레이어를 공격합니다!");
        float dist = Vector3.Distance(transform.position, player.position);

        // 전이 조건: 거리가 멀어지면 다시 추적
        if (dist > attackDist) currentState = State.Trace;
    }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 게임 내 NPC가 '대기, 이동, 공격'과 같이 유한한 개수의 상태를 가지고 조건에 따라 상태를 전환하며 행동하게 만드는 AI 알고리즘 모델의 이름은 무엇입니까?
   - **정답:** 유한 상태 머신 (FSM, Finite State Machine)
