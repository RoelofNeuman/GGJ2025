using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class mikeInput : MonoBehaviour
{
    public float micSenitivity = 100f; // mic volume amplifier
    public TMP_Dropdown micDropdown1; 

    public List<string> mikeList = new List<string>(); //list from mic devices 
    public string micreophoneDeviceName1; //name of input mic 
    public string micreophoneDeviceName2; //2nd input

    private AudioClip micInput1; //detect input mic 
    private AudioClip micInput2; //second

    private bool micStart1; // if mic is detected 
    private bool micStart2; // second mic

    private int sampleWindow = 128; //samples for analysis 

    //other stuff
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
        Debug.Log(bubbleDuration);
        playerOne = GameObject.FindGameObjectWithTag("Player1");
        playerTwo = GameObject.FindGameObjectWithTag("Player2");
    }

    private void Start() //on start, start detecting microphone input 
    {
        PopulateMic();
    }

    private void Update()
    {
      
        
        if (micStart1) // If the mic starts 
        {
            float volume1 = GetMicrophoneVolume(micInput1, micreophoneDeviceName1)* micSenitivity; // Get mic volume
            float volume2 = GetMicrophoneVolume(micInput2, micreophoneDeviceName2) * micSenitivity;
            // Smoothly adjust particle effect based on mic volume
            AdjustParticleEffect1(volume1);
            AdjustParticleEffect2(volume2);
            
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
            micInput1 = Microphone.Start(micreophoneDeviceName1, true, 10, AudioSettings.outputSampleRate);

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

}


