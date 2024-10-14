using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Btn4Func : MonoBehaviour
{
    public delegate void FuncBtn();
    public FuncBtn CallbackFunc;

    [SerializeField] private GameObject Goal = null;
    private Transform target = null;

    private void Update()
    {
        if ((target != null) && Input.GetKeyDown(KeyCode.E))
        {
            Goal.SetActive(true);
            CallbackFunc();
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = _col.gameObject.transform;
            Debug.Log("Btn4 Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = null;
            Debug.Log("Btn4 Exit");
        }
    }
}
