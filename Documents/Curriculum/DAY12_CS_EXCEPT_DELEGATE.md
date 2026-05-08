# 🚀 Day 12: 방어 코드와 대리인 (Exception & Delegate)

오늘의 목표는 "**프로그램의 예상치 못한 폭발을 막는 법(예외 처리)과 일을 대신 시키는 법(대리자)을 배운다**"입니다.

---

## 1. 예외 처리 (Exception): "에어백 설치"
사용자가 숫자를 넣어야 할 곳에 문자를 넣는 등, 예측할 수 없는 사고가 났을 때 프로그램이 꺼지지 않게 보호합니다.
- **`try`**: "이 코드를 한번 시도해봐!"
- **`catch`**: "만약 에러가 나면 여기서 처리해!"
- **`finally`**: "에러가 나든 안 나든 이건 꼭 실행해!"

---

## 2. 델리게이트 (Delegate): "심부름꾼"
함수 그 자체를 변수에 담아 전달하는 기술입니다. "내가 지금 당장 실행하지 않고, 나중에 네가 필요할 때 이 함수를 대신 실행해줘!"라고 할 때 씁니다.

---

## 3. 이벤트 (Event): "알림 벨"
델리게이트를 더 안전하게 감싼 기능입니다. "무슨 일이 터지면(예: 몬스터 사망) 나한테 알려줘!"라고 등록해두는 방식입니다. 유니티 UI 버튼이나 몬스터 사냥 보상 시스템에서 가장 많이 쓰입니다.

---

## 💻 실습 예제: 나눗셈 에러 막기와 몬스터 보상
**미션:** 0으로 나누는 실수를 `try-catch`로 막고, 몬스터가 죽었을 때 이벤트를 통해 전리품을 지급하는 구조를 만드세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

class Monster
{
    public delegate void DeathHandler(); // 델리게이트 정의
    public event DeathHandler OnDeath;  // 이벤트 선언

    public void Die()
    {
        Console.WriteLine("몬스터가 쓰러졌습니다!");
        OnDeath?.Invoke(); // 등록된 함수들 호출
    }
}

class Program
{
    static void Main()
    {
        // 1. 예외 처리 실습
        try {
            int n = 0;
            int result = 10 / n;
        } catch (Exception e) {
            Console.WriteLine("에러 발생: 0으로 나눌 수 없습니다!");
        }

        // 2. 이벤트 실습
        Monster m = new Monster();
        m.OnDeath += () => Console.WriteLine("보상으로 100골드를 획득했습니다!");
        m.Die();
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. 예외가 발생하든 안 하든 무조건 실행되는 블록의 이름은?
2. 함수를 변수처럼 담아서 전달할 수 있게 해주는 타입은?
3. `OnDeath?.Invoke();` 에서 `?.`는 어떤 의미인가요? (비유: 비어있지 않으면 실행해라)
