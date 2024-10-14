using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorHolder : MonoBehaviour
{
    [SerializeField]
    Mirror[] mirrors = null;
    [SerializeField]
    GameObject lightLoad = null;
    public bool go = false;
    private void Update()
    {
        foreach(Mirror mirror in mirrors)
        {
            if (mirror.gameObject.CompareTag("StandMirror") == false)
                return;
        }

        lightLoad.SetActive(true);
        go = true;
    }
}
