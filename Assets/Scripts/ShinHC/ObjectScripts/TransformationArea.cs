using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationArea : MonoBehaviour
{
    public delegate void funcTranfromation(string _name, Transform _spawnPos);
    public funcTranfromation delTransformation;

    [SerializeField]
    private Transform playerGameSpawnPos;

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player") == false)
            return;

        if (_col.gameObject.GetPhotonView().IsMine)
        {
            if (gameObject.tag == "NoxArea")
                delTransformation("Nox", playerGameSpawnPos);
            else
                delTransformation("Lux", playerGameSpawnPos);
        }

        Destroy(gameObject);
    }
}
