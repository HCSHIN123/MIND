using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nox : Player
{
    private bool inShadow = false;
    private void Start()
    {
        inLight = false;
        ApplyNickName();
    }
    private void Update()
    {
        if(inLight && !inShadow)
        {
            inLight = false;
            StartCoroutine(InvalidArea());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            Debug.Log("NOXºû");
            inLight = true;
        }

        if(other.CompareTag("ShadowTile"))
        {
            inShadow = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("ShadowTile"))
        {
            inShadow = false;
        }
        if (other.CompareTag("Light"))
        {
            inLight = false;
        }
    }

}
