# DAY 08: Shader Graph로 Toon, Rim Light, Outline 만들기

오늘의 목표는 DAY 04~07에서 배운 Shader Graph의 입력·연결·출력 흐름으로, 캐릭터가 배경에서 눈에 띄는 **비실사 표현**을 직접 만드는 것입니다. 오늘은 노드 이름을 많이 외우기보다, 각 Graph가 어떤 값을 만들고 최종적으로 어디에 연결하는지 확인합니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 비실사 렌더링, 셰이더로 게임 개성 표현
- Unity 6 재구성: URP Shader Graph로 Toon Band, Rim Light, Outline Shell과 전체 화면 Outline을 제작하고 Material에서 값을 조절합니다.

## 오늘의 완성 목표

Capsule 하나에 아래 세 가지 표현을 순서대로 적용한 뒤, 심화에서 화면 전체에 적용되는 Outline도 만듭니다.

| 표현 | 만들 Graph 또는 Material | 눈으로 확인할 결과 |
| :--- | :--- | :--- |
| Toon Band | `SG_ToonBand` | 표면 밝기가 부드럽게 섞이지 않고 밝은 면·어두운 면으로 나뉩니다. |
| Rim Light | `SG_ToonRim` | 카메라에서 옆으로 보이는 가장자리에 색 테두리 빛이 생깁니다. |
| Outline Shell | `SG_OutlineShell` | 원본 Capsule 바깥에 어두운 외곽선이 남습니다. |
| Screen Outline **(심화)** | `SG_ScreenOutline` | Capsule과 주변 불투명 오브젝트의 화면 경계에 같은 색 선이 생깁니다. |

Color Grading은 Shader Graph가 아니라 DAY 03에서 배운 Volume 기능입니다. Toon Band, Rim Light, 두 Outline 결과가 맞는지 먼저 확인한 뒤 마지막에 적용합니다.

## 1. 준비: 실습 대상과 폴더 만들기

1. `GraphicsLab` 씬을 열고, 결과를 분리하려면 `File > Save As`로 `Day08_NonPhotoreal` 씬으로 저장합니다.
2. Hierarchy에서 `GameObject > 3D Object > Capsule`을 만들고 `ToonCharacter`로 이름을 바꿉니다.
3. Project 창에 `GameGraphics/Day08/Graphs`, `GameGraphics/Day08/Materials` 폴더를 만듭니다.
4. Capsule을 선택하고 `Mesh Renderer`가 있는지 확인합니다. 이후 만든 Material은 모두 `Mesh Renderer > Materials > Element 0`에 연결합니다.
5. Scene View 또는 Main Camera를 Capsule의 정면이 아니라 약간 옆에서 보이도록 둡니다. Rim Light와 Outline은 옆면이 보여야 확인하기 쉽습니다.

> 이 문서는 정점이 고정된 Capsule을 기준으로 합니다. 애니메이션이 있는 Skinned Mesh의 Outline Shell은 본 애니메이션 동기화가 추가로 필요하므로, 먼저 Capsule에서 원리를 확인합니다.

## 2. Toon Band: 빛의 방향을 두 색으로 나누기

일반적인 Lit Shader Graph는 URP 조명을 자연스럽게 계산합니다. 이번 입문 실습은 계산 과정을 보이기 위해 **Unlit Shader Graph**에서 고정된 방향의 빛을 직접 만들고, 그 결과를 두 색 중 하나로 나눕니다. 따라서 실제 Directional Light를 자동으로 따라가는 완전한 Toon Lighting이 아니라, `LightDirectionWS`를 Material에서 조절하는 Toon Band입니다.

### Graph와 Material 만들기

1. Project 창에서 `Create > Shader Graph > URP > Unlit Shader Graph`를 선택하고 `SG_ToonBand`로 이름을 정합니다.
2. Graph를 열고 Graph Inspector에서 `Surface Type = Opaque`, `Render Face = Front`인지 확인합니다.
3. Blackboard에서 아래 Property를 만들고 모두 `Exposed`를 켭니다.
4. `Mat_ToonBand` Material을 만들고 Shader를 `Shader Graphs > SG_ToonBand`로 선택합니다.
5. `Mat_ToonBand`를 `ToonCharacter`의 Material 슬롯에 연결합니다.

| Property | 타입 | 시작값 | 역할 |
| :--- | :--- | :--- | :--- |
| `LitColor` | Color | 밝은 노랑 | 빛을 받는 면의 색입니다. |
| `ShadowColor` | Color | 어두운 보라 | 빛을 덜 받는 면의 색입니다. |
| `LightDirectionWS` | Vector3 | `(0.3, 0.8, 0.4)` | 빛이 오는 방향을 나타내는 월드 좌표 방향입니다. |
| `BandThreshold` | Float | `0.5` | 밝은 면으로 바뀌는 경계값입니다. |

### 노드를 한 줄씩 연결하기

`Normal Vector`는 표면이 향한 방향이고, `LightDirectionWS`는 빛이 오는 방향입니다. 두 방향이 비슷할수록 `Dot Product` 값이 커집니다. 그 값을 `Step`으로 두 구간으로 나누고, `Lerp`로 어두운 색 또는 밝은 색을 선택합니다.

1. `Normal Vector` 노드를 만들고 Space를 `World`로 설정합니다.
2. `Normalize` 노드를 만들고 `LightDirectionWS`를 연결합니다. Vector3 Property의 길이가 1이 아닐 수 있으므로 방향만 쓰기 위해 정규화합니다.
3. `Dot Product` 노드에 Normal Vector와 Normalize 결과를 연결합니다.
4. `Remap` 노드에서 In Min Max를 `(-1, 1)`, Out Min Max를 `(0, 1)`로 설정하고 Dot Product 결과를 연결합니다.
5. `Step` 노드의 Edge에 `BandThreshold`, In에 Remap 결과를 연결합니다. 결과는 0 또는 1에 가까운 값입니다.
6. `Lerp` 노드의 A에 `ShadowColor`, B에 `LitColor`, T에 Step 결과를 연결합니다.
7. Lerp 출력을 Master Stack Fragment의 `Base Color`에 연결합니다.

```text
Normal Vector (World) ───────────┐
                                 Dot Product ── Remap(-1~1 → 0~1) ── Step ── T
LightDirectionWS ── Normalize ──┘                                            │
ShadowColor ─────────────────────────────────────────────────────────────── Lerp ──> Base Color
LitColor ──────────────────────────────────────────────────────────────────┘
```

### 값으로 결과 확인하기

| 바꿀 값 | 먼저 시험할 값 | 예상 결과 |
| :--- | :--- | :--- |
| `BandThreshold` | `0.2` → `0.8` | 값이 커질수록 밝은 면이 줄어듭니다. |
| `LightDirectionWS` | X를 `-0.8` → `0.8` | 밝은 면이 Capsule의 좌우로 이동합니다. |
| `LitColor` | 노랑 → 흰색 | 밝은 면의 색만 바뀝니다. |
| `ShadowColor` | 보라 → 남색 | 어두운 면의 색만 바뀝니다. |

Capsule이 한 색으로만 보이면 먼저 `Normal Vector`의 Space가 World인지, `Lerp`의 T에 Step 출력이 연결됐는지 확인합니다. `LightDirectionWS`를 `(0, 0, 0)`으로 두면 방향이 없으므로 원하는 결과를 기대할 수 없습니다.

## 3. Rim Light: 카메라 가장자리에 빛 더하기

Rim Light는 카메라를 정면으로 향한 면보다 옆으로 보이는 면을 밝게 만듭니다. DAY 05의 보호막 Graph를 복제해도 되지만, 여기서는 Toon Band 위에 어떤 Graph 연결을 추가하는지 분명히 보기 위해 `SG_ToonRim`을 별도로 만듭니다.

### Graph와 Material 준비

1. `SG_ToonBand`를 복제하고 이름을 `SG_ToonRim`으로 바꿉니다. 원본 `SG_ToonBand`는 비교용으로 남깁니다.
2. Blackboard에 아래 Property를 추가하고 `Exposed`를 켭니다.
3. `Mat_ToonRim` Material을 만들고 `SG_ToonRim`을 선택합니다.
4. Capsule의 Material을 `Mat_ToonRim`으로 바꿉니다.

| 추가 Property | 타입 | 시작값 | 역할 |
| :--- | :--- | :--- | :--- |
| `RimColor` | Color | 청록 | 가장자리 빛의 색입니다. |
| `RimPower` | Float | `3` | 가장자리 빛의 폭을 조절합니다. |
| `RimIntensity` | Float | `2` | Rim Light의 밝기를 조절합니다. |

### Rim 노드 연결하기

1. `Fresnel Effect` 노드를 만듭니다. 기본 World Space Normal과 View Direction을 그대로 사용합니다.
2. `RimPower`를 Fresnel Effect의 Power에 연결합니다.
3. `RimColor`와 Fresnel 출력값을 `Multiply`로 곱합니다.
4. 그 결과와 `RimIntensity`를 다시 `Multiply`로 곱합니다.
5. 최종 결과를 Master Stack Fragment의 `Emission`에 연결합니다.

```text
RimPower ──> Fresnel Effect ──┐
RimColor ─────────────────── Multiply ── Multiply(RimIntensity) ──> Emission
```

`Fresnel Effect`는 법선과 카메라 방향의 차이를 이용해 가장자리에서 큰 값을 만듭니다. `Emission`에 연결하면 Scene Light의 방향이 바뀌어도 가독성용 Rim Light를 유지하기 쉽습니다.

| 바꿀 값 | 예상 결과 |
| :--- | :--- |
| `RimPower`를 `1` → `5` | 값이 커질수록 밝은 테두리가 얇아집니다. |
| `RimIntensity`를 `0` → `3` | 값이 커질수록 테두리 빛이 밝아집니다. |
| Camera를 옆으로 이동 | Rim Light가 보이는 가장자리 위치도 바뀝니다. |

Rim Light가 전혀 보이지 않으면 Material이 `Mat_ToonRim`인지, Fresnel 결과가 Emission에 연결됐는지, `RimIntensity`가 0이 아닌지 순서로 확인합니다.

## 4. Outline Shell (Inverted Hull): 바깥쪽으로 확장한 뒷면만 그리기

외곽선은 하나의 일반 Surface Graph만으로 원본 표면과 동시에 그리기보다, **원본과 Outline Shell을 각각 렌더링**하는 방식이 이해하기 쉽습니다. 원본 Capsule은 `Mat_ToonRim`으로 그립니다. 같은 Mesh를 한 번 더 복제한 Outline Shell은 정점을 법선 방향으로 조금 넓히고, 뒷면만 어두운 색으로 그립니다. 원본이 안쪽을 덮으므로 바깥 가장자리만 남습니다. 이 기법의 일반적인 이름은 **Inverted Hull**입니다.

### 이 표현을 부르는 이름

| 이름 | 뜻 | 이 문서에서의 위치 |
| :--- | :--- | :--- |
| **Inverted Hull** 또는 **Backface Extrusion** | Mesh를 법선 방향으로 확장하고 뒷면만 그려 실루엣 선을 남기는 기법 | 지금 만드는 `Outline Shell`입니다. |
| **Outline Shell** | Inverted Hull을 위해 원본 바깥에 한 겹 더 그리는 확장된 표면 | 학습용으로 복제한 `ToonOutlineShell`입니다. |
| **Screen Space Outline** | 화면의 Depth·Normal 차이로 경계를 찾는 기법 | 5절의 `SG_ScreenOutline`입니다. |
| **Multi-pass Outline** | 한 Mesh를 원본 Pass와 Outline Pass로 두 번 그리는 렌더링 구성 | 이 절 하단의 심화 방식입니다. Inverted Hull 자체와는 다른 이름의 구현 구성입니다. |

### Outline Shell 대상 만들기

1. `ToonCharacter`를 복제하고 이름을 `ToonOutlineShell`로 바꿉니다.
2. 두 Capsule의 Transform 위치·회전·크기가 완전히 같은지 확인합니다. 계층을 깔끔하게 유지하려면 두 오브젝트를 `ToonCharacterRoot` 빈 부모 아래에 둡니다.
3. `ToonOutlineShell`의 Material 슬롯에는 이후 만들 `Mat_OutlineShell`만 연결합니다.

> Shell은 원본 Mesh를 한 번 더 그리므로 비용이 추가됩니다. 학습용 Capsule과 중요한 캐릭터에만 사용하고, 씬의 모든 작은 소품에 적용하지 않습니다.

### Outline Graph와 Material 만들기

1. `Create > Shader Graph > URP > Unlit Shader Graph`로 `SG_OutlineShell`을 만듭니다.
2. Graph Inspector에서 `Surface Type = Opaque`, `Render Face = Back`으로 설정합니다. `Back`은 뒷면 삼각형만 그리므로, 확장된 Shell의 앞면이 원본을 덮지 않습니다.
3. Blackboard에 아래 Property를 만들고 `Exposed`를 켭니다.
4. `Mat_OutlineShell` Material을 만들고 Shader를 `SG_OutlineShell`로 선택한 뒤 `ToonOutlineShell`에 연결합니다.

| Property | 타입 | 시작값 | 역할 |
| :--- | :--- | :--- | :--- |
| `OutlineColor` | Color | 거의 검은 남색 | 외곽선 색입니다. |
| `OutlineWidth` | Float | `0.03` | 정점을 법선 방향으로 밀어낼 거리입니다. |

### Vertex와 Fragment를 각각 연결하기

1. `Position` 노드를 만들고 Space를 `Object`로 설정합니다.
2. `Normal Vector` 노드를 만들고 Space를 `Object`로 설정합니다.
3. `OutlineWidth`와 Object Space Normal을 `Multiply`로 곱합니다. 이것이 법선 방향 Offset입니다.
4. Object Space Position과 Offset을 `Add`로 더합니다.
5. Add 결과를 Master Stack Vertex의 `Position`에 연결합니다.
6. `OutlineColor`를 Master Stack Fragment의 `Base Color`에 연결합니다.

```text
Normal Vector (Object) ── Multiply(OutlineWidth) ──┐
                                                    Add ──> Vertex Position
Position (Object) ─────────────────────────────────┘

OutlineColor ──────────────────────────────────────────> Base Color
```

Vertex Position은 Object Space 최종 위치를 받습니다. 그러므로 Position과 Normal 모두 Object Space로 맞춰야 합니다. World Space Normal을 Object Space Position에 바로 더하면 Transform이 있는 오브젝트에서 예상과 다른 결과가 날 수 있습니다.

### Outline을 확인하는 순서

1. `OutlineWidth = 0`으로 두면 외곽선이 보이지 않거나 원본과 겹칩니다.
2. `OutlineWidth = 0.01`로 올리면 얇은 어두운 테두리가 보입니다.
3. `OutlineWidth = 0.05`로 올리면 테두리가 두꺼워집니다.
4. Capsule을 회전하고 Camera를 이동해도 테두리가 외곽을 따라가는지 확인합니다.

| 증상 | 먼저 확인할 것 |
| :--- | :--- |
| Outline 전체가 원본 앞을 덮음 | `Render Face = Back`인지, 원본과 Shell Material이 뒤바뀌지 않았는지 확인합니다. |
| 외곽선이 보이지 않음 | Shell이 원본과 같은 위치인지, `OutlineWidth`가 0보다 큰지 확인합니다. |
| 일부만 너무 두껍거나 갈라짐 | `OutlineWidth`를 낮추고 Capsule처럼 닫힌 Mesh에서 먼저 확인합니다. |
| 오브젝트를 회전하면 이상해짐 | Position과 Normal이 모두 Object Space인지 확인합니다. |

### 심화: 복제 GameObject 대신 URP 멀티패스로 그리기

앞의 실습은 `ToonCharacter`를 복제해 두 `Mesh Renderer`가 같은 Mesh를 각각 그리는 방법입니다. URP에서는 `Render Objects Renderer Feature`를 추가하면 복제 GameObject 없이 **같은 Renderer를 Outline Pass와 원본 Pass에서 두 번** 그릴 수 있습니다.

```text
Render Objects Renderer Feature ──> Outline Pass: 확장된 뒷면, 어두운 색
기본 URP Opaque Pass              ──> Surface Pass: 원래 Mesh, Toon Band + Rim Light
```

#### Renderer Feature로 Outline Pass 추가하기

1. `ToonCharacter`의 Layer를 새로 만든 `OutlineTarget`으로 바꿉니다. 원본 Material은 `Mat_ToonRim`으로 유지합니다.
2. 비교용 `ToonOutlineShell` GameObject는 비활성화합니다. 삭제하지 않고 두 방식의 결과를 비교할 수 있게 남겨 둡니다.
3. 현재 URP Renderer Data Asset을 선택하고 `Add Renderer Feature > Render Objects`를 추가합니다.
4. 이름을 `Outline Shell Pass`로 바꾸고 `Event = Before Rendering Opaques`로 설정합니다. Outline을 먼저 그린 뒤 기본 Opaque Pass가 원본 표면을 덮게 하는 순서입니다.
5. `Filters > Queue`는 `Opaque`, `Layer Mask`는 `OutlineTarget`으로 설정합니다.
6. `Overrides > Override Mode`를 `Material`로 설정하고 Material에 `Mat_OutlineShell`을 넣습니다.
7. Play Mode에서 `ToonCharacter` 하나만 남아 있어도 외곽선이 보이는지 확인합니다.

이 구성에서는 Renderer Feature가 먼저 `Mat_OutlineShell`로 같은 Mesh를 그립니다. 이어서 일반 URP Opaque Pass가 원래 `Mat_ToonRim`으로 같은 Mesh를 그리므로, Hierarchy에는 원본 캐릭터 하나만 남습니다. `Render Objects`는 Layer Mask로 대상을 고르고, 지정한 Material로 다시 렌더링할 수 있습니다. [Unity Render Objects Renderer Feature 문서](https://docs.unity.cn/6000.0/Documentation/Manual/urp/renderer-features/renderer-feature-render-objects.html)

#### Shader Graph와 커스텀 멀티패스 셰이더의 경계

| 방법 | 이 문서에서 바로 실습 가능 여부 | 특징 |
| :--- | :--- | :--- |
| 복제 GameObject + `SG_OutlineShell` | 가능 | 가장 이해하기 쉽습니다. Renderer가 두 개라서 Skinned Mesh에는 동기화 관리가 필요합니다. |
| Render Objects Renderer Feature + `SG_OutlineShell` | 가능 | 같은 Renderer를 파이프라인이 두 번 그립니다. 복제 Skinned Mesh Renderer가 필요하지 않습니다. |
| ShaderLab/HLSL의 커스텀 Pass + Renderer Feature | 심화 코드 작업 필요 | `OutlinePass`와 `SurfacePass`를 한 셰이더에 정의할 수 있지만, URP에서는 커스텀 `LightMode`를 요청하는 Renderer Feature도 추가해야 합니다. |

URP는 일반 Opaque 단계에서 Material의 Shader Pass 하나만 선택합니다. 따라서 ShaderLab 파일에 Outline Pass와 Surface Pass를 단순히 나란히 추가하는 것만으로는 두 Pass가 자동 실행되지 않습니다. 커스텀 셰이더를 만들 때는 Outline Pass에 예를 들어 `LightMode = "OutlinePass"`를 지정하고, Renderer Feature가 그 이름의 Pass를 별도로 그리도록 구성합니다. Shader Graph가 생성한 코드를 직접 수정하면 재저장 시 덮어써질 수 있으므로 수정하지 않습니다.

> 멀티패스는 Mesh를 두 번 그리는 비용을 없애는 방법은 아닙니다. 대신 Hierarchy를 단순하게 만들고, Skinned Mesh가 같은 Renderer의 애니메이션 변형을 두 Pass에서 공유하게 하는 구성입니다. 실제 프로젝트에서는 복제 방식과 Renderer Feature 방식의 비용·관리 편의성을 프로파일링해 선택합니다.

## 5. 전체 화면 Outline: 화면의 경계를 찾아 한 번에 그리기

Outline Shell은 지정한 Mesh를 한 번 더 그립니다. 반면 전체 화면 Outline은 카메라가 그린 화면의 이웃 픽셀을 비교해 경계를 찾습니다. 따라서 Capsule만이 아니라 화면에 보이는 여러 불투명 오브젝트에 같은 규칙을 적용할 수 있습니다.

| 비교 | Outline Shell | 전체 화면 Outline |
| :--- | :--- | :--- |
| 기준 | Mesh의 법선 방향 | 화면의 이웃 픽셀 Depth와 Normal |
| 적용 범위 | `ToonOutlineShell`처럼 지정한 Renderer | 카메라에 보이는 불투명 3D 오브젝트 전체 |
| 장점 | 캐릭터별 색·폭을 정밀하게 조절 | Mesh 복제 없이 장면의 표현을 통일 |
| 주의점 | Mesh를 한 번 더 렌더링 | UI·일부 투명 오브젝트는 기본적으로 포함되지 않을 수 있음 |

### Fullscreen Shader Graph와 Material 만들기

1. Project 창에서 `Create > Shader Graph > URP > Fullscreen Shader Graph`를 선택하고 `SG_ScreenOutline`으로 이름을 정합니다.
2. Blackboard에 아래 Property를 만들고 `Exposed`를 켭니다.
3. `Mat_ScreenOutline` Material을 만들고 Shader를 `Shader Graphs > SG_ScreenOutline`으로 선택합니다.

| Property | 타입 | 시작값 | 역할 |
| :--- | :--- | :--- | :--- |
| `OutlineColor` | Color | 거의 검은 남색 | 화면 경계에 덮을 색입니다. |
| `OutlineWidthPixels` | Float | `1` | 이웃 픽셀을 얼마나 떨어진 위치에서 읽을지 정합니다. `1`부터 시작합니다. |
| `NormalThreshold` | Float | `0.25` | 법선 방향 차이가 이 값보다 클 때 경계로 판단합니다. |
| `DepthThreshold` | Float | `0.5` | 깊이 차이가 이 값보다 클 때 경계로 판단합니다. |

### 이웃 픽셀의 Depth와 Normal 차이 연결하기

이 실습의 핵심은 현재 픽셀의 왼쪽·오른쪽, 위·아래 값이 얼마나 다른지 비교하는 것입니다. 두 면의 방향이 다르면 Normal 차이가 커지고, 서로 멀리 떨어지면 Depth 차이가 커집니다. 둘 중 하나라도 큰 자리를 외곽선으로 표시합니다.

1. `Screen Position` 노드를 만들고 Mode를 `Default`로 둡니다. `Split`의 `R`, `G`를 화면 UV로 사용합니다.
2. `Screen` 노드의 `Width`, `Height`를 각각 `Reciprocal`로 바꾼 뒤 `Combine`합니다. 이것이 화면에서 한 픽셀만 이동하는 `TexelStep`입니다.
3. `TexelStep`에 `OutlineWidthPixels`를 `Multiply`합니다. 결과를 `Offset`이라고 부릅니다.
4. UV에 `Vector2(Offset.x, 0)`, `Vector2(-Offset.x, 0)`, `Vector2(0, Offset.y)`, `Vector2(0, -Offset.y)`를 각각 `Add`해 Right·Left·Up·Down UV를 만듭니다.
5. `URP Sample Buffer` 노드를 네 개 만들고 Source Buffer를 모두 `NormalWorldSpace`로 설정합니다. 각 노드의 UV에 Right·Left·Up·Down UV를 연결합니다.
6. `Subtract(RightNormal, LeftNormal)`과 `Subtract(UpNormal, DownNormal)`을 만들고, 각각 `Length`를 구합니다. 두 값을 `Add`하면 `NormalDifference`입니다.
7. `Step(NormalThreshold, NormalDifference)`를 만들어 `NormalEdge`를 구합니다.
8. `Scene Depth` 노드를 네 개 만들고 각 노드의 `Eye` 출력을 사용합니다. 각 UV에는 앞에서 만든 Right·Left·Up·Down UV를 연결합니다.
9. `Abs(Subtract(RightDepth, LeftDepth))`와 `Abs(Subtract(UpDepth, DownDepth))`를 만들고 `Add`합니다. 이것이 `DepthDifference`입니다.
10. `Step(DepthThreshold, DepthDifference)`를 만들어 `DepthEdge`를 구합니다.
11. `Maximum(NormalEdge, DepthEdge)`를 만들어 `Edge`를 구합니다. `Edge`는 둘 중 하나가 경계면 `1`, 나머지는 `0`에 가까운 값입니다.

```text
Right Normal ─ Left Normal ─ Length ─┐
                                      Add ─ Step(NormalThreshold) ─ NormalEdge ─┐
Up Normal    ─ Down Normal ─ Length ─┘                                           Maximum ─ Edge
Right Depth  ─ Left Depth  ─ Abs ────┐                                           │
                                      Add ─ Step(DepthThreshold) ── DepthEdge ───┘
Up Depth     ─ Down Depth   ─ Abs ───┘
```

### 원래 화면 위에 선을 덮기

1. `URP Sample Buffer` 노드를 하나 더 만들고 Source Buffer를 `BlitSource`로 설정합니다. UV에는 1단계의 원래 화면 UV를 연결합니다.
2. `Lerp`를 만듭니다. `A`에는 `BlitSource`, `B`에는 `OutlineColor`, `T`에는 `Edge`를 연결합니다.
3. `Lerp` 출력을 Fullscreen Master Stack의 `Base Color`에 연결하고 저장합니다.

```text
BlitSource ───────── Lerp(A) ──> Base Color
OutlineColor ─────── Lerp(B)
Edge ─────────────── Lerp(T)
```

### URP Renderer에 전체 화면 Pass 연결하기

1. Project 창에서 현재 URP Renderer Data Asset을 선택합니다. Universal 3D 템플릿에서는 보통 `Assets/Settings` 아래에 있습니다.
2. Inspector에서 `Add Renderer Feature > Full Screen Pass Renderer Feature`를 선택합니다.
3. 이름을 `Screen Outline`으로 바꾸고 `Pass Material`에 `Mat_ScreenOutline`을 연결합니다.
4. `Injection Point = Before Rendering Post Processing`으로 설정합니다.
5. `Requirements`에서 `Color`, `Normal`, `Depth`를 켭니다. `Color`는 원래 화면을 `BlitSource`로 전달하고, `Normal`은 `NormalWorldSpace`, `Depth`는 `Scene Depth`를 읽을 수 있게 합니다.

Play Mode에서 Capsule 외에 Plane이나 Cube를 하나 더 놓고, 두 오브젝트의 경계에도 선이 생기는지 확인합니다. 선이 전혀 보이지 않으면 `Requirements`의 `Normal`·`Depth`·`Color`, `Pass Material`, `NormalThreshold` 순서로 확인합니다. 선이 너무 많으면 `NormalThreshold` 또는 `DepthThreshold`를 올리고, 선이 너무 가늘면 `OutlineWidthPixels`를 `2`로 올립니다.

> 전체 화면 방식은 매 프레임 화면 전체를 검사합니다. 따라서 캐릭터 하나만 선명하게 만들고 싶을 때는 4절의 Shell이 단순할 수 있고, 장면 전체에 같은 만화식 규칙을 적용하고 싶을 때는 이 방식을 선택합니다.

## 6. Volume Color Grading은 마지막에 적용하기

Color Grading은 Mesh Material이 아니라 카메라 화면 전체에 영향을 줍니다. 따라서 Toon Band, Rim Light, Outline Shell, Screen Outline이 각각 의도대로 보이는 것을 확인한 후에만 DAY 03의 Global Volume Profile에서 Contrast 또는 Color Filter를 조절합니다.

1. Global Volume을 선택하고 Profile을 엽니다.
2. `Color Adjustments` Override를 추가합니다.
3. Contrast를 작은 값부터 바꿉니다.
4. 필요하면 Color Filter를 약하게 적용합니다.
5. 다시 `Mat_ToonRim`의 Rim 값과 `Mat_OutlineShell`의 OutlineColor를 확인합니다.

색 보정을 먼저 하면 Shader Graph 연결 오류인지 Volume 효과인지 구분하기 어렵습니다.

## 7. 단계별 Play Mode 확인

| 순서 | 연결 상태 | 확인할 결과 |
| :--- | :--- | :--- |
| 1 | `Mat_ToonBand`만 적용 | Capsule이 `LitColor`와 `ShadowColor` 두 영역으로 나뉩니다. |
| 2 | `Mat_ToonRim`으로 교체 | Toon Band 위에 카메라 가장자리 Rim Light가 추가됩니다. |
| 3 | `ToonOutlineShell` 활성화 | 원본 Capsule 바깥에만 어두운 Outline이 보입니다. |
| 4 | `Screen Outline` 활성화 | Capsule·Cube·Plane의 화면 경계에 같은 색 선이 생깁니다. |
| 5 | Volume 적용 | 앞의 표현을 유지한 채 장면 전체 색감·대비만 달라집니다. |

한 단계가 실패하면 이후 단계의 값을 함께 바꾸지 말고, 바로 앞 단계의 Graph 연결·Material 슬롯·Inspector 값을 다시 확인합니다.

## 8. DAY 07 코드와 연결하기

Shader Graph에서 `Position (Object) + Normal (Object) × OutlineWidth → Vertex Position`으로 연결한 것은 DAY 07 코드에서 아래와 같은 역할입니다.

```hlsl
float3 outlineOffset = normalOS * outlineWidth;
float3 positionOS = input.positionOS.xyz + outlineOffset;
output.positionCS = TransformObjectToHClip(positionOS);
```

Graph의 Vertex Position은 Object Space 위치를 받습니다. HLSL의 `input.positionOS`와 `output.positionCS` 사이에 Offset 계산을 넣은 것과 같습니다. Rim Light의 Fresnel과 Toon Band의 Dot Product도 결국 Fragment 단계에서 최종 색·Emission으로 가는 값을 만드는 Graph 연결입니다.

## 9. 심화 미니 실습: 실제 게임 스타일을 축소 재현하기

앞 절에서 만든 세 Graph를 조합해 실제 게임의 카툰 표현 원리를 작은 Capsule 장면에서 시험합니다. 게임을 그대로 복제하는 것이 아니라, 각 게임이 선택한 표현 중 오늘 만든 Graph로 확인 가능한 부분만 재현합니다.

| 참고 게임 | 이 실습에서 시험할 표현 | 사용할 Graph |
| :--- | :--- | :--- |
| Team Fortress 2 | 따뜻한 밝은 면·차가운 그림자·Rim Highlight | `SG_ToonRim` |
| Guilty Gear Xrd | 캐릭터 실루엣을 강조하는 Inverted Hull | `SG_ToonRim`, `SG_OutlineShell` |
| Hi-Fi RUSH | 장면 전체를 통일하는 만화식 화면 경계 | `SG_ToonRim`, `SG_ScreenOutline` |

### 시작하기 전: 비교용 Material 세 개 만들기

1. `Mat_ToonRim`을 복제해 `Mat_StyleCharacter`로 이름을 바꿉니다. Shader가 `SG_ToonRim`인지 확인합니다.
2. `Mat_OutlineShell`을 복제해 `Mat_StyleShell`로 이름을 바꿉니다. Shader가 `SG_OutlineShell`인지 확인합니다.
3. `Mat_ScreenOutline`을 복제해 `Mat_StyleScreenOutline`으로 이름을 바꿉니다. Shader가 `SG_ScreenOutline`인지 확인합니다.
4. `ToonCharacter`에는 `Mat_StyleCharacter`, `ToonOutlineShell`에는 `Mat_StyleShell`을 연결합니다.
5. 5절에서 만든 `Screen Outline` Renderer Feature의 `Pass Material`은 `Mat_StyleScreenOutline`으로 바꿉니다.

### A. Team Fortress 2 영감: 검은 선 대신 빛으로 실루엣 읽기

Team Fortress 2는 검은 외곽선보다 Rim Highlight와 밝은 면·그림자 면의 색 대비를 중요한 가독성 도구로 사용한 사례입니다. [Illustrative Rendering 자료](https://advances.realtimerendering.com/s2007/Mitchell-IllustrativeRenderingInTF2%28Siggraph07%29.pdf)

1. `Mat_StyleCharacter`에서 `LitColor = #FFD58A`, `ShadowColor = #456FAD`, `BandThreshold = 0.45`로 바꿉니다.
2. `RimColor = #FFF3C4`, `RimIntensity = 0.35`로 바꿉니다.
3. `Mat_StyleShell`의 `OutlineWidth = 0`으로 바꿉니다.
4. URP Renderer의 `Screen Outline` Renderer Feature를 잠시 끕니다.

밝은 면은 따뜻하고 그림자는 차가운 색으로 갈리며, 검은 선 없이도 Rim Light가 Capsule의 형태를 읽기 쉽게 만드는지 확인합니다.

### B. Guilty Gear Xrd 영감: 캐릭터만 선명하게 분리하기

Guilty Gear Xrd는 캐릭터 외곽선에 Inverted Hull을 사용하고, 실제 게임에서는 Vertex Color로 부위별 선도 제어합니다. [GDC 발표 자료](https://ggxrd.com/Motomura_Junya_GuiltyGearXrd.pdf)

1. `Mat_StyleCharacter`에서 `RimColor = #F5C43D`, `RimPower = 3`, `RimIntensity = 0.8`로 바꿉니다.
2. `Mat_StyleShell`에서 `OutlineColor = #11152A`, `OutlineWidth = 0.035`로 바꿉니다.
3. 4절의 `Outline Shell Pass` Renderer Feature를 사용한다면 Override Material을 `Mat_StyleShell`로 바꾸고 `ToonOutlineShell`은 비활성화합니다. 복제 방식으로 비교한다면 Renderer Feature를 끄고 `ToonOutlineShell`을 활성화합니다.
4. URP Renderer의 `Screen Outline` Renderer Feature는 끈 상태로 둡니다.
5. Camera를 옆으로 이동하고 Capsule을 회전합니다.

Shell이 원본 Capsule 바깥에만 남는지, Rim Light와 어두운 선이 서로 다른 역할을 하는지 확인합니다. 이 실습은 일정한 폭만 사용하므로 실제 게임의 부위별 선 제어와는 다릅니다.

### C. Hi-Fi RUSH 영감: 장면 전체의 경계를 같은 규칙으로 그리기

Hi-Fi RUSH는 월드 전체를 위한 Deferred Toon Renderer와 여러 Render Pass를 사용합니다. 여기서는 그중 “캐릭터와 배경을 같은 화면 경계 규칙으로 묶는” 부분만 `SG_ScreenOutline`으로 축소 재현합니다. [GDC 발표](https://gdcvault.com/play/1034330/3D-Toon-Rendering-in-Hi)

1. Capsule 주변에 Cube와 Plane을 하나씩 둡니다. Capsule만이 아니라 배경 물체도 화면에 보이게 합니다.
2. `Mat_StyleCharacter`에서 `LitColor = #FFB74D`, `ShadowColor = #553D9C`, `RimColor = #00E5FF`, `RimIntensity = 1.5`로 바꿉니다.
3. `Mat_StyleShell`의 `OutlineWidth = 0`으로 바꿉니다. 캐릭터 전용 Shell과 화면 전체 Outline이 겹치지 않게 합니다.
4. `Mat_StyleScreenOutline`에서 `OutlineColor = #14213D`, `OutlineWidthPixels = 1`, `NormalThreshold = 0.25`, `DepthThreshold = 0.5`로 설정합니다.
5. URP Renderer의 `Screen Outline` Renderer Feature를 켭니다.

Capsule·Cube·Plane의 경계가 한 번에 표시되는지 확인합니다. `NormalThreshold` 또는 `DepthThreshold`를 올리면 꼭 필요한 경계만 남고, `OutlineWidthPixels`를 올리면 선이 두꺼워집니다.

### 세 표현을 비교하기

| 비교할 질문 | A. Team Fortress 2 영감 | B. Guilty Gear Xrd 영감 | C. Hi-Fi RUSH 영감 |
| :--- | :--- | :--- | :--- |
| 선을 그리는 범위는 어디인가요? | 선 대신 Rim Light 중심 | Capsule 한 개 | 카메라에 보이는 불투명 오브젝트 전체 |
| 꺼야 하는 표현은 무엇인가요? | Shell, 전체 화면 Outline | 전체 화면 Outline | Shell |
| 이 방식이 어울리는 경우는? | 조명으로 형태를 읽히고 싶을 때 | 주인공 한 명을 또렷하게 보이고 싶을 때 | 장면 전체의 만화식 규칙을 통일할 때 |

## 오늘의 정리

- Toon Band는 Normal과 고정 Light Direction의 Dot Product를 Step으로 나눠 두 색 중 하나를 고릅니다.
- Rim Light는 Fresnel 결과를 Emission에 더해 카메라 가장자리 가독성을 높입니다.
- Outline Shell은 원본과 별도 Mesh를 법선 방향으로 확장하고, 뒷면만 그려 외곽만 남깁니다.
- Shader Graph의 Vertex Position은 Object Space 위치를 받으므로 Position·Offset의 좌표 공간을 맞춰야 합니다.
- 심화 미니 실습에서는 세 Graph를 조합해, 게임마다 캐릭터 전용 선·빛 중심 가독성·장면 전체 선 중 무엇을 선택하는지 비교합니다.
- 다음 시간부터는 이펙트 프로그래밍으로 넘어가 Particle System을 다룹니다.
