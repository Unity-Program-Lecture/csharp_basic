# 🚀 Day 11: 유니티 UI 시스템 (UI & UX Implementation)

오늘의 목표는 "**유니티의 UGUI 시스템을 사용하여 게임 화면에 정보를 표시하고, 사용자 인터페이스(UI)를 설계 및 구현한다**"입니다.

---

## 1. UGUI (Unity Graphical User Interface)
유니티에서 UI를 만들기 위한 기본 시스템입니다.

### 📍 핵심 구성 요소
- **Canvas**: UI 요소가 그려지는 도화지. (모든 UI의 부모)
- **Rect Transform**: UI 전용 위치 컴포넌트. (앵커와 피벗 개념 포함)
- **Event System**: 버튼 클릭 등 사용자의 입력을 UI에 전달하는 엔진.

---

## 2. 주요 UI 컨트롤
- **Image**: 아이콘이나 배경 표시.
- **Text (TextMeshPro)**: 고퀄리티 텍스트 출력.
- **Button**: 클릭 이벤트 처리.
- **Slider**: 체력 바(HP Bar)나 게이지 표시.

---

## 💻 실습 예제: 체력 표시 슬라이더 구현
```csharp
using UnityEngine;
using UnityEngine.UI; // UI 시스템 사용을 위해 필수

public class HealthUI : MonoBehaviour
{
    public Slider hpSlider;
    public float maxHP = 100f;
    private float currentHP;

    void Start()
    {
        currentHP = maxHP;
        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        UpdateUI();
    }

    void UpdateUI()
    {
        // 슬라이더의 value는 0~1 사이의 값으로 설정 권장
        hpSlider.value = currentHP / maxHP;
    }
}
```

---

## ✍️ 평가 문항 대비 퀴즈
1. **문제:** 유니티 UI 요소들을 화면에 그리기 위해 반드시 부모로 존재해야 하는 컴포넌트는?
   - **정답:** 캔버스 (Canvas)
2. **문제:** UI 요소의 크기와 위치를 설정할 때, 해상도 변화에 대응하기 위해 사용하는 기준점 설정을 무엇이라 합니까?
   - **정답:** 앵커 (Anchor)
