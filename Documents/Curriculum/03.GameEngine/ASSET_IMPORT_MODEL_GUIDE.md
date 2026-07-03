# 보충: 모델링 에셋 임포트 가이드

이 문서의 목표는 3D 모델 파일을 "**게임 월드에 세울 조립 부품**"으로 이해하고, 파일 포맷의 차이, 사용 용도, Unity Model Import Settings에서 자주 조정하는 옵션을 정리하는 것입니다.

3D 모델은 단순한 모양 파일이 아닙니다. Mesh, Material, Texture, Rig, Animation, Scale, Collider, Prefab 구성이 함께 얽혀 있습니다. 임포트 설정을 잘못 잡으면 모델이 너무 크거나 작고, 바닥에 묻히고, 애니메이션이 꼬이고, 머티리얼이 분홍색으로 보일 수 있습니다.

## 1. 핵심 개념: "모델 파일은 조립 전 부품 상자다"

모델 파일을 Unity에 넣으면 바로 게임용 오브젝트가 완성되는 것처럼 보일 수 있습니다. 하지만 실무에서는 모델 파일 원본과 실제 게임에 배치할 Prefab을 구분합니다.

```text
원본 모델 파일(.fbx, .obj, .blend)
  -> Unity Import Settings 확인
  -> Material, Texture, Rig, Animation 정리
  -> 게임에서 사용할 Prefab 생성
```

원본 모델 파일은 재료이고, Prefab은 그 재료를 게임에 맞게 조립한 결과물입니다.

### 이 단어는 무슨 뜻인가요?

- **Mesh**: 3D 모델의 실제 형태를 이루는 점, 선, 면 데이터입니다.
- **Material**: 모델 표면이 어떤 색과 질감으로 보일지 정하는 설정입니다.
- **Texture**: Material에 연결되는 이미지 파일입니다.
- **Rig**: 캐릭터처럼 움직이는 모델의 뼈대 구조입니다.
- **Animation Clip**: 모델이나 뼈대가 시간에 따라 움직이는 데이터입니다.
- **Avatar**: 사람형 뼈대를 Unity Humanoid 애니메이션 시스템에 연결한 정보입니다.
- **Scale Factor**: 외부 모델 크기를 Unity 단위에 맞게 조정하는 값입니다.

## 2. 자주 쓰는 모델 파일 포맷

| 포맷 | 특징 | 주 사용 용도 | 주의할 점 |
| :--- | :--- | :--- | :--- |
| `FBX` | 게임 엔진 교환용으로 가장 흔함 | 캐릭터, 배경 오브젝트, 애니메이션 | Export 설정과 단위 확인 필요 |
| `OBJ` | 단순 Mesh 교환에 적합 | 정적 소품, 간단한 배경 모델 | Rig와 Animation에는 부적합 |
| `Blend` | Blender 작업 파일 | 수업 또는 개인 작업 원본 | 팀/빌드 환경에서 Blender 의존성 주의 |
| `GLB` 또는 `glTF` | 비교적 현대적인 교환 포맷 | 웹/실시간 3D 자료, 일부 외부 에셋 | 프로젝트 파이프라인과 호환성 확인 |

Unity 수업과 일반 게임 제작에서는 `FBX`가 가장 무난합니다. 캐릭터, 애니메이션, 소품, 배경을 모두 다루기 쉽고, 다른 도구와 주고받는 사례도 많습니다.

## 3. 모델 용도별 확인 방향

| 용도 | 예시 | 확인할 것 |
| :--- | :--- | :--- |
| 정적 소품 | 상자, 나무, 바위 | Scale, Material, Collider |
| 배경 구조물 | 벽, 바닥, 건물 | 크기, 피벗, 충돌, 라이트맵 관련 설정 |
| 캐릭터 | 플레이어, 몬스터 | Rig, Avatar, Animation, 충돌 캡슐과 모델 높이 |
| 무기/장비 | 검, 총, 방패 | 피벗 위치, 손 위치 연결, 크기 |
| 애니메이션 포함 모델 | 걷기, 공격, 대기 | Rig 탭, Animations 탭, Loop 설정 |

## 4. Unity Model Import Settings의 주요 탭

모델 파일을 Project 창에서 선택하면 Inspector에 여러 탭이 표시됩니다. 처음에는 `Model`, `Rig`, `Animation`, `Materials` 탭을 중심으로 봅니다.

| 탭 | 역할 | 자주 확인할 것 |
| :--- | :--- | :--- |
| `Model` | Mesh, Scale, 단위, 법선 등 기본 모델 설정 | Scale Factor, Normals, Tangents, Optimize Mesh |
| `Rig` | 뼈대와 애니메이션 타입 설정 | Animation Type, Avatar Definition |
| `Animation` | 포함된 애니메이션 클립 설정 | Clip 범위, Loop Time, Root Transform |
| `Materials` | 모델이 사용할 머티리얼 처리 | Material Creation, 추출 여부, 텍스처 연결 |

설정을 바꾼 뒤에는 `Apply`를 눌러야 변경이 적용됩니다.

## 5. Model 탭에서 자주 보는 옵션

| 옵션 | 의미 | 실무에서 보는 기준 |
| :--- | :--- | :--- |
| `Scale Factor` | 외부 모델 크기를 Unity 단위로 변환 | 캐릭터 키와 씬 기준 크기에 맞는지 확인 |
| `Convert Units` | 모델링 도구 단위를 Unity 단위로 변환 | 도구별 단위 차이 때문에 확인 필요 |
| `Import BlendShapes` | 표정/형태 변형 데이터 임포트 | 표정이나 변형이 필요할 때만 |
| `Import Cameras` | 모델 파일 안의 카메라 임포트 | 게임용 모델에서는 보통 불필요 |
| `Import Lights` | 모델 파일 안의 조명 임포트 | 씬 조명과 충돌할 수 있어 주의 |
| `Normals` | 표면 방향 정보 처리 | 그림자가 이상하면 확인 |
| `Tangents` | Normal Map에 필요한 접선 정보 | Normal Map을 쓸 때 확인 |

모델이 너무 크거나 작으면 코드보다 Import Settings의 Scale부터 확인합니다. 특히 캐릭터는 `CharacterController`나 Collider 높이와 모델 키가 맞아야 이동과 충돌이 자연스럽습니다.

## 6. Rig 탭에서 자주 보는 옵션

| 옵션 | 의미 | 사용 기준 |
| :--- | :--- | :--- |
| `Animation Type` | 애니메이션 뼈대 종류 | 사람형은 Humanoid, 그 외는 Generic, 애니메이션 없으면 None |
| `Avatar Definition` | Avatar를 새로 만들지 기존 것을 쓸지 | 같은 캐릭터 계열은 기존 Avatar 재사용 가능 |
| `Configure` | Humanoid 뼈대 연결 확인 | 팔, 다리, 머리 매핑 오류 확인 |

사람형 캐릭터라고 무조건 잘 움직이는 것은 아닙니다. 팔, 다리, 머리 뼈대가 Unity의 Humanoid 구조에 맞게 연결되어야 합니다. 애니메이션이 이상하게 꺾이면 `Rig` 탭의 Avatar 설정부터 확인합니다.

## 7. Animation 탭에서 자주 보는 옵션

| 옵션 | 의미 | 실무에서 보는 기준 |
| :--- | :--- | :--- |
| `Import Animation` | 애니메이션을 가져올지 정함 | 정적 소품이면 끌 수 있음 |
| `Clips` | 한 파일 안의 애니메이션 구간 | Idle, Walk, Attack 구간을 나눔 |
| `Loop Time` | 반복 재생 여부 | Idle, Walk, Run은 보통 On |
| `Root Transform` | 이동 정보를 루트에 어떻게 적용할지 | 코드 이동과 Root Motion 충돌 주의 |
| `Events` | 특정 프레임에 이벤트 호출 | 공격 판정, 발소리 타이밍 등에 사용 |

DAY 11에서 배운 것처럼 코드 이동과 Root Motion을 함께 쓰면 캐릭터가 예상보다 많이 움직일 수 있습니다. 수업에서는 먼저 코드가 이동을 담당하고 애니메이션은 시각 표현을 담당하도록 나누는 편이 이해하기 쉽습니다.

## 8. Materials 탭에서 자주 보는 옵션

모델 파일 안에 머티리얼 이름이 들어 있어도 Unity 프로젝트의 머티리얼과 자동으로 완벽히 맞는 것은 아닙니다.

| 상황 | 확인할 것 |
| :--- | :--- |
| 모델이 분홍색으로 보임 | 셰이더가 현재 렌더 파이프라인과 맞는지 확인 |
| 텍스처가 빠짐 | Texture 파일 경로와 Material 연결 확인 |
| 머티리얼이 모델 파일 안에 묶여 있음 | Extract Materials로 프로젝트 폴더에 분리 고려 |
| 여러 모델이 같은 재질을 써야 함 | 공용 Material을 만들어 연결 |

실무에서는 원본 모델이 자동으로 만든 머티리얼을 그대로 쓰기보다, 프로젝트의 `Materials` 폴더에 정리된 머티리얼을 만들어 연결하는 경우가 많습니다.

## 9. 모델에서 Prefab으로 만드는 흐름

모델 파일을 바로 씬에 계속 배치하면 나중에 수정하기 어렵습니다. 보통 다음 순서로 정리합니다.

```text
Assets/_Project/Models/Slime.fbx
Assets/_Project/Materials/SlimeBody.mat
Assets/_Project/Prefabs/Monsters/Slime.prefab
```

Prefab에는 모델뿐 아니라 실제 게임에 필요한 컴포넌트를 함께 붙입니다.

```text
Slime.prefab
  - Visual 자식: Slime 모델
  - Animator
  - Collider 또는 CharacterController
  - MonsterController 스크립트
  - AudioSource
  - 필요한 ScriptableObject 데이터 참조
```

이렇게 나누면 모델 파일이 바뀌어도 게임 오브젝트 구조를 Prefab에서 관리할 수 있습니다.

## 10. 실무에서 자주 생기는 문제

### 1. 모델 크기가 이상함

Blender, Maya, 3ds Max 등 도구마다 단위와 Export 설정이 다를 수 있습니다. Unity에서는 보통 1 unit을 1 meter처럼 생각하고 씬을 구성합니다. 캐릭터 키가 0.02이거나 100이면 `Scale Factor`와 Export 단위를 확인합니다.

### 2. 피벗 위치가 불편함

무기 피벗이 손잡이가 아니라 모델 중앙에 있으면 손에 붙일 때 회전이 어렵습니다. 문, 무기, 회전 장치처럼 기준점이 중요한 모델은 모델링 도구에서 피벗을 정리하거나 Unity에서 빈 부모 오브젝트를 만들어 보정합니다.

### 3. 머티리얼이 분홍색으로 보임

분홍색은 대체로 셰이더가 깨졌거나 현재 렌더 파이프라인과 맞지 않는다는 신호입니다. URP 프로젝트라면 Built-in용 머티리얼을 URP용으로 변환하거나, 새 URP/Lit 머티리얼을 만들어 텍스처를 다시 연결합니다.

### 4. 애니메이션이 이상하게 꺾임

Rig 타입이 잘못되었거나 Avatar 매핑이 틀렸을 수 있습니다. 사람형 캐릭터는 `Humanoid`, 일반 몬스터나 기계는 `Generic`이 더 자연스러울 수 있습니다.

### 5. 충돌이 모델과 맞지 않음

보이는 모델과 Collider는 별개입니다. 캐릭터 모델이 바뀌면 `CharacterController`의 `Height`, `Radius`, `Center`도 다시 확인해야 합니다.

### 6. 원본 모델을 직접 수정해서 참조가 꼬임

외부 에셋 원본을 마음대로 이동하거나 이름을 바꾸면 Prefab, Material, Animation 참조가 깨질 수 있습니다. 외부 원본은 유지하고, 게임에서 쓰는 Prefab과 Material을 `_Project` 아래에 따로 정리하는 습관이 좋습니다.

## 11. 추천 폴더 구조

```text
Assets/
  _Project/
    Models/
      Characters/
      Props/
      Environment/
    Materials/
      Characters/
      Props/
      Environment/
    Textures/
      Characters/
      Props/
      Environment/
    Animations/
      Characters/
    Prefabs/
      Characters/
      Props/
```

모델, 텍스처, 머티리얼, Prefab을 같은 폴더에 모두 섞으면 나중에 무엇이 원본이고 무엇이 게임용 결과물인지 헷갈립니다. 폴더 역할을 나누면 참조 문제를 추적하기 쉬워집니다.

## 12. 임포트 체크리스트

| 질문 | 확인할 것 |
| :--- | :--- |
| 모델 크기가 씬 기준과 맞나요? | Scale Factor, Convert Units, Export 단위 |
| 캐릭터인가요? | Rig 타입, Avatar, Animation Clip |
| 정적 소품인가요? | Import Animation이 불필요한지 확인 |
| 머티리얼이 정상인가요? | Shader, Texture 연결, URP 호환성 |
| 충돌이 필요한가요? | Collider를 Prefab에서 따로 구성 |
| 피벗이 중요한 모델인가요? | 손잡이, 문축, 회전 중심 확인 |
| 외부 에셋 원본인가요? | 원본 경로 유지, `_Project`에 Prefab/Material 분리 |

## 생각해보기

1. 모델 파일 원본을 바로 씬에 많이 배치하는 것보다 Prefab으로 정리하는 편이 좋은 이유는 무엇일까요?
2. 사람형 캐릭터와 바위 소품은 Rig 설정이 어떻게 달라질까요?
3. 캐릭터 모델을 바꾼 뒤 Collider를 다시 확인해야 하는 이유는 무엇일까요?

## 오늘의 정리

- 모델 파일은 게임 오브젝트 완성품이 아니라 Mesh, Material, Rig, Animation을 담은 원본 재료입니다.
- Unity에서는 `Model`, `Rig`, `Animation`, `Materials` 탭을 중심으로 임포트 설정을 확인합니다.
- 캐릭터는 Rig와 Animation, 정적 소품은 Scale과 Material, 배경 모델은 크기와 충돌 구성이 중요합니다.
- 모델 원본과 실제 게임용 Prefab을 구분하면 참조 꼬임과 수정 부담을 줄일 수 있습니다.
- 외부 모델 에셋은 원본을 보존하고, 프로젝트에서 사용할 Prefab과 Material을 `_Project` 아래에 정리하는 습관이 좋습니다.
