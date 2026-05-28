# DAY 09: Package Manager와 플러그인 확장

오늘의 목표는 Unity 패키지를 "**필요할 때 가져오는 공구 세트**"로 이해하고, Package Manager를 통해 기능을 추가하는 흐름을 익히는 것입니다.

## 1. 핵심 개념: "엔진에 공구 추가하기"

Unity 기본 기능만으로도 많은 것을 만들 수 있지만, 입력, 카메라, XR, Addressables처럼 별도 패키지로 제공되는 기능도 많습니다. Package Manager는 이런 공구 세트를 설치하고 버전을 관리하는 창입니다.

### 이 단어는 무슨 뜻인가요?

- **Package**: Unity 기능을 묶어 배포하는 단위입니다.
- **Package Manager**: 패키지를 설치, 제거, 업데이트하는 Unity 창입니다.
- **Registry**: 패키지를 내려받는 저장소입니다.
- **Plugin**: 엔진에 추가 기능을 붙이는 외부 모듈입니다.

## 실습 예제: Input System 패키지 확인하기

**미션:** Package Manager에서 Input System 패키지 설치 여부를 확인하고, 간단한 안내 스크립트를 작성합니다.

1. `Window > Package Manager`를 엽니다.
2. `Unity Registry`에서 `Input System`을 검색합니다.
3. 설치되어 있지 않다면 설치합니다.
4. 설치 후 Unity가 재시작을 요구하면 안내에 따라 프로젝트를 다시 엽니다.
5. `Edit > Project Settings > Player > Active Input Handling`에서 Input System 사용 상태를 확인합니다.

### 실행해보면

Package Manager 창에서 Input System 패키지의 설치 상태와 버전을 확인할 수 있습니다. 설치 후에는 `Player Input` 컴포넌트를 추가할 수 있는지도 함께 확인합니다.

### 생각해보기

1. 프로젝트마다 패키지 버전을 기록해야 하는 이유는 무엇일까요?
2. 외부 플러그인을 무작정 많이 설치하면 어떤 문제가 생길 수 있을까요?

## 오늘의 정리

- Package Manager는 Unity 기능 패키지를 관리하는 창입니다.
- 패키지는 프로젝트의 기능을 확장하지만 버전 관리가 필요합니다.
- 수업 프로젝트에서는 필요한 패키지만 설치하고 설치 이유를 기록합니다.
