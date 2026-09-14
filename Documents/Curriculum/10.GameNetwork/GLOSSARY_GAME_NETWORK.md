# 게임 네트워크 프로그래밍 용어 사전

각 DAY에서 처음 등장하는 약어는 영문 풀네임과 함께 다시 설명합니다. 이 문서는 빠르게 되찾아 보는 누적 사전입니다.

| 용어 | 풀네임 | 쉬운 설명 |
| :--- | :--- | :--- |
| IP | Internet Protocol | 네트워크에서 컴퓨터를 찾기 위한 주소 체계입니다. |
| IP 주소 | Internet Protocol address | 네트워크에서 특정 컴퓨터를 찾는 숫자 형태의 주소입니다. `127.0.0.1`은 내 컴퓨터를 가리키는 루프백 주소입니다. |
| Socket | Socket | 운영 체제가 프로그램에 제공하는 네트워크 송수신 접점입니다. `TcpListener`, `TcpClient`는 TCP 소켓 사용을 쉽게 감싼 클래스입니다. |
| Port | Port | 한 컴퓨터 안에서 목적지 프로그램을 구분하는 0~65535 범위의 번호입니다. |
| Endpoint | Endpoint | IP 주소와 포트를 묶은 통신 상대의 주소입니다. 예: `127.0.0.1:7777` |
| Protocol | Protocol | 통신하는 양쪽이 데이터 형식과 처리 순서에 대해 지키는 약속입니다. TCP·UDP 같은 전송 규칙과 `TYPE|VALUE` 같은 게임 메시지 규칙이 있습니다. |
| HTTPS | Hypertext Transfer Protocol Secure | HTTP 통신에 TLS 보안을 적용한 요청-응답 방식입니다. 로그인과 상점 같은 서비스 API에 사용합니다. |
| HTTP | Hypertext Transfer Protocol | 클라이언트의 요청에 서버가 응답하는 통신 규칙입니다. 암호화가 필요한 게임 서비스 기능에는 HTTPS를 사용합니다. |
| TLS | Transport Layer Security | 통신 내용을 암호화하고 통신 상대를 확인하는 보안 규약입니다. |
| Request/Response | Request/Response | 클라이언트가 서버에 처리를 요청하고, 서버가 그 결과를 돌려주는 통신 흐름입니다. |
| development certificate | development certificate | `localhost`에서 HTTPS를 시험하기 위해 내 컴퓨터에 설치하는 개발 전용 인증서입니다. 실제 서비스에 사용하지 않습니다. |
| access token | access token | 로그인 성공 뒤 인증 서버가 발급하는 짧은 수명의 사용자 확인표입니다. 게임 서버는 이 값을 검증해 연결한 사용자를 확인합니다. |
| PlayerId | Player Identifier | 게임 서버 안에서 플레이어를 구분하는 고유 식별자입니다. IP 주소와 달리 게임 계정을 나타냅니다. |
| AUTH | Authentication | 이 과정의 예시 프로토콜에서 토큰 인증 요청을 뜻하는 메시지 타입입니다. |
| TCP | Transmission Control Protocol | 순서와 도착을 확인하며 데이터를 전달하는 통신 방식입니다. |
| TcpListener | TcpListener | 서버가 지정한 IP 주소와 포트에서 TCP 연결 요청을 기다리고 수락하는 클래스입니다. |
| TcpClient | TcpClient | TCP 서버에 연결하거나 서버가 수락한 한 클라이언트 연결을 나타내는 클래스입니다. |
| UDP | User Datagram Protocol | 빠르게 보내지만 도착·순서를 기본 보장하지 않는 통신 방식입니다. |
| C/S | Client/Server | 요청하는 클라이언트와 규칙을 처리하는 서버의 역할 분리입니다. |
| P2P | Peer-to-Peer | 여러 참여자가 서로 통신 상대가 될 수 있는 방식입니다. |
| RPC | Remote Procedure Call | 다른 네트워크 참여자에게 함수 실행을 요청하는 방식입니다. |
| NGO | Netcode for GameObjects | Unity GameObject 중심의 고수준 네트워킹 패키지입니다. |
| API | Application Programming Interface | 프로그램 기능을 호출하기 위한 약속입니다. |
| SDK | Software Development Kit | 특정 플랫폼 개발에 필요한 도구·라이브러리 모음입니다. |
| JSON | JavaScript Object Notation | 사람이 읽기 쉬운 텍스트 데이터 표현 형식입니다. |
| Stream | Stream | 바이트 데이터가 순서대로 흐르는 입출력 통로입니다. |
| NetworkStream | NetworkStream | TCP 연결 상대와 바이트를 주고받는 네트워크용 Stream입니다. |
| StreamReader/StreamWriter | StreamReader/StreamWriter | Stream의 바이트를 문자열로 읽거나 문자열을 바이트로 써 주는 도구입니다. |
| FileStream | FileStream | 디스크 파일을 바이트 Stream으로 읽고 쓰는 타입입니다. |
| MemoryStream | MemoryStream | 메모리 안의 바이트 배열을 Stream처럼 읽고 쓰는 타입입니다. |
| BufferedStream | BufferedStream | 다른 Stream 앞에서 작은 입출력을 모아 처리하는 보조 Stream입니다. |
| CTS | CancellationTokenSource | 비동기 작업을 취소하도록 신호를 만드는 객체입니다. |
| CancellationToken | CancellationToken | 취소 신호를 작업에 전달하고, 작업이 취소를 확인할 수 있게 하는 값입니다. |
| IDisposable | IDisposable interface | 사용 뒤 명시적으로 정리해야 하는 자원에 `Dispose()` 약속을 제공하는 C# 인터페이스입니다. |
| using 선언 | using declaration | 선언한 `IDisposable` 객체를 현재 범위가 끝날 때 자동으로 정리하는 C# 문법입니다. |
| CBT/OBT | Closed/Open Beta Test | 제한된 대상/공개 대상에게 테스트하는 운영 단계입니다. |
