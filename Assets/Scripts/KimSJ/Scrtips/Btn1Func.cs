using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Btn1Func : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles = null;
    private Transform target = null;

    private void Update()
    {
        if ((target != null) && Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                PhotonNetwork.Destroy(tiles[i]);
               // Destroy(tiles[i]);
            }
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = _col.gameObject.transform;
            Debug.Log("Btn1 Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = null;
            Debug.Log("Btn1 Exit");
        }
    }
}
