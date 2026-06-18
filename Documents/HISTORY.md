# 📜 프로젝트 교육과정 개발 히스토리

## 2026-06-18

1. **게임엔진 과정에 Animation과 Animator 기초 차시 추가**
   - `DAY11_ANIMATION_ANIMATOR.md`를 추가하여 Animation Clip, Animator Controller, State, Transition, Parameter, Root Motion의 기초를 정리함.
   - CharacterController의 실제 이동 속도를 `Animator.SetFloat`로 전달하여 Idle과 Walk를 전환하는 실습을 추가함.
   - Animator Inspector 주요 프로퍼티와 Animation/Animator 창의 차이, 반복 Clip 설정, 자주 발생하는 연결 오류를 초보자 눈높이로 설명함.
2. **게임엔진 11~13일차 학습 흐름 재정렬**
   - 기존 Navigation 수업을 `DAY12_NAVIGATION_SYSTEM.md`로 이동하고 문서 제목의 일차를 갱신함.
   - Package Manager와 외부 라이브러리 수업을 `DAY13_PACKAGE_EXTERNAL_ASSET.md`에 통합하여 전체 18일 구성을 유지함.
   - 게임엔진 포트폴리오 과제, 통합 포트폴리오 가이드, 최종 점검표에 Animation과 Animator 활용 항목을 반영함.

## 2026-06-15

1. **게임엔진 10일차 외부 에셋 관리 기준 정정**
   - `DAY10_EXTERNAL_LIBRARY.md`에서 Asset Store 에셋을 무조건 `ThirdParty`로 옮기도록 보일 수 있는 설명을 수정함.
   - 외부 에셋 원본은 제작자 기본 경로를 유지하고, 직접 만든 씬/스크립트/Prefab Variant는 `_Project`에 분리하는 방식으로 안내를 정리함.
   - `.meta` 파일과 업데이트 시 중복 폴더, 참조 꼬임 위험을 초보자 눈높이로 보강함.
2. **게임엔진 11일차 UGUI 기초 설명 보강**
   - `DAY11_UIUX_ENGINE.md`에 Canvas Render Mode, RectTransform의 Anchor/Pivot/Pos 관계, Canvas Scaler, EventSystem과 Raycast Target 설명을 추가함.
   - 점수 버튼 실습 절차에 Canvas Scaler 설정과 Anchor/Pivot 배치 확인 과정을 보강함.
3. **게임엔진 12일차 사운드/VFX 기초 설명 보강**
   - `DAY12_AUDIO_VFX.md`에 Audio Listener의 역할, 중복 Listener 주의사항, AudioSource 주요 설정을 추가함.
   - ParticleSystem의 Main, Emission, Shape, Color over Lifetime, Size over Lifetime, Renderer 핵심 모듈 설명을 보강함.
   - 실습 절차에 Audio Listener 확인, Spatial Blend, ParticleSystem Looping/Duration/Emission 조정 단계를 추가함.
4. **게임엔진 13~14일차 VR 패키지 준비 절차 보강**
   - `DAY13_VR_FUNDAMENTALS.md`에 Package Manager로 설치할 XR 필수 패키지 목록과 OpenXR 활성화, XR Interaction Toolkit 샘플 Import 안내를 추가함.
   - `DAY14_VR_INTERACTION.md`에 VR 그랩 실습 전 XR Interaction Toolkit, Input System, OpenXR Plugin, Starter Assets 확인 절차를 추가함.

## 2026-06-01

1. **알고리즘 11~14일차 학습 흐름 재정렬 및 규격화**
   - `DAY11_ALGO_FSM.md`와 `DAY12_ALGO_GRAPH.md`의 학습 순서를 바꾸어 FSM을 먼저 배우고, 이후 그래프, BFS/DFS, A*로 이어지도록 재구성함.
   - `DAY11_ALGO_FSM.md`를 표준 학습 문서 구조로 재작성하고, 몬스터 상태 전환을 `OnDrawGizmos` 색상과 감지 반경으로 확인하는 예제로 교체함.
   - `DAY12_ALGO_GRAPH.md`를 노드와 간선 개념 중심으로 재작성하고, 그래프 연결 구조를 Scene 뷰 Gizmos로 확인하는 예제로 교체함.
   - `DAY13_ALGO_SEARCH.md`의 중복 본문, 공개 정답 노출, `Debug.Log` 중심 슈도 코드를 제거하고, BFS 방문 순서를 Gizmos 그리드로 확인하는 예제로 재작성함.
   - `DAY14_ALGO_ASTAR.md`의 Mermaid 도식, 운영 비율 표현, 고급 구현 노트형 구성을 제거하고, A*의 `F = G + H`, Open/Closed Set, 안전 반복 제한을 작은 그리드 Gizmos 예제로 설명하도록 재작성함.

## 2026-05-29

1. **알고리즘 5일차 게임 물리 설명 난이도 조정**
   - `Documents/Curriculum/02.Algorithm/DAY05_PHYSICS_MOTION.md`에서 미분/적분 중심 표현을 프레임별 변화량 누적 설명으로 바꿈.
   - 등가속도 운동의 이동 거리 공식을 평균 속도 관점으로 풀어 설명하고, Rigidbody 없는 중력 예제를 실전 물리 대체가 아닌 관찰 실험으로 명확히 정리함.

## 2026-05-26

1. **GitHub Desktop 참고 문서 추가**
   - `Documents/Reference/GITHUB_DESKTOP_GUIDE.md`를 생성하여 설치, 로그인, 저장소 생성, Clone, Commit, Push, Pull, Branch, Conflict 대응, C# 프로젝트 `.gitignore` 주의사항을 입문자용 비유 중심으로 정리함.
   - `Documents/Reference/` 참고 문서 폴더를 신설하여 공통 도구 사용 가이드를 분리 보관할 수 있도록 구성함.
2. **Unity 프로젝트 저장소 추가 주의사항 보강**
   - GitHub Desktop 가이드에 Unity 프로젝트에서 Git에 포함할 항목(`Assets`, `Packages`, `ProjectSettings`)과 제외할 항목(`Library`, `Temp`, `Obj`, `Build`, `Logs`)을 추가함.
   - Unity용 `.gitignore`, `.meta` 파일 관리, 씬/프리팹 충돌, 대용량 에셋 주의사항을 입문자용 비유와 체크리스트로 정리함.
3. **Unity 대용량 에셋 Git LFS 설정 설명 추가**
   - GitHub Desktop 설치 시 Git LFS 도구는 함께 설치되지만, 파일 추적 규칙은 자동 설정되지 않으므로 `git lfs track`과 `.gitattributes` 커밋이 필요하다는 내용을 보강함.
   - `.psd`, `.fbx`, `.wav`, `.mp4`, `.zip` 등 Unity 대용량 에셋 예시와 `.gitattributes` 작성 예시를 추가함.
4. **GitHub Desktop과 명령줄 Git 설치 관계 설명 추가**
   - GitHub Desktop만 사용하는 경우 기본 Git 작업은 가능하지만, 터미널의 `git` 명령어, 외부 도구 연동, `git lfs track` 사용을 위해서는 Git for Windows 설치 여부를 확인하도록 안내함.
   - `git --version` 확인 명령과 Git for Windows 다운로드 경로를 가이드에 추가함.
5. **Unity Git 설정 템플릿 다운로드 링크 추가**
   - GitHub 공식 Unity `.gitignore` 템플릿의 보기 링크와 Raw 다운로드 링크를 GitHub Desktop 가이드에 추가함.
   - Unity `.gitattributes` 템플릿의 보기 링크와 Raw 다운로드 링크를 Git LFS 설명 섹션에 추가함.
6. **C# 기초 재시험 대비 학습 요약 추가**
   - `Documents/Exam/01.CSharpBasic/RETAKE_EXAM_CSHARP_STUDY_GUIDE.md`를 생성하여 재시험 문항을 직접 노출하지 않고 핵심 개념, 코드 독해법, 실습형 대비 방법을 정리함.
   - 비공개 평가자료와 연결되는 보충 문서이므로 `Documents/Exam/01.CSharpBasic/` 내부에 보관함.

## 📅 2026-04-12 (어제)
- 1일차 수업 진행: 변수, 데이터 타입, 값/참조 형식, 함수 기초, if, for 학습.
- 초보 학생들이 메모리 구조와 박싱/언박싱 개념을 어려워함 확인.

## 📅 2026-04-13 (오늘 - 현재 세션)
### ✅ 완료된 작업
1. **1일차 복습 노트 (`DAY01_RECAP_202604131800.md`)**
   - 비유 중심의 개념 정리 (상자, 리모컨, 자판기).
   - 코드 독해 3원칙(위->아래, 오->왼, 안->밖) 추가.
2. **2일차 강의 자료 (`DAY02_CS_CONTROL_FLOW_202604131800.md`)**
   - 기본 연산자(산술, 비교, 논리) 섹션 추가.
   - 제어문(break, continue, switch, while, do-while) 및 입출력(Parse) 학습.
3. **3일차 강의 자료 (`DAY03_CS_METHOD_CLASS_202604131800.md`)**
   - 메소드(기능 분리)와 클래스(설계도와 실체) 개념 정립.
   - 접근 제한자(public/private) 비유 설명.
4. **4일차 강의 자료 (`DAY04_CS_INHERITANCE_202604131800.md`)**
   - 생성자(Constructor)와 오버로딩 개념.
   - 상속(Inheritance) 및 가상 메소드(virtual/override) 활용.
5. **5일차 강의 자료 (`DAY05_CS_ARRAY_MISSION_202604131800.md`)**
   - 배열(Array)과 foreach문.
   - Random 클래스를 활용한 게임 로직 구현.
6. **6~10일차 강의 자료 (2주차 집중 과정) 완료**
   - **6일차**: 인터페이스(Interface)의 정의와 다중 구현.
   - **7일차**: 컬렉션(List, Dictionary)의 기초와 제네릭 맛보기.
   - **8일차**: 제네릭(Generics) 메소드 및 클래스 설계.
   - **9일차**: Stack과 Queue의 특징 및 인벤토리 검색 로직.
   - **10일차**: 2주차 최종 미션 - RPG 인벤토리 시스템 완성.
7. **퀴즈 및 정답 시스템 구축**
   - 1~11일차 모든 강의 자료에 핵심 퀴즈 추가.
   - `QUIZ_ANSWERS_202604131800.md` 정답지 별도 생성.
8. **11일차 강의 자료 (`DAY11_CS_EXCEPTION_202604131800.md`)**
   - 예외 처리(try-catch-finally) 및 에러 방어 전략.

### 🚀 향후 로드맵 (4주 집중 과정)
- **1주차**: 클래스 심화, 상속, 생성자, 배열.
- **2주차**: 인터페이스, 제네릭, 컬렉션(List, Dictionary).
- **3주차**: 예외 처리, 대리자(Delegate), 이벤트, 람다.
- **4주차**: LINQ, 비동기(Task), GC 원리, 유니티 연결(Attribute).

### 📌 메모 및 주의사항
- 모든 예제는 학생들이 직접 타이핑(Typing) 하도록 유도할 것.
- 추상적인 용어 정의보다는 실생활 비유와 로직 해석(번역)에 집중할 것.
- 패턴 매칭 등 고급 문법은 초반에 생략하여 인지 부하를 줄임.

## 📅 2026-05-26 (알고리즘 과정 개발)
### ✅ 완료된 작업
1. **Course 01 (Algorithm) 기초 4회차 강의 자료 생성**
   - **1일차**: 게임 수학 - 벡터(Vector)의 개념 및 유니티 이동 로직.
   - **2일차**: 시간 복잡도(Big-O)와 유니티 프로파일러 활용법.
   - **3일차**: 정렬(Sorting) 알고리즘과 인벤토리 시스템 적용.
   - **4일차**: 재귀(Recursion) 호출과 하이어라키 탐색 알고리즘.
2. **NCS 매핑 가이드 적용**
   - 각 주제별로 NCS 알고리즘 요소를 유니티 게임 개발 실무와 연결하여 설명.
   - 실습 코드를 유니티 C# 스크립트 형식으로 제공.

## 📅 2026-06-01 ~ 06-05 (알고리즘 과정 개발 심화)
### ✅ 완료된 작업
1. **Course 01 (Algorithm) 심화 4회차 강의 자료 생성 (05~08회차)**
   - **5일차**: 스택과 큐 - UI 뒤로가기 및 알림 메시지 시스템 구현.
   - **6일차**: 연결 리스트 - 기차 구조 비유와 동적 버프/디버프 체인 관리.
   - **7일차**: 트리 - 계층 구조 이해 및 RPG 스킬 트리 로직 설계.
   - **8일차**: 그래프와 BFS - 타일 맵에서의 이동 범위 탐색 알고리즘 구현.
2. **Unity3D 실무 연계 강화**
   - 모든 실습 코드를 유니티 MonoBehaviour 기반의 실무 스니펫으로 제공.
   - 2026년 수업 일정에 맞춰 파일명 및 날짜 정규화.

## 📅 2026-06-09 ~ 06-12 (알고리즘 과정 개발: 길찾기 및 AI)
### ✅ 완료된 작업
1. **Course 01 (Algorithm) 고급 길찾기 및 AI 4회차 강의 자료 생성 (09~12회차)**
   - **9일차**: 다익스트라 알고리즘 - 가중치 그래프(산, 늪)에서의 최단 경로 탐색.
   - **10일차**: A* 알고리즘 기초 - 휴리스틱(H) 점수와 F=G+H 개념 정립.
   - **11일차**: A* 알고리즘 실전 - 유니티 Open/Closed List 기반 길찾기 루프 구현.
   - **12일차**: FSM(유한 상태 머신) - 몬스터 추적 및 공격 AI 상태 전이 설계.
2. **NCS 알고리즘 수행 준거 반영**
   - 가중치 그래프, 휴리스틱 탐색, 상태 머신 제어 등 NCS 주요 키워드를 강의 내용에 통합.
   - 실무 최적화 관점에서 알고리즘의 장단점 비교 퀴즈 추가.

## 📅 2026-06-15 ~ 06-19 (알고리즘 과정 개발: AI 심화 및 최적화 마스터)
### ✅ 완료된 작업
1. **Course 01 (Algorithm) 최종 5회차 강의 자료 생성 (13~17회차)**
   - **13일차**: Behavior Tree(BT) - Selector/Sequence 노드를 활용한 복합 AI 설계.
   - **14일차**: 공간 분할(Grid) - 대규모 객체 탐색 최적화를 위한 공간 분할 원리 및 구현.
   - **15일차**: 오브젝트 풀링 - 메모리 관리(GC 부하 감소)를 위한 객체 재사용 시스템 구축.
   - **16일차**: 절차적 생성 - Perlin Noise를 활용한 자연스러운 지형 데이터 생성 알고리즘.
   - **17일차**: 최종 시험(Final Exam) - 전체 과정 총정리 및 복합 미션 설계(미니 던전 생존).
2. **실무 최적화 및 시스템 설계 역량 강화**
   - 단순 구현을 넘어 '왜 이 알고리즘을 써야 하는가'에 대한 설계적 관점 제시.
   - 유니티 엔진의 성능적 특성(프레임 드랍, 가비지 컬렉션)과 알고리즘을 밀접하게 연결.

## 📅 2026-04-21 (전략)
... (기존 내용)

## 📅 2026-04-27 (오늘 - 현재 세션)
### ✅ 완료된 작업
1. **커리큘럼 14~17일차 전면 개편**
   - **14일차**: 열거형(Enum) + 구조체(Struct) 통합. 값 형식(Value Type)의 특징 집중 학습.
   - **15일차**: 파일 입출력(File I/O). 데이터 영속성 학습.
   - **16일차**: **(신설)** 시니어 보강 세션. 프로퍼티(Property), 2차원 배열, StringBuilder 등 누락된 핵심 실무 문법 보강.
   - **17일차**: 최종 랩업 및 비동기(Async/Await). 3주 대장정 마무리.
2. **신규 문서 생성 및 최적화**
   - `DAY14_CS_ENUM_STRUCT_202604271400.md`
   - `DAY16_CS_PROPERTIES_ARRAY2D_202604271400.md`
   - `DAY17_CS_FINAL_WRAPUP_202604271400.md` (순연)
3. **학습 맥락 강화**
   - 8일차 제네릭에서 누락되었던 `where` 제약 조건을 14일차 값/참조 형식과 연결하여 자연스럽게 노출.
   - 2차원 배열 학습을 통해 향후 알고리즘 실습(맵 데이터)을 위한 초석 마련.

### 🚀 향후 로드맵 업데이트
- **3주차**: 예외 처리, 대리자, 이벤트, 람다, **구조체(추가)**, 파일 입출력.
- **4주차**: LINQ, 비동기(Task), 최종 프로젝트 및 랩업.

## 📅 2026-05-12
### ✅ 완료된 작업
1. **커리큘럼 전체 예제 코드 유니티 스타일 개편 (Day 01~14)**
   - 모든 강의 자료의 예제 코드를 `UnityEngine.MonoBehaviour` 및 `Debug.Log` 스타일로 전면 교체.
2. **커리큘럼 구조 및 순서 재편성**
   - **1일차**: 변수/메모리 + 함수 통합.
   - **2일차**: 배열 + 제어 흐름 통합.
   - **3일차**: 클래스 + 프로퍼티.
   - **4일차**: 구조체(Struct) 단독 섹션.
   - **5일차**: 상속(Inheritance).
   - **6일차**: 인터페이스(Interface). (7일차와 순서 교체)
   - **7일차**: 매개변수 한정자(ref, out). (6일차와 순서 교체)
   - **10일차**: 열거형(Enum) + 박싱/언박싱.
3. **저장소 정규화**
   - 모든 강의 자료 파일명 정규화 및 타임스탬프 갱신.
   - 구 버전 문서 전면 삭제.

## 📅 2026-05-19 (오늘 - 현재 세션)
### ✅ 완료된 작업
1. **강의 자료 내용 보완 (`DAY09_CS_COLLECTIONS.md`)**
   - 각 컬렉션(List, Dictionary, Stack, Queue)의 필수 메소드 및 프로퍼티 상세 설명 추가.
   - `Insert`, `RemoveAt`, `TryGetValue`, `Peek`, `Count` 등 실무 핵심 기능을 초보자용 비유와 함께 업데이트.
   - 모든 설명된 메소드와 프로퍼티가 포함되도록 **샘플 코드 전면 개선** (실용적인 활용 사례 중심).

## 📅 2026-05-20 (오늘 - 현재 세션)
### ✅ 완료된 작업
1. **제미나이CLI 프로젝트 마이그레이션 완료**
   - 기존 제미나이CLI 기반의 설정을 Antigravity 에이전트 환경으로 안전하게 전환.
   - `.gemini/settings.json`에서 더 이상 사용되지 않는 `context-mode` MCP 서버 및 훅(`BeforeTool`, `AfterTool`, `PreCompress`, `SessionStart`)을 완전히 정제하여 성능 최적화.
2. **기존 프로젝트 메모리 보존**
   - 기존에 작성된 강의 계획 및 핵심 컨텍스트(`GEMINI.md`)와 강의 개발 히스토리(`Documents/HISTORY.md`)를 누락 없이 온전히 보존.
   - 향후 새로운 아티팩트 관리 방식을 적용하기 위한 기본 환경 설정 마련.
3. **3일차 알고리즘 교재 보강 설명 추가 (DAY03_MATH_MATRIX.md)**
   - 별첨 1의 1단계 뷰 변환(View Transform) 부분에 수학적 포뮬러 외에 학생들이 직관적으로 배울 수 있도록 상대성 원리와 역행렬을 이용한 "**우주 전체를 움직이는 마술**" 비유 설명을 요약하여 보강함.
   - 2단계 투영 변환(Projection Transform) 설명에 정규화된 장치 좌표계인 "**NDC** (Normalized Device Coordinates)"의 학술적/기술적 정의 및 하드웨어 독립성(Device Independence)의 중요성을 담은 상세 설명을 추가함.
   - 투영 변환 부분의 "**원근감의 비밀**" 설명에 멀리 있는 사물의 x, y 좌표가 중심 쪽으로 좁게 조여지는 기하학적 원리를 철길 비유 및 피라미드 압축 원리로 상세하게 해설하여 보강함.
   - 별첨 2 직교 투영 (Orthographic Projection) 의 주요 특징 섹션에 원근 투영과의 핵심 기하학적 차이점인 "**x, y 좌표 축소(왜곡) 생략**" 개념을 명확하게 정립하여 보강함.
   - 3단계 투영 분할 (Perspective Division) 섹션에 사용자의 뛰어난 기하학적 직관인 "**투영 변환(위치 조정) ➡️ 투영 분할(크기 조정)**" 및 "**직교 투영에서의 전체 생략**" 원리를 1분 직관 요약 꿀팁으로 최종 등재함.
4. **4일차 알고리즘 교재 회전 시각화 다이어그램 및 비유 설명 보강 (DAY04_MATH_QUATERNION.md)**
   - 오일러 회전의 세 축 짐벌락(Gimbal Lock) 정렬 한계와 쿼터니언의 단일 임의 회전 축(u) 및 회전각(θ)을 세련되게 대조 묘사하는 고품격 비교 SVG 다이어그램인 `rotation_euler_vs_quaternion.svg`를 디자인하여 교재 1절 하단에 연동 및 저장함.
   - 다이어그램 하단에 스마트폰 거치대와 둥근 농구공(트랙볼)의 실생활 비유를 적용하고, 실전 FPS 게임에서 짐벌락을 막기 위해 카메라 상하 각도를 "**89도 ~ 89.9도**" 로 한계 조절 (Clamp) 하는 정교한 꼼수/실무 사례를 텍스트로 보강하여 최고의 교육적 직관성을 확보함.
   - Slerp (구면 선형 보간) 섹션에 직선형 보간 (Lerp) 과 구면형 보간 (Slerp) 의 기하학적 차이점을 "**지구 내부 최단 직선 터널**" vs "**지구본 표면 비행기 항로**" 비유로 알기 쉽게 해설하여 보강함.
5. **4일차 알고리즘 교재 구성 및 보간 흐름 논리적 최적화 (DAY04_MATH_QUATERNION.md)**
   - 오일러/쿼터니언 차이의 실생활 비유 및 FPS 실전 팁을 쿼터니언 본문 설명 직후로 자연스럽게 배치하고, 그 흐름이 부드럽게 이어지도록 Lerp/Slerp 보간 개념 비교 단락을 그 뒤에 재배치하여 학습자의 논리적 이해도를 최적화함.
6. **5일차 게임 물리 교재 등속 직선 운동 기초 단락 보강 (DAY05_PHYSICS_MOTION.md)**
   - 등가속도 운동을 학습하기 전에 등속 직선 운동의 기본적인 정의, 변위 공식, 게임 내 활용 예시(캐릭터 이동, 기본 투사체) 및 C# 예제 코드를 선제적으로 추가하여 기초적인 물리 빌드업을 촘촘히 보강함.
## 2026-05-24

1. **알고리즘 5~15일차 학습 문서 중복 내용 정리**
   - `DAY06_PHYSICS_FORCE.md`에서 Day 05와 중복되는 등가속도 공식 설명을 제거하고, 퀴즈를 힘과 질량 관계 중심으로 정리함.
   - `DAY07_PHYSICS_COLLISION.md`에서 Day 08의 운동량/충격량 학습 내용과 겹치는 반발 계수 심화 별첨을 제거함.
   - `DAY15_ALGO_ASTAR.md`의 FSM 연동 문구를 Day 13 개념 반복이 아닌 A* 경로 갱신 설계 중심으로 조정함.
2. **8일차 ForceMode 힘 전달 방식 설명 보강**
   - `DAY08_PHYSICS_ADVANCED.md`에 `Rigidbody.AddForce(direction, ForceMode.Impulse)`와 함께 `Force`, `Acceleration`, `Impulse`, `VelocityChange`의 차이를 질량 영향, 호출 패턴, 사용 예시 중심으로 추가함.
3. **알고리즘 1~15일차 샘플 코드 Unity API 주석 보강**
   - 1~15일차 샘플 코드에서 UnityEngine/Unity Input System 메서드와 프로퍼티가 처음 등장하는 지점에만 초보자용 설명 주석을 추가함.
   - 앞 일차에서 이미 설명된 API는 뒤 일차 코드에서 반복 주석을 달지 않도록 정리함.
4. **7일차 Unity 물리 이벤트 Rigidbody 필수 조건 정정**
   - `DAY07_PHYSICS_COLLISION.md`의 Collision/Trigger 이벤트 표에서 중복 조건을 제거하고, 두 경우 모두 두 Collider 중 적어도 한쪽에 `Rigidbody`가 필요하다는 내용을 공통 조건으로 분리함.
5. **9일차 기본 포물선 운동의 3D 수평 이동 설명 보강**
   - `DAY09_PHYSICS_PROJECTILE.md`의 기본 포물선 운동 공식에 `z(t)`를 추가하고, 유니티 3D의 수평 이동이 XZ 평면에서 이루어진다는 설명을 보강함.
6. **9일차 중력 가속도 부호 표기 명확화**
   - `DAY09_PHYSICS_PROJECTILE.md`에서 중력 벡터를 $\vec{a} = (0, -g, 0)$로 표기하고, $g$는 양수인 중력 크기이며 실제 Y축 가속도는 $a_y = -g$라는 설명을 추가함.
7. **알고리즘 10~14일차 학습 문서 재편성**
   - `DAY11_DATASTRUCTURE_DICT.md`의 Dictionary 검색 내용을 `DAY10_DATASTRUCTURE_BASIC.md`에 통합하고 기존 11일차 문서를 삭제함.
   - 기존 `DAY12_ALGO_GRAPH.md`~`DAY15_ALGO_ASTAR.md`를 각각 `DAY11_ALGO_GRAPH.md`~`DAY14_ALGO_ASTAR.md`로 하루씩 당기고, 문서 제목과 A* 문서의 FSM 참조 일차를 함께 갱신함.

## 2026-05-21

1. **Codex 마이그레이션 및 학습 문서 작성 규칙 정리**
   - Gemini CLI의 프로젝트 지침을 Codex용 `AGENTS.md`로 이관함.
   - 과정별 학습 문서 작성 기준을 `Documents/LEARNING_DOCUMENT_GUIDE.md`로 문서화함.
   - 새 학습 문서 생성 시 파일명, 구조, 설명 방식, 실습 예제, 퀴즈 작성 기준을 우선 참조하도록 정리함.

## 2026-05-22

1. **Unity 6 기반 NCS 평가 산출물 작성 원칙 추가**
   - `Documents/LEARNING_DOCUMENT_GUIDE.md`에 NCS 교재의 평가 준거와 평가 방법을 존중하는 작성 원칙을 추가함.
   - 과정별 평가 산출물을 서술형 시험 10문제, 포트폴리오 과제, 평가자 체크리스트 확인 문제 1개로 구성하도록 정리함.
   - 평가 산출물 기본 폴더를 `Documents/Exam/과정폴더명/` 구조로 정의함.
   - Unity 6 학습/평가 문서에서 UI는 UGUI, 입력은 Input System을 기본으로 사용하도록 명시함.
   - 새 학습 문서는 `Documents/Curriculum/01.CSharpBasic/DAY01_CS_BASICS_METHODS.md`처럼 과정별 폴더 아래에 생성하도록 위치와 파일명 규칙을 변경함.
   - `Documents/Curriculum/02.Algorithm/`의 15개 학습 문서에 Unity 6/NCS 지침 보강 섹션을 추가함.
   - 게임 알고리즘 평가 산출물을 `Documents/Exam/02.Algorithm/` 아래의 서술형, 포트폴리오, 체크리스트 문서로 분리함.
    - 학생 공개용 학습문서에서 시험 관련 내부 경로와 평가 산출물 상세 안내를 제거하고, 비공개 교사용 운영 메모(`Documents/Exam/02.Algorithm/INSTRUCTOR_NOTES.md`)로 분리함.
    - `Documents/Exam`의 과정별 폴더명을 `Documents/Curriculum`의 과정 폴더명과 일치하도록 정리함.
    - `DAY02_MATH_DOTCROSS.md`에서 쿼터니언 학습 목표, Slerp 예제, 쿼터니언 퀴즈를 제거하고 내적/외적 기반 시야 및 좌우 판별 흐름으로 정리함.
    - `DAY02_MATH_DOTCROSS.md`의 내적/외적 기하학적 의미 설명을 보강하고, 그림자 투영 및 평행사변형 넓이 관점이 드러나도록 관련 SVG 2종을 개선함.
2. **3일차 행렬 교재 기하학적 의미 보강**
   - `DAY03_MATH_MATRIX.md`의 SRT 변환 순서 설명에 로컬 원점 기준 크기 조절, 제자리 회전, 월드 이동의 차이를 한눈에 보여주는 `day03_matrix_srt_transform.svg`를 추가함.
   - 별첨 1의 월드 좌표에서 화면 좌표까지의 변환 과정에 절두체, 클립 공간, NDC/화면 매핑의 기하학적 관계를 설명하는 `day03_projection_pipeline.svg`를 추가함.
   - 두 다이어그램 아래에 학생이 그림을 읽는 관점을 짚어주는 비유 중심 보충 설명을 추가함.

## 2026-05-26

1. **C# 기초 과정 재시험 평가 문서 추가**
   - `Documents/Exam/01.CSharpBasic/RETAKE_EXAM_CSHARP.md`를 추가함.
   - 1일차부터 9일차까지의 C# 기초 출제 범위를 기준으로 서술형 10문항을 구성함.
   - 본평가의 평가자 체크리스트 유형을 유지하되, 재시험용으로 클래스 수와 제네릭 제약 난도를 낮춘 미니 몬스터 관리 프로그램 과제를 추가함.

## 2026-05-27

1. **게임엔진 6일차 생명주기 그림 오류 수정**
   - `DAY06_UNITY_SCRIPTING.md`의 렌더링되지 않는 Mermaid `chronological-diagram` 블록을 제거함.
   - MonoBehaviour 생명주기 흐름을 별도 SVG 파일 `Images/day06_unity_lifecycle.svg`로 작성해 GitHub Markdown에서 안정적으로 표시되도록 교체함.
2. **게임엔진 학습 문서 제작 규격 정리**
   - `Documents/Curriculum/03.GameEngine/`의 1~15일차 문서를 `LEARNING_DOCUMENT_GUIDE.md`의 기본 흐름에 맞게 재구성함.
   - 공개 학습문서에서 내부 평가 운영성 문구와 Mermaid 도식 의존을 제거하고, 비유 중심 설명, 용어 풀이, 실습 예제, 실행 관찰, 생각해보기, 오늘의 정리 구조로 통일함.
   - Unity 6 기본값에 맞춰 입력 예제는 Input System 기준으로 정리함.
   - 규격을 맞추기 위해 억지로 넣은 단순 로그 출력형 샘플을 제거하거나 실제 동작을 만드는 실습으로 교체함.
3. **게임엔진 5일차 머티리얼 설명 보강**
   - 쉐이더와 머티리얼의 관계를 조리법과 재료표 비유로 설명함.
   - URP Lit 머티리얼의 Surface Options, Surface Inputs, Base Map, Metallic, Smoothness, Normal Map, Emission 등 기본 프로퍼티 설명과 비교 실습을 추가함.
