using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCube : MonoBehaviour
{

    [SerializeField]
    public int CubeNumber;
    [SerializeField]
    public Animator animator;
    public void Left()
    {
        if (CubeNumber < 4)
        {
            CubeNumber += 1;
            animator.SetFloat("Number", CubeNumber);
            Debug.Log("Left");
        }
    }

    public void Right()
    {
        if (CubeNumber > 1)
        {
            CubeNumber -= 1;
            animator.SetFloat("Number", CubeNumber);
            Debug.Log("Right");
        }
    }
}
