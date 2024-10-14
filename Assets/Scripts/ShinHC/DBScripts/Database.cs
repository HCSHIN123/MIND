using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Database : MonoBehaviour
{
    public delegate void DbCallBack();
    public DbCallBack DelConfirmID;
    public DbCallBack DelSuccessJoin;

    public delegate void DbLoginCallBack(string _msg);
    public DbLoginCallBack DelLoginResult;

    [SerializeField]
    private TextMeshProUGUI notify = null;

    [SerializeField]
    private TextMeshProUGUI successMsg = null;
    //학원    string ad = "https://192.168.2.55/"; 

    string ad = "https://192.168.200.102/";
    string myad = "http://127.0.0.1/";

    public class UserInfo
    {
        public UserInfo(string id, string pw, string name, string gender, string tel)
        {
            this.id = id;
            this.pw = pw;
            this.name = name;
            this.gender = gender;
            this.tel = tel;
        }

        public string id { get; set; }
        public string pw { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string tel { get; set; }
    }

    public void StartGetUserInfoCor(string _username)
    {
        StartCoroutine(GetUserInfoCoroutine(_username));
    }

    public IEnumerator JoinCouroutine(UserInfo _info) // 정보등록
    {
        WWWForm form = new WWWForm();
        form.AddField("joinID", _info.id);
        form.AddField("joinPW", _info.pw);
        form.AddField("joinName", _info.name);
        form.AddField("joinGender", _info.gender);
        form.AddField("joinTel", _info.tel);

        using (UnityWebRequest www = UnityWebRequest.Post(ad + "join.php", form))
        {
            www.certificateHandler = new BypassCertificate(); // 인증서 핸들러 추가
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(www.error);

            Debug.Log(www.downloadHandler.text);
        }
    }

    public IEnumerator IdCheckCoroutine(string _username) // 아이디 중복체크
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", _username);

        using (UnityWebRequest www = UnityWebRequest.Post(ad + "idcheck.php", form))
        {
            www.certificateHandler = new BypassCertificate(); // 인증서 핸들러 추가
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(www.error);
            else
            {
                if (www.downloadHandler.text == "valid") // 아이디 유효시
                    DelConfirmID?.Invoke();
                else
                    notify.text = "중복된 ID입니다";
            }
        }
    }

    private IEnumerator GetUserInfoCoroutine(string _username) // 회원정보 받아오기(로그인성공시)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", _username);

        using (UnityWebRequest www = UnityWebRequest.Post(ad + "getinfo.php", form))
        {
            www.certificateHandler = new BypassCertificate(); // 인증서 핸들러 추가
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else // 로그인 성공
            {
                Debug.Log(www.downloadHandler.text);
                string data = www.downloadHandler.text;
                User.Instance.userName = data;
                SceneManager.LoadScene("LobbyScene");
                successMsg.gameObject.SetActive(true);
                successMsg.text = data;
            }
        }
    }

    public IEnumerator LoginCoroutine(string _username, string _password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", _username);
        form.AddField("loginPass", _password);

        using (UnityWebRequest www = UnityWebRequest.Post(ad + "login.php", form))
        {
            www.certificateHandler = new BypassCertificate(); // 인증서 핸들러 추가
            yield return www.SendWebRequest();

            string msg = www.downloadHandler.text;

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(www.error);
            else
                DelLoginResult(msg);
        }
    }
}
public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true; // 모든 인증서를 신뢰
    }
}
