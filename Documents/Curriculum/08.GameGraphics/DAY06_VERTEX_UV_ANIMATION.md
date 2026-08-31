# DAY 06: 정점 변형과 UV 애니메이션

오늘의 목표는 셰이더가 색만 바꾸는 도구가 아니라 "**표면의 점과 무늬를 움직이는 도구**"라는 점을 이해하는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 정점 셰이더를 사용한 변환 처리
- Unity 6 재구성: Shader Graph의 Vertex Position과 UV Offset을 사용합니다.

## 1. 정점과 UV의 차이

정점은 모델을 이루는 점입니다. UV는 텍스처를 어디에 붙일지 알려 주는 종이 도안 같은 좌표입니다. 정점을 움직이면 모델의 실루엣이 변하고, UV를 움직이면 표면 무늬가 흐르는 것처럼 보입니다.

### 이 단어는 무슨 뜻인가요?

- **Vertex**: 3D 모델을 이루는 꼭짓점입니다.
- **Vertex Shader**: 정점 위치를 처리하는 셰이더 단계입니다.
- **UV Animation**: 텍스처 좌표를 시간에 따라 움직이는 표현입니다.
- **Sine Wave**: 부드럽게 오르내리는 파도 같은 값입니다.

## 2. 실습: UV 흐름과 정점 물결을 결합한 물 표면

오늘은 DAY05의 "무늬만 흐르는 물"에 **정점 물결**을 더합니다. 아래 두 결과는 다르므로 반드시 구분해서 봅니다.

| 연결한 곳 | 눈으로 보이는 결과 | 뜻 |
| :--- | :--- | :--- |
| Fragment의 `Base Color`에 UV 흐름 | 평평한 Plane 위에서 물 무늬만 이동합니다. | 텍스처 좌표가 움직입니다. |
| Vertex의 `Vertex Position`에 파도 Offset | Plane의 격자·실루엣 자체가 위아래로 움직입니다. | 메시 정점 위치가 움직입니다. |

### 완성 목표

- 격자 텍스처에서는 Plane이 부드럽게 위아래로 굽는 것이 보입니다.
- 물 텍스처에서는 물 무늬가 흐르면서 표면도 출렁입니다.
- Material Inspector의 `Amplitude`, `WaveFrequency`, `WaveSpeed`를 바꾸면 높이·파도 간격·속도가 각각 바뀝니다.

### 실습 Asset과 Inspector 준비

1. Hierarchy에서 `GameObject > 3D Object > Plane`을 만들고 이름을 `AnimatedSurface`로 바꿉니다. **Quad는 정점이 네 개뿐이므로 UV 흐름 확인에는 쓸 수 있어도, 부드러운 정점 물결 시험에는 적합하지 않습니다.**
2. `Create > Shader Graph > URP > Lit Shader Graph`로 `SG_VertexWave`를 만듭니다.
3. `GameGraphics/Materials`에서 `Mat_VertexWave`를 만들고 Shader를 `SG_VertexWave`로 지정한 뒤, `AnimatedSurface`의 `Mesh Renderer > Materials > Element 0`에 연결합니다.
4. Scene View에서 Camera가 Plane을 위에서만 보지 않도록, Main Camera를 Plane 옆의 낮은 각도로 옮깁니다. 그래야 위아래 높이 변화가 보입니다.
5. 아래 Property를 Blackboard에서 만들고 모두 `Exposed`를 켭니다.

| 이름 | 타입 | 기본값 예시 | 역할 |
| :--- | :--- | :--- | :--- |
| `WaveTexture` | Texture2D | `VertexWave_TestGrid.png` | 처음에는 정점 변형을 보기 쉬운 격자 텍스처를 사용합니다. |
| `Amplitude` | Float | `0.15` | 정점이 위아래로 움직이는 최대 높이입니다. |
| `WaveFrequency` | Float | `2` | Plane 한 칸에 나타나는 파도의 촘촘함입니다. |
| `WaveSpeed` | Float | `1.5` | 파도가 진행하는 속도입니다. |
| `UvTiling` | Vector2 | `(1, 1)` | 텍스처 반복 횟수입니다. |
| `UvFlowDirection` | Vector2 | `(0.03, 0.08)` | 물 무늬가 흐르는 방향입니다. |
| `UvFlowSpeed` | Float | `0.2` | 물 무늬가 흐르는 속도입니다. |

### 정점 물결 노드: 무엇을 어떤 순서로 연결하나요?

Plane의 Object Space에서는 X가 좌우, Y가 높이, Z가 앞뒤입니다. 이 실습은 X 위치마다 다른 `Sine` 값을 계산해, 그 결과를 **Y 방향 Offset**으로 만듭니다.

| 노드 | 하는 일 | 이 실습에서 확인할 출력 |
| :--- | :--- | :--- |
| `Position` (Space: `Object`) | 각 정점의 현재 위치를 가져옵니다. | `X`와 원래 Object Position |
| `Split` | Vector3 Position을 X, Y, Z로 나눕니다. | X에 해당하는 `R` 출력 |
| `Time` | 시간에 따라 계속 커지는 숫자를 만듭니다. | `Time` 출력 |
| `Multiply` | 위치·시간에 주파수와 속도를 곱합니다. | 파도 간격과 진행 속도를 조절한 값 |
| `Add` | 위치 기반 값과 시간 기반 값을 합칩니다. | 움직이는 파도의 입력값 |
| `Sine` | 숫자를 -1~1의 부드러운 파형으로 바꿉니다. | 위아래 방향 파도값 |
| `Combine` | X, Y, Z 값을 Vector3으로 묶습니다. | `(0, waveHeight, 0)` Offset |
| `Vertex Position` | Object Space의 최종 정점 위치를 받습니다. | 원래 위치 + Y 방향 Offset |

### 정점 물결을 한 줄씩 연결하기

1. `Position` 노드를 만들고 Space를 `Object`로 설정합니다. `Split` 노드를 만들고 Position을 Split의 입력에 연결합니다.
2. `Split`의 `R` (X)을 첫 번째 `Multiply`에, `WaveFrequency`를 같은 Multiply의 다른 입력에 연결합니다.
3. `Time` 노드의 `Time` 출력을 두 번째 `Multiply`에, `WaveSpeed`를 다른 입력에 연결합니다.
4. 두 Multiply 결과를 `Add`에 연결합니다. **위치값 + 시간값**이므로, 정점마다 다른 시점에 같은 파도가 지나갑니다.
5. `Add` 결과를 `Sine`에 연결합니다. 이때 출력은 -1에서 1까지 부드럽게 오르내립니다.
6. `Sine` 출력과 `Amplitude`를 세 번째 `Multiply`에 연결합니다. 이것이 최종 높이 `waveHeight`입니다.
7. `Combine` 노드를 만들고 `R = 0`, `G = waveHeight`, `B = 0`으로 연결합니다. Unity의 Y축은 위아래이므로 `G`에 넣습니다.
8. 처음 `Position (Object)` 출력과 Combine 결과를 `Add`로 더합니다. 이 마지막 Add 결과를 Master Stack Vertex 영역의 `Vertex Position`에 연결합니다.

```text
Position (Object) ── Split ── R(X) × WaveFrequency ──┐
                                                     Add ── Sine ── × Amplitude ── Combine(0, Y, 0)
Time ── × WaveSpeed ────────────────────────────────┘                              │
Position (Object) ───────────────────────────────────────────────────────────────── Add ──> Vertex Position
```

`Vertex Position`은 **Object Space 최종 위치**를 받습니다. 그래서 원래 `Position (Object)`에 Object Space Y Offset을 더합니다. World Space Position을 그대로 더하면 좌표 기준이 달라, 오브젝트를 옮기거나 회전할 때 예상과 다르게 보일 수 있습니다.

### 선택 심화: 두 방향 잔물결 추가하기

앞의 기본 그래프는 X축 하나만 사용하므로, 한 방향으로만 파도가 진행합니다. 여기서는 Split의 `B` (Z) 출력으로 두 번째 파도를 만들고 기존 파도와 더합니다. 두 방향의 파도가 만나는 지점에서는 높이가 합쳐져, 물 표면이 더 자연스럽게 흔들립니다.

1. Blackboard에 아래 Property를 새로 만들고 `Exposed`를 켭니다.

| 이름 | 타입 | 기본값 예시 | 역할 |
| :--- | :--- | :--- | :--- |
| `CrossWaveFrequency` | Float | `1.6` | Z축 방향 보조 파도의 촘촘함입니다. |
| `CrossWaveSpeed` | Float | `1.1` | Z축 방향 보조 파도의 진행 속도입니다. |
| `CrossWaveStrength` | Float | `0.5` | 보조 파도가 기본 파도에 섞이는 비율입니다. |

2. 기존 X축 파도 그래프는 그대로 둡니다. `Sine × Amplitude`의 결과를 `waveX`라고 생각합니다.
3. 같은 `Position (Object)`의 `Split` 노드에서 `B` (Z)를 새 `Multiply`에 연결하고, `CrossWaveFrequency`를 다른 입력에 연결합니다.
4. `Time`의 `Time` 출력을 새 `Multiply`에 연결하고, `CrossWaveSpeed`를 다른 입력에 연결합니다.
5. 두 Multiply 결과를 새 `Add`로 합친 뒤 `Sine`에 연결합니다.
6. 이 `Sine` 출력 × `Amplitude` × `CrossWaveStrength`의 결과를 만듭니다. 이것이 `waveZ`입니다.
7. `waveX`와 `waveZ`를 `Add`로 더합니다. 두 값이 같은 순간에 최대가 되어 물결이 너무 높아지지 않도록 결과에 `0.5`를 곱합니다.
8. 기존 Combine 노드의 `G` 입력을 `waveX` 대신 이 최종 결과로 바꿉니다. `R = 0`, `B = 0`은 그대로 둡니다.

```text
X branch: Split.R × WaveFrequency + Time × WaveSpeed
          └──────────────────────────────────────────> Sine × Amplitude ── waveX

Z branch: Split.B × CrossWaveFrequency + Time × CrossWaveSpeed
          └──────────────────────────────────────────> Sine × Amplitude × CrossWaveStrength ── waveZ

waveX + waveZ ── × 0.5 ──> Combine(0, Y, 0) ──> Vertex Position
```

| Inspector 값 | 처음 시험할 값 | 관찰할 결과 |
| :--- | :--- | :--- |
| `CrossWaveStrength` | `0` | 기존과 같은 한 방향 진행파입니다. |
| `CrossWaveStrength` | `0.5` | X축과 Z축 파도가 교차하는 잔물결입니다. |
| `CrossWaveFrequency` | `1.6` → `3` | 값이 클수록 보조 파도 간격이 짧아집니다. |
| `CrossWaveSpeed` | `1.1` → `2` | 값이 클수록 보조 파도가 빨라집니다. |

> 두 방향을 모두 `WaveFrequency = 2`, `WaveSpeed = 1.5`로 두면 규칙적인 격자 물결처럼 보일 수 있습니다. 보조 파도에는 기본 파도와 조금 다른 값 (`1.6`, `1.1`)을 주면 더 자연스럽게 보입니다.

### 단계별 테스트 리소스

아래 두 리소스는 한 번에 완성된 물결만 확인하지 않고, 어떤 연결이 결과를 바꿨는지 순서대로 확인하기 위한 자료입니다.

| 리소스 | 용도 | Unity Import 설정 |
| :--- | :--- | :--- |
| [WaterSurface_Albedo.png](Assets/DAY05_FlowingWater/WaterSurface_Albedo.png) | DAY05에서 만든 UV 흐름을 재사용해 최종 물결을 확인하는 기본 물 표면 텍스처 | `Wrap Mode = Repeat`, `Filter Mode = Bilinear` |
| [VertexWave_TestGrid.png](Assets/DAY06_WaterWave/VertexWave_TestGrid.png) | 정점이 위아래로 움직일 때 격자선과 흰 점이 굽는지 확인하는 대비용 텍스처 | `Wrap Mode = Repeat`, `Filter Mode = Point` 또는 `Bilinear` |

기본 물 텍스처만으로도 최종 결과를 만드는 데는 충분합니다. 하지만 물 무늬도 움직이고 정점도 동시에 움직이면, 무엇이 성공했고 무엇이 실패했는지 구분하기 어렵습니다. 아래의 **단계별 Play Mode 테스트**에서 격자 → 정점 물결 → 최종 물 텍스처 순서로 확인합니다.

> Plane 기본 메시처럼 정점 수가 적으면 격자선이 거의 굽지 않고 큰 조각 단위로 움직입니다. 이 경우 텍스처 문제가 아니라 메시 밀도 문제이므로, 정점이 더 촘촘한 Plane을 사용해 다시 확인합니다.

### UV 흐름 노드 연결하기

UV 흐름은 DAY05에서 배운 방식과 같습니다. 이번에는 정점 물결과 섞기 전에 **무늬만 움직이는 상태**를 먼저 확인합니다.

1. `UV`, `Tiling And Offset`, `Sample Texture 2D` 노드를 만듭니다.
2. `UV` 출력은 `Tiling And Offset`의 `UV` 입력에, `UvTiling`은 `Tiling` 입력에 연결합니다.
3. `Time`의 `Time` × `UvFlowSpeed` × `UvFlowDirection` 결과를 `Tiling And Offset`의 `Offset`에 연결합니다.
4. `Tiling And Offset`의 `Out`을 `Sample Texture 2D`의 `UV` 입력에 연결합니다.
5. `WaveTexture`를 Sample Texture 2D의 `Texture`에, `RGB` 출력을 Fragment의 `Base Color`에 연결합니다.

```text
Time × UvFlowSpeed × UvFlowDirection ──> Tiling And Offset : Offset
UV ────────────────────────────────────> Tiling And Offset : UV
UvTiling ──────────────────────────────> Tiling And Offset : Tiling
Tiling And Offset : Out ───────────────> Sample Texture 2D : UV ── RGB ──> Base Color
```

### 단계별 Play Mode 테스트

| 순서 | 연결 상태 | 확인할 결과 | 실패했을 때 |
| :--- | :--- | :--- | :--- |
| 1 | `WaveTexture` → Sample Texture 2D → `Base Color` | Plane에 격자 무늬가 보입니다. | Material과 Texture Property 연결을 확인합니다. |
| 2 | UV 흐름까지 연결, `Vertex Position`은 비워 둠 | 평평한 Plane에서 격자 무늬만 흐릅니다. | `Tiling And Offset`의 Out이 Sample Texture 2D UV에 연결됐는지 확인합니다. |
| 3 | 정점 물결을 `Vertex Position`에 연결, UV 흐름은 `0` | 격자선과 흰 점이 위아래로 출렁입니다. | Combine의 `G`에 waveHeight를 넣었는지, 낮은 각도의 Camera인지 확인합니다. |
| 4 | UV 흐름과 정점 물결을 둘 다 연결 | 무늬가 흐르면서 Plane도 출렁입니다. | 둘 중 하나를 임시로 끄고 2·3단계부터 다시 확인합니다. |
| 5 | 텍스처를 WaterSurface_Albedo로 교체 | 자연스러운 최종 물 표면이 보입니다. | Wrap Mode가 `Repeat`인지, Amplitude가 너무 크지 않은지 확인합니다. |

### 자주 발생하는 문제

| 문제 | 원인 후보 | 확인·수정 순서 |
| :--- | :--- | :--- |
| 텍스처는 흐르는데 Plane이 평평하다. | 마지막 Add가 `Vertex Position`이 아니라 Fragment에 연결됐다. | Master Stack의 **Vertex** 영역 `Vertex Position`에 연결했는지 확인합니다. |
| Plane 전체가 한 덩어리처럼 기울어진다. | 모든 정점에 같은 waveHeight를 썼다. | `Position (Object) > Split > R(X)`가 WaveFrequency와 곱해졌는지 확인합니다. |
| 물결이 너무 뾰족하거나 빠르다. | Amplitude·WaveFrequency·WaveSpeed가 너무 크다. | `0.15`, `2`, `1.5`부터 하나씩 조절합니다. |
| 격자 무늬가 끊기거나 사라진다. | Texture Wrap Mode가 `Clamp`거나 UvFlow 값이 너무 크다. | `Repeat`로 바꾸고 UvFlowSpeed를 `0.2`부터 확인합니다. |
| 가까이에서는 출렁이는데 멀리서 일부가 잘린다. | 변형이 Renderer Bounds 밖으로 나간다. | Amplitude를 낮추고, 필요하면 메시 Bounds를 넉넉하게 설정합니다. |

## 주의할 점

- 정점 수가 너무 적으면 부드럽게 변형되지 않습니다.
- 움직임 값이 너무 크면 모델이 찢어진 것처럼 보일 수 있습니다.
- 충돌 영역은 셰이더 변형을 따라가지 않습니다. 보이는 모양과 물리 판정은 다를 수 있습니다.

## Vertex·UV 애니메이션 연결 점검

Vertex 변형은 Graph Inspector의 Vertex 영역에서 처리합니다. 오브젝트마다 자기 축 기준으로 흔들리게 하려면 `Position` 노드 Space를 `Object`로 두고 `Time`과 `Sine`으로 만든 Object Space Offset을 더해 `Vertex Position` 블록에 연결합니다. 여러 오브젝트를 가로지르는 같은 방향의 바람을 만들려면 `Position (World)`에서 Noise·Time을 계산한 뒤, `Transform` 노드의 `From = World`, `To = Object`로 Offset을 변환해 원래 Object Position에 더합니다. `Vertex Position`은 Object Space 최종 위치를 받으므로 Object Position과 World Offset을 그대로 더지 않습니다.

처음에는 Amplitude를 작은 값으로 두고, 움직임이 너무 크면 모델이 원래 위치에서 멀어지거나 Bounds 밖으로 나갈 수 있다는 점을 확인합니다. Vertex Position에 선이 연결되지 않고 Fragment의 Base Color에만 연결되면 색만 변하고 메시가 흔들리지는 않습니다. 반대로 Base Color·Emission에 World Space Position으로 계산한 Noise를 쓰는 경우에는 Fragment 색 계산이므로 Object Space로 다시 변환할 필요가 없습니다.

UV 애니메이션은 `UV`에 Time 기반 Vector2 Offset을 더한 결과를 Sample Texture 2D의 UV 입력에 연결합니다. Offset X만 바꾸면 가로 흐름, Y만 바꾸면 세로 흐름입니다. 재생 중 텍스처가 끊기는 것처럼 보이면 Texture Import Settings의 Wrap Mode를 `Repeat`로 확인하고, 고정된 그림처럼 보이면 Time 출력과 Speed 값 연결을 확인합니다.

Shader Graph의 변형은 렌더링 결과만 바꿉니다. Collider, NavMesh, Raycast 판정은 자동으로 따라가지 않으므로, 보이는 파도나 흔들림을 실제 충돌 높이로 사용하면 안 됩니다.

## 오늘의 정리

- 정점 변형은 보이는 모델의 위치를 셰이더 단계에서 바꿉니다.
- UV 애니메이션은 텍스처 무늬를 움직입니다.
- 다음 시간에는 셰이더 코드를 짧게 읽어 Shader Graph 뒤에서 어떤 일이 일어나는지 감각을 잡습니다.
