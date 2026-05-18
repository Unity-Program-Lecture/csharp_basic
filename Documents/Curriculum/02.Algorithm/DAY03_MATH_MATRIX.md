# 🚀 Day 03: 게임 수학 - 행렬과 변환 (Matrix & Transform)

오늘의 목표는 "**공간의 변환을 담당하는 행렬(Matrix)의 원리를 이해하고, 월드(World)와 로컬(Local) 좌표계 간의 변환 과정을 마스터한다**"입니다.

---

## 1. 행렬(Matrix)이란? : "공간의 지도"
행렬은 여러 개의 숫자를 사각형 모양으로 배열한 것입니다. 게임에서는 주로 4x4 행렬을 사용하여 오브젝트의 **위치** (Translation), **회전** (Rotation), **크기** (Scale) 정보를 하나로 합쳐서 관리합니다.

### 📍 행렬 연산의 의미
- **행렬의 곱**: 두 변환을 합치는 과정입니다. (예: 회전시킨 후 이동하기)
- **변환 순서 (SRT)**: 유니티를 포함한 대부분의 엔진은 **Scale -> Rotation -> Translation** 순서로 행렬을 곱하여 최종 위치를 계산합니다. 순서가 바뀌면 오브젝트가 찌그러지거나 원하지 않는 궤도로 이동할 수 있습니다.

#### 💡 왜 SRT 순서인가요? (유니티 Transform 예시)
1. **Scale (크기)**: 먼저 오브젝트 자체의 크기를 결정합니다. (`transform.localScale`)
   - 만약 이동(T) 후에 크기(S)를 키우면, 원점으로부터의 거리까지 함께 커져버려 오브젝트가 엉뚱한 곳으로 날아갑니다.
2. **Rotation (회전)**: 결정된 크기를 바탕으로 제자리에서 회전합니다. (`transform.localRotation`)
   - 만약 이동(T) 후에 회전(R)을 하면, 오브젝트가 자신의 중심이 아닌 '세상의 중심(원점)'을 기준으로 크게 원을 그리며 회전하게 됩니다.
3. **Translation (이동)**: 크기와 회전이 완료된 오브젝트를 최종 목적지로 옮깁니다. (`transform.localPosition`)
   - 모든 로컬 변형이 끝난 상태에서 옮겨야 우리가 의도한 "그 자리"에 정확히 배치됩니다.

---

## 2. 좌표계 변환 (Coordinate Space Transformation)
오브젝트가 렌더링되기 위해서는 여러 좌표계를 거쳐야 합니다.

1. **로컬 공간** (Local Space): 오브젝트 자신의 중심이 (0,0,0)인 공간. (`transform.localPosition`)
2. **월드 공간** (World Space): 게임 세상의 절대 원점이 기준인 공간. (`transform.position`)
3. **뷰 공간** (View Space): 카메라가 원점이 되는 공간.
4. **투영 공간** (Projection Space): 3D 공간을 2D 화면으로 찌그러뜨린 공간.

> 💡 **핵심**: 부모-자식 관계에서 자식의 월드 위치는 **[부모의 월드 행렬] x [자식의 로컬 행렬]**로 계산됩니다.

---

## 💻 실습 예제: 행렬을 이용한 좌표 변환 시뮬레이션
유니티의 `Matrix4x4` 클래스를 활용하여 로컬 좌표를 월드 좌표로 직접 계산해 봅니다. 에디터에서 오브젝트를 움직여보며 실시간으로 변화를 확인하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class MatrixTest : MonoBehaviour
{
    // 기즈모를 사용하여 에디터 뷰에서 실시간으로 확인
    void OnDrawGizmos()
    {
        // 1. 현재 오브젝트의 로컬->월드 변환 행렬 가져오기
        // (내부적으로 Translation * Rotation * Scale이 결합된 상태)
        Matrix4x4 worldMatrix = transform.localToWorldMatrix;

        // 2. 로컬 상의 특정 점 (예: 내 오른쪽으로 2미터 지점)
        Vector3 localPos = new Vector3(2f, 0, 0);

        // 3. 행렬 곱을 통해 월드 좌표로 변환
        // MultiplyPoint3x4는 4x4 행렬을 3D 점에 적용하는 가장 일반적인 방법입니다.
        Vector3 worldPos = worldMatrix.MultiplyPoint3x4(localPos);

        // 시각화
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, worldPos); // 원점에서 변환된 점까지 선 그리기
        Gizmos.DrawSphere(worldPos, 0.1f);            // 변환된 최종 위치에 구체 그리기
        
        // 결과 출력 (프레임마다 찍히지 않도록 필요 시에만 사용)
        // Debug.Log($"로컬(2,0,0) -> 월드({worldPos})");
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

---

## 📎 별첨 1: 월드 좌표에서 화면(Screen)까지의 여정 (심화)

3D 세상의 한 점이 모니터 픽셀로 변환되는 과정은 **그래픽스 파이프라인** (Graphics Pipeline)의 핵심이며, 총 4단계의 수학적 변환을 거칩니다.

### 1단계: 뷰 변환 (View Transform)
- **공간 이동:** 월드 공간 -> **뷰 공간** (View/Eye Space)
- **상세 설명:** 카메라를 시점의 원점으로 만듭니다. 카메라의 월드 행렬의 역행렬을 곱하여 모든 물체를 카메라 기준으로 재정렬합니다.
- **핵심:** `P_view = ViewMatrix * P_world`

### 2단계: 투영 변환 (Projection Transform) - 🔍 심층 분석
- **공간 이동:** 뷰 공간 -> **클립 공간** (Clip Space)
- **수학적 목적:** 카메라의 시야 범위인 피라미드 모양의 **절두체** (Frustum) 공간을, 연산하기 편한 정육면체 모양의 **정규 뷰 볼륨** (Canonical View Volume)으로 매핑하는 단계입니다.
- **왜 상자로 만드나요?** 상자 모양 (NDC)으로 옮겨놓아야 화면 밖의 물체를 잘라내는 (Clipping) 계산이 매우 빠르고 효율적이기 때문입니다.
- **원근감의 비밀:** 멀리 있는 물체는 **x, y 좌표**를 좁게 모으고, **w 값**에 카메라로부터의 거리 정보를 저장하여 나중에 원근 나눗셈을 할 준비를 마칩니다.
- **핵심 파라미터:** FOV (시야각), Aspect Ratio (가로세로비), Near/Far (절단면) 설정이 이 행렬의 모양을 결정합니다.

### 3단계: 투영 분할 (Perspective Division)
- **공간 이동:** 클립 공간 -> **NDC 공간** (Normalized Device Coordinates)
- **상세 설명:** 4차원 벡터의 x, y, z를 w로 나눕니다. 이 나눗셈을 통해 멀리 있는 물체는 작아지고 가까운 물체는 커지는 실제 원근법이 시각적으로 완성됩니다. 모든 좌표는 -1 ~ 1 사이로 고정됩니다.
- **핵심:** `P_ndc = (x/w, y/w, z/w)`

### 4단계: 뷰포트 변환 (Viewport Transform)
- **공간 이동:** NDC 공간 -> **화면 공간** (Screen/Pixel Space)
- **상세 설명:** -1 ~ 1 사이의 비율 값을 실제 모니터 해상도 (예: 1920x1080)의 픽셀 위치로 매핑합니다.
- **수식:** `Pixel_X = (x + 1) * Width / 2`, `Pixel_Y = (1 - y) * Height / 2`

---

## 📎 별첨 2: 직교 투영 (Orthographic Projection)

모든 카메라가 원근감을 가지는 것은 아닙니다. **직교 투영**은 거리와 상관없이 물체의 크기를 일정하게 유지하는 방식입니다.

### 1. 주요 특징
- **평행 투영:** 모든 투영선이 카메라 방향과 평행합니다. 따라서 멀리 있는 물체와 가까이 있는 물체의 크기가 화면상에서 동일하게 보입니다.
- **시야 공간:** 절두체(Frustum)가 아닌 **직육면체(Box)** 형태의 시야 공간을 가집니다.
- **w 값의 역할:** 직교 투영 행렬은 `w` 값을 보통 `1`로 고정하거나 거리와 무관하게 처리합니다. 즉, 3단계의 **원근 나눗셈(Perspective Division)**을 해도 크기 변화가 없습니다.

### 2. 언제 사용하나요?
- **2D 게임:** 캐릭터나 배경의 깊이와 상관없이 일관된 크기로 보여야 할 때.
- **UI / HUD:** 화면 위에 고정된 정보를 표시할 때.
- **쿼터뷰 / 아이소메트릭:** 《디아블로》나 《문명》처럼 일정한 비율의 부감 시점을 유지해야 할 때.
- **CAD / 설계:** 치수와 비율이 정밀하게 유지되어야 하는 도면 제작 시.

### 3. 비교 요약
| 구분 | 원근 투영 (Perspective) | 직교 투영 (Orthographic) |
| :--- | :--- | :--- |
| **시야 모양** | 절두체 (피라미드) | 직육면체 (박스) |
| **거리와 크기** | 멀수록 작아짐 (원근감 있음) | 거리에 상관없이 일정 (원근감 없음) |
| **주요 용도** | 3D 액션, 레이싱, VR/AR | 2D 플랫포머, UI, 시뮬레이션 |
| **유니티 설정** | Camera - Projection - Perspective | Camera - Projection - Orthographic |

---

### 📌 요약: 최종 렌더링 공식
> `P_screen = M_viewport * (M_projection * M_view * M_world * P_local) / w`

이 과정을 통해 3D 월드의 한 점이 우리가 보는 2D 화면의 정확한 픽셀 위치로 결정됩니다.
