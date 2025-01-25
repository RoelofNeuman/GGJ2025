using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public List<Transform> waypoints;  // List of waypoints (target positions)
    public float moveSpeed = 5f; // Speed at which the player moves toward the waypoint
    public KeyCode moveKey = KeyCode.Space; // Key to press to move to the next waypoint

    private int currentWaypointIndex = 0; // Index to track the current waypoint
    private bool isMoving = false; // Flag to track whether the player is moving

    private void Update()
    {
        // Check if the move key is pressed and there are waypoints in the list
        if (Input.GetKeyDown(moveKey) && !isMoving && waypoints.Count > 0)
        {
            // Start moving to the next waypoint if not already moving
            isMoving = true;
            MoveToNextWaypoint();
        }

        // If the player is moving, interpolate toward the current waypoint
        if (isMoving && waypoints.Count > 0)
        {
            MoveToWaypoint();
        }
    }

    private void MoveToWaypoint()
    {
        // Move smoothly towards the current waypoint using MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);

        // If the player has reached the current waypoint, stop moving and delete it
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            // Delete the current waypoint and go to the next one
            Destroy(waypoints[currentWaypointIndex].gameObject);

            // Check if it's the last waypoint
            if (currentWaypointIndex == waypoints.Count - 1)
            {
                EndGame();
            }
            else
            {
                currentWaypointIndex++; // Move to the next waypoint
                isMoving = false; // Stop moving until the next key press
            }
        }
    }

    private void MoveToNextWaypoint()
    {
        // Move smoothly to the next waypoint (called after key press)
        if (currentWaypointIndex < waypoints.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);
        }
    }

    private void EndGame()
    {
        // Logic to end the game
        Debug.Log("You have reached the last waypoint! Game Over.");
        // You can add additional logic here like quitting the game or loading a new scene.
        Application.Quit(); // This will close the game if running as a build
        UnityEditor.EditorApplication.isPlaying = false; // Uncomment this line for Editor play mode (stop game in the editor)
    }
}
