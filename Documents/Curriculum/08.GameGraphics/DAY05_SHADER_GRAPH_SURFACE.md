# DAY 05: Shader Graph로 표면 표현 만들기

오늘의 목표는 Shader Graph를 사용해 단순한 색이 아니라 "**움직이는 표면의 성격**"을 만드는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 셰이더 알고리즘 이해 및 사용, 렌더링 효과 구현
- Unity 6 재구성: Noise, UV, Emission, Alpha를 사용해 게임용 표면 효과를 만듭니다.

## 1. 표면 효과는 무엇을 조합하나요?

용암, 물, 보호막 같은 표면은 대부분 색, 밝기, 투명도, 패턴, 시간 변화를 조합해 만듭니다. 모델 자체가 움직이지 않아도 UV와 Noise를 움직이면 표면이 살아 있는 것처럼 보입니다.

### 자주 쓰는 노드

| 노드 | 쓰임 |
| :--- | :--- |
| `UV` | 텍스처를 표면에 붙이는 좌표 |
| `Tiling And Offset` | 텍스처 반복과 이동 |
| `Simple Noise` | 불규칙한 무늬 생성 |
| `Lerp` | 두 값을 비율에 따라 섞기 |
| `Fresnel Effect` | 가장자리 빛 표현 |
| `Emission` | 스스로 빛나는 표현 |

### 노드를 읽는 방법: "재료, 계산기, 최종 출력"

Shader Graph의 노드는 이름을 외우기보다 세 가지 역할로 나누어 읽습니다. `UV`, `Time`, `Color`, Blackboard Property는 **재료**이고, `Multiply`, `Lerp`, `Smoothstep`은 재료를 바꾸는 **계산기**입니다. 마지막으로 `Base Color`, `Emission`, `Alpha`는 화면에 결과를 보내는 **최종 출력**입니다.

| 노드 | 들어오는 값 | 나오는 값 | 이 문서에서 하는 일 | 처음 확인할 방법 |
| :--- | :--- | :--- | :--- | :--- |
| `Time` | 없음 | 계속 커지는 시간 값 | 흐름·깜박임을 시작합니다. | 임시로 `Sine` 뒤 `Base Color`에 연결해 변화가 보이는지 확인합니다. |
| `UV` | 없음 | 텍스처 좌표 `Vector2` | 무늬를 어디에서 읽을지 정합니다. | Sample Texture 2D의 `UV`에 바로 연결합니다. |
| `Tiling And Offset` | UV, 반복 수, 이동량 | 바뀐 UV 좌표 | 무늬를 반복하고 흘립니다. | Output을 Sample Texture 2D의 `UV`에 연결합니다. |
| `Simple Noise` | UV, Scale | 0~1 범위의 흑백 무늬 | 용암 균열처럼 불규칙한 영역을 만듭니다. | Output을 Base Color에 임시 연결합니다. |
| `Fresnel Effect` | Normal, View Direction, Power | 가장자리가 큰 0~1 값 | 보호막 테두리만 밝게 만듭니다. | Output을 Base Color에 임시 연결합니다. |
| `Multiply` | 값 A, 값 B | 두 값을 곱한 값 | 색에 밝기·강도·속도를 적용합니다. | 한 입력을 `1`로 두고 결과 변화를 봅니다. |
| `Lerp` | A 색, B 색, T 값 | A와 B가 섞인 색 | Noise가 어두운 바위와 밝은 용암의 비율을 정하게 합니다. | T에 Noise를 연결하기 전 `0`과 `1`을 넣어 두 색을 확인합니다. |
| `Smoothstep` | Edge1, Edge2, In | 대비가 정리된 0~1 값 | 흐릿한 Noise를 용암 균열처럼 선명하게 만듭니다. | Output을 Base Color에 임시 연결합니다. |
| `Base Color` | 색 | 표면 기본색 | 빛을 받는 물체의 기본색을 정합니다. | RGB를 바로 연결해 텍스처가 보이는지 확인합니다. |
| `Emission` | 색과 밝기 | 자체 발광색 | 용암·보호막의 밝은 부분을 강조합니다. | 처음에는 낮은 강도부터 연결합니다. |
| `Alpha` | 0~1 숫자 | 투명도 | 보호막의 투명한 정도를 정합니다. | Graph를 Transparent로 바꾼 뒤 연결합니다. |

## 2. 실습 선택지

| 선택 | 구현 목표 |
| :--- | :--- |
| 용암 바닥 | Noise와 Emission으로 뜨거운 균열 표현 |
| 마법 보호막 | Fresnel과 Alpha로 가장자리 빛 표현 |
| 흐르는 물 | UV Offset으로 표면이 흐르는 느낌 표현 |

## 3. 보호막 Shader Graph 구성 예

```text
Fresnel Effect -> Multiply -> Emission
Base Color     -> Base Color
Alpha          -> Alpha
Time           -> Sine -> Alpha 보정
```

## 4. 보호막 Shader Graph 만들기

보호막은 가장자리가 밝고 가운데는 투명하게 보이면 그럴듯합니다. 핵심은 `Fresnel Effect`입니다. 카메라 시선과 표면 각도를 비교해서 가장자리 쪽 값을 크게 만들어 줍니다.

### 그래프 설정

1. `Create > Shader Graph > URP > Lit Shader Graph`를 선택합니다.
2. 이름을 `SG_Shield`로 지정합니다.
3. Graph Inspector에서 Surface Type을 `Transparent`로 바꿉니다.
4. 필요하면 Blend Mode를 `Alpha`로 둡니다.
5. Blackboard에 다음 프로퍼티를 만듭니다.

| 프로퍼티 | 타입 | 기본값 예시 | 역할 |
| :--- | :--- | :--- | :--- |
| `ShieldColor` | Color | 하늘색 | 보호막 기본 색 |
| `RimPower` | Float | 3 | 가장자리 빛의 두께 |
| `EmissionStrength` | Float | 2 | 빛나는 정도 |
| `AlphaStrength` | Float | 0.45 | 투명도 |
| `PulseSpeed` | Float | 2 | 깜박임 속도 |

### Material과 대상 오브젝트 준비

1. Project 창의 `GameGraphics/Shaders` 폴더에 `SG_Shield`를 저장하고, `GameGraphics/Materials`에서 `Create > Material`로 `Mat_Shield`를 만듭니다.
2. `Mat_Shield`의 Inspector `Shader` 드롭다운에서 `Shader Graphs > SG_Shield`를 선택합니다. 생성한 Graph를 Material 슬롯에 직접 끌어 놓아도 됩니다.
3. Hierarchy에서 Sphere 또는 Capsule을 선택하고 `Mesh Renderer > Materials > Element 0`에 `Mat_Shield`를 연결합니다.
4. Material Inspector의 `ShieldColor`, `RimPower`, `EmissionStrength`, `AlphaStrength`, `PulseSpeed`가 보이는지 확인합니다. 보이지 않으면 Graph Blackboard의 Exposed와 저장 상태를 확인합니다.
5. 투명 Material은 Scene View보다 Game View에서 Camera·배경과 함께 확인합니다. 비교할 때는 Camera와 Light를 고정합니다.

### 노드 연결 순서

| 단계 | 연결 |
| :--- | :--- |
| 1 | `Fresnel Effect` 노드를 만듭니다. |
| 2 | `RimPower`를 `Fresnel Effect`의 Power에 연결합니다. |
| 3 | `Fresnel Effect` 결과와 `ShieldColor`를 `Multiply`로 곱합니다. |
| 4 | 그 결과에 `EmissionStrength`를 한 번 더 곱합니다. |
| 5 | 최종 값을 Master Stack의 `Emission`에 연결합니다. |
| 6 | `ShieldColor`는 Master Stack의 `Base Color`에도 연결합니다. |
| 7 | `Fresnel Effect` 결과와 `AlphaStrength`를 곱해 `Alpha`에 연결합니다. |

```text
Fresnel Effect + RimPower -> Multiply(ShieldColor) -> Multiply(EmissionStrength) -> Emission
ShieldColor ---------------------------------------------------------------> Base Color
Fresnel Effect -> Multiply(AlphaStrength) --------------------------------> Alpha
```

### 보호막을 한 단계씩 테스트하기

한 번에 Emission과 Alpha를 모두 연결하면 투명도 문제인지 Fresnel 문제인지 알기 어렵습니다. 아래 순서로 한 선씩 추가합니다.

| 단계 | 연결 | Game View에서 확인할 결과 | 실패했을 때 확인할 곳 |
| :--- | :--- | :--- | :--- |
| 1 | `ShieldColor` → `Base Color` | Sphere 전체가 ShieldColor로 보입니다. | Material이 Sphere에 연결됐는지 확인합니다. |
| 2 | `Fresnel Effect` → `Base Color` | 가운데는 어둡고 가장자리만 밝아집니다. | 카메라가 Sphere를 보고 있는지, Fresnel의 Power가 너무 크지 않은지 확인합니다. |
| 3 | `Fresnel × ShieldColor × EmissionStrength` → `Emission` | 가장자리가 색을 유지한 채 밝게 빛납니다. | Multiply 한 입력이 0이 아닌지, EmissionStrength를 `0.5`부터 올려 봅니다. |
| 4 | `Fresnel × AlphaStrength` → `Alpha` | 가운데보다 테두리가 덜 투명하게 보입니다. | Graph Inspector의 Surface Type이 `Transparent`인지 확인합니다. |
| 5 | `Time × PulseSpeed` → `Sine` → `Remap` → Emission 곱셈 | 테두리 밝기가 천천히 반복됩니다. | `Time`의 Time 출력과 Remap 범위가 연결됐는지 확인합니다. |

`Fresnel Effect`는 카메라를 향한 면보다 **옆으로 보이는 가장자리**에 큰 값을 냅니다. 따라서 정면에서 크게 보이는 평면보다 Sphere나 Capsule에서 먼저 시험하는 편이 이해하기 쉽습니다. `RimPower`를 올리면 밝은 테두리는 더 얇아지고, 내리면 더 넓어집니다.

### 깜박임 추가하기

1. `Time` 노드의 Time 출력을 `Multiply`에 연결합니다.
2. 다른 입력에는 `PulseSpeed`를 연결합니다.
3. `Sine` 노드로 부드럽게 반복되는 값을 만듭니다.
4. `Remap`으로 값을 `0.5~1.0` 정도로 바꿉니다.
5. 이 값을 Emission 계산에 곱합니다.

깜박임은 보호막이 작동 중이라는 느낌을 줍니다. 단, 너무 빠르면 눈이 피곤하므로 처음에는 느리게 설정합니다.

## 5. 용암 표면 Shader Graph 만들기

용암은 균열 무늬와 발광이 중요합니다.

### 용암 준비와 Property

1. Plane을 만들고 이름을 `LavaSurface`로 정합니다.
2. `Create > Shader Graph > URP > Lit Shader Graph`로 `SG_Lava`를 만들고, `Mat_Lava` Material을 만들어 Plane에 연결합니다.
3. 아래 Property를 Blackboard에서 만들고 `Exposed`를 켭니다.

| 이름 | 타입 | 기본값 예시 | 역할 |
| :--- | :--- | :--- | :--- |
| `LavaTiling` | Vector2 | `(2, 2)` | Noise 무늬의 반복 수 |
| `FlowDirection` | Vector2 | `(0.03, 0.08)` | 용암 무늬가 이동하는 방향 |
| `FlowSpeed` | Float | `0.2` | 무늬 이동 속도 |
| `NoiseScale` | Float | `6` | 균열 무늬의 크기 |
| `DarkRockColor` | Color | 짙은 갈색 | 식은 바위 영역 색 |
| `LavaColor` | Color | 주황색 | 뜨거운 균열 색 |
| `EmissionStrength` | Float | `2` | 균열이 빛나는 세기 |

### 용암 노드 연결 순서

1. `UV` → `Tiling And Offset`의 `UV`에 연결하고, `LavaTiling`을 `Tiling`에 연결합니다.
2. `Time`의 `Time` × `FlowSpeed` × `FlowDirection` 결과를 `Tiling And Offset`의 `Offset`에 연결합니다.
3. `Tiling And Offset`의 `Out`을 `Simple Noise`의 `UV`에, `NoiseScale`을 Noise의 `Scale`에 연결합니다.
4. `Simple Noise` 출력은 `Smoothstep`의 `In`에 연결합니다. 처음에는 `Edge1 = 0.45`, `Edge2 = 0.65`로 둡니다.
5. `DarkRockColor`를 `Lerp`의 A에, `LavaColor`를 B에, `Smoothstep` 출력을 T에 연결합니다.
6. `Lerp` 출력을 `Base Color`에 연결합니다.
7. `Smoothstep` 출력 × `LavaColor` × `EmissionStrength` 결과를 `Emission`에 연결합니다.

| 단계 | 연결 |
| :--- | :--- |
| 1 | `Simple Noise`로 불규칙한 무늬를 만듭니다. |
| 2 | `Tiling And Offset`으로 UV를 천천히 움직입니다. |
| 3 | Noise 값을 `ColorRamp`처럼 쓰기 위해 `Smoothstep` 또는 `Step`으로 대비를 만듭니다. |
| 4 | 어두운 바위색과 주황색 발광색을 `Lerp`로 섞습니다. |
| 5 | 밝은 부분은 `Emission`에 연결합니다. |

```text
UV -> Tiling And Offset(Time으로 Offset 변화) -> Simple Noise
Simple Noise -> Smoothstep -> Lerp(DarkRock, LavaOrange) -> Base Color
Simple Noise -> Smoothstep -> Multiply(EmissionStrength) -> Emission
```

### 용암을 한 단계씩 테스트하기

| 단계 | 임시 연결 또는 설정 | 눈으로 확인할 결과 | 안 되면 확인할 것 |
| :--- | :--- | :--- | :--- |
| 1 | `Simple Noise` → `Base Color` | 흑백의 불규칙한 무늬가 보입니다. | Noise의 UV가 `Tiling And Offset`의 Out에 연결됐는지 확인합니다. |
| 2 | `Time` 흐름 → `Tiling And Offset > Offset` | 흑백 무늬가 천천히 움직입니다. | FlowSpeed가 0이 아닌지, Vector2 방향 값이 `(0, 0)`이 아닌지 확인합니다. |
| 3 | `Smoothstep` → `Base Color` | 회색 중간 영역이 줄고 균열 경계가 선명해집니다. | Edge1이 Edge2보다 작은지 확인합니다. |
| 4 | `Lerp(DarkRockColor, LavaColor, Smoothstep)` → `Base Color` | 어두운 바위와 주황 균열이 섞여 보입니다. | Lerp의 A/B/T 순서를 확인합니다. |
| 5 | `Smoothstep × LavaColor × EmissionStrength` → `Emission` | 주황 균열만 더 밝게 보입니다. | EmissionStrength를 `0.5`부터 올리고 Bloom은 나중에 확인합니다. |

`Simple Noise`는 그 자체로는 "밝은 곳과 어두운 곳이 섞인 무늬"일 뿐입니다. `Smoothstep`은 그 무늬의 경계선을 정리하고, `Lerp`는 밝은 곳을 어떤 색으로 바꿀지 결정합니다. 즉, **Noise가 위치를 고르고, Smoothstep이 경계를 선명하게 만들고, Lerp가 색을 입힙니다.**

## 6. 흐르는 물 Shader Graph 만들기

이 실습의 물은 **메시를 흔들지 않습니다.** 표면의 물 무늬가 이동하는 것처럼 보이게 하는 Fragment 단계 실습입니다. 실제로 Plane이 출렁이는 물결은 다음 시간 DAY06의 `Vertex Position` 실습에서 만듭니다.

### 완성 목표

- Plane 위에 파란 물 무늬가 보입니다.
- Play Mode에서 무늬가 한 방향으로 천천히 흐릅니다.
- Material Inspector에서 흐름 속도와 방향을 바꿀 수 있습니다.

### 준비할 리소스와 오브젝트

1. [WaterSurface_Albedo.png](Assets/DAY05_FlowingWater/WaterSurface_Albedo.png)를 Project 창에서 선택합니다.
2. Inspector에서 `Wrap Mode`를 `Repeat`, `Filter Mode`를 `Bilinear`로 설정하고 `Apply`를 누릅니다.
3. Hierarchy에서 `GameObject > 3D Object > Plane`을 만들고 이름을 `WaterSurface`로 바꿉니다.
4. `Create > Shader Graph > URP > Lit Shader Graph`를 선택해 `SG_FlowingWater`를 만듭니다.
5. `Create > Material`로 `Mat_FlowingWater`를 만들고 Shader를 `SG_FlowingWater`로 지정합니다. 이 Material을 `WaterSurface`의 `Mesh Renderer > Materials > Element 0`에 연결합니다.

### Blackboard Property 만들기

Graph의 Blackboard에서 아래 Property를 만들고 모두 `Exposed`를 켭니다. Property는 Material마다 바꿀 수 있는 손잡이입니다.

| 이름 | 타입 | 기본값 예시 | 역할 |
| :--- | :--- | :--- | :--- |
| `WaterTexture` | Texture2D | `WaterSurface_Albedo.png` | 물 표면 무늬 |
| `WaterTiling` | Vector2 | `(1.5, 1.5)` | 무늬 반복 횟수 |
| `FlowDirection` | Vector2 | `(0.05, 0.12)` | 흐르는 방향과 방향별 비율 |
| `FlowSpeed` | Float | `0.35` | 흐르는 전체 속도 |
| `EmissionStrength` | Float | `0.15` | 물 무늬를 조금 밝게 보이게 하는 값 |

### 노드를 한 줄씩 연결하기

아래 순서대로 노드를 추가합니다. 먼저 UV Offset까지만 연결해서 무늬가 흐르는지 확인하고, 그 다음 색과 Emission을 연결합니다.

1. `UV`, `Time`, `Multiply` 노드를 만듭니다.
2. `Time` 노드의 `Time` 출력과 `FlowSpeed`를 첫 번째 `Multiply`에 연결합니다. 이 값은 시간이 갈수록 커지는 숫자입니다.
3. 첫 번째 `Multiply` 결과와 `FlowDirection`을 두 번째 `Multiply`에 연결합니다. 결과는 시간이 흐를수록 바뀌는 `Vector2` Offset입니다.
4. `Tiling And Offset` 노드를 만들고, `UV` 출력은 `UV` 입력에, `WaterTiling`은 `Tiling` 입력에, 두 번째 `Multiply` 결과는 `Offset` 입력에 연결합니다.
5. `Sample Texture 2D` 노드를 만들고 `WaterTexture`를 `Texture` 입력에, `Tiling And Offset`의 `Out`을 `UV` 입력에 연결합니다.
6. `Sample Texture 2D`의 `RGB`를 Master Stack Fragment의 `Base Color`에 연결합니다.
7. `RGB`와 `EmissionStrength`를 세 번째 `Multiply`에 연결하고, 결과를 Master Stack Fragment의 `Emission`에 연결합니다.
8. Master Stack Fragment의 `Smoothness`에는 `0.85`를 입력합니다.

```text
Time ── Multiply(FlowSpeed) ── Multiply(FlowDirection) ── Offset
UV ──────────────────────────────────────────────────────── UV
WaterTiling ──────────────────────────────────────────────── Tiling
                                                            ↓
                                                   Tiling And Offset
                                                            ↓
WaterTexture ───────────────────────────────────── Sample Texture 2D
                                                            ├── RGB ──> Base Color
                                                            └── RGB × EmissionStrength ──> Emission
```

### 먼저 확인하고 다음 선을 연결하세요

| 단계 | 임시 연결 또는 설정 | Play Mode에서 볼 결과 | 안 되면 확인할 것 |
| :--- | :--- | :--- | :--- |
| 1 | `WaterTexture` → `Sample Texture 2D` → `Base Color` | Plane에 물 무늬가 보입니다. | Texture Property에 텍스처가 들어갔는지, Material이 Plane에 연결됐는지 확인합니다. |
| 2 | `UV` → `Tiling And Offset` → `Sample Texture 2D` | 무늬 크기가 `WaterTiling` 값에 따라 바뀝니다. | `Tiling And Offset`의 `Out`이 Sample Texture 2D의 `UV`에 연결됐는지 확인합니다. |
| 3 | `Time × FlowSpeed × FlowDirection` → `Offset` | 무늬가 한 방향으로 흐릅니다. | Texture의 Wrap Mode가 `Repeat`인지, `Time`의 `Time` 출력인지 확인합니다. |
| 4 | `RGB × EmissionStrength` → `Emission` | 어두운 곳에서도 물 무늬가 약간 살아납니다. | EmissionStrength를 `0.05`부터 올리고, Bloom은 나중에 따로 확인합니다. |

### Material Inspector 실험

`Mat_FlowingWater`를 선택하고 한 번에 값 하나만 바꿉니다.

| 바꿀 값 | 권장 실험 | 관찰할 결과 |
| :--- | :--- | :--- |
| `FlowSpeed` | `0` → `0.35` → `1` | 0이면 멈추고, 값이 클수록 무늬가 빨라집니다. |
| `FlowDirection` | `(0.05, 0.12)` → `(0.12, 0)` | 첫 값은 대각선, 두 번째 값은 가로 흐름입니다. |
| `WaterTiling` | `(1.5, 1.5)` → `(4, 4)` | 값이 클수록 작은 무늬가 더 많이 반복됩니다. |
| `EmissionStrength` | `0.15` → `0.5` | 밝아지지만 너무 크면 물이 발광체처럼 보입니다. |

### 자주 발생하는 문제

| 문제 | 원인 후보 | 해결 순서 |
| :--- | :--- | :--- |
| 물 텍스처가 한 번만 보이고 흐르다 끊긴다. | Texture Wrap Mode가 `Clamp`다. | `WaterSurface_Albedo.png`의 Wrap Mode를 `Repeat`로 바꾸고 Apply를 누릅니다. |
| 물 무늬가 멈춰 있다. | `Time` 또는 `Offset` 연결이 빠졌다. | `Time` 출력부터 두 `Multiply`, `Tiling And Offset > Offset`까지 선을 역순으로 확인합니다. |
| Plane 전체가 한 색으로만 보인다. | Texture Property 또는 Sample Texture 2D 연결이 빠졌다. | `WaterTexture`가 Texture 입력에 들어갔는지, `RGB`가 Base Color에 연결됐는지 확인합니다. |
| 물결처럼 표면이 위아래로 움직일 것으로 기대했는데 평평하다. | 이 실습은 UV 흐름만 구현한다. | DAY06에서 `Vertex Position`에 Sine Offset을 연결합니다. |

## 7. Shader Graph 디버깅 습관

Shader Graph는 중간 값을 눈으로 확인해야 빨리 고칠 수 있습니다.

| 확인 방법 | 설명 |
| :--- | :--- |
| 노드 Preview 보기 | 각 노드 아래 미리보기로 값이 나오는지 확인합니다. |
| 임시로 Base Color에 연결 | 중간 계산을 Base Color에 연결해 화면에서 직접 봅니다. |
| 값 범위 줄이기 | 너무 크거나 작은 값은 `Saturate`, `Clamp`, `Remap`으로 조절합니다. |
| 프로퍼티 이름 정리 | `Float1` 같은 이름을 그대로 두지 않고 의미 있는 이름으로 바꿉니다. |
| 한 번에 하나씩 추가 | Noise, Fresnel, Emission을 한 번에 만들지 말고 단계별로 확인합니다. |

## 표면 효과를 단계별로 검사하기

보호막 그래프는 Graph Inspector에서 `Surface Type = Transparent`, 필요하면 `Blend Mode = Alpha`를 먼저 설정합니다. Blackboard의 색·강도·속도·투명도 프로퍼티는 Exposed 상태인지 확인하고, Fresnel 결과는 가장자리만 밝아지는지 Main Preview에서 먼저 봅니다. Fresnel, Emission, Alpha를 모두 한 번에 연결하지 말고 하나씩 연결해 어느 선이 결과를 바꾸는지 확인합니다.

용암 그래프는 `UV`에 Time 기반 Offset을 더한 뒤 Noise에 연결합니다. Noise 출력은 먼저 Base Color에 연결해 움직이는 무늬만 확인하고, 그 다음 Color Ramp 또는 Multiply를 거쳐 Emission으로 보냅니다. Noise 값이 너무 대비가 약하면 Remap 또는 Power로 범위를 조절하고, 지나치게 밝으면 Emission 강도와 Bloom을 동시에 올리지 말고 하나씩 낮춥니다.

Material Inspector에서는 같은 Shader Graph를 쓰는 Material마다 색·속도·Emission 강도가 독립적으로 바뀌는지 확인합니다. 투명 보호막이 뒤의 물체를 이상하게 가리면 Surface Type, Alpha 입력, Blend Mode, Render Face 순서로 확인합니다.

## 오늘의 정리

- Shader Graph 표면 효과는 UV, Noise, 시간, 색, 투명도 조합으로 시작할 수 있습니다.
- 보호막은 Fresnel, 용암은 Noise와 Emission이 핵심 출발점입니다.
- 멋진 효과도 결국 작은 노드 계산이 쌓인 결과입니다.
- 다음 시간에는 꼭짓점 위치와 UV를 움직여 표면 자체가 움직이는 것처럼 보이게 합니다.
