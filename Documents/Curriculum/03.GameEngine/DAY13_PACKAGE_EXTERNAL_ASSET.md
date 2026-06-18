# DAY 13: Package Manager와 외부 에셋 관리

오늘의 목표는 Unity 패키지와 외부 에셋을 "**필요한 공구를 들여오고 출처별로 정리하는 공구실**"처럼 이해하고, Package Manager로 기능을 추가한 뒤 외부 자료를 안전하게 관리하는 흐름을 익히는 것입니다.

## 1. 핵심 개념: "필요한 공구만 들여오고 출처를 기록하기"

Unity 기본 기능만으로도 많은 것을 만들 수 있지만, AI Navigation, ProBuilder, XR처럼 별도 패키지로 제공되는 기능도 많습니다. Package Manager는 이런 기능을 설치하고 버전을 관리하는 창입니다.

외부 기능과 에셋은 편리하지만 무작정 추가하면 프로젝트가 무거워지고 구조가 어지러워질 수 있습니다. UPM 패키지는 `Packages`와 `manifest.json`을 통해 버전이 관리되고, Asset Store 에셋은 보통 `Assets` 폴더 안에 들어옵니다. 따라서 설치 이유, 출처, 버전, 업데이트 방식을 함께 확인해야 합니다.

### 이 단어는 무슨 뜻인가요?

- **Package**: Unity 기능을 묶어 배포하는 단위입니다.
- **Package Manager**: 패키지를 설치, 제거, 업데이트하는 Unity 창입니다.
- **Registry**: 패키지를 내려받는 저장소입니다.
- **Plugin**: 엔진에 추가 기능을 붙이는 외부 모듈입니다.
- **UPM**: Unity Package Manager로 관리되는 패키지 방식입니다.
- **manifest.json**: 프로젝트가 사용하는 패키지 목록과 버전을 기록하는 파일입니다.
- **Asset Store 에셋**: Unity Asset Store에서 내려받아 `Assets` 안에 추가하는 리소스입니다.
- **격리 폴더**: 외부 자료와 직접 만든 자료가 섞이지 않도록 역할을 나누어 두는 폴더입니다.
- **.meta 파일**: Unity가 에셋을 구분하기 위해 만드는 신분증 같은 파일입니다. 이 파일이 사라지거나 새로 만들어지면 기존 참조가 깨질 수 있습니다.

## 2. UPM 패키지와 Asset Store 에셋 비교

| 구분 | UPM 패키지 | Asset Store 에셋 |
| :--- | :--- | :--- |
| 주 관리 위치 | `Packages`, `manifest.json` | 주로 `Assets` 폴더 |
| 설치 방법 | Package Manager | Package Manager의 My Assets 또는 에셋 Import |
| 버전 확인 | Package Manager에서 확인 | 배포 페이지와 가져온 에셋 정보 확인 |
| 주의할 점 | 프로젝트 및 Unity 버전 호환성 | 폴더 이동, 중복 Import, 원본 참조 손상 |

패키지나 플러그인은 기능을 확장하지만 버전이 바뀌면 API나 설정 방식도 달라질 수 있습니다. 수업 프로젝트에서는 필요한 도구만 설치하고, 팀 프로젝트라면 설치 이유와 버전을 기록합니다.

## 실습 1: ProBuilder 패키지 확인하기

**미션:** Package Manager에서 ProBuilder 설치 여부를 확인하고, 패키지가 Unity에 기능을 추가하는 흐름을 살펴봅니다.

1. `Window > Package Manager`를 엽니다.
2. 패키지 목록을 `Unity Registry`로 변경합니다.
3. `ProBuilder`를 검색하고 설치 상태와 버전을 확인합니다.
4. 설치되어 있지 않다면 `Install`을 눌러 설치합니다.
5. 설치 후 `Tools > ProBuilder` 메뉴 또는 ProBuilder 창이 추가되었는지 확인합니다.

ProBuilder는 레벨 블록아웃을 빠르게 만드는 도구입니다. 설치 자체보다 "**어떤 기능이 필요해서 어떤 패키지를 추가했는지**" 설명할 수 있는 것이 중요합니다.

### 생각해보기

1. 편리한 패키지를 무작정 많이 설치하면 어떤 문제가 생길까요?
2. 팀원이 같은 프로젝트를 열 때 패키지 버전 정보가 필요한 이유는 무엇일까요?

## 3. 외부 에셋 추천 폴더 구조

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

## 실습 2: 외부 Prefab을 안전하게 생성하기

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

- Package Manager는 Unity 기능 패키지의 설치 상태와 버전을 관리하는 창입니다.
- 패키지와 플러그인은 프로젝트를 확장하지만 설치 이유와 호환성을 확인해야 합니다.
- UPM 패키지와 Asset Store 에셋은 들어오는 경로와 관리 방식이 다릅니다.
- 외부 자료는 출처를 알아볼 수 있게 관리하되, Asset Store 에셋은 기본 경로를 유지하는 편이 안전할 수 있습니다.
- 직접 만든 씬, 스크립트, Prefab Variant는 `_Project`에 두어 외부 원본과 분리합니다.
- 간단한 검사용 스크립트로 누락된 참조를 빠르게 확인할 수 있습니다.
