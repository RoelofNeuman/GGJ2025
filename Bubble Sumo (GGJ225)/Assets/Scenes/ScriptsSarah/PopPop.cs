using UnityEngine;

public class PopPop : MonoBehaviour
{
    public ParticleSystem bubbleParticles;
    public float playerDistance;
    private float bubbleDuration;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        bubbleDuration = bubbleParticles.main.startLifetime.constant;
    }

    // Update is called once per frame
    void Update()
    {
        float playerMiddleDistance = playerDistance / 2;
        var main = bubbleParticles.main;
        main.startSpeed = playerMiddleDistance / bubbleDuration;


    }

    
}
