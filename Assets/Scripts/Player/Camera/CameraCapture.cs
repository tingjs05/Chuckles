using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCapture : MonoBehaviour
{
    [Header("Capturing Variables")]
    [SerializeField] private float captureDuration = 0.5f;
    [SerializeField] private float captureDelay = 0.05f;
    [SerializeField] private CameraCaptureArea captureArea;

    [Header("Capture Quality")]
    [SerializeField] private float basePictureQuality = 100f;
    [SerializeField] private float movingPenalty = 10f;
    
    private FlashlightController flashlight;
    private Movement movement;
    private Collider[] captureAreaColliders;
    private float pictureQuality;
    private bool movingWhenPictureTaken;

    public static event Action<float> TakenPictureOfEnemy;

    // Start is called before the first frame update
    void Start()
    {
        // get components
        flashlight = GetComponent<FlashlightController>();
        movement = GetComponent<Movement>();
        captureAreaColliders = captureArea.GetComponents<Collider>();

        // set whatever this variable is
        movingWhenPictureTaken = false;

        // disable capture area
        SetCaptureAreaCollider(false);

        // subscribe to event when pictures are taken
        if (flashlight != null) flashlight.TakePicture += Capture;
        else Debug.LogWarning("FlashlightController not found! Camera capture function would not work!");
        
        // subscribe to camera capture event
        if (captureArea != null) captureArea.CapturedEnemy += CapturedEnemy;
        else Debug.LogWarning("CameraCaptureArea is not set! Camera capture function would not work!");
    }

    void SetCaptureAreaCollider(bool enabled)
    {
        foreach (Collider collider in captureAreaColliders)
        {
            collider.enabled = enabled;
        }
    }

    void Capture()
    {
        StartCoroutine(Coroutine_Capture());
    }

    IEnumerator Coroutine_Capture()
    {
        // check if player is moving when picture is being taken
        if (movement != null && movement.IsMoving) movingWhenPictureTaken = true;

        yield return new WaitForSeconds(captureDelay);
        SetCaptureAreaCollider(true);
        yield return new WaitForSeconds(captureDuration);
        SetCaptureAreaCollider(false);

        // reset the moving check thingy
        movingWhenPictureTaken = false;
    }

    void CapturedEnemy()
    {
        Debug.Log("Captured Clown!");

        pictureQuality = basePictureQuality;

        // deduct picture quality if character is moving when player is taking the photo
        if (movingWhenPictureTaken) pictureQuality -= movingPenalty;

        // ensure picture quality does not drop below 0
        pictureQuality = pictureQuality < 0f? 0f : pictureQuality;

        // invoke event and pass in picture quality
        TakenPictureOfEnemy?.Invoke(pictureQuality);
    }
}
