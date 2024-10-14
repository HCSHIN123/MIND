using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPath : MonoBehaviour
{
    public Lux lux;

    [SerializeField]
    private Transform[] points;
    private int curTargetPointIdx = 0;
    
    private bool isMoving = false;
    private Vector3 dir;
    [SerializeField] private float speed = 3.5f;
    [SerializeField]
    MirrorHolder mh = null;


    private void Update()
    {
        if (isMoving)
        {
            MoveToTargetPoint();
        }
    }
    public void MoveToTargetPoint()
    {
        if (Vector3.Distance( points[curTargetPointIdx].position, lux.transform.position) < 0.1f)
        {
            if(curTargetPointIdx == points.Length - 1)
            {
                isMoving = false;
                lux.GetComponent<CharacterController>().enabled = true;
                lux.isOnLightPath = false ;
                return;
            }

            dir = points[++curTargetPointIdx].position - lux.transform.position;
            dir.Normalize();
        }

        lux.transform.position += dir * speed * Time.deltaTime;

    }

    public void StartMove()
    {
       
        isMoving = true;
        lux.GetComponent<CharacterController>().enabled = false;
        lux.transform.position = points[curTargetPointIdx].position;

        dir = points[++curTargetPointIdx].position - lux.transform.position;
        dir.Normalize();
    }

   
}
