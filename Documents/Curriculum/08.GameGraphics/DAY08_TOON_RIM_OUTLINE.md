# DAY 08: 비실사 렌더링과 후처리 표현

오늘의 목표는 현실 같은 그래픽만 좋은 그래픽이 아니라, 게임의 장르와 감정에 맞는 "**의도된 화면 스타일**"이 중요하다는 점을 배우는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 비실사 렌더링, 셰이더로 게임 개성 표현
- Unity 6 재구성: Toon, Rim Light, Outline, 후처리 색감 조절을 실습합니다.

## 1. 비실사 렌더링이란?

비실사 렌더링은 현실처럼 보이기보다 만화, 애니메이션, 일러스트 같은 느낌을 목표로 하는 렌더링입니다. 셰이더는 게임의 "화풍"을 만드는 도구가 될 수 있습니다.

### 주요 표현

| 표현 | 설명 |
| :--- | :--- |
| Toon Shading | 음영을 부드럽게 섞지 않고 단계적으로 나눕니다. |
| Rim Light | 가장자리에 빛을 둘러 캐릭터를 돋보이게 합니다. |
| Outline | 외곽선을 추가해 만화 같은 실루엣을 만듭니다. |
| Color Grading | 장면 전체 색감과 대비를 조절합니다. |

## 2. 실습: 캐릭터 강조 머티리얼

1. 캐릭터 또는 Capsule을 준비합니다.
2. Shader Graph에서 Normal Vector와 View Direction을 사용합니다.
3. Fresnel Effect로 Rim Light를 만듭니다.
4. 색 보정 Volume을 추가해 장면 전체 분위기를 맞춥니다.

### 실습 대상과 설정 만들기

1. Hierarchy에서 `GameObject > 3D Object > Capsule`을 만들고 `ToonCharacter`로 이름을 바꿉니다. Capsule의 Mesh Renderer는 DAY 04~05의 Material을 연결할 대상으로 사용합니다.
2. `SG_Shield`를 바로 수정하지 않고 `SG_ToonRim`으로 복제합니다. Project 창에서 Graph를 복제한 뒤 이름을 바꾸고 더블 클릭해 엽니다.
3. Blackboard에서 `RimColor`, `RimPower`, `RimIntensity`를 Exposed Property로 만들고, Graph Inspector에서 Target이 URP인지 확인합니다. Rim만 표현할 때는 불투명 Material을 유지하고, 투명 표현이 필요한 경우에만 Surface Type을 Transparent로 바꿉니다.
4. `Mat_ToonRim`을 만들고 `ToonCharacter > Mesh Renderer > Materials > Element 0`에 연결합니다. Material Inspector에서 Rim 값을 한 번에 하나씩 바꿔 Game View의 외곽 밝기 변화를 확인합니다.
5. Color Grading은 DAY 03에서 만든 Global Volume의 Profile에 추가합니다. 셰이더의 Rim 값이 맞는지 먼저 확인한 뒤 Volume의 Contrast·Color Filter를 조절합니다.

## Toon·Rim Light 설정 확인 절차

Rim Light는 `Normal Vector`와 `View Direction`의 Dot Product를 만든 뒤 One Minus 또는 Power로 가장자리 범위를 좁혀 만듭니다. Graph Inspector에서 Target이 URP인지와 Surface 설정을 먼저 확인합니다. Power 값이 높을수록 얇고 선명한 테두리 빛이 되고, Rim Color와 Intensity는 Blackboard에서 Exposed로 만들어 Material Inspector에서 조절합니다. 결과는 Emission에 연결해 조명 방향이 달라도 가독성용 가장자리 빛이 남는지 확인합니다.

Toon 단계는 빛의 연속적인 값을 Step 또는 비교 노드로 구간화합니다. 단계 수가 너무 적으면 표면이 지나치게 딱딱해지고, 너무 많으면 일반 Lit 표현과 차이가 줄어듭니다. Outline은 메시를 바깥으로 확장하는 방식이라면 Cull, Surface 설정, 메시 Scale에 따라 두께가 달라질 수 있으므로 작은 Sphere와 실제 캐릭터 크기 모두에서 확인합니다. Volume 색 보정은 마지막에 적용해, 셰이더 자체의 색 오류와 후처리 효과를 구분합니다.

## 오늘의 정리

- 셰이더는 게임의 화풍을 만드는 도구입니다.
- Rim Light와 Outline은 캐릭터 가독성을 높이는 데 유용합니다.
- 다음 시간부터는 이펙트 프로그래밍으로 넘어가 Particle System을 다룹니다.
