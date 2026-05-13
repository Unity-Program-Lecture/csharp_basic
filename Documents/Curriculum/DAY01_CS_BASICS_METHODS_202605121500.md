# 🚀 Day 01: 변수, 메모리 그리고 함수

오늘의 목표는 **"데이터를 담는 변수와 이를 처리하는 함수의 개념을 익히고, 메모리 구조와 실행 원리를 마스터한다"**입니다.

---

## 1. 변수(Variable): "데이터를 담는 상자"
프로그래밍에서 가장 먼저 배우는 것은 '기억'하는 법입니다. 변수는 메모리에 이름을 붙여 데이터를 저장하는 상자입니다.

```csharp
// 변수 선언과 데이터 담기
int age = 20;            // 정수 상자
string name = "제미니";   // 문장 상자
float height = 180.5f;   // 소수점 상자

Debug.Log(name);         // 상자 안의 내용물 꺼내보기 (콘솔 출력)
```

---

## 2. 데이터 타입의 크기: "상자의 종류"
컴퓨터는 모든 데이터를 **0과 1(Bit)**로 기억합니다.
- **int (4바이트)**: 정수 상자.
- **float (4바이트)**: 소수점 상자.
- **bool (1바이트)**: 참/거짓 상자.
- **string (가변)**: 문장 상자.

---

## 3. 실행 원리와 메모리 (요리사와 조리대)
- **CPU (요리사)**: 명령을 실행합니다.
- **메모리 (조리대)**: 데이터와 코드를 잠시 올려두는 곳입니다.
- **스택 (Stack)**: 빠르고 자동입니다. 기본 숫자 타입을 담습니다.
- **힙 (Heap)**: 넓지만 관리가 필요합니다. 실제 객체와 문장을 담습니다.

---

## 4. 함수(Method): "코드 묶음 자판기"
함수는 특정한 작업을 수행하는 코드들을 하나로 묶어 이름을 붙인 것입니다.
- **비유**: 동전(입력)을 넣고 버튼을 누르면 음료수(결과)가 나오는 자판기와 같습니다.

```csharp
// 1. 함수 정의 (자판기 만들기)
void SayHello(string name)
{
    Debug.Log($"안녕하세요, {name}님!");
}

// 2. 함수 호출 (자판기 사용)
SayHello("플레이어"); 
```

---

## 💻 실습 예제: 캐릭터 정보와 공격력 계산
```csharp
using UnityEngine;

public class Day01_Practice : MonoBehaviour
{
    // 공격력을 계산해서 돌려주는 함수 (입력 O, 출력 O)
    int CalculateDamage(int power, int level)
    {
        return power * level;
    }

    // 메시지를 출력하는 함수 (입력 O, 출력 X)
    void ShowMessage(string name, int damage)
    {
        Debug.Log($"{name}이(가) {damage}의 데미지로 공격합니다!");
    }

    void Start()
    {
        // 1. 변수 선언 (데이터 담기)
        string playerName = "Gemini";
        int basePower = 10;
        int level = 5;
        
        // 2. 함수 호출 (데이터 처리)
        int finalDamage = CalculateDamage(basePower, level);
        ShowMessage(playerName, finalDamage);
        
        // 3. 값 복사 확인 (값 타입의 특징)
        int a = 100;
        int b = a; 
        b -= 50;
        Debug.Log($"a: {a}, b: {b}"); // 결과: a=100, b=50 (원본 a는 유지됨)
    }
}
```

---

## ✍️ 핵심 퀴즈
1. `new` 키워드로 만든 객체는 어느 메모리 구역에 저장되나요?
2. 결과값을 돌려주지 않는 함수의 반환 타입은 무엇인가요?
3. 함수 정의 부분에 적는 재료의 이름을 '매개변수'라고 한다면, 호출할 때 넣는 실젯값은?
