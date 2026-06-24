# 게임 그래픽 프로그래밍 포트폴리오 과제

- 교과목: 게임 그래픽 프로그래밍
- NCS 능력단위: `0803020531_18v4 게임 그래픽 프로그래밍`
- 평가 환경: Unity 6, URP, C#, Shader Graph, Particle System, Visual Effect Graph

## 과제 개요

Unity 6 프로젝트에서 작은 그래픽 쇼케이스 씬을 제작합니다. 목표는 완성된 게임 하나를 만드는 것이 아니라, 셰이더와 이펙트를 사용해 게임 장면의 시각 표현을 직접 설계하고 구현했다는 점을 확인하는 것입니다.

## 필수 구현 범위

1. 그래픽 콘셉트
   - 장면의 분위기와 참고 스타일을 3문장 이상으로 설명합니다.
   - 예: 마법 훈련장, 용암 던전, SF 실험실, 숲속 회복 지점

2. Shader Graph 머티리얼
   - Shader Graph 기반 머티리얼을 1개 이상 제작합니다.
   - Emission, Fresnel, Noise, UV Animation, Alpha 중 2개 이상을 사용합니다.

3. PBR 머티리얼 비교
   - Metallic, Smoothness, Normal, Emission 중 2개 이상을 조절한 머티리얼을 포함합니다.

4. Particle System 이펙트
   - Built-in Particle System 기반 이펙트 2개 이상을 제작합니다.
   - 최소 1개는 Prefab으로 만들어 코드에서 재생합니다.

5. Visual Effect Graph 이펙트
   - VFX Graph 기반 이펙트 1개 이상을 제작합니다.
   - Spawn, Initialize, Update, Output 흐름을 설명할 수 있어야 합니다.

6. 코드 연동
   - 입력, 충돌, 체력 변화, 공격 판정 중 1개 이상의 게임 사건에 이펙트를 연결합니다.

7. 스크린샷
   - 최종 씬, Shader Graph, Particle System Inspector, VFX Graph 화면을 각각 1장 이상 캡처합니다.

## 제출 산출물

- Unity 6 프로젝트 또는 프로젝트 폴더
- 구현 씬 1개 이상
- Shader Graph 에셋
- Particle System Prefab 2개 이상
- Visual Effect Graph 에셋 1개 이상
- 이펙트 연동 C# 스크립트
- 구현 설명서
- 실행 화면 캡처 4장 이상

## 제출 전 확인

| 확인 항목 | 완료 |
| :--- | :--- |
| Unity 6 URP 프로젝트에서 실행된다. |  |
| Shader Graph 머티리얼이 씬 오브젝트에 적용되어 있다. |  |
| 셰이더 노드의 핵심 흐름을 설명할 수 있다. |  |
| Particle System 이펙트가 2개 이상 있다. |  |
| 이펙트 Prefab이 코드에서 재생된다. |  |
| Visual Effect Graph 이펙트가 씬에서 재생된다. |  |
| VFX Graph의 주요 프로퍼티를 조절할 수 있다. |  |
| 스크린샷 4장 이상을 포함했다. |  |
| 성능을 위해 조절 가능한 값이 정리되어 있다. |  |

## 마무리 기준

좋은 그래픽 포트폴리오는 단순히 밝고 화려한 화면이 아닙니다. 어떤 분위기를 만들고 싶었는지, 그 분위기를 셰이더와 이펙트의 어떤 설정으로 구현했는지, 게임 사건과 어떻게 연결했는지 설명할 수 있어야 합니다.

