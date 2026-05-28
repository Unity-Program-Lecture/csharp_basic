# DAY 12: 사운드와 파티클 효과

오늘의 목표는 사운드와 VFX를 "**게임의 손맛을 알려 주는 반응**"으로 이해하고, AudioSource와 ParticleSystem을 함께 재생하는 방법을 익히는 것입니다.

## 1. 핵심 개념: "보이는 반응과 들리는 반응"

플레이어가 버튼을 누르거나 공격이 맞았을 때 아무 반응이 없으면 동작이 어색합니다. 사운드는 귀로 결과를 알려 주고, 파티클은 눈으로 결과를 보여 줍니다. 둘을 함께 쓰면 작은 상호작용도 훨씬 분명해집니다.

### 이 단어는 무슨 뜻인가요?

- **AudioClip**: 재생할 사운드 파일입니다.
- **AudioSource**: AudioClip을 실제로 재생하는 컴포넌트입니다.
- **ParticleSystem**: 불꽃, 연기, 반짝임 같은 입자 효과를 만드는 컴포넌트입니다.
- **Play One Shot**: 같은 AudioSource에서 짧은 효과음을 한 번 재생하는 방식입니다.

## 실습 예제: 효과음과 파티클 동시에 실행하기

**미션:** 스페이스바를 누르면 파티클과 효과음이 동시에 재생되도록 합니다.

1. 빈 GameObject에 `AudioSource`를 붙입니다.
2. 자식으로 ParticleSystem을 하나 만듭니다.
3. 아래 스크립트를 부모 GameObject에 붙이고 참조를 연결합니다.

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

스페이스바를 누를 때마다 파티클이 재생되고 효과음이 한 번 들립니다. 파티클의 Duration과 사운드 길이가 비슷하면 반응이 더 자연스럽습니다.

### 생각해보기

1. 공격 효과음이 너무 늦게 들리면 플레이어는 어떤 느낌을 받을까요?
2. 같은 파티클이라도 색과 크기를 바꾸면 어떤 상황에 재사용할 수 있을까요?

## 오늘의 정리

- AudioSource는 사운드를 재생하는 컴포넌트입니다.
- ParticleSystem은 시각 효과를 만드는 컴포넌트입니다.
- 소리와 파티클을 함께 재생하면 상호작용 피드백이 뚜렷해집니다.
