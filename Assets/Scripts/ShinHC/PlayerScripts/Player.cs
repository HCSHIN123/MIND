using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using Unity.VisualScripting;
using StarterAssets;
public class Player : MonoBehaviourPunCallbacks
{
    [SerializeField]
    protected float timeInvalidArea = 2.5f;
    protected CharacterController cc;

    Transform[] checkPoints;
    public Transform[] CheckPoints { get { return checkPoints; } set { checkPoints = value; } }

    private int curCheckpPointIdx = 0;
    private int maxCheckPointIdx;

    public PhotonView PV;
    public LobbyRoomManager lobbyManager;
    public GameRoomManager gameRoomManager;
    protected bool inLight = false;

    private WaitForSecondsRealtime wait0_2 = new WaitForSecondsRealtime(0.2f);


    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        ApplyNickName();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P) && gameObject.GetPhotonView().IsMine)//ġƮŰ
        {
            MoveToNextCheckPoint();
        }

    }
    public void ApplyNickName()
    {

        if (photonView.IsMine)
        {
            GetComponent<ThirdPersonController>().nameTag.text = PhotonNetwork.NickName;
            GetComponent<ThirdPersonController>().nameTag.color = Color.white;
        }
        else
        {
            GetComponent<ThirdPersonController>().nameTag.text = photonView.Owner.NickName;
            GetComponent<ThirdPersonController>().nameTag.color = Color.red;
        }
    }

    [PunRPC]
    public void RPCNickname()
    {
        GetComponentInChildren<ThirdPersonController>().nameTag.text = PhotonNetwork.NickName;
    }

    //public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    //{
    //    if (PhotonNetwork.CurrentRoom.Name != "LobbyRoom" && !PhotonNetwork.IsMasterClient)
    //    {
    //        PV.RPC("EnterGameScene", RpcTarget.AllBuffered, null);
    //    }
    //}
    [PunRPC]
    void EnterGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    [PunRPC]
    public void EnterGame(string _roomName)
    {
        if (photonView.IsMine)  // �� Ŭ���̾�Ʈ�� ĳ�������� Ȯ��
            StartGame(_roomName);
    }
    private void StartGame(string _roomName)
    {
        lobbyManager.EnterGameRoom(_roomName);  // �κ�Ŵ������� ���ӷ��������
    }




    protected IEnumerator InvalidArea() // 1�� �� üũ����Ʈ�� �̵�.. 5���� ���� ���� : �߰��� ��ȿ�� ������ �ǵ��� ������ ����Ǹ� �ȵǴϱ�
    {
        for (int i = 0; i < 5; ++i)
        {
            yield return wait0_2;
        }
        ReturnCurCheckPoint();
    }

    public void Init(Transform[] _checkPoints)
    {
        checkPoints = _checkPoints;
        maxCheckPointIdx = checkPoints.Length;
    }

    public void MoveToNextCheckPoint()
    {
        ++curCheckpPointIdx;
        curCheckpPointIdx %= maxCheckPointIdx;
        Debug.Log("CURIDX : " + curCheckpPointIdx + "MAX : " + maxCheckPointIdx);
        transform.position = checkPoints[curCheckpPointIdx].position;
    }
    public void ReturnCurCheckPoint()
    {
        cc.enabled = false;
        transform.position = checkPoints[curCheckpPointIdx].position;
        cc.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            inLight = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        inLight = false;
    }
}
