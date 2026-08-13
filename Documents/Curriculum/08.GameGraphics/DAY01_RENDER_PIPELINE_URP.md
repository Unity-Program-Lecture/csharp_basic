# DAY 01: Unity 6 렌더링 파이프라인과 URP 기초

오늘의 목표는 Unity가 게임 월드를 화면에 그리는 과정을 "**무대 뒤 조명실에서 장면을 순서대로 켜는 일**"로 이해하는 것입니다.

## NCS 연결

- 능력단위: `0803020531_18v4 게임 그래픽 프로그래밍`
- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 렌더링, 렌더링 파이프라인, 게임 엔진 그래픽 출력
- Unity 6 재구성: URP 프로젝트에서 카메라, 라이트, 머티리얼, 렌더러 설정을 확인합니다.

## 1. 핵심 개념: "화면은 한 번에 그려지지 않는다"

Unity 화면은 카메라가 보는 오브젝트를 모아 GPU가 처리한 결과입니다. 오브젝트는 메시, 머티리얼, 라이트, 카메라, 렌더 파이프라인 설정을 거쳐 화면에 나타납니다.

### 이 단어는 무슨 뜻인가요?

- **렌더링**: 3D 공간의 데이터를 2D 화면 이미지로 바꾸는 과정입니다.
- **렌더 파이프라인**: 어떤 순서와 규칙으로 화면을 그릴지 정한 작업 흐름입니다.
- **SRP**: Scriptable Render Pipeline의 줄임말입니다. Unity가 제공하는 C# 기반 렌더링 파이프라인 구조로, 프로젝트가 어떤 렌더링 규칙과 렌더러를 사용할지 Asset으로 정할 수 있게 합니다.
- **URP**: Universal Render Pipeline의 줄임말입니다. Unity 6 수업에서 기본으로 사용할 경량 범용 렌더 파이프라인입니다.
- **GPU**: 그래픽 계산을 주로 담당하는 장치입니다.
- **Draw Call**: CPU가 GPU에게 "이 오브젝트를 그려 줘"라고 요청하는 단위입니다.

## 2. SRP와 URP의 관계

SRP는 "렌더링 공정을 Asset과 C# 코드로 구성할 수 있는 공통 구조"이고, URP는 그 구조를 사용해 Unity가 제공하는 범용 렌더링 파이프라인입니다. 이 과정에서는 렌더러 C# 코드를 새로 만드는 대신, **URP가 제공하는 Asset과 Inspector 설정으로 SRP의 렌더링 규칙을 선택하고 조절**합니다.

SRP는 Vertex·Fragment (Pixel) 셰이더 사이에 끼어드는 단계가 아닙니다. 카메라가 한 프레임을 그릴 때 **무엇을, 어떤 순서와 조건으로 GPU에 그릴지 정하는 상위 Render Loop**입니다. Shader는 SRP가 제출한 Draw Call 안에서 각 메시의 정점 위치와 픽셀 색을 계산합니다.

### 설정 구조

```text
Project Settings > Graphics / Quality
└── URP Asset                         ← 프로젝트의 렌더링 규칙
    └── Renderer List
        └── Universal Renderer Data    ← 실제 그리기 순서와 Renderer Feature
            └── Renderer Features      ← 필요한 경우 추가하는 렌더 단계

Main Camera
└── Universal Additional Camera Data
    └── Renderer (Default 또는 선택한 Renderer)

Mesh Renderer + Material + URP Shader
```

### 한 프레임이 그려지는 흐름

```text
Camera가 보는 씬 데이터
        ↓
URP의 SRP Render Loop
  1. Culling: Camera에 보이는 Mesh·Light를 추립니다.
  2. Render Pass 구성: 그림자, 불투명, 투명, 후처리의 순서를 준비합니다.
  3. Draw Call 제출: 각 Material과 Shader에 그리기 명령을 보냅니다.
        ↓
URP Shader 실행
  4. Vertex 단계: 메시 정점의 위치를 계산합니다.
  5. Fragment/Pixel 단계: 화면 픽셀의 색을 계산합니다.
        ↓
Camera의 최종 화면 출력
```

따라서 `URP Asset`과 `Universal Renderer Data`는 1\~3단계의 규칙과 패스를 정하고, Material의 URP Shader 또는 Shader Graph는 4\~5단계의 계산 규칙을 정합니다. DAY 04 이후 Shader Graph를 배울 때도 "SRP/URP가 그릴 순서를 정하고, Graph가 그 순서 안에서 표면 값을 계산한다"고 연결해서 이해합니다.

각 항목의 역할은 다음과 같습니다.

| 구성 요소 | 역할 | 수업에서 확인할 위치 |
| :--- | :--- | :--- |
| SRP | Camera별 Render Loop에서 Culling·패스 구성·Draw Call 제출을 지휘하는 Unity의 기반 구조 | `Project Settings > Graphics`의 SRP 설정 |
| URP Asset | Render Scale, 조명, 그림자, 품질처럼 Render Loop의 프로젝트 공통 규칙을 보관 | Project 창에서 URP Asset 선택 |
| Universal Renderer Data | 그림자·불투명·투명·후처리 같은 Render Pass의 Renderer 구성 | URP Asset의 Renderer List에서 연결된 Renderer 선택 |
| Renderer Feature | 기본 Renderer 흐름에 필요한 추가 Render Pass를 넣는 확장 지점 | Universal Renderer Data Inspector의 Renderer Features |
| Camera | 무엇을 어느 범위에서 볼지 정하고 사용할 Renderer를 선택 | Main Camera의 Universal Additional Camera Data |
| Mesh Renderer·Material·Shader | SRP가 제출한 Draw Call에서 메시와 표면의 Vertex·Fragment 계산 규칙을 정함 | 씬 오브젝트의 Mesh Renderer와 Material Inspector |

처음에는 Renderer Feature를 추가하지 않습니다. DAY 01~03에서는 URP 기본 Renderer가 카메라·라이트·Lit Material을 어떤 Render Pass 순서로 함께 처리하는지 관찰합니다. Renderer Feature는 기본 흐름만으로 표현할 수 없는 화면 효과가 필요할 때, 목적과 비용을 이해한 뒤 추가합니다.

### Inspector에서 URP 구조 따라가기

1. `Project Settings > Graphics`에서 URP Asset을 찾습니다.
2. Project 창에서 같은 URP Asset을 선택하고 Inspector의 Renderer List를 펼칩니다.
3. 목록의 Default Renderer 또는 연결된 Renderer Data Asset을 선택합니다. Inspector에 `Universal Renderer Data`와 Renderer Features 목록이 보이는지 확인합니다.
4. Main Camera를 선택하고 `Universal Additional Camera Data`에서 Renderer가 Default인지, 별도 Renderer를 지정했는지 확인합니다. DAY 01에서는 Default Renderer를 사용합니다.
5. Sphere를 선택해 `Mesh Renderer > Materials > Element 0`의 Material을 열고, Shader가 `Universal Render Pipeline/Lit`인지 확인합니다.

이 연결 중 하나가 끊기면 화면이 예상과 달라질 수 있습니다. 예를 들어 Graphics·Quality에 URP Asset이 없으면 URP Shader를 올바르게 처리할 수 없고, Camera가 다른 Renderer를 쓰면 Renderer Feature나 후처리 결과가 기본 Camera와 달라질 수 있습니다.

## 3. Unity 6 그래픽 실험 씬 만들기

1. Unity 6에서 URP 템플릿 프로젝트를 만듭니다.
2. `GraphicsLab` 씬을 생성합니다.
3. Plane, Sphere, Cube를 배치합니다.
4. Directional Light와 Camera가 있는지 확인합니다.
5. 오브젝트마다 다른 머티리얼을 적용합니다.

## 4. Inspector에서 확인할 것

| 대상 | 확인 항목 | 의미 |
| :--- | :--- | :--- |
| Camera | Projection, Clear Flags, Clipping Planes | 어떤 범위를 어떤 방식으로 볼지 정합니다. |
| Light | Type, Intensity, Color, Shadows | 화면의 밝기와 그림자 품질을 정합니다. |
| Mesh Renderer | Materials, Shadow Casting | 어떤 머티리얼로 그릴지 정합니다. |
| URP Asset | Render Scale, Quality, Shadows | 프로젝트 전체 렌더링 품질을 정합니다. |
| Universal Renderer Data | Renderer Features, Rendering 순서 | URP가 실제로 사용할 Renderer 구성을 정합니다. |
| Universal Additional Camera Data | Renderer, Post Processing | Camera별 URP Renderer와 후처리 사용 여부를 정합니다. |

## 5. URP 프로젝트 설정 확인

1. `Edit > Project Settings > Graphics`를 열고 `Scriptable Render Pipeline Settings` 칸에 URP Asset이 들어 있는지 확인합니다. 비어 있으면 Built-in 파이프라인으로 동작하며 URP Shader Graph가 분홍색으로 보일 수 있습니다.
2. `Project Settings > Quality`에서 현재 선택한 품질 단계의 `Render Pipeline Asset`도 확인합니다. Graphics에는 URP Asset이 있어도 Quality 단계가 다른 Asset을 쓰면 Play Mode에서 그림자, Render Scale, 후처리 결과가 달라질 수 있습니다.
3. Project 창에서 URP Asset을 선택해 Inspector의 `Rendering`, `Quality`, `Lighting`, `Shadows` 묶음을 차례로 펼칩니다. 처음에는 Render Scale `1`, 그림자 활성화 상태, Main Light Shadow 설정만 읽고 임의로 여러 값을 바꾸지 않습니다.
4. `GraphicsLab` Hierarchy에는 Main Camera, Directional Light, Plane, Sphere, Cube가 있는지 확인합니다. Plane은 바닥, Sphere와 Cube는 같은 조명에서 머티리얼·셰이더 차이를 비교하는 대상입니다.
5. Sphere를 세 개 복제해 같은 위치 간격으로 두고, 각 Mesh Renderer의 `Materials > Element 0`에 서로 다른 머티리얼을 연결합니다. 한 번에 조명과 머티리얼을 모두 바꾸지 말고 하나만 바꾼 뒤 Game View에서 명암·반사를 비교합니다.

## 실습 미션

`GraphicsLab` 씬에서 같은 Sphere 3개를 만들고, 라이트 강도와 머티리얼 색을 하나씩 바꿉니다. 각 변화에 대해 "바꾼 Inspector 항목 / 이전 값 / 새 값 / 화면에서 달라진 점"을 실습 노트에 한 줄씩 기록합니다.

## 오늘의 정리

- 게임 그래픽은 오브젝트만으로 만들어지지 않고 카메라, 라이트, 머티리얼, 렌더 파이프라인이 함께 만든 결과입니다.
- URP 설정은 수업 전체의 기본 그래픽 규칙입니다.
- 다음 시간에는 머티리얼과 텍스처를 이용해 표면의 느낌을 바꿉니다.
