# 🚀 Day 05: 구조체 (Struct) - "가볍고 빠른 데이터 상자"

오늘의 목표는 "**클래스와 비슷하지만 다른 구조체(Struct)를 배우고, 값 형식의 데이터 복사 원리를 이해한다**"입니다.

---

## 1. 구조체(Struct)란? : "작고 가벼운 데이터 묶음"
구조체는 여러 개의 변수를 하나로 묶어주는 도구입니다. 클래스와 매우 비슷해 보이지만, 메모리 사용 방식과 동작 원리가 다릅니다.
- **비유**: 클래스가 거대한 '설계도'라면, 구조체는 작고 가벼운 '포스트잇'이나 '쪽지'와 같습니다.
- **용도**: 좌표(X, Y), 색상(R, G, B), 크기(W, H) 등 작고 단순한 데이터를 다룰 때 주로 씁니다.

```csharp
public struct Point 
{
    public int x;
    public int y;

    // 생성자도 가질 수 있습니다!
    public Point(int x, int y) 
    {
        this.x = x;
        this.y = y;
    }
}
```

---

## 2. 구조체의 특징 (클래스와의 차이점)
1. **값 형식(Value Type)**: 상자를 복사하면 내용물이 통째로 복사됩니다. (원본은 안전!)
2. **스택(Stack) 메모리**: 힙(Heap)을 거치지 않고 스택에서 빠르고 깔끔하게 처리됩니다.
3. **상속 불가**: 다른 구조체나 클래스로부터 상속을 받을 수 없습니다.

```csharp
// --- 1. 구조체 (값 형식) : "내용물 복사" ---
Point p1 = new Point(10, 10);
Point p2 = p1; // p1의 내용이 p2로 통째로 복사됨 (별개의 상자)
p2.x = 20;     // p2를 바꿔도 p1은 그대로 10!

// --- 2. 클래스 (참조 형식) : "주소 복사" ---
public class PointClass { public int x; }

PointClass c1 = new PointClass { x = 10 };
PointClass c2 = c1; // c1의 '위치 주소'가 c2로 복사됨 (같은 상자를 가리킴)
c2.x = 20;          // c2를 바꾸면? c1.x도 20으로 바뀜! (충격)

// 3. 상속 불가 (구조체만의 특징)
// public struct SuperPoint : Point { } 
```

---

## 💻 실습 예제: 위치 정보(Vector2) 관리
**미션:** 2차원 좌표를 담는 `Position` 구조체를 만들고, 값을 복사했을 때 원본이 유지되는지 확인해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine;

public struct Position
{
    public int x;
    public int y;

    public void Show()
    {
        Debug.Log($"현재 좌표: ({x}, {y})");
    }
}

public class Day05_Practice : MonoBehaviour
{
    void Start()
    {
        Position p1 = new Position { x = 10, y = 20 };
        
        // 값 복사 발생! (상자 자체가 하나 더 생김)
        Position p2 = p1; 
        p2.x = 99;

        Debug.Log("[원본 p1]");
        p1.Show(); // 결과: (10, 20)
        
        Debug.Log("[복사본 p2]");
        p2.Show(); // 결과: (99, 20)
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. 구조체는 값 형식인가요, 참조 형식인가요?
2. 구조체는 상속이 가능한가요?
3. 유니티에서 위치 정보를 나타내는 `Vector3`는 클래스일까요, 구조체일까요? (힌트: 가볍게 자주 쓰임)

---

## 🎯 종합 연습 문제

### [심화 미션: 몬스터 사냥 시스템 (Level 4)]
4일차 시스템에 **구조체**를 도입하여 몬스터의 보상 정보와 위치 정보를 추가합니다.

**[요구 사항]**
1. 보상 정보를 담는 `Reward` 구조체를 만드세요. (필드: `string itemName`, `int gold`)
2. 위치 정보를 담는 `Point` 구조체를 만드세요. (필드: `int x`, `int y`)
3. `Monster` 클래스에 `Reward`와 `Point` 타입의 멤버 변수를 추가하세요.
4. 몬스터 생성 시 각각 다른 위치와 보상을 설정합니다.
5. 몬스터가 죽었을 때(`HP == 0`), 해당 몬스터의 위치(`x, y`)와 드랍 아이템, 골드 정보를 출력하는 기능을 추가하세요.
   - 예: `(10, 5) 위치에서 '슬라임' 처치! [보상: 동전 100개, 물약 1개]`

**[프로그래밍 힌트]**
- 구조체는 클래스 내부에서 일반 변수처럼 선언하여 사용할 수 있습니다.
- `Debug.Log($"({pos.x}, {pos.y}) ...")`와 같이 문자열 보간법을 활용해 보세요.

