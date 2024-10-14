using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyRoomManager : MonoBehaviourPunCallbacks
{
    // 상태를 나타내는 열거형
    private enum SITUATION
    {
        ENTER_GAMEROOM,   // 게임 룸에 입장할 때
        LEAVE_GAMEROOM,   // 게임 룸에서 나갈 때
        ENTER_LOBBYROOM,  // 로비 룸에 입장할 때
        NONE,             // 현재 상황이 없는 경우
    }

    [SerializeField]
    private CinemachineVirtualCamera cinemachine;  // Cinemachine 가상 카메라

    private const string LobbyRoomName = "LobbyRoom";  // 로비 룸의 이름
    private SITUATION currentSituation = SITUATION.ENTER_LOBBYROOM;  // 현재 상황
    private string targetRoomName;  // 목표 게임 룸 이름

    // RoomOptions
    private RoomOptions lobbyRoomOptions = new();  // 로비 룸의 설정
    private RoomOptions gameRoomOptions = new();   // 게임 룸의 설정

    [SerializeField]
    private int maxPlayerInLobby = 10;  // 로비에서의 최대 플레이어 수
    [SerializeField]
    private int maxPlayerInGame = 2;    // 게임에서의 최대 플레이어 수

    private void Awake()
    {
        // 로비 및 게임 룸의 최대 플레이어 수 설정
        lobbyRoomOptions.MaxPlayers = maxPlayerInLobby;
        gameRoomOptions.MaxPlayers = maxPlayerInGame;

        
        PhotonNetwork.NickName = User.Instance.userName;

        // Photon 네트워크 연결 설정
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // 마스터 서버에 연결되었을 때 호출되는 콜백
        switch (currentSituation)
        {
            case SITUATION.ENTER_GAMEROOM:
                // 게임 룸에 입장 요청
                PhotonNetwork.JoinOrCreateRoom(targetRoomName, gameRoomOptions, null);
                break;
            case SITUATION.ENTER_LOBBYROOM:
                // 로비에 입장 요청
                Debug.Log("마스터 서버에 연결 성공");
                PhotonNetwork.JoinLobby();
                break;
            case SITUATION.LEAVE_GAMEROOM:
            case SITUATION.NONE:
                // 게임 룸을 나가거나 상황이 없는 경우
                break;
        }
    }

    public override void OnJoinedLobby()
    {
        // 로비에 입장했을 때 호출되는 콜백
        Debug.Log("로비에 입장");
        currentSituation = SITUATION.NONE;
        // 로비 룸에 입장하거나 생성 요청
        PhotonNetwork.JoinOrCreateRoom(LobbyRoomName, lobbyRoomOptions, null);
    }

    public override void OnJoinedRoom()
    {
        // 룸에 입장했을 때 호출되는 콜백
        if (currentSituation == SITUATION.ENTER_GAMEROOM)
        {
            // 게임 룸에 입장하는 상황
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            // 로비 룸에 입장하는 상황
            SetupPlayerInLobby();
        }
    }

    private void SetupPlayerInLobby()
    {
        // 플레이어 오브젝트를 로비에서 생성
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(0f, 0.5f, 0f), Quaternion.identity);

        // 플레이어의 CameraRoot를 Cinemachine 가상 카메라의 Follow 대상으로 설정
        CameraRoot cameraRoot = player.GetComponentInChildren<CameraRoot>();
        if (cameraRoot != null)
        {
            cinemachine.Follow = cameraRoot.transform;
        }

        // 플레이어 컴포넌트의 lobbyManager를 현재 인스턴스로 설정
        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null)
        {
            playerComponent.lobbyManager = this;
        }
    }

    public void EnterGameRoom(string roomName)
    {
        // 게임 방으로 입장 요청 메서드
        Debug.Log("게임 방으로 입장 요청");
        currentSituation = SITUATION.ENTER_GAMEROOM;
        targetRoomName = roomName;
        // 현재 방을 나가고 게임 방으로 입장 요청
        PhotonNetwork.LeaveRoom();
    }
}
