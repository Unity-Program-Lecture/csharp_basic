# DAY 10: 외부 라이브러리와 에셋 격리

오늘의 목표는 외부 라이브러리와 에셋을 "**빌려 온 공구를 따로 보관하는 선반**"처럼 관리하고, UPM 패키지와 Asset Store 에셋의 차이를 이해하는 것입니다.

## 1. 핵심 개념: "프로젝트 안에서 섞이지 않게 정리하기"

외부 에셋은 편리하지만 프로젝트 구조를 어지럽힐 수 있습니다. UPM 패키지는 `Packages`와 `manifest.json`을 통해 버전이 관리되고, Asset Store 에셋은 보통 `Assets` 폴더 안에 들어옵니다. 그래서 외부 자료는 위치와 출처를 분리해 두는 편이 좋습니다.

### 이 단어는 무슨 뜻인가요?

- **UPM**: Unity Package Manager로 관리되는 패키지 방식입니다.
- **manifest.json**: 프로젝트가 사용하는 패키지 목록과 버전을 기록하는 파일입니다.
- **Asset Store 에셋**: Unity Asset Store에서 내려받아 `Assets` 안에 추가하는 리소스입니다.
- **격리 폴더**: 외부 자료를 프로젝트 내부 제작물과 섞이지 않게 따로 두는 폴더입니다.

## 2. 추천 폴더 구조

```text
Assets/
  _Project/
    Scripts/
    Scenes/
    Prefabs/
  ThirdParty/
    AssetStore/
    Plugins/
```

`_Project`에는 직접 만든 수업 자료를 두고, `ThirdParty`에는 외부에서 가져온 자료를 둡니다. 이렇게 나누면 삭제, 업데이트, 라이선스 확인이 쉬워집니다.

## 실습 예제: 외부 Prefab을 안전하게 생성하기

**미션:** `ThirdParty`에 둔 외부 Prefab을 직접 씬에 배치하지 않고, 생성 지점에서 복사본으로 만들어 봅니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class ExternalPrefabSpawner : MonoBehaviour
{
    [SerializeField] private GameObject externalPrefab;
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        if (externalPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("외부 Prefab 또는 생성 지점이 연결되지 않았습니다.");
            return;
        }

        Instantiate(externalPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
```

</details>

### 실행해보면

Inspector에 외부 Prefab과 생성 지점을 연결하면 Play 시 해당 위치에 복사본이 생성됩니다. 외부 에셋 원본은 `ThirdParty` 폴더에 두고, 씬에서는 필요한 복사본만 사용한다는 흐름을 확인할 수 있습니다.

### 생각해보기

1. 외부 에셋을 `ThirdParty`에 따로 두면 어떤 실수가 줄어들까요?
2. 패키지 버전이 바뀌면 프로젝트에는 어떤 영향이 생길 수 있을까요?

## 오늘의 정리

- UPM 패키지와 Asset Store 에셋은 들어오는 경로와 관리 방식이 다릅니다.
- 외부 자료는 프로젝트 내부 제작물과 폴더를 나누어 관리합니다.
- 간단한 검사용 스크립트로 누락된 참조를 빠르게 확인할 수 있습니다.
