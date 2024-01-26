using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    // Reference to the player object
    public GameObject player;

    // Adjust this speed value to control the rotation speed of the spotlight
    public float rotationSpeed = 5f;

    void Update()
    {
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
}