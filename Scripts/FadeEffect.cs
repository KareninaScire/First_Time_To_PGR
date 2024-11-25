using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public static FadeEffect instance;

    public Image fadeImage; // UI Image 用于覆盖屏幕渐变
    public float fadeDuration = 1.0f; // 渐变持续时间

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // 保持对象跨场景存在
    }

    //逐渐变黑
    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    //逐渐变透明
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));//颜色渐变
            fadeImage.color = color;
            yield return null;//暂停，等下一帧
        }

        fadeImage.gameObject.SetActive(false);
    }

    //渐变切换场景
    public IEnumerator FadeOutAndIn(System.Action onFadeComplete)
    {
        yield return FadeOut();
        onFadeComplete.Invoke();
        yield return FadeIn();
    }
}