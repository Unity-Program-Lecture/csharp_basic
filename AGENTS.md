# Codex Project Context: C# Basic Course (csharp_basic)

이 저장소는 SBS 게임 아카데미의 C# 기초 및 게임 프로그래밍 교육과정 개발, 강의 자료, 예제 코드를 관리합니다. 대상은 프로그래밍 입문자와 게임 개발 지망생이며, 설명은 비유 중심으로 쉽게 풀고 실습 가능한 코드 예제를 우선합니다.

## 프로젝트 개요

- 목적: C# 언어 기초부터 객체지향 프로그래밍(OOP), 자료구조, 유니티 프로그래밍 준비까지의 교육과정 수립 및 자료 아카이브.
- 주요 기술: C# (.NET SDK), Console Application.
- 대상: 프로그래밍 입문자 및 게임 개발 지망생.

## 디렉토리 구조

- `Code/`: 일자별(`DayXX`) 수업 예제 및 프로젝트 코드. 예: `Code/Day03/HuntMonster`.
- `Documents/`: 핵심 개념 정리, 일자별 강의 노트, 퀴즈 정답지 등 Markdown 문서.
- `과정개발 초안_평가도구초안개발본/`: 교육 과정 설계 및 평가 도구 PDF 문서.
- `게임계열_이수자평가문항 정리/`: NCS 기반 게임 기획/프로그래밍 평가 문항 모음.

## 주요 명령어

- 빌드 및 실행 예시: `dotnet run --project Code/Day03/HuntMonster/HuntMonster.csproj`
- 새 콘솔 프로젝트 생성: `dotnet new console -n ProjectName`
- 테스트 실행: 아직 별도 테스트 프로젝트 없음.

## 개발 및 문서 컨벤션

1. 새 Markdown 문서는 의미 있는 파일명을 사용하고, 파일명 뒤에 날짜나 시간 접미사를 붙이지 않습니다. 예: `DAY16_CS_ASYNC.md`
2. 강의 자료 및 기술 문서는 기본적으로 `Documents/` 디렉토리에 생성합니다.
3. Markdown에서 강조(`**`)와 큰따옴표(`"`)가 함께 쓰일 경우 강조 기호를 따옴표 안쪽에 배치합니다. 예: `"**강조문구**"`
4. 강조 기호가 소괄호 등 특수기호와 인접해 렌더링 오류가 나지 않도록 공백이나 배치를 조정합니다. 예: `**단어** (설명)`
5. 추상적 용어보다는 실생활 비유(상자, 리모컨 등)를 사용합니다.
6. 코드 설명은 코드 독해 3원칙인 `위->아래`, `오->왼`, `안->밖`을 기반으로 합니다.
7. 학생들이 직접 타이핑할 수 있는 예제 코드를 제공합니다.
8. 교육 과정의 상세 일정 및 일수 계산은 `Documents/COURSE_SCHEDULE_SUMMARY.md`를 최우선으로 참조합니다.
9. 정교한 기하학적 표현이나 겹침 등 시각적 설명이 필요한 경우 별도 `.svg` 파일로 저장하고 Markdown 이미지 태그로 삽입합니다. GitHub 호환성을 위해 표준 SVG 네임스페이스를 포함하고, 다크/라이트 테마 모두에서 보이는 중간 톤 이상의 고대비 색상을 사용합니다.

## 주요 참조 파일

- `Documents/HISTORY.md`: 전체 교육과정 개발 및 업데이트 이력.
- `Documents/LEARNING_DOCUMENT_GUIDE.md`: 과정별 학습 문서 작성 규칙.
- `Documents/QUIZ_ANSWERS_*.md`: 각 일자별 퀴즈 정답 및 해설.
- `Documents/COURSE_SCHEDULE_SUMMARY.md`: 전공 교과목별 상세 교육 일정 및 소요 일수 요약본.
- `26국기_교수계획서.pdf`: 공식 강의 계획 및 일정표.

## Codex 작업 메모

- 기존 Gemini CLI 설정은 `.gemini/settings.json`, `.geminiignore`에 남아 있습니다.
- Codex는 이 `AGENTS.md`를 우선 프로젝트 지침으로 사용합니다.
- Gemini 전용 개인 설정(`.gemini/settings.json`의 OAuth, 모델명, MCP 서버 등)은 Codex 설정으로 자동 변환하지 않습니다. 필요한 MCP나 도구 설정은 현재 Codex 환경에서 별도로 활성화합니다.
