# DAY 09: Particle System 기초

오늘의 목표는 이펙트를 "**작은 조각을 많이 뿌려 하나의 현상처럼 보이게 하는 기술**"로 이해하는 것입니다.

## NCS 연결

- 능력단위 요소: 이펙트 프로그래밍하기
- 관련 학습 내용: 게임 이펙트 구성 방법 이해 및 사용
- Unity 6 재구성: Built-in Particle System으로 폭발, 먼지, 히트 효과를 만듭니다.

## 1. Particle은 무엇인가요?

불꽃, 연기, 먼지, 마법 가루처럼 작은 이미지나 메시가 많이 모여 하나의 현상처럼 보이는 효과를 파티클 이펙트라고 합니다.

### 이 단어는 무슨 뜻인가요?

- **Particle**: 화면에 뿌려지는 작은 입자입니다.
- **Emitter**: 파티클이 생성되는 위치와 모양입니다.
- **Lifetime**: 파티클 하나가 살아 있는 시간입니다.
- **Emission**: 파티클이 얼마나 자주 생성되는지 정하는 설정입니다.
- **Renderer**: 파티클이 어떤 이미지나 메시로 보일지 정합니다.

## 2. 실습: 히트 이펙트 만들기

1. 빈 GameObject를 만들고 Particle System을 추가합니다.
2. Duration을 짧게 설정하고 Looping을 끕니다.
3. Start Lifetime, Start Speed, Start Size를 조절합니다.
4. Color over Lifetime으로 처음은 밝고 끝은 투명하게 만듭니다.
5. Shape를 Sphere 또는 Cone으로 바꿔 퍼지는 방향을 조절합니다.

## 주요 모듈

| 모듈 | 역할 |
| :--- | :--- |
| Main | 기본 시간, 속도, 크기 |
| Emission | 생성 개수와 빈도 |
| Shape | 생성 위치와 방향 |
| Color over Lifetime | 시간에 따른 색 변화 |
| Size over Lifetime | 시간에 따른 크기 변화 |
| Renderer | 표시 방식과 머티리얼 |

## 스크린샷 체크포인트

- `Images/day09_particle_system_modules.png`: Particle System 모듈이 펼쳐진 Inspector
- `Images/day09_hit_effect_scene.png`: 히트 이펙트가 재생되는 Scene 또는 Game 화면

## 오늘의 정리

- 파티클은 작은 입자 여러 개로 큰 현상을 표현합니다.
- Particle System은 Inspector 설정만으로도 많은 이펙트를 만들 수 있습니다.
- 다음 시간에는 파티클 이펙트를 프리팹으로 만들고 재사용합니다.

