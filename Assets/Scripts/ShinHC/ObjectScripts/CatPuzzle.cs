using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CatPuzzle : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject top;
    [SerializeField] GameObject mid;
    [SerializeField] GameObject bottom;
    [SerializeField] private AudioSource successSound;
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public Animator animator2;

    private void Update()
    {
        if (CheckAnswer())
        {
            //Debug.Log("성공!");
            
            //Debug.Log("Open Door");
            GetComponent<PhotonView>().RPC("EndingSceneLoad", RpcTarget.All);
            
        }
        else
            Debug.Log("실패!");
    }

    [PunRPC]
    public void EndingSceneLoad()
    {
        successSound.Play();
        animator.SetBool("Labber", true);
        animator2.SetBool("Labber", true);
    }
                
    private bool CheckAnswer()
    {
        return (Mathf.Abs( top.transform.localPosition.z - mid.transform.localPosition.z) < 2f &&
            Mathf.Abs(mid.transform.localPosition.z - bottom.transform.localPosition.z) < 2f);
    }
}
