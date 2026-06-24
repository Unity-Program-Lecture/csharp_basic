# DAY 10: Particle System 모듈과 프리팹화

오늘의 목표는 이펙트를 한 번 만들고 끝내는 것이 아니라 "**필요할 때 꺼내 쓰는 도구 상자**"로 정리하는 것입니다.

## NCS 연결

- 능력단위 요소: 이펙트 프로그래밍하기
- 관련 학습 내용: 게임 엔진에서 이펙트 작성, 프리팹화, 테스트
- Unity 6 재구성: Particle System을 Prefab으로 만들고 여러 상황에서 재사용합니다.

## 1. 이펙트 프리팹이 필요한 이유

공격, 피격, 회복, 폭발 같은 효과는 여러 곳에서 반복됩니다. 씬마다 새로 만들면 설정이 달라지고 수정하기 어렵습니다. Prefab으로 만들면 같은 이펙트를 안정적으로 재사용할 수 있습니다.

## 2. 실습: 이펙트 프리팹 3종 만들기

| 프리팹 | 목적 | 핵심 설정 |
| :--- | :--- | :--- |
| `FX_HitSpark` | 공격 적중 | 짧은 Lifetime, 빠른 Speed |
| `FX_HealGlow` | 회복 | 초록색, 천천히 상승 |
| `FX_ExplosionSmall` | 폭발 | Burst Emission, 큰 Size 변화 |

## 3. 프리팹 점검표

- Looping이 의도에 맞게 켜져 있나요?
- Play On Awake가 필요한가요?
- 재생이 끝난 뒤 제거할 방법이 있나요?
- 머티리얼이 누락되지 않았나요?
- 씬 스케일과 이펙트 크기가 맞나요?

## 스크린샷 체크포인트

- `Images/day10_effect_prefabs.png`: Project 창의 이펙트 프리팹 3종
- `Images/day10_effect_prefab_inspector.png`: Prefab Inspector와 Particle System 설정

## 오늘의 정리

- 이펙트는 Prefab으로 정리해야 반복 사용과 수정이 쉽습니다.
- 이펙트의 시작, 지속, 종료 시점을 명확히 정해야 합니다.
- 다음 시간에는 C# 코드로 이펙트를 재생합니다.

