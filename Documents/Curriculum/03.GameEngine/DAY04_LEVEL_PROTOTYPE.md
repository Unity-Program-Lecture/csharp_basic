# DAY 04: 레벨 프로토타입과 플레이 테스트

오늘의 목표는 레벨 프로토타입을 "**종이 모형으로 먼저 지어 보는 건물**"처럼 이해하고, 완성도보다 빠른 확인에 집중하는 제작 흐름을 익히는 것입니다.

## 1. 핵심 개념: "작게 만들고 바로 걸어보기"

프로토타입은 예쁜 결과물이 아니라 질문에 답하는 도구입니다. "이 길이 너무 좁은가?", "점프 거리가 적당한가?", "카메라가 목표를 잘 보여 주는가?" 같은 질문을 빠르게 확인합니다. Unity에서는 단순한 큐브와 임시 색상만으로도 충분히 테스트할 수 있습니다.

### 이 단어는 무슨 뜻인가요?

- **Prototype**: 완성 전에 핵심 동작만 빠르게 확인하는 실험판입니다.
- **Play Test**: 직접 플레이하며 문제를 찾는 과정입니다.
- **Iteration**: 만들고, 테스트하고, 고치는 반복 과정입니다.
- **Scale**: 오브젝트와 공간의 크기 감각입니다.

## 실습 예제: 발판 간격 테스트하기

**미션:** 서로 다른 간격의 발판 3개를 만들고, 어느 간격이 가장 자연스러운지 기록합니다.

1. `Platform_A`, `Platform_B`, `Platform_C` 큐브를 만듭니다.
2. 각 발판의 X 간격을 다르게 배치합니다.
3. 가운데 발판에 아래 스크립트를 붙여 움직이는 발판으로 바꿉니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = startPosition + Vector3.right * offset;
    }
}
```

</details>

### 실행해보면

Play를 누르면 가운데 발판이 좌우로 움직입니다. `moveDistance`와 `moveSpeed`를 바꾸면 발판 난이도가 어떻게 달라지는지 바로 체감할 수 있습니다.

### 생각해보기

1. 플레이어 이동 속도가 빨라지면 발판 간격은 어떻게 조정해야 할까요?
2. 테스트용 색상을 쓰면 어떤 정보를 더 빨리 구분할 수 있을까요?

## 오늘의 정리

- 프로토타입은 빠른 확인을 위한 임시 제작물입니다.
- 플레이 테스트는 눈으로 보기보다 직접 조작하며 문제를 찾는 과정입니다.
- 숫자와 체감을 함께 기록하면 다음 수정 방향을 잡기 쉽습니다.
