# 🚀 7일차: 몬스터 사냥꾼의 가방 (List와 Dictionary)

오늘의 목표는 **"개수가 정해지지 않은 데이터를 자유롭게 넣고 빼는 법(List)과, 이름으로 데이터를 찾는 법(Dictionary)을 배운다"**입니다.

---

## 🛠️ 시작 전 준비물
`List`와 `Dictionary`는 C#에서 제공하는 '특별한 도구상자' 안에 들어있습니다. 이 도구들을 꺼내 쓰려면 코드 맨 윗줄에 반드시 다음 한 줄을 적어줘야 합니다.

```csharp
using System.Collections.Generic; // 이 줄이 없으면 List와 Dictionary를 쓸 수 없어요!
```

---

## 1. List<T>: "실시간 몬스터 스폰 목록"
배열은 처음에 3마리로 정하면 더 늘릴 수 없지만, `List`는 몬스터가 소환될 때마다 **자동으로 칸이 늘어납니다.**

### 💡 주요 기능
- **`Add("오크")`**: 새로운 몬스터가 필드에 스폰됩니다.
- **`RemoveAt(0)`**: 특정 번호(인덱스)의 몬스터를 목록에서 제거합니다.
- **`Count`**: 현재 필드에 살아있는 몬스터가 몇 마리인지 알려줍니다.
- **인덱서(`[]`)**: 배열처럼 `fieldMonsters[0]`과 같이 번호를 써서 특정 위치의 몬스터를 지목하거나 바꿀 수 있습니다.

### 🖼️ 그림으로 이해하는 List의 동작
**1) `Add("고블린")` : 맨 뒤에 새 칸을 만들고 추가됩니다.**
```text
[ 슬라임 ] [ 오크 ] + [ 고블린 ]  ➜  [ 슬라임 ] [ 오크 ] [ 고블린 ]
  (0번)     (1번)      (추가!)        (0번)     (1번)     (2번)
```

**2) `RemoveAt(1)` : 중간이 빠지면 뒤의 데이터들이 앞으로 당겨집니다! (중요 ★)**
```text
[0:슬라임] [1:오크] [2:고블린] [3:드래곤]
             ↓ (1번 오크 삭제!)
[0:슬라임] [ 빠짐 ] [2:고블린] [3:드래곤]
             ⬅️ ⬅️ (한 칸씩 당기기)
[0:슬라임] [1:고블린] [2:드래곤]
```
> **주의!**: 1번이 사라지면 2번이었던 고블린이 **새로운 1번**이 됩니다. 순서가 바뀌기 때문에 리스트를 다룰 때는 항상 인덱스 변화에 주의해야 합니다.

### 💻 실습 예제: 필드 몬스터 관리
```csharp
using System;
using System.Collections.Generic;

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. 필드에 있는 몬스터 이름 목록
            List<string> fieldMonsters = new List<string>();

            // 2. 몬스터 스폰 (추가)
            fieldMonsters.Add("슬라임");
            fieldMonsters.Add("오크");
            fieldMonsters.Add("고블린");

            Console.WriteLine("--- [1] 몬스터 스폰 직후 ({0}마리) ---", fieldMonsters.Count);
            foreach (string m in fieldMonsters) 
            {
                Console.WriteLine("- " + m);
            }

            // 3. 인덱서로 접근 및 수정
            Console.WriteLine("\n[소식] 1번 몬스터 오크가 '강한 오크'로 진화했습니다!");
            fieldMonsters[1] = "강한 오크"; 

            Console.WriteLine("--- [2] 진화 후 필드 상황 ({0}마리) ---", fieldMonsters.Count);
            foreach (string m in fieldMonsters)
            {
                Console.WriteLine("- " + m);
            }

            // 4. 몬스터 처치 (삭제)
            Console.WriteLine("\n[전투] 펑! 0번 몬스터 {0}을(를) 처치했습니다.", fieldMonsters[0]);
            fieldMonsters.RemoveAt(0); // 0번(슬라임) 삭제

            // 5. 삭제 후 변화 확인
            Console.WriteLine("\n--- [3] 처치 후 필드 상황 ({0}마리) ---", fieldMonsters.Count);
            foreach (string m in fieldMonsters)
            {
                // 슬라임이 사라지고 '강한 오크'가 0번이 된 것을 확인할 수 있습니다!
                Console.WriteLine("필드에 [ {0} ]이(가) 배회 중입니다.", m);
            }
        }
    }
}
```

---

## 2. Dictionary<K, V>: "전리품 인벤토리"
배열은 반드시 번호(0, 1, 2...)로만 데이터를 찾아야 하지만, 딕셔너리는 **아이템 이름(Key)**을 대면 바로 **개수(Value)**가 나오는 **"똑똑한 배열"**입니다.

### 💡 주요 기능
- **Key(키)**: 데이터를 찾을 기준(열쇠)입니다. (예: 아이템 이름)
- **Value(값)**: 열쇠로 열면 나오는 실제 데이터입니다. (예: 아이템 개수)
- **인덱서(`[]`)**: 딕셔너리의 **'입구'**입니다. 이 괄호 안에 열쇠(Key)를 넣으면 상자 안의 내용물(Value)을 꺼내주거나 새로 넣을 수 있습니다.
- **`inventory["포션"] = 10`**: 포션이라는 이름표가 붙은 칸에 숫자 10을 저장합니다.
- **`ContainsKey("동전")`**: 가방에 해당 아이템이 있는지 먼저 확인하여 에러를 방지합니다.
- **`Remove("녹슨 칼")`**: 더 이상 필요 없는 특정 열쇠와 그 데이터를 가방에서 지웁니다.
- **`foreach (var item in inventory)`**: 가방 안의 모든 아이템(Key)과 개수(Value)를 하나씩 꺼내봅니다.

### 💻 실습 예제: 아이템 획득 시스템
```csharp
using System;
using System.Collections.Generic;

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 키: 아이템 이름, 값: 개수
            Dictionary<string, int> inventory = new Dictionary<string, int>();

            // 1. 아이템 획득 및 수정
            inventory["녹슨 칼"] = 1;
            inventory["동전"] = 50;
            inventory["동전"] = 100; // 이미 있으면 값이 100으로 바뀝니다(수정)

            Console.WriteLine("--- [1] 아이템 획득 직후 ---");
            foreach (var item in inventory)
            {
                Console.WriteLine("- {0} : {1}개", item.Key, item.Value);
            }

            // 2. 아이템 삭제
            Console.WriteLine("\n[가방] 필요 없는 '녹슨 칼'을 버렸습니다.");
            inventory.Remove("녹슨 칼");

            Console.WriteLine("--- [2] 삭제 후 가방 상황 ---");
            foreach (var item in inventory)
            {
                Console.WriteLine("- {0} : {1}개", item.Key, item.Value);
            }

            // 3. 아이템 확인
            Console.WriteLine("\n--- [3] 특정 아이템 검색 ---");
            string searchItem = "동전";
            if (inventory.ContainsKey(searchItem))
            {
                Console.WriteLine("{0}을(를) {1}개 가지고 있습니다.", searchItem, inventory[searchItem]);
            }
        }
    }
}
```

---

## 3. 💡 [필살기] 인벤토리에서 안전하게 아이템 꺼내기
가방에 없는 아이템을 꺼내려고 하면(`inventory["전설의 검"]`) 게임이 멈춰버립니다(에러). 이를 방지하는 가장 안전한 방법이 **`TryGetValue`**입니다.

### TryGetValue: "한 번에 찾아서 꺼내기"
```csharp
int count;
// "포션"이 가방에 있는지 찾아서(Try), 있으면 count에 개수를 담아줘(out)!
if (inventory.TryGetValue("포션", out count)) 
{
    Console.WriteLine("포션을 사용합니다. 남은 개수: " + count);
}
else 
{
    Console.WriteLine("가방에 포션이 없습니다.");
}
```
> **왜 쓰나요?**: `ContainsKey`로 확인하고 `[]`로 또 찾으면 두 번 일하는 셈이지만, `TryGetValue`는 한 번만 일하므로 성능도 좋고 코드도 안전합니다.

---

## 4. 7일차 미션: "진짜 전리품 가방 만들기"
몬스터를 잡을 때마다 아이템이 가방에 쌓이는 로직을 완성해 보세요.

### 🎮 시나리오
당신은 지금 사냥 중입니다. 몬스터를 잡을 때마다 어떤 몬스터를 잡았는지 입력하세요.
- **슬라임**을 잡으면 **"젤리"**를 1개 얻습니다.
- **오크**를 잡으면 **"이빨"**을 1개 얻습니다.
- 이미 가지고 있는 아이템이라면 개수가 늘어나야 하고, 처음 얻는 아이템이라면 새로 가방에 넣어야 합니다.

### 📋 미션 단계
1. 빈 `inventory` 딕셔너리를 만듭니다. (Key: 아이템 이름, Value: 개수)
2. `for` 문을 사용하여 3번 반복하며 "잡은 몬스터(슬라임/오크): "를 입력받습니다.
   - **Tip**: 만약 이전 수업에서 `while` 문을 이용한 전투 로직을 이미 완성했다면, 몬스터가 죽는 시점에 아이템을 획득하도록 기존 코드에 통합해도 좋습니다!
3. 입력받은 이름에 따라 획득할 아이템 이름을 정합니다.
4. **[중요]** 가방에 이미 그 아이템이 있는지 `ContainsKey`로 확인합니다.
   - **있다면**: `inventory[아이템] = inventory[아이템] + 1;` (개수 증가)
   - **없다면**: `inventory[아이템] = 1;` (새로 추가)
5. 마지막에 가방에 담긴 모든 아이템과 개수를 출력하세요.

### ✅ 실행 결과 예시
```text
잡은 몬스터(슬라임/오크): 슬라임
[획득] 젤리를 얻었습니다!
잡은 몬스터(슬라임/오크): 오크
[획득] 이빨을 얻었습니다!
잡은 몬스터(슬라임/오크): 슬라임
[획득] 젤리를 얻었습니다!

--- 최종 인벤토리 ---
- 젤리 : 2개
- 이빨 : 1개
```

---

## ⚠️ [절대 주의] 달리는 기차의 바퀴를 갈지 마세요!
`foreach` 문으로 가방을 열어보고 있는 도중에, 가방 안의 아이템을 **버리거나(Remove) 새로 넣는(Add) 행동**은 절대로 하면 안 됩니다!

- **왜 안 되나요?**: `foreach`는 데이터의 처음부터 끝까지 순서대로 훑고 있는 중입니다. 그런데 중간에 데이터가 사라지거나 늘어나면 순서가 꼬여서 컴퓨터가 당황하고 게임이 멈춰버립니다. (이를 **'달리는 기차의 바퀴를 갈아 끼우려 하는 행동'**이라고 비유합니다.)
- **해결 방법**:
  1. 데이터를 수정(개수 변경 등)하는 것은 괜찮습니다.
  2. 삭제나 추가가 꼭 필요하다면 `foreach` 대신 `for` 문을 사용하거나, 루프가 다 끝난 뒤에 처리해야 합니다.

---

## ✍️ 7일차 핵심 퀴즈
1. `List`에서 데이터를 추가할 때 쓰는 메소드는?
2. `Dictionary`에서 아이템 이름처럼 '찾는 기준'이 되는 데이터를 무엇이라 부르나요?
3. 없는 아이템을 꺼내려 할 때 발생하는 에러를 막기 위한 안전한 메소드는?
4. `foreach` 문이 돌아가는 도중에 `Remove`나 `Add`를 하면 어떻게 되나요?
