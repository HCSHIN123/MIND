using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartCredit : MonoBehaviour
{
    [SerializeField] private Image bg; 
    [SerializeField] private TMP_Text targetText;
    private float delay = 0.0625f;
    [SerializeField]
    private TextAsset storyText = null;

    public bool isReading = true;

    private void Start()
    {
        bg.enabled = false;
       
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) 
        {
            StopAllCoroutines();
            targetText.text = " ";
            bg.enabled = false;
            this.gameObject.SetActive(false);
        }
    }
    public void PlayerCredit()
    {

        StartCoroutine(textPrint(delay, storyText));
    }

    public IEnumerator textPrint(float _delay, TextAsset _storyText)
    {
        bg.enabled = true;
        string[] lines = _storyText.text.Split("`");

        for (int i = 0; i < lines.Length; i++)
        {
            char[] ch = lines[i].ToCharArray();

            for (int j = 0; j < ch.Length; j++)
            {
                targetText.text += ch[j];
                yield return new WaitForSeconds(_delay);
            }

            targetText.text = "";

            if (lines[i] == null)
            {
                break;
            }
        }
        bg.enabled = false;
        this.gameObject.SetActive(false);
    }
}