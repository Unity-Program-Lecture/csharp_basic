# DAY 02: Hierarchy와 Component 구조

오늘의 목표는 Unity 씬을 "**책상 위에 쌓인 도구 상자들**"처럼 보고, 부모-자식 GameObject 구조와 Component 조합 방식을 익히는 것입니다.

## 1. 핵심 개념: "상자 안의 작은 상자"

Hierarchy는 씬에 놓인 오브젝트 목록입니다. 오브젝트를 부모-자식으로 묶으면 부모를 움직일 때 자식도 함께 움직입니다. 캐릭터 몸통 아래에 무기, 이름표, 이펙트를 넣어 두면 캐릭터가 이동할 때 모두 같이 따라가는 식입니다.

Component는 오브젝트의 기능입니다. 같은 GameObject라도 `Rigidbody`를 붙이면 물리 오브젝트가 되고, `Light`를 붙이면 조명이 됩니다.

### 이 단어는 무슨 뜻인가요?

- **Hierarchy**: 씬 안의 GameObject 목록과 부모-자식 관계를 보여주는 창입니다.
- **Parent**: 다른 오브젝트를 품고 함께 움직이는 상위 오브젝트입니다.
- **Child**: 부모를 기준으로 위치가 계산되는 하위 오브젝트입니다.
- **Prefab**: 반복해서 배치할 수 있도록 저장해 둔 GameObject 묶음입니다.

## 실습 예제: 캐릭터 구조 만들기

**미션:** 빈 GameObject를 캐릭터 루트로 만들고, 몸체와 이름표 역할을 하는 자식 오브젝트를 붙입니다.

1. `Create Empty`로 `PlayerRoot`를 만듭니다.
2. `PlayerRoot` 아래에 `Body` 큐브를 자식으로 둡니다.
3. `PlayerRoot` 아래에 `NameAnchor` 빈 오브젝트를 자식으로 둡니다.
4. 아래 스크립트를 `PlayerRoot`에 붙여 부모를 회전시켰을 때 자식들이 함께 움직이는지 확인합니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class CharacterTurntable : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 45f;

    void Start()
    {
        Debug.Log("부모 오브젝트가 회전하면 자식 오브젝트도 함께 따라갑니다.");
    }

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}
```

</details>

### 실행해보면

Play를 누르면 `PlayerRoot`가 천천히 회전하고, 자식인 `Body`와 `NameAnchor`도 함께 회전합니다. 자식을 부모 밖으로 꺼내면 더 이상 같은 방식으로 따라가지 않는 점을 비교할 수 있습니다.

### 생각해보기

1. 무기를 캐릭터의 자식으로 두면 어떤 점이 편할까요?
2. 부모의 크기를 바꾸면 자식의 화면 크기는 어떻게 바뀔까요?

## 오늘의 정리

- Hierarchy는 오브젝트의 배치와 포함 관계를 보여줍니다.
- 부모 Transform이 움직이면 자식 Transform도 함께 영향을 받습니다.
- Prefab은 자주 쓰는 오브젝트 묶음을 재사용하기 위한 저장본입니다.
