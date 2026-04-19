# 🚀 [알고리즘 04] 반복의 마법: 재귀(Recursion)

학습 목표: 자기 자신을 호출하는 '재귀'의 원리를 이해하고, 유니티의 복잡한 오브젝트 계층 구조(Hierarchy)를 탐색하는 알고리즘을 구현해 봅니다.

---

## 💡 개념 설명 (NCS 알고리즘: 재귀 호출)

### 1. 재귀(Recursion)란 무엇인가요?
어떤 문제를 해결하기 위해 **함수가 자기 자신을 다시 호출**하는 것을 말합니다.

- **일상 비유**: "거울 속에 비친 거울 속에 비친 거울..."과 같은 형태입니다.
- **핵심 요소**:
    1. **자기 자신 호출**: 문제를 더 작은 단위로 쪼개어 전달합니다.
    2. **탈출 조건(Base Case)**: 더 이상 자신을 호출하지 않고 멈추는 지점입니다. 이게 없으면 컴퓨터가 멈춰버립니다(Stack Overflow).

### 2. 왜 게임에서 재귀를 쓰나요?
게임은 부모 오브젝트 안에 자식이 있고, 그 자식 안에 또 자식이 있는 '트리(Tree) 구조'로 이루어져 있습니다. 특정 이름을 가진 자식을 찾으려면 몇 단계나 깊이 들어가야 할지 미리 알 수 없기 때문에, 재귀 알고리즘이 매우 효율적입니다.

---

## 💻 실습 예제

**미션:** 현재 오브젝트의 모든 자식들을 깊이(Depth)에 상관없이 끝까지 추적하여, 특정 이름을 가진 오브젝트를 찾는 재귀 함수를 작성하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class HierarchySearcher : MonoBehaviour
{
    void Start()
    {
        string nameToFind = "HiddenTreasure";
        Transform result = FindChildRecursive(this.transform, nameToFind);

        if (result != null)
            Debug.Log($"찾았다! 위치: {result.position}");
        else
            Debug.Log("찾지 못했습니다.");
    }

    // 재귀 알고리즘: 자식의 자식까지 모두 탐색
    Transform FindChildRecursive(Transform parent, string targetName)
    {
        // 1. 현재 확인 중인 대상이 목표인지 확인 (탈출 조건 1)
        if (parent.name == targetName)
            return parent;

        // 2. 모든 자식들에 대해 자기 자신(함수)을 다시 호출
        foreach (Transform child in parent)
        {
            Transform found = FindChildRecursive(child, targetName);
            
            // 3. 자식의 자식에서 찾았다면 결과 반환 (탈출 조건 2)
            if (found != null)
                return found;
        }

        // 4. 아무곳에서도 찾지 못했다면 null 반환
        return null;
    }
}
```

</details>

---

## ✍️ 정리 및 퀴즈

1. **질문**: 재귀 함수에서 '탈출 조건'이 빠지면 어떤 오류가 발생할까요?
2. **질문**: 위 실습 코드에서 `foreach` 문이 하는 역할은 무엇인가요? 왜 자식 오브젝트마다 함수를 다시 호출해야 할까요?
