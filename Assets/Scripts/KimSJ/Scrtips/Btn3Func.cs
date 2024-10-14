using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Btn3Func : MonoBehaviour
{
    //[SerializeField] private GameObject Lift = null;
    //[SerializeField] private GameObject LiftGoal = null;
    [SerializeField] private MovingPlat mp = null;
    private Transform target = null;

    //private float Speed = 10.0f; 

    private void Update()
    {
        if ((target != null) && Input.GetKeyDown(KeyCode.E))
        {
            mp.StartMove();
        }
    }

    //private IEnumerator GoLiftCoroutine()
    //{
    //    while (true)
    //    {
    //        Lift.transform.position = Vector3.MoveTowards(Lift.transform.position, LiftGoal.transform.position, Speed * Time.deltaTime);

    //        yield return new WaitForEndOfFrame();

    //        if (Lift.transform.position.x >= 45.0f)
    //        {
    //            yield return new WaitForSeconds(1.0f);
    //            StartCoroutine("BackLiftCoroutine");
    //            StopCoroutine("GoLiftCoroutine");
    //            yield break;
    //        }
    //    }
    //}

    //private IEnumerator BackLiftCoroutine()
    //{
    //    while (true)
    //    {
    //        Lift.transform.position = Vector3.MoveTowards(LiftGoal.transform.position, Lift.transform.position, Speed * Time.deltaTime);

    //        yield return new WaitForEndOfFrame();

    //        if (Lift.transform.position.x <= 5.0f)
    //        {
    //            yield return new WaitForSeconds(1.0f);
    //            StartCoroutine("GoLiftCoroutine");
    //            StopCoroutine("BackLiftCoroutine");
    //            yield break;
    //        }
    //    }
    //}
    
    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = _col.gameObject.transform;
            Debug.Log("Btn3 Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = null;
            Debug.Log("Btn3 Exit");
        }
    }
}
