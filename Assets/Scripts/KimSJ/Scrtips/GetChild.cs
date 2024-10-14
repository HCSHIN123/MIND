using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChild : MonoBehaviour
{
    private GameObject Parent;
    private GameObject Child;

    private Transform target;

    private void Start()
    {
        Parent = GameObject.Find("Moving Plat");
    }

    private void Update()
    {
        if (target != null)
        {
            Child = target.gameObject;

            Child.transform.SetParent(Parent.gameObject.transform);
        }
        else if (target == null)
        {
            Child.transform.SetParent(target);
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = _col.gameObject.transform;
            Debug.Log("Plat Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = null;
            Debug.Log("Plat xit");
        }
    }
}
