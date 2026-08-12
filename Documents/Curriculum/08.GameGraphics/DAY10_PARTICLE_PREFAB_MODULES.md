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

### 테스트 이펙트를 Prefab으로 바꾸는 순서

1. DAY 09에서 만든 테스트 Particle System을 Hierarchy에서 선택하고 이름을 `FX_HitSpark`처럼 역할 중심으로 정합니다.
2. Project 창에 `GameGraphics/Prefabs/Effects` 폴더를 만들고, Hierarchy의 이펙트 오브젝트를 그 폴더로 끌어 놓습니다. Project 창에 파란 Prefab Asset이 생기면 생성이 완료된 것입니다.
3. Prefab Asset을 더블 클릭해 Prefab Mode로 엽니다. 이 상태에서 Main, Emission, Shape, Renderer 모듈을 수정하면 모든 씬 인스턴스의 공통 기준을 바꿀 수 있습니다.
4. Prefab Mode를 나와 씬에 Prefab을 두 개 배치합니다. 한쪽 인스턴스만 Inspector에서 바꿨다면 해당 항목이 Override로 표시되는지 확인하고, 공통 수정이 필요하면 `Overrides > Apply All`이 아니라 필요한 항목만 검토한 뒤 적용합니다.
5. `FX_HealGlow`, `FX_ExplosionSmall`도 같은 방식으로 만들되, 원본 `FX_HitSpark`를 복제할 때는 Emission·Color·Lifetime·Renderer Material을 각 효과 목적에 맞게 반드시 바꿉니다.

## 3. 프리팹 점검표

- Looping이 의도에 맞게 켜져 있나요?
- Play On Awake가 필요한가요?
- 재생이 끝난 뒤 제거할 방법이 있나요?
- 머티리얼이 누락되지 않았나요?
- 씬 스케일과 이펙트 크기가 맞나요?

## Effect Prefab을 Inspector에서 검증하기

Project 창의 `Prefabs/Effects` 폴더에 `HitEffect`, `HealEffect`, `ExplosionEffect`처럼 역할이 드러나는 이름으로 Prefab을 만듭니다. Prefab Mode로 열어 Particle System Main 모듈의 Looping, Duration, Start Lifetime과 Emission의 Bursts를 확인합니다. 한 번만 발생하는 Prefab은 Looping이 꺼져 있어야 하며, 재생 시간이 끝난 뒤에도 남아 있으면 코드에서 Destroy하는 시간과 Main Duration·Start Lifetime의 합을 비교합니다.

Prefab의 Transform Scale은 `(1, 1, 1)`을 기본으로 두고, 크기 조절은 먼저 Start Size 또는 Size over Lifetime에서 합니다. Scale로만 키우면 다른 씬 Scale에서 효과가 과도하게 커져 비교가 어려워집니다. Renderer 모듈의 Material과 정렬 결과를 확인하고, 같은 Prefab을 서로 다른 위치에 배치해도 방향·크기·재생 시간이 같은 규칙으로 유지되는지 Play Mode에서 확인합니다.

Prefab을 수정한 뒤에는 Prefab Mode에서 변경이 저장됐는지와 씬 인스턴스가 Override 상태인지 확인합니다. 테스트용 인스턴스만 바꾼 값을 원본 규칙으로 착각하지 않도록, 공통 수정은 Prefab Asset에 적용하고 상황별 차이는 명시적으로 Override합니다.

## 오늘의 정리

- 이펙트는 Prefab으로 정리해야 반복 사용과 수정이 쉽습니다.
- 이펙트의 시작, 지속, 종료 시점을 명확히 정해야 합니다.
- 다음 시간에는 C# 코드로 이펙트를 재생합니다.
