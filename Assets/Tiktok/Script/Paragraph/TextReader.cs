using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextReader : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;

    [Header("CUSTOMIZE")]
    [SerializeField] private float letterDelay;
    [SerializeField] private float wordDelay;

    [SerializeField] private string[] paragraphs;

    private void Awake()
    {
        StartCoroutine(Read());
    }

    private IEnumerator Read()
    {
        WaitForSeconds waitForLetter = new WaitForSeconds(letterDelay);
        WaitForSeconds waitForWord = new WaitForSeconds(wordDelay);

        string currentSentence = "";

        displayText.text = currentSentence;

        yield return new WaitForSeconds(1);

        while (true)
        {
            for (int i = 0; i < paragraphs.Length; i++)
            {
                for (int j = 0; j < paragraphs[i].Length; j++)
                {
                    currentSentence += paragraphs[i][j];

                    displayText.text = currentSentence;

                    if (paragraphs[i][j] == ' ')
                    {
                        yield return waitForWord;
                    }
                    else
                    {
                        yield return waitForLetter;
                    }
                }

                currentSentence = "";

                yield return waitForWord;
            }

            yield return waitForWord;
        }
    }
}
