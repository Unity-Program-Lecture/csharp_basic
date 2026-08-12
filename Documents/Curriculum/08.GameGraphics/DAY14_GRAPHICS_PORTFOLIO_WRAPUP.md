# DAY 14: 그래픽 포트폴리오 통합

오늘의 목표는 지금까지 만든 셰이더와 이펙트를 하나의 작은 게임 장면에 묶어, NCS 기준의 셰이더 프로그래밍과 이펙트 프로그래밍 역량을 확인하는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기, 이펙트 프로그래밍하기
- 관련 학습 내용: 셰이더 적용, 이펙트 작성, 코드 출력, 테스트와 디버깅
- Unity 6 재구성: Shader Graph, Particle System, Visual Effect Graph, C# 이벤트 연동을 통합합니다.

## 1. 최종 씬 요구 사항

| 구분 | 필수 구현 |
| :--- | :--- |
| 셰이더 | Shader Graph 기반 머티리얼 1개 이상 |
| 표면 표현 | Emission, Fresnel, UV Animation, Noise 중 2개 이상 |
| 이펙트 | Particle System 이펙트 2개 이상 |
| VFX Graph | Visual Effect Graph 이펙트 1개 이상 |
| 코드 연동 | 입력, 충돌, 상태 변화 중 1개 이상에 이펙트 연결 |
| 공간 배치 분석 | 이펙트 1개 이상에 발생 위치, 방향, 크기, 카메라 거리, 지속 시간을 기록 |
| 플레이 규칙 통합 | 이펙트가 어떤 게임 사건에서 한 번 재생되는지 설명 |
| 검증 기록 | Inspector·Graph·Play Mode 확인 결과를 표로 작성 |

## 2. 권장 주제

| 주제 | 설명 |
| :--- | :--- |
| 마법 훈련장 | 보호막 셰이더, 마법탄 히트 이펙트, 에너지 VFX |
| 용암 던전 | 용암 Shader Graph, 불꽃 파티클, 열기 VFX |
| SF 실험실 | 홀로그램 셰이더, 스파크 파티클, 전기장 VFX |

## 3. 최종 씬 조립 순서

1. `GraphicsLab`을 `GraphicsPortfolio`로 `File > Save As`하여, 이전 DAY의 비교용 씬을 보존합니다.
2. Hierarchy에 `Environment`, `ShaderTargets`, `ParticleEffects`, `VfxEffects`, `EffectInput` 빈 GameObject를 만들고 역할별 오브젝트를 아래에 정리합니다. 폴더가 아니라 Hierarchy의 부모 오브젝트이므로 런타임에 필요한 컴포넌트는 해당 자식에 붙입니다.
3. `ShaderTargets` 아래 Sphere·Capsule 같은 대상에 Material을 연결하고, 각 Mesh Renderer의 Material 슬롯과 Material Inspector의 Exposed Property를 확인합니다.
4. `ParticleEffects`에는 DAY 10의 Prefab 인스턴스를 배치하고, 한 번 재생되는 이펙트는 Looping이 꺼져 있는지 확인합니다. `VfxEffects`에는 Visual Effect GameObject를 배치하고 Asset·Bounds·SpawnRate를 확인합니다.
5. `EffectInput`에는 DAY 11·13의 PlayerInput과 Controller를 두고, Camera·Effect Prefab·Visual Effect 참조와 Action Map을 각각 연결합니다.
6. Play Mode에서 Shader 표현, Particle System, VFX Graph, 입력 사건을 하나씩 확인한 뒤 마지막에 함께 실행합니다. 문제가 나면 모든 값을 동시에 바꾸지 말고 해당 영역의 Inspector 연결부터 다시 확인합니다.

## 4. 발표 구성

1. 어떤 화면 스타일을 목표로 했는지 설명합니다.
2. Shader Graph에서 사용한 주요 노드를 설명합니다.
3. Particle System 또는 VFX Graph의 Spawn, Lifetime, Color, Size 설정을 설명합니다.
4. 코드가 어떤 사건에서 이펙트를 재생하는지 설명합니다.
5. 성능을 위해 어떤 값을 조절할 수 있는지 설명합니다.
6. 이펙트의 위치, 방향, 크기, 지속 시간이 게임 공간과 플레이 규칙에 맞는 이유를 설명합니다.

## 5. 최종 통합 테스트

최종 씬에서는 이펙트가 "보인다"만 확인하지 말고, 게임 규칙과 함께 확인합니다.

| 확인 항목 | 기대 결과 |
| :--- | :--- |
| 공간 배치 | 이펙트가 대상 또는 충돌 위치에서 의도한 방향으로 재생된다. |
| 플레이 규칙 | 공격, 피격, 회복, 목표 달성 등 정한 사건에서만 재생된다. |
| 중복 방지 | 같은 사건이 한 번 발생했을 때 이펙트가 불필요하게 여러 번 생성되지 않는다. |
| 시각 전달 | 카메라 거리에서 이펙트가 보이되 조작 대상과 HUD를 가리지 않는다. |
| 성능 | 입자 수, Lifetime, Spawn Rate, Bounds 중 조절한 값을 설명할 수 있다. |

## 최종 Inspector·실행 검증 순서

최종 씬을 열고 먼저 URP Asset, Main Camera, Directional Light, Volume을 확인합니다. 다음으로 주요 오브젝트의 Mesh Renderer에 Shader Graph Material이 연결됐는지, Material Inspector의 Exposed Property가 의도한 값인지 확인합니다. Graph에서는 최종 표현을 만드는 핵심 노드 세 개 이상과 각 노드의 입력·출력을 설명할 수 있어야 합니다.

Particle System Prefab은 Looping, Burst, Duration, Start Lifetime, Renderer Material을 확인하고, VFX Graph는 Spawn·Initialize·Update·Output이 이어지는지와 Visual Effect 컴포넌트의 Asset·Bounds·Exposed Property를 확인합니다. 이펙트가 특정 게임 사건에서 한 번만 재생되는지 Play Mode에서 확인하고, 관련 C# 스크립트의 Prefab 참조·LayerMask·이벤트 등록 상태를 읽어 봅니다.

제출 설명서에는 화면 이미지 대신 다음을 표로 기록합니다: 씬 이름, URP·Quality 설정, 주요 Material과 Exposed Property 값, Particle System 모듈 값, VFX Graph의 SpawnRate·Lifetime·Bounds, 이펙트 발생 조건, 저사양에서 줄이거나 끌 값, 실제 실행 확인 결과. 이 기록으로 다른 사람이 같은 Inspector 상태를 재현할 수 있어야 합니다.

## 오늘의 정리

- 그래픽 프로그래밍은 화면을 예쁘게 만드는 작업이면서 동시에 게임 상태를 읽히게 만드는 작업입니다.
- 셰이더는 표면의 규칙을 만들고, 이펙트는 사건의 순간을 강조합니다.
- 좋은 포트폴리오는 화려함보다 의도, 구현, 테스트 결과가 분명해야 합니다.
- 이펙트는 3D 공간과 게임 규칙 안에서 재생될 때 비로소 장면의 의미를 강화합니다.
