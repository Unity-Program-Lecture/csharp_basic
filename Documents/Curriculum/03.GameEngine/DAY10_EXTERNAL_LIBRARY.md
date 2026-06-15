# DAY 10: 외부 라이브러리와 에셋 격리

오늘의 목표는 외부 라이브러리와 에셋을 "**빌려 온 공구를 따로 보관하는 선반**"처럼 관리하고, UPM 패키지와 Asset Store 에셋의 차이를 이해하는 것입니다.

## 1. 핵심 개념: "프로젝트 안에서 섞이지 않게 정리하기"

외부 에셋은 편리하지만 프로젝트 구조를 어지럽힐 수 있습니다. UPM 패키지는 `Packages`와 `manifest.json`을 통해 버전이 관리되고, Asset Store 에셋은 보통 `Assets` 폴더 안에 들어옵니다. 그래서 외부 자료는 출처를 알아볼 수 있게 분리하되, 업데이트 방식까지 함께 생각해야 합니다.

### 이 단어는 무슨 뜻인가요?

- **UPM**: Unity Package Manager로 관리되는 패키지 방식입니다.
- **manifest.json**: 프로젝트가 사용하는 패키지 목록과 버전을 기록하는 파일입니다.
- **Asset Store 에셋**: Unity Asset Store에서 내려받아 `Assets` 안에 추가하는 리소스입니다.
- **격리 폴더**: 외부 자료와 직접 만든 자료가 섞이지 않도록 역할을 나누어 두는 폴더입니다.
- **.meta 파일**: Unity가 에셋을 구분하기 위해 만드는 신분증 같은 파일입니다. 이 파일이 사라지거나 새로 만들어지면 기존 참조가 깨질 수 있습니다.

## 2. 추천 폴더 구조

```text
Assets/
  _Project/
    Scripts/
    Scenes/
    Prefabs/
    Materials/
  AssetStoreVendorName/
  PluginName/
  ThirdParty/
    SmallManualAssets/
```

`_Project`에는 직접 만든 수업 자료를 둡니다. Asset Store 에셋이나 플러그인은 제작자가 정한 기본 폴더 이름을 유지하는 편이 안전한 경우가 많습니다. 업데이트할 때 Unity가 다시 원래 경로로 파일을 가져올 수 있기 때문입니다.

`ThirdParty` 폴더는 작은 수동 에셋처럼 업데이트 도구가 따로 없고, 우리가 위치를 직접 관리해도 되는 자료에만 사용합니다. 이미 가져온 Asset Store 에셋을 억지로 `ThirdParty` 아래로 옮기면 업데이트 때 중복 폴더가 생기거나, Prefab과 Material 참조가 꼬일 수 있습니다.

중요한 규칙은 "**외부 원본을 마음대로 옮기기보다, 우리가 쓰는 결과물을 `_Project`에 따로 만든다**"입니다. 예를 들어 외부 Prefab 원본은 그대로 두고, 수업에서 쓰는 Prefab Variant나 래퍼 Prefab을 `_Project/Prefabs`에 만들어 연결합니다.

## 실습 예제: 외부 Prefab을 안전하게 생성하기

**미션:** 외부 Prefab 원본을 직접 씬에 많이 배치하지 않고, `_Project`에서 관리하는 생성 스크립트를 통해 필요한 위치에 복사본을 만들어 봅니다.

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

Inspector에 외부 Prefab과 생성 지점을 연결하면 Play 시 해당 위치에 복사본이 생성됩니다. 외부 에셋 원본은 제작자 기본 경로나 별도 출처 폴더에 그대로 두고, 우리 프로젝트의 씬과 스크립트는 `_Project`에서 관리한다는 흐름을 확인할 수 있습니다.

### 생각해보기

1. Asset Store 에셋을 가져온 뒤 폴더를 마음대로 옮기면 업데이트할 때 어떤 문제가 생길 수 있을까요?
2. 외부 원본은 그대로 두고 `_Project`에 Prefab Variant나 래퍼 Prefab을 만들면 어떤 실수가 줄어들까요?

## 오늘의 정리

- UPM 패키지와 Asset Store 에셋은 들어오는 경로와 관리 방식이 다릅니다.
- 외부 자료는 출처를 알아볼 수 있게 관리하되, Asset Store 에셋은 기본 경로를 유지하는 편이 안전할 수 있습니다.
- 직접 만든 씬, 스크립트, Prefab Variant는 `_Project`에 두어 외부 원본과 분리합니다.
- 간단한 검사용 스크립트로 누락된 참조를 빠르게 확인할 수 있습니다.
