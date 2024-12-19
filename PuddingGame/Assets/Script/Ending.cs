using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    [SerializeField] GameObject player; //플레이어
    [SerializeField] GameObject hand; //손
    [SerializeField] float handSpeed; // 손 이동 속도
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration;
    [SerializeField] StartGame game;
    private bool endingTriggered = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            endingTriggered = true;
            StartCoroutine(PlayingEnding());
        }
    }
    IEnumerator PlayingEnding()
    {
        while(Vector3.Distance(hand.transform.position, player.transform.position) > 0.1f)
        {
            hand.transform.position = Vector3.MoveTowards(
                hand.transform.position, player.transform.position, handSpeed * Time.deltaTime);
            yield return null;
        }

        yield return StartCoroutine(FadeOut());
        game.StartButton();

        endingTriggered = false;
    }

    IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        Color fadeColor = fadeImage.color;
        float elapsedTime = 0f;
        while(elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }
    }
}
