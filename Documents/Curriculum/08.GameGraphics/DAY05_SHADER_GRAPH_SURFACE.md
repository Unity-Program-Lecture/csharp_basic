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

### 깜박임 추가하기

1. `Time` 노드의 Time 출력을 `Multiply`에 연결합니다.
2. 다른 입력에는 `PulseSpeed`를 연결합니다.
3. `Sine` 노드로 부드럽게 반복되는 값을 만듭니다.
4. `Remap`으로 값을 `0.5~1.0` 정도로 바꿉니다.
5. 이 값을 Emission 계산에 곱합니다.

깜박임은 보호막이 작동 중이라는 느낌을 줍니다. 단, 너무 빠르면 눈이 피곤하므로 처음에는 느리게 설정합니다.

## 5. 용암 표면 Shader Graph 만들기

용암은 균열 무늬와 발광이 중요합니다.

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

## 6. Shader Graph 디버깅 습관

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
