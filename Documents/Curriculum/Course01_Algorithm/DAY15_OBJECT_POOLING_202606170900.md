# 🚀 DAY15: Object Pooling - 가비지 컬렉터와의 전쟁
_최종 수정일: 202606170900_

## 🚀 학습 목표
- 자주 생성되고 파괴되는 객체가 성능(GC)에 미치는 영향을 이해합니다.
- Queue나 Stack을 활용한 객체 풀링(Object Pooling) 시스템을 구현합니다.

---

## 💡 개념 설명: 왜 풀링(Pooling)인가?
- **비유: 식당의 접시**
    - 손님이 올 때마다 새 접시를 사고(Instantiate), 손님이 가면 접시를 깨뜨려 버리는(Destroy) 식당이 있다고 합시다. 돈이 엄청나게 많이 들고 쓰레기도 많이 나오겠죠?
    - 현명한 사장님은 접시를 '씻어서 다시 사용'합니다. 이것이 바로 **풀링(Pooling)**입니다.
- **게임에서의 활용:**
    - 총알, 파티클, 적 몬스터 등 수없이 많이 나오고 사라지는 것들을 메모리에서 완전히 지우지 않고, '비활성화'했다가 필요할 때 다시 '활성화'하여 사용합니다.
    - 이렇게 하면 메모리를 정리하는 **가비지 컬렉터(GC)**가 일을 덜 하게 되어 프레임 드랍(렉)이 줄어듭니다.

---

## 💻 실습 예제: Simple Bullet Pool

**미션:** `Queue<GameObject>`를 사용하여 총알을 미리 생성해두고, 필요할 때 꺼내 쓰고 반납하는 풀 시스템을 만드세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        // 미리 생성해서 비활성화 상태로 보관
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // 풀에서 하나 꺼내기
    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // 풀이 모자라면 새로 생성 (확장 전략)
            return Instantiate(bulletPrefab, position, rotation);
        }
    }

    // 사용이 끝나면 풀에 반납하기
    public void ReturnBullet(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
```

</details>

---

## ✍️ 복합 퀴즈
1. `Instantiate`와 `Destroy`를 반복할 때 발생하는 '메모리 쓰레기'를 관리하는 시스템의 이름은 무엇인가요?
2. 오브젝트 풀을 사용할 때, 객체를 반납하기 전(SetActive(false))에 반드시 초기화해야 할 데이터는 무엇이 있을까요?
