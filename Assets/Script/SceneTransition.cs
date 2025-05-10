using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image transitionImage;
    public float fadeTime = 1f;

    void Start()
    {
        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        yield return StartCoroutine(FadeOut());
    }

    public void CallTransition()
    {
        StartCoroutine(TransitionEffect());
    }

    public void EndGame(string sceneName)
    {
        StartCoroutine(EndGameCoroutine(sceneName));
    }

    private IEnumerator TransitionEffect()
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator EndGameCoroutine(string sceneName)
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = transitionImage.color;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeTime);
            transitionImage.color = c;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = transitionImage.color;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeTime);
            transitionImage.color = c;
            yield return null;
        }
    }
}
