# 🚀 [알고리즘 02] 알고리즘의 효율성: 시간 복잡도와 Big-O

학습 목표: 코드의 실행 속도가 데이터 양에 따라 어떻게 변하는지(Big-O) 이해하고, 유니티 프로파일러를 통해 성능 병목 지점을 찾는 방법을 배웁니다.

---

## 💡 개념 설명 (NCS 알고리즘: 시간 복잡도 분석)

### 1. 시간 복잡도(Big-O)가 왜 게임에서 중요한가요?
게임은 매초 60번(60 FPS) 화면을 그려야 합니다. 즉, 한 프레임당 주어진 시간은 약 **0.016초**입니다. 만약 내가 짠 알고리즘이 너무 느려서 이 시간을 초과하면 게임이 끊기는 '렉'이 발생합니다.

- **O(1) - 상수 시간**: 데이터가 1개든 100만 개든 속도가 똑같음. (예: 배열의 인덱스로 접근하기)
- **O(N) - 선형 시간**: 데이터가 늘어난 만큼 속도도 비례해서 느려짐. (예: 루프 한 번 돌기)
- **O(N²) - 2차 시간**: 데이터가 조금만 늘어나도 엄청나게 느려짐. (예: 루프 안에 루프가 있는 중첩 반복문)

### 2. 유니티 프로파일러(Profiler)란?
"내 코드가 왜 느리지?"라고 추측만 하는 게 아니라, 실제로 어떤 함수가 CPU를 얼마나 점유하고 있는지 눈으로 확인하는 **돋보기**와 같습니다. 알고리즘 최적화의 첫 걸음은 프로파일러로 정확한 수치를 측정하는 것입니다.

---

## 💻 실습 예제

**미션:** 수많은 오브젝트 중에서 특정 이름을 가진 오브젝트를 찾는 O(N) 알고리즘을 작성하고, 프로파일러에서 확인 가능한 샘플러를 추가하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;
using UnityEngine.Profiling; // 프로파일러 사용을 위한 네임스페이스

public class PerformanceTest : MonoBehaviour
{
    public GameObject[] allObjects; // 10,000개의 오브젝트가 있다고 가정

    void Update()
    {
        // 프로파일러에 "FindObjectAlgorithm"이라는 이름으로 측정 지점 표시
        Profiler.BeginSample("FindObjectAlgorithm");

        string targetName = "TargetCube";
        GameObject foundOne = null;

        // O(N) 알고리즘: 최악의 경우 배열 전체를 다 뒤져야 함
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].name == targetName)
            {
                foundOne = allObjects[i];
                break;
            }
        }

        Profiler.EndSample();
    }
}
```

</details>

---

## ✍️ 정리 및 퀴즈

1. **질문**: 몬스터가 10마리일 때와 1,000마리일 때, 이중 반복문(O(N²))을 사용하면 연산 횟수는 각각 어떻게 변할까요?
2. **질문**: 유니티에서 `Update()` 함수 안에 `GameObject.Find()`를 직접 넣는 것이 왜 성능에 치명적일 수 있는지 시간 복잡도 관점에서 설명해 보세요.
