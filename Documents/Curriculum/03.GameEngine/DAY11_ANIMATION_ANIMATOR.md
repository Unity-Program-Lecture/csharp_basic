# DAY 11: Animation과 Animator 기초

오늘의 목표는 캐릭터 애니메이션을 "**상황에 맞는 동작 카드를 골라 재생하는 시스템**"으로 이해하고, `Animator`를 사용해 이동 상태에 따라 Idle과 Walk 애니메이션이 전환되도록 구성하는 것입니다.

## 1. 핵심 개념: "Animation은 동작이고 Animator는 동작 관리자다"

`Animation Clip`은 걷기, 뛰기, 공격하기처럼 시간에 따라 자세가 변하는 하나의 동작 자료입니다. `Animator`는 여러 Animation Clip 중 현재 상황에 맞는 동작을 선택하고 자연스럽게 전환하는 컴포넌트입니다.

리모컨에 여러 채널이 저장되어 있어도 버튼을 누르기 전에는 어떤 채널을 보여 줄지 정해지지 않습니다. 같은 방식으로 Animation Clip이 여러 개 있어도 `Animator Controller`가 상태와 전환 규칙을 관리해야 게임 상황에 맞는 동작이 재생됩니다.

### 이 단어는 무슨 뜻인가요?

- **Animation Clip**: Idle, Walk, Run처럼 하나의 동작을 기록한 에셋입니다.
- **Animator**: Animator Controller를 실행하여 현재 상태의 애니메이션을 재생하는 컴포넌트입니다.
- **Animator Controller**: 애니메이션 상태와 전환 규칙을 저장하는 에셋입니다.
- **State**: 현재 재생할 동작 하나를 나타내는 상태입니다.
- **Transition**: 한 State에서 다른 State로 넘어가는 연결선과 조건입니다.
- **Parameter**: 코드와 Animator Controller가 상태 정보를 주고받는 값입니다.
- **Avatar**: 사람형 모델의 뼈 구조를 Unity 애니메이션 구조에 연결하는 정보입니다.
- **Root Motion**: 애니메이션에 기록된 루트 뼈의 이동을 실제 GameObject 이동에 적용하는 방식입니다.

## 2. Animation 창과 Animator 창 구분

| 창 | 역할 | 비유 |
| :--- | :--- | :--- |
| `Animation` | Animation Clip의 키와 시간을 확인하거나 편집 | 동작 카드 한 장을 만드는 작업대 |
| `Animator` | 여러 State와 Transition을 연결 | 상황에 따라 카드를 고르는 흐름도 |

이름이 비슷하지만 역할은 다릅니다. `Window > Animation > Animation`에서는 선택한 오브젝트의 개별 동작을 확인하고, `Window > Animation > Animator`에서는 상태 전환 구조를 확인합니다.

## 3. Animator Controller의 기본 구조

Animator Controller를 열면 여러 State와 화살표가 보입니다.

- `Entry`는 상태 머신이 시작되는 입구입니다.
- 주황색 State는 처음 재생되는 기본 상태입니다.
- Transition의 `Conditions`는 상태를 바꾸는 조건입니다.
- `Has Exit Time`은 현재 애니메이션을 일정 부분 재생한 뒤 전환할지 결정합니다.
- `Transition Duration`은 두 동작을 섞어 전환하는 시간입니다.

이동 입력에 즉시 반응해야 하는 Idle과 Walk 전환에서는 보통 `Has Exit Time`을 끄고 Parameter 조건을 사용합니다. 공격처럼 동작을 끝까지 보여 주어야 하는 경우에는 Exit Time이 필요할 수 있습니다.

### Parameter 종류

| 종류 | 사용 예 | 코드 연결 |
| :--- | :--- | :--- |
| `Float` | 이동 속도, 조준 정도 | `SetFloat` |
| `Int` | 무기 번호, 자세 번호 | `SetInteger` |
| `Bool` | 이동 중, 지상 여부 | `SetBool` |
| `Trigger` | 공격, 피격처럼 한 번 발생하는 사건 | `SetTrigger` |

`Float`와 `Bool`은 현재 상태를 계속 전달하는 값입니다. `Trigger`는 초인종처럼 한 번 눌렀다는 사건을 전달할 때 사용합니다.

## 4. Animator Inspector 주요 프로퍼티

| 프로퍼티 | 의미 | 수업에서 확인할 것 |
| :--- | :--- | :--- |
| `Controller` | 사용할 Animator Controller | 만든 Controller 에셋이 연결되어 있는지 확인 |
| `Avatar` | 모델의 뼈대 연결 정보 | Humanoid 모델이라면 올바른 Avatar가 연결되었는지 확인 |
| `Apply Root Motion` | 애니메이션의 이동을 실제 위치에 적용 | CharacterController 이동 실습에서는 끔 |
| `Update Mode` | Animator가 갱신되는 시간 기준 | 기본 실습에서는 `Normal` 사용 |
| `Culling Mode` | 화면 밖에서 애니메이션을 계산할지 결정 | 기본 실습에서는 기본값을 유지 |

DAY 10에서 `CharacterController.Move`가 실제 이동을 담당하므로 이번 실습에서는 `Apply Root Motion`을 끕니다. 코드 이동과 Root Motion을 동시에 적용하면 캐릭터가 예상보다 빠르게 움직이거나 충돌체와 모델이 어긋날 수 있습니다.

## 5. Animation Clip 가져오기와 확인

외부 모델 파일에 애니메이션이 포함되어 있다면 Project 창에서 모델을 선택하고 Inspector의 `Rig`와 `Animations` 탭을 확인합니다.

1. `Rig` 탭에서 모델에 맞는 Animation Type을 확인합니다.
2. `Animations` 탭에서 Idle과 Walk Clip의 구간과 이름을 확인합니다.
3. 반복 동작인 Idle과 Walk는 `Loop Time`을 켭니다.
4. `Apply`를 눌러 Import Settings 변경을 저장합니다.

모든 모델이 Humanoid일 필요는 없습니다. 사람형 캐릭터는 주로 `Humanoid`, 형태가 다른 생물이나 기계는 `Generic`을 사용할 수 있습니다. 이 수업에서는 이미 동작이 포함된 모델이나 준비된 Animation Clip을 사용하며, 뼈대를 직접 만들거나 동작을 직접 제작하는 과정은 다루지 않습니다.

## 실습 예제: 이동 속도로 Idle과 Walk 전환하기

**미션:** DAY 10에서 만든 CharacterController 캐릭터에 Animator를 연결하고, 실제 이동 속도에 따라 Idle과 Walk가 전환되도록 만듭니다.

### 1단계: Animator Controller 만들기

1. Project 창에서 `Create > Animator Controller`를 선택합니다.
2. 이름을 `PlayerAnimatorController`로 정합니다.
3. Animator 창에 Idle과 Walk Animation Clip을 드래그해 State를 만듭니다.
4. Idle을 기본 State로 지정합니다.
5. `Float` Parameter를 만들고 이름을 `Speed`로 정합니다.
6. Idle에서 Walk로 Transition을 만들고 `Speed > 0.1` 조건을 지정합니다.
7. Walk에서 Idle로 Transition을 만들고 `Speed < 0.1` 조건을 지정합니다.
8. 두 Transition의 `Has Exit Time`을 끕니다.

### 2단계: 캐릭터 구조 연결하기

```text
Player_CC
  Visual
```

1. DAY 10의 `Player_CC` 아래에 캐릭터 모델을 `Visual` 자식으로 배치합니다.
2. `Visual`에 `Animator` 컴포넌트를 추가합니다.
3. `Controller`에 `PlayerAnimatorController`를 연결합니다.
4. `Apply Root Motion`을 끕니다.

충돌과 실제 위치 이동은 부모 `Player_CC`가 담당하고, 보이는 모델과 애니메이션은 자식 `Visual`이 담당합니다. 역할을 나누면 모델을 바꾸더라도 이동 코드와 충돌 설정을 유지하기 쉽습니다.

### 3단계: 속도를 Animator에 전달하기

아래 스크립트를 `SimpleAnimatorDriver.cs`로 만들고 `Visual`에 붙입니다.

```csharp
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimpleAnimatorDriver : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    private Animator animator;
    private CharacterController characterController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        Vector3 horizontalVelocity = characterController.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude;
        animator.SetFloat(SpeedHash, speed, 0.1f, Time.deltaTime);
    }
}
```

### 코드 읽기

1. `CharacterController.velocity`에서 현재 이동 속도를 가져옵니다.
2. 위아래 속도인 `y`를 제외하여 평면 이동 속도만 계산합니다.
3. `magnitude`로 이동 방향을 하나의 속도 값으로 바꿉니다.
4. `SetFloat`로 Animator의 `Speed` Parameter에 값을 전달합니다.
5. `0.1f`의 감쇠 시간을 사용해 값이 급격히 바뀌지 않도록 합니다.

코드는 위에서 아래로 읽고, 대입문은 오른쪽 값을 계산한 뒤 왼쪽 변수에 저장한다고 읽습니다. `horizontalVelocity.magnitude`가 먼저 계산되고, 그 결과가 `speed` 상자에 들어갑니다.

### 실행해보면

캐릭터가 멈춰 있을 때는 Idle State가 재생되고, 이동하면 Walk State로 전환됩니다. Animator 창을 Play 모드에서 열면 현재 State가 강조되고 `Speed` 값이 변하는 것도 확인할 수 있습니다.

### 문제가 생겼다면

- 캐릭터가 움직이지만 애니메이션이 바뀌지 않으면 Parameter 이름과 대소문자를 확인합니다.
- 모델이 미끄러지듯 이동하면 Walk Clip의 재생 속도와 캐릭터 이동 속도를 비교합니다.
- 캐릭터가 두 배로 이동하는 느낌이면 `Apply Root Motion`이 꺼져 있는지 확인합니다.
- 애니메이션이 한 번만 재생되면 Idle과 Walk Clip의 `Loop Time`을 확인합니다.

### 생각해보기

1. Animation Clip과 Animator Controller는 각각 어떤 역할을 담당할까요?
2. Idle과 Walk 전환에서 `Has Exit Time`을 끄는 이유는 무엇일까요?
3. CharacterController 이동과 Root Motion을 동시에 사용하면 어떤 문제가 생길 수 있을까요?
4. 공격처럼 한 번 발생하는 동작에는 `Bool`과 `Trigger` 중 어느 Parameter가 더 어울릴까요?

## 심화 학습으로 남겨 둘 내용

다음 기능은 Animator의 중요한 기능이지만 이번 기초 차시에서는 구현하지 않습니다.

- `Blend Tree`를 사용한 걷기와 달리기 혼합
- `Layer`와 `Avatar Mask`를 사용한 상체와 하체 동작 분리
- `Animation Event`로 발소리나 공격 판정 호출
- `IK`를 사용한 손과 발 위치 보정
- `Timeline`을 사용한 연출 애니메이션 제작

## 오늘의 정리

- Animation Clip은 하나의 동작 자료이고, Animator는 상황에 맞는 동작을 선택해 재생합니다.
- Animator Controller는 State, Transition, Parameter로 애니메이션 흐름을 구성합니다.
- 이동 속도는 `SetFloat`, 지속 상태는 `SetBool`, 한 번의 사건은 `SetTrigger`로 전달할 수 있습니다.
- CharacterController가 이동을 담당하는 실습에서는 `Apply Root Motion`을 끕니다.
- 엔진 과정에서는 기본 상태 전환과 코드 연결을 익히고, 복잡한 Blend Tree와 Layer 구성은 후속 과정에서 확장합니다.
