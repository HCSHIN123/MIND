using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFlat : MonoBehaviour
{
    

    [SerializeField]
    public bool isMoving = false;
    private bool toEnd = true;
    [SerializeField]
    private GameObject end;
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float margin = 0.1f;
   
    public Player player;


    private Vector3 moveDir = Vector3.zero;
    private Vector3 endPos;
    private Vector3 startPos;

    private Vector3 lastPosition;
    private Vector3 moveVec; 
    public Vector3 MoveVec{ get { return moveVec;} }

    private void Awake()
    {
        endPos = end.transform.position;
        startPos = transform.position;
        moveDir = endPos - startPos;
        moveDir.Normalize();
    }
    private void Update()
    {
        if (isMoving)
        {
            MoveProcess();
        }
    }

    private void MoveProcess()
    {
        if (isArrived())
        {
            toEnd = !toEnd;
            moveDir = -moveDir;
        }
        moveVec = moveDir * moveSpeed * Time.deltaTime;
        transform.position += moveVec;

        if (Vector3.Distance(player.transform.position, transform.position) >= 3f)
            return;

        CharacterController cc = player.gameObject.GetComponent<CharacterController>();
        if (cc != null)
           {
                cc.enabled = false;
                player.transform.position += moveVec;
                cc.enabled = true;
           }
        
    }

    private bool isArrived()
    {
        return Vector3.Distance(transform.position, toEnd ? endPos : startPos) < margin;
    }

    public void StartMove()
    {
        isMoving = true;
    }
   
   

   


}
