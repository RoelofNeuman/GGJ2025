using JetBrains.Annotations;
using UnityEngine;

public class ResizeTest : MonoBehaviour
{

    public float growthRate = 0.1f; // Growth rate per frame

    void Update()
    {
        // Check if the "W" key is pressed
        if (Input.GetKey(KeyCode.W))
        {
            // Increase the scale along the X-axis
            transform.localScale += new Vector3(growthRate, 0, 0);

            // Move the box to the left to keep the right face stationary
            transform.position -= new Vector3(growthRate / 2, 0, 0);
        }
    }
}




