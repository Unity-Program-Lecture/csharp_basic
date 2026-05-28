# DAY 07: Unity 리소스와 Prefab 관리

오늘의 목표는 에셋을 "**창고에 정리해 둔 재료**"로 이해하고, Prefab을 사용해 같은 오브젝트를 안정적으로 반복 배치하는 방법을 익히는 것입니다.

## 1. 핵심 개념: "원본 하나로 여러 복사본 관리하기"

게임에는 같은 물체가 많이 등장합니다. 나무, 총알, 몬스터, 아이템을 매번 새로 만들면 수정하기 어렵습니다. Prefab은 원본 설계도이고, 씬에 놓인 복사본들은 그 설계도를 바탕으로 만들어진 인스턴스입니다.

### 이 단어는 무슨 뜻인가요?

- **Asset**: 프로젝트에 저장된 이미지, 모델, 사운드, 스크립트 같은 파일입니다.
- **Prefab**: GameObject 구성을 파일로 저장한 재사용 설계도입니다.
- **Instance**: Prefab을 씬에 배치해 만든 복사본입니다.
- **Instantiate**: 코드에서 Prefab 복사본을 생성하는 함수입니다.

## 실습 예제: 아이템 Prefab 생성하기

**미션:** 아이템 Prefab을 만들고, 스페이스바를 누르면 새 아이템이 생성되도록 합니다.

1. 큐브를 만들고 이름을 `CoinItem`으로 바꿉니다.
2. Project 창으로 끌어 Prefab으로 저장합니다.
3. 빈 GameObject에 아래 스크립트를 붙이고 Prefab을 연결합니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Vector3 spawnPosition = transform.position + Vector3.forward * 2f;
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
```

</details>

### 실행해보면

스페이스바를 누를 때마다 연결한 Prefab의 복사본이 씬에 생성됩니다. Prefab 원본의 머티리얼을 바꾸면 새로 생성되는 아이템에도 같은 설정이 적용됩니다.

### 생각해보기

1. 몬스터를 Prefab으로 만들면 어떤 수정이 쉬워질까요?
2. Prefab 원본과 씬에 놓인 인스턴스는 어떤 관계일까요?

## 오늘의 정리

- Asset은 프로젝트에 저장된 재료입니다.
- Prefab은 GameObject 묶음을 재사용하기 위한 원본 설계도입니다.
- `Instantiate`를 사용하면 코드에서 Prefab 인스턴스를 만들 수 있습니다.
