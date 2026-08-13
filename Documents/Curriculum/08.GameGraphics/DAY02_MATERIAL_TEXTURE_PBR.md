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

## 2. 실습용 PBR 텍스처 다운로드

아래 파일은 이 문서의 실습을 위해 만든 1024×1024 PNG 텍스처입니다. 같은 이름으로 시작하는 파일끼리 하나의 재질 세트입니다. 링크를 클릭해 파일을 열고 저장하거나, 압축 파일을 한 번에 내려받습니다.

- [전체 PBR 텍스처 세트 다운로드 (ZIP)](Assets/DAY02_PBR_Textures/DAY02_PBR_Texture_Set.zip)

| 재질 | Base Map | Normal Map | Metallic·Smoothness Map | Occlusion Map |
| :--- | :--- | :--- | :--- | :--- |
| 돌 | [Stone_BaseMap.png](Assets/DAY02_PBR_Textures/Stone_BaseMap.png) | [Stone_NormalMap.png](Assets/DAY02_PBR_Textures/Stone_NormalMap.png) | [Stone_MetallicSmoothness.png](Assets/DAY02_PBR_Textures/Stone_MetallicSmoothness.png) | [Stone_Occlusion.png](Assets/DAY02_PBR_Textures/Stone_Occlusion.png) |
| 금속 | [Metal_BaseMap.png](Assets/DAY02_PBR_Textures/Metal_BaseMap.png) | [Metal_NormalMap.png](Assets/DAY02_PBR_Textures/Metal_NormalMap.png) | [Metal_MetallicSmoothness.png](Assets/DAY02_PBR_Textures/Metal_MetallicSmoothness.png) | [Metal_Occlusion.png](Assets/DAY02_PBR_Textures/Metal_Occlusion.png) |

### 파일 이름으로 역할 구분하기

- `BaseMap`: 눈에 보이는 기본 색입니다. 색 정보이므로 `sRGB (Color Texture)`를 켭니다.
- `NormalMap`: 표면의 방향 정보를 담은 파란색 계열 이미지입니다. `Texture Type`을 `Normal map`으로 바꿉니다.
- `MetallicSmoothness`: RGB에는 금속성, Alpha에는 매끄러움이 들어 있습니다. 색이 아닌 수치 정보이므로 sRGB를 끕니다.
- `Occlusion`: 틈과 홈을 어둡게 만들어 입체감을 보강합니다. 색이 아닌 수치 정보이므로 sRGB를 끕니다.

### Unity 프로젝트로 임포트하기

1. ZIP 파일의 압축을 풀거나 필요한 PNG 파일을 저장합니다.
2. Unity Project 창에서 `Assets/GameGraphics/Textures/DAY02` 폴더를 만듭니다.
3. 내려받은 PNG 파일을 Project 창의 `DAY02` 폴더로 끌어 놓습니다.
4. `Stone_BaseMap`과 `Metal_BaseMap`을 함께 선택합니다. Inspector에서 `Texture Type`은 `Default`, `sRGB (Color Texture)`는 체크, `Wrap Mode`는 `Repeat`로 설정한 뒤 `Apply`를 누릅니다.
5. 이름이 `NormalMap`으로 끝나는 두 파일을 함께 선택합니다. `Texture Type`을 `Normal map`으로 바꾸고 `Fix Now`가 표시되면 누른 뒤 `Apply`를 누릅니다.
6. 이름이 `MetallicSmoothness` 또는 `Occlusion`으로 끝나는 파일은 `Texture Type`을 `Default`, `sRGB (Color Texture)`는 체크 해제로 설정합니다. `MetallicSmoothness` 파일은 `Alpha Source`가 `Input Texture Alpha`인지도 확인한 뒤 `Apply`를 누릅니다.

> Base Map은 "**색 사진**", 나머지 맵은 "**숫자가 적힌 설계도**"라고 생각하면 쉽습니다. 설계도에 sRGB 색 보정을 적용하면 원래 수치가 달라질 수 있습니다.

## 3. 실습: 같은 모델, 다른 재질

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

### 내려받은 텍스처를 Material에 연결하기

`Mat_Stone`과 `Mat_Metal`의 Shader를 `Universal Render Pipeline/Lit`으로 맞춘 뒤, 같은 접두사의 파일을 다음 슬롯에 끌어 놓습니다.

| 텍스처 이름 끝부분 | URP Lit 슬롯 | 먼저 관찰할 내용 |
| :--- | :--- | :--- |
| `_BaseMap` | `Base Map` | 돌의 색과 금속 패널의 색 |
| `_MetallicSmoothness` | `Metallic Map` | 돌은 금속성이 낮고, 금속은 금속성이 높다는 차이 |
| `_NormalMap` | `Normal Map` | 돌 틈, 패널 경계와 리벳의 굴곡 |
| `_Occlusion` | `Occlusion Map` | 돌 틈과 패널 경계가 더 분명해지는 변화 |

1. 먼저 Base Map만 연결한 상태를 확인합니다.
2. Metallic Map, Normal Map, Occlusion Map을 하나씩 추가합니다. 한 번에 모두 연결하면 어떤 맵이 어떤 변화를 만들었는지 구분하기 어렵습니다.
3. Material Inspector의 `Tiling`을 `X 2`, `Y 2`로 바꾸어 텍스처가 반복되는 모습도 확인합니다.
4. Metallic Map을 연결한 뒤 Smoothness는 먼저 `1`로 두어 파일 Alpha의 결과를 보고, 이후 값을 낮추며 반사의 선명도가 어떻게 달라지는지 비교합니다.
5. Normal Map 효과가 너무 강하면 슬롯 오른쪽의 강도를 낮추고, 너무 약하면 높입니다. 모델의 꼭짓점 수는 그대로라는 점도 함께 확인합니다.

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
