# 게임 그래픽 프로그래밍 환경 설정 안내

이 문서는 `08.GameGraphics` 수업을 시작하기 전에 Unity 프로젝트와 작업 환경을 한 번에 준비하기 위한 별첨 안내서입니다. 수업 기본 환경은 **Unity 6 + URP + C# + Shader Graph + Particle System + Visual Effect Graph**입니다.

> 이 과정은 Built-in Render Pipeline이나 HDRP가 아니라 URP를 기준으로 합니다. 기존 프로젝트를 억지로 URP로 바꾸기보다 새 URP 프로젝트에서 시작하는 편이 안전합니다.

## 1. 최소 환경

| 항목 | 최소 기준 | 수업 권장 기준 | 이유 |
| :--- | :--- | :--- | :--- |
| Unity Editor | Unity 6 `6000.0.0` 이상 | Unity Hub에서 설치한 Unity 6 `6000.0` 최신 패치 | URP 17 계열과 Shader Graph·VFX Graph 실습 기준입니다. |
| 프로젝트 템플릿 | `Universal 3D` | 새 프로젝트 `GameGraphicsLab` | URP Asset과 기본 렌더링 구성이 함께 생성됩니다. |
| 운영체제 | Windows 10/11 64-bit | Windows 11 64-bit | 수업의 PC Play Mode와 Standalone 확인 기준입니다. |
| 그래픽 하드웨어 | DirectX 11 이상을 지원하는 GPU | Compute Shader를 지원하는 외장 또는 내장 GPU | DAY 12~13의 VFX Graph는 GPU Compute Shader를 사용합니다. |
| IDE | C# 편집 가능한 IDE 1개 | Visual Studio 또는 VS Code | DAY 07, DAY 11, DAY 13에서 C# 코드와 셰이더 구조를 확인합니다. |

VFX Graph는 모든 장비에서 같은 결과를 보장하지 않습니다. DAY 12 시작 전에는 대상 PC에서 `SystemInfo.supportsComputeShaders`가 `true`인지 확인하고, 지원하지 않는 장비에서는 DAY 09~11의 Particle System 결과를 기본 제출물로 사용합니다.

## 2. 새 URP 프로젝트 만들기

1. Unity Hub에서 `New project`를 선택합니다.
2. `Universal 3D` 템플릿을 선택합니다.
3. 프로젝트 이름을 `GameGraphicsLab`으로 정하고, 학습용 폴더에 생성합니다.
4. 처음 열리면 Package Manager가 설치를 마칠 때까지 기다린 뒤 Console의 Error를 먼저 확인합니다.
5. `Assets/GameGraphics/Scenes/GraphicsLab` 씬을 만들고 저장합니다.

처음부터 다음 폴더를 만들면 DAY별 에셋을 찾기 쉽습니다.

```text
Assets
└── GameGraphics
    ├── Scenes
    ├── Materials
    ├── Shaders
    ├── Textures
    ├── Prefabs
    │   └── Effects
    ├── VFX
    ├── Scripts
    └── Input
```

## 3. 필수 패키지와 버전 규칙

그래픽 패키지는 Unity 6에 포함된 호환 버전을 함께 사용합니다. URP·Shader Graph·VFX Graph는 서로 다른 주 버전으로 섞지 않습니다. 예를 들어 URP가 `17.0.x`라면 Shader Graph와 VFX Graph도 `17.0.x` 계열을 사용합니다.

| 패키지 | Package ID | 최소 기준 | 설치·확인 방법 | 사용하는 DAY |
| :--- | :--- | :--- | :--- | :--- |
| Universal Render Pipeline | `com.unity.render-pipelines.universal` | `17.0.0` 이상 | `Universal 3D` 템플릿에 포함됐는지 Package Manager에서 확인 | DAY 01~14 |
| Shader Graph | `com.unity.shadergraph` | `17.0.0` 이상, URP와 같은 계열 | URP 설치 상태에서 Package Manager 확인 | DAY 04~08, 14 |
| Visual Effect Graph | `com.unity.visualeffectgraph` | `17.0.0` 이상, URP·Shader Graph와 같은 계열 | Package Manager의 `All`에서 설치 | DAY 12~14 |
| Input System | `com.unity.inputsystem` | `1.17.0` 이상 | Package Manager에서 설치·버전 확인 | DAY 11, 13 |
| Test Framework | `com.unity.test-framework` | Unity 6 기본 포함 버전 | 선택 사항. 코드 자동 검증을 추가할 때만 사용 | 선택 |

### 패키지 설치 순서

1. `Window > Package Manager`를 엽니다.
2. `Packages: Unity Registry` 또는 `All`에서 `Universal RP`와 `Shader Graph`가 설치됐는지 확인합니다.
3. `Visual Effect Graph`를 선택하고 `Install`합니다.
4. `Input System`을 설치합니다.
5. 설치 후 Unity가 재시작을 요청하면 재시작하고, Console Error가 없는지 확인합니다.

VFX Graph는 Scriptable Render Pipeline에서 동작하고 Compute Shader로 시뮬레이션합니다. 또한 VFX Graph와 렌더 파이프라인은 같은 버전 계열로 맞춰야 합니다. [Visual Effect Graph 시작하기](https://docs.unity3d.com/kr/Packages/com.unity.visualeffectgraph%4010.8/manual/GettingStarted.html)

## 4. 프로젝트 설정

### URP와 색 공간

`Edit > Project Settings`에서 다음을 확인합니다.

| 위치 | 설정 | 수업 기본값 | 확인 이유 |
| :--- | :--- | :--- | :--- |
| Graphics | Scriptable Render Pipeline Settings | URP Asset 연결됨 | 분홍색 셰이더와 Built-in Shader 혼용을 방지합니다. |
| Quality | 각 사용할 품질 단계의 Render Pipeline Asset | 같은 URP Asset 또는 의도한 URP Asset | Play Mode 품질 변경 시 씬 표현이 갑자기 달라지는 일을 막습니다. |
| Player > Other Settings | Color Space | `Linear` | 조명·PBR·Bloom 결과를 일관되게 비교합니다. |
| Player > Active Input Handling | `Input System Package (New)` 또는 `Both` | DAY 11·13의 `PlayerInput` 사용 조건입니다. |

DAY 03에서 Volume을 확인할 때는 Main Camera의 URP 추가 설정에서 Post Processing이 활성화되어 있는지도 확인합니다. Volume Profile을 만들기 전에 이 항목이 꺼져 있으면 Bloom과 Color Adjustments 결과가 보이지 않을 수 있습니다.

### 텍스처 가져오기 기본 점검

DAY 02부터는 텍스처를 씬에 넣기 전에 Inspector의 Texture Type을 확인합니다.

| 용도 | Texture Type | 주의 |
| :--- | :--- | :--- |
| Base Map, Emission, Mask | `Default` | 색 정보가 필요하면 sRGB 설정을 유지합니다. |
| Normal Map | `Normal map` | Default 상태로 연결하지 말고 Unity의 변환을 적용합니다. |
| Metallic·Roughness·AO 등 데이터 맵 | `Default` | 색이 아닌 수치 데이터이므로 프로젝트의 머티리얼 규칙에 맞춰 sRGB 설정을 확인합니다. |

## 5. IDE 설정

Visual Studio 또는 VS Code 중 하나를 사용합니다.

### VS Code를 사용할 때

Extensions에서 Microsoft 제공 확장을 설치합니다.

| 확장 | 용도 |
| :--- | :--- |
| `C#` | C# 언어 기능 |
| `C# Dev Kit` | 프로젝트 탐색·디버그·솔루션 기능 |
| `Unity` | Unity 파일과 C# 작업 보조 |

Unity Package Manager에서는 `Unity Visual Studio Editor` (`com.unity.ide.visualstudio`) `2.0.20` 이상을 설치합니다. VS Code 전용의 예전 `Visual Studio Code Editor` 패키지는 이 과정의 필수 조건으로 사용하지 않습니다.

### Visual Studio를 사용할 때

Unity 설치 워크로드가 포함된 Visual Studio를 사용하고, Unity에서 `Edit > Preferences > External Tools`의 External Script Editor가 올바른 IDE를 가리키는지 확인합니다.

## 6. 공통 씬 구성

처음에는 DAY 01에 필요한 뼈대만 둡니다. Volume, Particle System, VFX Graph, `PlayerInput`, 이펙트 Prefab은 미리 만들지 않고 각 DAY의 실습에서 추가합니다.

```text
GraphicsLab
├── Main Camera
├── Directional Light
├── Plane
├── Sphere
└── Cube
```

| 오브젝트 | 처음에 필요한 이유 | 이후 DAY에서 추가할 것 |
| :--- | :--- | :--- |
| Main Camera | 씬과 머티리얼·이펙트 결과를 확인합니다. | DAY 03에서 Clipping Planes와 화면 구도를 조절합니다. |
| Directional Light | PBR 머티리얼의 명암을 비교합니다. | DAY 03에서 색·세기·방향을 바꾸고 Point Light를 추가합니다. |
| Plane | 재질과 클릭 위치 이펙트의 기준 바닥입니다. | DAY 11에서 Collider와 `Ground` Layer를 확인합니다. |
| Sphere, Cube | Lit 머티리얼과 Shader Graph 결과를 비교합니다. | DAY 04~08에서 Shader Graph 머티리얼을 적용합니다. |

DAY 11에는 `PlayerInput`과 `ClickEffectSpawner`를 붙일 빈 오브젝트를 만들고, Plane에 Collider와 `Ground` Layer를 설정합니다. `PlayerInput`의 Behavior는 `Send Messages`이며 Action은 아래 이름을 그대로 사용합니다.

| Action | Type | Binding 예시 | 연결 메서드 |
| :--- | :--- | :--- | :--- |
| `Point` | Value / Vector2 | `<Pointer>/position` | `OnPoint` |
| `Click` | Button | `<Mouse>/leftButton` | `OnClick` |
| `LowIntensity` | Button | `<Keyboard>/1` | `OnLowIntensity` |
| `HighIntensity` | Button | `<Keyboard>/2` | `OnHighIntensity` |

## 7. DAY별 시작 전 점검

| DAY | 시작 전에 확인할 것 |
| :--- | :--- |
| DAY 01 | URP Asset 연결, `GraphicsLab` 씬, Camera·Light·Plane·Sphere·Cube |
| DAY 02 | URP Lit Material, Base Map·Normal Map·Metallic Map의 Import Settings |
| DAY 03 | Main Camera, Directional Light, Global Volume과 Volume Profile |
| DAY 04~08 | URP Shader Graph 생성 메뉴, Material 적용 대상 Mesh Renderer |
| DAY 09~10 | Particle System, Effect Prefab 폴더, 재생할 테스트 오브젝트 |
| DAY 11 | Input System, `PlayerInput`, Point·Click Action, Plane Collider·Ground Layer |
| DAY 12 | Visual Effect Graph 설치, Compute Shader 지원 확인 |
| DAY 13 | `SpawnRate` Exposed Property, LowIntensity·HighIntensity Action |
| DAY 14 | 최종 씬, Shader Graph·Particle Prefab·VFX Graph·게임 사건 연결 |

## 8. 수업 시작 전 5분 점검표

| 확인 항목 | 완료 |
| :--- | :--- |
| Unity Editor가 Unity 6 `6000.0` 이상이다. |  |
| 새 프로젝트가 `Universal 3D` 템플릿으로 생성됐다. |  |
| Graphics와 Quality에 URP Asset이 연결돼 있다. |  |
| URP·Shader Graph·VFX Graph가 같은 `17.0.x` 계열이다. |  |
| Input System이 `1.17.0` 이상이고 Active Input Handling이 맞다. |  |
| VFX Graph를 사용할 PC가 Compute Shader를 지원한다. |  |
| Color Space가 `Linear`다. |  |
| `GraphicsLab` 씬에 Camera·Directional Light·Plane·Sphere·Cube가 있다. |  |
| Plane에 Collider가 있고 DAY 11부터 사용할 `Ground` Layer를 정했다. |  |
| Console에 빨간 Error가 없다. |  |

## 9. 자주 막히는 문제

| 증상 | 원인 후보 | 확인 순서 |
| :--- | :--- | :--- |
| 오브젝트가 분홍색으로 보임 | Built-in Shader 사용, URP Asset 미연결, 패키지 버전 불일치 | Graphics·Quality의 URP Asset, Material Shader, 패키지 계열 순서로 확인 |
| Shader Graph 생성 메뉴가 없음 | Shader Graph 패키지 미설치 또는 URP 프로젝트가 아님 | Package Manager와 Render Pipeline을 확인 |
| Bloom·색 보정이 보이지 않음 | Volume Profile 미연결, Camera Post Processing 비활성 | Global Volume과 Camera 설정을 확인 |
| VFX Graph 메뉴·컴포넌트가 없음 | VFX Graph 패키지 미설치 | Package Manager에서 설치 후 재시작 |
| VFX가 재생되지 않음 | Compute Shader 미지원, Bounds·Spawn Rate·Output·Visual Effect 에셋 연결 오류 | PC 지원 여부, Inspector 연결, Graph 흐름 순서로 확인 |
| 클릭 이펙트가 생성되지 않음 | PlayerInput Action 이름, Camera, Ground LayerMask, Plane Collider 오류 | `Point`·`Click`, `Send Messages`, Camera·LayerMask·Collider 순서로 확인 |

## 오늘의 정리

- 이 과정은 Unity 6의 URP 프로젝트에서 시작합니다.
- URP·Shader Graph·VFX Graph는 같은 `17.0.x` 패키지 계열을 유지합니다.
- 공통 씬은 DAY 01의 Camera·Light·Plane·Sphere·Cube만 두고, 기능 오브젝트는 해당 DAY에 추가합니다.
- VFX Graph는 패키지 설치뿐 아니라 GPU Compute Shader 지원까지 확인해야 합니다.
