using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag(playerTag))
        {
            EndGame(collision.collider.gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            EndGame(other.gameObject);
        }
    }

    private void EndGame(GameObject Player)
    {
        Debug.Log("gameover");
        Destroy(Player);
        
        Application.Quit();
    }





}
