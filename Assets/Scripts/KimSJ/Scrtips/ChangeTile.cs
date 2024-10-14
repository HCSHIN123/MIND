using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTile : MonoBehaviour
{
    //[SerializeField] private GameObject tile = null;
    //private Transform target = null;
    //private bool isLight = false;

    ////private GameObject LightTile = null;
    ////private GameObject ShadowTile = null;

    //[SerializeField] private GameObject[] mats = new GameObject[2];
    //private int state = 0;
    //public static int LStack = 0;
    ////private void Start()
    ////{
    ////    LightTile = Resources.Load<GameObject>("Prefabs/LightTile");
    ////    ShadowTile = Resources.Load<GameObject>("Prefabs/ShadowTile");
    ////}

    //private void Update()
    //{
    //    if ((target != null) && Input.GetKeyDown(KeyCode.E))
    //    {
    //        if (isLight == false)
    //        {
    //            OnLight(state);
    //            state = 1;
    //        }
    //        else if (isLight == true)
    //        {
    //            OffLight(state);
    //            state = 0;
    //        }
    //    }
    //    Debug.Log(LStack);
    //}

    //private void OnLight(int _state)
    //{
    //    if (LStack > 0)
    //    {
    //        tile.GetComponent<MeshRenderer>().material = mats[_state];
    //        isLight = true;
    //        LStack = LStack - 1 ;
    //    }
    //}

    //private void OffLight(int _state)
    //{
    //    tile.GetComponent<MeshRenderer>().material = mats[_state];
    //    isLight = false;
    //    LStack = LStack + 1;
    //}

    //private void OnTriggerEnter(Collider _col)
    //{
    //    if (_col.CompareTag("Player"))
    //    {
    //        target = _col.gameObject.transform;
    //        Debug.Log("Tile Enter");
    //    }
    //}

    //private void OnTriggerExit(Collider _col)
    //{
    //    if (_col.CompareTag("Player"))
    //    {
    //        target = null;
    //        Debug.Log("Tile Exit");
    //    }
    //}
}
