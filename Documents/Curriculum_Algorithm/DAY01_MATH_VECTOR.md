# 🚀 Day 01: 게임 수학 기초 (벡터와 공간)

오늘의 목표는 **"게임 공간을 구성하는 핵심 수학인 벡터(Vector)를 이해하고, 유니티 엔진에서 오브젝트를 이동시키는 실습을 진행한다"**입니다. 전체 학습의 30%는 이론, 70%는 유니티 엔진 기반 실습으로 진행됩니다.

---

## 1. 💡 이론 (30%): 벡터와 행렬 변환

### 📍 유니티의 3차원 좌표계
유니티는 **왼손 좌표계(Left-handed)**를 사용하며, 각 축은 고유한 색상으로 구분됩니다.

<div align="center">

<svg width="300" height="250" viewBox="0 0 300 250" xmlns="http://www.w3.org/2000/svg">
  <path d="M 50 180 L 250 180" stroke="#ccc" stroke-dasharray="2" />
  <path d="M 150 50 L 150 200" stroke="#ccc" stroke-dasharray="2" />
  <line x1="150" y1="150" x2="150" y2="30" stroke="#2ecc71" stroke-width="4" marker-end="url(#arrow-green)" />
  <text x="155" y="40" fill="#2ecc71" font-weight="bold">Y (Up)</text>
  <line x1="150" y1="150" x2="270" y2="150" stroke="#e74c3c" stroke-width="4" marker-end="url(#arrow-red)" />
  <text x="250" y="145" fill="#e74c3c" font-weight="bold">X (Right)</text>
  <line x1="150" y1="150" x2="100" y2="210" stroke="#3498db" stroke-width="4" marker-end="url(#arrow-blue)" />
  <text x="70" y="225" fill="#3498db" font-weight="bold">Z (Forward)</text>
  <defs>
    <marker id="arrow-red" markerWidth="10" markerHeight="10" refX="0" refY="3" orient="auto" markerUnits="strokeWidth"><path d="M0,0 L0,6 L9,3 z" fill="#e74c3c" /></marker>
    <marker id="arrow-green" markerWidth="10" markerHeight="10" refX="0" refY="3" orient="auto" markerUnits="strokeWidth"><path d="M0,0 L0,6 L9,3 z" fill="#2ecc71" /></marker>
    <marker id="arrow-blue" markerWidth="10" markerHeight="10" refX="0" refY="3" orient="auto" markerUnits="strokeWidth"><path d="M0,0 L0,6 L9,3 z" fill="#3498db" /></marker>
  </defs>
  <circle cx="150" cy="150" r="5" fill="#34495e" />
  <text x="135" y="165" fill="#34495e" font-size="12">Origin (0,0,0)</text>
</svg>

*[그림 1-1] 유니티 엔진의 표준 좌표축 (X:빨강, Y:초록, Z:파랑)*
</div>

### 📍 절대 좌표 vs 상대 좌표
게임 오브젝트가 어디에 있는지 말할 때, 기준이 누구냐에 따라 좌표가 달라집니다.

1. **절대 좌표 (World Space)**: 
   - **기준**: 전체 월드의 중심 (0, 0, 0).
   - **특징**: 변하지 않는 우주의 중심 원점입니다. `transform.position`으로 접근합니다.
2. **상대 좌표 (Local Space)**:
   - **기준**: 나를 감싸고 있는 **부모(Parent)**의 위치.
   - **특징**: 부모가 움직이면 나도 따라 움직이지만, 부모와의 거리는 변하지 않습니다. `transform.localPosition`으로 접근합니다.

<div align="center">

<svg width="450" height="320" viewBox="0 0 450 320" xmlns="http://www.w3.org/2000/svg">
<defs>
<marker id="a-r" markerWidth="8" markerHeight="8" refX="0" refY="3" orient="auto"><path d="M0,0 L0,6 L9,3 z" fill="#e74c3c" /></marker>
<marker id="a-g" markerWidth="8" markerHeight="8" refX="0" refY="3" orient="auto"><path d="M0,0 L0,6 L9,3 z" fill="#2ecc71" /></marker>
<marker id="a-b" markerWidth="8" markerHeight="8" refX="0" refY="3" orient="auto"><path d="M0,0 L0,6 L9,3 z" fill="#3498db" /></marker>
</defs>
<circle cx="80" cy="240" r="4" fill="#34495e" />
<line x1="80" y1="240" x2="140" y2="240" stroke="#e74c3c" stroke-width="2" marker-end="url(#a-r)" />
<line x1="80" y1="240" x2="80" y2="180" stroke="#2ecc71" stroke-width="2" marker-end="url(#a-g)" />
<line x1="80" y1="240" x2="50" y2="280" stroke="#3498db" stroke-width="2" marker-end="url(#a-b)" />
<text x="40" y="230" fill="#34495e" font-size="11" font-weight="bold">World Origin (0,0,0)</text>
<path d="M 220 120 L 260 120 L 260 160 L 220 160 Z" fill="#95a5a6" stroke="#2c3e50" />
<path d="M 220 120 L 240 100 L 280 100 L 260 120 Z" fill="#bdc3c7" stroke="#2c3e50" />
<path d="M 260 120 L 280 100 L 280 140 L 260 160 Z" fill="#7f8c8d" stroke="#2c3e50" />
<text x="180" y="180" fill="#2c3e50" font-size="12" font-weight="bold">Parent (World Pos)</text>
<line x1="240" y1="140" x2="310" y2="140" stroke="#e74c3c" stroke-width="2" stroke-dasharray="3" marker-end="url(#a-r)" />
<line x1="240" y1="140" x2="240" y2="70" stroke="#2ecc71" stroke-width="2" stroke-dasharray="3" marker-end="url(#a-g)" />
<line x1="240" y1="140" x2="200" y2="190" stroke="#3498db" stroke-width="2" stroke-dasharray="3" marker-end="url(#a-b)" />
<circle cx="340" cy="110" r="15" fill="#f1c40f" stroke="#d35400" stroke-width="2" />
<text x="330" y="145" fill="#d35400" font-size="12" font-weight="bold">Child</text>
<line x1="240" y1="140" x2="330" y2="115" stroke="#3498db" stroke-width="3" marker-end="url(#a-b)" />
<text x="250" y="115" fill="#2980b9" font-size="11" font-weight="bold" transform="rotate(-15, 250, 115)">상대 위치 (Local Pos)</text>
<path d="M 80 240 Q 150 240 235 155" fill="none" stroke="#95a5a6" stroke-width="1" stroke-dasharray="4" marker-end="url(#a-r)" />
<text x="90" y="210" fill="#7f8c8d" font-size="11" font-style="italic">World Pos = Parent + Local</text>
</svg>

*[그림 1-2] 3D 공간에서의 절대 좌표와 상대 좌표의 관계*
</div>

> 🚌 **비유**: 달리는 버스 안에서 내가 앞으로 한 걸음(Local +1m) 걸어갔을 때, 나의 실제 위치(World)는 버스가 달린 거리까지 포함한 값이 됩니다.

### 📍 벡터(Vector)란 무엇인가?
벡터는 공간에서 **'크기(Magnitude)'**와 **'방향(Direction)'**을 동시에 가진 화살표와 같습니다. 유니티에서 캐릭터의 위치, 이동할 방향, 가할 힘 등을 모두 이 벡터로 표현합니다.

- **성분 표현**: 3D 공간에서는 (x, y, z)라는 세 개의 숫자로 벡터를 나타냅니다.
- **예시**: (2, 3, 4) 벡터는 "원점에서 X축으로 2, Y축으로 3, Z축으로 4만큼 이동한 지점을 가리키는 화살표"입니다.

<div align="center">
<svg width="400" height="300" viewBox="0 0 400 300" xmlns="http://www.w3.org/2000/svg">
<defs>
<marker id="v-r" markerWidth="8" markerHeight="8" refX="0" refY="3" orient="auto"><path d="M0,0 L0,6 L9,3 z" fill="#e74c3c" /></marker>
<marker id="v-g" markerWidth="8" markerHeight="8" refX="0" refY="3" orient="auto"><path d="M0,0 L0,6 L9,3 z" fill="#2ecc71" /></marker>
<marker id="v-b" markerWidth="8" markerHeight="8" refX="0" refY="3" orient="auto"><path d="M0,0 L0,6 L9,3 z" fill="#3498db" /></marker>
<marker id="v-main" markerWidth="10" markerHeight="10" refX="0" refY="3" orient="auto"><path d="M0,0 L0,6 L9,3 z" fill="#f1c40f" /></marker>
</defs>
<!-- Axes -->
<line x1="150" y1="200" x2="300" y2="200" stroke="#e74c3c" stroke-width="2" marker-end="url(#v-r)" />
<line x1="150" y1="200" x2="150" y2="50" stroke="#2ecc71" stroke-width="2" marker-end="url(#v-g)" />
<line x1="150" y1="200" x2="80" y2="270" stroke="#3498db" stroke-width="2" marker-end="url(#v-b)" />
<!-- Guides for (2, 3, 4) -->
<path d="M 230 200 L 230 110 L 150 110" fill="none" stroke="#95a5a6" stroke-width="1" stroke-dasharray="4" />
<path d="M 230 110 L 190 150 L 110 150" fill="none" stroke="#95a5a6" stroke-width="1" stroke-dasharray="4" />
<path d="M 190 150 L 190 240" fill="none" stroke="#95a5a6" stroke-width="1" stroke-dasharray="4" />
<!-- The Point P(2,3,4) -->
<circle cx="190" cy="150" r="5" fill="#f1c40f" stroke="#d35400" />
<text x="200" y="145" fill="#2c3e50" font-size="12" font-weight="bold">P (2, 3, 4)</text>
<!-- Main Vector Arrow -->
<line x1="150" y1="200" x2="185" y2="155" stroke="#f1c40f" stroke-width="4" marker-end="url(#v-main)" />
<!-- Labels -->
<text x="270" y="215" fill="#e74c3c" font-size="12">X</text>
<text x="135" y="45" fill="#2ecc71" font-size="12">Y</text>
<text x="65" y="260" fill="#3498db" font-size="12">Z</text>
<text x="120" y="190" fill="#7f8c8d" font-size="11">Origin</text>
</svg>

*[그림 1-3] 공간상의 한 점(2, 3, 4)을 가리키는 벡터 화살표*
</div>

---

## 2. 💻 실습 (70%): 유니티 Transform 제어
**미션:** 유니티 엔진의 `Transform` 컴포넌트와 `Vector3` 구조체를 활용하여 플레이어 오브젝트를 키보드 입력에 따라 이동시키세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // 1. 사용자 입력 받기 (수학적 벡터 방향 생성)
        float h = Input.GetAxis("Horizontal"); // X축
        float v = Input.GetAxis("Vertical");   // Z축

        // 2. 방향 벡터 만들기
        Vector3 moveDir = new Vector3(h, 0, v).normalized; // 크기를 1로 정규화

        // 3. 실제 이동 처리 (벡터 * 스칼라(속도) * 시간)
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
    }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 3D 공간에서 물체의 위치나 방향을 나타낼 때 사용하는 수학적 개념은 무엇입니까?
   - **정답:** 벡터(Vector)
2. **문제:** 부모 오브젝트의 위치를 기준으로 하는 자식 오브젝트의 좌표계를 무엇이라 합니까?
   - **정답:** 상대 좌표계 (Local Space)
