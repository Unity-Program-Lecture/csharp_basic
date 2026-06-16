# DAY 16: 게임엔진 응용 프로그래밍 정리

오늘의 목표는 15일 동안 배운 내용을 "**작은 게임 제작 체크리스트**"로 다시 묶고, Unity 프로젝트를 스스로 점검하는 방법을 정리하는 것입니다.

## 1. 핵심 개념: "작은 게임은 여러 부품의 조립품"

Unity 프로젝트는 하나의 거대한 기능이 아니라 작은 부품의 조립입니다. 씬에는 오브젝트가 있고, 오브젝트에는 컴포넌트가 있고, 스크립트는 컴포넌트 사이의 규칙을 연결합니다. 마지막 날에는 새 기능을 더 넣기보다 지금까지 만든 요소가 서로 잘 연결되는지 확인합니다.

### 이 단어는 무슨 뜻인가요?

- **Scene Check**: 씬에 필요한 오브젝트와 참조가 있는지 확인하는 과정입니다.
- **Play Test**: 실제로 실행하며 입력, 물리, UI, 사운드가 이어지는지 확인하는 과정입니다.
- **Build Settings**: 실행 파일을 만들 플랫폼과 씬 목록을 정하는 설정입니다.
- **Refactor**: 동작은 유지하면서 코드와 구조를 더 읽기 좋게 정리하는 작업입니다.

## 2. 마무리 점검표

| 항목 | 확인 질문 |
| :--- | :--- |
| 씬 | 시작 씬이 Build Settings에 들어 있나요? |
| 입력 | 플레이어 조작이 의도대로 작동하나요? |
| 물리 | Collider와 Rigidbody 설정이 맞나요? |
| UI | 점수, 버튼, 안내 문구가 화면에 잘 보이나요? |
| 사운드/VFX | 중요한 행동에 피드백이 있나요? |
| 에셋 | 외부 에셋과 직접 만든 에셋이 구분되어 있나요? |

## 실습 예제: 필수 참조 점검하기

**미션:** 최종 씬에서 꼭 연결되어야 하는 오브젝트가 빠졌는지 실행 시 확인합니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class FinalSceneChecker : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Player가 연결되지 않았습니다.");
        }

        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera가 연결되지 않았습니다.");
        }

        Debug.Log("최종 씬 점검을 완료했습니다.");
    }
}
```

</details>

### 실행해보면

Inspector에서 참조를 비워 둔 항목은 경고로 표시됩니다. 참조를 연결한 뒤 다시 실행하면 경고 없이 점검 완료 메시지를 볼 수 있습니다.

### 생각해보기

1. 최종 제출 전에 Play Test를 여러 번 해야 하는 이유는 무엇일까요?
2. 경고 메시지를 친절하게 쓰면 팀 작업에서 어떤 도움이 될까요?

## 오늘의 정리

- 게임엔진 수업의 핵심은 GameObject, Component, Scene, 입력, 물리, UI의 연결입니다.
- 최종 점검은 새 기능 추가보다 누락된 참조와 실행 흐름 확인에 집중합니다.
- 작은 체크리스트를 사용하면 프로젝트 마무리 실수를 줄일 수 있습니다.
