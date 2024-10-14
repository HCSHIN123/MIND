using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCubeRight : MonoBehaviour
{

    private Transform target = null;

    [SerializeField]
    public CatCube Mother;

    private void Update()
    {   
        if ((target != null) && Input.GetKeyDown(KeyCode.E))
        {
            Mother.Right();
        }
    }

    private void OnTriggerEnter(Collider _col)
{
    if (_col.gameObject.CompareTag("Player"))
    {
        target = _col.gameObject.transform;

        Debug.Log("Right Enter");
    }
}

private void OnTriggerExit(Collider _col)
{
    if (_col.gameObject.CompareTag("Player"))
    {
        target = null;
        Debug.Log("Right Exit");
    }
}
}
