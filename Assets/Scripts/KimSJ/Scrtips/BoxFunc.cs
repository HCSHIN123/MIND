using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFunc : MonoBehaviour
{
    private Transform target = null;

    private void Update()
    {
        if ((target != null) && Input.GetKeyDown(KeyCode.E))
        {
            // ChangeTile.LStack++;
            target.GetComponent<Lux>().AddLight();
            Destroy(this.gameObject);
            //Debug.Log(ChangeTile.LStack);
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = _col.gameObject.transform;
            Debug.Log("Box Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = null;
            Debug.Log("Box Exit");
        }
    }
}