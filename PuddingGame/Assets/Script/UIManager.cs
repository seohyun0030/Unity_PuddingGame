using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI TestText;
    public Image fadeImage;
    public float fadeOutTime = .5f;
    public void Start()
    {
        if(fadeImage != null)
        {
            StartCoroutine(FadeOut());
        }
    }
    private IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;

        while (color.a > 0)
        {
            color.a -= Time.deltaTime * fadeOutTime;
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);

    }
}
