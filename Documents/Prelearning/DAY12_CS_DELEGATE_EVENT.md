# 🚀 12일차: 소식을 전합니다! (델리게이트와 이벤트)

오늘의 목표는 **"메소드를 변수처럼 전달하고, 특정 상황이 발생했을 때 여러 기능을 동시에 실행하는 알림 시스템을 배운다"**입니다.

---

## 1. 델리게이트(Delegate): "대리자 혹은 대역"
메소드를 직접 호출하는 대신, 메소드를 담아두었다가 대신 실행해 주는 **'메소드 바구니'**입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **델리게이트 (Delegate)**: "대신한다"는 뜻입니다. 특정 형식의 메소드를 대신 보관하고 실행합니다.
- **콜백 (Callback)**: 일이 끝난 뒤에 나중에 실행해 달라고 부탁해 둔 메소드를 말합니다.

### 💻 실습 예제: 어떤 계산기든 돌리는 만능 실행기
**미션:** 두 숫자를 계산하는 메소드를 델리게이트에 담아 실행해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

namespace Day12
{
    // 1. "정수 둘을 받아 정수를 돌려주는 메소드"를 담을 형식을 선언합니다.
    delegate int CalcDelegate(int a, int b);

    internal class Program
    {
        static int Add(int a, int b) => a + b;
        static int Multiply(int a, int b) => a * b;

        static void Main(string[] args)
        {
            CalcDelegate myCalc; // 바구니 생성

            myCalc = Add; // 더하기 메소드 담기
            Console.WriteLine("더하기 결과: " + myCalc(10, 20));

            myCalc = Multiply; // 곱하기 메소드로 갈아 끼우기
            Console.WriteLine("곱하기 결과: " + myCalc(10, 20));
        }
    }
}
```

</details>

---

## 2. 이벤트(Event): "구독과 좋아요"
델리게이트의 기능을 더 안전하게 감싼 **'알림 장치'**입니다. 유니티에서 "버튼을 눌렀을 때", "몬스터가 죽었을 때"와 같은 신호를 보낼 때 핵심적으로 사용됩니다.

### 💡 이 단어는 무슨 뜻인가요?
- **발행자 (Publisher)**: 이벤트(사건)를 발생시키는 사람 (예: 몬스터)
- **구독자 (Subscriber)**: 이벤트가 발생하면 행동할 사람 (예: UI, 보상 시스템)
- **`+=` (구독)**: "그 일이 생기면 나한테도 알려줘!"라고 등록하는 것입니다.

### 💻 실습 예제: 버튼 클릭 시스템
**미션:** 버튼이 클릭되었을 때 여러 메시지가 동시에 출력되는 이벤트 시스템을 시뮬레이션해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

namespace Day12
{
    class MyButton
    {
        // 1. 이벤트 선언 (델리게이트를 기반으로 함)
        public event Action OnClick;

        public void Press()
        {
            Console.WriteLine("버튼이 눌렸습니다!");
            OnClick?.Invoke(); // 구독자가 있다면 모두 실행!
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            MyButton btn = new MyButton();

            // 2. 이벤트 구독 (여러 개 등록 가능)
            btn.OnClick += () => Console.WriteLine("사운드 재생: 딸깍!");
            btn.OnClick += () => Console.WriteLine("UI 팝업: 메뉴 열림");

            // 3. 실행
            btn.Press();
        }
    }
}
```

</details>

---

## 3. 12일차 미션: "알람 시계 만들기"
다음 조건에 맞는 프로그램을 만들어보세요.

1. `AlarmClock` 클래스를 만듭니다.
2. `Action` 타입의 `OnAlarm` 이벤트를 선언합니다.
3. `Main`에서 `AlarmClock`의 이벤트에 "기상!", "불 켜기", "음악 틀기" 기능을 구독(`+=`)시킵니다.
4. 시계의 특정 메소드를 호출했을 때 등록된 모든 기능이 실행되는지 확인하세요.

---

## 4. 12일차 심화 미션: "이벤트 기반 보상 시스템"

**[미션 목표]**
몬스터 클래스에 '사망 이벤트'를 추가하여, 몬스터가 죽었을 때 플레이어가 직접 확인하지 않아도 보상 지급, 로그 기록, UI 업데이트가 자동으로 이루어지는 시스템을 설계합니다.

---

### 1) 요구 사항

#### 1. 몬스터 이벤트 선언
* `Monster` 클래스에 `public event Action OnDead;` 이벤트를 추가합니다.
* `Hp`가 0 이하가 되는 순간 `OnDead?.Invoke();`를 호출하여 사망 소식을 알립니다.

#### 2. 구독 시스템 구축
* **RewardSystem**: 몬스터 사망 시 인벤토리에 아이템을 추가하는 메서드를 이벤트에 등록합니다.
* **CombatLogger**: 몬스터 사망 시 "XXX가 처치되었습니다!"라는 로그를 `Stack`에 쌓는 메서드를 이벤트에 등록합니다.
* **QuestTracker**: 처치한 몬스터의 숫자를 올리는 메서드를 이벤트에 등록합니다.

#### 3. 다이나믹 상호작용
* 플레이어가 몬스터를 공격하여 죽게 만들었을 때, 위 3가지 시스템이 플레이어의 직접적인 코드 호출 없이 실행되는지 확인하세요.

---

### 2) 프로그래밍 힌트
* `OnClick += MethodName;` 형식을 사용하여 객체 생성 직후에 이벤트를 구독시킵니다.
* 람다식(`() => { ... }`)을 사용하면 별도의 메서드 정의 없이 간결하게 이벤트를 구독할 수 있습니다.
* 유니티에서는 이 방식(Observer Pattern)을 통해 코드 간의 결합도를 낮추고 유지보수를 편하게 합니다.

---
## ✍️ 12일차 핵심 퀴즈
1. 델리게이트와 이벤트의 가장 큰 차이점(안전성 관련)은 무엇인가요?
2. 이벤트에 기능을 추가할 때 사용하는 기호(`+=`)와 제거할 때 사용하는 기호는 무엇인가요?
