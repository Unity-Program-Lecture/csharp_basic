# 🚀 Day 13: 현대적 프로그래밍 (Lambda & LINQ)

오늘의 목표는 "**이름 없는 함수(람다)와 데이터를 요리조리 골라내는 기술(LINQ)을 배워 코드를 획기적으로 줄여본다**"입니다.

---

## 1. 람다식 (Lambda): "이름 없는 일꾼"
즉석에서 만들어 쓰는 짧은 함수입니다. `(입력) => { 내용 }`

---

## 2. LINQ (링크): "데이터 필터링의 마법"
`Where`, `OrderBy`, `Select` 등을 사용하여 데이터를 쉽게 가공합니다.

---

## 💻 실습 예제: 강력한 아이템 필터링
```csharp
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Day13_Practice : MonoBehaviour
{
    void Start()
    {
        List<int> numbers = new List<int> { 5, 2, 8, 1, 9, 4, 10 };

        // 짝수만 골라서 큰 순서대로 정렬
        var result = numbers.Where(n => n % 2 == 0)
                            .OrderByDescending(n => n)
                            .ToList();

        Debug.Log("결과: " + string.Join(", ", result));
    }
}
```

---

## ✍️ 핵심 퀴즈
1. 람다식에서 사용하는 `=>` 기호의 이름은?
2. LINQ에서 특정 조건으로 데이터를 걸러낼 때 사용하는 메소드 이름은?
3. LINQ를 사용하여 데이터를 오름차순으로 정렬하는 메소드는?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 12)]
**LINQ**와 **람다식**을 사용하여 복잡한 몬스터 목록에서 원하는 데이터만 쏙쏙 뽑아내는 기능을 구현합니다.

**[요구 사항]**
1. `List<Monster> allMonsters`에 10마리 이상의 다양한 몬스터를 담으세요.
2. **LINQ 필터링:** 
   - 체력(HP)이 100 미만인 몬스터만 골라내어 목록을 만듭니다.
   - 보스(`MonsterRank.Boss`) 몬스터가 한 마리라도 있는지 `Any()`로 확인합니다.
3. **LINQ 정렬:** 
   - 체력이 높은 순서(`OrderByDescending`)로 몬스터를 정렬하여 이름을 출력합니다.
4. **람다 활용:** 몬스터 리스트의 `ForEach` 메소드와 람다식을 사용하여 모든 몬스터에게 일괄적으로 데미지를 입히는 한 줄 코드를 작성해 보세요.

**[프로그래밍 힌트]**
- `using System.Linq;`가 코드 상단에 있어야 합니다.
- `var result = allMonsters.Where(m => m.HP > 0).ToList();` 처럼 `ToList()`를 붙여야 다시 리스트가 됩니다.
- 람다식은 `(인자) => 식`의 형태로 간결하게 작성하세요.

