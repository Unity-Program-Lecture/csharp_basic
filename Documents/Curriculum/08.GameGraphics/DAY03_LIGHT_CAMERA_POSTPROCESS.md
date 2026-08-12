# DAY 03: 조명, 그림자, 카메라와 색 보정

오늘의 목표는 조명과 카메라를 "**게임 장면의 촬영 감독**"처럼 이해하고, 같은 오브젝트도 빛과 색 보정에 따라 다르게 보인다는 점을 확인하는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 조명 모델, 렌더링 품질 향상
- Unity 6 재구성: URP Light, Shadow, Volume, Camera 설정을 사용합니다.

## 1. 핵심 개념: "보이는 것은 물체와 빛의 합작이다"

게임 화면에서 색은 머티리얼 혼자 결정하지 않습니다. 빛의 방향, 색, 세기, 그림자, 카메라 노출, 후처리가 함께 결과를 만듭니다.

### 이 단어는 무슨 뜻인가요?

- **Directional Light**: 태양처럼 한 방향에서 전체 씬을 비추는 빛입니다.
- **Point Light**: 전구처럼 한 지점에서 사방으로 퍼지는 빛입니다.
- **Spot Light**: 손전등처럼 원뿔 모양으로 비추는 빛입니다.
- **Shadow**: 빛이 막혀 어두워진 영역입니다.
- **Volume**: 색 보정, Bloom, Vignette 같은 후처리 효과를 담는 설정 묶음입니다.

## 2. Lighting 창 설정은 어디에 적용되나요?

`Window > Rendering > Lighting`은 **현재 활성 씬의 조명 설정**을 다루는 창입니다. 즉 `Lighting_Day`를 열어 조명 환경이나 베이크 설정을 바꾸면, 기본적으로 `Lighting_Sunset`이나 `Lighting_Dungeon`을 열었을 때 그 설정을 직접 바꾸는 것은 아닙니다.

다만 Lighting 창에는 씬마다 연결하는 `Lighting Settings Asset`이 있습니다. 이 Asset은 여러 씬에서 공유할 수 있으므로, 같은 Asset을 `Lighting_Day`와 `Lighting_Sunset`에 연결했다면 Asset Inspector에서 값을 바꾼 결과는 두 씬에 모두 적용됩니다. 아래처럼 적용 범위를 구분합니다.

| 설정 대상 | 적용 범위 | 수업에서의 관리 기준 |
| :--- | :--- | :--- |
| Directional Light·Point Light GameObject | 해당 GameObject가 있는 씬 | 낮·저녁·던전 씬마다 각자 배치·색·세기를 조절합니다. |
| Lighting 창의 Environment 설정 | 현재 활성 씬 | Skybox, Ambient, Fog를 비교 씬마다 다르게 실험할 수 있습니다. |
| Lighting Settings Asset의 Scene·Lightmapping 설정 | Asset을 연결한 씬 | 베이크·Mixed Lighting 규칙이 다른 비교 씬은 별도 Asset을 만듭니다. |
| 베이크된 Lighting Data | 베이크를 실행한 씬 | 다른 씬의 베이크 결과를 그대로 재사용하지 않습니다. |
| URP Asset·Quality·Color Space | 프로젝트 또는 품질 단계 | DAY 01의 프로젝트 설정입니다. 낮·저녁·던전 비교를 위해 씬마다 바꾸지 않습니다. |
| Volume Profile | Profile Asset을 연결한 Volume | 씬마다 다른 분위기가 필요하면 별도 Profile Asset을 만들어 연결합니다. |

### 비교 씬별 Lighting Settings Asset 만들기

1. `Lighting_Day` 씬을 활성화하고 `Window > Rendering > Lighting`을 엽니다.
2. `Scene` 탭의 `Lighting Settings` 항목을 확인합니다. 이미 다른 비교 씬과 공유 중인 Asset이라면 이름을 확인한 뒤 바로 값을 바꾸지 않습니다.
3. `New Lighting Settings`를 눌러 새 Asset을 만들고 `GameGraphics/Settings/Lighting` 폴더에 `LS_Day`로 저장합니다. Unity는 새 Asset을 현재 활성 씬에 연결합니다.
4. `Lighting_Sunset`, `Lighting_Dungeon`을 각각 열어 같은 방법으로 `LS_Sunset`, `LS_Dungeon`을 만듭니다.
5. 베이크를 사용할 경우, 각 씬에서 해당 Lighting Settings Asset과 Baked Lightmaps 목록을 확인한 뒤 그 씬을 대상으로 Generate Lighting을 실행합니다.

이 수업에서는 URP Asset과 Quality 설정은 프로젝트 공통으로 유지하고, Light·Environment·Volume·Lighting Settings Asset만 비교 씬의 목적에 맞게 나눕니다. 그러면 "조명 연출의 차이"와 "프로젝트 렌더링 규칙의 차이"를 혼동하지 않을 수 있습니다.

### Lighting 창 탭과 항목 읽기

Lighting 창의 탭은 사용 중인 렌더 파이프라인과 베이크 기능에 따라 일부가 비어 있거나 보이지 않을 수 있습니다. DAY 03에서는 Environment와 Scene 탭을 먼저 사용하고, Baked Lightmaps는 베이크 결과를 확인할 때만 엽니다.

| 탭 | 주요 항목 | 무엇을 정하거나 확인하는가 | DAY 03에서의 사용 |
| :--- | :--- | :--- | :--- |
| `Scene` | Lighting Settings, Realtime Lighting, Mixed Lighting, Lightmapping Settings | 현재 씬의 Lighting Settings Asset과 실시간·혼합·베이크 조명 계산 규칙 | Asset을 씬별로 분리하고, 베이크 실습 전 설정을 확인합니다. |
| `Environment` | Skybox Material, Sun Source, Environment Lighting, Environment Reflections, Fog | 현재 씬의 하늘·주변광·반사·안개 | 낮·저녁·던전의 전체 분위기를 만들 때 사용합니다. |
| `Adaptive Probe Volumes` | APV 배치·베이크 관련 설정 | URP/HDRP의 간접광 프로브 볼륨 | 이 과정의 기본 실습에서는 다루지 않습니다. 큰 실내 씬의 동적 오브젝트 간접광이 필요할 때 별도 학습합니다. |
| `Realtime Lightmaps` | 현재 씬의 실시간 GI 라이트맵 목록 | Enlighten 실시간 GI가 만든 결과 확인 | Unity 6 URP 기본 실습에서는 비어 있을 수 있으며 값을 만드는 탭이 아닙니다. |
| `Baked Lightmaps` | 라이트맵, Lighting Data Asset | 현재 씬에서 베이크된 조명 결과 확인 | Generate Lighting 후 결과가 생겼는지 확인합니다. |
| 창 하단 Control 영역 | GPU Baking Device, GPU Baking Profile, Bake on Scene Load, Generate Lighting | 베이크 장치·메모리·자동 생성·수동 베이크 제어 | 베이크를 사용할 때만 변경합니다. 기본은 수동 `Generate Lighting`으로 둡니다. |

#### Scene 탭: 조명 계산 규칙

`Lighting Settings`는 현재 씬이 쓰는 Asset입니다. `Realtime Lighting`, `Mixed Lighting`, `Lightmapping Settings`는 Light GameObject 하나의 밝기를 바꾸는 곳이 아니라, 간접광·혼합광·베이크 데이터를 **어떤 방식과 품질로 계산할지** 정하는 규칙입니다.

| 항목 | 의미 | 처음 실습에서의 기준 |
| :--- | :--- | :--- |
| Realtime Lighting | 실행 중 또는 사전 계산된 실시간 전역 조명 관련 규칙 | DAY 03에서는 값을 임의로 바꾸지 않고, 해당 프로젝트에서 사용 가능한지만 확인합니다. |
| Mixed Lighting | Light가 실시간 그림자와 베이크 결과를 어떻게 섞을지 정하는 규칙 | Directional Light의 Mode를 Mixed로 쓸 때만 별도 비교합니다. |
| Lightmapping Settings | 베이크 엔진, 해상도, 샘플, 압축처럼 베이크 시간·품질·용량에 영향을 주는 규칙 | 처음에는 기본값으로 한 번 베이크하고, 노이즈·시간 문제가 있을 때 한 항목씩 조절합니다. |
| Lighting Data Asset | Generate Lighting이 만든 라이트맵·프로브 등의 결과 데이터 | 직접 수치 편집하지 않고, 현재 씬의 결과가 연결됐는지 확인합니다. |

#### Environment 탭: 장면 전체의 빛과 배경

Environment 탭은 개별 Light가 아니라 장면 전체에 깔리는 빛과 반사를 조절합니다. Light의 Intensity를 올리기 전에 아래 값을 확인하면 "왜 어두운가"를 더 정확히 판단할 수 있습니다.

| 항목 | 의미 | 조절할 때 확인할 점 |
| :--- | :--- | :--- |
| Skybox Material | 씬 배경과 Skybox 기반 환경광·반사의 원본 | 낮·저녁에 다른 Skybox를 쓰면 주변광과 반사도 함께 달라집니다. |
| Sun Source | Skybox가 태양 방향·세기를 참조할 Directional Light | `None`이면 Unity가 가장 밝은 Directional Light를 사용하므로, 비교 씬에서는 의도한 Directional Light를 직접 지정합니다. |
| Environment Lighting > Source | 주변광의 원본: `Skybox`, `Gradient`, `Color` | Skybox는 하늘색 영향을, Gradient는 하늘·지평선·지면 색을, Color는 균일한 주변광을 만듭니다. |
| Environment Lighting > Intensity Multiplier | 주변광 밝기 | Light Intensity와 함께 올리면 장면이 과노출될 수 있으므로 한쪽만 바꾼 뒤 비교합니다. |
| Environment Reflections > Source | 반사에 쓸 Skybox 또는 Custom Cubemap | Metallic·Smoothness가 높은 DAY 02 Material에서 차이가 잘 보입니다. |
| Environment Reflections > Intensity Multiplier | 전역 반사 밝기 | 금속 표면이 너무 검거나 과하게 반짝일 때 먼저 확인합니다. |
| Fog | 카메라 거리별 안개 | `Linear`는 Start·End 거리, `Exponential` 계열은 Density를 조절합니다. Camera Far Clip과 함께 확인합니다. |

#### 베이크 Control 영역: 언제 실행하나요?

`Generate Lighting`은 현재 열린 씬의 미리 계산된 조명 데이터를 생성합니다. Light의 Color·Intensity·Mode, Static 오브젝트, Skybox 또는 Lighting Settings Asset을 바꾼 뒤 베이크 조명을 쓰고 있다면 다시 실행해야 결과가 갱신됩니다. 반대로 DAY 03에서 실시간 Light·Volume 차이만 보는 중이라면 매번 Generate Lighting을 누를 필요가 없습니다.

`Bake on Scene Load`는 씬을 열 때 데이터가 없거나 오래됐으면 자동으로 베이크할지 정합니다. 수업에서는 예기치 않은 긴 대기를 피하기 위해 `Never`를 기본으로 두고, 필요할 때 `Generate Lighting`을 수동으로 실행합니다. `Clear Baked Data`는 열린 씬의 베이크 결과를 지우는 기능이므로, 비교가 끝나기 전에는 사용하지 않습니다.

## 3. 실습: 낮, 저녁, 던전 조명 만들기

1. 같은 씬을 복제해 `Lighting_Day`, `Lighting_Sunset`, `Lighting_Dungeon` 상태를 만듭니다.
2. Directional Light의 Rotation, Color, Intensity를 조절합니다.
3. Point Light를 추가해 횃불처럼 배치합니다.
4. Global Volume을 만들고 Bloom, Color Adjustments를 추가합니다.

### 씬과 오브젝트를 만드는 순서

1. DAY 02의 `GraphicsLab`을 연 뒤 `File > Save As`로 `Lighting_Day`를 저장합니다. 이 파일을 다시 `Save As`하여 `Lighting_Sunset`, `Lighting_Dungeon`을 만듭니다. 한 씬 안에서 상태를 섞지 않아야 세 조명 결과를 되돌려 비교할 수 있습니다.
2. Hierarchy에서 `Directional Light`를 선택하고 Transform의 Rotation부터 바꿉니다. 이때 Light의 Intensity·Color는 아직 기본값으로 두어 그림자 방향만 먼저 비교합니다.
3. 횃불 위치에는 `GameObject > Light > Point Light`를 선택합니다. 새 Point Light의 Transform을 벽이나 바닥에서 약간 띄우고, Inspector의 Range가 씬 전체를 덮지 않도록 먼저 작게 잡습니다.
4. `GameObject > Volume > Global Volume`을 선택합니다. 새 오브젝트의 Volume 컴포넌트에서 `New`를 눌러 Profile Asset을 만들고 `GameGraphics/Settings` 폴더에 `VP_Dungeon`처럼 저장합니다.
5. Main Camera를 선택해 `Universal Additional Camera Data`의 Post Processing을 켭니다. 이 설정이 꺼져 있으면 Volume Profile에 값을 넣어도 Game View에 후처리가 적용되지 않습니다.

## 주요 설정

| 설정 | 사용 목적 |
| :--- | :--- |
| Light Intensity | 전체 밝기 조절 |
| Shadow Strength | 그림자의 진하기 조절 |
| Bloom | 밝은 부분이 번져 보이는 효과 |
| Color Adjustments | 장면의 색감과 대비 조절 |
| Camera Clipping Planes | 카메라가 보는 거리 범위 조절 |

## Light·Camera·Volume Inspector 확인 절차

Directional Light를 선택해 Inspector의 `Light` 컴포넌트에서 Color, Intensity, Shadow Type, Rotation을 차례로 조절합니다. 낮·저녁·던전 상태는 한 번에 모두 바꾸지 말고, 먼저 Rotation만 바꿔 그림자 방향을 확인한 뒤 Color와 Intensity를 조절합니다. Point Light는 Range가 씬 크기에 비해 지나치게 크지 않은지, Intensity가 다른 Light를 덮지 않는지 확인합니다.

Global Volume을 만들면 Inspector의 `Profile`에 새 Volume Profile을 생성합니다. `Add Override`로 `Post-processing > Bloom`, `Post-processing > Color Adjustments`를 추가하고, 각 항목의 왼쪽 체크 상자가 켜져 있는지 확인합니다. Bloom은 Threshold를 낮출수록 더 많은 밝은 영역이 번지고, Intensity를 올릴수록 번짐이 강해집니다. Color Adjustments의 Post Exposure는 전체 밝기, Contrast는 명암 차이, Color Filter는 장면 전체 색조를 바꿉니다.

결과가 보이지 않으면 Main Camera의 Inspector에서 URP 추가 설정의 Post Processing이 켜져 있는지, Volume이 Global인지, Profile의 Override 체크가 켜져 있는지 순서대로 확인합니다. Camera의 Clipping Planes는 Near가 너무 크면 가까운 이펙트가 잘리고 Far가 너무 작으면 먼 배경이 사라지는 원인이 됩니다.

## 오늘의 정리

- 조명은 오브젝트의 입체감과 분위기를 만듭니다.
- 그림자와 후처리는 화면의 완성도를 높이지만 성능 비용도 있습니다.
- 다음 시간부터 Shader Graph로 직접 표면 계산을 만들어 봅니다.
