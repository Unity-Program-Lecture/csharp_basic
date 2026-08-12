# DAY 06: 정점 변형과 UV 애니메이션

오늘의 목표는 셰이더가 색만 바꾸는 도구가 아니라 "**표면의 점과 무늬를 움직이는 도구**"라는 점을 이해하는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 정점 셰이더를 사용한 변환 처리
- Unity 6 재구성: Shader Graph의 Vertex Position과 UV Offset을 사용합니다.

## 1. 정점과 UV의 차이

정점은 모델을 이루는 점입니다. UV는 텍스처를 어디에 붙일지 알려 주는 종이 도안 같은 좌표입니다. 정점을 움직이면 모델의 실루엣이 변하고, UV를 움직이면 표면 무늬가 흐르는 것처럼 보입니다.

### 이 단어는 무슨 뜻인가요?

- **Vertex**: 3D 모델을 이루는 꼭짓점입니다.
- **Vertex Shader**: 정점 위치를 처리하는 셰이더 단계입니다.
- **UV Animation**: 텍스처 좌표를 시간에 따라 움직이는 표현입니다.
- **Sine Wave**: 부드럽게 오르내리는 파도 같은 값입니다.

## 2. 실습: 흔들리는 풀 또는 흐르는 표면

1. Plane 또는 Quad를 준비합니다.
2. Shader Graph에서 Time과 Sine을 사용합니다.
3. Object Position 또는 UV 좌표를 이용해 위치마다 다른 흔들림을 만듭니다.
4. Vertex Position에 작은 Offset을 더합니다.

### 실습 Asset과 Inspector 준비

1. Hierarchy에서 `GameObject > 3D Object > Plane` 또는 `Quad`를 만들고 이름을 `AnimatedSurface`로 바꿉니다. Plane은 정점이 적으므로 흔들림이 각져 보이면 더 촘촘한 Mesh를 사용해야 한다는 점을 먼저 확인합니다.
2. `GameGraphics/Shaders`에서 DAY 04의 Shader Graph를 복제하거나 `Create > Shader Graph > URP > Lit Shader Graph`로 `SG_VertexWave`를 만듭니다.
3. `GameGraphics/Materials`에서 `Mat_VertexWave`를 만들고 Shader를 `SG_VertexWave`로 지정한 뒤, `AnimatedSurface`의 `Mesh Renderer > Materials > Element 0`에 연결합니다.
4. UV 흐름에 텍스처를 쓸 때는 텍스처 Asset을 선택해 Inspector의 Wrap Mode를 `Repeat`로 바꾸고 Apply를 누릅니다. 그 뒤 Sample Texture 2D의 Texture 입력과 UV 입력을 각각 연결합니다.
5. Material Inspector에 `Amplitude`, `Speed`, `Tiling`, `UvSpeed`처럼 조절할 값을 Exposed로 만들고, 처음에는 Amplitude를 작은 값으로 둡니다.

## 주의할 점

- 정점 수가 너무 적으면 부드럽게 변형되지 않습니다.
- 움직임 값이 너무 크면 모델이 찢어진 것처럼 보일 수 있습니다.
- 충돌 영역은 셰이더 변형을 따라가지 않습니다. 보이는 모양과 물리 판정은 다를 수 있습니다.

## Vertex·UV 애니메이션 연결 점검

Vertex 변형은 Graph Inspector의 Vertex 영역에서 처리합니다. 오브젝트마다 자기 축 기준으로 흔들리게 하려면 `Position` 노드 Space를 `Object`로 두고 `Time`과 `Sine`으로 만든 Object Space Offset을 더해 `Vertex Position` 블록에 연결합니다. 여러 오브젝트를 가로지르는 같은 방향의 바람을 만들려면 `Position (World)`에서 Noise·Time을 계산한 뒤, `Transform` 노드의 `From = World`, `To = Object`로 Offset을 변환해 원래 Object Position에 더합니다. `Vertex Position`은 Object Space 최종 위치를 받으므로 Object Position과 World Offset을 그대로 더지 않습니다.

처음에는 Amplitude를 작은 값으로 두고, 움직임이 너무 크면 모델이 원래 위치에서 멀어지거나 Bounds 밖으로 나갈 수 있다는 점을 확인합니다. Vertex Position에 선이 연결되지 않고 Fragment의 Base Color에만 연결되면 색만 변하고 메시가 흔들리지는 않습니다. 반대로 Base Color·Emission에 World Space Position으로 계산한 Noise를 쓰는 경우에는 Fragment 색 계산이므로 Object Space로 다시 변환할 필요가 없습니다.

UV 애니메이션은 `UV`에 Time 기반 Vector2 Offset을 더한 결과를 Sample Texture 2D의 UV 입력에 연결합니다. Offset X만 바꾸면 가로 흐름, Y만 바꾸면 세로 흐름입니다. 재생 중 텍스처가 끊기는 것처럼 보이면 Texture Import Settings의 Wrap Mode를 `Repeat`로 확인하고, 고정된 그림처럼 보이면 Time 출력과 Speed 값 연결을 확인합니다.

Shader Graph의 변형은 렌더링 결과만 바꿉니다. Collider, NavMesh, Raycast 판정은 자동으로 따라가지 않으므로, 보이는 파도나 흔들림을 실제 충돌 높이로 사용하면 안 됩니다.

## 오늘의 정리

- 정점 변형은 보이는 모델의 위치를 셰이더 단계에서 바꿉니다.
- UV 애니메이션은 텍스처 무늬를 움직입니다.
- 다음 시간에는 셰이더 코드를 짧게 읽어 Shader Graph 뒤에서 어떤 일이 일어나는지 감각을 잡습니다.
