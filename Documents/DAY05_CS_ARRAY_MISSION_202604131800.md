# 🚀 5일차: 데이터 묶음과 랜덤 (배열과 실전 미션)

오늘의 목표는 **"많은 데이터를 한 번에 관리하는 법(배열)을 익히고, 1주차에 배운 모든 내용을 쏟아부어 작은 게임 로직을 완성한다"**입니다.

---

## 1. 배열(Array): "변수 아파트"
변수 하나가 단독 주택이라면, 배열은 같은 종류의 변수들이 모여 사는 **'아파트'**입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **인덱스 (Index)**: 아파트의 **'호수'**입니다. C#에서는 **0번**부터 시작한다는 점을 꼭 기억하세요!
- **`[]` (대괄호)**: 배열을 만들거나 특정 호수에 접근할 때 사용합니다.
- **`Length` (길이)**: 배열에 총 몇 개의 칸이 있는지 알려줍니다.

### 💻 실습 예제: 학생 성적 관리하기
**미션:** 정수형 배열을 선언하고 인덱스를 사용하여 값을 할당한 뒤, for문을 통해 배열의 모든 요소를 순차적으로 출력해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. 3칸짜리 정수 배열 만들기
            int[] scores = new int[3];

            // 2. 각 칸에 값 넣기
            scores[0] = 90;
            scores[1] = 85;
            scores[2] = 100;

            // 3. for문을 이용해 모든 성적 출력하기
            Console.WriteLine("--- 성적 리스트 ---");
            for (int i = 0; i < scores.Length; i++)
            {
                Console.WriteLine("{0}번 학생 점수: {1}", i + 1, scores[i]);
            }
        }
    }
}
```

</details>

---

## 2. foreach문: "하나씩 꺼내보기"
배열 안에 있는 모든 내용을 순서대로 꺼낼 때 쓰는 가장 쉽고 안전한 반복문입니다.

### 💻 실습 예제: 인벤토리 아이템 훑어보기
**미션:** string형 배열에 아이템 이름을 저장하고, foreach문을 사용하여 인벤토리의 모든 항목을 간결하게 출력해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] inventory = { "낡은 검", "빨간 포션", "나무 방패", "마법서" };

            Console.WriteLine("[ 인벤토리 목록 ]");
            foreach (string item in inventory) // inventory 안의 아이템을 하나씩 꺼내 item에 담음
            {
                Console.WriteLine("- {0}", item);
            }
        }
    }
}
```

</details>

---

## 3. Random 클래스: "운명의 주사위"
게임에서 빠질 수 없는 요소인 '무작위(Random)' 값을 만드는 방법입니다.

### 💻 실습 예제: 주사위 굴리기
**미션:** Random 클래스를 활용하여 1부터 6 사이의 무작위 숫자를 생성하고, 주사위 던지기 결과를 시뮬레이션해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random(); // 랜덤 기계 소환

            Console.WriteLine("주사위를 굴립니다...");
            int diceValue = random.Next(1, 7); // 1부터 6까지(7 미만) 랜덤 생성
            
            Console.WriteLine("결과: {0}", diceValue);
        }
    }
}
```

</details>

---

## 4. ⚔️ 1주차 최종 미션: "몬스터 대격돌"
지금까지 배운 **변수, 연산자, 제어문, 클래스, 상속, 배열, 랜덤**을 모두 활용해보세요!

### **[미션 조건]**
1. `Monster` 부모 클래스를 만듭니다. (이름, HP, 공격력 변수 포함)
2. `Slime`, `Orc` 클래스가 `Monster`를 상속받게 합니다.
3. `Monster[] monsters = new Monster[2];` 배열에 슬라임과 오크를 한 마리씩 넣습니다.
4. `Random`을 사용하여 몬스터가 플레이어를 공격하게 하거나, 플레이어가 몬스터를 공격하는 로직을 짭니다.
5. 모든 몬스터의 HP가 0이 되면 "승리!"를 출력하고 게임을 종료합니다.

---

### **[정답 예시 가이드]**
**미션:** 배열과 foreach문을 조합하여 여러 객체의 상태 정보를 한꺼번에 조회하고 출력하는 기능을 구현해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
// Hint: 배열과 foreach를 활용해 몬스터 상태를 한 번에 보여주세요!
foreach (Monster m in monsters)
{
    Console.WriteLine("{0}의 남은 HP: {1}", m.name, m.hp);
}
```

</details>

---

## ✍️ 5일차 핵심 퀴즈
1. `int[] numbers = new int[5];` 배열에서 마지막 칸의 인덱스 번호는 몇 번인가요?
2. `Random.Next(1, 10)`을 실행하면 10이라는 숫자가 나올 수 있나요?
