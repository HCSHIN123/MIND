using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Lux : Player
{
    public bool isOnLightPath = false;

    private int lightStack = 0; 

    private void Start()
    {
        inLight = true;
        ApplyNickName();
    }
    
    public void AddLight()
    {
        lightStack++;
    }

    public bool UseLight()
    {
        if (lightStack == 0)
            return false;
        lightStack--;
        return true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Light") || other.CompareTag("MovingPlat"))
        {
            if (!inLight)
                StopAllCoroutines();
            Debug.Log("LUXºû");
            inLight = true;
        }
        
        if(!isOnLightPath && other.CompareTag("LightPathStart"))
        {
            isOnLightPath=true;
            other.GetComponent<LightPath>().StartMove();

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light") && !isOnLightPath)
        {
            Debug.Log("LUX¾î¶Ö");
            inLight = false;
            StartCoroutine(InvalidArea());
        }

       

    }
}
