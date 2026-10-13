using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkLauncher : MonoBehaviour
{
    // SerializeField는 private 필드를 Inspector에서 연결할 수 있게 합니다.
    [SerializeField] private TextMeshProUGUI statusText;

    // 로컬 실습에서는 두 실행 창 모두 같은 컴퓨터의 같은 포트를 사용합니다.
    private const string HostAddress = "127.0.0.1";
    private const ushort HostPort = 7777;

    // Button On Click()에서 호출하는 Host 시작 메서드입니다.
    public void StartHost()
    {
        // NetworkManager.Singleton은 씬의 유일한 NetworkManager에 접근합니다.
        NetworkManager manager = NetworkManager.Singleton;
        // IsListening은 이미 서버 또는 클라이언트가 실행 중인지 확인합니다.
        if (manager.IsListening)
        {
            return;
        }
        // GetComponent는 같은 오브젝트의 UnityTransport 컴포넌트를 가져옵니다.
        UnityTransport transport = manager.GetComponent<UnityTransport>();
        // SetConnectionData(접속 주소, 포트, 서버 수신 주소)로 직접 연결 설정을 지정합니다.
        // 세 번째 인자도 루프백으로 지정하므로 다른 컴퓨터의 접속은 받지 않습니다.
        transport.SetConnectionData(HostAddress, HostPort, HostAddress);
        // StartHost는 서버와 로컬 Client를 함께 시작하고 성공 여부를 반환합니다.
        bool started = manager.StartHost();
        statusText.text = started ? "Host 시작" : "Host 시작 실패";
    }

    // Button On Click()에서 호출하는 Client 연결 시작 메서드입니다.
    public void StartClient()
    {
        NetworkManager manager = NetworkManager.Singleton;
        if (manager.IsListening)
        {
            return;
        }
        UnityTransport transport = manager.GetComponent<UnityTransport>();
        // 클라이언트의 목적지는 같은 컴퓨터의 127.0.0.1:7777입니다.
        transport.SetConnectionData(HostAddress, HostPort);
        // StartClient는 이미 시작된 Host 또는 Server로 연결을 시도합니다.
        // true는 시작 성공이지 접속 완료가 아닙니다. 완료 여부는 Update에서 확인합니다.
        bool started = manager.StartClient();
        statusText.text = started ? "Client 연결 시도" : "Client 시작 실패";
    }

    // Update는 매 프레임 실행됩니다. 버튼을 누른 뒤 실제 연결 상태를 표시합니다.
    private void Update()
    {
        NetworkManager manager = NetworkManager.Singleton;
        if (manager == null)
        {
            return;
        }
        if (manager.IsHost)
        {
            // ConnectedClientsIds는 서버에 연결된 참여자 ID(Identifier, 식별자) 목록입니다.
            // Host 자신의 로컬 Client도 포함되므로 별도 Client가 붙으면 2명이 됩니다.
            statusText.text = $"Host 실행 / 참여자 {manager.ConnectedClientsIds.Count}명";
        }
        else if (manager.IsConnectedClient)
        {
            // IsConnectedClient는 연결 승인과 초기 동기화까지 완료됐는지 확인합니다.
            statusText.text = "Client 연결 완료";
        }
    }
}
