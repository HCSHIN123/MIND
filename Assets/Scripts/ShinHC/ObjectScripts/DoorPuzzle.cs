using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPuzzle : MonoBehaviour
{
    public bool endOpen = false;
    public bool canOpen = false;
    [SerializeField] GameObject left;
    [SerializeField] GameObject right;
    [SerializeField] private float openSpeed = 2f;
    private Vector3 dirToL;
    private Vector3 dirToR;

    private void Start()
    {
        dirToL = left.transform.position - right.transform.position;
        dirToL.Normalize();
        dirToR = right.transform.position - left.transform.position;
        dirToR.Normalize();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            canOpen = true;
    }

    public void OpenDoor()
    {
        StartCoroutine(OpenCoroutine());
    }

    IEnumerator OpenCoroutine()
    {
        while(Vector3.Distance(left.transform.position, right.transform.position) < 10f)
        {
            left.transform.position += dirToL * openSpeed * Time.deltaTime;
            right.transform.position += dirToR * openSpeed * Time.deltaTime;
            yield return null;
        }
        endOpen = true;
    }

}
