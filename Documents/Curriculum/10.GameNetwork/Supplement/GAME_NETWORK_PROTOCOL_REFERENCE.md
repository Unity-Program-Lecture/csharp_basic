# 게임에서 사용하는 네트워크 프로토콜 참고 안내

> 이 문서는 게임 네트워크에서 자주 만나는 통신 방식의 역할과 선택 기준을 정리한 **참고 자료**입니다. DAY01~DAY15의 필수 실습 범위에는 `Task`, HTTP/HTTPS, TCP, UDP, Netcode for GameObjects (NGO)가 포함됩니다. WebSocket, gRPC, Protocol Buffers, QUIC은 "무엇을 언제 쓰는가"를 이해하기 위한 참고 대상입니다.

## 1. 먼저 구분할 것: "길", "대화 규칙", "짐 포장 방식"

네트워크 기술은 모두 같은 종류가 아닙니다. 택배에 비유하면 다음과 같습니다.

- **전송 계층 (Transport layer)**: 데이터를 옮기는 길입니다. TCP와 UDP, QUIC이 여기에 해당합니다.
- **응용 계층 프로토콜 (Application-layer protocol)**: 길 위에서 어떤 형식으로 요청하고 응답할지 정한 대화 규칙입니다. HTTP, WebSocket, gRPC가 여기에 해당합니다.
- **직렬화 (Serialization)**: 메모리 속 객체를 네트워크로 보낼 수 있는 바이트 묶음으로 바꾸는 포장 방법입니다. JSON과 Protocol Buffers가 여기에 해당합니다.
- **전송 계층 보안 (Transport Layer Security, TLS)**: 전송 중인 내용을 암호화하고 상대방을 확인하는 보안 장치입니다. HTTPS와 WSS에서 사용합니다.

```text
게임의 명령과 데이터
        │
        ├─ 직렬화: JSON / Protocol Buffers
        ├─ 대화 규칙: HTTP(S) / WebSocket(S) / gRPC / 게임 전용 패킷 규칙
        ├─ 전송 경로: TCP / UDP / QUIC
        └─ 보안 덮개: TLS (필요한 구간에 적용)
```

따라서 **Protocol Buffers는 WebSocket 기반 기술이 아닙니다.** Protocol Buffers는 데이터를 포장하는 방식이고, 포장한 결과는 TCP, UDP, HTTP, WebSocket, gRPC 등 여러 경로로 보낼 수 있습니다.

## 2. 핵심 전송 방식

### 2.1 TCP (Transmission Control Protocol)

**TCP (Transmission Control Protocol, 전송 제어 프로토콜)**는 연결을 만든 뒤, 보낸 바이트가 순서대로 도착하도록 관리하는 전송 방식입니다. 받는 쪽에서 누락을 확인하고 필요하면 다시 보내므로 신뢰성이 높습니다.

TCP는 "번호표가 있는 택배"에 가깝습니다. 1번 상자 뒤에 2번 상자가 오도록 맞춰 주며, 상자가 사라지면 다시 보냅니다. 다만 재전송을 기다리는 동안 최신 위치 정보까지 늦어질 수 있으므로, 매 프레임마다 바뀌는 좌표에는 신중히 사용해야 합니다.

- 잘 맞는 예: 로그인, 채팅, 상점 구매, 인벤토리 변경, 반드시 도착해야 하는 중요 명령
- C# 학습 API 예: `TcpClient`, `TcpListener`, `NetworkStream`
- 주의: TCP는 **바이트 흐름 (byte stream)** 이므로 메시지 경계를 보장하지 않습니다. `ReadLineAsync`처럼 구분자를 쓰거나, 길이 정보를 앞에 붙이는 패킷 규칙이 필요합니다.

### 2.2 UDP (User Datagram Protocol)

**UDP (User Datagram Protocol, 사용자 데이터그램 프로토콜)**는 연결 확인이나 재전송을 기본으로 처리하지 않는 전송 방식입니다. 여기서 **데이터그램 (datagram)** 은 한 번에 보내는 독립된 데이터 묶음을 뜻합니다.

UDP는 "빨리 보내는 엽서"에 가깝습니다. 중간에 한 장이 사라지거나 순서가 바뀔 수 있지만, 최신 위치가 곧 다시 오므로 오래된 위치를 기다릴 필요가 없는 게임 상황에 알맞습니다.

- 잘 맞는 예: 이동 입력, 짧은 간격의 위치·방향 스냅샷, 빠르게 갱신되는 상태 정보
- C# 학습 API 예: `UdpClient`
- 주의: 순서, 중복, 유실, 신뢰성은 게임 프로토콜 또는 네트워크 라이브러리가 처리해야 합니다. "UDP가 빠르다"만으로 모든 데이터를 UDP로 보내면 안 됩니다.

### 2.3 QUIC (Quick UDP Internet Connections)

**QUIC (Quick UDP Internet Connections)**은 UDP 위에서 연결 관리, 암호화, 여러 데이터 흐름을 제공하는 현대적인 전송 프로토콜입니다. 기본적으로 **TLS 1.3 (Transport Layer Security version 1.3)** 암호화를 사용합니다.

QUIC은 하나의 데이터 흐름이 늦어져도 다른 흐름이 함께 멈추는 문제를 줄이도록 설계되었습니다. **다중화 (multiplexing)** 는 하나의 연결 안에 여러 독립 흐름을 함께 흘려보내는 기능입니다.

- 활용 예: HTTP/3, WebTransport 기반 서비스
- 학습 위치: 현재 과정에서는 개념 참고 대상입니다. Unity 게임의 실시간 동기화는 먼저 UDP와 NGO의 전송 방식을 확실히 익힙니다.
- 주의: 일부 네트워크 환경은 UDP 또는 QUIC을 제한할 수 있으므로, 실제 서비스는 TCP 기반 경로로 돌아갈 수 있는 **폴백 (fallback, 대체 경로)** 도 고려합니다.

## 3. 서버 API와 실시간 대화 방식

### 3.1 HTTP와 HTTPS (Hypertext Transfer Protocol / HTTP Secure)

**HTTP (Hypertext Transfer Protocol, 하이퍼텍스트 전송 프로토콜)**는 클라이언트가 요청을 보내고 서버가 응답을 돌려주는 **요청-응답 (request-response)** 방식의 응용 계층 프로토콜입니다. 한 요청의 결과를 받은 뒤 다음 일을 판단하는 웹 API에 잘 맞습니다.

**HTTPS (Hypertext Transfer Protocol Secure)**는 HTTP 통신에 TLS 보안을 적용한 방식입니다. 로그인 토큰, 결제 요청, 계정 정보처럼 노출되면 안 되는 데이터를 보낼 때 사용합니다.

- 잘 맞는 예: 로그인, 계정 생성, 게임 설정 내려받기, 점검 공지, 상점·결제 서버 호출, 콘텐츠 목록 조회
- 장점: 모바일과 웹 환경에서 널리 지원되고, 서버 API와 연동하기 쉽습니다.
- 한계: 요청할 때마다 응답을 기다리는 기본 구조이므로, 매 프레임 상태를 주고받는 실시간 게임 루프의 주 통로로는 적합하지 않습니다.

### 3.2 WebSocket과 WSS (WebSocket Secure)

**WebSocket**은 최초 연결 뒤 클라이언트와 서버가 양방향으로 메시지를 계속 주고받을 수 있는 통신 방식입니다. HTTP 요청으로 연결을 시작한 뒤 지속 연결로 전환합니다.

**WSS (WebSocket Secure)**는 WebSocket에 TLS를 적용한 보안 연결입니다. HTTPS와 마찬가지로 외부 서비스에서는 WSS 사용을 기본으로 생각합니다.

WebSocket은 "전화를 연결해 둔 뒤 서로 바로 말하는 방식"에 가깝습니다. 다만 보통 TCP 위에서 동작하므로, UDP처럼 일부 상태만 버리고 최신 값을 즉시 받는 특성은 기본 제공하지 않습니다.

- 잘 맞는 예: 채팅, 로비, 길드 알림, 친구 접속 알림, 턴제 게임의 이벤트 알림, 웹 클라이언트와의 실시간 통신
- 주의: 고빈도 위치 동기화에 무조건 WebSocket을 쓰기보다, 지연과 메시지량, 재전송이 필요한지부터 판단합니다.

### 3.3 gRPC (Google Remote Procedure Call)

**gRPC (Google Remote Procedure Call)**는 원격 서버의 함수를 마치 내 프로그램의 메서드처럼 호출하도록 돕는 **원격 프로시저 호출 (Remote Procedure Call, RPC)** 프레임워크입니다. 일반적으로 HTTP/2와 Protocol Buffers를 기본 조합으로 사용합니다.

**HTTP/2 (Hypertext Transfer Protocol version 2)**는 한 연결에서 여러 요청과 응답을 효율적으로 처리할 수 있도록 개선된 HTTP 버전입니다. gRPC의 **스트리밍 (streaming)** 은 요청 또는 응답을 한 번에 끝내지 않고 여러 메시지로 이어 보내는 기능입니다.

- 잘 맞는 예: 계정·인벤토리·랭킹 같은 백엔드 API, 게임 서버와 운영 서버 사이의 내부 통신
- 호출 형태: 한 번 요청·한 번 응답, 서버 스트리밍, 클라이언트 스트리밍, 양방향 스트리밍
- 주의: gRPC는 WebSocket이 아니며, 실시간 액션 게임의 매 프레임 위치 동기화 전용 기술도 아닙니다. 신뢰성 있는 서비스 API와 서버 간 통신에서 특히 강점이 있습니다.

## 4. 데이터 포장 방식

### 4.1 JSON (JavaScript Object Notation)

**JSON (JavaScript Object Notation)**은 키와 값으로 데이터를 텍스트 형태로 표현하는 직렬화 형식입니다. 사람이 읽기 쉬워서 설정 파일, 운영 도구, 웹 API의 요청·응답에서 많이 사용합니다.

- 장점: 로그를 읽기 쉽고, 디버깅과 외부 API 연동이 편합니다.
- 한계: 텍스트이므로 같은 데이터를 보낼 때 이진 형식보다 크기가 커질 수 있습니다.

### 4.2 Protocol Buffers (Protobuf)

**Protocol Buffers (프로토콜 버퍼스, Protobuf)**는 Google이 만든 이진 직렬화 형식입니다. `.proto` 파일에 메시지 구조를 정의하면 여러 언어에서 사용할 코드를 생성할 수 있습니다. **이진 (binary)** 은 사람이 바로 읽는 텍스트가 아니라 컴퓨터가 처리하기 좋은 바이트 표현을 뜻합니다.

- 장점: 데이터 크기가 작고, 여러 언어의 서버·클라이언트가 같은 데이터 구조를 공유하기 좋습니다.
- 관계: gRPC는 기본적으로 Protocol Buffers를 사용하지만, Protocol Buffers 자체는 통신 경로가 아닙니다.
- 주의: 작은 패킷이라고 해서 항상 Protobuf가 정답은 아닙니다. 디버그 편의성, 버전 관리, 팀의 도구 환경도 함께 판단합니다.

## 5. 게임 기능에서 고르는 기준

| 게임 상황 | 우선 검토할 방식 | 선택 이유 |
| --- | --- | --- |
| 로그인, 공지, 원격 설정, 상점 | HTTPS | 보안이 필요한 요청-응답 API에 적합합니다. |
| 채팅, 로비 알림, 길드 알림 | WSS 또는 TCP 기반 연결 | 양방향 알림과 반드시 도착해야 하는 메시지에 적합합니다. |
| 조작 입력, 위치·방향 스냅샷 | UDP 또는 게임 엔진 전송 계층 | 최신 상태가 중요하고, 오래된 상태는 버려도 되는 경우가 많습니다. |
| 공격 판정, 아이템 획득, 재화 변경 | 서버 권한 검증 + 신뢰성 있는 전달 | 중요한 결과는 서버가 검증하고, 유실되면 안 됩니다. 전송 방식은 엔진·라이브러리의 신뢰성 채널을 포함해 설계합니다. |
| 게임 서버와 계정·랭킹 서버의 통신 | gRPC | 명확한 서비스 계약과 여러 언어 간 연동에 적합합니다. |
| 위 방식으로 보낼 데이터 구조 | JSON 또는 Protocol Buffers | 사람이 읽기 쉬운지, 데이터 크기와 처리 효율이 중요한지에 따라 고릅니다. |

**서버 권한 (server authority)** 이란 결과를 최종 확정할 권한을 서버가 갖는 설계입니다. 예를 들어 클라이언트가 "적을 처치했다"고 보내도, 서버가 거리·쿨다운·체력 등을 검증한 뒤 보상을 확정해야 합니다. 프로토콜 선택은 보안 설계를 대신하지 않습니다.

## 6. Unity와 Netcode for GameObjects의 위치

**NGO (Netcode for GameObjects)**는 Unity에서 멀티플레이 게임을 만들 때 사용하는 고수준 네트워킹 라이브러리입니다. 이 과정의 Unity 실습은 NGO가 제공하는 `NetworkVariable`, **RPC (Remote Procedure Call, 원격 프로시저 호출)**, 소유권, 서버 권한 개념을 중심으로 진행합니다.

NGO 아래에는 Unity Transport처럼 UDP와 WebSocket 위에서 연결을 다룰 수 있는 전송 계층이 있습니다. 학습자는 먼저 DAY04의 TCP, DAY05의 UDP로 "전송 방식의 차이"를 이해한 뒤, DAY09~DAY11에서 NGO가 그 복잡함을 어떻게 감싸는지 확인합니다.

```text
게임 플레이 코드
        ↓
Netcode for GameObjects (동기화, RPC, 소유권)
        ↓
Unity Transport (연결과 전송을 다루는 계층)
        ↓
UDP 또는 지원되는 전송 경로
```

## 7. 혼동을 막는 빠른 확인

| 질문 | 답 |
| --- | --- |
| "Protobuf는 WebSocket 기반인가?" | 아닙니다. Protobuf는 데이터를 바이트로 포장하는 직렬화 형식입니다. |
| "gRPC는 WebSocket인가?" | 아닙니다. 일반적으로 HTTP/2와 Protocol Buffers를 사용하는 RPC 프레임워크입니다. |
| "WebSocket이면 UDP처럼 빠른가?" | 보통 TCP 특성을 가지므로, 유실을 허용하는 UDP 상태 동기화와 성질이 다릅니다. |
| "HTTPS는 실시간 게임에 전혀 못 쓰는가?" | 로그인·상점·설정 같은 게임 서비스 기능에는 매우 흔히 사용합니다. 다만 매 프레임 상태 동기화의 주 통로는 아닙니다. |
| "UDP면 중요한 데이터도 잃어도 되는가?" | 아닙니다. 중요한 결과는 서버 권한으로 검증하고 신뢰성 있는 전달 또는 재시도 규칙을 설계해야 합니다. |

## 8. 더 살펴볼 공식 자료

- [Microsoft .NET QUIC 개요](https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/quic/quic-overview)
- [Microsoft .NET HTTP/3와 QUIC](https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-http3)
- [MDN HTTP 개요](https://developer.mozilla.org/en-US/docs/Web/HTTP/Guides/Overview)
- [MDN WebSocket API](https://developer.mozilla.org/en-US/docs/Web/API/WebSockets_API)
- [gRPC 소개](https://grpc.io/docs/what-is-grpc/introduction/)
- [Protocol Buffers 개요](https://protobuf.dev/overview/)
- [Unity Multiplayer 개요](https://docs.unity3d.com/jp/current/Manual/multiplayer-overview.html)

다음 단계에서는 이 문서를 "정답 목록"으로 외우기보다, 기능마다 **유실되어도 되는가**, **순서가 중요한가**, **서버 검증이 필요한가**, **연결을 계속 유지해야 하는가**를 질문하며 선택합니다.
