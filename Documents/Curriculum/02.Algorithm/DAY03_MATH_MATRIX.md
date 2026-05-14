# 🚀 Day 03: 게임 수학 - 행렬과 변환 (Matrix & Transform)

오늘의 목표는 "**공간의 변환을 담당하는 행렬(Matrix)의 원리를 이해하고, 월드(World)와 로컬(Local) 좌표계 간의 변환 과정을 마스터한다**"입니다.

---

## 1. 행렬(Matrix)이란? : "공간의 지도"
행렬은 여러 개의 숫자를 사각형 모양으로 배열한 것입니다. 게임에서는 주로 4x4 행렬을 사용하여 오브젝트의 **위치(Translation)**, **회전(Rotation)**, **크기(Scale)** 정보를 하나로 합쳐서 관리합니다.

### 📍 행렬 연산의 의미
- **행렬의 곱**: 두 변환을 합치는 과정입니다. (예: 회전시킨 후 이동하기)
- **변환 순서(SRT)**: 유니티를 포함한 대부분의 엔진은 **Scale -> Rotation -> Translation** 순서로 행렬을 곱하여 최종 위치를 계산합니다. (순서가 바뀌면 결과가 달라집니다!)

---

## 2. 좌표계 변환 (Coordinate Space Transformation)
오브젝트가 렌더링되기 위해서는 여러 좌표계를 거쳐야 합니다.

1. **로컬 공간 (Local Space)**: 오브젝트 자신의 중심이 (0,0,0)인 공간. (`transform.localPosition`)
2. **월드 공간 (World Space)**: 게임 세상의 절대 원점이 기준인 공간. (`transform.position`)
3. **뷰 공간 (View Space)**: 카메라가 원점이 되는 공간.
4. **투영 공간 (Projection Space)**: 3D 공간을 2D 화면으로 찌그러뜨린 공간.

> 💡 **핵심**: 부모-자식 관계에서 자식의 월드 위치는 **[부모의 월드 행렬] x [자식의 로컬 행렬]**로 계산됩니다.

---

## 💻 실습 예제: 행렬을 이용한 좌표 변환 시뮬레이션
유니티의 `Matrix4x4` 클래스를 활용하여 로컬 좌표를 월드 좌표로 직접 계산해 봅니다.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class MatrixTest : MonoBehaviour
{
    void Start()
    {
        // 1. 현재 오브젝트의 변환 행렬 가져오기
        Matrix4x4 worldMatrix = transform.localToWorldMatrix;

        // 2. 로컬 상의 특정 점 (예: 내 오른쪽으로 2미터 지점)
        Vector3 localPos = new Vector3(2f, 0, 0);

        // 3. 행렬 곱을 통해 월드 좌표로 변환
        Vector3 worldPos = worldMatrix.MultiplyPoint3x4(localPos);

        Debug.Log($"나의 월드 위치: {transform.position}");
        Debug.Log($"로컬(2,0,0) 지점의 실제 월드 위치: {worldPos}");
        
        // 시각적으로 확인 (기즈모 사용 권장)
        Debug.DrawLine(transform.position, worldPos, Color.yellow, 10f);
    }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 오브젝트의 이동, 회전, 크기 변환 정보를 하나의 수식으로 처리하기 위해 사용하는 수학적 도구는 무엇입니까?
   - **정답:** 행렬 (Matrix)
2. **문제:** 유니티에서 자식 오브젝트의 좌표를 월드 좌표로 변환할 때 사용하는 행렬 프로퍼티 이름은?
   - **정답:** `transform.localToWorldMatrix`
