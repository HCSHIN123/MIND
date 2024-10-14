using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampSwitch : MonoBehaviour
{
    private bool isUsable = false;
    public bool under = false;
    [SerializeField] GameObject lamp = null;
    Vector3 prevPos;
    Vector3 underPos = new Vector3(0f, -1000f, 0f);

    private void Start()
    {
        prevPos = lamp.transform.position;
        if(under)
            lamp.transform.position = underPos;
    }
    private void Update()
    {
        if (isUsable && Input.GetKeyUp(KeyCode.E))
        {
            if(under)
                lamp.transform.position = prevPos;
            else
                lamp.transform.position = underPos;

            under = !under;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        isUsable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            isUsable = false;
    }
}
