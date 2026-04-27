# 🚀 Day 07: 게임 자료구조 응용 (오브젝트 풀링)

오늘의 목표는 **"총알이나 몬스터처럼 자주 생성되고 파괴되는 오브젝트를 관리할 때, 메모리 부하를 막기 위해 풀링(Pooling) 기법을 이해하고 적용한다"**입니다.

---

## 1. 💡 이론 (30%): 오브젝트 풀링(Object Pooling)
- **문제점**: 유니티에서 `Instantiate`(생성)와 `Destroy`(파괴)는 컴퓨터에 아주 무거운 작업입니다. 짧은 시간에 수백 개의 총알을 만들고 부수면 렉(프레임 드랍)이 발생합니다.
- **해결책**: "미리 왕창 만들어두고 돌려쓰자!"
  1. 게임 시작 시 오브젝트를 필요한 만큼 미리 생성하여 비활성화(`SetActive(false)`) 해둡니다. (이를 '풀(Pool)'에 담는다고 합니다.)
  2. 필요할 때 풀에서 꺼내서 활성화(`SetActive(true)`)하여 사용합니다.
  3. 다 쓰면 파괴하지 않고 다시 비활성화하여 풀에 돌려보냅니다.
- **활용 자료구조**: 주로 `Queue`나 `List`를 사용하여 오브젝트 풀을 관리합니다.

---

## 2. 💻 실습 (70%): 총알 풀링 시스템 구현
**미션:** 큐(Queue) 자료구조를 사용하여, 총알을 쏘고 파괴하는 대신 풀에서 꺼내고 돌려받는 오브젝트 풀링을 구현하세요.

<details>
<summary>코드 보기</summary>

```csharp
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 10;
    
    // 비활성화된 총알들을 보관할 큐
    private Queue<GameObject> bulletQueue = new Queue<GameObject>();

    void Start()
    {
        // 1. 미리 생성해서 큐에 넣기
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false); // 안 보이게 끔
            bulletQueue.Enqueue(obj); // 큐에 보관
        }
    }

    // 2. 필요할 때 큐에서 꺼내기
    public GameObject GetBullet()
    {
        if (bulletQueue.Count > 0)
        {
            GameObject obj = bulletQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // 큐가 비었다면 추가로 1개 생성해서 반환 (유동적 대처)
            GameObject obj = Instantiate(bulletPrefab);
            return obj;
        }
    }

    // 3. 다 쓴 총알을 큐로 돌려주기 (총알 스크립트에서 호출)
    public void ReturnBullet(GameObject obj)
    {
        obj.SetActive(false);
        bulletQueue.Enqueue(obj);
    }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 풀링(Pooling)을 수행하여 게임에 적용하며, 풀링을 수행하기 위해서 어떤 자료구조와 알고리즘이 활용되는지 설명하세요.
   - **정답:** 총알이나 이펙트처럼 빈번하게 생성/파괴되는 객체의 메모리 부하(Instantiate/Destroy)를 줄이기 위해, 시작 전 미리 객체들을 만들어 비활성화 상태로 자료구조에 보관해두고 재사용하는 기법입니다. 이를 관리하기 위해 주로 **큐(Queue)**나 **리스트(List)** 자료구조를 활용합니다.
