using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string userName;

    private static User _instance;

    // 인스턴스에 접근할 수 있는 공용 프로퍼티
    public static User Instance
    {
        get
        {
            // 인스턴스가 없으면 새로 생성합니다.
            if (_instance == null)
            {
                // 씬에 SingletonExample 오브젝트가 있는지 찾습니다.
                _instance = FindObjectOfType<User>();

                // 만약 오브젝트가 없다면 새로 생성합니다.
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<User>();
                    singletonObject.name = typeof(User).ToString() + " (Singleton)";

                    // 씬 전환 시 파괴되지 않도록 설정합니다.
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    // 싱글톤 인스턴스가 이미 있는지 확인하는 보호자 역할
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

    // SingletonExample 클래스에서 제공할 메서드들
    public void MySingletonMethod()
    {
        Debug.Log("Singleton method called!");
    }
}
