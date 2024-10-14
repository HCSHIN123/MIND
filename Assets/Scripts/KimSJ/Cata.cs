using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Cata : MonoBehaviour
{
    [SerializeField] private EndingCredit ec;
    private bool isUsed = false;
   

    private void OnTriggerEnter(Collider other)
    {
        if(!isUsed && other.GetComponent<Player>().PV.IsMine)
        {
            isUsed = true;
            ec.StartStory(other.gameObject.GetComponent<ThirdPersonController>());
        }
    }
}
