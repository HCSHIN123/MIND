using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Networking;
using TMPro; //웹서버접근시

public class Login : MonoBehaviour
{
    public delegate void LoginDelegate(string _msg);
    public LoginDelegate DelNotify;
    public delegate void JoinBtnDelegate(GameObject _me);
    public JoinBtnDelegate DelJoinBtn;

    [SerializeField]
    private TMPro.TMP_InputField idInputField = null;
    [SerializeField]
    private TMPro.TMP_InputField pwInputField = null;

    [SerializeField]
    private Button loginBtn = null;

    [SerializeField]
    private Button joinBtn = null;

  
    private Database db = null;
    public Database DB { set { DB = value; } get { return db; } }
    void Start()
    {
        loginBtn.onClick.AddListener(() =>
        {
            StartCoroutine(db.LoginCoroutine(idInputField.text, pwInputField.text));
        });

        joinBtn.onClick.AddListener(() =>
        {
            DelJoinBtn?.Invoke(this.gameObject);
        });

        db.DelLoginResult = (string _msg) =>
        {
            LoginResult(_msg);
        };
    }

    private void LoginResult(string _msg)
    {
        switch (_msg)
        {
            case "invalid id":
                DelNotify("ID가 틀렸습니다");
                break;
            case "invalid pw":
                DelNotify("비밀번호가 틀렸습니다");
                break;
            case "success":
                DelNotify("로그인성공!!!!");
                
                db.StartGetUserInfoCor(idInputField.text);
                this.gameObject.SetActive(false);
                break;
            
            default:
                break;
        }
    }
}
//php
//$ : 변수임을 표기 
//localhost:3308 = 127.0.0.1:3306
// 기본아이디는 무조건  root
//conn 접속 성공했나 안했나
// mysqli 접속
// . : 문자열 합쳐주는거
// $result->num_rows : 몇줄짜리인가
// $result->fetch_assoc() : 하나씩 빼옴, foreach 같은거
// echo : 클라에서 서버로 메세지 보내주는거