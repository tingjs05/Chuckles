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
    [SerializeField] private float movingPenalty = 65f;
    [SerializeField] private float distancePenalty = 25f;
    [SerializeField] private float centralisedPenalty = 10f;
    
    // components
    private FlashlightController flashlight;
    private Movement movement;
    private Collider[] captureAreaColliders;

    // private variables for calculating stuff
    private float pictureQuality;
    private bool movingWhenPictureTaken;
    private Vector3 centreLine;
    private Vector3 directionOfEnemy;

    // public events
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

    void CapturedEnemy(Transform enemy)
    {
        pictureQuality = basePictureQuality;

        // deduct picture quality if character is moving when player is taking the photo
        if (movingWhenPictureTaken) pictureQuality -= movingPenalty;

        // deduct picture quality based on how centralised or how far the enemy is
        pictureQuality -= centralisedPenalty * CalculateCentralisedPenaltyScale(enemy);

        // ensure picture quality does not drop below 0
        pictureQuality = pictureQuality < 0f? 0f : pictureQuality;

        // invoke event and pass in picture quality
        TakenPictureOfEnemy?.Invoke(pictureQuality);
        Debug.Log("Captured Clown! Picture Quality: " + pictureQuality);
    }

    float CalculateCentralisedPenaltyScale(Transform enemy)
    {
        // get direction player is poiting, and ignore y-axis
        Debug.DrawRay(captureArea.transform.position, captureArea.transform.forward * 10f, Color.red);
        centreLine = captureArea.transform.forward;
        centreLine.y = 0f;

        // compare with direction of enemy from player, and ignore y-axis
        directionOfEnemy = (enemy.position - transform.position).normalized;
        directionOfEnemy.y = 0f;

        // calculate dot product of direction and centre
        return 1f - (Vector3.Dot(directionOfEnemy, centreLine) / Vector3.Dot(centreLine, centreLine)) * centreLine.magnitude;
    }
}
