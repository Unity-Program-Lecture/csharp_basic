# DAY 07: 간단한 셰이더 코드와 렌더링 사고

오늘의 목표는 셰이더 코드를 완전히 외우는 것이 아니라, "**GPU가 각 점과 픽셀을 어떤 순서로 계산하는지**"를 읽는 감각을 갖는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 정점 셰이더, 픽셀 셰이더, 셰이더 코드 작성과 테스트
- Unity 6 재구성: URP에서 간단한 Unlit 셰이더 구조를 읽습니다.

## 1. Shader Graph와 코드의 관계

DAY 04에서 Shader Graph의 `Vertex Position`과 `Base Color`·`Emission`이 서로 다른 실행 단계에 연결된다는 점을 확인했습니다. Shader Graph는 내부적으로 셰이더 코드를 만들어 냅니다. 이 DAY는 정점·픽셀 단계를 처음 배우는 시간이 아니라, 이미 사용한 Graph 구조가 HLSL 코드에서는 어떻게 보이는지 확인하는 시간입니다.

### 이 단어는 무슨 뜻인가요?

- **HLSL**: Unity 셰이더에서 자주 만나는 GPU용 프로그래밍 언어입니다.
- **Vertex 단계**: 모델의 점을 화면 공간으로 옮기는 단계입니다.
- **Fragment/Pixel 단계**: 화면에 찍힐 색을 계산하는 단계입니다.
- **Unlit**: 조명 계산 없이 지정한 색을 그대로 보여주는 방식입니다.

## 2. 코드 읽기 예시

```hlsl
// 개념 이해용 축약 예시입니다.
float4 baseColor;

Varyings vert(Attributes input)
{
    Varyings output;
    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
    return output;
}

half4 frag(Varyings input) : SV_Target
{
    return baseColor;
}
```

읽는 순서는 간단합니다. `vert`에서 오브젝트의 점을 화면 좌표로 바꾸고, `frag`에서 픽셀 색을 돌려줍니다.

## 실습 미션

Shader Graph로 만든 그래프와 셰이더 코드 예시를 비교하며 다음 질문에 답합니다.

1. 그래프의 Base Color는 코드에서 어떤 값에 가까울까요?
2. 정점 단계에서 위치를 바꾸면 어떤 결과가 생길까요?
3. 픽셀 단계에서 색을 바꾸면 어떤 결과가 생길까요?

## Editor에서 Graph와 코드를 비교하는 순서

1. DAY 04 또는 DAY 06에서 만든 Shader Graph Asset을 더블 클릭해 Graph 창을 엽니다.
2. Blackboard에서 `BaseColor` 같은 프로퍼티를 선택하고 Display Name과 Reference 이름을 구분해 읽습니다. Material Inspector에는 Exposed된 Display Name이 보이고, 코드·C# 연결에서는 Reference 이름을 사용합니다.
3. Graph의 Vertex Position 입력과 Base Color·Emission·Alpha 출력을 찾습니다. 이 위치를 위 예시의 `vert`와 `frag` 함수에 각각 대응시켜 봅니다.
4. Material을 선택해 `BaseColor` 값을 바꾸고 Game View 결과를 확인합니다. 다음에는 Graph의 Base Color 연결을 임시로 다른 Color 노드로 바꾼 뒤 저장하여, Inspector 값과 Graph 연결 중 어느 쪽이 결과를 바꾸는지 구분합니다.

이 DAY의 HLSL 예시는 구조를 읽기 위한 축약 코드입니다. 기존 URP Lit Shader나 Shader Graph가 생성한 코드를 직접 수정해 수업 Asset으로 저장하지 않습니다. 코드 작성 실습은 별도 검증된 URP 셰이더 템플릿을 사용할 때 진행합니다.

## Graph와 코드 구조를 대조하는 방법

Shader Graph의 Vertex Position 연결은 코드의 정점 함수가 반환하는 위치와 대응하고, Base Color·Emission·Alpha 연결은 코드의 Fragment 함수가 반환하는 색과 대응합니다. 코드에서 `float4`는 보통 색 또는 위치처럼 네 값을 묶은 자료이며, Fragment의 `return` 직전 값이 화면의 한 픽셀 색이 됩니다.

코드를 읽을 때는 먼저 Properties에서 Material Inspector에 노출되는 값을 찾고, 다음으로 Vertex 함수에서 위치가 바뀌는지, Fragment 함수에서 텍스처·빛·색이 어떻게 합쳐지는지 순서로 봅니다. Inspector에서 바꾼 값과 코드의 변수 이름이 다르면 연결되지 않으므로 Property 이름, Reference 이름, C#에서 사용하는 문자열을 각각 구분해 확인합니다.

## 오늘의 정리

- 정점 단계는 위치, 픽셀 단계는 색을 주로 다룹니다.
- Shader Graph를 쓰더라도 셰이더의 기본 흐름을 알면 오류를 이해하기 쉽습니다.
- 다음 시간에는 Toon, Rim Light, Outline 같은 비실사 표현을 다룹니다.
