using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class GameRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string roomName;

    [SerializeField]
    private bool isStarted = false;

    [SerializeField]
    private UICountDown countDown = null;

    private Collider[] colliders = new Collider[10];
    private MeshRenderer meshRenderer;
    private int playerLayerMask;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        playerLayerMask = LayerMask.GetMask("Player");

        if (countDown == null)
        {
            Debug.LogError("UICountDown component is not assigned.");
        }
    }

    private void FixedUpdate()
    {
        // 게임이 이미 시작된 경우 더 이상 처리하지 않음
        if (isStarted)
            return;

        // 현재 위치에서 정해진 범위 내에 플레이어가 있는지 감지
        int colliderCount = Physics.OverlapBoxNonAlloc(transform.position, transform.localScale * 0.5f, colliders, transform.rotation, playerLayerMask);

        // 정확히 두 명의 플레이어가 감지된 경우 게임 시작 처리
        if (colliderCount == 2)
        {
            foreach (Collider player in colliders)
            {
                if (player != null && player.CompareTag("Player"))
                {
                    PhotonView pv = player.gameObject.GetComponent<PhotonView>();
                    if (pv != null)
                    {
                        // 현재 클라이언트가 소유한 PhotonView인 경우만 카운트다운 시작
                        if (pv.IsMine)
                        {
                            // 카운트다운 시작 후 완료 시 EnterGame RPC 호출
                            countDown.CountDown(3, () =>
                            {
                                pv.RPC("EnterGame", RpcTarget.AllBuffered, roomName);
                            });
                        }
                    }
                }
            }

            // 게임이 시작되었음을 나타내고, 포털의 색상을 변경
            isStarted = true;
            meshRenderer.material.color = Color.white;
        }
    }
}
