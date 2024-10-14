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
        // ������ �̹� ���۵� ��� �� �̻� ó������ ����
        if (isStarted)
            return;

        // ���� ��ġ���� ������ ���� ���� �÷��̾ �ִ��� ����
        int colliderCount = Physics.OverlapBoxNonAlloc(transform.position, transform.localScale * 0.5f, colliders, transform.rotation, playerLayerMask);

        // ��Ȯ�� �� ���� �÷��̾ ������ ��� ���� ���� ó��
        if (colliderCount == 2)
        {
            foreach (Collider player in colliders)
            {
                if (player != null && player.CompareTag("Player"))
                {
                    PhotonView pv = player.gameObject.GetComponent<PhotonView>();
                    if (pv != null)
                    {
                        // ���� Ŭ���̾�Ʈ�� ������ PhotonView�� ��츸 ī��Ʈ�ٿ� ����
                        if (pv.IsMine)
                        {
                            // ī��Ʈ�ٿ� ���� �� �Ϸ� �� EnterGame RPC ȣ��
                            countDown.CountDown(3, () =>
                            {
                                pv.RPC("EnterGame", RpcTarget.AllBuffered, roomName);
                            });
                        }
                    }
                }
            }

            // ������ ���۵Ǿ����� ��Ÿ����, ������ ������ ����
            isStarted = true;
            meshRenderer.material.color = Color.white;
        }
    }
}
