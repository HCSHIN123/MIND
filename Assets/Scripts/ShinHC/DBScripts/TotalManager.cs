using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TotalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject join = null;
    [SerializeField]
    private GameObject login = null;

    [SerializeField]
    private TextMeshProUGUI notify = null;

    private void Awake()
    {
        login.SetActive(true);
        join.SetActive(false);

        Login lo = login.GetComponent<Login>();
        lo.DelNotify = Notify;
        lo.DelJoinBtn = SwitchUI;

        Join jo = join.GetComponent<Join>();
        jo.DelNotify = Notify;
        jo.DelJoinBtn = SwitchUI;
    }


    private void Notify(string _msg)
    {
        StartCoroutine(NotifyCoroutine(_msg));
    }

    private IEnumerator NotifyCoroutine(string _msg)
    {
        notify.text = _msg;
        yield return new WaitForSecondsRealtime(5f);
        notify.text = "";
    }

    private void SwitchUI(GameObject _other)
    {
        if (_other == join)
        {
            join?.SetActive(false);
            login?.SetActive(true);
        }
        else
        {
            login?.SetActive(false);
            join?.SetActive(true);
        }

    }

}
