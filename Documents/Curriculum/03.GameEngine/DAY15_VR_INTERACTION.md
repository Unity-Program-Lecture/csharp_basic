# DAY 15: VR 상호작용과 물리 레이어

오늘의 목표는 VR 그랩 상호작용을 "**손으로 물건을 집어 드는 규칙**"으로 이해하고, 잡을 수 있는 오브젝트와 충돌 레이어를 구분하는 이유를 배우는 것입니다.

## 1. 핵심 개념: "잡을 수 있는 물건에는 손잡이가 필요하다"

VR에서 물건을 잡으려면 컨트롤러, 잡을 수 있는 오브젝트, 충돌 범위, 입력 액션이 서로 맞아야 합니다. 모든 물체가 모든 물체와 충돌하면 손이나 몸이 불필요하게 튕길 수 있으므로 레이어를 나누어 충돌 대상을 정리합니다.

이 수업은 13일차에서 설치한 XR 패키지가 준비되어 있다는 전제로 진행합니다. `XR Interaction Toolkit`이 없으면 `XR Grab Interactable` 같은 컴포넌트를 붙일 수 없고, `Input System`이나 XR Interaction Toolkit 샘플 입력 액션이 없으면 컨트롤러 버튼 입력을 연결하기 어렵습니다.

### 이 단어는 무슨 뜻인가요?

- **XR Interaction Toolkit**: VR/AR 상호작용을 만들기 위한 Unity 패키지입니다.
- **Input Action**: 버튼, 트리거, 조이스틱 입력을 기능 단위로 묶은 입력 설정입니다.
- **Interactor**: 잡거나 가리키는 손 역할의 오브젝트입니다.
- **Interactable**: 잡히거나 선택될 수 있는 대상 오브젝트입니다.
- **XR Grab Interactable**: XR Interaction Toolkit에서 물건을 잡을 수 있게 하는 컴포넌트입니다.
- **Layer Collision Matrix**: 어떤 레이어끼리 충돌할지 정하는 물리 설정 표입니다.
- **Rigidbody**: 잡힌 물체가 물리적으로 움직이도록 만드는 컴포넌트입니다.

## 2. 그랩 실습 전에 다시 확인할 패키지

14일차 실습은 패키지 설치보다 상호작용 구성에 집중합니다. 그래도 실습 전에 아래 항목을 한 번 더 확인해야 합니다.

| 확인 항목 | 필요한 이유 |
| :--- | :--- |
| `XR Interaction Toolkit` 설치 | `XR Grab Interactable`, XR Controller, Interactor 계열 컴포넌트를 사용하기 위해 필요합니다. |
| `Input System` 설치 | VR 컨트롤러의 트리거, 그립, 이동 입력을 액션으로 받기 위해 필요합니다. |
| `OpenXR Plugin` 활성화 | 실제 XR 런타임과 기기 입력을 연결하기 위해 필요합니다. |
| XR Interaction Toolkit `Starter Assets` Import | 기본 입력 액션과 프리셋을 빠르게 연결하기 위해 필요합니다. |

Package Manager에서 패키지를 설치한 뒤에도 샘플을 Import하지 않으면 입력 액션 에셋이 없어서 컨트롤러 버튼 연결이 비어 있을 수 있습니다. 이럴 때는 코드 문제가 아니라 프로젝트 준비 단계가 빠진 것입니다.

## 실습 예제: 잡을 수 있는 큐브 준비하기

**미션:** VR에서 잡을 대상 큐브에 필요한 기본 컴포넌트를 붙이고, 물리 레이어 구성을 확인합니다.

1. Package Manager에서 `XR Interaction Toolkit`, `Input System`, `OpenXR Plugin`이 준비되어 있는지 확인합니다.
2. XR Interaction Toolkit의 `Starter Assets` 또는 수업용 입력 액션 샘플이 Import되어 있는지 확인합니다.
3. 큐브를 만들고 `GrabCube`로 이름을 바꿉니다.
4. `Rigidbody`와 `Collider`가 있는지 확인합니다.
5. `XR Grab Interactable`을 붙입니다.
6. 잡을 물체를 `Grabbable` 같은 별도 레이어로 분류합니다.
7. 손 컨트롤러와 잡을 물체가 충돌해야 하는지, 손이 플레이어 몸과 충돌해야 하는지 Layer Collision Matrix에서 비교합니다.

### 실행해보면

XR Interaction Toolkit과 입력 액션 구성이 되어 있다면 Play를 눌렀을 때 컨트롤러가 물체를 선택하고 잡을 수 있습니다. 레이어 충돌 설정을 바꾸면 손, 물체, 환경 사이의 물리 반응이 어떻게 달라지는지 비교할 수 있습니다. `XR Grab Interactable` 컴포넌트가 보이지 않으면 패키지 설치 상태를 먼저 확인합니다.

### 생각해보기

1. 손 컨트롤러와 잡을 물체의 레이어를 구분하면 어떤 문제가 줄어들까요?
2. 너무 무거운 Rigidbody를 잡을 때 플레이어가 기대하는 느낌은 어떻게 달라질까요?
3. 패키지는 설치되어 있는데 컨트롤러 입력이 비어 있다면 어떤 준비 단계를 다시 확인해야 할까요?

## 오늘의 정리

- VR 그랩 실습에는 XR Interaction Toolkit, Input System, OpenXR Plugin 준비가 필요합니다.
- VR 상호작용은 Interactor와 Interactable의 관계로 이해할 수 있습니다.
- 잡을 수 있는 물체에는 Collider와 Rigidbody가 필요합니다.
- `XR Grab Interactable`은 XR Interaction Toolkit 패키지에서 제공되는 컴포넌트입니다.
- 레이어 충돌 설정은 원하지 않는 물리 반응을 줄이는 데 중요합니다.
