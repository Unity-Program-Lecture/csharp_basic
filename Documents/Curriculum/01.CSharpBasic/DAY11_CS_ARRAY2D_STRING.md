# 🚀 Day 11: 실무 데이터 처리 (2D Array & StringBuilder)

오늘의 목표는 "**바둑판 모양의 2차원 공간을 다루는 법과 대량의 문자열을 효율적으로 처리하는 법을 마스터한다**"입니다.

---

## 1. 2차원 배열: "평면 공간 데이터"
행과 열을 가진 바둑판 형태의 배열입니다. `map[y, x]` 순서로 접근합니다.

```csharp
int[,] map = new int[5, 5];
map[0, 0] = 1; // 0행 0열에 1 대입

// 선언과 동시에 초기화
int[,] grid = {
    { 1, 2 },
    { 3, 4 }
};
```

---

## 2. StringBuilder: "문자열 건설 현장"
문자열을 아주 많이 합쳐야 할 때 메모리 낭비를 줄이기 위해 사용합니다.

```csharp
using System.Text;

StringBuilder sb = new StringBuilder();
sb.Append("Hello ");
sb.AppendLine("World!");
sb.AppendFormat("Level: {0}", 10);

string result = sb.ToString();
```

---

## 💻 실습 예제: 미로 맵 정보 생성
```csharp
using UnityEngine;
using System.Text;

public class Day11_Practice : MonoBehaviour
{
    void Start()
    {
        int[,] map = {
            { 1, 1, 1 },
            { 1, 0, 1 },
            { 1, 1, 1 }
        };

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== 맵 정보 ===");

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                string tile = (map[y, x] == 1) ? "■" : "□";
                sb.Append(tile + " ");
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }
}
```

---

## ✍️ 핵심 퀴즈
1. `int[,] arr = new int[3, 4];`에서 전체 칸수는 몇 개인가요?
2. 문자열을 수천 번 합쳐야 할 때 성능을 위해 사용하는 클래스는?
3. 문자열 앞뒤의 공백을 제거해주는 메소드의 이름은?

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 10)]
**2차원 배열**로 월드 맵을 구성하고, **StringBuilder**를 사용하여 전체 전투 상황을 한 번에 리포트합니다.

**[요구 사항]**
1. **월드 맵:** `Monster[,] worldMap = new Monster[5, 5];`를 만듭니다.
   - 특정 좌표에 몬스터 객체를 배치하세요. (예: `worldMap[2, 3] = new BossMonster();`)
2. **전체 현황 리포트 (StringBuilder):**
   - 현재 맵에 배치된 모든 몬스터의 이름, HP, 등급 정보를 하나의 큰 문자열로 합칩니다.
   - 루프를 다 돌고 나서 `Debug.Log(sb.ToString());` 한 번만 호출하여 출력하세요.
3. **문자열 처리:** 사용자로부터 입력받은 몬스터 이름의 앞뒤 공백을 제거(`Trim`)하고, 대소문자 구분 없이(`ToLower`) 검색하여 해당 몬스터의 정보를 보여주는 기능을 만드세요.

**[프로그래밍 힌트]**
- `sb.AppendFormat()`을 사용하면 더 멋지게 문자열을 조립할 수 있습니다.
- 2차원 배열의 빈 공간은 `null`이므로, 접근 전 반드시 `if (worldMap[y, x] != null)` 체크를 해야 합니다.

