# DAY 04: 셰이더 실행 구조와 Shader Graph 기초

오늘의 목표는 셰이더가 정점과 화면 조각을 어떻게 계산하는지 먼저 이해하고, Shader Graph를 "**그 계산을 선으로 조립하는 도구**"로 사용해 색이 변하는 머티리얼을 만드는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 셰이더 알고리즘 이해 및 사용
- Unity 6 재구성: 정점·Fragment 단계의 실행 구조를 Shader Graph의 Vertex·Fragment 블록과 연결합니다.

## 1. Shader는 어떤 순서로 동작하나요?

DAY 01에서는 SRP/URP가 어떤 오브젝트를 어떤 Render Pass 순서로 그릴지 결정한다는 점을 배웠습니다. 이제는 그 Draw Call 안에서 **Shader가 오브젝트 하나를 어떻게 계산하는지** 봅니다. SRP/URP는 "무엇을 그릴지"를 지휘하고, Shader는 "선택된 메시의 모양과 표면 색을 어떻게 계산할지"를 처리합니다.

```text
SRP/URP의 Renderer 단위 Culling
  Camera에 보이지 않는 Renderer는 Draw Call을 만들지 않음
        ↓
Mesh의 정점 데이터 (Position, Normal, UV)
        ↓
Vertex 단계: 각 정점의 최종 위치와 다음 단계에 전달할 값을 계산
        ↓
삼각형 조립·Backface Culling: 뒷면 삼각형을 제외할 수 있음
        ↓
Rasterization: 남은 삼각형 내부를 Fragment 후보로 채움
        ↓
Fragment/Pixel 단계: 각 Fragment의 색·투명도·발광을 계산
        ↓
Depth·Stencil·Alpha Clip 검사, Blend 처리 후 Camera 화면에 출력
```

### 이 단어는 무슨 뜻인가요?

- **Vertex**: 메시를 이루는 꼭짓점입니다. Cube의 모서리, 캐릭터 메시의 표면도 많은 Vertex로 이루어집니다.
- **Vertex 단계**: 각 Vertex의 위치를 계산합니다. 여기서 위치를 바꾸면 메시의 실루엣이 흔들리거나 부풀 수 있습니다.
- **Renderer 단위 Culling**: SRP/URP가 Camera Frustum, Layer 등의 조건으로 Renderer 전체를 Draw Call에 넣을지 먼저 판단하는 과정입니다. 이 단계는 Vertex Shader보다 앞입니다.
- **Backface Culling**: 정점 변환 뒤 삼각형의 앞면·뒷면을 판단해, 보통 Camera 반대쪽을 향한 뒷면을 Rasterization하지 않는 과정입니다.
- **Rasterization**: 삼각형으로 둘러싼 영역을 화면에 그릴 작은 Fragment 후보로 나누는 과정입니다.
- **Fragment/Pixel 단계**: 각 Fragment의 표면 색·알파·발광 등을 계산하는 단계입니다. 이후 깊이·스텐실·Alpha Clip 검사를 통과한 결과가 기존 화면 색과 Blend되어 픽셀값을 갱신합니다.
- **Material Property**: Material Inspector에서 넣는 색, 텍스처, 숫자입니다. Shader 계산에 전달되는 입력값입니다.

처음에는 "Vertex는 모양·위치, Fragment는 표면 색"으로 구분하면 충분합니다. 조명, 그림자, 깊이 테스트의 내부 구현을 모두 외우는 것이 오늘 목표는 아닙니다.

### Shader Graph에 대응시키기

| Shader 실행 위치 | Shader Graph에서 주로 연결하는 곳 | 눈으로 확인하는 결과 |
| :--- | :--- | :--- |
| Vertex 단계 | Master Stack의 `Vertex Position` | 메시 자체가 흔들리거나 부풀고 위치가 변합니다. |
| Fragment 단계 | `Base Color`, `Alpha`, `Emission`, `Normal` | 메시 모양은 그대로이고 색·투명도·발광·표면 음영이 변합니다. |
| Material 입력 | Blackboard의 Exposed Property와 Material Inspector | 같은 Graph를 써도 Material마다 색·속도·강도를 다르게 조절합니다. |
| 계산 재료 | `Time`, `UV`, `Noise`, `Multiply` 같은 Node | 이 Node가 어느 출력에 연결되는지에 따라 Vertex 또는 Fragment 계산에 사용됩니다. |

예를 들어 `Time → Sine → Multiply → Base Color`는 표면 색이 변하는 Fragment 계산이고, 같은 값을 `Vertex Position`에 연결하면 메시 위치가 변하는 Vertex 계산입니다. Node 이름만 외우지 말고 **어느 최종 입력으로 가는 선인가**를 먼저 봅니다.

### Position 노드의 Space는 무엇인가요?

`Position` 노드의 `Space` 드롭다운은 "어느 좌표 공간의 Position 값을 꺼낼지"를 정합니다. 이 설정은 그래프 전체에 한 번만 적용하는 전역 옵션이 아니라, **Position 노드마다 선택하는 값의 기준**입니다. 방향·법선 계열 노드도 같은 방식으로 각 노드의 Space를 선택합니다.

| Space | Position 값의 기준 | 어울리는 계산 | 주의 |
| :--- | :--- | :--- | :--- |
| `Object` | 오브젝트 자신의 로컬 원점 기준 | 검, 나무 한 개, 캐릭터 한 명처럼 오브젝트를 따라가는 변형 | Transform이 이동·회전해도 계산 패턴이 오브젝트와 함께 움직이며, `Vertex Position`의 최종 입력은 Object Space 위치입니다. |
| `World` | 씬 전체의 월드 좌표 기준 | 여러 오브젝트를 가로지르는 바람·파도·Noise 패턴 | URP에서는 일반적으로 Absolute World와 같은 기준으로 생각해도 됩니다. |
| `Absolute World` | 카메라 상대 보정이 없는 절대 월드 좌표 | 넓은 월드에 고정된 패턴, 파이프라인 차이를 명확히 해야 하는 계산 | 이 과정의 URP에서는 World와 큰 차이를 느끼기 어렵습니다. |
| `View` | Camera를 원점으로 보는 좌표 | 카메라 거리·방향에 따른 효과 | Camera가 움직이면 값도 달라집니다. |
| `Tangent` | 메시 표면의 Tangent·Bitangent·Normal 축 기준 | Normal Map, 표면 결을 따른 방향 계산 | 일반 위치 변형의 첫 선택은 아닙니다. |

`Screen Position`은 Position 노드의 Space가 아니라 별도 노드입니다. 화면 왜곡, 화면 가장자리 효과처럼 화면 좌표가 필요한 Fragment 계산에서 주로 사용합니다.

#### Vertex Position에 넣을 때의 공간 규칙

`Vertex Position` 블록의 최종 입력은 Object Space 위치입니다. 따라서 World Space에서 바람이나 Noise를 계산했더라도, 최종 결과를 바로 더하지 않고 Object Space로 변환한 뒤 연결합니다. Object Position과 World Offset을 그대로 더하면 서로 다른 기준의 값을 섞는 오류가 됩니다.

```text
Object Position
        ↓ Object → World
World Space에서 Time·Noise로 Offset 계산
        ↓ World → Object
Object Position + 변환된 Offset
        ↓
Vertex Position
```

Fragment의 `Base Color`, `Alpha`, `Emission`은 위치가 아니라 표면의 보이는 값을 받습니다. 따라서 World Space Position으로 계산한 Noise 결과를 색 계산에 사용하는 것은 가능하며, 그 경우 Object Space로 되돌릴 필요가 없습니다.

## 2. Shader Graph란?

Shader Graph는 셰이더 코드를 직접 쓰지 않고 노드를 연결해 셰이더를 만드는 도구입니다. Unity 6 공식 문서에서는 Shader Graph가 코드를 쓰는 대신 그래프 프레임워크에서 노드를 만들고 연결하며, 변경 결과를 즉시 확인할 수 있는 도구라고 설명합니다.

### Graph에서 쓰는 용어

- **Node**: 색, 숫자, 좌표, 연산 같은 작은 기능 블록입니다.
- **Edge**: 노드와 노드를 연결하는 선입니다.
- **Blackboard**: 그래프 밖에서 조절할 수 있는 프로퍼티를 모아 두는 영역입니다.
- **Master Stack**: Vertex와 Fragment 계산의 최종 출력이 모이는 곳입니다.
- **Preview**: 그래프 결과를 작은 창에서 바로 보는 기능입니다.

## 3. 실습: Fragment 색이 바뀌는 머티리얼

**미션:** 메시 모양은 바꾸지 않고 Fragment 단계의 `Base Color` 또는 `Emission`만 바꾸는 Graph를 만듭니다.

1. `Create > Shader Graph > URP > Lit Shader Graph`를 선택합니다.
2. 이름을 `SG_ColorPulse`로 지정합니다.
3. Blackboard에 `BaseColor`, `PulseSpeed` 프로퍼티를 추가합니다.
4. `Time`, `Sine`, `Multiply`, `Color` 노드를 연결해 색이 천천히 변하게 만듭니다.
5. 그래프를 저장하고 머티리얼을 만들어 Sphere에 적용합니다.

## 4. Shader Graph 창 사용법

Shader Graph 창은 처음 보면 복잡하지만, 책상 위 작업 공간처럼 나누어 보면 쉽습니다.

| 영역 | 하는 일 | 주로 하는 작업 |
| :--- | :--- | :--- |
| Blackboard | 머티리얼 Inspector에 노출할 값을 만듭니다. | 색, 속도, 세기 같은 프로퍼티 추가 |
| Graph Area | 노드를 놓고 선으로 연결합니다. | `Space` 또는 우클릭으로 노드 검색 |
| Graph Inspector | 선택한 노드나 그래프 설정을 바꿉니다. | Surface Type, Blend Mode, Two Sided 설정 |
| Main Preview | 현재 셰이더 결과를 미리 봅니다. | 저장 전 색과 움직임 확인 |
| Master Stack | Vertex와 Fragment의 최종 출력 위치입니다. | Vertex Position 또는 Base Color, Alpha, Emission 등에 연결 |

### 새 프로퍼티 만들기

1. Blackboard의 `+` 버튼을 누릅니다.
2. `Color`, `Float`, `Vector2`, `Texture2D` 중 필요한 타입을 고릅니다.
3. 이름을 알아보기 쉽게 정합니다. 예: `BaseColor`, `PulseSpeed`
4. 프로퍼티를 Graph Area로 끌어다 놓으면 노드처럼 사용할 수 있습니다.
5. 저장 후 머티리얼 Inspector에서 값이 보이는지 확인합니다.

프로퍼티는 "**머티리얼마다 조절 가능한 손잡이**"입니다. 같은 Shader Graph를 쓰더라도 머티리얼 A는 파란색, 머티리얼 B는 빨간색으로 다르게 만들 수 있습니다.

### 노드 추가와 연결

1. Graph Area에서 `Space`를 누르거나 우클릭합니다.
2. 검색창에 `Time`, `Sine`, `Multiply`처럼 필요한 노드 이름을 입력합니다.
3. 노드의 오른쪽 포트는 결과가 나가는 곳, 왼쪽 포트는 값이 들어오는 곳입니다.
4. 포트끼리 선으로 연결합니다.
5. 잘못 연결했다면 선을 선택해 지우거나 다른 포트로 다시 연결합니다.

```text
Time -> Sine -> Multiply -> Base Color 또는 Emission
```

위 흐름은 시간이 흐를수록 값이 변하고, 그 값을 색이나 밝기에 연결한다는 뜻입니다.

## 5. 색이 바뀌는 Fragment 그래프 구성

초보자는 아래 순서대로 연결하면 됩니다.

| 단계 | 노드 | 연결 |
| :--- | :--- | :--- |
| 1 | `Time` | `Time` 출력 사용 |
| 2 | `Multiply` | `Time`과 `PulseSpeed`를 곱함 |
| 3 | `Sine` | 곱한 값을 부드러운 반복 값으로 바꿈 |
| 4 | `Remap` 또는 `Add/Multiply` | `-1~1` 값을 색에 쓰기 좋은 범위로 조절 |
| 5 | `Lerp` | 어두운 색과 밝은 색을 섞음 |
| 6 | Master Stack Fragment 영역 | `Base Color` 또는 `Emission`에 연결 |

값이 너무 빨리 바뀌면 `PulseSpeed`를 낮춥니다. 색 변화가 너무 약하면 `Lerp`에 들어가는 두 색의 차이를 크게 잡습니다.

## 6. 머티리얼에 적용하기

1. Shader Graph를 저장합니다.
2. Project 창에서 `Create > Material`을 선택합니다.
3. 머티리얼의 Shader를 방금 만든 `SG_ColorPulse`로 변경합니다.
4. Sphere나 Cube의 Mesh Renderer에 머티리얼을 넣습니다.
5. Play 모드 또는 Scene 뷰에서 색 변화가 보이는지 확인합니다.

## 노드 읽는 순서

Shader Graph도 코드처럼 `위->아래`, `오->왼`, `안->밖`으로 읽을 수 있습니다. 먼저 Master Stack에서 Vertex Position인지, Base Color·Emission 같은 Fragment 출력인지 확인한 뒤 그 입력 쪽으로 거꾸로 따라갑니다.

- 숫자와 색 프로퍼티가 입력입니다.
- Time과 Sine이 시간에 따른 변화를 만듭니다.
- Multiply가 변화량을 조절합니다.
- Master Stack의 Base Color가 Fragment 단계의 최종 표면 색을 받습니다.

## 자주 막히는 지점

| 증상 | 확인할 것 |
| :--- | :--- |
| 머티리얼 Inspector에 값이 보이지 않음 | Blackboard 프로퍼티로 만들었는지, 그래프를 저장했는지 확인 |
| 오브젝트가 분홍색으로 보임 | URP용 Shader Graph인지, 그래프 저장 중 오류가 없는지 확인 |
| 투명도가 적용되지 않음 | Graph Inspector에서 Surface Type을 Transparent로 바꿨는지 확인 |
| 색이 움직이지 않음 | `Time` 노드가 연결되어 있는지, Play 모드 또는 Preview에서 보고 있는지 확인 |
| 너무 밝게 번짐 | Emission에 연결한 값이 과하지 않은지 확인 |
| 메시가 움직일 것으로 예상했는데 색만 변함 | 선이 `Vertex Position`이 아니라 Base Color·Emission 같은 Fragment 입력에 연결됐는지 확인 |

## Shader Graph와 Material Inspector 확인 절차

Blackboard에서 `BaseColor`는 Color, `PulseSpeed`는 Float로 만듭니다. 각 프로퍼티를 선택해 Reference 이름이 공백 없이 고유한지, `Exposed`가 켜져 있는지 확인합니다. Exposed가 꺼져 있으면 그래프 안에서는 값을 써도 Material Inspector의 조절 항목으로 나타나지 않습니다.

Graph Inspector에서는 Target이 URP인지, Surface Type이 불투명 표면이면 `Opaque`인지 투명 표면이면 `Transparent`인지 확인합니다. Graph Area에서는 `Time` 출력에 `PulseSpeed`를 곱하고, 그 결과를 Master Stack Fragment 영역의 색 또는 Emission 변화에 연결합니다. 선을 연결한 뒤 Main Preview에서 메시 모양은 유지되고 표면 색만 변하는지 확인한 뒤 저장합니다.

그 다음 Project 창에서 이 그래프를 Shader로 쓰는 Material을 만들고 Inspector를 엽니다. `BaseColor`, `PulseSpeed`가 보이면 Exposed Property 연결이 성공한 것입니다. Sphere의 Mesh Renderer에 이 Material을 연결한 뒤 BaseColor를 바꾸어도 다른 Material을 쓰는 Sphere까지 바뀌면 안 됩니다. 그 현상이 생기면 Material Asset을 공유한 것인지, Renderer의 Material 슬롯이 올바른지 확인합니다.

## 오늘의 정리

- SRP/URP는 그릴 순서를 정하고, Shader는 Draw Call 안에서 Vertex와 Fragment 계산을 수행합니다.
- Shader Graph는 이 계산을 노드로 조립하는 도구입니다.
- `Vertex Position`은 모양·위치, `Base Color`·`Alpha`·`Emission`은 표면 표현과 연결됩니다.
- Blackboard 프로퍼티를 만들면 머티리얼마다 값을 다르게 줄 수 있습니다.
- 다음 시간에는 Fragment 단계의 UV, Noise, Emission을 조합해 용암, 물, 보호막 같은 표면 표현을 만듭니다.
