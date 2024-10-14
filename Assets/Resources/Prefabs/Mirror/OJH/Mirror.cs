using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    private Transform target = null;
    private Transform stand = null;
   
    private void Update()
    {
        if ((stand != null) && Input.GetKeyDown(KeyCode.E))
        {

            
            if (this.CompareTag("OccupiedMirror"))
            {
                Debug.Log("StandMirror");
                this.transform.parent = null;
                this.transform.parent = stand;
                this.transform.localPosition = new Vector3(0f, 1.4f, 0f);
                this.transform.localRotation = Quaternion.identity;
                
                this.tag = "StandMirror";
            }


        }
        
        if ((target != null) && Input.GetKeyDown(KeyCode.E))
        // if(Input.GetKeyDown(KeyCode.E))
        {

            // if (stand != null) return;
            //상호작용할 코드
            // 나의 태그가 Mirror이면
            if (this.CompareTag("Mirror"))
            {
                                
                Debug.Log("Mirror");
                this.transform.parent = target;
                // rb.useGravity = false;
                this.transform.localPosition = new Vector3(0f, 1f, 1f);
                this.transform.rotation = Quaternion.identity;
                this.tag = "OccupiedMirror";
            }
            

        }
        
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = _col.gameObject.transform;
            Debug.Log("Player collision Enter");
        }

        if (_col.CompareTag("Stand"))
        {
            stand = _col.gameObject.transform;
            Debug.Log("Stand collision Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.CompareTag("Player"))
        {
            target = null;
            Debug.Log("Player collision Exit");
        }

        if (_col.CompareTag("Stand"))
        {
            stand = null;
            Debug.Log("Stand collision Exit");
        }
    }


}