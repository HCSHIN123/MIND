using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlat : MonoBehaviour
{
    [SerializeField] GameObject goal;
    private float moveSpeed = 2.5f;

    private Transform target = null;
    
    Vector3 startPos;
    Vector3 goalPos;
    private Vector3 dir = Vector3.zero;
    bool isMove = false;

    void Start()
    {
        goalPos = goal.transform.position;
        startPos = transform.position;
        dir = goalPos - startPos;
    }

    void Update()
    {
        if(isMove)
        {
            Move();
        }
    }

    private void Move()
    {
        if(Vector3.Distance(transform.position, goalPos) < 1f)
        {
            dir = startPos - transform.position;
        }
        else if(Vector3.Distance(transform.position, startPos) < 1f)
        {
            dir = goalPos - transform.position;
        }

        dir.Normalize();
        transform.position += moveSpeed * dir * Time.deltaTime; 
    }

    public void StartMove()
    {
        isMove = true;
    }
}