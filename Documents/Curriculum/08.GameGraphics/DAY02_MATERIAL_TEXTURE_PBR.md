# DAY 02: 머티리얼, 텍스처, PBR 기초

오늘의 목표는 머티리얼을 "**오브젝트가 입는 옷감의 성질표**"로 이해하고, 색뿐 아니라 금속성, 거칠기, 노멀을 조절해 표면의 느낌을 만드는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 물리 기반 렌더링, 렌더링 품질 향상
- Unity 6 재구성: URP Lit 머티리얼과 텍스처 슬롯을 사용합니다.

## 1. PBR은 왜 필요한가요?

PBR은 Physically Based Rendering의 줄임말입니다. 빛이 표면에 닿을 때 현실과 비슷한 규칙으로 반사되도록 만드는 렌더링 방식입니다. 나무, 금속, 고무, 물은 색만 다른 것이 아니라 빛을 반사하는 방식도 다릅니다.

### 이 단어는 무슨 뜻인가요?

- **Albedo/Base Map**: 표면의 기본 색 또는 색 텍스처입니다.
- **Metallic**: 표면이 금속처럼 빛을 반사하는 정도입니다.
- **Smoothness**: 표면이 매끈해서 반사가 또렷한 정도입니다.
- **Normal Map**: 실제 모델을 울퉁불퉁하게 만들지 않고 표면의 굴곡처럼 보이게 하는 텍스처입니다.
- **Emission**: 오브젝트 자체가 빛나는 것처럼 보이게 하는 색입니다.

## 2. 실습: 같은 모델, 다른 재질

1. Sphere 4개를 나란히 배치합니다.
2. `Mat_Stone`, `Mat_Metal`, `Mat_Plastic`, `Mat_Glow` 머티리얼을 만듭니다.
3. Metallic과 Smoothness 값을 바꾸며 빛 반사를 관찰합니다.
4. Emission을 켠 머티리얼은 어두운 배경에서 확인합니다.

### Material 만들기와 적용 순서

1. Project 창의 `GameGraphics/Materials` 폴더를 열고 빈 곳에서 우클릭한 뒤 `Create > Material`을 선택합니다.
2. 이름을 `Mat_Stone`, `Mat_Metal`, `Mat_Plastic`, `Mat_Glow`로 바꿉니다. Material을 복제할 때는 이름과 값이 함께 복사되므로, 네 개를 먼저 만든 뒤 각각 Inspector 값을 바꾸는 편이 안전합니다.
3. Material을 선택해 Inspector 맨 위 `Shader`가 `Universal Render Pipeline/Lit`인지 확인합니다. 다른 Shader라면 Shader 드롭다운에서 같은 항목을 선택합니다.
4. Hierarchy에서 Sphere를 선택하고 `Mesh Renderer > Materials`를 펼칩니다. `Element 0` 슬롯에 Material을 Project 창에서 끌어 놓습니다.
5. 네 Sphere를 모두 배치한 뒤에는 Camera·Directional Light를 고정합니다. 같은 조명 조건에서 한 Material의 한 값만 바꾸어야 표면 차이를 비교할 수 있습니다.

## Inspector 핵심 값

| 속성 | 낮을 때 | 높을 때 |
| :--- | :--- | :--- |
| Metallic | 플라스틱, 돌, 천 느낌 | 금속 느낌 |
| Smoothness | 빛이 넓게 퍼짐 | 반사가 또렷함 |
| Normal Strength | 평평해 보임 | 표면 굴곡이 강함 |
| Emission | 스스로 빛나지 않음 | 발광체처럼 보임 |

## URP Lit 머티리얼 Inspector 읽는 순서

Project 창에서 만든 Material을 선택한 뒤 Inspector를 위에서 아래로 읽습니다. `Shader`가 `Universal Render Pipeline/Lit`인지 먼저 확인하고, 다음으로 `Surface Inputs`의 Base Map 색·텍스처, Metallic Map, Smoothness, Normal Map, Emission을 봅니다. Material 값을 바꿨는데 Sphere가 달라지지 않으면 Sphere의 `Mesh Renderer > Materials > Element 0`에 이 Material이 실제로 연결됐는지 확인합니다.

| 비교 대상 | Inspector에서 바꿀 값 | 관찰할 결과 |
| :--- | :--- | :--- |
| 고무 공 | Metallic `0` 부근, Smoothness 낮음 | 넓고 흐린 반사, 거친 표면 |
| 금속 공 | Metallic `1` 부근, Smoothness 높음 | 주변 빛과 색을 강하게 반사 |
| 발광 표지 | Emission 체크, HDR Color와 강도 | 조명이 약해도 자체 발광 색이 남음 |

Normal Map을 쓸 때는 텍스처 Inspector의 Texture Type을 `Normal map`으로 바꾼 뒤 `Fix Now` 또는 Apply를 누릅니다. Normal Map 슬롯에 텍스처를 넣었는데 효과가 지나치게 약하거나 반대로 보이면 Texture Type과 Normal Map 강도를 먼저 확인합니다. Metallic·AO처럼 색이 아닌 수치 정보를 담은 맵은 Import Settings의 sRGB 설정도 프로젝트 규칙과 맞는지 확인합니다.

## 생각해보기

1. 빨간색 금속 공과 빨간색 고무 공은 어떤 값이 달라야 할까요?
2. Normal Map은 모델의 실제 꼭짓점 수를 늘리지 않는데도 왜 울퉁불퉁해 보일까요?

## 오늘의 정리

- 머티리얼은 색뿐 아니라 빛 반사 규칙을 함께 담습니다.
- PBR 값은 게임의 사실감과 스타일을 결정합니다.
- 다음 시간에는 라이트, 그림자, 카메라 후처리로 장면 분위기를 만듭니다.
