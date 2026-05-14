# 🚀 Day 11: 게임 자료구조 기초 (데이터 검색)

오늘의 목표는 **"자료구조의 개념을 이해하고, 특정 키(Key)를 사용하여 빠르게 값(Value)을 찾아야 하는 상황에 적합한 자료구조를 게임에 적용해 본다"**입니다.

---

## 1. 💡 이론 (30%): 자료구조와 검색 효율성
- **자료구조 (Data Structure)**: 데이터를 메모리에 효율적으로 저장하고 관리하는 방법입니다.
- **List vs Dictionary**:
  - `List`: 순서대로 데이터를 쌓을 때 유리하지만, 특정 데이터를 찾으려면 처음부터 끝까지 뒤져야 합니다. (순차 탐색)
  - `Dictionary`: 이름표(Key)를 붙여 데이터를 저장합니다. 아이템 이름으로 아이템 정보를 찾을 때 검색 속도가 월등히 빠릅니다. (해시 테이블 기반)

---

## 2. 💻 실습 (70%): 아이템 도감 시스템
**미션:** C#의 자료구조를 사용하여, 아이템 이름(Key)을 입력하면 해당 아이템의 설명(Value)을 즉시 찾아 출력하는 아이템 도감을 만드세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using System.Collections.Generic; // 제네릭 자료구조를 위해 필수!

public class ItemEncyclopedia : MonoBehaviour
{
    // Key(문자열: 이름)와 Value(문자열: 설명)를 매칭하는 딕셔너리 선언
    private Dictionary<string, string> itemDB = new Dictionary<string, string>();

    void Start()
    {
        // 1. 자료 저장
        itemDB.Add("빨간포션", "체력을 50 회복시켜 줍니다.");
        itemDB.Add("강철검", "공격력이 10 증가합니다.");

        // 2. 자료 검색
        SearchItem("강철검");
        SearchItem("없는아이템");
    }

    void SearchItem(string itemName)
    {
        // Key가 존재하는지 확인 후 Value 가져오기
        if (itemDB.ContainsKey(itemName))
        {
            Debug.Log($"{itemName} 설명: {itemDB[itemName]}");
        }
        else
        {
            Debug.Log("해당 아이템을 찾을 수 없습니다.");
        }
    }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 자료를 저장하는 중에 Key를 사용하여 Value를 찾아야 하는 경우가 있습니다. 이럴 때 사용할 수 있는 C#의 자료구조를 써주세요.
   - **정답:** Dictionary (딕셔너리)
2. **문제:** 게임 내 수많은 아이템 중에서 '특정 이름의 아이템'을 찾을 때 List보다 Dictionary를 사용하는 것이 좋은 이유는 무엇입니까?
   - **정답:** List는 처음부터 끝까지 검색해야 하지만, Dictionary는 Key를 통해 즉시 데이터를 찾아낼 수 있어 검색 속도가 훨씬 빠르기 때문입니다.
