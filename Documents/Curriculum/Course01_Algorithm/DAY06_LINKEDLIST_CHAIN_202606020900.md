# 🚀 [알고리즘 06] 유연한 연결: 연결 리스트(LinkedList) 체인

학습 목표: 배열의 한계를 넘어서는 '연결 리스트'의 유연함을 배우고, 게임 속 버프/디버프(상태 이상) 효과를 관리하는 체인 시스템을 구현해 봅니다.

---

## 💡 개념 설명 (NCS 알고리즘: 동적 자료구조 이해)

### 1. 연결 리스트(LinkedList)란 무엇인가요?
연결 리스트는 '기차 칸'과 같습니다. 각 기차 칸(노드)은 데이터와 함께 '다음 칸은 누구인가?'에 대한 정보를 가집니다.

- **일상 비유**: 보물 찾기 지도. "현재 장소의 보물을 찾고, 다음 장소가 적힌 쪽지를 확인하라."
- **배열과의 차이점**:
  - **배열(Array)**: "번호표가 있는 의자들". 중간에 누가 끼어들려면 모든 사람이 옆으로 한 칸씩 이동해야 함. (매우 힘듦!)
  - **연결 리스트**: "손을 맞잡은 사람들". 중간에 누군가 끼어들려면 앞사람과 뒷사람의 손만 다시 잡으면 됨. (매우 빠름!)

### 2. 게임 속 '상태 이상(Buff)'과 연결 리스트
캐릭터가 독에 걸리고, 공격력이 강화되고, 속도가 느려지는 등 여러 효과가 동시에 적용될 때 이를 효과적으로 관리하려면 연결 리스트가 유리합니다.

- **효과 추가/삭제**: 버프 지속 시간이 끝나서 사라질 때, 다른 버프들의 위치를 옮길 필요 없이 연결만 끊어주면 됩니다.
- **실시간 변화**: 게임 도중 버프가 수십 개씩 생겼다 사라졌다 하는 액션 게임에서 메모리를 효율적으로 사용하게 해줍니다.

---

## 💻 실습 예제

**미션:** 유니티에서 `LinkedList<T>`를 사용하여 캐릭터의 '상태 이상(Buff)' 시스템을 만드세요. 새로운 버프가 추가되고, 특정 시간이 지나면 리스트에서 제거되는 알고리즘을 구현합니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour
{
    // 1. 버프 정보를 담는 클래스
    public class Buff
    {
        public string name;
        public float duration;

        public Buff(string name, float duration)
        {
            this.name = name;
            this.duration = duration;
        }
    }

    // 2. 연결 리스트 선언 (삽입과 삭제가 빈번할 때 유리)
    private LinkedList<Buff> activeBuffs = new LinkedList<Buff>();

    public void AddBuff(string buffName, float time)
    {
        Buff newBuff = new Buff(buffName, time);
        activeBuffs.AddLast(newBuff); // 리스트 끝에 추가
        Debug.Log($"버프 획득: {buffName} ({time}초)");
    }

    void Update()
    {
        // 3. 리스트를 순회하며 시간 체크 (삭제 시 에러 방지를 위해 역순 처리나 노드 기반 삭제 권장)
        var currentNode = activeBuffs.First;
        while (currentNode != null)
        {
            var nextNode = currentNode.Next; // 다음 노드 미리 저장
            currentNode.Value.duration -= Time.deltaTime;

            if (currentNode.Value.duration <= 0)
            {
                Debug.Log($"버프 종료: {currentNode.Value.name}");
                activeBuffs.Remove(currentNode); // O(1)에 가까운 삭제 성능
            }
            currentNode = nextNode;
        }
    }

    void OnGUI()
    {
        // 화면에 현재 적용 중인 버프 표시
        int y = 10;
        foreach (var buff in activeBuffs)
        {
            GUI.Label(new Rect(10, y, 200, 20), $"{buff.name}: {buff.duration:F1}s");
            y += 25;
        }
    }
}
```

</details>

---

## ✍️ 정리 및 퀴즈

1. **질문**: 데이터가 중간에 자주 삽입되거나 삭제되는 상황에서 `List<T>`보다 `LinkedList<T>`가 유리한 이유는 무엇인가요?
2. **질문**: 연결 리스트의 각 칸(Node)이 가져야 할 두 가지 핵심 정보는 무엇인가요?
3. **질문**: 만약 100만 명의 플레이어 순위를 관리해야 한다면, 특정 등수의 사람을 바로 찾아야 할 때 연결 리스트가 좋은 선택일까요? (탐색 성능 측면에서 생각해보세요.)
