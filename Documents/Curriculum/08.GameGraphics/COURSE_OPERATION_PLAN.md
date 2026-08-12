# 게임 그래픽 프로그래밍 운영 계획

이 문서는 `26국기_교수계획서.pdf`의 `0803020531_18v4 게임 그래픽 프로그래밍` 주요 학습내용을 Unity 6 학습 문서에 배치한 운영표입니다. 수업은 하루 6시간, 최대 4교시로 운영하며 1교시는 90분을 기준으로 합니다.

## 교수계획서 반영표

| 교수계획서 주요 학습내용 | 반영 문서 | 수행 산출물 또는 확인 방법 |
| :--- | :--- | :--- |
| 정점 셰이더로 변환 처리 | DAY 06, DAY 07 | Vertex Position 변형 또는 정점 단계 코드 설명 |
| 픽셀 셰이더로 렌더링 처리 | DAY 04, DAY 07 | Shader Graph 표면 출력 또는 Fragment/Pixel 단계 설명 |
| 조명 모델로 사실적인 음영 구현 | DAY 02, DAY 03 | PBR 머티리얼과 조명 비교 씬 |
| 물리 기반 렌더링으로 영상 품질 향상 | DAY 02, DAY 05 | Metallic, Smoothness, Normal, Emission 비교 |
| 3D 그래픽 요소와 공간 배치 분석 | DAY 09, DAY 14 | 이펙트 위치·방향·크기·카메라 거리 분석표 |
| 분석한 그래픽 요소의 3D 공간 구현 | DAY 09~DAY 13 | Particle System 또는 VFX Graph 이펙트와 Prefab |
| 플레이 규칙에 맞춘 그래픽 요소 통합 | DAY 11, DAY 14 | 게임 사건 기반 이펙트 재생·중복 방지·통합 테스트 |

## 일자별 4교시 운영

| 일자 | 1교시 | 2교시 | 3교시 | 4교시 |
| :--- | :--- | :--- | :--- | :--- |
| DAY 01 | 렌더링 흐름 | URP와 Camera | Light·Mesh Renderer | 그래픽 실험 씬 |
| DAY 02 | 텍스처·머티리얼 | Metallic·Smoothness | Normal·Emission | PBR 비교 실습 |
| DAY 03 | 빛과 그림자 | 카메라 | Volume·색 보정 | 조명 비교 씬 |
| DAY 04 | 셰이더 실행 구조와 Shader Graph | Vertex·Fragment 구분 | Property·Node와 Fragment 색 출력 연결 | 머티리얼 적용 |
| DAY 05 | 표면 표현 분석 | 보호막 또는 용암 | Noise·Fresnel | 그래프 디버깅 |
| DAY 06 | 정점과 UV | Vertex Position | UV Animation | 변형 테스트 |
| DAY 07 | Shader Graph와 코드 | Vertex 단계 | Fragment/Pixel 단계 | 코드 읽기·테스트 |
| DAY 08 | Toon·Rim·Outline | 후처리 표현 | 스타일 비교 | 캐릭터 강조 적용 |
| DAY 09 | Particle 개념 | 히트 이펙트 제작 | 3D 공간 배치 분석 | Play Mode 확인 |
| DAY 10 | Particle Module | 이펙트 3종 | Prefab화 | 재사용 점검 |
| DAY 11 | 게임 사건 분석 | 코드 연동 | 발생 위치 결정 | 재생·정리 테스트 |
| DAY 12 | VFX Graph 역할 | Spawn·Initialize | Update·Output | GPU Spark 구현 |
| DAY 13 | 노출 프로퍼티 | 코드 제어 | 성능 값 조절 | 디버깅 점검 |
| DAY 14 | 최종 씬 조립 | 플레이 규칙 통합 | 통합 테스트 | 포트폴리오 발표 정리 |

## 문서 분할 기준

현재 DAY 문서는 각각 하나의 실습 목표를 다루며 하루 4교시 안에 사용할 수 있는 분량입니다. 한 DAY 문서가 두 개 이상의 독립 그래픽 실습을 포함하게 되면 `DAY##_A_주제.md`, `DAY##_B_주제.md`로 나누고, 같은 날의 교시는 합계 4개를 넘기지 않습니다.
