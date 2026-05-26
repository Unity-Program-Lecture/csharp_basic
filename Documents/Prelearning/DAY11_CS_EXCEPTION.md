# 🚀 11일차: 에러가 나도 당황하지 말자! (예외 처리)

오늘의 목표는 **"프로그램이 예상치 못한 상황에서 갑자기 꺼지는 것을 막고, 안전하게 에러를 처리하는 방법을 배운다"**입니다.

---

## 1. 예외(Exception): "예상치 못한 사고"
문법이 틀린 것은 **'에러(Error)'**라고 하지만, 문법은 맞는데 실행 중에 발생하는 사고를 **'예외(Exception)'**라고 합니다. (예: 숫자를 0으로 나누기, 없는 파일 열기)

### 💡 이 단어는 무슨 뜻인가요?
- **`try`**: "일단 이 코드를 **시도**해봐!"
- **`catch`**: "만약 사고(예외)가 나면 여기서 **잡아줘!**"
- **`finally`**: "사고가 나든 안 나든, 마지막엔 **무조건 실행해!**" (주로 뒷정리용)

### 💻 실습 예제: 0으로 나누기 방어 작전
**미션:** try-catch-finally를 이용해 0으로 나누기 오류 및 형식 오류(문자 입력 등)를 안전하게 처리합니다.
<details><summary>코드 보기</summary>

```csharp
using System;

namespace Day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("나눌 숫자를 입력하세요: ");
                int num = int.Parse(Console.ReadLine());
                
                int result = 100 / num; // 만약 0을 입력하면 사고 발생!
                Console.WriteLine("결과: " + result);
            }
            catch (DivideByZeroException) // 0으로 나눌 때 발생하는 예외만 잡음
            {
                Console.WriteLine("[에러] 0으로는 나눌 수 없습니다!");
            }
            catch (FormatException) // 숫자가 아닌 글자를 입력했을 때
            {
                Console.WriteLine("[에러] 숫자만 입력해주세요!");
            }
            catch (Exception e) // 그 외 모든 예외를 다 잡는 대장님
            {
                Console.WriteLine("[알 수 없는 에러] " + e.Message);
            }
            finally
            {
                Console.WriteLine("프로그램을 안전하게 종료합니다.");
            }
        }
    }
}
```

</details>

---

## 2. throw: "내가 에러를 던지다"
어떤 상황이 발생했을 때, 컴퓨터가 사고로 인식하게끔 **직접 예외를 발생**시키는 기능입니다.

### 💻 실습 예제: 나이 입력 검사기
**미션:** throw를 이용해 특정 조건(나이 미달 등)에서 의도적으로 예외를 발생시키고 이를 처리하는 로직을 구현합니다.
<details><summary>코드 보기</summary>

```csharp
using System;

namespace Day11
{
    internal class Program
    {
        static void CheckAge(int age)
        {
            if (age < 0)
            {
                // 직접 에러를 만들어서 던집니다!
                throw new Exception("나이는 0보다 작을 수 없습니다.");
            }
            Console.WriteLine("나이가 확인되었습니다: " + age);
        }

        static void Main(string[] args)
        {
            try
            {
                CheckAge(-5);
            }
            catch (Exception e)
            {
                Console.WriteLine("잡았다! : " + e.Message);
            }
        }
    }
}
```

</details>

---

## 3. 자주 만나는 3대 예외
1. **`NullReferenceException`**: 아무것도 없는(`null`) 리모컨의 버튼을 눌렀을 때 발생합니다. (유니티에서 가장 많이 봅니다!)
2. **`IndexOutOfRangeException`**: 배열의 범위를 벗어난 호수(인덱스)를 불렀을 때 발생합니다.
3. **`FormatException`**: "ABC"를 `int.Parse` 하려고 할 때 발생합니다.

---

## 4. 11일차 심화 미션: "안전한 게임 환경 구축"

**[미션 목표]**
`try-catch` 블록과 예외 던지기(`throw`)를 사용하여 프로그램의 비정상 종료를 방지하고, 예상치 못한 상황(잘못된 입력, 데이터 부재 등)을 우아하게 처리하는 방어적 프로그래밍 기법을 학습합니다.

---

### 1) 요구 사항

#### 1. 입력 예외 처리 (`try-catch`)
* **숫자 입력**: 공격 타겟 번호를 입력받을 때, 문자열을 입력하여 발생하는 `FormatException`을 처리합니다. 잘못 입력하면 "숫자만 입력해 주세요!"라고 안내하고 다시 입력받습니다.
* **범위 확인**: 리스트에 없는 인덱스 번호를 입력했을 때 발생하는 `ArgumentOutOfRangeException`을 처리합니다.

#### 2. 논리 예외 던지기 (`throw`)
* **아이템 부족**: 인벤토리에 없는 아이템을 사용하려 할 때, 혹은 개수가 0일 때 `InvalidOperationException`을 던지고 이를 캐치하여 안내 메시지를 띄웁니다.
* **게임 오버**: 플레이어의 체력이 0이 되었을 때 사용자 정의 예외(`GameOverException`)를 던져 게임 루프를 즉시 빠져나가게 설계합니다.

#### 3. 마무리 작업 (`finally`)
* 전투가 중단되거나 종료될 때, `finally` 블록을 사용하여 "전투 시스템을 안전하게 종료합니다."라는 메시지를 항상 출력하도록 합니다.

---

### 2) 프로그래밍 힌트
* `int.Parse` 대신 `int.TryParse`를 쓰는 것이 더 권장되지만, 학습을 위해 `try-catch`로 감싸는 연습을 해보세요.
* `Exception` 클래스를 상속받아 `class GameOverException : Exception`과 같이 나만의 예외 클래스를 만들 수 있습니다.
* 예외 처리는 프로그램의 흐름을 제어하는 용도보다는, '정말 예외적인 상황'을 처리하는 용도로 사용해야 함을 명심하세요.


**[심화 과제 (선택 사항)]**
- **로깅 시스템**: 예외가 발생할 때마다 그 내용을 `Stack` 전투 로그에 기록하여 나중에 확인할 수 있도록 연동해 보세요.
- **다중 Catch**: 하나의 `try` 블록에 여러 종류의 예외(`FormatException`, `DivideByZeroException` 등)를 순서대로 배치하여 각각 다르게 대응하는 로직을 구성해 보세요.

---
## ✍️ 11일차 핵심 퀴즈
1. 예외가 발생하든 발생하지 않든, 무조건 마지막에 실행되어야 하는 코드는 어느 블록에 넣나요?
2. 유니티에서 변수에 오브젝트를 연결하지 않고 사용할 때 발생하는 가장 흔한 에러의 이름은 무엇인가요?

---

**Tip**: 모든 예외를 `catch (Exception e)`로 잡는 것보다, 구체적인 예외(DivideByZero 등)를 먼저 잡는 것이 좋은 습관입니다!
