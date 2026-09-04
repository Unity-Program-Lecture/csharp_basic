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

## 4. Shader Graph 포트폴리오 표현 완성하기

DAY 14에서는 새 Graph를 무작정 많이 만들지 않습니다. DAY 05의 표면 표현, DAY 06의 정점·UV 애니메이션, DAY 08의 비실사 표현 중 하나를 골라 장면의 역할에 맞게 완성합니다. 원본 Graph는 보존하고, 포트폴리오용 복제본에서만 값을 바꿉니다.

| 장면 주제 | 시작 Graph 예시 | 포트폴리오에서 보강할 연결 | 전달할 게임 상태 |
| :--- | :--- | :--- | :--- |
| 마법 훈련장 | `SG_Shield` 또는 `SG_ToonRim` | Fresnel × 색 × Emission | 보호·선택·피격 대상을 눈에 띄게 합니다. |
| 용암 던전 | `SG_Lava` | Noise·Time·Lerp·Emission | 위험 지역의 열기와 흐름을 보여 줍니다. |
| SF 실험실 | `SG_ToonBand` 또는 `SG_OutlineShell` | Dot Product·Step 또는 Vertex Position Offset | 중요한 장치·캐릭터의 실루엣을 강조합니다. |

### Material과 Graph를 안전하게 분리하기

1. 사용할 Graph Asset을 복제해 `SG_Portfolio_주제`처럼 이름을 바꿉니다. DAY 05~08의 원본은 비교 자료로 남깁니다.
2. 복제 Graph를 열고 Blackboard에서 포트폴리오용 Property를 확인합니다. Color, Float, Texture2D 중 현재 장면에서 실제로 조절할 값만 `Exposed`로 둡니다.
3. 저장 후 `Mat_Portfolio_주제` Material을 새로 만듭니다. 기존 Day Material을 직접 바꾸면 이전 실습 장면까지 함께 달라질 수 있습니다.
4. Material의 Shader가 복제한 Graph인지 확인한 뒤, 대상 Mesh Renderer의 정확한 Material 슬롯에 연결합니다.
5. Inspector에서 Property를 하나씩 바꾸고, Graph의 어느 연결이 그 값을 받는지 역방향으로 따라갑니다.

### Graph를 읽고 하나씩 완성하기

Graph는 Master Stack의 최종 출력에서 시작해 거꾸로 읽습니다. 아래 세 가지 중 장면에 필요한 한 가지 이상을 완성합니다.

| 최종 출력 | 연결 예시 | 확인할 결과 |
| :--- | :--- | :--- |
| Fragment `Base Color` | `Lerp(어두운 색, 밝은 색, Step 또는 Noise)` | 표면 색 구역 또는 무늬가 바뀝니다. |
| Fragment `Emission` | `Fresnel 또는 Noise × Color × Intensity` | 빛나는 가장자리·균열·에너지 부분이 보입니다. |
| Vertex `Position` | `Position (Object) + Normal (Object) × Offset` | Mesh 실루엣 또는 Outline Shell 위치가 바뀝니다. |

1. `Base Color` 또는 `Emission`을 쓸 때는 먼저 원본 색 하나만 연결해 Material·Mesh Renderer 연결이 맞는지 확인합니다.
2. 다음으로 Time, Noise, Fresnel, Step 중 필요한 계산 노드를 하나만 추가하고 Preview와 Game View의 변화를 확인합니다.
3. 마지막에 Blackboard Property를 연결해 Material Inspector에서 속도·세기·색을 조절합니다.
4. Vertex Position을 쓸 때는 `Position`과 Offset의 Space가 모두 Object인지 확인합니다. World Space Offset을 그대로 더지 않습니다.

### 포트폴리오 Shader Graph 확인표

| 확인 항목 | 기록할 내용 | 기대 결과 |
| :--- | :--- | :--- |
| 대상 | Graph Asset, Material, 적용한 GameObject | 어느 오브젝트에 어떤 Graph가 쓰였는지 재현할 수 있습니다. |
| 입력 | Exposed Property 이름·시작값 | Material Inspector에서 같은 값을 찾을 수 있습니다. |
| 연결 | 최종 출력까지의 핵심 노드 3개 이상 | 값이 어떤 순서로 바뀌는지 설명할 수 있습니다. |
| 결과 | Base Color, Emission, Vertex Position 중 사용한 출력 | 색·빛·실루엣 중 무엇이 달라지는지 분명합니다. |
| Play Mode | Camera 거리에서의 관찰 결과 | 게임 중에도 대상과 효과가 구분됩니다. |

## 5. 발표 구성

1. 어떤 화면 스타일을 목표로 했는지 설명합니다.
2. Shader Graph에서 사용한 주요 노드를 설명합니다.
3. Particle System 또는 VFX Graph의 Spawn, Lifetime, Color, Size 설정을 설명합니다.
4. 코드가 어떤 사건에서 이펙트를 재생하는지 설명합니다.
5. 성능을 위해 어떤 값을 조절할 수 있는지 설명합니다.
6. 이펙트의 위치, 방향, 크기, 지속 시간이 게임 공간과 플레이 규칙에 맞는 이유를 설명합니다.

## 6. 최종 통합 테스트

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
