# DAY 13: VFX Graph 제어와 성능

오늘의 목표는 VFX Graph 이펙트를 "**멋있지만 조절 가능한 장치**"로 만들고, 게임 상황과 성능에 맞게 켜고 끄는 방법을 배우는 것입니다.

## NCS 연결

- 능력단위 요소: 이펙트 프로그래밍하기
- 관련 학습 내용: 게임 이펙트 특성 파악, 엔진에서 사용, 테스트와 디버깅
- Unity 6 재구성: Exposed Property, Visual Effect 컴포넌트, 품질 옵션을 사용합니다.

## 1. 왜 성능을 생각해야 하나요?

이펙트는 화면을 풍부하게 만들지만 너무 많으면 프레임이 떨어집니다. 좋은 그래픽 프로그래밍은 무조건 화려하게 만드는 것이 아니라, 필요한 순간에 필요한 만큼 보여주는 것입니다.

### 조절할 수 있는 값

| 값 | 효과 |
| :--- | :--- |
| Spawn Rate | 입자 생성량 |
| Lifetime | 입자가 남아 있는 시간 |
| Bounds | 이펙트가 보이는 영역 |
| Texture Size | 입자 텍스처 품질 |
| Quality Toggle | 낮은 사양에서 끄거나 줄이는 옵션 |

## 2. 실습: 코드로 VFX 강도 조절

**미션:** 키 입력으로 VFX Graph의 강도를 바꿉니다.

## 3. VFX Graph 프로퍼티 노출하기

코드에서 VFX Graph 값을 바꾸려면 먼저 그래프 안의 값을 외부로 꺼내야 합니다. 이것을 Exposed Property라고 생각하면 됩니다. 즉, 그래프 안에 숨어 있는 손잡이를 Inspector와 C# 코드에서 만질 수 있게 만드는 것입니다.

### SpawnRate 노출 절차

1. VFX Graph의 Blackboard에서 `+` 버튼을 누릅니다.
2. `float` 프로퍼티를 추가합니다.
3. 이름을 `SpawnRate`로 바꿉니다.
4. 프로퍼티의 Exposed 설정이 켜져 있는지 확인합니다.
5. `SpawnRate` 프로퍼티를 Graph Area로 끌어다 놓습니다.
6. Spawn Context의 Constant Spawn Rate 값 입력에 연결합니다.
7. 그래프를 저장합니다.
8. 씬의 Visual Effect 컴포넌트에서 `SpawnRate` 값이 보이는지 확인합니다.

이름은 대소문자까지 코드와 같아야 합니다. 코드에서 `SetFloat("SpawnRate", value)`라고 쓰면 VFX Graph 프로퍼티 이름도 정확히 `SpawnRate`여야 합니다.

### 추가로 노출하기 좋은 값

| 프로퍼티 | 타입 | 연결 위치 | 사용 예 |
| :--- | :--- | :--- | :--- |
| `SpawnRate` | Float | Spawn Rate | 입자 개수 조절 |
| `EffectColor` | Color 또는 Vector4 | Output Color | 속성에 따라 색 변경 |
| `ParticleSize` | Float | Set Size | 품질 옵션에 따라 크기 변경 |
| `UpForce` | Float 또는 Vector3 | Add Force | 바람, 폭발 방향 조절 |

## 4. 코드에서 제어할 때의 흐름

```text
PlayerInput Action 발생 -> OnLowIntensity 또는 OnHighIntensity 호출
-> VisualEffect.SetFloat("SpawnRate", 값)
-> VFX Graph의 SpawnRate 프로퍼티 변경
-> 생성되는 입자 수 변화
```

VFX Graph는 모든 값을 코드에서 직접 만드는 방식보다, 그래프에서 기본 표현을 만든 뒤 중요한 손잡이만 코드로 조절하는 방식이 수업에 적합합니다.

### Input System 설정

Player 오브젝트 또는 빈 GameObject에 `PlayerInput` 컴포넌트를 추가하고 다음 Action을 준비합니다.

| Action 이름 | Action Type | Binding 예시 | 역할 |
| :--- | :--- | :--- | :--- |
| `LowIntensity` | Button | `<Keyboard>/1` | 낮은 강도로 전환합니다. |
| `HighIntensity` | Button | `<Keyboard>/2` | 높은 강도로 전환합니다. |

`PlayerInput`의 Behavior는 `Send Messages`로 설정합니다. 그러면 Action 이름에 맞춰 `OnLowIntensity`, `OnHighIntensity` 메서드가 호출됩니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class VfxIntensityController : MonoBehaviour
{
    [SerializeField] private VisualEffect visualEffect;
    [SerializeField] private string spawnRateName = "SpawnRate";
    [SerializeField] private float lowRate = 20f;
    [SerializeField] private float highRate = 200f;

    public void OnLowIntensity(InputValue value)
    {
        if (value.isPressed)
        {
            visualEffect.SetFloat(spawnRateName, lowRate);
        }
    }

    public void OnHighIntensity(InputValue value)
    {
        if (value.isPressed)
        {
            visualEffect.SetFloat(spawnRateName, highRate);
        }
    }
}
```

</details>

## 5. VFX Graph 성능 조절 기준

VFX Graph는 GPU에서 많은 입자를 처리할 수 있지만, 무제한으로 써도 된다는 뜻은 아닙니다. 수업에서는 다음 값을 우선 조절합니다.

| 상황 | 먼저 줄일 값 | 이유 |
| :--- | :--- | :--- |
| 프레임이 떨어짐 | `SpawnRate` | 화면에 존재하는 입자 수가 줄어듭니다. |
| 이펙트가 너무 오래 남음 | Lifetime | 동시에 살아 있는 입자 수가 줄어듭니다. |
| 화면 밖 이펙트가 계속 계산됨 | Bounds | 보이지 않는 이펙트의 낭비를 줄입니다. |
| 지나치게 밝고 지저분함 | Output Color, Alpha | 시각적 피로를 줄입니다. |
| 저사양 옵션 필요 | Quality Toggle | 이펙트를 약하게 하거나 끕니다. |

## 6. 디버깅 체크리스트

| 문제 | 확인 순서 |
| :--- | :--- |
| 코드가 값을 바꿔도 변화가 없음 | 프로퍼티 이름 대소문자, Exposed 설정, 그래프 저장 여부 확인 |
| `SetFloat` 호출은 되는데 입자 수가 그대로임 | `SpawnRate`가 실제 Spawn Context에 연결되어 있는지 확인 |
| Play 모드에서만 값이 초기화됨 | Visual Effect 컴포넌트의 Override 값과 그래프 기본값 확인 |
| 입력이 동작하지 않음 | `PlayerInput` Behavior가 `Send Messages`인지, Action 이름과 메서드 이름이 맞는지 확인 |
| 이펙트가 갑자기 잘림 | Bounds 크기가 이펙트 움직임보다 작은지 확인 |

## 스크린샷 체크포인트

- `Images/day13_vfx_exposed_property.png`: `SpawnRate`가 Exposed Property로 보이는 VFX Graph
- `Images/day13_player_input_actions.png`: `LowIntensity`, `HighIntensity` Action이 설정된 Input Actions 화면
- `Images/day13_vfx_quality_compare.png`: 낮은 강도와 높은 강도 비교 화면
- `Images/day13_vfx_component_exposed_values.png`: Visual Effect 컴포넌트에서 노출 프로퍼티가 보이는 화면

## 오늘의 정리

- VFX Graph는 노출 프로퍼티를 통해 코드와 연결할 수 있습니다.
- Unity 6 수업 예제에서는 `PlayerInput` Action을 통해 입력과 VFX 제어를 연결합니다.
- 이펙트는 성능 옵션을 함께 설계해야 실제 게임에 넣기 쉽습니다.
- 다음 시간에는 셰이더와 이펙트를 하나의 포트폴리오 씬으로 통합합니다.
