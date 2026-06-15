# DAY 12: 사운드와 파티클 효과

오늘의 목표는 사운드와 VFX를 "**게임의 손맛을 알려 주는 반응**"으로 이해하고, AudioSource, Audio Listener, ParticleSystem을 함께 사용하는 방법을 익히는 것입니다.

## 1. 핵심 개념: "보이는 반응과 들리는 반응"

플레이어가 버튼을 누르거나 공격이 맞았을 때 아무 반응이 없으면 동작이 어색합니다. 사운드는 귀로 결과를 알려 주고, 파티클은 눈으로 결과를 보여 줍니다. 둘을 함께 쓰면 작은 상호작용도 훨씬 분명해집니다.

Unity에서 소리는 `AudioSource` 혼자만으로 완성되지 않습니다. `AudioSource`가 스피커라면, `Audio Listener`는 그 소리를 듣는 귀입니다. 보통 Main Camera에 Audio Listener가 붙어 있으며, 씬 안에는 일반적으로 하나만 두는 것이 좋습니다.

### 이 단어는 무슨 뜻인가요?

- **AudioClip**: 재생할 사운드 파일입니다.
- **AudioSource**: AudioClip을 실제로 재생하는 컴포넌트입니다.
- **Audio Listener**: 씬 안의 소리를 듣는 귀 역할을 하는 컴포넌트입니다.
- **ParticleSystem**: 불꽃, 연기, 반짝임 같은 입자 효과를 만드는 컴포넌트입니다.
- **Play One Shot**: 같은 AudioSource에서 짧은 효과음을 한 번 재생하는 방식입니다.
- **Loop**: 사운드나 파티클을 한 번만 재생하지 않고 반복 재생하는 옵션입니다.

## 2. AudioSource와 Audio Listener

사운드를 재생하려면 재생하는 쪽과 듣는 쪽이 필요합니다.

```text
AudioClip     = 소리 파일
AudioSource   = 소리를 내는 스피커
Audio Listener = 소리를 듣는 귀
```

대부분의 3D 게임에서는 Main Camera에 `Audio Listener`가 붙어 있습니다. 플레이어가 보는 위치에서 소리를 듣는 것이 자연스럽기 때문입니다. 만약 카메라를 새로 만들거나 교체했다면, 새 카메라에 Audio Listener가 있는지 확인해야 합니다.

주의할 점은 Audio Listener를 여러 개 두지 않는 것입니다. 귀가 여러 개 있으면 Unity가 어떤 위치에서 소리를 들어야 할지 헷갈릴 수 있고, Console에 경고가 뜰 수 있습니다. 기본 카메라를 지우지 않고 새 카메라를 만들면 Audio Listener가 중복되는 일이 자주 생깁니다.

AudioSource에서 자주 만지는 값은 다음과 같습니다.

| 항목 | 의미 | 예시 |
| :--- | :--- | :--- |
| `AudioClip` | 재생할 소리 파일 | 공격음, 점프음, 버튼음 |
| `Play On Awake` | 시작하자마자 자동 재생할지 | 배경음악은 켜고, 공격 효과음은 끄는 경우가 많음 |
| `Loop` | 반복 재생할지 | 배경음악, 기계 소리 |
| `Volume` | 소리 크기 | 너무 크면 다른 피드백을 덮어 버림 |
| `Pitch` | 소리 높낮이와 재생 속도 | 같은 효과음도 약간 다르게 들리게 만들 수 있음 |
| `Spatial Blend` | 2D 소리와 3D 소리의 비율 | UI 버튼음은 2D, 폭발음은 3D에 가까움 |

UI 버튼음처럼 화면 어디서나 똑같이 들려야 하는 소리는 `Spatial Blend`를 2D에 가깝게 둡니다. 반대로 폭발, 발소리, 몬스터 울음소리처럼 위치가 중요한 소리는 3D에 가깝게 두면 거리와 방향에 따라 다르게 들립니다.

## 3. ParticleSystem의 핵심 구성요소

ParticleSystem은 작은 입자를 많이 뿌려서 불꽃, 연기, 먼지, 마법 효과처럼 보이게 만드는 컴포넌트입니다. Inspector에는 항목이 매우 많지만, 처음에는 아래 모듈만 잡아도 대부분의 간단한 효과를 만들 수 있습니다.

| 모듈 | 쉽게 말하면 | 주로 조절하는 것 |
| :--- | :--- | :--- |
| `Main` | 파티클 전체 기본 설정 | 재생 시간, 반복 여부, 시작 수명, 시작 속도, 시작 크기 |
| `Emission` | 입자를 얼마나 뿌릴지 | 초당 생성량, 한 번에 터지는 Burst |
| `Shape` | 어디서 어떤 모양으로 뿌릴지 | 원, 구, 박스, 콘 방향 |
| `Color over Lifetime` | 시간이 지나며 색이 어떻게 바뀔지 | 처음엔 노랑, 끝에는 투명한 주황 |
| `Size over Lifetime` | 시간이 지나며 크기가 어떻게 바뀔지 | 점점 커지는 연기, 점점 작아지는 불꽃 |
| `Renderer` | 어떤 재질과 방식으로 보일지 | 파티클 Material, 정렬 방식 |

`Main`은 파티클의 기본 성격을 정합니다. `Duration`은 효과가 몇 초 동안 재생되는지, `Looping`은 반복할지, `Start Lifetime`은 입자 하나가 얼마나 오래 살아남는지, `Start Speed`는 입자가 얼마나 빠르게 움직이는지를 뜻합니다.

`Emission`은 입자 수를 정합니다. `Rate over Time`은 계속 뿌리는 양이고, `Bursts`는 특정 순간에 한꺼번에 터지는 양입니다. 폭발 효과는 Burst가 어울리고, 모닥불이나 연기는 Rate over Time이 어울립니다.

`Shape`는 입자가 출발하는 모양입니다. 검 끝에서 번쩍이는 효과는 작은 Cone이나 Sphere가 어울리고, 바닥 먼지는 넓은 Circle이나 Box가 어울립니다. Shape가 맞지 않으면 색이 예뻐도 효과가 엉뚱한 방향으로 보일 수 있습니다.

`Color over Lifetime`과 `Size over Lifetime`은 파티클이 살아 있는 동안 변하는 모습을 만듭니다. 예를 들어 불꽃은 처음에 밝고 작게 시작해서 점점 어두워지며 사라지고, 연기는 처음엔 작지만 점점 커지며 투명해지는 식으로 만들 수 있습니다.

## 실습 예제: 효과음과 파티클 동시에 실행하기

**미션:** 스페이스바를 누르면 파티클과 효과음이 동시에 재생되도록 합니다.

1. Main Camera에 `Audio Listener`가 있는지 확인합니다.
2. 씬에 Audio Listener가 두 개 이상 있으면 하나만 남깁니다.
3. 빈 GameObject에 `AudioSource`를 붙입니다.
4. 효과음용 AudioSource는 `Play On Awake`를 끕니다.
5. UI 버튼음처럼 위치가 중요하지 않은 효과음이면 `Spatial Blend`를 2D 쪽에 둡니다.
6. 자식으로 ParticleSystem을 하나 만듭니다.
7. ParticleSystem의 `Main`에서 `Looping`을 끄고, `Duration`을 짧게 조정합니다.
8. `Emission`의 Burst 또는 `Rate over Time`을 조절해 입자 수를 맞춥니다.
9. 아래 스크립트를 부모 GameObject에 붙이고 참조를 연결합니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class EffectPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip effectClip;
    [SerializeField] private ParticleSystem effectParticles;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            effectParticles.Play();
            audioSource.PlayOneShot(effectClip);
        }
    }
}
```

</details>

### 실행해보면

스페이스바를 누를 때마다 파티클이 재생되고 효과음이 한 번 들립니다. 파티클의 Duration과 사운드 길이가 비슷하면 반응이 더 자연스럽습니다. 소리가 들리지 않으면 AudioSource의 Clip, Volume, Play On Awake 설정뿐 아니라 Main Camera의 Audio Listener도 함께 확인합니다.

### 생각해보기

1. 공격 효과음이 너무 늦게 들리면 플레이어는 어떤 느낌을 받을까요?
2. 같은 파티클이라도 색과 크기를 바꾸면 어떤 상황에 재사용할 수 있을까요?
3. UI 버튼음은 2D 사운드와 3D 사운드 중 어느 쪽이 더 자연스러울까요?
4. 폭발 효과를 만들 때 `Emission`의 Burst를 쓰면 어떤 장점이 있을까요?

## 오늘의 정리

- AudioSource는 사운드를 재생하는 컴포넌트입니다.
- Audio Listener는 씬의 소리를 듣는 귀 역할을 하며, 보통 Main Camera에 하나만 둡니다.
- AudioSource의 Play On Awake, Loop, Volume, Pitch, Spatial Blend를 상황에 맞게 조절합니다.
- ParticleSystem은 Main, Emission, Shape, Color over Lifetime, Size over Lifetime, Renderer 같은 핵심 모듈로 시각 효과를 만듭니다.
- 소리와 파티클을 함께 재생하면 상호작용 피드백이 뚜렷해집니다.
