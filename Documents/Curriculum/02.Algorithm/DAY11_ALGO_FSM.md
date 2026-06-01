# Day 11: 게임 알고리즘 기초 - 유한 상태 머신

오늘의 목표는 "**몬스터가 현재 어떤 상태인지 구분하고, 조건에 따라 상태가 바뀌는 흐름을 Scene 뷰에서 눈으로 확인하는 것**"입니다.

게임 캐릭터는 항상 한 가지 행동만 하고 있는 것처럼 보입니다. 대기 중이거나, 플레이어를 쫓거나, 공격하거나, 쓰러져 있습니다. 이런 행동을 상태로 나누면 몬스터 AI를 훨씬 단순하게 만들 수 있습니다.

## 1. 핵심 개념: "지금 무엇을 하고 있는가?"

FSM은 캐릭터의 행동을 여러 상태로 나누고, 현재는 그중 하나만 실행하게 만드는 방식입니다.

예를 들어 몬스터는 다음처럼 움직일 수 있습니다.

```text
Idle -> Chase -> Attack
```

플레이어가 멀리 있으면 `Idle`, 가까워지면 `Chase`, 공격 거리 안으로 들어오면 `Attack`이 됩니다.

### 이 단어는 무슨 뜻인가요?

#### FSM

`Finite State Machine`의 줄임말입니다. 한국어로는 유한 상태 머신이라고 부릅니다. 정해진 상태 중 하나를 현재 상태로 가지고, 조건에 따라 다른 상태로 전환되는 구조입니다.

#### State

현재 행동 상태입니다. 예를 들어 `Idle`, `Chase`, `Attack` 같은 값입니다.

#### Transition

상태가 다른 상태로 바뀌는 일입니다. 예를 들어 플레이어가 가까워져 `Idle`에서 `Chase`로 바뀌는 것이 전환입니다.

#### Condition

상태 전환을 일으키는 조건입니다. 거리, 체력, 시야, 시간 같은 값이 조건이 될 수 있습니다.

## 2. FSM을 게임에 쓰는 이유

상태를 나누지 않으면 한 메서드 안에 "대기, 추적, 공격, 사망" 코드가 뒤섞이기 쉽습니다.

FSM을 사용하면 다음처럼 생각할 수 있습니다.

| 상태 | 하는 일 | 다음 상태로 바뀌는 조건 |
| --- | --- | --- |
| `Idle` | 가만히 대기한다 | 플레이어가 감지 거리 안에 들어온다 |
| `Chase` | 플레이어에게 다가간다 | 공격 거리 안에 들어온다 |
| `Attack` | 공격한다 | 플레이어가 공격 거리 밖으로 나간다 |

## 실습 예제: Gizmos로 보는 몬스터 상태 전환

**미션:** 플레이어와 몬스터 사이의 거리에 따라 몬스터 상태가 바뀌고, 상태별 색상이 Scene 뷰에 표시되도록 만듭니다.

### 준비하기

1. Unity 씬에 빈 오브젝트 `MonsterFSM`을 만듭니다.
2. 플레이어 역할을 할 오브젝트를 만들고 `Player`로 이름을 바꿉니다.
3. `MonsterFSM`에 아래 스크립트를 붙입니다.
4. Inspector의 `Player` 필드에 `Player` 오브젝트를 연결합니다.
5. Scene 뷰 오른쪽 위의 `Gizmos` 버튼을 켭니다.
6. Play 모드에서 `Player`를 가까이 옮기며 상태 색이 바뀌는지 확인합니다.

```csharp
using UnityEngine;

public class MonsterStateGizmo : MonoBehaviour
{
    private enum MonsterState
    {
        Idle,
        Chase,
        Attack
    }

    [Header("Target")]
    [Tooltip("몬스터가 감지하고 추적할 플레이어 Transform입니다.")]
    [SerializeField] private Transform player;

    [Header("Distance")]
    [Tooltip("이 거리 안으로 플레이어가 들어오면 Chase 상태로 전환합니다.")]
    [SerializeField] private float chaseDistance = 6f;

    [Tooltip("이 거리 안으로 플레이어가 들어오면 Attack 상태로 전환합니다.")]
    [SerializeField] private float attackDistance = 1.8f;

    private MonsterState currentState = MonsterState.Idle;

    private void Update()
    {
        if (player == null)
        {
            currentState = MonsterState.Idle;
            return;
        }

        // Vector3.Distance는 두 위치 사이의 거리를 계산합니다.
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackDistance)
        {
            currentState = MonsterState.Attack;
        }
        else if (distance <= chaseDistance)
        {
            currentState = MonsterState.Chase;
        }
        else
        {
            currentState = MonsterState.Idle;
        }
    }

    private void OnDrawGizmos()
    {
        // OnDrawGizmos는 Scene 뷰에 개발용 시각 표시를 그릴 때 사용하는 Unity 메시지 메서드입니다.
        Gizmos.color = GetStateColor();
        Gizmos.DrawSphere(transform.position, 0.35f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        if (player == null)
        {
            return;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, player.position);
    }

    private Color GetStateColor()
    {
        switch (currentState)
        {
            case MonsterState.Chase:
                return Color.yellow;
            case MonsterState.Attack:
                return Color.red;
            default:
                return Color.gray;
        }
    }
}
```

### 실행해보면

플레이어가 멀리 있으면 몬스터는 회색으로 보입니다. 플레이어가 노란 감지 원 안으로 들어오면 몬스터가 노란색으로 바뀝니다. 빨간 공격 원 안으로 들어오면 빨간색으로 바뀝니다.

이 색 변화가 상태 전환입니다. Console 로그를 보지 않아도 Scene 뷰에서 현재 상태를 바로 확인할 수 있습니다.

### 생각해보기

1. `Attack` 상태에서 플레이어가 멀어지면 왜 다시 `Chase`나 `Idle`로 돌아가야 할까요?
2. 체력이 0이 되면 어떤 상태를 추가할 수 있을까요?
3. 상태가 너무 많아지면 `switch` 문만으로 관리하기 어려워지는 이유는 무엇일까요?

## 오늘의 정리

- FSM은 캐릭터가 현재 어떤 상태인지 관리하는 구조이다.
- 상태는 한 번에 하나만 선택된다.
- 전환 조건이 만족되면 다른 상태로 바뀐다.
- 거리 조건을 사용하면 `Idle`, `Chase`, `Attack` 같은 몬스터 행동을 쉽게 나눌 수 있다.
- Scene 뷰의 Gizmos를 사용하면 상태 전환을 눈으로 확인할 수 있다.
