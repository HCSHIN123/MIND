using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string userName;

    private static User _instance;

    // �ν��Ͻ��� ������ �� �ִ� ���� ������Ƽ
    public static User Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ���� �����մϴ�.
            if (_instance == null)
            {
                // ���� SingletonExample ������Ʈ�� �ִ��� ã���ϴ�.
                _instance = FindObjectOfType<User>();

                // ���� ������Ʈ�� ���ٸ� ���� �����մϴ�.
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<User>();
                    singletonObject.name = typeof(User).ToString() + " (Singleton)";

                    // �� ��ȯ �� �ı����� �ʵ��� �����մϴ�.
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    // �̱��� �ν��Ͻ��� �̹� �ִ��� Ȯ���ϴ� ��ȣ�� ����
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // SingletonExample Ŭ�������� ������ �޼����
    public void MySingletonMethod()
    {
        Debug.Log("Singleton method called!");
    }
}
