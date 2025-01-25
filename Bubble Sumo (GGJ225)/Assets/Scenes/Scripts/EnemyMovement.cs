using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    // dont touch yet plz :)


    // Public variables to control movement behavior
    public float minMoveDistance = 1f; // Minimum distance the enemy will move
    public float maxMoveDistance = 5f; // Maximum distance the enemy will move
    public float minTimeInterval = 1f; // Minimum time between movements
    public float maxTimeInterval = 3f; // Maximum time between movements
    public float movementSpeed = 2f; // Speed of smooth movement

    private float nextMoveTime; // Time until the next movement
    private Vector3 targetPosition; // Target position for smooth movement
    private bool isMoving = false; // Is the enemy currently moving

    private void Start()
    {
        // Schedule the first movement
        ScheduleNextMove();
    }

    private void Update()
    {
        // If the enemy is moving, smoothly move towards the target position
        if (isMoving)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Check if the enemy has reached the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }

        // Check if it is time to move and the enemy is not already moving
        if (Time.time >= nextMoveTime && !isMoving)
        {
            StartMove();
            ScheduleNextMove();
        }
    }

    private void StartMove()
    {
        // Determine a random distance to move forward
        float moveDistance = Random.Range(minMoveDistance, maxMoveDistance);

        // Calculate the target position based on the forward direction
        targetPosition = transform.position + transform.right * moveDistance;

        // Set the moving flag to true
        isMoving = true;

        // Optionally, log the movement (for debugging purposes)
        Debug.Log($"Enemy is moving forward by {moveDistance} units to {targetPosition}.");
    }

    private void ScheduleNextMove()
    {
        // Determine a random time interval for the next movement
        float timeInterval = Random.Range(minTimeInterval, maxTimeInterval);

        // Set the time for the next move
        nextMoveTime = Time.time + timeInterval;

        // Optionally, log the interval (for debugging purposes)
        Debug.Log($"Next move scheduled in {timeInterval} seconds.");
    }

    
}
