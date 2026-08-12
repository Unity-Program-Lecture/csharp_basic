# DAY 09: Particle System 기초

오늘의 목표는 이펙트를 "**작은 조각을 많이 뿌려 하나의 현상처럼 보이게 하는 기술**"로 이해하는 것입니다.

## NCS 연결

- 능력단위 요소: 이펙트 프로그래밍하기
- 관련 학습 내용: 게임 이펙트 구성 방법 이해 및 사용
- Unity 6 재구성: Built-in Particle System으로 폭발, 먼지, 히트 효과를 만듭니다.

## 1. Particle은 무엇인가요?

불꽃, 연기, 먼지, 마법 가루처럼 작은 이미지나 메시가 많이 모여 하나의 현상처럼 보이는 효과를 파티클 이펙트라고 합니다.

### 이 단어는 무슨 뜻인가요?

- **Particle**: 화면에 뿌려지는 작은 입자입니다.
- **Emitter**: 파티클이 생성되는 위치와 모양입니다.
- **Lifetime**: 파티클 하나가 살아 있는 시간입니다.
- **Emission**: 파티클이 얼마나 자주 생성되는지 정하는 설정입니다.
- **Renderer**: 파티클이 어떤 이미지나 메시로 보일지 정합니다.

## 2. 실습: 히트 이펙트 만들기

1. 빈 GameObject를 만들고 Particle System을 추가합니다.
2. Duration을 짧게 설정하고 Looping을 끕니다.
3. Start Lifetime, Start Speed, Start Size를 조절합니다.
4. Color over Lifetime으로 처음은 밝고 끝은 투명하게 만듭니다.
5. Shape를 Sphere 또는 Cone으로 바꿔 퍼지는 방향을 조절합니다.

### Particle System을 만들고 모듈을 여는 순서

1. Hierarchy의 빈 곳에서 `GameObject > Effects > Particle System`을 선택하고 이름을 `FX_HitSpark_Test`로 바꿉니다.
2. 새 오브젝트를 선택한 Inspector의 Particle System 컴포넌트에서 Main 모듈을 펼칩니다. Duration, Looping, Start Lifetime, Start Speed, Start Size를 먼저 설정합니다.
3. 왼쪽 체크 상자를 켜서 Emission, Shape, Color over Lifetime, Size over Lifetime, Renderer 모듈을 필요한 만큼 활성화합니다. 체크 상자가 꺼진 모듈은 값이 보여도 실행에 적용되지 않습니다.
4. Emission의 `+` 버튼으로 Burst를 하나 추가하고, Rate over Time을 `0`으로 둡니다. Scene View의 Particle Effect 패널 재생 버튼과 Play Mode 결과를 모두 확인합니다.
5. 이펙트 위치는 대상 Mesh의 중심 또는 충돌 지점에 두고, Transform Scale이 아니라 Start Size로 먼저 크기를 맞춥니다.

## 3. 이펙트를 놓을 3D 공간 분석하기

이펙트는 혼자 예쁘게 보이면 끝이 아닙니다. 같은 폭발도 캐릭터 발밑, 무기 끝, 벽 표면, 아주 먼 배경에 놓일 때 필요한 크기와 방향이 다릅니다. 만들기 전에 "어디에서, 누구에게, 언제 보여 줄 것인가"를 정합니다.

| 분석 항목 | 히트 이펙트 예시 | Unity 6에서 확인할 것 |
| :--- | :--- | :--- |
| 발생 위치 | 공격이 맞은 대상의 중심 또는 충돌 지점 | Transform 또는 Raycast Hit Point |
| 방향 | 타격 방향 반대쪽으로 불꽃이 퍼진다. | Rotation, Shape Module 방향 |
| 크기 | 캐릭터 키의 약 1/4 안에서 보인다. | Start Size와 씬 오브젝트 Scale 비교 |
| 카메라 거리 | 플레이어가 알아볼 수 있지만 화면을 가리지 않는다. | Game View에서 카메라와 겹침 확인 |
| 지속 시간 | 맞았다는 사실만 전달하고 곧 사라진다. | Start Lifetime과 Stop Action |
| 게임 규칙 | 실제 피격이 확정된 한 번에만 재생한다. | 피격 이벤트와 중복 재생 여부 |

이 표는 교수계획서의 "3D 그래픽 요소와 공간 배치 분석"을 Unity 실습으로 바꾼 것입니다. 이펙트의 위치·크기·방향·시간을 게임 장면과 규칙에 맞게 정하면, 장식이 아니라 플레이를 읽게 해 주는 신호가 됩니다.

## 주요 모듈

| 모듈 | 역할 |
| :--- | :--- |
| Main | 기본 시간, 속도, 크기 |
| Emission | 생성 개수와 빈도 |
| Shape | 생성 위치와 방향 |
| Color over Lifetime | 시간에 따른 색 변화 |
| Size over Lifetime | 시간에 따른 크기 변화 |
| Renderer | 표시 방식과 머티리얼 |

## Particle System Inspector 읽는 순서

Particle System 오브젝트를 선택한 뒤 Inspector의 모듈을 위에서 아래로 읽습니다. Main 모듈에서는 Duration, Looping, Start Lifetime, Start Speed, Start Size, Start Color, Simulation Space를 먼저 정합니다. 한 번 터지는 히트 이펙트는 Looping을 끄고, Simulation Space는 이펙트가 발생한 자리에 남아야 하면 World, 이동하는 무기에 붙어야 하면 Local을 선택합니다.

Emission 모듈은 `Rate over Time`으로 계속 나오는 입자 수를, `Bursts`로 한 번에 나오는 입자 수를 정합니다. 히트 이펙트는 보통 Rate over Time을 `0`으로 두고 Bursts를 하나 넣어 의도하지 않은 계속 재생을 막습니다. Shape에서는 Cone·Sphere·Box 중 발생 형태를 정하고, Velocity over Lifetime과 Size over Lifetime은 입자가 태어난 뒤의 이동과 크기 변화를 담당합니다.

Color over Lifetime은 시작·중간·끝의 색과 Alpha를 정합니다. 끝 Alpha를 `0`으로 내려야 입자가 갑자기 사라지는 느낌을 줄일 수 있습니다. Renderer 모듈에서는 Render Mode, Material, Sorting Fudge를 확인합니다. Material이 없거나 URP와 맞지 않으면 분홍색·검은 사각형이 보일 수 있으며, 연출이 재생되지 않으면 GameObject 활성 상태와 Main 모듈의 Play On Awake도 함께 확인합니다.

## 오늘의 정리

- 파티클은 작은 입자 여러 개로 큰 현상을 표현합니다.
- Particle System은 Inspector 설정만으로도 많은 이펙트를 만들 수 있습니다.
- 이펙트는 발생 위치, 방향, 크기, 지속 시간, 게임 규칙을 함께 정해야 장면에 자연스럽게 들어갑니다.
- 다음 시간에는 파티클 이펙트를 프리팹으로 만들고 재사용합니다.
