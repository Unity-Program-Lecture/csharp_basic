# 🚀 Day 02: 배열과 제어 흐름 (데이터 묶음과 판단)

오늘의 목표는 "**데이터를 연속적으로 담는 배열을 배우고, 조건에 따라 실행을 제어하는 판단과 반복을 마스터한다**"입니다.

---

## 1. 배열(Array): "연속된 데이터 상자"
배열은 동일한 타입의 데이터를 메모리에 다닥다닥 붙여서 저장하는 기초 자료구조입니다.
- **특징**: 0번부터 시작하며, 한 번 정한 크기는 바꿀 수 없습니다.
- **속성**: `Length`를 통해 배열이 총 몇 칸인지 알 수 있습니다.

### 📦 배열의 선언과 여러 가지 초기화 방법
```csharp
// 방법 1: 크기를 먼저 정하고 나중에 값 넣기
int[] scores = new int[3]; 
scores[0] = 80;
scores[1] = 90;
scores[2] = 100;

// 방법 2: 선언과 동시에 값 나열하기 (가장 많이 씀)
string[] monsters = { "슬라임", "오크", "골렘" };

// 방법 3: new 키워드와 함께 값 나열하기
float[] positions = new float[] { 1.5f, 2.0f, 3.5f };

// 방법 4: 크기와 값을 동시에 지정 (크기가 맞지 않으면 에러!)
int[] levels = new int[2] { 10, 20 };

Debug.Log($"첫 번째 몬스터: {monsters[0]}");
Debug.Log($"총 점수 데이터 수: {scores.Length}");
```

---

## 2. 연산자 (Operator): "계산 도구"
- **산술**: `+`, `-`, `*`, `/`, `%`(나머지)
- **비교**: `==`(같다), `!=`(다르다), `>`, `<`
- **논리**: `&&`(AND: 둘 다 참), `||`(OR: 하나라도 참), `!`(NOT: 참거짓 반전)

```csharp
int hp = 100;
int mp = 50;
bool hasItem = true;

// 1. 산술 및 비교 연산
int damage = 10 + 5;
bool isLowHP = hp < 30;

// 2. 논리 연산 예시
// AND (&&): 둘 다 맞아야 함 (체력이 낮고 아이템이 있을 때)
bool canUseHeal = (hp < 50) && hasItem; 

// OR (||): 하나만 맞아도 됨 (체력이 없거나 마나가 없을 때)
bool isDanger = (hp <= 0) || (mp <= 0);

// NOT (!): 결과를 반대로 뒤집음 (아이템이 "없는" 상태인지 확인)
bool noItem = !hasItem; 
```

---

## 3. 조건문 (if, switch): "선택의 순간"
- **if-else**: "만약 ~라면 A를 하고, 아니면 B를 해라!"
- **switch**: 딱 떨어지는 값(상태 등)을 판별할 때 유리합니다.

```csharp
int score = 85;

// if문 예시
if (score >= 90) 
{
    Debug.Log("A등급");
}
else 
{
    Debug.Log("B등급");
}

// switch문 예시
string state = "Run";
switch (state)
{
    case "Idle":
        Debug.Log("대기 중");
        break;

    case "Run":  
        Debug.Log("달리는 중"); 
        break;
}
```

---

## 4. 반복문 (for, while): "반복의 마법"
- **for**: 반복 횟수가 정해져 있을 때 주로 사용합니다. (배열과 찰떡궁합!)
- **while**: 특정 조건이 참인 동안 무한히 반복할 때 사용합니다.

```csharp
// for문으로 배열 출력
for (int i = 0; i < 3; i++)
{
    Debug.Log($"{i}번 반복 중...");
}

// while문 예시
int count = 0;
while (count < 3)
{
    Debug.Log($"카운트: {count}");
    count++;
}
```

---

## 💻 실습 예제: 점수 관리와 등급 판별
**미션:** 배열에 담긴 3명의 점수를 출력하고, 평균 점수에 따라 등급을 매겨보세요. (유니티에서 확인)

<details>
<summary>코드 보기</summary>

```csharp
using UnityEngine; // 유니티 엔진 기능을 사용하기 위해 필수!

public class Day02_Practice : MonoBehaviour
{
    // 유니티가 시작될 때 자동으로 실행되는 함수
    private void Start()
    {
        int[] scores = { 85, 92, 78 };
        int total = 0;

        // 1. 반복문으로 총점 계산
        for (int i = 0; i < scores.Length; i++)
        {
            total += scores[i];
            Debug.Log($"{i + 1}번 학생 점수: {scores[i]}");
        }

        float average = (float)total / scores.Length;
        Debug.Log($"평균 점수: {average:F1}");

        // 2. 조건문으로 등급 판별
        if (average >= 90)
        {
            Debug.Log("등급: A");
        }
        else if (average >= 80)
        {
            Debug.Log("등급: B");
        }
        else
        {
            Debug.Log("등급: C");
        }
    }
}
```

</details>

---

## ✍️ 핵심 퀴즈
1. `int[] arr = new int[5];`에서 마지막 칸에 접근하는 인덱스 번호는?
2. `10 % 3`의 결과값은 얼마인가요?
3. 반복을 강제로 중단하고 싶을 때 사용하는 키워드는?

---

## 🎯 종합 연습 문제

### [미션 1: 장비 정보 관리하기 (Day 01 + Day 02 기초)]
**요구 사항:**
1. 캐릭터의 이름(`string`)과 현재 레벨(`int`)을 변수로 선언하고 초기화하세요.
2. 무기들의 공격력을 담은 배열 `int[] weaponDamages = { 10, 25, 40 };`을 만듭니다.
3. **공격력을 계산하는 함수** `int CalculateFinalDamage(int baseDamage, int level)`를 만듭니다. 이 함수는 `기본 공격력 + 레벨`을 반환합니다.
4. `for` 반복문을 사용하여 각 무기에 대해 위 함수를 호출하고, 결과를 출력하세요.
   - 출력 예시: `1번 무기 최종 공격력: 15 (기본 10 + 레벨 5)`


---

### [심화 미션: 몬스터 사냥 시스템 (Level 1)]
3일차부터 확장될 '몬스터 사냥 시스템'의 기초 로직을 설계해 봅니다.

**[요구 사항]**
1. 몬스터 4마리의 체력을 담는 배열 `int[] monsterHPs = { 50, 150, 250, 350 };`을 선언합니다.
2. 플레이어의 공격력을 `int playerDamage = 200;`으로 설정합니다.
3. `for` 반복문을 사용하여 배열에 담긴 모든 몬스터를 한 번씩 공격합니다.
4. **조건문(if-else)**을 활용하여 공격 결과를 판별하세요:
   - 몬스터의 HP가 플레이어의 공격력보다 **작거나 같으면**: `"몬스터 처치 성공!"` 출력
   - 몬스터의 HP가 플레이어의 공격력보다 **크면**: `"몬스터가 너무 강합니다... (남은 HP: XX)"` 출력
5. (도전) 몬스터를 처치할 때마다 `killedCount`라는 변수의 값을 1씩 증가시켜, 반복문이 끝난 뒤 **"총 처치한 몬스터 수: X마리"**를 출력해 보세요.

**[프로그래밍 힌트]**
- 배열의 칸수는 `monsterHPs.Length`를 활용하면 안전합니다.
- 남은 HP는 `몬스터HP - 플레이어공격력`으로 계산할 수 있습니다.
- `killedCount`는 반복문 밖에서 0으로 시작해야 데이터가 누적됩니다.

