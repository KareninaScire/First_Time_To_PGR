using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointsEvent : MonoBehaviour
{
    private bool gameStarted;
    private float startTime;
    private float timeVal;
    public int hitNum;
    public GameObject[] points;
    
    // Start is called before the first frame update
    void Start()
    {
        gameStarted = true;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            if (Time.time - startTime>= 10)
            {//游戏结束
                gameStarted = false;
                if (hitNum>=3)
                {//游戏胜利
                    GameManager.Instance.LoadNextScript(3);
                }
                else
                {//游戏失败
                    GameManager.Instance.LoadNextScript();
                }
                gameObject.SetActive(false);
            }
            else
            {//游戏进行的内容
                timeVal-=Time.deltaTime;
                if (timeVal<=0)
                {
                    ShowPoints();
                    timeVal = 2;
                }
            }
        }
    }

    private void ShowPoints()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].SetActive(false);
        }
        points[Random.Range(0, points.Length)].SetActive(true);
    }
    
    public void HitPoint(GameObject obj)
    {
        AudioSourceManager.Instance.PlaySound("Hit");
        hitNum++;
        obj.SetActive(false);
    }
}
