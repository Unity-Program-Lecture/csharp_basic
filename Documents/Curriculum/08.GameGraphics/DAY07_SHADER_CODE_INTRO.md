# DAY 07: 간단한 셰이더 코드와 렌더링 사고

오늘의 목표는 셰이더 코드를 완전히 외우는 것이 아니라, "**GPU가 각 점과 픽셀을 어떤 순서로 계산하는지**"를 읽는 감각을 갖는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 정점 셰이더, 픽셀 셰이더, 셰이더 코드 작성과 테스트
- Unity 6 재구성: URP에서 간단한 Unlit 셰이더 구조를 읽습니다.

## 1. Shader Graph와 코드의 관계

Shader Graph는 내부적으로 셰이더 코드를 만들어 냅니다. 코드를 직접 쓰는 수업은 짧게 다루되, 정점 단계와 픽셀 단계가 나뉜다는 구조는 반드시 이해해야 합니다.

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

## 스크린샷 체크포인트

- `Images/day07_shader_code_compare.png`: Shader Graph와 코드 구조를 나란히 정리한 화면

## 오늘의 정리

- 정점 단계는 위치, 픽셀 단계는 색을 주로 다룹니다.
- Shader Graph를 쓰더라도 셰이더의 기본 흐름을 알면 오류를 이해하기 쉽습니다.
- 다음 시간에는 Toon, Rim Light, Outline 같은 비실사 표현을 다룹니다.

