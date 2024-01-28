using System;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public Light sun;
    public float minutesInFullDay = 10f;

    public float dayIntensity = 1f;
    public float nightIntensity = 0.1f;

    [Range(0, 1)]
    public float currentTimeNormalised = 0f;

    public float timeMultiplier = 1f;

    public bool isDay => Math.Abs(currentTimeNormalised - 1f) < 0.01f;


    public event Action DayEnded;

    [EasyButtons.Button]
    public void ResetDay() { currentTimeNormalised = 0; }

    public void SetTime(float time)
    {
        currentTimeNormalised = Mathf.Clamp01(time);
        sun.intensity = Mathf.Lerp(nightIntensity, dayIntensity, currentTimeNormalised);
        if (isDay) DayEnded?.Invoke();
    }

    private void FixedUpdate()
    {
        if (isDay) return;
        SetTime(currentTimeNormalised + (Time.deltaTime / (minutesInFullDay * 60f)) * timeMultiplier);
    }
}