using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 50f; //滚动速度
    private RectTransform rectTransform;
    private float endPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        endPosition = rectTransform.rect.height + Screen.height;//计算滚动结束位置
    }

    private void Update()
    {
        //滚动文本
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.unscaledDeltaTime;

        //超出屏幕后退出游戏
        if (rectTransform.anchoredPosition.y > endPosition)
        {
            Application.Quit();
        }
    }
    
}