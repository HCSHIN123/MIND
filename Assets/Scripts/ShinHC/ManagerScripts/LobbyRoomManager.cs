using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyRoomManager : MonoBehaviourPunCallbacks
{
    // ���¸� ��Ÿ���� ������
    private enum SITUATION
    {
        ENTER_GAMEROOM,   // ���� �뿡 ������ ��
        LEAVE_GAMEROOM,   // ���� �뿡�� ���� ��
        ENTER_LOBBYROOM,  // �κ� �뿡 ������ ��
        NONE,             // ���� ��Ȳ�� ���� ���
    }

    [SerializeField]
    private CinemachineVirtualCamera cinemachine;  // Cinemachine ���� ī�޶�

    private const string LobbyRoomName = "LobbyRoom";  // �κ� ���� �̸�
    private SITUATION currentSituation = SITUATION.ENTER_LOBBYROOM;  // ���� ��Ȳ
    private string targetRoomName;  // ��ǥ ���� �� �̸�

    // RoomOptions
    private RoomOptions lobbyRoomOptions = new();  // �κ� ���� ����
    private RoomOptions gameRoomOptions = new();   // ���� ���� ����

    [SerializeField]
    private int maxPlayerInLobby = 10;  // �κ񿡼��� �ִ� �÷��̾� ��
    [SerializeField]
    private int maxPlayerInGame = 2;    // ���ӿ����� �ִ� �÷��̾� ��

    private void Awake()
    {
        // �κ� �� ���� ���� �ִ� �÷��̾� �� ����
        lobbyRoomOptions.MaxPlayers = maxPlayerInLobby;
        gameRoomOptions.MaxPlayers = maxPlayerInGame;

        
        PhotonNetwork.NickName = User.Instance.userName;

        // Photon ��Ʈ��ũ ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // ������ ������ ����Ǿ��� �� ȣ��Ǵ� �ݹ�
        switch (currentSituation)
        {
            case SITUATION.ENTER_GAMEROOM:
                // ���� �뿡 ���� ��û
                PhotonNetwork.JoinOrCreateRoom(targetRoomName, gameRoomOptions, null);
                break;
            case SITUATION.ENTER_LOBBYROOM:
                // �κ� ���� ��û
                Debug.Log("������ ������ ���� ����");
                PhotonNetwork.JoinLobby();
                break;
            case SITUATION.LEAVE_GAMEROOM:
            case SITUATION.NONE:
                // ���� ���� �����ų� ��Ȳ�� ���� ���
                break;
        }
    }

    public override void OnJoinedLobby()
    {
        // �κ� �������� �� ȣ��Ǵ� �ݹ�
        Debug.Log("�κ� ����");
        currentSituation = SITUATION.NONE;
        // �κ� �뿡 �����ϰų� ���� ��û
        PhotonNetwork.JoinOrCreateRoom(LobbyRoomName, lobbyRoomOptions, null);
    }

    public override void OnJoinedRoom()
    {
        // �뿡 �������� �� ȣ��Ǵ� �ݹ�
        if (currentSituation == SITUATION.ENTER_GAMEROOM)
        {
            // ���� �뿡 �����ϴ� ��Ȳ
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            // �κ� �뿡 �����ϴ� ��Ȳ
            SetupPlayerInLobby();
        }
    }

    private void SetupPlayerInLobby()
    {
        // �÷��̾� ������Ʈ�� �κ񿡼� ����
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(0f, 0.5f, 0f), Quaternion.identity);

        // �÷��̾��� CameraRoot�� Cinemachine ���� ī�޶��� Follow ������� ����
        CameraRoot cameraRoot = player.GetComponentInChildren<CameraRoot>();
        if (cameraRoot != null)
        {
            cinemachine.Follow = cameraRoot.transform;
        }

        // �÷��̾� ������Ʈ�� lobbyManager�� ���� �ν��Ͻ��� ����
        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null)
        {
            playerComponent.lobbyManager = this;
        }
    }

    public void EnterGameRoom(string roomName)
    {
        // ���� ������ ���� ��û �޼���
        Debug.Log("���� ������ ���� ��û");
        currentSituation = SITUATION.ENTER_GAMEROOM;
        targetRoomName = roomName;
        // ���� ���� ������ ���� ������ ���� ��û
        PhotonNetwork.LeaveRoom();
    }
}
