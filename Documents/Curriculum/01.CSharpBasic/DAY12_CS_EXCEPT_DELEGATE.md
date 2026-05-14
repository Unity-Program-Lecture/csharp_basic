# 🚀 Day 12: 방어 코드와 대리인 (Exception & Delegate)

오늘의 목표는 "**프로그램의 폭발을 막는 법(예외 처리)과 일을 대신 시키는 법(대리자)을 배운다**"입니다.

---

## 1. 예외 처리 (Exception): "에어백 설치"
`try-catch-finally`를 사용하여 예상치 못한 에러에도 프로그램이 멈추지 않게 합니다.

```csharp
try 
{
    int[] arr = new int[2];
    arr[5] = 10; // 에러 발생!
}
catch (System.IndexOutOfRangeException e) 
{
    Debug.LogError($"배열 범위 초과: {e.Message}");
}
finally 
{
    Debug.Log("이 코드는 무조건 실행됩니다.");
}
```

---

## 2. 델리게이트 (Delegate): "심부름꾼"
함수 자체를 변수에 담아 전달하는 기술입니다.

```csharp
public delegate void MyDelegate(string msg);

void ShowMessage(string message) { Debug.Log(message); }

// 사용 예시
MyDelegate del = ShowMessage;
del("안녕!");
```

---

## 3. 이벤트 (Event): "알림 벨"
"무슨 일이 터지면 나한테 알려줘!"라고 등록해두는 안전한 델리게이트입니다.

---

## 💻 실습 예제: 몬스터 사망 이벤트
```csharp
using UnityEngine;

public class Monster
{
    public delegate void DeathHandler();
    public event DeathHandler OnDeath;

    public void Die()
    {
        Debug.Log("몬스터가 쓰러졌습니다!");
        OnDeath?.Invoke(); 
    }
}

public class Day12_Practice : MonoBehaviour
{
    void Start()
    {
        Monster m = new Monster();
        m.OnDeath += () => Debug.Log("보상을 획득했습니다!");
        m.Die();
    }
}
```

---

## ✍️ 핵심 퀴즈
1. 예외가 발생하든 안 하든 무조건 실행되는 블록의 이름은?
2. 함수를 변수처럼 담아서 전달할 수 있게 해주는 타입은?
3. `OnDeath?.Invoke();` 에서 `?.`는 어떤 의미인가요?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 11)]
**예외 처리**로 예기치 못한 에러를 방지하고, **이벤트**를 활용해 몬스터 사망 시 자동으로 보상이 지급되는 시스템을 만듭니다.

**[요구 사항]**
1. **이벤트 시스템:** `Monster` 클래스에 `event Action<Monster> OnDeadEvent`를 만듭니다.
   - 몬스터가 죽을 때 `OnDeadEvent?.Invoke(this)`를 호출하세요.
2. **보상 매니저:** `RewardManager` 클래스를 만들고, 몬스터 소환 시 해당 몬스터의 `OnDeadEvent`에 보상 지급 함수를 등록합니다.
   - 등록된 함수는 몬스터가 죽을 때 "X 몬스터로부터 보상을 획득했습니다!"를 출력합니다.
3. **예외 처리 (Try-Catch):** 플레이어의 공격력을 계산할 때 0으로 나누기(`DivideByZeroException`)나 잘못된 인덱스 접근(`IndexOutOfRangeException`)이 발생할 수 있는 상황을 가정하고, 에러 메시지를 예쁘게 출력한 뒤 게임이 멈추지 않게 하세요.

**[프로그래밍 힌트]**
- `Action<T>`는 리턴값이 없는 함수를 담는 미리 정의된 편리한 델리게이트입니다.
- `OnDeadEvent += (monster) => { ... }`와 같이 람다식을 사용해 이벤트를 구독할 수 있습니다.
- `finally` 블록에서 "데미지 계산 시도 완료"와 같은 메시지를 남겨보세요.

