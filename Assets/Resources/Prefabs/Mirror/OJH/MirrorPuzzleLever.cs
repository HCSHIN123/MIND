using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPuzzleLever : MonoBehaviour
{
    private Transform target = null;

    [SerializeField] private GameObject lamp1 = null;
    [SerializeField] private GameObject lamp2 = null;
    [SerializeField] private GameObject lamp3 = null;

    private void Update()
    {
        if ((target != null) && Input.GetKeyDown(KeyCode.E))
        {

            //상호작용할 코드
            // 나의 태그가 Lever1이면
            if (this.CompareTag("Lever1"))
            {
                Debug.Log("Lever1");

                if (lamp1.activeSelf)
                {
                    // Debug.Log("lamp1-1");
                    lamp1.SetActive(false);
                }
                else if (!lamp1.activeSelf)
                {
                    // Debug.Log("lamp1-2");
                    lamp1.SetActive(true);
                }

                if (lamp2.activeSelf)
                {
                    lamp2.SetActive(false);
                }
                else if (!lamp2.activeSelf)
                {
                    lamp2.SetActive(true);
                }
            }
            // 나의 태그가 Lever2이면
            if (this.CompareTag("Lever2"))
            {
                Debug.Log("Lever2");

                if (lamp2.activeSelf)
                {
                    // Debug.Log("lamp1-1");
                    lamp2.SetActive(false);
                }
                else if (!lamp2.activeSelf)
                {
                    // Debug.Log("lamp1-2");
                    lamp2.SetActive(true);
                }

                if (lamp3.activeSelf)
                {
                    lamp3.SetActive(false);
                }
                else if (!lamp3.activeSelf)
                {
                    lamp3.SetActive(true);
                }
            }
            // 나의 태그가 Lever3이면
            if (this.CompareTag("Lever3"))
            {
                Debug.Log("Lever3");

                if (lamp3.activeSelf)
                {
                    // Debug.Log("lamp1-1");
                    lamp3.SetActive(false);
                }
                else if (!lamp3.activeSelf)
                {
                    // Debug.Log("lamp1-2");
                    lamp3.SetActive(true);
                }

                if (lamp1.activeSelf)
                {
                    lamp1.SetActive(false);
                }
                else if (!lamp1.activeSelf)
                {
                    lamp1.SetActive(true);
                }
            }

            Debug.Log("1");
        }
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.tag == "Player")
        {
            target = _col.gameObject.transform;
            Debug.Log("collision Enter");
        }
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.gameObject.tag == "Player")
        {
            target = null;
            Debug.Log("collision Exit");
        }
    }
}