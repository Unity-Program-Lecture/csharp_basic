# DAY 11: 이펙트와 게임 코드 연동

오늘의 목표는 이펙트를 "**게임 사건이 일어났다는 신호등**"처럼 사용하고, 입력이나 충돌이 발생했을 때 알맞은 위치에서 재생하는 것입니다.

## NCS 연결

- 능력단위 요소: 이펙트 프로그래밍하기
- 관련 학습 내용: 게임 엔진에서 이펙트를 코드로 출력
- Unity 6 재구성: C# 스크립트에서 Particle System Prefab을 생성하고 재생합니다.

## 1. 코드 연동의 기본 흐름

```text
게임 사건 발생 -> 위치 결정 -> 이펙트 생성 -> 재생 -> 일정 시간 뒤 제거
```

예를 들어 공격이 맞았다면 맞은 위치에 `FX_HitSpark`를 만들고 재생합니다.

## 실습 예제: 클릭 위치에 이펙트 재생

**미션:** 마우스로 바닥을 클릭하면 해당 위치에 파티클 이펙트를 재생합니다.

### Input System 설정

Player 오브젝트 또는 빈 GameObject에 `PlayerInput` 컴포넌트를 추가하고 다음 Action을 준비합니다.

| Action 이름 | Action Type | Binding 예시 | 역할 |
| :--- | :--- | :--- | :--- |
| `Point` | Value / Vector2 | `<Pointer>/position` | 마우스 또는 터치 위치를 읽습니다. |
| `Click` | Button | `<Mouse>/leftButton` | 클릭 입력을 받습니다. |

`PlayerInput`의 Behavior는 `Send Messages`로 설정합니다. 그러면 Action 이름에 맞춰 `OnPoint`, `OnClick` 메서드가 호출됩니다.

### Input Actions Asset과 PlayerInput 연결 순서

1. Project 창의 `GameGraphics/Input` 폴더에서 `Create > Input Actions`를 선택하고 `GraphicsInputActions`로 이름을 바꿉니다.
2. Asset을 더블 클릭해 Input Actions 편집기를 열고 `Gameplay` Action Map을 만듭니다.
3. `+`로 `Point` Action을 추가하고 Action Type을 `Value`, Control Type을 `Vector2`로 설정한 뒤 Binding에 `<Pointer>/position`을 추가합니다.
4. `Click` Action은 Type을 `Button`으로 두고 Binding에 `<Mouse>/leftButton`을 추가합니다. 저장 버튼을 누르거나 창을 닫기 전에 변경 사항이 저장됐는지 확인합니다.
5. Hierarchy에서 `EffectInput` 빈 GameObject를 만들고 `PlayerInput`과 `ClickEffectSpawner`를 함께 추가합니다. PlayerInput의 Actions에 `GraphicsInputActions`, Default Map에 `Gameplay`, Behavior에 `Send Messages`를 지정합니다.
6. `ClickEffectSpawner`의 Target Camera, Effect Prefab, Ground Mask를 연결한 뒤 Play Mode에서 Console Error 없이 `OnPoint`, `OnClick`이 호출되는지 확인합니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickEffectSpawner : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private ParticleSystem effectPrefab;
    [SerializeField] private LayerMask groundMask;

    private Vector2 pointerPosition;

    public void OnPoint(InputValue value)
    {
        pointerPosition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        Ray ray = targetCamera.ScreenPointToRay(pointerPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            ParticleSystem effect = Instantiate(effectPrefab, hit.point, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }
    }
}
```

</details>

## 코드 읽기

- `OnPoint`는 `PlayerInput`의 `Point` Action이 보낸 화면 좌표를 저장합니다.
- `OnClick`은 `Click` Action이 눌렸을 때만 이펙트를 생성합니다.
- `targetCamera.ScreenPointToRay`는 화면 클릭 위치에서 씬으로 광선을 쏩니다.
- `Physics.Raycast`는 광선이 바닥에 닿았는지 확인합니다.
- `Instantiate`는 이펙트 프리팹을 클릭 위치에 만듭니다.
- `Destroy`는 이펙트가 끝난 뒤 오브젝트를 정리합니다.

## ClickEffectSpawner Inspector 연결 절차

`ClickEffectSpawner`를 빈 GameObject에 붙인 뒤 Inspector의 `Target Camera`에는 Main Camera를, `Effect Prefab`에는 DAY 10에서 만든 Particle System Prefab을 끌어 놓습니다. `Ground Mask`에는 클릭을 받을 Plane의 Layer만 선택합니다. LayerMask가 `Nothing`이면 Raycast가 항상 실패하고, `Everything`이면 캐릭터나 장식 오브젝트를 바닥으로 잘못 인식할 수 있습니다.

Plane에는 Collider가 있어야 합니다. Plane을 선택해 Inspector 상단 Layer를 `Ground`로 바꾸고 Collider가 활성화돼 있는지 확인합니다. PlayerInput을 같은 오브젝트 또는 입력을 전달받는 오브젝트에 붙이고 Actions Asset, Default Action Map, Behavior `Send Messages`를 지정합니다. Action 이름 `Point`, `Click`은 각각 `OnPoint`, `OnClick` 메서드 이름과 정확히 대응합니다.

Play Mode에서 먼저 마우스를 움직여 Point 입력이 들어오는지, 다음으로 Plane을 클릭했을 때만 이펙트가 한 번 생성되는지 확인합니다. 이펙트가 여러 번 생성되면 Click Action Binding, `value.isPressed` 검사, PlayerInput 중복 여부를 확인합니다. 생성은 되지만 바로 사라지면 Prefab의 Duration·Start Lifetime과 `Destroy` 시간을 확인합니다.

## 오늘의 정리

- 이펙트는 게임 사건과 연결될 때 의미가 생깁니다.
- Unity 6 수업 예제에서는 Input System과 `PlayerInput`을 사용합니다.
- 생성한 이펙트는 재생 후 정리해야 씬이 지저분해지지 않습니다.
- 다음 시간에는 GPU 기반 대량 이펙트를 다루는 Visual Effect Graph를 시작합니다.
