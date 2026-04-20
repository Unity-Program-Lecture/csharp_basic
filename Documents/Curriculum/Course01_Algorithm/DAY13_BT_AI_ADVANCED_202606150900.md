# 🚀 DAY13: Behavior Tree(BT) - 복합 AI 설계
_최종 수정일: 202606150900_

## 🚀 학습 목표
- FSM의 한계를 이해하고 Behavior Tree(BT)의 구조를 파악합니다.
- 복합 노드(Sequence, Selector)와 리프 노드(Action, Condition)를 활용하여 정교한 AI를 설계합니다.

---

## 💡 개념 설명: FSM vs Behavior Tree
- **FSM(상태 머신)은 '기분' 중심:** "지금은 공격 중이야", "지금은 도망 중이야". 상태가 많아질수록 선(Transition)이 꼬여서 '스파게티'가 되기 쉽습니다.
- **BT(행동 트리)는 '결정' 중심:** 상향식(Bottom-up)으로 판단을 쌓아 올립니다. "공격할 수 있나? -> 예 -> 공격", "아니오 -> 도망". 계층 구조 덕분에 확장성이 매우 뛰어납니다.

### 주요 노드 비유
1. **Selector (OR 상자):** "하나만 성공하면 끝!" (배고프면 밥 먹기, 아니면 잠자기)
2. **Sequence (AND 상자):** "모두 성공해야 끝!" (문 열기 -> 방에 들어가기 -> 불 켜기)
3. **Action/Condition:** 실제로 하는 행동이나 체크하는 조건.

---

## 💻 실습 예제: 복합 AI 행동 로직

**미션:** 몬스터가 '플레이어 감지 -> 거리 체크 -> 공격' 순서로 행동하고, 플레이어가 없으면 '순찰'을 수행하는 트리를 구성하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using System.Collections.Generic;

// 기초 노드 클래스
public abstract class Node
{
    public abstract bool Execute();
}

// Selector: 자식 중 하나라도 true면 성공
public class Selector : Node
{
    private List<Node> children;
    public Selector(List<Node> nodes) => children = nodes;

    public override bool Execute()
    {
        foreach (var node in children)
        {
            if (node.Execute()) return true;
        }
        return false;
    }
}

// Sequence: 모든 자식이 true여야 성공
public class Sequence : Node
{
    private List<Node> children;
    public Sequence(List<Node> nodes) => children = nodes;

    public override bool Execute()
    {
        foreach (var node in children)
        {
            if (!node.Execute()) return false;
        }
        return true;
    }
}

// 실무 적용 예시 (간략화)
public class MonsterAI : MonoBehaviour
{
    Node rootNode;

    void Start()
    {
        // 트리 구성: (플레이어 발견 AND 공격) OR 순찰
        var attackSequence = new Sequence(new List<Node> {
            new ConditionNode(() => IsPlayerInSight()),
            new ActionNode(() => AttackPlayer())
        });

        rootNode = new Selector(new List<Node> {
            attackSequence,
            new ActionNode(() => Patrol())
        });
    }

    void Update() => rootNode.Execute();

    bool IsPlayerInSight() { /* 로직 */ return true; }
    bool AttackPlayer() { Debug.Log("공격!"); return true; }
    bool Patrol() { Debug.Log("순찰 중..."); return true; }
}

public class ActionNode : Node { /* 구현 생략 */ public ActionNode(System.Func<bool> action) {} public override bool Execute() => true; }
public class ConditionNode : Node { /* 구현 생략 */ public ConditionNode(System.Func<bool> cond) {} public override bool Execute() => true; }
```

</details>

---

## ✍️ 복합 퀴즈
1. Selector 노드에서 첫 번째 자식이 성공(Success)을 반환하면 두 번째 자식은 실행될까요?
2. FSM에 비해 BT가 가지는 가장 큰 설계상 이점은 무엇인가요?
