# DAY 03: Cinemachine 카메라 추적과 시점 제어

오늘의 목표는 Cinemachine을 사용해 플레이어를 따라가는 카메라를 만들고, 조작 대상과 화면 구성이 자연스럽게 연결되는 흐름을 이해하는 것입니다.

## 1. 핵심 개념: "카메라는 플레이어 조작을 따라가는 안내자이다"

게임 클라이언트에서 카메라는 단순히 장면을 찍는 오브젝트가 아닙니다. 플레이어가 어디로 이동하는지, 무엇을 바라봐야 하는지, 어떤 상황을 알아야 하는지 계속 보조합니다.

Cinemachine은 이런 카메라 동작을 직접 스크립트로 모두 만들지 않고도 구성할 수 있게 해 주는 Unity 카메라 도구입니다. 실무에서도 플레이어 추적, 시점 전환, 연출 카메라, 줌, 흔들림 등을 만들 때 자주 사용합니다.

### 이 단어는 무슨 뜻인가요?

- **Cinemachine**: Unity에서 카메라 추적과 전환을 쉽게 구성하는 패키지입니다.
- **Virtual Camera**: 실제 Camera를 직접 렌더링하지 않고, 카메라가 따라야 할 위치와 시점 규칙을 제공하는 가상 카메라입니다.
- **Follow**: 카메라가 따라갈 대상입니다.
- **Look At**: 카메라가 바라볼 대상입니다.
- **Priority**: 여러 Virtual Camera 중 어떤 카메라를 우선 사용할지 정하는 값입니다.
- **Body**: 카메라 위치를 어떻게 따라갈지 정하는 설정입니다.
- **Aim**: 카메라가 대상을 어떻게 바라볼지 정하는 설정입니다.
- **Blend**: 카메라가 전환될 때 부드럽게 이어지는 방식입니다.

## 2. 엔진 과정 카메라와 클라이언트 과정 카메라의 차이

`03.GameEngine` 과정에서는 카메라를 장면을 보여 주는 엔진 오브젝트로 다룹니다. `04.GameClient` 과정에서는 카메라를 플레이어 조작과 연결된 클라이언트 시스템으로 다룹니다.

| 구분 | 핵심 관점 |
| :--- | :--- |
| 엔진 과정 카메라 | Game View 구성, 투영 방식, 고정 시점, 장면 확인 |
| 클라이언트 과정 카메라 | 플레이어 추적, 시점 보조, 조작 피드백, 카메라 전환 |

## 3. Cinemachine 기본 구성

Cinemachine을 사용할 때 실제 화면을 렌더링하는 것은 여전히 `Main Camera`입니다. Cinemachine Virtual Camera는 `Main Camera`가 어디에 있고 무엇을 바라볼지 지시합니다.

기본 구성은 다음 순서로 이해합니다.

1. `Main Camera`에 Cinemachine Brain이 붙어 있는지 확인합니다.
2. Cinemachine Virtual Camera를 생성합니다.
3. Virtual Camera의 `Follow`에 플레이어 Transform을 연결합니다.
4. Virtual Camera의 `Look At`에 플레이어 Transform 또는 시선 기준 오브젝트를 연결합니다.
5. Body와 Aim 설정으로 따라가는 거리, 높이, 부드러움을 조정합니다.

## 실습 예제: 플레이어를 따라가는 Cinemachine 카메라 만들기

**미션:** `DAY02`에서 만든 플레이어 이동 오브젝트를 Cinemachine 카메라가 따라가도록 구성합니다.

1. Package Manager에서 `Cinemachine`이 설치되어 있는지 확인합니다.
2. 씬에 플레이어 역할을 하는 오브젝트를 준비합니다.
3. `Main Camera`를 선택하고 `Cinemachine Brain` Component가 있는지 확인합니다.
4. 메뉴에서 Cinemachine Virtual Camera를 생성합니다.
5. Virtual Camera의 `Follow`에 플레이어 오브젝트를 연결합니다.
6. Virtual Camera의 `Look At`에도 플레이어 오브젝트를 연결합니다.
7. 카메라가 플레이어보다 약간 위와 뒤에서 따라오도록 Body 설정을 조정합니다.
8. Play 버튼을 누르고 플레이어를 움직여 카메라가 따라오는지 확인합니다.

### 권장 설정 예시

쿼터뷰 추적 카메라를 만들 때는 다음 방향으로 조정합니다.

```text
Follow: Player
Look At: Player
Body: Framing Transposer 또는 Position Composer 계열 설정
Tracked Object Offset: X 0, Y 1, Z 0
Camera Distance: 8 전후
Camera 위치 감각: 플레이어보다 위와 뒤
```

프로젝트의 Cinemachine 버전에 따라 설정 이름이 다를 수 있습니다. 중요한 것은 Virtual Camera가 플레이어를 따라가고, `Main Camera`가 Cinemachine Brain을 통해 그 결과를 받아 화면에 보여 준다는 구조입니다.

## 4. 시점 전환의 기본 원리

여러 Virtual Camera를 만들면 상황에 따라 카메라를 바꿀 수 있습니다. 예를 들어 기본 추적 카메라, 목표 지점 카메라, 클로즈업 카메라를 따로 만들 수 있습니다.

Cinemachine에서는 일반적으로 `Priority`가 높은 Virtual Camera가 선택됩니다. 따라서 특정 상황에서 한 카메라의 `Priority`를 높이면 자연스럽게 시점이 전환됩니다.

```csharp
using Unity.Cinemachine;
using UnityEngine;

public class CameraPrioritySwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera normalCamera;
    [SerializeField] private CinemachineCamera focusCamera;

    public void ShowFocusCamera()
    {
        normalCamera.Priority = 10;
        focusCamera.Priority = 20;
    }

    public void ShowNormalCamera()
    {
        normalCamera.Priority = 20;
        focusCamera.Priority = 10;
    }
}
```

위 코드는 카메라를 직접 움직이지 않습니다. 두 Virtual Camera의 우선순위만 바꾸어 Cinemachine이 자연스럽게 카메라를 전환하게 합니다.

단, 이 스크립트만 오브젝트에 붙여 둔다고 해서 시점이 자동으로 바뀌지는 않습니다. `ShowFocusCamera` 또는 `ShowNormalCamera`를 버튼, Trigger, 상호작용, 키 입력 같은 다른 흐름에서 호출해야 합니다.

예를 들어 특정 구역에 들어갔을 때 목표 오브젝트를 보여 주는 카메라로 전환하려면 다음처럼 사용할 수 있습니다.

```csharp
using UnityEngine;

public class CameraFocusZone : MonoBehaviour
{
    [SerializeField] private CameraPrioritySwitcher cameraSwitcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraSwitcher.ShowFocusCamera();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraSwitcher.ShowNormalCamera();
        }
    }
}
```

사용 순서는 다음과 같습니다.

1. 기본 추적용 Virtual Camera와 포커스용 Virtual Camera를 씬에 만듭니다.
2. `CameraPrioritySwitcher`를 빈 오브젝트에 붙입니다.
3. `normalCamera`에는 기본 추적 카메라를, `focusCamera`에는 포커스 카메라를 연결합니다.
4. Trigger Collider가 있는 구역 오브젝트를 만들고 `Is Trigger`를 켭니다.
5. 구역 오브젝트에 `CameraFocusZone`을 붙입니다.
6. `cameraSwitcher` 슬롯에 `CameraPrioritySwitcher`가 붙은 오브젝트를 연결합니다.
7. 플레이어 오브젝트에 `Player` 태그가 설정되어 있는지 확인합니다.

이제 플레이어가 Trigger 구역에 들어가면 포커스 카메라의 우선순위가 높아지고, 구역에서 나오면 기본 카메라의 우선순위가 다시 높아집니다.

### 실행해보면

플레이어가 이동하면 Virtual Camera가 Follow 대상의 위치를 기준으로 카메라를 갱신합니다. `Main Camera`는 Cinemachine Brain을 통해 이 결과를 받아 Game View에 출력합니다.

Trigger 구역에 들어가면 `focusCamera`의 `Priority`가 더 높아져 포커스 시점으로 전환됩니다. Trigger 구역에서 나오면 `normalCamera`의 `Priority`가 더 높아져 기본 추적 시점으로 돌아옵니다.

### 생각해보기

1. 카메라가 플레이어를 너무 늦게 따라오면 어떤 문제가 생길까요?
2. 카메라가 너무 가까우면 플레이어는 어떤 정보를 놓치게 될까요?
3. 전투, 탐험, 퍼즐 장면에서 필요한 카메라 거리는 서로 같을까요?

## 오늘의 정리

- 클라이언트 과정의 카메라는 플레이어 조작과 연결된 시스템입니다.
- Cinemachine은 플레이어 추적, 시점 전환, 카메라 보간을 쉽게 구성하게 해 줍니다.
- `Main Camera`는 실제 화면을 렌더링하고, Virtual Camera는 카메라 규칙을 제공합니다.
- 실무형 프로젝트에서는 직접 추적 스크립트를 작성하기 전에 Cinemachine으로 기본 카메라를 구성하는 것이 효율적입니다.
