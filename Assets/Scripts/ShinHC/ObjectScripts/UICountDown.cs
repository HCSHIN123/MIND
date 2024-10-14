using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class UICountDown : MonoBehaviour
{
    // TextMeshProUGUI 컴포넌트를 참조할 변수
    private TextMeshProUGUI text;

    // 1초 간격의 WaitForSecondsRealtime 객체
    private WaitForSecondsRealtime wait1 = new WaitForSecondsRealtime(1f);

    // 카운트다운이 진행 중인지 나타내는 변수
    public bool isCounting = false;

    // 카운트다운이 완료되었을 때 호출할 액션
    public Action onComplete = null;

    private void Awake()
    {
        // TextMeshProUGUI 컴포넌트를 초기화
        text = GetComponent<TextMeshProUGUI>();

        // 컴포넌트가 제대로 할당되었는지 확인하는 로깅을 추가할 수 있음
        if (text == null)
        {
            Debug.LogError("TextMeshProUGUI component is not assigned.");
        }
    }

    // 카운트다운을 시작하는 메서드
    public void CountDown(int _time, Action _action)
    {
        // 완료 후 호출할 액션을 설정
        onComplete = _action;

        // 카운트다운이 진행 중임을 표시
        isCounting = true;

        // 카운트다운 코루틴을 시작
        StartCoroutine(COR_CountDown(_time));
    }

    // 카운트다운을 수행하는 코루틴
    IEnumerator COR_CountDown(int _time)
    {
        // 지정된 시간 동안 카운트다운을 수행
        for (int i = _time; i > 0; --i)
        {
            // 텍스트를 현재 카운트값으로 설정
            text.text = i.ToString();

            // 1초 동안 대기
            yield return wait1;
        }

        // 카운트다운이 완료된 후 텍스트를 "0"으로 설정
        text.text = "0";

        // 카운트다운 완료 후 액션이 설정되어 있으면 호출
        onComplete?.Invoke();

        // 카운트다운이 완료되었음을 표시
        isCounting = false;
    }
}
