using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DoorManager : MonoBehaviour
{
    [SerializeField]
    private DoorPuzzle lightDoors;
    [SerializeField]
    private DoorPuzzle darkDoors;

    private bool isOpenning = false;

    private void Update()
    {
        if(lightDoors.endOpen && darkDoors.endOpen)
            this.gameObject.SetActive(false);

        if (!isOpenning && lightDoors.canOpen && darkDoors.canOpen)
        {
            gameObject.GetComponent<PhotonView>().RPC("Open", RpcTarget.All);
        }
    }
    [PunRPC]
    public void Open()
    {
        isOpenning = true;
        lightDoors.OpenDoor();
        darkDoors.OpenDoor();
    }
}
