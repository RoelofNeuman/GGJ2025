using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class mikeInput : MonoBehaviour
{
    public float micSenitivity1 = 100f; // mouse sensitivity
    public float PlayerMoveSpeed1 = 1f; // player movespeed
    
    public TMP_Dropdown micDropdown1;
    //public Dropdown micDropdown1;

    public List<string> mikeList = new List<string>(); //list from mic devices 
    public string micreophoneDeviceName1; //name of input mic 

    private AudioClip micInput1; //detect input mic 

    private bool micStart1; // if mic is detected 

    private int sampleWindow = 128; //samples for analysis 



    private bool canMove = true;
    //knockback
    public float kb;
    public float kbStunTime;
    public Rigidbody body;

    //other stuff
    public ParticleSystem bubbleParticles;
    public float playerDistance;
    private float bubbleDuration;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        bubbleDuration = bubbleParticles.main.startLifetime.constant;
    }


    private void Start() //on start, start detecting microphone input 
    {
        canMove = true;
        
        PopulateMic();
        
    }

    private void Update()
    {
        if (micStart1) // If the mic starts 
        {
            float volume1 = GetMicrophoneVolume(micInput1, micreophoneDeviceName1); // Get mic volume

            if (volume1 > micSenitivity1 / 1000f) // If volume exceeds sensitivity
            {
                transform.Translate(Vector3.up * PlayerMoveSpeed1 * Time.deltaTime); // Move player to the left
            }

            // Adjust particle effect based on mic volume
            AdjustParticleEffect(volume1);
        }

        float playerMiddleDistance = playerDistance / 2;
    }

    private void AdjustParticleEffect(float micVolume)
    {
        // Adjust particle properties based on mic volume
        var main = bubbleParticles.main;
        main.startLifetime = micVolume * 2f; // Adjust lifetime (scale by 2)
        main.startSpeed = micVolume * 10f;  // Adjust speed (scale by 10)

        var emission = bubbleParticles.emission;
        emission.rateOverTime = micVolume * 50f; // Adjust emission rate (scale by 50)


    }

    private void PopulateMic()
    {
        //clear existing list of device check new avalable ones 
        mikeList.Clear();

        foreach (var device in Microphone.devices) 
        {
            mikeList.Add(device);
        }

        if (mikeList.Count > 0) //populate the dropdown
        {
            //micDropdown1.ClearOptions();
            micDropdown1.AddOptions(mikeList);
            micDropdown1.onValueChanged.AddListener(delegate { selectedMicrophones1(micDropdown1.value); });

            selectedMicrophones1(0); //set default microphones 
        }
        else
        {
            Debug.LogError("no microphone detected");
        }
    }

    private void selectedMicrophones1(int index)
    {
        //set selected microphone for player 1
        if (index >= 0 && index < mikeList.Count) 
        {
            micreophoneDeviceName1 = mikeList[index];
            MicroStartingPlayer1();
        }
    }


    private void MicroStartingPlayer1()
    {
        //initialise mic input
        if (!string.IsNullOrEmpty(micreophoneDeviceName1)) 
        {
            if(micInput1 != null)
            {
                //stop prev mic input
                Microphone.End(micreophoneDeviceName1);
            }
            micInput1 = Microphone.Start(micreophoneDeviceName1, true, 10, AudioSettings.outputSampleRate);

            //wait till mic is ready 
            while (Microphone.GetPosition(micreophoneDeviceName1) <= 0) { }

            micStart1 = true;
        }
        else
        {
            Debug.LogError("no mic detected");
        }
    }

    private float GetMicrophoneVolume(AudioClip micInput,string micDevice)
    {
        //return 0 if no mic avalable 
        if(micInput == null)
        {
            return 0f;
        }
        float[] sample = new float[sampleWindow]; //buffer hold audio samples 
        int micPosition = Microphone.GetPosition(micDevice) - sampleWindow + 1; //get mic pos

        //if mic pos invalid, return 0 
        if (micPosition < 0)
        { return 0f; }

        micInput.GetData(sample, micPosition); //fetch audit data into the buffer 

        //calc avrg volume from asmple 
        float sum = 0f;

        for (int i = 0; i < sampleWindow; i++)
        {
            sum += sample[i] * sample[i];
        }
        return Mathf.Sqrt(sum / sampleWindow); //return RMs value of teh sample 

    }

    private float getMicVolume(AudioClip micInput, string micDevice)
    {
        if (micInput == null)
        {
            return 0f;
        }

        float[] sample = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(micDevice) - sampleWindow + 1;

        if (micPosition < 0)
        {
            return 0f;
        }

        micInput.GetData(sample, micPosition);

        float sum = 0f;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += sample[i] * sample[i];
        }
        return Mathf.Sqrt(sum / sampleWindow);
    }

    private void OnApplicationQuit()
    {
        if (micInput1)//stop mic input when end of game 
        {
            Microphone.End(micreophoneDeviceName1);
        }
    }

}


