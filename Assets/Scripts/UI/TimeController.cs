using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeController : MonoBehaviour
{
    private float currentTime;
    public Image watchTimer;
    public float totalTime;

    private void Start()
    {
        currentTime = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            watchTimer.fillAmount = currentTime / totalTime;
        }
        else
        {
            Debug.Log("Game Over");
        }

    }
}
