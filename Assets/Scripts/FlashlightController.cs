using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FlashlightController : MonoBehaviour
{
    // Reference to the player object
    public GameObject player;

    // Adjust this speed value to control the rotation speed of the spotlight
    public float rotationSpeed = 5f;

    // References to the lights
    private Light flashlight;
    private Light lantern;

    private bool isHoldingFlashlight = true;

    [SerializeField] private float CameraCooldown;
    private float timeSinceLastFlash;

    private void Start()
    {
        flashlight = GameObject.Find("Flashlight").GetComponent<Light>();
        lantern = GameObject.Find("Lantern").GetComponent<Light>();

        timeSinceLastFlash = CameraCooldown;
    }

    void Update()
    {
        // Handling Inputs 
        HandleInputs();

        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a ray in the game world
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Create a plane at the player's position to intersect with the ray
        Plane playerPlane = new Plane(Vector3.up, player.transform.position);

        float hitDistance;

        // Check if the ray intersects with the player's plane
        if (playerPlane.Raycast(ray, out hitDistance))
        {
            // Get the point of intersection on the plane
            Vector3 targetPoint = ray.GetPoint(hitDistance);

            // Calculate the direction from the player to the target point
            Vector3 lookDirection = targetPoint - player.transform.position;
            lookDirection.y = 0; // Keep the rotation in the horizontal plane

            // Rotate the spotlight towards the target point
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isHoldingFlashlight = !isHoldingFlashlight;
            SwapItem();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (flashlight.enabled && isHoldingFlashlight)
            {
                flashlight.enabled = false;
            }
            else if (isHoldingFlashlight)
            {
                flashlight.enabled = true;
            }
            else if (timeSinceLastFlash > CameraCooldown)
            {
                timeSinceLastFlash = 0f;
                StartCoroutine(CameraFlash());
            }
        }
        timeSinceLastFlash += Time.deltaTime;
    }

    void SwapItem()
    {
        if (isHoldingFlashlight)
        {
            flashlight.enabled = true;
            flashlight.spotAngle = 60f;
            lantern.spotAngle = 60f;
        }
        else
        {
            flashlight.enabled = false;
            flashlight.spotAngle = 100f;
            lantern.spotAngle = 90f;
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
        //Add take picture function calling?
    }
}