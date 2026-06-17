# DAY 10: Character Controller와 캐릭터 이동 기초

오늘의 목표는 `CharacterController`를 "**캡슐 모양으로 길을 더듬으며 움직이는 이동 부품**"으로 이해하고, Rigidbody 물리 이동과 다른 방식의 캐릭터 이동을 구성하는 것입니다.

## 1. 핵심 개념: "캐릭터는 굴러가는 공이 아니라 조종되는 캡슐이다"

Rigidbody는 힘, 중력, 충돌 반응을 물리 엔진에 맡기는 방식입니다. 반면 `CharacterController`는 캐릭터를 직접 이동시키면서 벽, 바닥, 계단, 경사면과의 충돌을 확인하는 컴포넌트입니다.

게임의 플레이어 캐릭터는 공처럼 밀려다니기보다 입력에 맞춰 안정적으로 움직여야 하는 경우가 많습니다. 이때 `CharacterController`를 사용하면 캡슐 형태의 충돌 범위를 기준으로 이동, 바닥 확인, 계단 오르기, 경사 제한을 다룰 수 있습니다.

### 이 단어는 무슨 뜻인가요?

- **CharacterController**: 캐릭터 이동에 특화된 캡슐형 충돌 컴포넌트입니다.
- **Move**: 개발자가 계산한 이동량만큼 캐릭터를 움직이는 함수입니다.
- **SimpleMove**: 속도와 중력을 간단히 처리해 이동하는 함수입니다.
- **isGrounded**: 캐릭터가 바닥에 닿아 있는지 알려 주는 값입니다.
- **Slope Limit**: 캐릭터가 걸어 올라갈 수 있는 최대 경사 각도입니다.
- **Step Offset**: 캐릭터가 자동으로 올라갈 수 있는 계단 높이입니다.
- **Skin Width**: 충돌체가 살짝 겹칠 수 있게 허용하는 여유 폭입니다.

## 2. CharacterController Inspector 주요 프로퍼티

| 프로퍼티 | 의미 | 수업에서 확인할 것 |
| :--- | :--- | :--- |
| `Slope Limit` | 올라갈 수 있는 최대 경사 각도 | 값이 낮으면 완만한 언덕도 못 올라갈 수 있음 |
| `Step Offset` | 자동으로 올라갈 수 있는 계단 높이 | 캐릭터 키보다 크면 부자연스러움 |
| `Skin Width` | 충돌 판정의 여유 폭 | 너무 작으면 벽이나 바닥에 걸리는 느낌이 날 수 있음 |
| `Min Move Distance` | 이 거리보다 작은 이동을 무시하는 값 | 초급 실습에서는 보통 `0`으로 둠 |
| `Center` | 캡슐 충돌 범위의 중심 위치 | 모델과 충돌 캡슐이 어긋나면 조정 |
| `Radius` | 캡슐의 반지름 | 너무 크면 좁은 길을 못 지나감 |
| `Height` | 캡슐의 높이 | 캐릭터 모델 키와 비슷하게 맞춤 |

`Skin Width`는 캐릭터가 충돌면에 너무 딱 붙어서 떨리거나 끼는 현상을 줄이는 데 중요합니다. 일반적으로 너무 작게 두지 않고, 캐릭터 `Radius`에 비례해 적당한 여유를 둡니다.

## 3. Rigidbody 이동과 CharacterController 이동 비교

| 구분 | Rigidbody | CharacterController |
| :--- | :--- | :--- |
| 이동 방식 | 힘, 속도, 물리 계산 중심 | 코드에서 이동량을 직접 전달 |
| 충돌 반응 | 물리 엔진이 밀림과 반동을 계산 | 벽과 바닥을 감지하되 이동은 직접 제어 |
| 주 용도 | 공, 상자, 발사체, 물리 오브젝트 | 플레이어, NPC, 안정적인 캐릭터 이동 |
| 중력 | Rigidbody가 처리 가능 | 코드에서 직접 누적하는 경우가 많음 |

## 실습 예제: CharacterController로 기본 이동 만들기

**미션:** 입력 값을 임시로 코드에 넣어 `CharacterController.Move`로 캐릭터를 이동시킵니다.

1. 캡슐 오브젝트를 하나 만들고 이름을 `Player_CC`로 정합니다.
2. `CharacterController` 컴포넌트를 추가합니다.
3. Inspector에서 `Height`, `Radius`, `Center`가 캡슐 모델과 맞는지 확인합니다.
4. 아래 스크립트를 `SimpleCharacterControllerMover.cs`로 만들고 `Player_CC`에 붙입니다.
5. Play 버튼을 누르고 방향키 또는 WASD로 이동을 확인합니다.

```csharp
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterControllerMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;
        move *= moveSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -1f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}
```

이 예제는 `Input System`이 아니라 기존 축 입력을 사용해 이동 구조만 먼저 확인합니다. `Input System`을 사용한 실제 플레이어 조작 구조는 클라이언트 과정에서 다시 정리합니다.

### 실행해보면

캐릭터가 입력 방향으로 이동하고, 바닥에 닿아 있을 때 아래로 계속 가속되지 않도록 `isGrounded`를 사용합니다. 벽이 있으면 `CharacterController`의 캡슐 범위가 벽을 통과하지 않도록 막습니다.

### 생각해보기

1. `Radius`가 너무 크면 좁은 통로에서 어떤 문제가 생길까요?
2. `Step Offset`을 너무 크게 하면 캐릭터 움직임이 왜 어색해질까요?
3. Rigidbody 이동과 CharacterController 이동 중 플레이어 캐릭터에 더 안정적인 방식은 어느 쪽일까요?

## 오늘의 정리

- `CharacterController`는 캐릭터 이동에 특화된 캡슐형 충돌 컴포넌트입니다.
- Rigidbody처럼 물리 힘에 맡기는 방식이 아니라, 코드에서 계산한 이동량을 `Move`로 전달합니다.
- Inspector의 `Slope Limit`, `Step Offset`, `Skin Width`, `Radius`, `Height` 값은 이동 감각과 충돌 안정성에 직접 영향을 줍니다.
- 엔진 과정에서는 컴포넌트의 역할과 이동 원리를 확인하고, 클라이언트 과정에서는 입력과 카메라를 연결한 조작 구조로 확장합니다.
