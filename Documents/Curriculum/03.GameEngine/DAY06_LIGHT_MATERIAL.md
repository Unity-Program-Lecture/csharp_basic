# DAY 06: 조명과 머티리얼 기초

오늘의 목표는 조명과 머티리얼을 "**무대 조명과 물체 표면의 옷감**"처럼 이해하고, URP 기본 머티리얼에서 색, 금속성, 거칠기, 표면 요철, 발광을 조절해 보는 것입니다.

## 1. 핵심 개념: "쉐이더는 조리법, 머티리얼은 재료표"

조명은 물체를 보이게 하고 분위기를 만듭니다. 하지만 같은 조명을 받아도 플라스틱 공, 쇠구슬, 나무 상자는 전부 다르게 보입니다. 이 차이를 만드는 것이 머티리얼입니다.

여기서 **쉐이더**와 **머티리얼**을 구분해야 합니다.

- **쉐이더**는 빛을 어떻게 계산할지 정한 조리법입니다.
- **머티리얼**은 그 조리법에 넣을 색, 텍스처, 금속성, 매끄러움 같은 재료표입니다.

예를 들어 `Universal Render Pipeline/Lit` 쉐이더는 빛을 받는 일반 물체를 그리기 위한 조리법입니다. `Mat_Wood`, `Mat_Metal`, `Mat_Glass` 같은 머티리얼은 같은 Lit 쉐이더를 쓰더라도 서로 다른 색과 프로퍼티 값을 넣기 때문에 다른 표면처럼 보입니다.

### 이 단어는 무슨 뜻인가요?

- **Light**: 씬을 밝히는 컴포넌트입니다.
- **Shader**: 표면이 빛을 받았을 때 화면에 어떤 색으로 그릴지 계산하는 규칙입니다.
- **Material**: Shader에 넣을 색, 텍스처, 반사값 같은 설정을 저장한 에셋입니다.
- **Texture**: 표면에 붙이는 이미지입니다. 색 그림, 요철 정보, 금속성 정보처럼 용도별로 쓰입니다.
- **Base Map**: 물체의 기본 색 또는 기본 색 텍스처입니다.
- **Metallic**: 표면이 금속처럼 주변 환경을 반사하는 정도입니다.
- **Smoothness**: 표면이 매끄럽게 반사되는 정도입니다.
- **Normal Map**: 실제 모델을 더 복잡하게 만들지 않고 표면의 작은 굴곡처럼 보이게 하는 텍스처입니다.
- **Emission**: 물체 자체가 빛나는 것처럼 보이게 하는 설정입니다.

## 2. Unity Light 기본 이해하기

라이트는 씬 안의 물체를 볼 수 있게 만드는 "**무대 조명**"입니다. 어두운 무대에 배우가 서 있어도 조명이 없으면 관객은 배우의 표정과 옷 색을 볼 수 없습니다. Unity에서도 모델과 머티리얼이 이미 준비되어 있어도, 조명이 어떻게 비추는지에 따라 색, 그림자, 분위기가 크게 달라집니다.

Light 컴포넌트는 보통 `GameObject > Light` 메뉴에서 만들거나, 기존 라이트 오브젝트의 Inspector에서 설정합니다. 입문 단계에서는 라이트를 "**어디에서, 어떤 방향으로, 얼마나 강하게 비추는가**"로 먼저 읽으면 됩니다.

### 라이트 종류

| 종류 | 쉬운 비유 | 주로 쓰는 상황 |
| :--- | :--- | :--- |
| **Directional Light** | 태양빛처럼 씬 전체를 한 방향으로 비추는 빛 | 야외 낮, 전체 분위기, 기본 씬 조명 |
| **Point Light** | 전구처럼 한 점에서 사방으로 퍼지는 빛 | 횃불, 전등, 마법 구슬, 작은 광원 |
| **Spot Light** | 손전등처럼 원뿔 모양으로 비추는 빛 | 무대 조명, 가로등, 탐조등, 시선 유도 |
| **Area Light** | 넓은 면에서 부드럽게 퍼지는 빛 | 창문, 네온 간판, 스튜디오 조명 느낌 |

처음에는 `Directional Light` 하나로 전체 밝기를 잡고, 필요한 장소에 `Point Light`나 `Spot Light`를 추가하는 방식으로 연습합니다.

### 라이트 Inspector에서 먼저 볼 값

| 항목 | 쉬운 의미 | 값이 바뀌면 |
| :--- | :--- | :--- |
| **Type** | 라이트의 모양 | 태양빛, 전구빛, 손전등빛처럼 비추는 방식이 바뀜 |
| **Color** | 빛의 색 | 흰색, 노란색, 파란색처럼 씬의 분위기가 바뀜 |
| **Intensity** | 빛의 세기 | 값이 클수록 더 밝고 강하게 보임 |
| **Range** | 빛이 닿는 거리 | `Point Light`, `Spot Light`에서 영향 범위가 넓어짐 |
| **Spot Angle** | 원뿔 빛의 각도 | `Spot Light`의 빛줄기가 좁아지거나 넓어짐 |
| **Shadow Type** | 그림자 사용 여부 | 그림자가 생기면 입체감이 강해지지만 성능 비용도 커짐 |

라이트를 많이 배치하면 씬이 화려해질 수 있지만, 실시간 게임에서는 성능 비용도 함께 늘어납니다. 수업 실습에서는 먼저 라이트 수를 적게 두고, `Intensity`, `Range`, `Color`, `Shadow Type`을 바꿔 보며 차이를 눈으로 확인합니다.

### 빛과 그림자가 만드는 정보

빛은 단순히 화면을 밝게 하는 기능만 하지 않습니다. 밝은 면과 어두운 면의 차이는 물체의 방향과 형태를 알려 주고, 그림자는 물체가 바닥에 붙어 있는지 떠 있는지를 보여 줍니다. 그래서 조명은 그래픽 효과이면서 동시에 플레이어가 공간을 이해하게 도와주는 정보입니다.

예를 들어 바닥에 그림자가 없으면 큐브가 공중에 떠 있는지 바닥 위에 놓여 있는지 헷갈릴 수 있습니다. 반대로 그림자가 너무 진하면 물체의 색과 디테일이 묻힐 수 있습니다. 좋은 라이트 설정은 "**잘 보이게 하기**"와 "**분위기 만들기**" 사이의 균형을 맞추는 작업입니다.

### 라이트와 머티리얼은 함께 봐야 한다

라이트가 같아도 머티리얼이 다르면 반응이 달라집니다. 거친 돌은 빛이 넓게 퍼져 둔하게 보이고, 매끄러운 금속은 빛이 좁고 선명하게 반사됩니다. 반대로 머티리얼이 같아도 라이트 색과 방향이 바뀌면 완전히 다른 분위기로 보입니다.

그래서 조명 수업에서는 라이트만 따로 보지 않고, 항상 `Universal Render Pipeline/Lit` 머티리얼의 `Base Color`, `Metallic`, `Smoothness` 값과 함께 관찰합니다.

## 3. URP Lit 머티리얼 기본 설정

Unity 6의 URP 프로젝트에서 일반 3D 오브젝트는 보통 `Universal Render Pipeline/Lit` 쉐이더를 사용합니다. Inspector에서 머티리얼을 선택하면 크게 `Surface Options`와 `Surface Inputs`를 확인합니다.

### Surface Options: 어떻게 그릴 것인가

| 항목 | 쉬운 의미 | 수업에서 확인할 것 |
| :--- | :--- | :--- |
| **Surface Type** | 불투명 물체인지, 투명 물체인지 | 벽, 바닥은 `Opaque`, 유리나 보호막은 `Transparent` |
| **Blending Mode** | 투명 색을 뒤 배경과 섞는 방식 | 투명 머티리얼에서만 주로 확인 |
| **Render Face** | 앞면만 그릴지, 양면을 그릴지 | 얇은 종이, 풀잎처럼 양쪽이 보여야 하면 `Both` |
| **Alpha Clipping** | 알파값이 낮은 부분을 잘라낼지 | 철망, 잎사귀처럼 구멍 난 텍스처에 사용 |
| **Receive Shadows** | 다른 물체의 그림자를 받을지 | 바닥, 벽은 켜고 특수 효과는 끌 수 있음 |

### Surface Inputs: 표면이 어떤 재질인가

| 항목 | 쉬운 의미 | 값이 커지면 |
| :--- | :--- | :--- |
| **Base Map / Base Color** | 표면의 기본 색과 이미지 | 색이 더 진하거나 텍스처 무늬가 보임 |
| **Workflow Mode** | 반사를 `Metallic` 값으로 볼지, `Specular` 색으로 볼지 | 초급 수업에서는 보통 `Metallic`으로 시작 |
| **Metallic Map / Metallic** | 금속 같은 정도 | 주변 환경색을 더 강하게 반사함 |
| **Smoothness** | 표면의 매끄러운 정도 | 반사 하이라이트가 작고 선명해짐 |
| **Normal Map** | 표면의 작은 요철 | 돌기, 흠집, 홈이 있는 것처럼 보임 |
| **Occlusion Map** | 틈이나 구석의 어두움 | 홈과 구석이 더 눌려 보임 |
| **Emission** | 스스로 빛나는 색 | 네온, 용암, 마법 문양처럼 보임 |

입문 단계에서는 `Base Color`, `Metallic`, `Smoothness` 세 가지만 바꿔도 재질 차이를 꽤 잘 볼 수 있습니다. `Normal Map`과 `Emission`은 텍스처나 특수 효과가 필요할 때 추가로 다룹니다.

## 4. 쉐이더와 머티리얼의 관계 확인하기

같은 큐브 3개에 같은 쉐이더를 쓰더라도 머티리얼 값이 다르면 전혀 다른 물체처럼 보입니다.

| 머티리얼 예시 | Shader | Base Color | Metallic | Smoothness | 느낌 |
| :--- | :--- | :--- | :--- | :--- | :--- |
| `Mat_Plastic` | URP/Lit | 빨강 | 0 | 0.35 | 장난감 플라스틱 |
| `Mat_RoughStone` | URP/Lit | 회색 | 0 | 0.05 | 거친 돌 |
| `Mat_Metal` | URP/Lit | 밝은 회색 | 1 | 0.8 | 매끄러운 금속 |
| `Mat_Glow` | URP/Lit | 검정 | 0 | 0.2 | Emission을 켜면 빛나는 표식 |

즉, 쉐이더를 바꾸면 "**그리는 계산법**"이 바뀌고, 머티리얼 값을 바꾸면 "**그 계산법에 넣는 재료**"가 바뀝니다.

## 실습 예제: URP Lit 머티리얼 비교하기

**미션:** 큐브 4개에 서로 다른 URP Lit 머티리얼을 적용하고, `Base Color`, `Metallic`, `Smoothness`, `Emission` 차이를 눈으로 비교합니다.

1. 큐브 4개를 나란히 배치합니다.
2. 씬에 기본 `Directional Light`가 없다면 `GameObject > Light > Directional Light`로 하나 만듭니다.
3. Directional Light의 `Rotation`을 바꿔 빛이 큐브의 앞, 옆, 위에서 들어오는 차이를 확인합니다.
4. Directional Light의 `Color`, `Intensity`, `Shadow Type`을 바꿔 밝기와 그림자 변화를 확인합니다.
5. `Mat_Plastic`, `Mat_RoughStone`, `Mat_Metal`, `Mat_Glow` 머티리얼을 만듭니다.
6. 각 머티리얼의 Shader를 `Universal Render Pipeline/Lit`으로 둡니다.
7. `Mat_Plastic`: `Base Color`는 빨강, `Metallic`은 `0`, `Smoothness`는 `0.35`로 둡니다.
8. `Mat_RoughStone`: `Base Color`는 회색, `Metallic`은 `0`, `Smoothness`는 `0.05`로 둡니다.
9. `Mat_Metal`: `Base Color`는 밝은 회색, `Metallic`은 `1`, `Smoothness`는 `0.8`로 둡니다.
10. `Mat_Glow`: `Emission`을 켜고 푸른색 계열로 설정합니다.
11. 각 큐브의 `Mesh Renderer > Materials` 슬롯을 확인하고, 머티리얼 에셋을 바꾸면 표면 설정 묶음이 함께 바뀐다는 점을 확인합니다.
12. 같은 머티리얼을 두 큐브에 함께 적용한 뒤, 머티리얼 에셋의 값을 바꾸면 두 큐브가 동시에 바뀌는지도 확인합니다.
13. 큐브 근처에 `Point Light`를 하나 추가하고 `Range`와 `Intensity`를 조절해 가까운 물체만 밝아지는지 확인합니다.

### 실행해보면

플라스틱 머티리얼은 색은 뚜렷하지만 반사가 부드럽고 약합니다. 금속 머티리얼은 주변 빛과 환경을 더 강하게 반사합니다. Smoothness가 높은 물체는 하이라이트가 작고 선명하며, 낮은 물체는 빛이 넓게 퍼져 거칠게 보입니다.

Emission을 켠 머티리얼은 표면 자체가 밝아 보입니다. 다만 Emission이 켜졌다고 해서 항상 주변 오브젝트를 실제 조명처럼 밝히는 것은 아니므로, 빛을 비추는 역할은 `Light` 컴포넌트와 구분해서 봐야 합니다.

### 생각해보기

1. 같은 회색 큐브라도 `Metallic`이 `0`일 때와 `1`일 때 느낌이 어떻게 달라지나요?
2. `Smoothness`가 너무 높으면 돌이나 흙 재질이 왜 어색해 보일까요?
3. `Shader`를 바꾸는 것과 `Material` 값을 바꾸는 것은 어떤 차이가 있나요?
4. `Directional Light`와 `Point Light`는 빛이 퍼지는 방식이 어떻게 다른가요?
5. 그림자를 켜면 물체의 위치와 공간감이 왜 더 잘 보일까요?

## 오늘의 정리

- 라이트는 씬을 밝히고, 물체의 형태와 위치를 읽을 수 있게 도와주는 무대 조명입니다.
- `Directional Light`는 태양빛처럼 전체 방향을 만들고, `Point Light`와 `Spot Light`는 특정 위치나 방향을 강조할 때 사용합니다.
- `Color`, `Intensity`, `Range`, `Shadow Type`은 라이트 Inspector에서 먼저 확인할 핵심 값입니다.
- 쉐이더는 빛과 표면을 계산하는 조리법이고, 머티리얼은 그 조리법에 넣는 값 묶음입니다.
- URP 기본 3D 머티리얼은 보통 `Universal Render Pipeline/Lit` 쉐이더를 사용합니다.
- 초급 단계에서는 `Base Color`, `Metallic`, `Smoothness`만 비교해도 플라스틱, 돌, 금속의 차이를 설명할 수 있습니다.
- `Normal Map`은 실제 모델을 복잡하게 만들지 않고 표면의 작은 요철을 보여 주는 데 사용합니다.
- `Emission`은 물체가 스스로 빛나는 느낌을 주지만, 실제 조명 역할과는 구분해야 합니다.

## 별첨: 기본 Unity 쉐이더 문법

Unity에서 직접 작성하는 쉐이더 파일은 보통 `.shader` 확장자를 사용합니다. 초급 단계에서는 모든 문법을 외우기보다, 쉐이더가 어떤 순서로 구성되는지 읽을 수 있으면 충분합니다.

Unity 6의 URP 프로젝트에서는 ShaderLab이라는 Unity용 포장 문법 안에 HLSL 코드를 작성합니다.

```text
Shader
└─ Properties
   └─ Material Inspector에 노출할 값
└─ SubShader
   └─ 실제 렌더링 조건과 Pass 묶음
      └─ Pass
         └─ HLSLPROGRAM
            ├─ vertex 함수
            └─ fragment 함수
```

### 기본 구조

```ShaderLab
Shader "폴더이름/쉐이더이름"
{
    Properties
    {
        // 머티리얼 Inspector에 보일 값
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // HLSL 코드 작성 위치

            ENDHLSL
        }
    }
}
```

각 부분의 역할은 다음과 같습니다.

| 문법 | 역할 |
| :--- | :--- |
| `Shader "이름"` | Unity의 Shader 선택 메뉴에 표시될 이름 |
| `Properties` | 머티리얼 Inspector에 노출할 색, 숫자, 텍스처 값 |
| `SubShader` | 특정 렌더링 환경에서 사용할 실제 쉐이더 묶음 |
| `Tags` | URP용 쉐이더인지, 불투명 물체인지 같은 렌더링 조건 |
| `Pass` | 물체를 한 번 그리는 렌더링 단계 |
| `HLSLPROGRAM` | 실제 GPU에서 실행될 HLSL 코드 시작 |
| `#pragma vertex` | 어떤 함수를 정점 처리에 사용할지 지정 |
| `#pragma fragment` | 어떤 함수를 픽셀 색상 처리에 사용할지 지정 |
| `ENDHLSL` | HLSL 코드 종료 |

## 별첨 예제 1: 색상만 출력하는 URP Unlit 쉐이더

이 예제는 조명 계산을 하지 않고, 머티리얼에서 지정한 색을 그대로 출력합니다. 빛의 영향을 받지 않으므로 표식, 디버그 오브젝트, 단순 색상 확인에 적합합니다.

1. Project 창에서 `Create > Shader > Blank Shader` 또는 텍스트 파일을 만들어 `.shader` 확장자로 저장합니다.
2. 파일 이름을 `URP_Unlit_Color.shader`로 정합니다.
3. 아래 코드를 붙여 넣습니다.
4. 새 머티리얼을 만들고 Shader를 `SBS/URP Unlit Color`로 선택합니다.

```ShaderLab
Shader "SBS/URP Unlit Color"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _BaseColor;
            }
            ENDHLSL
        }
    }
}
```

### 코드 읽기

- `Properties`의 `_BaseColor`는 머티리얼 Inspector에서 바꿀 수 있는 색입니다.
- `Attributes`는 모델에서 들어오는 정점 정보를 받습니다.
- `Varyings`는 정점 함수에서 픽셀 함수로 넘길 값을 담습니다.
- `vert` 함수는 오브젝트 공간 좌표를 화면에 그릴 수 있는 좌표로 바꿉니다.
- `frag` 함수는 최종 픽셀 색을 반환합니다.
- `TransformObjectToHClip`은 URP에서 오브젝트 좌표를 화면용 좌표로 변환할 때 사용하는 함수입니다.

## 별첨 예제 2: 위쪽으로 갈수록 밝아지는 색상 쉐이더

이 예제는 모델의 로컬 Y 좌표를 이용해 아래쪽은 어둡게, 위쪽은 밝게 보이도록 만듭니다. 조명을 계산하지 않아도 간단한 색상 변화 원리를 확인할 수 있습니다.

```ShaderLab
Shader "SBS/URP Height Color"
{
    Properties
    {
        _BottomColor("Bottom Color", Color) = (0.1, 0.1, 0.1, 1)
        _TopColor("Top Color", Color) = (0.2, 0.8, 1, 1)
        _Height("Height Range", Float) = 2
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float height01 : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _BottomColor;
                half4 _TopColor;
                float _Height;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.height01 = saturate((IN.positionOS.y / _Height) + 0.5);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return lerp(_BottomColor, _TopColor, IN.height01);
            }
            ENDHLSL
        }
    }
}
```

### 코드 읽기

- `_BottomColor`는 아래쪽 색입니다.
- `_TopColor`는 위쪽 색입니다.
- `_Height`는 색이 바뀌는 높이 범위를 조절합니다.
- `saturate`는 값을 `0`에서 `1` 사이로 제한합니다.
- `lerp(a, b, t)`는 `t` 값에 따라 `a`와 `b` 사이의 색을 섞습니다.

## 별첨 실습: 직접 확인하기

1. 큐브 또는 캡슐 오브젝트를 하나 만듭니다.
2. `SBS/URP Unlit Color` 쉐이더를 사용하는 머티리얼을 적용합니다.
3. 머티리얼의 `Base Color`를 바꾸고 Game View에서 확인합니다.
4. 같은 오브젝트에 `SBS/URP Height Color` 쉐이더를 사용하는 머티리얼을 적용합니다.
5. `Bottom Color`, `Top Color`, `Height Range` 값을 바꾸며 색 변화가 어떻게 달라지는지 확인합니다.

### 주의할 점

- 이 별첨 예제는 빛을 계산하지 않는 Unlit 계열입니다.
- `Directional Light`를 돌려도 색이 크게 바뀌지 않는 것이 정상입니다.
- 조명 영향을 받는 표면을 만들려면 URP Lit 구조와 조명 계산을 추가로 다루어야 합니다.
- 포트폴리오에서는 직접 쉐이더를 많이 만드는 것보다, 쉐이더와 머티리얼의 역할 차이를 설명할 수 있는 것이 우선입니다.
