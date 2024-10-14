using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class UICountDown : MonoBehaviour
{
    // TextMeshProUGUI ������Ʈ�� ������ ����
    private TextMeshProUGUI text;

    // 1�� ������ WaitForSecondsRealtime ��ü
    private WaitForSecondsRealtime wait1 = new WaitForSecondsRealtime(1f);

    // ī��Ʈ�ٿ��� ���� ������ ��Ÿ���� ����
    public bool isCounting = false;

    // ī��Ʈ�ٿ��� �Ϸ�Ǿ��� �� ȣ���� �׼�
    public Action onComplete = null;

    private void Awake()
    {
        // TextMeshProUGUI ������Ʈ�� �ʱ�ȭ
        text = GetComponent<TextMeshProUGUI>();

        // ������Ʈ�� ����� �Ҵ�Ǿ����� Ȯ���ϴ� �α��� �߰��� �� ����
        if (text == null)
        {
            Debug.LogError("TextMeshProUGUI component is not assigned.");
        }
    }

    // ī��Ʈ�ٿ��� �����ϴ� �޼���
    public void CountDown(int _time, Action _action)
    {
        // �Ϸ� �� ȣ���� �׼��� ����
        onComplete = _action;

        // ī��Ʈ�ٿ��� ���� ������ ǥ��
        isCounting = true;

        // ī��Ʈ�ٿ� �ڷ�ƾ�� ����
        StartCoroutine(COR_CountDown(_time));
    }

    // ī��Ʈ�ٿ��� �����ϴ� �ڷ�ƾ
    IEnumerator COR_CountDown(int _time)
    {
        // ������ �ð� ���� ī��Ʈ�ٿ��� ����
        for (int i = _time; i > 0; --i)
        {
            // �ؽ�Ʈ�� ���� ī��Ʈ������ ����
            text.text = i.ToString();

            // 1�� ���� ���
            yield return wait1;
        }

        // ī��Ʈ�ٿ��� �Ϸ�� �� �ؽ�Ʈ�� "0"���� ����
        text.text = "0";

        // ī��Ʈ�ٿ� �Ϸ� �� �׼��� �����Ǿ� ������ ȣ��
        onComplete?.Invoke();

        // ī��Ʈ�ٿ��� �Ϸ�Ǿ����� ǥ��
        isCounting = false;
    }
}
