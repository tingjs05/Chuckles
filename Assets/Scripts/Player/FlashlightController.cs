using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FlashlightController : MonoBehaviour
{
    // Adjust this speed value to control the rotation speed of the spotlight
    [Header("Control Values")]
    [SerializeField] private float rotationSpeed = 5f;

    // References to the lights
    [Header("LightReferences")]
    [SerializeField] private Light flashlight;
    [SerializeField] private Light lantern;

    [Header("Flashlight Variables")]
    [SerializeField] private float flashlightSpotAngle_flashlight = 60f;
    [SerializeField] private float lanternSpotAngle_flashlight = 60f;

    [Header("Camera Variables")]
    [SerializeField] private float flashlightSpotAngle_camera = 100f;
    [SerializeField] private float lanternSpotAngle_camera = 90f;
    [SerializeField] private float cameraCooldown = 2f;
    // camera flash color
    [SerializeField] private Color cameraFlashColor = Color.white;

    // private variables
    private Color originalFlashlightColor;
    private Coroutine cameraFlash;

    // public properties
    public bool IsHoldingFlashlight { get; private set; } = true;

    // public events
    public event Action TakePicture;

    private void Start()
    {
        originalFlashlightColor = flashlight.color;
    }

    void Update()
    {
        HandleInputs();
        HandleFlashlightRotation();
    }

    void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            IsHoldingFlashlight = !IsHoldingFlashlight;
            SwapItem();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (IsHoldingFlashlight)
            {
                flashlight.enabled = !flashlight.enabled;
            }
            else if (cameraFlash == null)
            {
                cameraFlash = StartCoroutine(CameraFlash());
            }
        }
    }

    void HandleFlashlightRotation()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a ray in the game world
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Create a plane at the player's position to intersect with the ray
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        float hitDistance;

        // Check if the ray intersects with the player's plane
        if (playerPlane.Raycast(ray, out hitDistance))
        {
            // Get the point of intersection on the plane
            Vector3 targetPoint = ray.GetPoint(hitDistance);

            // Calculate the direction from the player to the target point
            Vector3 lookDirection = targetPoint - transform.position;
            lookDirection.y = 0; // Keep the rotation in the horizontal plane

            // Rotate the spotlight towards the target point
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            flashlight.transform.rotation = Quaternion.Slerp(flashlight.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    void SwapItem()
    {
        if (IsHoldingFlashlight)
        {
            flashlight.enabled = true;
            flashlight.spotAngle = flashlightSpotAngle_flashlight;
            lantern.spotAngle = lanternSpotAngle_flashlight;
            flashlight.color = originalFlashlightColor;
        }
        else
        {
            flashlight.enabled = false;
            flashlight.spotAngle = flashlightSpotAngle_camera;
            lantern.spotAngle = lanternSpotAngle_camera;
            flashlight.color = cameraFlashColor;
        }
    }

    IEnumerator CameraFlash()
    {
        flashlight.enabled = true;
        yield return new WaitForSeconds(0.4f);
        flashlight.enabled = false;
        yield return new WaitForSeconds(0.1f);
        flashlight.enabled = true;
        yield return new WaitForSeconds(0.1f);
        flashlight.enabled = false;
        // take picture
        TakePicture?.Invoke();
        // wait for cooldown before resetting
        yield return new WaitForSeconds(cameraCooldown);
        cameraFlash = null;
    }
}