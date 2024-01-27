using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCapture : MonoBehaviour
{
    [SerializeField] private float captureDuration = 0.5f;
    [SerializeField] private float captureDelay = 0.05f;
    [SerializeField] private Collider captureArea;

    private FlashlightController flashlight;

    // Start is called before the first frame update
    void Start()
    {
        // get flashlight component
        flashlight = GetComponent<FlashlightController>();

        // disable capture area
        captureArea.enabled = false;

        // subscript to event when pictures are taken
        if (flashlight != null) 
            flashlight.TakePicture += Capture;
        else 
            Debug.LogWarning("FlashlightController not found! Camera capture function would not work!");
    }

    void Capture()
    {
        StartCoroutine(Coroutine_Capture());
    }

    IEnumerator Coroutine_Capture()
    {
        yield return new WaitForSeconds(captureDelay);
        captureArea.enabled = true;
        yield return new WaitForSeconds(captureDuration);
        captureArea.enabled = false;
    }
}
