using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    //only for the death waypoint 

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (CompareTag("Player"))
    //    {
    //        Application.Quit();
    //        Debug.LogError("no work");
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FinalWaypoint"))
        {
            Destroy(gameObject);
            Application.Quit();
            Debug.LogError("player erradicated");
        }
    }




}
