# 🚀 Day 07: 에셋 관리와 리소스 최적화 (Prefabs & Pooling)

오늘의 목표는 "**반복 사용되는 오브젝트를 설계도화하는 프리팹(Prefab)의 개념을 이해하고, 메모리 관리를 위한 오브젝트 풀링을 적용한다**"입니다.

---

## 1. 프리팹 (Prefab): "오브젝트 붕어빵 틀"
미리 구성된 게임 오브젝트를 프로젝트 에셋으로 저장한 것입니다.
- **장점**: 한 곳에서 수정하면 모든 복사본(Instance)에 동시에 적용됩니다.
- **용도**: 총알, 몬스터, 코인 등 대량으로 생성되는 물체.

---

## 2. 리소스 최적화: 오브젝트 풀링 (Object Pooling)
유니티의 `Instantiate`(생성)와 `Destroy`(파괴)는 매우 무거운 작업입니다.
- **원리**: 시작할 때 미리 만들어 놓고, 필요할 때 활성화(`SetActive`)하여 사용한 뒤 다시 비활성화하는 방식입니다.
- **효과**: 가비지 컬렉터(GC)의 부하를 줄여 프레임 드랍을 방지합니다.

---

## 💻 실습 예제: 큐(Queue)를 이용한 총알 풀링 시스템 (교재 실습)
1. 총알(Bullet) 프리팹을 만듭니다.
2. 풀링 매니저 스크립트를 작성하여 큐에 총알을 담고 관리합니다.

<details>
<summary>코드 보기</summary>

```csharp
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        // 10개를 미리 생성하여 비활성화 상태로 풀에 저장
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetBullet()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return Instantiate(bulletPrefab); // 부족할 경우 새로 생성
    }

    public void ReturnBullet(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 유니티에서 게임 오브젝트를 파일 형태로 저장하여 재사용할 수 있게 만든 에셋을 무엇이라 합니까?
   - **정답:** 프리팹 (Prefab)
2. **문제:** 빈번한 오브젝트 생성/파괴로 인한 렉을 줄이기 위해 미리 객체를 생성해 두고 재사용하는 기법의 명칭은?
   - **정답:** 오브젝트 풀링 (Object Pooling)
