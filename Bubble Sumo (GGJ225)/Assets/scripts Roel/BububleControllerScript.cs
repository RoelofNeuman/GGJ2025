using UnityEngine;
using System.Collections;

public class BububleControllerScript : MonoBehaviour
{
    public ParticleSystem bubbleParticlesOne;
    public ParticleSystem bubbleParticlesTwo;
    private GameObject playerOne;
    private GameObject playerTwo;
    private float playerDistance;
    private float bubbleDuration;
  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        bubbleDuration = bubbleParticlesOne.main.startLifetime.constant;
        playerOne = GameObject.FindGameObjectWithTag("Player1");
        playerTwo = GameObject.FindGameObjectWithTag("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate Distance between players
        playerDistance = Vector3.Distance(playerOne.transform.position, playerTwo.transform.position);
        float playerMiddleDistance = playerDistance / 2;


        var mainOne = bubbleParticlesOne.main;
        mainOne.startSpeed = playerMiddleDistance / bubbleDuration;
        var mainTwo = bubbleParticlesTwo.main;
        mainTwo.startSpeed = playerMiddleDistance / bubbleDuration;


    }
}
