# 🚀 Day 13: 현대적 프로그래밍 (Lambda & LINQ)

오늘의 목표는 "**이름 없는 함수(람다)와 데이터를 요리조리 골라내는 기술(LINQ)을 배워 코드를 획기적으로 줄여본다**"입니다.

---

## 1. 람다식 (Lambda): "이름 없는 일꾼"
함수를 미리 정의하지 않고, 필요한 순간에 즉석에서 만들어 쓰는 짧은 코드입니다.
- **문법**: `(입력) => { 실행내용 }`
- **비유**: 정식 요리사를 고용하는 대신, 길거리 음식을 사 먹듯 간편하게 기능을 구현하는 것.

---

## 2. LINQ (링크): "데이터 필터링의 마법"
리스트나 딕셔너리에 들어있는 수많은 데이터 중 내가 원하는 것만 골라내거나 정렬하는 기술입니다.
- **주요 메소드**: 
    - `Where`: 조건에 맞는 데이터만 골라내기
    - `OrderBy`: 순서대로 정렬하기
    - `Select`: 데이터에서 필요한 부분만 추출하기

---

## 💻 실습 예제: 강력한 아이템 필터링
**미션:** 리스트에 담긴 숫자들 중 짝수만 골라내어 큰 순서대로 정렬하는 코드를 LINQ로 단 한 줄로 작성해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;
using System.Collections.Generic;
using System.Linq; // LINQ 필수!

class Program
{
    static void Main()
    {
        List<int> numbers = new List<int> { 5, 2, 8, 1, 9, 4, 10 };

        // LINQ 마법: 짝수만 골라서(Where), 큰 순서로 정렬(OrderByDescending)
        var result = numbers.Where(n => n % 2 == 0)
                            .OrderByDescending(n => n)
                            .ToList();

        Console.WriteLine("결과: " + string.Join(", ", result)); 
        // 결과: 10, 8, 4, 2
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. 람다식에서 사용하는 `=>` 기호의 이름은?
2. LINQ에서 특정 조건으로 데이터를 걸러낼 때 사용하는 메소드 이름은?
3. LINQ를 사용하여 데이터를 오름차순(작은 순서)으로 정렬하는 메소드는?
