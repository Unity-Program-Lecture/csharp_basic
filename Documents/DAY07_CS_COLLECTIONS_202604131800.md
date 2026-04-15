# 🚀 7일차: 변하는 바구니 (List와 Dictionary)

오늘의 목표는 **"개수가 정해지지 않은 데이터를 자유롭게 넣고 빼는 법(List)과, 이름으로 데이터를 찾는 법(Dictionary)을 배운다"**입니다.

---

## 1. List<T>: "늘어나는 배열"
배열은 처음에 정한 칸수(예: 3칸)를 바꿀 수 없지만, List는 데이터를 넣는 대로 **자동으로 칸이 늘어납니다.**

### 💡 이 단어는 무슨 뜻인가요?
- **`<T>` (제네릭)**: "어떤 타입(Type)을 담을 것인가"를 정하는 괄호입니다. (예: `List<int>`는 정수 바구니)
- **`Add()`**: 바구니에 데이터를 새로 추가합니다.
- **`Remove()` / `RemoveAt()`**: 데이터를 삭제합니다.
- **`Count`**: 배열의 `Length`처럼, 지금 바구니에 몇 개가 들어있는지 알려줍니다.

### 💻 실습 예제: 게임 서버 접속자 목록
**미션:** List<T>를 사용하여 가변적인 데이터 목록을 관리하고, 요소를 추가 및 삭제하며 현재 상태를 확인하는 코드를 작성해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;
using System.Collections.Generic; // List를 쓰기 위해 꼭 필요합니다!

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. 문자열을 담는 리스트 만들기
            List<string> users = new List<string>();

            // 2. 유저 추가
            users.Add("IronMan");
            users.Add("SpiderMan");
            users.Add("Hulk");

            // 3. 유저 한 명 삭제
            users.Remove("Hulk");

            // 4. 전체 유저 출력
            Console.WriteLine("--- 현재 접속 유저 ({0}명) ---", users.Count);
            foreach (string user in users)
            {
                Console.WriteLine("- {0}", user);
            }
        }
    }
}
```

</details>

---

## 2. Dictionary<K, V>: "데이터 사전"
번호(0, 1, 2...)가 아닌 **이름(Key)**으로 **내용(Value)**을 찾는 바구니입니다. 마치 영어 사전에서 단어를 찾으면 뜻이 나오는 것과 같습니다.

### 💡 이 단어는 무슨 뜻인가요?
- **Key (키)**: 데이터를 찾기 위한 **'열쇠'**입니다. (중복될 수 없습니다!)
- **Value (밸류)**: 열쇠를 열면 나오는 **'진짜 데이터'**입니다.

### 💻 실습 예제: 몬스터 정보 사전
**미션:** Dictionary<K, V>를 활용하여 키(Key)와 값(Value) 쌍으로 데이터를 저장하고, 특정 키를 통해 데이터를 빠르게 검색하는 방법을 익혀 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;
using System.Collections.Generic;

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 키는 string(이름), 값은 int(체력)인 딕셔너리 만들기
            Dictionary<string, int> monsterHPs = new Dictionary<string, int>();

            // 데이터 저장
            monsterHPs["슬라임"] = 30;
            monsterHPs["오크"] = 150;
            monsterHPs["드래곤"] = 5000;

            // 데이터 찾기
            string searchName = "오크";
            if (monsterHPs.ContainsKey(searchName)) // 사전에 이름이 있는지 확인
            {
                Console.WriteLine("{0}의 체력은 {1}입니다.", searchName, monsterHPs[searchName]);
            }
        }
    }
}
```

</details>

---

## 3. 왜 컬렉션을 쓰나요?
1. **유연성**: 게임에서 아이템을 획득하거나 버릴 때, 몬스터가 생성되거나 죽을 때처럼 **개수가 계속 변할 때** 필수적입니다.
2. **속도**: Dictionary를 쓰면 수만 개의 데이터 중에서도 내가 원하는 것을 **눈 깜짝할 새(Key)**에 찾을 수 있습니다.

---

## 4. 7일차 미션: "학생부 관리 프로그램"
다음 기능을 가진 프로그램을 만들어보세요.

1. `students`라는 이름의 `Dictionary<string, int>`를 만듭니다. (Key: 학생 이름, Value: 점수)
2. 사용자로부터 학생 이름과 점수를 입력받아 딕셔너리에 저장합니다. (3번 반복)
3. 이후, 특정 학생의 이름을 입력하면 그 학생의 점수를 출력해주는 검색 기능을 만드세요.
4. 만약 이름이 없으면 "등록되지 않은 학생입니다."를 출력하세요.

---

**Tip**: `using System.Collections.Generic;`을 쓰지 않으면 `List`와 `Dictionary`를 인식하지 못하니 주의하세요!

---

## 5. 7일차 심화 미션: "동적 인벤토리와 스폰 시스템"

**[미션 목표]**
크기가 고정된 배열 대신, 실행 중에 요소를 자유롭게 추가하거나 삭제할 수 있는 `List<T>`와 키-값 쌍으로 데이터를 관리하는 `Dictionary<K, V>`를 활용합니다. 이를 통해 보다 유연한 게임 데이터 관리 시스템을 설계합니다.

---

### 1) 요구 사항

#### 1. 동적 몬스터 관리 (`List<T>`)
* `List<Monster> monsterList = new List<Monster>();`를 생성합니다.
* **Spawn 명령**: 사용자가 "스폰"을 입력하면 랜덤한 몬스터(슬라임, 오크 등)가 리스트에 추가됩니다.
* **자동 제거**: 전투 중 체력이 0이 된 몬스터는 리스트에서 즉시 제거(`Remove`)되도록 설계합니다.

#### 2. 아이템 가방 구현 (`Dictionary<K, V>`)
* `Dictionary<string, int> inventory = new Dictionary<string, int>();`를 사용하여 아이템 이름과 개수를 저장합니다.
* **아이템 획득**: 몬스터 처치 시 특정 아이템(예: "빨간 포션")의 개수를 1 증가시킵니다. 
* 이미 있는 아이템이라면 숫자를 올리고, 처음 얻는 아이템이라면 새로 추가하는 로직을 고민해 보세요.

#### 3. 전체 목록 출력
* 현재 필드에 있는 모든 몬스터의 목록과 인벤토리에 담긴 아이템 및 그 개수를 각각 출력하는 기능을 만듭니다.

---

### 2) 프로그래밍 힌트
* `List`에서 요소를 제거할 때 `for` 문을 정방향으로 돌리며 삭제하면 인덱스 꼬임 현상이 발생할 수 있으니 주의하세요. (역순 루프 권장)
* `Dictionary`에 값이 있는지 확인할 때는 `inventory.ContainsKey("아이템명")` 메서드를 활용하면 안전합니다.
* `foreach (var item in inventory)`를 통해 딕셔너리의 모든 내용을 출력할 수 있습니다. (item.Key, item.Value 활용)


**[심화 과제 (선택 사항)]**
- **정렬 기능**: `monsterList.Sort()`를 응용하여 체력이 낮은 순서대로 몬스터를 정렬해 보세요. (IComparable 인터페이스 학습 권장)
- **아이템 사용**: 인벤토리에서 아이템 이름을 입력하면 개수가 1 줄어들고 효과가 나타나는 로직을 추가해 보세요.

---
## ✍️ 7일차 핵심 퀴즈
1. `List`에서 특정 위치의 데이터를 지울 때 사용하는 메소드는 무엇인가요?
2. `Dictionary`에서 중복된 'Key'를 가질 수 있나요?
