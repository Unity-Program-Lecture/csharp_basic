# 🚀 Day 12: 사운드와 파티클 시스템 (Audio & VFX)

오늘의 목표는 "**게임의 생동감을 불어넣는 사운드 효과와 시각 효과(VFX)를 유니티 엔진 기능을 활용하여 구현한다**"입니다.

---

## 1. 오디오 시스템 (Audio System)
1. **Audio Listener**: 소리를 듣는 귀. (보통 메인 카메라에 1개 존재)
2. **Audio Source**: 소리를 내는 스피커.
3. **Audio Clip**: 실제 음원 파일.

---

## 2. 파티클 시스템 (Particle System)
불, 연기, 마법 효과 등 대량의 작은 입자를 생성하여 표현하는 시각 효과입니다.
- **Emitter**: 입자가 뿜어져 나오는 형태 결정.
- **Shape**: 입자가 생성되는 모양 (원뿔, 상자 등).
- **LifeTime**: 입자가 유지되는 시간.

---

## 💻 실습 예제: 폭발 효과와 사운드 동시 실행
```csharp
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public ParticleSystem vfx;
    public AudioSource audioSource;

    public void Explode()
    {
        // 1. 시각 효과 실행
        vfx.Play();

        // 2. 사운드 효과 실행
        audioSource.Play();

        Debug.Log("펑!! 효과 발생");
    }
}
```

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 유니티에서 실제 소리 파일을 담고 있는 에셋의 명칭은?
   - **정답:** 오디오 클립 (Audio Clip)
2. **문제:** 마법이나 불꽃처럼 수많은 작은 입자들을 조절하여 만드는 시각 효과 시스템의 이름은?
   - **정답:** 파티클 시스템 (Particle System)
