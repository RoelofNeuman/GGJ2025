using JetBrains.Annotations;
using UnityEngine;

public class ResizeTest : MonoBehaviour
{
    public float growthRate = 0.1f; // Growth rate per frame
    public Vector3 growthDirection = Vector3.left; // Direction of resizing (default: top face)

    void Update()
    {
        // Check if the "W" key is pressed
        if (Input.GetKey(KeyCode.W))
        {
            // Scale the box
            transform.localScale += growthRate * growthDirection;

            // Move the box to keep one face fixed
            transform.position += (growthRate / 2) * growthDirection;
        }
    }
}


