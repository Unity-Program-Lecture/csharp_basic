# DAY 16: VR 기초와 XR Origin

오늘의 목표는 VR 환경을 "**플레이어의 머리와 손을 게임 세상에 연결하는 장치**"로 이해하고, OpenXR과 XR Origin의 기본 구조를 익히는 것입니다.

## 1. 핵심 개념: "현실 좌표를 게임 좌표로 옮기기"

VR은 플레이어의 머리와 손 위치를 계속 추적해서 Unity 씬 안의 카메라와 컨트롤러에 반영합니다. XR Origin은 현실의 기준점과 게임 월드의 기준점을 이어 주는 중심 오브젝트입니다. 카메라가 머리 역할을 하고, 왼손과 오른손 컨트롤러가 손 역할을 합니다.

### 이 단어는 무슨 뜻인가요?

- **Package Manager**: Unity 기능 패키지를 설치하고 버전을 확인하는 창입니다.
- **OpenXR**: 여러 VR 기기를 공통 방식으로 다루기 위한 표준입니다.
- **XR Origin**: VR 플레이어의 기준 위치를 담당하는 루트 오브젝트입니다.
- **HMD**: 머리에 쓰는 VR 디스플레이 장치입니다.
- **Controller**: VR에서 손 입력과 위치를 담당하는 장치입니다.
- **Teleportation**: VR 멀미를 줄이기 위해 목표 지점으로 순간 이동하는 방식입니다.

## 2. VR 실습 전에 설치할 패키지

VR 실습은 Unity 기본 프로젝트만으로 바로 진행하기 어렵습니다. 9일차에서 배운 `Window > Package Manager`를 열고, 수업 프로젝트에 필요한 XR 패키지를 먼저 설치해야 합니다. 패키지를 설치하면 `Packages/manifest.json`에 기록되므로, 프로젝트를 공유할 때도 어떤 기능을 사용했는지 추적할 수 있습니다.

| Package Manager 표시 이름 | 패키지 ID | 필요한 이유 |
| :--- | :--- | :--- |
| `XR Plugin Management` | `com.unity.xr.management` | 프로젝트에서 어떤 XR 플러그인을 사용할지 관리합니다. |
| `OpenXR Plugin` | `com.unity.xr.openxr` | Meta Quest, SteamVR 등 여러 XR 기기를 OpenXR 방식으로 연결합니다. |
| `XR Interaction Toolkit` | `com.unity.xr.interaction.toolkit` | XR Origin, 컨트롤러, 텔레포트, 그랩 상호작용을 구성합니다. |
| `Input System` | `com.unity.inputsystem` | VR 컨트롤러 입력을 액션 단위로 처리합니다. |

패키지를 설치한 뒤에는 `Project Settings > XR Plug-in Management`에서 대상 플랫폼의 `OpenXR`을 켭니다. PC VR로 테스트한다면 Standalone 설정을 확인하고, Quest 같은 기기로 빌드한다면 Android 설정도 확인합니다.

`XR Interaction Toolkit`은 설치만으로 끝나지 않을 수 있습니다. Package Manager의 해당 패키지 상세 화면에서 `Samples`를 열고, 수업에 필요한 `Starter Assets` 또는 XR Origin 관련 샘플을 Import해야 입력 액션과 기본 프리셋을 더 쉽게 사용할 수 있습니다.

## 3. XR Origin 기본 구조

```text
XR Origin
  Camera Offset
    Main Camera
    Left Controller
    Right Controller
```

`XR Origin`을 움직이면 플레이어 전체 기준점이 이동합니다. `Main Camera`는 HMD 움직임을 따라가고, 컨트롤러 오브젝트는 손의 위치와 회전을 따라갑니다.

## 실습 예제: 텔레포트 위치 표시하기

**미션:** XR Origin 구조를 만들고, 텔레포트 가능한 바닥과 불가능한 바닥을 구분합니다.

1. Package Manager에서 `XR Plugin Management`, `OpenXR Plugin`, `XR Interaction Toolkit`, `Input System`이 설치되어 있는지 확인합니다.
2. `Project Settings > XR Plug-in Management`에서 `OpenXR`을 켭니다.
3. XR Interaction Toolkit의 `Starter Assets` 또는 XR Origin 관련 샘플을 Import합니다.
4. 씬에 `XR Origin`을 배치하고 `Main Camera`, `Left Controller`, `Right Controller` 구조를 확인합니다.
5. 바닥 오브젝트를 만들고 텔레포트 가능한 영역으로 사용할 레이어를 지정합니다.
6. 텔레포트가 되면 안 되는 장애물에는 다른 레이어를 지정합니다.

### 실행해보면

Play를 누르면 XR Origin이 플레이어 기준점 역할을 합니다. 텔레포트 가능한 바닥과 불가능한 오브젝트를 나누면 플레이어가 이동할 수 있는 공간을 더 명확하게 설계할 수 있습니다. XR Origin이나 입력 액션이 보이지 않으면 XR Interaction Toolkit 샘플이 Import되어 있는지도 다시 확인합니다.

### 생각해보기

1. VR에서 플레이어를 계속 빠르게 밀면 멀미가 날 수 있는 이유는 무엇일까요?
2. 텔레포트 가능한 바닥과 불가능한 바닥은 어떻게 구분하면 좋을까요?
3. XR 실습에서 패키지 설치 기록이 `manifest.json`에 남는 것이 왜 중요할까요?

## 오늘의 정리

- VR은 현실의 머리와 손 좌표를 Unity 오브젝트로 옮기는 방식입니다.
- VR 실습 전에는 Package Manager에서 XR 관련 패키지를 먼저 설치해야 합니다.
- OpenXR은 `XR Plugin Management` 설정에서 켜야 실제 XR 런타임과 연결됩니다.
- XR Origin은 VR 플레이어 전체의 기준점입니다.
- OpenXR은 여러 VR 기기를 공통 방식으로 다루기 위한 표준입니다.
