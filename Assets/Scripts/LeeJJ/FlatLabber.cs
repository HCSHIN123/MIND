using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatLabber : MonoBehaviour
{
    [SerializeField]
    private MovingFlat plat;
    private bool canStart = false;

    private void Update()
    {
        if (canStart && Input.GetKeyDown(KeyCode.E))
        {
            plat.StartMove();
            canStart = false;
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.CompareTag("Player"))
        {
            canStart = true;
        }
    }

    
}