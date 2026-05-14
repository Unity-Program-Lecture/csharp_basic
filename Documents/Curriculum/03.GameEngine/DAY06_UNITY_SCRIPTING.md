# 🚀 Day 06: 유니티 스크립팅과 생명주기 (Unity Scripting & Lifecycle)

오늘의 목표는 "**유니티의 스크립트 실행 순서(Lifecycle)를 이해하고, 엔진과 C# 코드가 상호작용하는 원리를 마스터한다**"입니다.

---

## 1. 모노비헤이비어 (MonoBehaviour)
유니티의 모든 스크립트 컴포넌트는 `MonoBehaviour`를 상속받아야 엔진의 제어를 받을 수 있습니다.

### 📍 핵심 생명주기 메소드 (Execution Order)
1. **Awake**: 객체 생성 시 가장 먼저 호출. (설정 초기화)
2. **Start**: 첫 번째 프레임 업데이트 전에 호출.
3. **Update**: 매 프레임마다 호출. (로직 처리)
4. **FixedUpdate**: 일정한 시간 간격으로 호출. (물리 연산 전용)
5. **OnDestroy**: 객체가 파괴될 때 호출. (정리 작업)

---

## 2. 엔진 메시지 시스템
유니티는 특정 상황이 발생하면 스크립트의 메소드를 자동으로 호출합니다. (이벤트 방식)
- **비유**: 벨이 울리면(`Collision`) 문을 여는(`OnCollisionEnter`) 것과 같습니다.

---

## 💻 실습 예제: 델리게이트를 이용한 동적 행동 교체 (교재 응용)
NCS 교재의 '함수 변수화' 개념을 유니티 스크립팅에 적용해 봅니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem; // 최신 인풋 시스템

public class PlayerAction : MonoBehaviour
{
    public delegate void BehaviorDelegate();
    public BehaviorDelegate currentBehavior;

    void Start()
    {
        currentBehavior = Idle;
    }

    void Update()
    {
        currentBehavior?.Invoke();

        if (Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame) currentBehavior = Jump;
            if (Keyboard.current.digit1Key.wasPressedThisFrame) currentBehavior = Idle;
        }
    }

    void Idle() { Debug.Log("숨 고르기 중..."); }
    void Jump() { Debug.Log("점프!!"); }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 유니티 스크립트에서 물리 엔진 연산을 처리할 때 사용해야 하는 전용 업데이트 메소드는?
   - **정답:** `FixedUpdate()`
2. **문제:** 모든 유니티 컴포넌트 스크립트가 상속받아야 하는 기본 클래스의 이름은?
   - **정답:** `MonoBehaviour`
