# 🚀 DAY16: Procedural Generation - 절차적 맵 생성 기초
_최종 수정일: 202606180900_

## 🚀 학습 목표
- 무작위성(Randomness)과 절차적 생성(Procedural Generation)의 차이를 이해합니다.
- 단순 무작위 생성을 넘어, 규칙이 있는 알고리즘 맵 생성의 원리를 배웁니다.

---

## 💡 개념 설명: 무작위 vs 절차적
- **단순 무작위:** 그냥 주사위를 던지는 것과 같습니다. 땅이 있어야 할 곳에 바다가 생기거나, 맵이 끊기는 등 엉망진창이 될 수 있습니다.
- **절차적 생성:** "주변에 땅이 3개 이상이면 여기도 땅으로 만들어!" 같은 **규칙(알고리즘)**을 가지고 맵을 만듭니다. 덕분에 매번 다르면서도 사람이 만든 것 같은 자연스러운 맵이 나옵니다.
- **핵심 기술:**
    1. **Perlin Noise:** 부드러운 언덕이나 지형을 만들 때 사용합니다.
    2. **Cellular Automata:** 동굴 구조를 만들 때 자주 사용합니다.

---

## 💻 실습 예제: Simple Perlin Map

**미션:** 2차원 배열과 `Mathf.PerlinNoise`를 활용하여 높낮이가 있는 지형 데이터 맵을 생성하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public float scale = 10f; // 노이즈의 조밀함 조절

    void Start() => GenerateMap();

    void GenerateMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 0.0 ~ 1.0 사이의 값 반환
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                // 샘플 값에 따라 타일 생성 결정
                if (sample > 0.5f)
                    CreateTile(x, y, Color.green); // 육지
                else
                    CreateTile(x, y, Color.blue);  // 바다
            }
        }
    }

    void CreateTile(int x, int y, Color color)
    {
        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tile.transform.position = new Vector3(x, 0, y);
        tile.GetComponent<Renderer>().material.color = color;
    }
}
```

</details>

---

## ✍️ 복합 퀴즈
1. `Random.Range`와 `Mathf.PerlinNoise`의 결정적인 차이점은 무엇인가요? (힌트: 연속성)
2. 절차적 생성 알고리즘을 사용할 때, 항상 같은 결과의 맵을 다시 생성하고 싶다면 어떤 값을 고정해야 할까요?
