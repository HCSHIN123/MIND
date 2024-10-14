using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;

public class Join : MonoBehaviour
{
    public delegate void JoinDelegate(string _msg);
    public JoinDelegate DelNotify;
    public delegate void JoinBtnDelegate(GameObject _me);
    public JoinBtnDelegate DelJoinBtn;

    [SerializeField]
    private TMPro.TMP_InputField idInputField = null;

    [SerializeField]
    private Button idCheckBtn = null;

    [SerializeField]
    private TMPro.TMP_InputField pwInputField = null;

    [SerializeField]
    private TMPro.TMP_InputField pwCheckInputField = null;

    [SerializeField]
    private Button pwCheckBtn = null;

    [SerializeField]
    private TMPro.TMP_InputField nameInputField = null;

    [SerializeField]
    private Toggle[] toggleBoxs = new Toggle[2];
    private string gender = "";

    [SerializeField]
    private TMPro.TMP_InputField telInputField = null;

    [SerializeField]
    private Button joinBtn = null;

    private Database db = null;
    public Database DB { set { DB = value; } get { return db; } }

    [SerializeField]
    private Sprite checkSprite = null;


    enum eCheck
    {
        ID = 0b_000001,
        PW = 0b_000010,
        PW_CHECK = 0b_000100,
        NAME = 0b_001000,
        GENDER = 0b_010000,
        TEL = 0b_100000,
    }

    private int checkNum = 0b_000000;


    private void Start()
    {
        idCheckBtn.onClick.AddListener(() => //���̵� Ȯ�ι�ư ����
        {
            if(string.IsNullOrWhiteSpace(idInputField.text) || idInputField.text.Count<char>() > 10)
            {
                DelNotify("���̵� �Է��ϼ���");
                return;
            }

            StartCoroutine(db.IdCheckCoroutine(idInputField.text));
        });

        pwCheckBtn.onClick.AddListener(() => //���Ȯ�ι�ư����
        {
            if (pwInputField.text != pwCheckInputField.text )
            {
                DelNotify("��й�ȣ ����ġ");
            }
            else if(isUsablePW(pwInputField.text) == false)
            {
                DelNotify("��й�ȣ�� ���ĺ� �빮��, �ҹ���, ���ڸ� ��� �����ؾ��մϴ�");
            }
            else
            {
                // ��й�ȣ Ȯ�� ����  
                pwCheckBtn.interactable = false;
                pwCheckBtn.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                pwCheckBtn.GetComponent<Image>().sprite = checkSprite;
                pwInputField.interactable = false;
                pwCheckInputField.interactable = false;
                checkNum |= (int)eCheck.PW;
                checkNum |= (int)eCheck.PW_CHECK;
            }
        });

        joinBtn.onClick.AddListener(() =>// ȸ�����Թ�ư ����
        {
            Debug.Log(checkNum);
            if(isJoinable())
            {
                //insert
                DelNotify("ȸ������ ����");

                StartCoroutine(db.JoinCouroutine(new Database.UserInfo(idInputField.text,
                    pwInputField.text, nameInputField.text, gender, telInputField.text)));
                DelJoinBtn?.Invoke(this.gameObject);
            }
        });

        db.DelConfirmID = () => // ���̵� ��ȿ��Ȯ���� ó���� �ݹ� ����
        {
            idCheckBtn.interactable = false;
            idCheckBtn.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            idCheckBtn.GetComponent<Image>().sprite = checkSprite;
            idInputField.interactable = false;
            checkNum |= (int)eCheck.ID;
        };

    }

    private bool isUsablePW(string _pw)//�빮�� �ҹ��� ���� + 8~10�ڸ� ȥ������ üũ
    {
        if(_pw.Count<char>() < 8 || _pw.Count<char>() > 10)
            return false;
        
        //0:�빮�� 1:�ҹ��� 2:����
        bool[] bCondition = new bool[3]{ false, false, false};

        foreach(char ch in _pw)
        {
            if(ch >= 'A' && ch <= 'Z')
            {
                bCondition[0] = true;
                continue;
            }
            if(ch >= 'a' && ch <= 'z')
            {
                bCondition[1] = true;
                continue;
            }
            if(ch >= '0' &&  ch <= '9')
            {
                bCondition[2] = true;
                continue;
            }
        }

        foreach(bool b in bCondition)
        {
            if (b == false)
                return false;
        }

        return true;
    }
    private bool isJoinable()
    {

        if (nameInputField.text.Length == 0)
        {
           DelNotify("�̸��� �Է��ϼ���");
            checkNum &= (int)eCheck.NAME;
            return false;
        }
        checkNum |= (int)eCheck.NAME;

        if (toggleBoxs[0].isOn || toggleBoxs[1].isOn)
        {
            gender = (toggleBoxs[0].isOn ? "men" : "women");
            checkNum |= (int)eCheck.GENDER;
        }
        else
        {
            DelNotify("������ �����ϼ���");
            checkNum &= (int)eCheck.GENDER;
            return false;
        }

        if(telInputField.text.Length == 0)
        {
            DelNotify("��ȭ��ȣ�� �Է��ϼ���");
            return false;
        }

        checkNum |= (int)eCheck.TEL;

        if (checkNum == 0b111111)
            return true;

        return false;
    }
}
