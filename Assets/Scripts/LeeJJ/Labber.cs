using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labber : MonoBehaviour
{
    private Transform target = null;

    [SerializeField]
    public Animator animator;
    [SerializeField]
    public Animator animator2;

    [SerializeField]
    public CatCube cube1;
    [SerializeField]
    public CatCube cube2;
    [SerializeField]
    public CatCube cube3;
    [SerializeField]
    public CatCube cube4;

    private void Update()
    {
        if ((target != null) && Input.GetKeyDown(KeyCode.E))
        {
        
            if(cube1.CubeNumber == 1 && cube2.CubeNumber == 2 &&  cube3.CubeNumber == 3 && cube4.CubeNumber == 4)
            {
                animator.SetBool("Labber", true);
                animator2.SetBool("Labber", true);
                Debug.Log("Open Door");
            }
            else
            {
                Debug.Log("Not Open");
            }
            
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.CompareTag("Player"))
        {
            target = _col.gameObject.transform;
            Debug.Log("Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.gameObject.CompareTag("Player"))
        {
            target = null;
            Debug.Log("Exit");
        }
    }
}