# DAY 08: Shader Graph로 Toon, Rim Light, Outline 만들기

오늘의 목표는 DAY 04~07에서 배운 Shader Graph의 입력·연결·출력 흐름으로, 캐릭터가 배경에서 눈에 띄는 **비실사 표현**을 직접 만드는 것입니다. 오늘은 노드 이름을 많이 외우기보다, 각 Graph가 어떤 값을 만들고 최종적으로 어디에 연결하는지 확인합니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 비실사 렌더링, 셰이더로 게임 개성 표현
- Unity 6 재구성: URP Shader Graph로 Toon Band, Rim Light, Outline Shell을 제작하고 Material에서 값을 조절합니다.

## 오늘의 완성 목표

Capsule 하나에 아래 세 가지 표현을 순서대로 적용합니다.

| 표현 | 만들 Graph 또는 Material | 눈으로 확인할 결과 |
| :--- | :--- | :--- |
| Toon Band | `SG_ToonBand` | 표면 밝기가 부드럽게 섞이지 않고 밝은 면·어두운 면으로 나뉩니다. |
| Rim Light | `SG_ToonRim` | 카메라에서 옆으로 보이는 가장자리에 색 테두리 빛이 생깁니다. |
| Outline Shell | `SG_OutlineShell` | 원본 Capsule 바깥에 어두운 외곽선이 남습니다. |

Color Grading은 Shader Graph가 아니라 DAY 03에서 배운 Volume 기능입니다. 세 Graph 결과가 맞는지 먼저 확인한 뒤 마지막에 적용합니다.

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
| :--- | :--- | :--- |
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

## 4. Outline Shell: 바깥쪽으로 확장한 뒷면만 그리기

외곽선은 하나의 일반 Surface Graph만으로 원본 표면과 동시에 그리기보다, **원본과 Outline Shell을 각각 렌더링**하는 방식이 이해하기 쉽습니다. 원본 Capsule은 `Mat_ToonRim`으로 그립니다. 같은 Mesh를 한 번 더 복제한 Outline Shell은 정점을 법선 방향으로 조금 넓히고, 뒷면만 어두운 색으로 그립니다. 원본이 안쪽을 덮으므로 바깥 가장자리만 남습니다.

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
| :--- | :--- | :--- |
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

## 5. Volume Color Grading은 마지막에 적용하기

Color Grading은 Mesh Material이 아니라 카메라 화면 전체에 영향을 줍니다. 따라서 Toon Band, Rim Light, Outline Shell이 각각 의도대로 보이는 것을 확인한 후에만 DAY 03의 Global Volume Profile에서 Contrast 또는 Color Filter를 조절합니다.

1. Global Volume을 선택하고 Profile을 엽니다.
2. `Color Adjustments` Override를 추가합니다.
3. Contrast를 작은 값부터 바꿉니다.
4. 필요하면 Color Filter를 약하게 적용합니다.
5. 다시 `Mat_ToonRim`의 Rim 값과 `Mat_OutlineShell`의 OutlineColor를 확인합니다.

색 보정을 먼저 하면 Shader Graph 연결 오류인지 Volume 효과인지 구분하기 어렵습니다.

## 6. 단계별 Play Mode 확인

| 순서 | 연결 상태 | 확인할 결과 |
| :--- | :--- | :--- |
| 1 | `Mat_ToonBand`만 적용 | Capsule이 `LitColor`와 `ShadowColor` 두 영역으로 나뉩니다. |
| 2 | `Mat_ToonRim`으로 교체 | Toon Band 위에 카메라 가장자리 Rim Light가 추가됩니다. |
| 3 | `ToonOutlineShell` 활성화 | 원본 Capsule 바깥에만 어두운 Outline이 보입니다. |
| 4 | Volume 적용 | 앞의 표현을 유지한 채 장면 전체 색감·대비만 달라집니다. |

한 단계가 실패하면 이후 단계의 값을 함께 바꾸지 말고, 바로 앞 단계의 Graph 연결·Material 슬롯·Inspector 값을 다시 확인합니다.

## 7. DAY 07 코드와 연결하기

Shader Graph에서 `Position (Object) + Normal (Object) × OutlineWidth → Vertex Position`으로 연결한 것은 DAY 07 코드에서 아래와 같은 역할입니다.

```hlsl
float3 outlineOffset = normalOS * outlineWidth;
float3 positionOS = input.positionOS.xyz + outlineOffset;
output.positionCS = TransformObjectToHClip(positionOS);
```

Graph의 Vertex Position은 Object Space 위치를 받습니다. HLSL의 `input.positionOS`와 `output.positionCS` 사이에 Offset 계산을 넣은 것과 같습니다. Rim Light의 Fresnel과 Toon Band의 Dot Product도 결국 Fragment 단계에서 최종 색·Emission으로 가는 값을 만드는 Graph 연결입니다.

## 오늘의 정리

- Toon Band는 Normal과 고정 Light Direction의 Dot Product를 Step으로 나눠 두 색 중 하나를 고릅니다.
- Rim Light는 Fresnel 결과를 Emission에 더해 카메라 가장자리 가독성을 높입니다.
- Outline Shell은 원본과 별도 Mesh를 법선 방향으로 확장하고, 뒷면만 그려 외곽만 남깁니다.
- Shader Graph의 Vertex Position은 Object Space 위치를 받으므로 Position·Offset의 좌표 공간을 맞춰야 합니다.
- 다음 시간부터는 이펙트 프로그래밍으로 넘어가 Particle System을 다룹니다.
