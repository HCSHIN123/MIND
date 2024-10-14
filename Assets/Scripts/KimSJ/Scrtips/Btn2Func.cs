using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn2Func : MonoBehaviour
{
    [SerializeField] private GameObject mirror = null;
    // [SerializeField] private GameObject light = null;

    // [SerializeField] private Material[] mats = null;
    private Transform target = null;

    int state = 0;

    private void Update()
    {
        

        if ((target != null) && Input.GetKeyDown(KeyCode.E))
        {
            
            if (state == 0)
            {
                // 20240614 OJH 수정
                // state++;

                // light.GetComponent<MeshRenderer>().material = mats[state];
                mirror.SetActive(true);
                state = 1;
            }
            else if (state == 1)
            {
                // 20240614 OJH 수정
                // state--;
                // light.GetComponent<MeshRenderer>().material = mats[state];
                mirror.SetActive(false);
                state = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = _col.gameObject.transform;
            Debug.Log("Btn2 Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = null;
            Debug.Log("Btn2 Exit");
        }
    }
}
