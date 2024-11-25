using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private HitPointsEvent hitPointsEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        hitPointsEvent = transform.parent.GetComponent<HitPointsEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HitPoint()
    {
        AudioSourceManager.Instance.PlaySound("Hit");
        hitPointsEvent.hitNum++;
        gameObject.SetActive(false);
    }
}
