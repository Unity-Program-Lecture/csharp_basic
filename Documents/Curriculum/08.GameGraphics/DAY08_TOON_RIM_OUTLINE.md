# DAY 08: 비실사 렌더링과 후처리 표현

오늘의 목표는 현실 같은 그래픽만 좋은 그래픽이 아니라, 게임의 장르와 감정에 맞는 "**의도된 화면 스타일**"이 중요하다는 점을 배우는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기
- 관련 학습 내용: 비실사 렌더링, 셰이더로 게임 개성 표현
- Unity 6 재구성: Toon, Rim Light, Outline, 후처리 색감 조절을 실습합니다.

## 1. 비실사 렌더링이란?

비실사 렌더링은 현실처럼 보이기보다 만화, 애니메이션, 일러스트 같은 느낌을 목표로 하는 렌더링입니다. 셰이더는 게임의 "화풍"을 만드는 도구가 될 수 있습니다.

### 주요 표현

| 표현 | 설명 |
| :--- | :--- |
| Toon Shading | 음영을 부드럽게 섞지 않고 단계적으로 나눕니다. |
| Rim Light | 가장자리에 빛을 둘러 캐릭터를 돋보이게 합니다. |
| Outline | 외곽선을 추가해 만화 같은 실루엣을 만듭니다. |
| Color Grading | 장면 전체 색감과 대비를 조절합니다. |

## 2. 실습: 캐릭터 강조 머티리얼

1. 캐릭터 또는 Capsule을 준비합니다.
2. Shader Graph에서 Normal Vector와 View Direction을 사용합니다.
3. Fresnel Effect로 Rim Light를 만듭니다.
4. 색 보정 Volume을 추가해 장면 전체 분위기를 맞춥니다.

## 스크린샷 체크포인트

- `Images/day08_rim_light_graph.png`: Rim Light Shader Graph
- `Images/day08_toon_result.png`: 적용 전/후 비교 화면

## 오늘의 정리

- 셰이더는 게임의 화풍을 만드는 도구입니다.
- Rim Light와 Outline은 캐릭터 가독성을 높이는 데 유용합니다.
- 다음 시간부터는 이펙트 프로그래밍으로 넘어가 Particle System을 다룹니다.

