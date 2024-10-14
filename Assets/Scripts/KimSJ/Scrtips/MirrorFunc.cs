using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorFunc : MonoBehaviour
{
    private Transform target = null;
    [SerializeField]
    private GameObject copiedNox = null;
    [SerializeField]
    private Btn4Func btn = null;

    private bool canUse = true;
    private bool inMirror = false;
    private Vector3 spawnPoint = Vector3.zero;

    private void Start()
    {
        copiedNox.SetActive(false);
        spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2.0f);
        btn.CallbackFunc = ReturnPosition;
    }

    private void Update()
    {
        if (canUse && inMirror && Input.GetKeyUp(KeyCode.E))
        {
            if (ShadowFogFunc.DStack > 0)
            {
                copiedNox.SetActive(true);
                copiedNox.transform.position = target.transform.position;
                Debug.Log("SP : " + spawnPoint);
                target.GetComponent<CharacterController>().enabled = false;
                target.transform.position = spawnPoint;
                target.GetComponent<CharacterController>().enabled = true;
                canUse = false;
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    public void ReturnPosition()
    {
        target.GetComponent<CharacterController>().enabled = false;
        target.transform.position = copiedNox.transform.position;
        target.GetComponent<CharacterController>().enabled = true;
        copiedNox.SetActive(false);
        target = null;
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = _col.gameObject.transform;
            inMirror = true;
            Debug.Log("Mirror Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            // target = null;
            // canUse = false;
            Debug.Log("Mirror Exit");
            inMirror = false;
        }
    }
}
