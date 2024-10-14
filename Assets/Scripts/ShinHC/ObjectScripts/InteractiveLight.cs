using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class InteractiveLight : MonoBehaviour
{

    [SerializeField]
    private GameObject floor;
    [SerializeField]
    private GameObject body;
    [SerializeField]
    private GameObject head;
    [SerializeField]
    private LayerMask layerMask;

    private Vector3 prevPos;
    private Quaternion prevRot;
    private Vector3 prevScale;

    
    private bool upScale = true;

    private void Start()
    {
        ApplyRealTimeState();
    }

    private void FixedUpdate()
    {
        if (prevPos != transform.position || prevRot != transform.rotation || prevScale != transform.localScale)
            ApplyRealTimeState();

        prevPos = transform.position;
        prevRot = transform.rotation;
        prevScale = transform.localScale;
    }

    private void ApplyRealTimeState()
    {
        Debug.Log("PREv " + "/ Cur " + transform);
        RaycastHit hit;
        Vector3 direction = floor.transform.position - head.transform.position;
        direction.Normalize();

        if (Physics.Raycast(floor.transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            floor.transform.position += direction * hit.distance;
            body.transform.position += direction * hit.distance * 0.5f;
            body.transform.localScale += new Vector3(0f, (head.transform.position.y - body.transform.position.y), 0f);
            //head.transform.position += -direction * hit.distance * 0.5f;
            //head.transform.localScale += new Vector3(0f, (head.transform.position.y - head.transform.position.y) + 0.5f, 0f);
        }
    }
}
