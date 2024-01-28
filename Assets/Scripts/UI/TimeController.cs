using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeController : MonoBehaviour
{
    private float currentTime;
    public Image watchTimer;
    public float totalTime;
    public DayCycle dayCycle;
    private void Start()
    {
        currentTime = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        watchTimer.fillAmount = 1-dayCycle.currentTimeNormalised;
        // if (currentTime > 0)
        // {
        //     currentTime -= Time.deltaTime;
        //     
        // }
        // else
        // {
        //     // wtv goes here
        // }

    }
}
