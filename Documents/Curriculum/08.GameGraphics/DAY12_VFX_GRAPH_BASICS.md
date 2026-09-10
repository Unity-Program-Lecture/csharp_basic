# DAY 12: Visual Effect Graph 입문

오늘의 목표는 Visual Effect Graph를 "**GPU에서 대량 파티클을 계산하는 이펙트 제작판**"으로 이해하고, 간단한 GPU 파티클 효과를 만드는 것입니다.

> 이 문서의 실습 절차는 Unity 6의 Visual Effect Graph `17.0.x`와 `Simple Particle System` Template을 기준으로 합니다. Template을 다르게 선택하면 처음 보이는 Context와 Block이 달라집니다.

## NCS 연결

- 능력단위 요소: 이펙트 프로그래밍하기
- 관련 학습 내용: 게임 이펙트 구성 방법 이해 및 사용
- Unity 6 재구성: Visual Effect Graph로 많은 입자를 사용하는 이펙트를 제작합니다.

## 1. Visual Effect Graph는 언제 쓰나요?

Unity 6 공식 문서에 따르면 Visual Effect Graph는 대규모 비주얼 이펙트를 만들기 위한 패키지이며, GPU에서 파티클 동작을 시뮬레이션해 Built-in Particle System보다 더 많은 파티클을 다룰 수 있습니다. 많은 입자와 세밀한 커스터마이즈가 필요할 때 VFX Graph를 사용합니다.

### Particle System과 비교

| 구분 | Particle System | Visual Effect Graph |
| :--- | :--- | :--- |
| 계산 중심 | CPU/엔진 컴포넌트 기반 | GPU 시뮬레이션 중심 |
| 장점 | 배우기 쉽고 작은 이펙트에 적합 | 대량 파티클과 복잡한 효과에 강함 |
| 예시 | 히트, 회복, 작은 폭발 | 마법 폭풍, 에너지장, 대량 먼지 |

## 2. 실습: GPU Spark 이펙트

1. Package Manager에서 Visual Effect Graph가 사용 가능한지 확인합니다.
2. `Create > Visual Effects > Visual Effect Graph`에서 `Simple Particle System` Template을 선택합니다.
3. `VFX_GpuSpark` 에셋을 만듭니다.
4. 에셋을 Scene 뷰에 끌어 놓아 Visual Effect 오브젝트를 만듭니다.
5. Spawn Rate, Velocity, Color, Lifetime을 조절합니다.

### 패키지와 Graph 편집기 준비

> 이 실습은 Unity 6 **URP** 프로젝트와 Compute Shader를 지원하는 PC가 필요합니다. Built-in Render Pipeline에서는 VFX Graph를 사용할 수 없습니다. 시작 전에 [환경 설정 안내](ENVIRONMENT_SETUP_GUIDE.md)의 DAY 12 점검 항목과 URP·VFX Graph 패키지 버전 계열을 확인합니다.

1. `Window > Package Manager`를 열고 Packages 목록을 `Unity Registry` 또는 `All`로 바꿉니다.
2. `Visual Effect Graph`를 선택하고 Install을 누릅니다. 설치가 끝난 뒤 Console Error가 없는지 확인하고, Unity가 재시작을 요청하면 재시작합니다.
3. Project 창에서 `Assets/GameGraphics` 아래에 `VFX` 폴더가 없다면 먼저 만듭니다.
4. `Assets/GameGraphics/VFX` 폴더에서 `Create > Visual Effects > Visual Effect Graph`를 선택합니다.
5. 열리는 Template 창에서 `Simple Particle System`을 선택하고, 이름을 `VFX_GpuSpark`로 입력한 뒤 Create를 누릅니다. Template을 선택하지 않으면 이후 Graph 모양과 실습 안내가 맞지 않습니다.
6. `VFX_GpuSpark`를 더블 클릭해 VFX Graph 창을 엽니다. 이 Template이 만든 Context와 Block을 실습의 출발점으로 사용합니다.
7. Block이 필요한 Context 안에 없으면 **해당 Context 내부**를 우클릭해 `Create Block`을 선택하거나, Context 위에 마우스를 올리고 Space를 누른 뒤 검색해 추가합니다.
8. Graph를 저장합니다. Scene 배치는 6단계에서 Graph Asset을 Scene 뷰로 끌어 놓는 방법만 사용합니다.

## 3. Visual Effect Graph 창 사용법

VFX Graph는 Shader Graph와 비슷하게 노드를 연결하지만, 더 큰 단위인 Context 흐름을 먼저 읽어야 합니다.

| 영역 | 하는 일 | 주로 하는 작업 |
| :--- | :--- | :--- |
| Blackboard | 외부에서 조절할 프로퍼티를 만듭니다. | Spawn Rate, Color, Size 노출 |
| Graph Area | Context와 Operator를 배치합니다. | 블록 추가, 값 연결 |
| Context | Spawn, Initialize, Update, Output 같은 큰 처리 단계입니다. | 파티클의 생명 주기 구성 |
| Block | Context 안에 들어가는 세부 명령입니다. | Set Velocity, Set Color, Set Size |
| Operator | 값을 계산하는 노드입니다. | Random Number, Vector, Multiply |
| Visual Effect 컴포넌트 | 씬에서 VFX Graph 에셋을 재생합니다. | 에셋 연결, 프로퍼티 값 조절 |

VFX Graph는 "**입자 공장 라인**"처럼 보면 좋습니다. Spawn에서 입자를 만들고, Initialize에서 초기 상태를 붙이고, Update에서 살아 있는 동안 움직이고, Output에서 화면에 보여 줍니다.

## 4. VFX Graph 기본 블록

| 영역 | 역할 |
| :--- | :--- |
| Spawn | 입자가 언제 얼마나 생길지 정합니다. |
| Initialize | 처음 위치, 속도, 수명, 크기를 정합니다. |
| Update | 살아 있는 동안 움직임과 변화를 계산합니다. |
| Output | 최종적으로 화면에 어떻게 보일지 정합니다. |

### Context 읽는 순서

```text
Spawn -> Initialize Particle -> Update Particle -> Output Particle
```

- **Spawn**: "몇 개를 태어나게 할까?"
- **Initialize**: "태어날 때 위치, 속도, 크기, 수명은?"
- **Update**: "살아 있는 동안 중력, 회전, 색 변화는?"
- **Output**: "최종적으로 점, 사각형, 메시 중 무엇으로 보일까?"

## 5. GPU Spark 만들기 상세 절차

### 1단계: Spawn 설정

1. 맨 위의 `Spawn` Context를 찾습니다. 이 Context는 입자가 몇 개 생길지를 정하는 큰 상자입니다.
2. Context 안에서 `Rate` 입력 칸이 있는 Block을 찾습니다. Unity 6 VFX Graph 17에서는 Block 검색에 `Constant Rate`를 입력해 추가할 수 있습니다. 일부 화면에서는 `Constant Spawn Rate`로 표시될 수 있습니다.
3. `Rate` 오른쪽 숫자 칸을 클릭하고 `80`을 입력한 뒤 Enter를 누릅니다.
4. `Rate` Block이 없다면 Spawn Context의 검은 빈 공간을 우클릭해 `Create Block`을 선택하고 `Constant Rate`를 검색해 추가합니다. Graph 전체의 빈 공간이 아니라 **Spawn Context 내부**를 클릭해야 합니다.
5. 숫자 `80`을 기준 결과로 둡니다. 결과 확인 뒤 너무 적으면 `20`, 더 화려하게 비교하려면 `200`으로 한 번에 하나씩 바꿔 봅니다.

### 실습 결과를 고정하는 기본값

아래 값은 `Simple Particle System` Template에서 만들 최종 상태입니다. Block이 이미 있으면 값을 바꾸고, 없으면 **해당 Context 안에서만** `Create Block`으로 추가합니다. 처음 Graph에 무엇이 들어 있는지를 외워서 찾지 말고, 표의 Context·검색어·값 순서로 확인합니다.

| 위치 | 설정 | 값 |
| :--- | :--- | :--- |
| Initialize Particle Context Inspector | Capacity | `128` |
| Spawn | `Constant Rate` 검색 후 Rate | `80` |
| Initialize | Set Lifetime Random (Uniform) | `0.4 ~ 1.2` |
| Initialize | Set Position (Shape : Sphere) > Radius | `0.1` |
| Initialize | Set Velocity Random (Per Component) > A | `(-1, 2, -1)` |
| Initialize | Set Velocity Random (Per Component) > B | `(1, 4, 1)` |
| Initialize | Set Size Random | `0.03 ~ 0.12` |
| Update | Add Force > Force | `(0, -9.81, 0)` |
| Update | Drag > Drag | `1.5` |
| Update | Set Color over Life | 시작 `#FF8A00`, 끝 `#FF8A00` Alpha `0` |
| Output Particle Quad Inspector | Blend Mode | `Additive` |

`Capacity`는 동시에 살아 있을 수 있는 입자 수의 상한입니다. 이 예제는 최대 약 `80 x 1.2 = 96`개가 동시에 살아 있을 수 있으므로, 여유를 둔 `128`로 시작합니다.

### 2단계: Initialize 설정

Initialize Particle Context에서 다음 Block을 확인합니다. 이미 있다면 값을 바꾸고, 없다면 Initialize Particle Context 내부에서 Block 이름을 검색해 추가합니다.

| Block | 실제 설정 | 결과 |
| :--- | :--- | :--- |
| Set Lifetime Random (Uniform) | `0.4 ~ 1.2` | 입자마다 사라지는 시간이 달라집니다. |
| Set Position (Shape : Sphere) | Radius `0.1` | 입자가 작은 구 영역에서 시작합니다. |
| Set Velocity Random (Per Component) | A `(-1, 2, -1)`, B `(1, 4, 1)` | 위쪽으로 솟으면서 좌우·앞뒤로 퍼집니다. |
| Set Size Random | `0.03 ~ 0.12` | 입자 크기가 조금씩 달라집니다. |

### 3단계: Update 설정

Update Particle Context에서 다음 Block을 확인합니다. 이미 있다면 값을 바꾸고, 없다면 Update Particle Context 내부에서 Block 이름을 검색해 추가합니다.

| Block | 사용 이유 |
| :--- | :--- |
| Add Force | Force를 `(0, -9.81, 0)`으로 두면 위로 퍼진 입자가 아래로 떨어집니다. |
| Drag | Drag를 `1.5`로 두면 시간이 지나며 속도가 줄어듭니다. |
| Set Color over Life | 수명 비율을 이용해 시작은 주황색, 끝은 투명하게 만듭니다. |

`Age over Lifetime`은 여기서 추가하는 Block 이름이 아니라 입자의 수명 비율을 뜻합니다. 색이나 크기를 시간에 따라 바꾸려면 `Set Color over Life` 또는 `Set Size over Life` Block을 사용합니다.

### 4단계: Output 설정

Output Particle Quad Context에서 다음을 확인합니다. `Output Particle Quad`가 없다면 이 실습의 Template 선택부터 다시 확인합니다.

- Output 타입이 `Output Particle Quad`인지 확인합니다.
- Output Particle Quad Inspector의 Blend Mode를 `Additive`로 설정합니다.
- 색은 Output에서 막연히 정하지 말고 Update의 `Set Color over Life` Block에서 시작 `#FF8A00`, 끝 Alpha `0`으로 설정합니다.
- 처음에는 Texture 없이 Quad 결과를 확인합니다. 기본 결과가 보인 뒤에만 불꽃 텍스처를 넣어 비교합니다.

## 6. 씬에 배치하고 재생 확인하기

1. Project 창에서 `VFX_GpuSpark` 에셋을 Scene 뷰의 빈 곳으로 끌어 놓습니다. Unity가 카메라 앞에 Visual Effect GameObject를 만들고 에셋을 연결합니다.
2. Hierarchy에서 방금 생긴 GameObject를 선택하고 이름을 `VFX_GpuSpark_Player`로 바꿉니다.
3. Play 모드에서 다음 결과가 보이는지 확인합니다: **작은 주황색 입자가 중심에서 위쪽과 바깥쪽으로 퍼진 뒤, 아래로 떨어지며 투명해져 사라집니다.**
4. Game 뷰가 이 결과를 보이도록 스크린샷을 1장 남깁니다. 이것이 이 실습의 기본 완료 증거입니다.

보이지 않으면 먼저 씬 카메라가 이펙트 위치를 보고 있는지 확인합니다. 그 다음 Bounds, Spawn Rate, Output Color, Visual Effect 컴포넌트의 에셋 연결을 확인합니다.

## 7. 자주 막히는 지점

| 증상 | 확인할 것 |
| :--- | :--- |
| 아무것도 보이지 않음 | Visual Effect 컴포넌트에 Graph 에셋이 연결되어 있는지 확인 |
| Scene 뷰에는 보이는데 Game 뷰에는 안 보임 | 카메라 위치와 Clipping Plane 확인 |
| 입자가 너무 빨리 사라짐 | Lifetime 값을 늘림 |
| 입자가 한 점에만 뭉침 | Position Shape 또는 Velocity 설정 확인 |
| 입자 수가 Rate보다 적어 보임 | Initialize Particle Context의 Capacity를 `128` 이상으로 설정 |
| 편집 중 결과가 이상함 | 그래프 저장 후 Visual Effect 컴포넌트가 최신 에셋을 쓰는지 확인 |

## VFX Graph와 Visual Effect Inspector 확인 절차

`Simple Particle System` Template을 선택했다면 Graph Area는 `Spawn > Initialize Particle > Update Particle > Output Particle Quad` 순서로 읽습니다. Spawn에는 `Constant Rate`와 Rate `80`을 둡니다. Initialize에는 Set Lifetime Random (Uniform), Set Position (Shape : Sphere), Set Velocity Random (Per Component), Set Size Random을 확인합니다. Update에는 Add Force, Drag, Set Color over Life를 확인합니다. Output에는 Output Particle Quad와 Additive Blend Mode를 설정합니다. Context 사이 연결이 끊기면 해당 단계의 입자가 만들어지거나 보이지 않습니다.

VFX Graph Asset을 씬으로 끌어 놓아 만든 GameObject를 선택하면 `Visual Effect` 컴포넌트가 보입니다. Inspector의 Visual Effect Asset 필드에 만든 Graph가 연결돼 있는지, Pause·Play 상태가 의도한지, 노출한 프로퍼티가 있다면 값이 표시되는지 확인합니다. 에셋을 바꾼 뒤 결과가 갱신되지 않으면 Graph 저장, Asset 필드, Console Error 순서로 확인합니다.

VFX가 보이지 않으면 Spawn Rate가 `0`이 아닌지, Initialize Lifetime과 Size가 `0`이 아닌지, Output Particle의 색 Alpha가 `0`이 아닌지부터 확인합니다. 그 다음 Camera가 이펙트 위치를 보는지, Visual Effect의 Bounds가 이동 범위를 포함하는지, GPU Compute Shader를 지원하는 환경인지 확인합니다.

## 오늘의 정리

- Visual Effect Graph는 대량 파티클과 복잡한 이펙트에 적합합니다.
- VFX Graph는 Spawn, Initialize, Update, Output 흐름으로 읽습니다.
- Context는 큰 단계, Block은 단계 안의 세부 명령, Operator는 값을 계산하는 도구입니다.
- 다음 시간에는 VFX Graph의 노출 프로퍼티와 성능 조절을 다룹니다.
