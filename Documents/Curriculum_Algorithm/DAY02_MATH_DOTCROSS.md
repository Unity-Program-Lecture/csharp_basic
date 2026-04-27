# 🚀 Day 02: 게임 수학 심화 (내적/외적과 회전)

오늘의 목표는 **"벡터의 내적과 외적을 통해 적의 시야를 판별하고, 쿼터니언(Quaternion)을 이용해 짐벌락 현상 없이 물체를 회전시키는 법을 마스터한다"**입니다.

---

## 1. 💡 이론 (30%): 벡터 연산과 쿼터니언

### 📍 내적 (Dot Product): "너 내 시야에 있니?"
두 벡터의 내적은 타겟이 나의 정면을 기준으로 어느 각도에 있는지 판별하는 데 사용됩니다.

<div align="center">
<svg width="300" height="200" viewBox="0 0 300 200" xmlns="http://www.w3.org/2000/svg">
  <!-- Monster Forward -->
  <line x1="150" y1="180" x2="150" y2="80" stroke="#3498db" stroke-width="4" marker-end="url(#arrow-blue)" />
  <text x="155" y="90" fill="#3498db" font-weight="bold">Forward (A)</text>
  <!-- To Player -->
  <line x1="150" y1="180" x2="250" y2="100" stroke="#e74c3c" stroke-width="4" marker-end="url(#arrow-red)" />
  <text x="220" y="140" fill="#e74c3c" font-weight="bold">To Player (B)</text>
  <!-- Angle Arc -->
  <path d="M 150 140 A 40 40 0 0 1 180 155" fill="none" stroke="#f1c40f" stroke-width="3" />
  <text x="165" y="135" fill="#f1c40f" font-weight="bold">θ</text>
  <!-- Formula -->
  <text x="50" y="50" fill="#2c3e50" font-size="14" font-weight="bold">A · B = |A||B| cos θ</text>
  <defs>
    <marker id="arrow-red" markerWidth="10" markerHeight="10" refX="0" refY="3" orient="auto"><path d="M0,0 L0,6 L9,3 z" fill="#e74c3c" /></marker>
    <marker id="arrow-blue" markerWidth="10" markerHeight="10" refX="0" refY="3" orient="auto"><path d="M0,0 L0,6 L9,3 z" fill="#3498db" /></marker>
  </defs>
</svg>
<p><i>[그림 2-1] 벡터 내적을 이용한 각도 판별</i></p>
</div>

- **활용**: 몬스터 전방 벡터와 플레이어 방향 벡터의 내적값이 **0.5 이상이면 시야각 60도 이내**로 판단할 수 있습니다.

### 📍 외적 (Cross Product): "수직인 뿔 만들기"
두 벡터에 모두 수직인 세 번째 벡터를 구합니다. 유니티에서는 주로 **좌우 판별**이나 **표면의 방향(Normal)**을 구할 때 씁니다.

---

## 2. 💻 실습 (70%): 적 시야 판별 및 부드러운 회전
**미션:** 몬스터가 플레이어를 향해 부드럽게 회전하고, 플레이어가 몬스터의 시야각(전방 60도) 내에 들어왔는지 내적(`Vector3.Dot`)을 이용해 판별하세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public class MonsterSight : MonoBehaviour
{
    public Transform player;
    public float sightAngle = 60f; // 시야각 (좌우 합쳐 120도)
    public float rotSpeed = 3f;

    void Update()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;

        // 1. 내적을 이용한 시야 판별
        float dot = Vector3.Dot(transform.forward, dirToPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg; // 아크코사인으로 각도 추출

        if (angle < sightAngle)
        {
            Debug.Log("플레이어 발견!");
            
            // 2. 쿼터니언을 이용한 부드러운 회전(Slerp)
            Quaternion targetRot = Quaternion.LookRotation(dirToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
        }
    }
}
```

</details>

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 두 벡터 사이의 각도를 구하거나, 타겟이 내 앞/뒤 어디에 있는지 판별할 때 주로 쓰이는 수학 연산은?
   - **정답:** 내적 (Dot Product)
2. **문제:** 3D 회전 시 축이 겹쳐 회전 자유도를 잃는 현상을 방지하기 위해 유니티 엔진에서 내부적으로 사용하는 회전 체계는?
   - **정답:** 쿼터니언 (Quaternion)
