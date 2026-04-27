# 🚀 Day 11: 실무 데이터 처리 (2D Array & StringBuilder)

오늘의 목표는 **"바둑판 모양의 2차원 공간을 다루는 법과 대량의 문자열을 효율적으로 처리하는 법을 마스터한다"**입니다.

---

## 1. 2차원 배열: "평면 공간 데이터"
1차원 배열이 줄이라면, 2차원 배열은 칸입니다. 게임 맵이나 바둑판을 만들 때 필수입니다.
- **선언**: `int[,] map = new int[5, 5];` (5x5 크기의 상자)
- **접근**: `map[행, 열]` 또는 `map[y, x]` 순서로 접근합니다.

---

## 2. 문자열의 비밀: "string은 불변(Immutable)이다"
C#에서 `string`은 한 번 만들어지면 내용을 바꿀 수 없습니다. `s = s + "!"`를 할 때마다 컴퓨터는 새로운 문자열을 계속 만들어내어 메모리를 낭비합니다.

### 🏗️ StringBuilder: "문자열 전용 건설 현장"
문자열을 아주 많이 합쳐야 할 때는 `StringBuilder`를 사용하세요. 메모리를 낭비하지 않고 훨씬 빠르게 동작합니다.

---

## 3. 데이터 가공 기술 (Split & Trim)
- **`Split`**: 문자열을 특정 기호(쉼표 등)를 기준으로 쪼개어 배열로 만듭니다. (파일 로드 시 필수!)
- **`Trim`**: 문자열 앞뒤의 불필요한 공백을 제거합니다.

---

## 💻 실습 예제: 미로 맵 출력과 데이터 조립
**미션:** 2차원 배열로 된 맵을 출력하고, `StringBuilder`를 사용해 전체 맵 상태를 하나의 긴 문자열로 만들어 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;
using System.Text; // StringBuilder 필수!

class Program
{
    static void Main()
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
                Console.Write(tile + " ");
                sb.Append(tile);
            }
            sb.AppendLine();
        }

        Console.WriteLine("\n[저장된 로그 출력]");
        Console.WriteLine(sb.ToString());
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. `int[,] arr = new int[3, 4];`에서 전체 칸수는 몇 개인가요?
2. 문자열을 수천 번 합쳐야 할 때 성능을 위해 사용하는 클래스는?
3. "Player,10,100" 이라는 문자열을 쉼표 단위로 쪼개고 싶을 때 사용하는 메소드는?
