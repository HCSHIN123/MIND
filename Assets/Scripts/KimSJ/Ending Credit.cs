using StarterAssets;
using System.Collections;
using TMPro;
using UnityEngine;

public class EndingCredit : MonoBehaviour
{
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private AudioSource Typing;
    private float delay = 0.070f;
    [SerializeField] private string[] storyFiles = { "Story", "Story1", "Story2", "Story3", "Story4", "Story5", "Story6" };
    private float fadeDuration = 1.0f;

    ThirdPersonController cpc = null;
    
    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            End();
        }
    }
    private void End()
    {
        cpc.canUpdate = true;
        this.gameObject.SetActive(false);
    }

    public void StartStory(ThirdPersonController _cc)
    {
        cpc = _cc;
        cpc.canUpdate = false;
        StartCoroutine(LoadAndPrintStories());
    }
    private IEnumerator LoadAndPrintStories()
    {
        Typing.loop = true;

        foreach (string storyFile in storyFiles)
        {
            TextAsset storyText = Resources.Load<TextAsset>(storyFile);

            if (storyText != null)
            {
                yield return StartCoroutine(FadeIn());

                Typing.Play();  // �ؽ�Ʈ ��� ���� �� Ÿ���� �Ҹ� ���
                yield return StartCoroutine(textPrint(delay, storyText));

                Typing.Stop();  // ������ �ؽ�Ʈ ��� �� ���� ����
                yield return new WaitForSeconds(2.0f); // 2�� ��� �� ���� ���丮 ����

                yield return StartCoroutine(FadeOut());
            }
            else
            {
                Debug.LogError($"Story file {storyFile} not found in Resources");
            }
        }
        End();
    }

    public IEnumerator textPrint(float _delay, TextAsset _storyText)
    {
        string[] segments = _storyText.text.Split('\'');

        foreach (string segment in segments)
        {
            if (string.IsNullOrWhiteSpace(segment))
            {
                continue;
            }

            char[] characters = segment.ToCharArray();

            foreach (char ch in characters)
            {
                targetText.text += ch;
                yield return new WaitForSeconds(_delay);
            }

            yield return new WaitForSeconds(_delay * 5);  // ���� ���׸�Ʈ�� �Ѿ�� �� ��� ���
            targetText.text = ""; // �ؽ�Ʈ �ʱ�ȭ
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = targetText.color;
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            targetText.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 1f;
        targetText.color = color;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = targetText.color;
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            targetText.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 0f;
        targetText.color = color;
    }
}
