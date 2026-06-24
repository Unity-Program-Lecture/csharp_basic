# DAY 04: Shader Graph 입문

오늘의 목표는 Shader Graph를 "**코드를 선으로 연결하는 셰이더 조립판**"으로 이해하고, 노드를 연결해 색이 변하는 머티리얼을 만드는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 셰이더 알고리즘 이해 및 사용
- Unity 6 재구성: Shader Graph로 시각적 셰이더 제작을 시작합니다.

## 1. Shader Graph란?

Shader Graph는 셰이더 코드를 직접 쓰지 않고 노드를 연결해 셰이더를 만드는 도구입니다. Unity 6 공식 문서에서는 Shader Graph가 코드를 쓰는 대신 그래프 프레임워크에서 노드를 만들고 연결하며, 변경 결과를 즉시 확인할 수 있는 도구라고 설명합니다.

### 이 단어는 무슨 뜻인가요?

- **Node**: 색, 숫자, 좌표, 연산 같은 작은 기능 블록입니다.
- **Edge**: 노드와 노드를 연결하는 선입니다.
- **Blackboard**: 그래프 밖에서 조절할 수 있는 프로퍼티를 모아 두는 영역입니다.
- **Master Stack**: 최종 표면 색, 알파, 노멀 같은 출력이 모이는 곳입니다.
- **Preview**: 그래프 결과를 작은 창에서 바로 보는 기능입니다.

## 2. 실습: 색이 바뀌는 머티리얼

1. `Create > Shader Graph > URP > Lit Shader Graph`를 선택합니다.
2. 이름을 `SG_ColorPulse`로 지정합니다.
3. Blackboard에 `BaseColor`, `PulseSpeed` 프로퍼티를 추가합니다.
4. `Time`, `Sine`, `Multiply`, `Color` 노드를 연결해 색이 천천히 변하게 만듭니다.
5. 그래프를 저장하고 머티리얼을 만들어 Sphere에 적용합니다.

## 3. Shader Graph 창 사용법

Shader Graph 창은 처음 보면 복잡하지만, 책상 위 작업 공간처럼 나누어 보면 쉽습니다.

| 영역 | 하는 일 | 학생이 자주 하는 작업 |
| :--- | :--- | :--- |
| Blackboard | 머티리얼 Inspector에 노출할 값을 만듭니다. | 색, 속도, 세기 같은 프로퍼티 추가 |
| Graph Area | 노드를 놓고 선으로 연결합니다. | `Space` 또는 우클릭으로 노드 검색 |
| Graph Inspector | 선택한 노드나 그래프 설정을 바꿉니다. | Surface Type, Blend Mode, Two Sided 설정 |
| Main Preview | 현재 셰이더 결과를 미리 봅니다. | 저장 전 색과 움직임 확인 |
| Master Stack | 최종 출력 위치입니다. | Base Color, Alpha, Emission 등에 연결 |

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

## 4. 색이 바뀌는 그래프 구성

초보자는 아래 순서대로 연결하면 됩니다.

| 단계 | 노드 | 연결 |
| :--- | :--- | :--- |
| 1 | `Time` | `Time` 출력 사용 |
| 2 | `Multiply` | `Time`과 `PulseSpeed`를 곱함 |
| 3 | `Sine` | 곱한 값을 부드러운 반복 값으로 바꿈 |
| 4 | `Remap` 또는 `Add/Multiply` | `-1~1` 값을 색에 쓰기 좋은 범위로 조절 |
| 5 | `Lerp` | 어두운 색과 밝은 색을 섞음 |
| 6 | Master Stack | `Base Color` 또는 `Emission`에 연결 |

값이 너무 빨리 바뀌면 `PulseSpeed`를 낮춥니다. 색 변화가 너무 약하면 `Lerp`에 들어가는 두 색의 차이를 크게 잡습니다.

## 5. 머티리얼에 적용하기

1. Shader Graph를 저장합니다.
2. Project 창에서 `Create > Material`을 선택합니다.
3. 머티리얼의 Shader를 방금 만든 `SG_ColorPulse`로 변경합니다.
4. Sphere나 Cube의 Mesh Renderer에 머티리얼을 넣습니다.
5. Play 모드 또는 Scene 뷰에서 색 변화가 보이는지 확인합니다.

## 노드 읽는 순서

Shader Graph도 코드처럼 `위->아래`, `오->왼`, `안->밖`으로 읽을 수 있습니다.

- 숫자와 색 프로퍼티가 입력입니다.
- Time과 Sine이 시간에 따른 변화를 만듭니다.
- Multiply가 변화량을 조절합니다.
- Master Stack의 Base Color가 최종 표면 색을 받습니다.

## 자주 막히는 지점

| 증상 | 확인할 것 |
| :--- | :--- |
| 머티리얼 Inspector에 값이 보이지 않음 | Blackboard 프로퍼티로 만들었는지, 그래프를 저장했는지 확인 |
| 오브젝트가 분홍색으로 보임 | URP용 Shader Graph인지, 그래프 저장 중 오류가 없는지 확인 |
| 투명도가 적용되지 않음 | Graph Inspector에서 Surface Type을 Transparent로 바꿨는지 확인 |
| 색이 움직이지 않음 | `Time` 노드가 연결되어 있는지, Play 모드 또는 Preview에서 보고 있는지 확인 |
| 너무 밝게 번짐 | Emission에 연결한 값이 과하지 않은지 확인 |

## 스크린샷 체크포인트

- `Images/day04_shader_graph_basic.png`: `SG_ColorPulse` 그래프 전체 화면
- `Images/day04_shader_graph_material.png`: 머티리얼 Inspector에서 프로퍼티를 조절하는 화면
- `Images/day04_shader_graph_blackboard.png`: Blackboard에 `BaseColor`, `PulseSpeed`가 추가된 화면
- `Images/day04_shader_graph_inspector.png`: Graph Inspector에서 Surface 설정을 확인하는 화면

## 오늘의 정리

- Shader Graph는 셰이더 알고리즘을 노드로 조립하는 도구입니다.
- Blackboard 프로퍼티를 만들면 머티리얼마다 값을 다르게 줄 수 있습니다.
- 다음 시간에는 Shader Graph로 용암, 물, 보호막 같은 표면 표현을 만듭니다.
