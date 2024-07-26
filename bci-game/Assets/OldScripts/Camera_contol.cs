using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Define the desired y position
    public float desiredY = 5f;

    // Update is called once per frame
    void Update()
    {
        // Get the current position of the camera
        Vector3 currentPosition = transform.position;

        // Set the y position to the desired y value
        currentPosition.y = desiredY;

        // Update the camera's position
        transform.position = currentPosition;
    }
}
