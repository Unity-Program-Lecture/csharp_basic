# 🚀 Day 02: 함수(Method) - "마법의 자판기"

오늘의 목표는 **"반복되는 코드를 묶어 재사용하는 함수의 개념을 이해하고, 입력(매개변수)과 출력(반환값)을 마스터한다"**입니다.

---

## 1. 함수(Method)란? : "코드 묶음 자판기"
함수는 특정한 작업을 수행하는 코드들을 하나로 묶어 이름을 붙인 것입니다.
- **비유**: 동전(입력)을 넣고 버튼을 누르면 음료수(결과)가 나오는 자판기와 같습니다.
- **장점**: 똑같은 코드를 여러 번 쓸 필요가 없고, 수정이 쉽습니다.

---

## 2. 함수의 구조
```csharp
반환타입 함수이름(매개변수)
{
    // 실행할 코드
    return 결과값;
}
```
1. **반환 타입**: 결과물로 무엇을 줄 것인가? (없으면 `void`)
2. **함수 이름**: 이 자판기를 뭐라고 부를 것인가? (보통 동사로 시작)
3. **매개변수(Parameter)**: 작업을 위해 필요한 재료는 무엇인가?
4. **return**: 결과를 밖으로 내보내고 함수를 종료합니다.

---

## 3. 입력과 출력의 조합
- **입력 O, 출력 O**: `int Add(int a, int b) { return a + b; }`
- **입력 O, 출력 X**: `void PrintName(string name) { Console.WriteLine(name); }`
- **입력 X, 출력 O**: `int GetRandom() { return 7; }`
- **입력 X, 출력 X**: `void SayHello() { Console.WriteLine("안녕!"); }`

---

## 💻 실습 예제: 캐릭터 전투 메시지 함수
**미션:** 공격자의 이름과 데미지를 받아 공격 메시지를 출력하는 함수를 만들고 사용해 보세요.

<details>
<summary>코드 보기</summary>

```csharp
using System;

class Program
{
    // 공격 메시지를 출력하는 함수 (입력 O, 출력 X)
    static void ShowAttack(string attacker, int damage)
    {
        Console.WriteLine($"{attacker}이(가) {damage}의 데미지로 공격합니다!");
    }

    // 데미지를 계산해주는 함수 (입력 O, 출력 O)
    static int CalculateDamage(int power, int level)
    {
        return power * level;
    }

    static void Main()
    {
        int myDamage = CalculateDamage(10, 5); // 50 계산
        ShowAttack("전사", myDamage);          // 메시지 출력
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. 결과값을 돌려주지 않는 함수의 반환 타입은 무엇인가요?
2. 함수를 종료하고 결과값을 밖으로 내보낼 때 사용하는 키워드는?
3. 함수 정의 부분에 적는 재료의 이름을 '매개변수'라고 한다면, 실제로 함수를 호출할 때 넣는 값은 무엇이라고 부를까요? (힌트: ㅇㅅ)
