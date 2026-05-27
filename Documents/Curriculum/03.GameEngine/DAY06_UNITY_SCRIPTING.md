# 🚀 Day 06: 유니티 스크립팅 생명주기와 신형 Input System 캐릭터 이동 제어

오늘의 목표는 **"유니티의 MonoBehaviour 생명주기(Lifecycle) 흐름과 실행 주기를 이해하고, 신형 Input System 패키지를 기반으로 입력 감도 및 데드존(Deadzone) 필터가 반영된 이벤트 기반의 캐릭터 물리 이동 및 점프를 설계하고 구현하는 능력을 완수한다"**입니다.

---

## 1. 💡 이론 (30%): MonoBehaviour 생명주기와 실행 타이밍

유니티의 모든 스크립트 컴포넌트는 `MonoBehaviour`를 상속받음으로써 엔진의 네이티브 루프 루틴에 등록됩니다.

### 📍 생명주기 핵심 단계 및 실행 순서 (Lifecycle Diagram)

![MonoBehaviour 핵심 생명주기 흐름](Images/day06_unity_lifecycle.svg)

그림을 읽을 때는 `Awake -> OnEnable -> Start`를 "**수업 시작 전 준비**"로 보고, 가운데 반복 구간을 "**수업이 진행되는 동안 계속 도는 엔진의 시계**"로 보면 됩니다. 입력 확인은 `Update`, 물리 이동과 점프 힘 적용은 `FixedUpdate`에 두면 두 시계의 역할을 분리해서 이해하기 쉽습니다.

---

## 2. 🎮 신형 Input System vs 구형 Input Manager

최신 Unity 6에서는 구형 API인 `Input.GetAxis("Horizontal")` 대신 패키지 기반의 **신형 Input System**을 사용하는 것이 표준 규격입니다.

| 비교 항목 | 구형 Input Manager | 신형 Input System (Package) |
| :--- | :--- | :--- |
| **작업 흐름** | 매 프레임 `Update()` 내에서 수동 폴링 | 입력 신호를 액션 에셋(Input Action)과 결합한 **이벤트 기반 구독** |
| **멀티 디바이스** | 하드웨어 기기마다 매핑 코드를 따로 작성해야 함 | 하나의 액션에 키보드, 마우스, 게임패드, 모바일을 통합 바인딩 |
| **입력 정규화 & 데드존** | 조이스틱 감도 쏠림 현상을 코드로 수동 보정해야 함 | 액션 프로세서(Processors) 설정으로 데드존(Deadzone) 및 감도 자동 튜닝 |

### 📌 입력 벡터 정규화(Normalization)의 중요성
대각선 이동 시 키보드의 W($x=0, y=1$)와 D($x=1, y=0$)를 동시에 누르면 입력 벡터 크기는 $\sqrt{1^2 + 1^2} \approx 1.414$가 되어, 축 단방향 이동보다 속도가 빨라지는 물리 오류가 발생합니다.
- **해결책**: 입력 벡터 `Vector2`를 항상 크기가 `1`인 단위 벡터로 정규화(`.normalized`)하여 대각선 속도 쏠림을 보정합니다.

---

## 💻 3. 실습 (70%): 신형 Input System 기반 캐릭터 물리 제어 구현

**미션:** 신형 Input System 컴포넌트의 이벤트를 구독하여 플레이어가 입력한 방향으로 Rigidbody를 조작해 가속하되, 조이스틱 데드존 오차를 최소화하고 점프 기능을 물리 엔진과 연동하는 정밀 컴포넌트(`PhysicsPlayerController.cs`)를 작성하세요.

### 🛠️ 물리 캐릭터 컨트롤러 스크립트

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsPlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 6f;
    
    [Header("입력 감도 & 필터 설정")]
    [Range(0.05f, 0.5f)] 
    [SerializeField] private float inputDeadzone = 0.1f; // 데드존 임계값
    
    private Rigidbody rb;
    private Vector2 rawInputMovement;
    private Vector3 movementVector;
    private bool isGrounded;
    private bool isJumpPending;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        // Rigidbody 충돌 시 회전(기울어짐)을 방지하여 물리 조작성 확보
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    /// <summary>
    /// 1. 신형 Input System Event: 이동 액션 발생 시 호출 (이벤트 구독 방식)
    /// Input Action 에셋에서 Value 타입의 Vector2로 설정해야 함
    /// </summary>
    public void OnMove(InputValue value)
    {
        rawInputMovement = value.Get<Vector2>();
        
        // 입력 값이 설정한 미세 데드존 이하라면 입력을 무시하여 쏠림 현상 예방 (노이즈 필터링)
        if (rawInputMovement.sqrMagnitude < inputDeadzone * inputDeadzone)
        {
            rawInputMovement = Vector2.zero;
        }
    }

    /// <summary>
    /// 2. 신형 Input System Event: 점프 액션 발생 시 호출 (이벤트 구독 방식)
    /// </summary>
    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            isJumpPending = true; // FixedUpdate에서 처리하기 위한 예약
        }
    }

    void Update()
    {
        // 3. 방향 벡터의 기하학적 정규화 처리 (대각선 속도 쏠림 보정)
        // Y축(up)은 Rigidbody 점프로 해결하므로 수평 2D 공간의 3D화(X, Z) 진행
        Vector3 normalizedDirection = new Vector3(rawInputMovement.x, 0f, rawInputMovement.y);
        
        if (normalizedDirection.sqrMagnitude > 1f)
        {
            normalizedDirection.Normalize();
        }

        movementVector = normalizedDirection;
    }

    void FixedUpdate()
    {
        // 4. 고정 물리 프레임 주기(0.02초)에서 물리 이동 실행
        MoveCharacter();

        // 5. 물리 점프 로직 실행
        if (isJumpPending)
        {
            JumpCharacter();
        }
    }

    private void MoveCharacter()
    {
        // 리지드바디의 속도(velocity)를 직접 계산하되, 기존 점프 속도(Y)는 보존
        Vector3 targetVelocity = movementVector * moveSpeed;
        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
    }

    private void JumpCharacter()
    {
        // 물리적인 순간적인 수직 충격 부여 (Impulse)
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumpPending = false;
        isGrounded = false;
        Debug.Log("<color=cyan>[Jump]</color> 물리 점프가 정상 실행되었습니다.");
    }

    void OnCollisionEnter(Collision collision)
    {
        // 단순 지면 안착 판정 (기초)
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
        }
    }
}
```

---

## 🎯 NCS 능력단위 학습 가이드 & 평가 만족 요건

본 강의 내용은 **"게임엔진 응용 프로그래밍(NCS 0803020527_18v4)"**의 **수행준거 2.2 사용자 입력 처리 구현**을 완벽하게 충족합니다.

| NCS 평가 준거 | 학습 대응 영역 | 만족 기법 및 로직 |
| :--- | :--- | :--- |
| **사용자 입력 처리 구현** | 다양한 하드웨어 입력 환경에 대응하는 로직 구현 | 신형 Input System 기반 바인딩 및 이벤트 핸들러(`OnMove`, `OnJump`) 연동 |
| **입력 데이터 무결성 검증** | 미세 잡음 및 과도 입력 보정 | 입력 감도 임계 제어(`Deadzone`), 대각선 쏠림 제한을 위한 단위 벡터 정규화(`.Normalize`) 구현 |

---

## ✍️ 평가 문항 대비 핵심 퀴즈

1. **문제:** 매 프레임 가변적인 그래픽 업데이트 주기(`Update`)가 아닌 일정한 고정 간격으로 실행되어 속도 감쇠와 충돌 판정을 신뢰할 수 있게 연산할 수 있는 유니티 생명주기 루틴은 무엇인가요?
   - **정답:** `FixedUpdate()`

2. **문제:** 키보드 입력을 받아 2차원 평면 이동 벡터를 생성할 때, 대각선 이동 벡터가 단방향 벡터보다 길어져 캐릭터가 빠르게 쏠려 나가는 결함을 예방하기 위해 C#에서 연산하는 벡터 최적화 메서드는 무엇인가요?
   - **정답:** `Normalize()` (또는 `.normalized` 프로퍼티)

3. **문제:** 신형 Input System에서 가속도계, 아날로그 조이스틱 등 물리 기기의 미세 쏠림이나 중립 오차 현상으로 입력 값이 미세하게 들어올 때 이를 무시하도록 설정하는 감도 보정 영역을 무엇이라 합니까?
   - **정답:** 데드존 (Deadzone)

