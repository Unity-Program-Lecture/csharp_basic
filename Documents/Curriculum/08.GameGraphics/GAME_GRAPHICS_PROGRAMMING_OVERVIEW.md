# 게임 그래픽 프로그래밍 과정 개요

- 교과목: 게임 그래픽 프로그래밍
- NCS 능력단위: `0803020531_18v4 게임 그래픽 프로그래밍`
- 기본 교재: `LM0803020530_18v4,+LM0803020531_18v4_게임+인공지능과+그래픽+프로그래밍.pdf`
- 수업 환경: Unity 6, URP, C#, Shader Graph, Particle System, Visual Effect Graph
- 배정 시간: 90시간

## 과정 목표

게임 그래픽 프로그래밍은 화면을 "**플레이어가 게임 상태를 느끼는 무대 조명**"처럼 다룹니다. 같은 캐릭터라도 머티리얼, 조명, 셰이더, 이펙트가 달라지면 전혀 다른 게임처럼 보입니다.

이 과정의 목표는 Unity 6에서 오브젝트가 어떻게 화면에 그려지는지 이해하고, Shader Graph와 간단한 셰이더 코드로 렌더링 표현을 바꾸며, Particle System과 Visual Effect Graph로 게임 이벤트에 맞는 이펙트를 구현하는 것입니다.

## NCS 연결

| 능력단위 요소 | NCS 학습 내용 | Unity 6 재구성 |
| :--- | :--- | :--- |
| `0803020531_18v4.1` 셰이더 프로그래밍하기 | 셰이더 알고리즘 이해 및 사용 | URP 렌더링 흐름, 머티리얼, Shader Graph, 정점/픽셀 처리, 조명, PBR, 간단한 HLSL 셰이더 |
| `0803020531_18v4.2` 이펙트 프로그래밍하기 | 게임 이펙트 구성 방법 이해 및 사용 | Particle System, Trail, Decal, Visual Effect Graph, 이벤트 연동, 성능 조절, 프리팹화 |

## 14일 학습 흐름

| 일차 | 주제 | 주요 산출물 |
| :--- | :--- | :--- |
| DAY 01 | Unity 6 렌더링 파이프라인과 URP 기초 | 그래픽 실험용 씬 |
| DAY 02 | 머티리얼, 텍스처, PBR 기초 | 금속/거칠기 비교 머티리얼 |
| DAY 03 | 조명, 그림자, 카메라와 색 보정 | 조명 비교 씬 |
| DAY 04 | 셰이더 실행 구조와 Shader Graph 기초 | Vertex·Fragment 구분과 색 변화 Shader Graph |
| DAY 05 | Shader Graph로 표면 표현 만들기 | 용암/물/보호막 중 1개 |
| DAY 06 | 정점 변형과 UV 애니메이션 | 흔들리는 풀 또는 흐르는 표면 |
| DAY 07 | 간단한 셰이더 코드와 렌더링 사고 | Unlit 또는 Toon 셰이더 |
| DAY 08 | 비실사 렌더링과 후처리 표현 | 외곽선/림라이트 표현 |
| DAY 09 | Particle System 기초 | 폭발 또는 히트 이펙트 |
| DAY 10 | Particle System 모듈과 프리팹화 | 재사용 가능한 이펙트 프리팹 |
| DAY 11 | 이펙트와 게임 코드 연동 | 입력/충돌 기반 이펙트 재생 |
| DAY 12 | Visual Effect Graph 입문 | GPU 파티클 이펙트 |
| DAY 13 | VFX Graph 고급 제어와 성능 | 노출 프로퍼티와 품질 옵션 |
| DAY 14 | 그래픽 포트폴리오 통합 | 셰이더 + 이펙트 통합 씬 |

## 수업 설정 확인 기준

이 과정의 학습 문서는 화면 이미지 대신 Inspector와 설정 창을 직접 읽도록 작성합니다. 각 DAY는 다음 네 가지를 기록하거나 설명할 수 있어야 합니다.

1. **대상**: 어떤 GameObject, Asset, Graph, Project Settings를 선택하는가?
2. **경로**: Inspector 또는 메뉴에서 어떤 항목을 여는가?
3. **값과 연결**: 어떤 프로퍼티·노드·참조를 바꾸고 어디에 연결하는가?
4. **확인 결과**: Play Mode 또는 Preview에서 무엇이 달라져야 하는가? 다르면 어떤 순서로 점검하는가?

따라서 제출과 수업 준비에는 이미지 파일이 아니라 Inspector 값표, Graph 연결 설명, Play Mode 확인 기록을 사용합니다.

## 공식 문서 참고

- Unity Manual: [Creating shaders with Shader Graph](https://docs.unity3d.com/6000.0/Documentation/Manual/shader-graph.html)
- Unity Manual: [Visual Effect Graph](https://docs.unity3d.com/6000.0/Documentation/Manual/VFXGraph.html)
