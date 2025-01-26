using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class mikeInput : MonoBehaviour
{
    public float micSenitivity; // mic volume amplifier
    public TMP_Dropdown micDropdown1; 

    public List<string> mikeList = new List<string>(); //list from mic devices 
    public string micreophoneDeviceName1; //name of input mic 
    public string micreophoneDeviceName2; //2nd input

    private AudioClip micInput1; //detect input mic 
    private AudioClip micInput2; //second

    private bool micStart1; // if mic is detected 
    private bool micStart2; // second mic
    private List<float> volumeAverage1 =new List<float>();
    private List<float> volumeAverage2 =new List<float>();
    private float endScore1;
    private float endScore2;

    private int sampleWindow = 128; //samples for analysis 

    //other stuff
    public ParticleSystem bubbleParticlesOne;
    public ParticleSystem bubbleParticlesTwo;
    private GameObject playerOne;
    private GameObject playerTwo;
    private float playerDistance;
    private float bubbleDuration;



    //countdown before blow 
    public TMP_Text countdownText; // Reference to a UI Text component for the timer display
    private int countdown = 8; // Countdown duration in seconds
    public TMP_Text resultText;











    void Awake()
    {
        bubbleDuration = bubbleParticlesOne.main.startLifetime.constant;
        Debug.Log(bubbleDuration);
        playerOne = GameObject.FindGameObjectWithTag("Player1");
        playerTwo = GameObject.FindGameObjectWithTag("Player2");
    }

    private void Start() //on start, start detecting microphone input 
    {
        PopulateMic();
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {

        if (micStart1) // If the mic starts 
        {
            float volume1 = GetMicrophoneVolume(micInput1, micreophoneDeviceName1) * micSenitivity; // Get mic volume
            float volume2 = GetMicrophoneVolume(micInput2, micreophoneDeviceName2) * micSenitivity;
            // Smoothly adjust particle effect based on mic volume
            AdjustParticleEffect1(volume1);
            AdjustParticleEffect2(volume2);

            volumeAverage1.Add(volume1);
            volumeAverage2.Add(volume2);

        }

        else
        {
            micStart1= false;
        }
    }

            
        

    private void AdjustParticleEffect1(float micVolume)
    {
        playerDistance = Vector3.Distance(playerOne.transform.position, playerTwo.transform.position);
        float playerMiddleDistance = playerDistance / 2;

        // Define minimum and maximum effect values
        float minLifetime = 0.5f;
        float maxLifetime = 5f;

        float minSpeed = 1f;
        float maxSpeed = 20f;

        float minEmissionRate = 10f;
        float maxEmissionRate = 100f;

        // Smoothly interpolate between the minimum and maximum values
        float lifetime = Mathf.Lerp(minLifetime, maxLifetime, micVolume); // Scale lifetime smoothly
        float speed = Mathf.Lerp(minSpeed, maxSpeed, micVolume);          // Scale speed smoothly
        float emissionRate = Mathf.Lerp(minEmissionRate, maxEmissionRate, micVolume); // Scale emission rate smoothly

        // Apply the interpolated values to the particle system
        var mainOne = bubbleParticlesOne.main;
        mainOne.startLifetime = lifetime;
        mainOne.startSpeed = speed;

        var emission = bubbleParticlesOne.emission;
        emission.rateOverTime = emissionRate;

        // Adjust the particle size based on microphone volume
        float minSize = 0.5f;  // Minimum size of the particles
        float maxSize = 2f;    // Maximum size of the particles

        // Use micVolume to interpolate between the minSize and maxSize
        float particleSize = Mathf.Lerp(minSize, maxSize, micVolume);

        // Apply the size change
        var sizeOverLifetime = bubbleParticlesOne.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(particleSize);
    }
    private void AdjustParticleEffect2(float micVolume)
    {
        playerDistance = Vector3.Distance(playerOne.transform.position, playerTwo.transform.position);
        float playerMiddleDistance = playerDistance / 2;

        // Define minimum and maximum effect values
        float minLifetime = 0.5f;
        float maxLifetime = 5f;

        float minSpeed = 1f;
        float maxSpeed = 20f;

        float minEmissionRate = 10f;
        float maxEmissionRate = 100f;

        // Smoothly interpolate between the minimum and maximum values
        float lifetime = Mathf.Lerp(minLifetime, maxLifetime, micVolume); // Scale lifetime smoothly
        float speed = Mathf.Lerp(minSpeed, maxSpeed, micVolume);          // Scale speed smoothly
        float emissionRate = Mathf.Lerp(minEmissionRate, maxEmissionRate, micVolume); // Scale emission rate smoothly

        // Apply the interpolated values to the particle system
        var mainTwo = bubbleParticlesTwo.main;
        mainTwo.startLifetime = lifetime;
        mainTwo.startSpeed = speed;

        var emission = bubbleParticlesTwo.emission;
        emission.rateOverTime = emissionRate;

        // Adjust the particle size based on microphone volume
        float minSize = 0.5f;  // Minimum size of the particles
        float maxSize = 2f;    // Maximum size of the particles

        // Use micVolume to interpolate between the minSize and maxSize
        float particleSize = Mathf.Lerp(minSize, maxSize, micVolume);

        // Apply the size change
        var sizeOverLifetime = bubbleParticlesTwo.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(particleSize);
    }

    private void PopulateMic()
    {
        //clear existing list of device check new available ones 
        mikeList.Clear();

        foreach (var device in Microphone.devices)
        {
            mikeList.Add(device);
        }

        if (mikeList.Count > 0) //populate the dropdown
        {
            micDropdown1.AddOptions(mikeList);
            micDropdown1.onValueChanged.AddListener(delegate { selectedMicrophones1(micDropdown1.value); });

            selectedMicrophones1(0); //set default microphones 
            selectedMicrophones2(1);
        }
        else
        {
            Debug.LogError("No microphone detected!");
            // Provide UI feedback if no mic is detected
            micDropdown1.GetComponentInChildren<TMP_Text>().text = "No microphones available";
        }
    }

    private void selectedMicrophones1(int index)
    {
        //set selected microphone for player 1
        if (index >= 0 && index <= mikeList.Count)
        {
            micreophoneDeviceName1 = mikeList[index];
            StartCoroutine(MicroStartingPlayer1());
        }
    }
    private void selectedMicrophones2(int index)
    {
        //set selected microphone for player 1
        if (index >= 0 && index <= mikeList.Count)
        {
            micreophoneDeviceName2 = mikeList[index];
            StartCoroutine(MicroStartingPlayer2());
        }
    }

    private IEnumerator MicroStartingPlayer1()
    {
        //initialize mic input asynchronously
        if (!string.IsNullOrEmpty(micreophoneDeviceName1))
        {
            if (micInput1 != null)
            {
                //stop prev mic input
                Microphone.End(micreophoneDeviceName1);
            }
            micInput1 = Microphone.Start(micreophoneDeviceName1, true, 10, AudioSettings.outputSampleRate); //when dont want add end so you dont get the microphone recording, when other stuff is happening stop emmiter so no play

           while (Microphone.GetPosition(micreophoneDeviceName1) <= 0)
           {
                yield return null; // Wait for the microphone to be ready without freezing the game
           }

            micStart1 = true;
        }
        else
        {
            Debug.LogError("No mic detected");
        }
    }
    private IEnumerator MicroStartingPlayer2()
    {
        //initialize mic input asynchronously
        if (!string.IsNullOrEmpty(micreophoneDeviceName2))
        {
            if (micInput2 != null)
            {
                //stop prev mic input
                Microphone.End(micreophoneDeviceName2);
            }
            micInput2 = Microphone.Start(micreophoneDeviceName2, true, 10, AudioSettings.outputSampleRate);

            while (Microphone.GetPosition(micreophoneDeviceName1) <= 0)
            {
                yield return null; // Wait for the microphone to be ready without freezing the game
            }

            micStart1 = true;
        }
        else
        {
            Debug.LogError("No mic detected");
        }
    }

    private float GetMicrophoneVolume(AudioClip micInput, string micDevice)
    {
        //return 0 if no mic available 
        if (micInput == null)
        {
            return 0f;
        }
        float[] sample = new float[sampleWindow]; //buffer hold audio samples 
        int micPosition = Microphone.GetPosition(micDevice) - sampleWindow + 1; //get mic pos

        //if mic pos invalid, return 0 
        if (micPosition < 0)
        { return 0f; }

        micInput.GetData(sample, micPosition); //fetch audio data into the buffer 

        //calc avrg volume from sample 
        float sum = 0f;

        for (int i = 0; i < sampleWindow; i++)
        {
            sum += sample[i] * sample[i];
        }
        return Mathf.Sqrt(sum / sampleWindow); //return RMS value of the sample 
    }

    private void OnApplicationQuit()
    {
        if (micInput1) //stop mic input when end of game 
        {
            Microphone.End(micreophoneDeviceName1);
        }
        if (micInput2)
        {
            Microphone.End(micreophoneDeviceName2);
        }
    }
    //8 second timer for blowing 
    // Countdown before blow 

    IEnumerator StartCountdown()
    {
        // Loop for the countdown
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString(); // Update the countdown text
            yield return new WaitForSeconds(1f); // Wait for 1 second
            countdown--; // Decrease the countdown
        }

        countdownText.text = "results are"; // Update the text when countdown ends
        yield return new WaitForSeconds(1f); // Wait for 1 second before calculating score 
        calculateScore(); // Call the method to calculate the score
    }

    // Method to calculate the score based on microphone input
    public GameObject waypoint; // Reference to the waypoint GameObject (set this in the Inspector)
    public float moveSpeed = 5f; // Speed at which the losing player moves toward the waypoint

    private GameObject loser; // Reference to the losing player

    private void calculateScore()
    {
        // Calculate the average volume for each player
        foreach (float volume in volumeAverage1)
            endScore1 += volume;
        endScore1 /= volumeAverage1.Count; // Calculate the average volume for player 1

        foreach (float volume in volumeAverage2)
            endScore2 += volume;
        endScore2 /= volumeAverage2.Count; // Calculate the average volume for player 2

        volumeAverage1.Clear(); // Clear the volume data for player 1
        volumeAverage2.Clear(); // Clear the volume data for player 2

        // Determine the winner and loser
        if (endScore1 > endScore2)
        {
            resultText.text = "Player 1 wins with volume: " + endScore1;
            loser = playerTwo; // Player 2 is the loser
        }
        else if (endScore2 > endScore1)
        {
            resultText.text = "Player 2 wins with volume: " + endScore2;
            loser = playerOne; // Player 1 is the loser
        }
        else
        {
            resultText.text = "It's a tie!";
            resultText.text += "\nPlayer 1 score: " + endScore1;
            resultText.text += "\nPlayer 2 score: " + endScore2;
            return; // No loser if it's a tie
        }

        // Start the loser moving toward the waypoint
        StartCoroutine(MoveToWaypoint(loser));
    }

    // Coroutine to move the losing player to the waypoint
    private IEnumerator MoveToWaypoint(GameObject player)
    {
        while (Vector3.Distance(player.transform.position, waypoint.transform.position) > 0.1f)
        {
            // Move the player towards the waypoint at the specified speed
            player.transform.position = Vector3.MoveTowards(player.transform.position, waypoint.transform.position, moveSpeed * Time.deltaTime);

            yield return null; // Wait for the next frame
        }

        // Once the player reaches the waypoint, delete the player

        SceneManager.LoadScene(4);
    }

}


