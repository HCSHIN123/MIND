using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFogFunc : MonoBehaviour
{
    private bool canUse = false;
    public static int DStack = 0;

    private void Update()
    {
        Debug.Log("FOG : " + canUse);
        if (canUse && Input.GetKeyDown(KeyCode.E))
        {
            DStack = DStack + 1;
            Destroy(this.gameObject);
            Debug.Log(PlayerCtrl.DStack);
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            canUse = true;
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            canUse = false;
        }
    }
}
