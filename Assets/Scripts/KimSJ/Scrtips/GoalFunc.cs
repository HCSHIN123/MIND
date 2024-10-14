using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoalFunc : MonoBehaviour
{
    [SerializeField] private GameObject goal = null;
    private Transform target = null;

    private void Start()
    {
        goal.SetActive(false);
    }

    private void Update()
    {
        if ((target != null) && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(goal);
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = _col.gameObject.transform;
            Debug.Log("Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = null;
            Debug.Log("Exit");
        }
    }
}