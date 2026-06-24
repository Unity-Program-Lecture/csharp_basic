# DAY 14: 그래픽 포트폴리오 통합

오늘의 목표는 지금까지 만든 셰이더와 이펙트를 하나의 작은 게임 장면에 묶어, NCS 기준의 셰이더 프로그래밍과 이펙트 프로그래밍 역량을 확인하는 것입니다.

## NCS 연결

- 능력단위 요소: 셰이더 프로그래밍하기, 이펙트 프로그래밍하기
- 관련 학습 내용: 셰이더 적용, 이펙트 작성, 코드 출력, 테스트와 디버깅
- Unity 6 재구성: Shader Graph, Particle System, Visual Effect Graph, C# 이벤트 연동을 통합합니다.

## 1. 최종 씬 요구 사항

| 구분 | 필수 구현 |
| :--- | :--- |
| 셰이더 | Shader Graph 기반 머티리얼 1개 이상 |
| 표면 표현 | Emission, Fresnel, UV Animation, Noise 중 2개 이상 |
| 이펙트 | Particle System 이펙트 2개 이상 |
| VFX Graph | Visual Effect Graph 이펙트 1개 이상 |
| 코드 연동 | 입력, 충돌, 상태 변화 중 1개 이상에 이펙트 연결 |
| 캡처 | 결과 화면 3장 이상 |

## 2. 권장 주제

| 주제 | 설명 |
| :--- | :--- |
| 마법 훈련장 | 보호막 셰이더, 마법탄 히트 이펙트, 에너지 VFX |
| 용암 던전 | 용암 Shader Graph, 불꽃 파티클, 열기 VFX |
| SF 실험실 | 홀로그램 셰이더, 스파크 파티클, 전기장 VFX |

## 3. 발표 구성

1. 어떤 화면 스타일을 목표로 했는지 설명합니다.
2. Shader Graph에서 사용한 주요 노드를 설명합니다.
3. Particle System 또는 VFX Graph의 Spawn, Lifetime, Color, Size 설정을 설명합니다.
4. 코드가 어떤 사건에서 이펙트를 재생하는지 설명합니다.
5. 성능을 위해 어떤 값을 조절할 수 있는지 설명합니다.

## 스크린샷 체크포인트

- `Images/portfolio_final_scene.png`: 최종 씬 전체 화면
- `Images/portfolio_shader_graph.png`: 주요 Shader Graph 화면
- `Images/portfolio_vfx_graph.png`: 주요 VFX Graph 화면
- `Images/portfolio_effect_play.png`: 게임 이벤트에 따라 이펙트가 재생되는 화면

## 오늘의 정리

- 그래픽 프로그래밍은 화면을 예쁘게 만드는 작업이면서 동시에 게임 상태를 읽히게 만드는 작업입니다.
- 셰이더는 표면의 규칙을 만들고, 이펙트는 사건의 순간을 강조합니다.
- 좋은 포트폴리오는 화려함보다 의도, 구현, 테스트 결과가 분명해야 합니다.

