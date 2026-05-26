# 🚀 13일차: 데이터 요리사 되기! (람다와 LINQ)

오늘의 목표는 **"이름 없는 메소드(람다)로 코드를 줄이고, 방대한 데이터에서 내가 원하는 것만 쏙쏙 골라내는 법(LINQ)을 배운다"**입니다.

---

## 1. 람다식(Lambda Expression): "한 줄로 끝내는 메소드"
메소드를 따로 만들지 않고, 그 자리에서 바로 실행하는 **'익명 함수'**입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`=>` (람다 연산자)**: "~으로 간다(goes to)"는 뜻입니다. 왼쪽은 **재료(입력)**, 오른쪽은 **행동(코드)**입니다.
- **익명 함수**: 이름이 없는 메소드입니다. 한 번만 쓰고 버릴 간단한 기능을 만들 때 최고입니다.

### 💻 실습 예제: 화살표 함수 연습
**미션:** 람다식을 사용하여 리스트에서 짝수만 출력하거나, 두 숫자를 더하는 한 줄 코드를 작성해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6 };

            // 1. 기존 방식: 메소드 따로 만들기 (번거로움)
            // 2. 람다 방식: 그 자리에서 바로!
            var evens = numbers.Where(n => n % 2 == 0); // n이 2로 나누어떨어지면(true) 골라라!

            Console.WriteLine("짝수 목록:");
            foreach (var n in evens)
            {
                Console.WriteLine(n);
            }
        }
    }
}
```

</details>

---

## 2. LINQ(링크): "데이터 전용 언어"
리스트나 배열 같은 데이터 모음집에서 원하는 데이터만 뽑아내거나 정렬하는 **'마법의 주문'**입니다.

### 💡 이 단어는 무슨 뜻인가요?
- **`Where`**: 조건을 걸어 **필터링**합니다. (예: "체력이 0보다 큰 놈만!")
- **`OrderBy`**: 순서대로 **정렬**합니다. (예: "레벨 높은 순으로!")
- **`Select`**: 데이터 중 **특정 정보만** 추출합니다. (예: "몬스터 이름만 줘!")

### 💻 실습 예제: 게임 랭킹 정렬
**미션:** LINQ를 사용하여 점수가 높은 순서대로 플레이어 이름을 출력해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day13
{
    class Player { public string Name; public int Score; }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Player> players = new List<Player>
            {
                new Player { Name = "아이언맨", Score = 100 },
                new Player { Name = "헐크", Score = 500 },
                new Player { Name = "스파이더맨", Score = 300 }
            };

            // LINQ로 고득점자순 정렬하기
            var sorted = players.OrderByDescending(p => p.Score);

            foreach (var p in sorted)
            {
                Console.WriteLine("{0}: {1}점", p.Name, p.Score);
            }
        }
    }
}
```

</details>

---

## 3. 13일차 미션: "단어 필터링 시스템"
다음 조건에 맞는 프로그램을 만들어보세요.

1. `string` 타입의 리스트에 과일 이름 5개를 넣습니다. (예: "사과", "바나나", "파인애플" 등)
2. LINQ의 `Where`를 사용하여 이름이 3글자 이상인 과일만 골라내세요.
3. LINQ의 `OrderBy`를 사용하여 가나다순으로 정렬한 뒤 출력하세요.

---

## 4. 13일차 심화 미션: "정예 몬스터 검색 시스템"

**[미션 목표]**
수많은 몬스터가 담긴 리스트에서 LINQ와 람다식을 사용하여 특정 조건(최고 체력, 보스급 등)을 만족하는 대상을 효율적으로 찾아내는 기능을 구현합니다.

---

### 1) 요구 사항

#### 1. 대규모 몬스터 생성
* `List<Monster>`에 슬라임, 오크, 드래곤 등 10마리 이상의 몬스터를 `Random`하게 생성하여 담습니다.

#### 2. 조건별 데이터 추출 (`LINQ`)
* **정예 몬스터**: 현재 체력이 100 이상인 몬스터만 골라 리스트로 만듭니다.
* **보스 몬스터**: 이름에 "드래곤"이 포함된 몬스터 중 가장 체력이 높은 한 마리를 찾습니다. (`FirstOrDefalut` 사용)
* **사망 목록**: 체력이 0이 되어 리스트에서 제거되어야 할 몬스터들의 '이름'만 따로 추출합니다.

#### 3. 통계 계산
* 모든 몬스터의 평균 체력과 가장 낮은 체력을 LINQ의 `Average()`, `Min()` 메서드로 계산하여 출력합니다.

---

### 2) 프로그래밍 힌트
* `monsterList.Where(m => m.Hp > 100).ToList()`와 같은 한 줄 코드로 필터링이 가능합니다.
* 특정 이름을 포함하는지 확인할 때는 `m.Name.Contains("드래곤")`을 조건식에 활용하세요.
* `using System.Linq;`가 선언되어 있지 않으면 이 마법 같은 기능들을 사용할 수 없으니 주의하세요!

---
## ✍️ 13일차 핵심 퀴즈
1. LINQ를 사용하여 데이터를 내림차순(큰 순서대로) 정렬할 때 쓰는 메서드 이름은 무엇인가요?
2. 람다식 `(n) => n * 2`에서 `n`은 무엇을 의미하나요?
