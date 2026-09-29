# 참고: Unity 6에서 SQLite `GameShop.db` 사용하기

이 문서는 7일차에서 앞서 만든 SQLite `GameShop.db`를 Unity 6에서 읽는 방법을 설명합니다. `com.gilzoide.sqlite-net`은 SQLite-net과 Unity 플랫폼용 SQLite 라이브러리를 함께 제공하는 UPM (Unity Package Manager, Unity 패키지 관리자) 패키지입니다.

> 이 문서의 코드는 Unity 6이 지원하는 **C# 9.0** 기준입니다.

## 1. 이 패키지를 쓰는 이유

SQLite의 데이터는 한 개의 파일에 저장됩니다. 따라서 콘솔에서 만든 `GameShop.db`와 Unity에서 읽는 `GameShop.db`는 같은 SQLite 형식입니다. 이 패키지는 Windows, macOS, Linux, Android, iOS, WebGL을 지원한다고 안내합니다. WebGL에서는 IndexedDB 기반 저장소를 사용합니다.

| 구분 | 7일차의 선택 |
| :--- | :--- |
| DB 형식 | SQLite |
| Unity 패키지 | `com.gilzoide.sqlite-net` |
| 설치 방법 | Unity Package Manager의 Git URL |
| 읽을 표 | 4일차의 `Item` |
| DB 파일 위치 | 읽기 전용 원본은 Unity 자산, 실행 중 저장은 `Application.persistentDataPath` |

## 2. UPM 패키지 설치하기

1. Unity 6 프로젝트를 엽니다.
2. 메뉴에서 `Window > Package Manager`를 엽니다.
3. 왼쪽 위 `+` 버튼을 누르고 `Add package from git URL...`을 선택합니다.
4. 아래 URL 전체를 붙여 넣고 `Add`를 누릅니다.

```text
https://github.com/gilzoide/unity-sqlite-net.git#1.3.2
```

5. 설치와 재컴파일이 끝날 때까지 기다립니다.
6. Package Manager의 `In Project` 목록에서 `SQLite-net` 패키지가 보이고, Console에 오류가 없는지 확인합니다.

> 이 패키지는 NuGetForUnity가 필요하지 않습니다. NuGetForUnity는 LiteDB처럼 NuGet으로만 배포되는 라이브러리를 Unity에서 시험할 때 사용합니다.

## 3. 기존 `GameShop.db`를 Unity 자산으로 준비하기

1. 4일차의 콘솔 프로젝트에서 `GameShop.db`를 찾습니다.
2. 원본은 보관하고, 복사본 파일 이름을 `GameShop.sqlite`로 바꿉니다. 확장자만 바뀌며 SQLite 데이터 내용은 바뀌지 않습니다.
3. Unity 프로젝트의 `Assets/Databases/` 폴더를 만들고 `GameShop.sqlite` 복사본을 넣습니다.
4. Project 창에서 `GameShop.sqlite`를 선택합니다. SQLite 자산으로 가져와졌는지 Inspector에서 확인합니다.

`GameShop.sqlite` 자산은 배포용 읽기 전용 원본입니다. 플레이어가 수량·골드를 바꾸는 저장 데이터는 처음 실행할 때 별도 복사본을 만들어 `Application.persistentDataPath`에 저장해야 합니다.

> Android와 WebGL은 StreamingAssets의 SQLite 파일을 일반 파일 경로로 여는 방식을 지원하지 않습니다. 이 패키지의 SQLite 자산은 이 플랫폼에서 메모리로 읽히므로, 원본을 읽는 수업 실습은 같은 방식으로 진행할 수 있습니다.

## 4. `Item` 표 읽기

새 스크립트 `Assets/Scripts/GameShopDatabaseReader.cs`를 만들고 아래 코드를 직접 입력합니다. `GameShop.sqlite` 자산을 Inspector에서 `Database Asset` 칸에 연결합니다.

```csharp
using System.Collections.Generic;
using SQLite;
using TMPro;
using UnityEngine;

public class ItemRow
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
}

public class GameShopDatabaseReader : MonoBehaviour
{
    [SerializeField] private SQLiteAsset databaseAsset;
    [SerializeField] private TMP_Text itemText;

    private void Start()
    {
        using (SQLiteConnection database = databaseAsset.CreateConnection())
        {
            List<ItemRow> items = database.Query<ItemRow>(
                "SELECT ItemId, Name, Price FROM Item ORDER BY ItemId");

            if (items.Count == 0)
            {
                itemText.text = "Item 표에 데이터가 없습니다.";
                return;
            }

            ItemRow firstItem = items[0];
            itemText.text = firstItem.Name + " : " + firstItem.Price + " Gold";
            Debug.Log("SQLite Item: " + itemText.text);
        }
    }
}
```

`SELECT ItemId, Name, Price FROM Item`은 1일차에서 배운 것처럼 "Item 표에서 세 열을 조회한다"는 뜻입니다. 여기서 처음 추가된 `ORDER BY ItemId`는 조회한 행을 `ItemId`의 작은 값부터 정렬하라는 문법입니다. 큰 값부터 보고 싶다면 뒤에 `DESC`를 붙여 `ORDER BY ItemId DESC`라고 씁니다.

### 코드 읽기

1. `SQLiteAsset`은 Unity에 포함한 읽기 전용 SQLite 원본입니다.
2. `CreateConnection()`은 그 원본 DB에 연결합니다.
3. `Query<ItemRow>()`는 SQL의 각 결과 행을 `ItemRow` 객체로 바꿉니다.
4. 첫 번째 아이템을 TextMeshPro UI와 Unity Console에 함께 출력합니다.

## 5. 실행 확인 순서

1. Canvas 안에 TextMeshPro 텍스트 하나를 만들고 `ItemText`로 이름 붙입니다.
2. 빈 GameObject를 만들고 `GameShopDatabaseReader`를 붙입니다.
3. `Database Asset`에 `GameShop.sqlite`를 연결합니다.
4. `Item Text`에 Canvas의 `ItemText`를 연결합니다.
5. Play Mode를 누릅니다.
6. DB Browser에서 확인한 `Item` 첫 행의 이름·가격이 화면과 Console에 함께 보이는지 확인합니다. 예를 들어 첫 행이 `회복 포션`, `30`이면 `회복 포션 : 30 Gold`가 표시됩니다.

## 6. 이번 실습의 범위

`GameShopDatabaseReader`는 배포용 읽기 전용 자산의 `Item`만 조회합니다. 구매, 골드·인벤토리 변경, 저장 후 재조회는 구현하지 않았습니다. 이런 변경 데이터를 Unity에서 다루려면 `Application.persistentDataPath`의 쓰기 가능한 DB를 열고 구매 트랜잭션과 화면 갱신까지 별도로 구현해야 합니다. 앞서 C# 콘솔에서 수행한 구매 결과가 이 자산에 자동으로 반영되지는 않습니다.

> 온라인 게임에서는 Unity 클라이언트가 서버 운영 DB를 직접 열지 않습니다. `Unity -> 서버/API -> DB` 구조에서 서버가 구매·골드 변경 규칙을 확인합니다.

## 7. 문제 해결

| 증상 | 확인 순서 |
| :--- | :--- |
| `SQLiteAsset` 형식이 보이지 않음 | 패키지 설치 완료와 `GameShop.sqlite` 확장자 확인 |
| `no such table: Item` | 4일차에서 만든 `GameShop.db` 복사본인지, 표 이름이 `Item`인지 확인 |
| Inspector 연결 칸이 비어 있음 | 스크립트 컴파일 오류를 먼저 해결한 뒤 GameObject를 다시 선택 |
| WebGL에서 파일 경로 오류 | `Application.streamingAssetsPath`의 직접 파일 열기를 쓰지 않았는지 확인 |

## 공식 참고

- [Unity용 SQLite-net 패키지](https://github.com/gilzoide/unity-sqlite-net)
- [SQLite 파일 형식의 플랫폼 호환성](https://www.sqlite.org/about.html)
- [Unity Package Manager 창](https://docs.unity3d.com/Manual/upm-ui.html)
