# DAY 07: Shader Graph에서 HLSL 코드로

오늘의 목표는 셰이더 코드를 외우는 것이 아닙니다. DAY 04와 DAY 06에서 Shader Graph로 연결한 선이 HLSL 코드에서는 어떤 **입력·함수·출력**으로 보이는지 읽는 것입니다.

```text
Shader Graph의 Blackboard·Vertex Position·Base Color
                    ↓
Material 값·정점 함수·픽셀 함수가 있는 HLSL 코드
```

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 정점 셰이더, 픽셀 셰이더, 셰이더 코드 작성과 테스트
- Unity 6 재구성: URP Unlit 셰이더 전체를 읽고, Shader Graph의 연결과 대응합니다.

## 오늘 도착할 곳

Cube에 적용한 Material의 색을 바꾸면 Cube 색이 바뀌는 가장 단순한 URP Unlit 셰이더를 읽습니다. 조명·텍스처·그림자는 잠시 제외합니다. 그러면 정점의 위치를 처리하는 부분과 픽셀 색을 처리하는 부분을 분명히 나눠 볼 수 있습니다.

이 문서의 예제는 Unity 6 URP용 `.shader` 파일 전체입니다. 기존 URP Lit Shader나 Shader Graph가 생성한 코드를 수정하지 말고, 별도의 학습용 Shader 파일에만 사용합니다.

## 1. 어제 만든 Shader Graph는 코드에서 어디에 있나요?

DAY 04에서 만든 Shader Graph의 주요 부분은 아래 코드의 역할과 연결됩니다.

| Shader Graph | HLSL 코드에서 대응하는 곳 | 하는 일 |
| :--- | :--- | :--- |
| Blackboard의 Exposed Property | `Properties`와 `UnityPerMaterial` | Material Inspector에서 조절할 값을 준비합니다. |
| Master Stack의 Vertex Position | `vert` 함수 | 정점의 화면 위치를 계산합니다. |
| Master Stack의 Base Color | `frag` 함수의 `return` | 픽셀의 최종 색을 계산합니다. |
| Graph의 연결선 | 변수 대입과 함수 호출 | 값이 어디에서 어디로 가는지 나타냅니다. |

Shader Graph에서는 선을 따라 값을 읽었습니다. HLSL에서는 `=`의 오른쪽 값이 왼쪽 변수로 들어갑니다. 따라서 코드도 **최종 출력에서 시작해, 필요한 입력 쪽으로 거꾸로** 읽으면 이해하기 쉽습니다.

## 2. GPU가 코드를 실행하는 순서

```text
Mesh의 정점 데이터
  POSITION, NORMAL, UV 등
        ↓ GPU가 Attributes 입력을 채움
vert 함수: 정점마다 실행
        ↓ Varyings output을 작성해 반환
삼각형 조립과 Rasterization
        ↓ Varyings의 전달값을 픽셀마다 보간
frag 함수: 삼각형 안의 픽셀마다 실행
        ↓ SV_Target으로 최종 색 반환
Camera 화면
```

먼저 GPU가 Mesh의 위치·법선·UV 같은 정점 데이터를 읽어 `vert` 함수의 입력에 넣습니다. `vert` 함수는 정점마다 실행되어 화면에 배치할 위치와 다음 단계에 전달할 값을 직접 작성합니다. GPU는 정점 세 개로 삼각형을 만들고, 삼각형 내부의 각 픽셀에 전달값을 보간합니다. 마지막으로 `frag` 함수가 픽셀마다 실행되어 색을 반환합니다.

> 정점 단계는 Cube의 꼭짓점처럼 **점마다** 실행되고, 픽셀 단계는 화면에서 Cube가 차지한 **픽셀마다** 실행됩니다. 화면에서 Cube가 크게 보일수록 `frag` 함수의 실행 횟수가 많아집니다.

## 3. 전체 예제 코드

아래 파일을 `Day07UnlitColor.shader`처럼 별도로 만들고 붙여 넣을 수 있습니다. 코드를 처음 볼 때는 `frag`의 반환값, `vert`의 `positionCS`, 그 다음 `Attributes` 순서로 거꾸로 읽습니다.

```hlsl
// Inspector에서 보일 Shader 메뉴 이름입니다.
Shader "GameGraphics/Day07 Unlit Color"
{
    // Material Inspector에 노출할 값을 선언합니다.
    Properties
    {
        // _BaseColor라는 색 값을 만들고, 기본값을 하늘색으로 설정합니다.
        _BaseColor ("Base Color", Color) = (0.2, 0.7, 1.0, 1.0)
    }

    // 렌더링에 사용할 Shader 설정 묶음입니다.
    SubShader
    {
        // 이 Shader가 URP용이며, 불투명 물체를 그린다고 표시합니다.
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        // 한 번의 그리기 작업을 정의합니다.
        Pass
        {
            // 이 Pass의 이름입니다.
            Name "ForwardUnlit"

            // URP의 일반적인 전방 렌더링 단계에서 실행한다고 표시합니다.
            Tags { "LightMode" = "UniversalForward" }

            // 여기부터 HLSL 셰이더 코드입니다.
            HLSLPROGRAM

            // 정점마다 vert 함수를 실행하도록 지정합니다.
            #pragma vertex vert

            // 화면의 픽셀마다 frag 함수를 실행하도록 지정합니다.
            #pragma fragment frag

            // TransformObjectToHClip 같은 URP 제공 함수를 가져옵니다.
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Material마다 다른 값을 SRP Batcher 호환 방식으로 묶습니다.
            CBUFFER_START(UnityPerMaterial)

                // Properties의 _BaseColor와 연결되는 실제 HLSL 변수입니다.
                half4 _BaseColor;

            // Material 값 묶음을 끝냅니다.
            CBUFFER_END

            // 메시에서 정점 데이터를 받는 입력 구조체입니다.
            struct Attributes
            {
                // 메시의 정점 위치를 positionOS에 넣습니다.
                // POSITION은 "정점 위치 데이터"를 뜻하는 시맨틱입니다.
                // OS는 Object Space, 즉 오브젝트 기준 좌표입니다.
                float4 positionOS : POSITION;
            };

            // vert 함수가 계산한 값을 frag 함수로 보내는 구조체입니다.
            struct Varyings
            {
                // 화면에 그릴 최종 정점 위치를 담습니다.
                // SV_POSITION은 GPU가 삼각형을 화면에 배치할 때 쓰는 시맨틱입니다.
                // CS는 Clip Space, 즉 투영까지 끝난 화면 배치용 좌표입니다.
                float4 positionCS : SV_POSITION;
            };

            // 정점마다 한 번 실행되는 정점 셰이더 함수입니다.
            // GPU가 POSITION 데이터를 input.positionOS에 채워서 전달합니다.
            Varyings vert(Attributes input)
            {
                // frag 단계로 보낼 빈 출력 구조체를 만듭니다.
                Varyings output;

                // 오브젝트 기준 정점 위치를 Clip Space 위치로 변환합니다.
                // 오브젝트 변환, 카메라 변환, 투영 변환이 반영됩니다.
                output.positionCS =
                    TransformObjectToHClip(input.positionOS.xyz);

                // 채운 출력 구조체를 GPU에 반환합니다.
                return output;
            }

            // 화면에서 삼각형으로 덮인 픽셀마다 한 번 실행되는 함수입니다.
            // SV_Target은 반환값을 최종 픽셀 색으로 사용하라는 시맨틱입니다.
            half4 frag(Varyings input) : SV_Target
            {
                // Material Inspector의 Base Color를 그대로 픽셀 색으로 반환합니다.
                return _BaseColor;
            }

            // HLSL 코드 블록을 끝냅니다.
            ENDHLSL
        }
    }
}
```

## 4. 구조체와 함수: 누가 값을 채우나요?

### `Attributes`: GPU가 채우는 입력 상자

`Attributes`는 정점 셰이더의 입력 구조체입니다. 이름은 작성자가 정하는 관례일 뿐이며, `MeshInput`처럼 바꿔도 됩니다. 중요한 것은 필드 뒤의 시맨틱입니다.

```hlsl
struct Attributes
{
    float4 positionOS : POSITION;
};
```

`positionOS`도 작성자가 정한 이름입니다. `positionOS`는 "Object Space Position"이라는 뜻의 관례입니다. 반면 `: POSITION`은 GPU에 "이 필드에는 Mesh의 정점 위치를 넣어라"고 알려 주는 시맨틱입니다. GPU가 `vert`를 호출하기 전에 이 시맨틱을 보고 `input.positionOS`를 자동으로 채웁니다.

Mesh에서 받을 수 있는 대표 시맨틱은 다음과 같습니다.

| `Attributes`의 시맨틱 | Mesh에서 받는 값 | 보통 쓰는 자료형 |
| :--- | :--- | :--- |
| `POSITION` | 정점 위치 | `float3`, `float4` |
| `NORMAL` | 정점 법선 | `float3` |
| `TANGENT` | 정점 탄젠트 | `float4` |
| `TEXCOORD0` ~ `TEXCOORD3` | UV 채널 0~3 | `float2` 등 |
| `COLOR` | 정점 색 | `float4` |
| `BLENDWEIGHT`, `BLENDINDICES` | 스킨드 메시의 뼈 가중치·인덱스 | `float4`, `uint4` |

`POSITION`은 Mesh의 정점 위치 채널 하나를 받으므로 일반적인 `Attributes` 구조체에서는 하나만 사용합니다. 월드 좌표가 필요해도 `POSITION`을 하나 더 받지 않고, 받은 Object Space 좌표를 변환합니다.

### `vert`: 작성자가 출력 상자를 채우는 함수

`Varyings vert(Attributes input)`은 `Attributes`를 받아 `Varyings`를 반환하는 함수입니다. `input`은 GPU가 채웠지만, `output`은 빈 상자입니다. 작성자가 화면 위치와 필요한 전달값을 넣어야 합니다.

```hlsl
Varyings output;
output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
return output;
```

`TransformObjectToHClip`은 URP의 `Core.hlsl`이 제공하는 함수입니다. Object Space 위치를 다음 경로로 변환해 Clip Space 위치를 만듭니다.

```text
Object Space → World Space → View Space (카메라 기준) → Clip Space
```

따라서 `positionCS`는 Camera Space 위치가 아니라, 카메라의 원근 또는 직교 투영까지 적용된 Clip Space 위치입니다. `SV_POSITION`은 GPU가 삼각형을 화면의 어디에 배치할지 알기 위한 특별한 출력 시맨틱입니다. `Varyings`에는 보통 하나만 둡니다.

### `Varyings`: 정점 단계와 픽셀 단계를 잇는 전달 상자

`Varyings`도 작성자가 정한 구조체 이름입니다. `vert`에서 값을 넣어 반환하면 GPU가 삼각형 안의 각 픽셀에 맞게 값을 보간해 `frag` 입력으로 전달합니다.

```hlsl
struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 uv         : TEXCOORD0;
    float3 positionWS : TEXCOORD1;
};
```

`TEXCOORD0`, `TEXCOORD1`은 여기서 "UV만 넣는 칸"이 아니라 정점에서 픽셀로 값을 보내는 번호 있는 전달 통로입니다. 예를 들어 `positionWS`처럼 월드 좌표를 전달할 수 있습니다. 같은 구조체 안에서는 `TEXCOORD0`을 두 번 쓰지 않고, `TEXCOORD1`, `TEXCOORD2`처럼 번호를 올립니다.

`Attributes`와 `Varyings`는 서로 다른 단계의 구조체이므로 두 구조체에 각각 `TEXCOORD0`이 하나씩 있는 것은 정상입니다. `Attributes`의 `TEXCOORD0`은 Mesh의 첫 번째 UV를 받고, `Varyings`의 `TEXCOORD0`은 `vert`에서 `frag`로 보낼 첫 번째 통로입니다.

### `frag`: 픽셀 색을 반환하는 함수

`half4 frag(Varyings input) : SV_Target`은 화면에서 삼각형으로 덮인 픽셀마다 실행됩니다. `SV_Target`은 반환값을 최종 픽셀 색으로 쓰라는 출력 시맨틱입니다. `half4`는 빨강(R), 초록(G), 파랑(B), 알파(A) 네 값을 묶은 색입니다.

이 기본 예제에서는 `frag`가 `input`을 사용하지 않습니다. Material 색 하나만 반환하기 때문입니다. UV·월드 좌표·법선처럼 `Varyings`에 담아 넘긴 값을 사용할 때 `input.uv`, `input.positionWS`처럼 읽습니다.

```hlsl
half4 frag(Varyings input) : SV_Target
{
    return _BaseColor;
}
```

## 5. 시맨틱: 이름이 아니라 데이터의 역할

시맨틱은 변수 이름이 아니라 변수 뒤의 `: POSITION`, `: SV_POSITION`처럼 붙는 표지입니다. GPU는 이 표지를 보고 어떤 데이터를 자동으로 채울지, 어디로 전달할지, 어떤 출력으로 처리할지 결정합니다.

| 코드 위치 | 주로 사용하는 시맨틱 | 역할 |
| :--- | :--- | :--- |
| `vert` 입력인 `Attributes` | `POSITION`, `NORMAL`, `TEXCOORD0` 등 | Mesh 정점 데이터 받기 |
| `vert` 출력인 `Varyings` | `SV_POSITION`, `TEXCOORD0` 등 | 화면 위치 출력, `frag`로 값 전달 |
| `frag` 입력인 `Varyings` | `SV_POSITION`, `TEXCOORD0` 등 | 보간된 위치·UV·법선 등 받기 |
| `frag` 반환값 | `SV_Target`, 필요 시 `SV_Depth` | 최종 색 또는 깊이 출력 |

`Attributes`의 `POSITION`과 `Varyings`의 `SV_POSITION`은 이름이 비슷해도 역할이 다릅니다.

- `POSITION`: Mesh에서 **Object Space 정점 위치를 입력받는** 시맨틱입니다.
- `SV_POSITION`: `vert`가 계산한 **화면 배치용 Clip Space 위치를 출력하는** 시맨틱입니다.

## 6. Material 값은 어디서 오나요?

Shader Graph의 Blackboard에서 Exposed한 `BaseColor`는 Material Inspector의 손잡이였습니다. HLSL에서는 `Properties`와 `UnityPerMaterial`이 같은 역할을 합니다.

```hlsl
Properties
{
    _BaseColor ("Base Color", Color) = (0.2, 0.7, 1.0, 1.0)
}

CBUFFER_START(UnityPerMaterial)
    half4 _BaseColor;
CBUFFER_END
```

`Properties`의 `_BaseColor`와 `CBUFFER` 안의 `_BaseColor`는 이름이 같아야 연결됩니다. `CBUFFER_START(UnityPerMaterial)`과 `CBUFFER_END`는 함수가 아니라 Material별 값을 GPU에 전달하는 상수 버퍼의 시작과 끝을 표시하는 Unity 매크로입니다.

`UnityPerMaterial`은 HLSL 예약어는 아니지만, URP의 SRP Batcher가 Material 속성 묶음으로 기대하는 특별한 이름입니다. Material마다 달라지는 색·숫자·벡터는 이 안에 선언합니다. 반대로 오브젝트 변환 행렬 같은 Unity 엔진 값은 `UnityPerDraw` 상수 버퍼와 관련이 있으며, 이 입문 예제에서는 `Core.hlsl`의 제공 기능을 사용하므로 직접 선언하지 않습니다.

## 7. 월드 좌표가 필요하면 어떻게 하나요?

`POSITION`으로 받는 값은 Object Space 위치입니다. 월드 좌표가 필요하면 `POSITION`을 하나 더 만들지 않고 변환 함수로 만듭니다. 그리고 픽셀 단계에서도 필요할 때만 `Varyings`의 비어 있는 전달 통로에 넣습니다.

```hlsl
struct Varyings
{
    float4 positionCS : SV_POSITION;
    float3 positionWS : TEXCOORD0;
};

Varyings vert(Attributes input)
{
    Varyings output;

    output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);

    return output;
}
```

이 방식은 DAY 06의 규칙과 같습니다. 서로 다른 좌표 공간의 값을 바로 더하지 않습니다. 예를 들어 Vertex Position에 넣을 최종 위치는 Object Space여야 하므로, World Space에서 계산한 Offset은 Object Space로 변환한 뒤 더합니다.

## 8. Shader Graph와 다시 비교하기

1. DAY 04 또는 DAY 06에서 만든 Shader Graph Asset을 엽니다.
2. Blackboard의 `BaseColor`를 고릅니다. Display Name은 Inspector에 보이는 이름이고, Reference 이름은 코드·C#에서 쓰는 이름입니다.
3. Graph의 `Base Color` 연결을 찾습니다. 이 예제에서는 `return _BaseColor;`가 가장 단순한 대응입니다.
4. Master Stack의 `Vertex Position` 연결을 찾습니다. 이 예제에서는 `TransformObjectToHClip(input.positionOS.xyz)`가 원래 위치를 화면용 위치로 바꾸는 대응입니다.
5. DAY 06의 `Position (Object) + Offset → Vertex Position`을 떠올립니다. HLSL에서는 `input.positionOS`에 Object Space Offset을 더한 뒤 `TransformObjectToHClip`에 넣는 형태가 됩니다.

## 9. 실습: 코드와 결과를 함께 확인하기

1. Project 창에서 `Create > Shader > Unlit Shader`를 선택하고 이름을 `Day07UnlitColor`로 만듭니다.
2. 파일 전체를 이 문서의 예제 코드로 바꾸고 저장합니다.
3. Material을 만들고 Shader를 `GameGraphics/Day07 Unlit Color`로 선택합니다.
4. Cube의 `Mesh Renderer > Materials > Element 0`에 Material을 연결합니다.
5. Material Inspector의 `Base Color`를 빨강·초록·파랑으로 바꿉니다.

| 확인할 결과 | 코드에서 찾을 곳 | 다르면 확인할 곳 |
| :--- | :--- | :--- |
| Cube의 색이 바뀜 | `return _BaseColor;` | Material이 Cube에 적용됐는지, `_BaseColor` 이름이 두 곳에서 같은지 확인합니다. |
| Cube가 화면에 보임 | `output.positionCS`와 `SV_POSITION` | `TransformObjectToHClip` 줄이 남아 있는지, Console의 첫 오류가 무엇인지 확인합니다. |
| Shader 목록에 보임 | `Shader "GameGraphics/Day07 Unlit Color"` | 파일을 저장하고, Console에 Shader 오류가 없는지 확인합니다. |
| 분홍색으로 보이지 않음 | URP Tag와 `Core.hlsl` | 프로젝트가 URP인지, Console의 첫 Shader 오류가 무엇인지 확인합니다. |

## 오늘의 정리

- Shader Graph의 Blackboard는 HLSL의 `Properties`와 `UnityPerMaterial`에, Graph의 선은 변수 대입과 함수 호출에 대응합니다.
- GPU는 `Attributes`의 시맨틱을 보고 정점 입력을 채웁니다. `vert`는 `Varyings output`을 직접 채워 반환합니다.
- `POSITION`은 Object Space 정점 위치를 받는 입력 시맨틱이고, `SV_POSITION`은 Clip Space 화면 위치를 출력하는 시맨틱입니다.
- `Varyings`의 `TEXCOORD0~n`은 정점에서 픽셀로 UV·월드 좌표·법선 등을 전달하는 통로입니다.
- `frag`는 픽셀마다 실행되어 `SV_Target`으로 최종 색을 반환합니다.
- 다음 시간에는 같은 전달 구조를 이용해 Toon, Rim Light, Outline 같은 비실사 표현을 만듭니다.
