using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class mikeAverageScore : MonoBehaviour
{
    public List<string> mikeList = new List<string>(); //list from mic devices 
    public string micDevice1; // Microphone device name for Player 1
    public string micDevice2; // Microphone device name for Player 2

    private AudioClip micInput1; // Microphone input for Player 1
    private AudioClip micInput2; // Microphone input for Player 2

    private bool mic1Started = false;
    private bool mic2Started = false;

    private int sampleWindow = 128; // Number of audio samples for analysis

    private void Start()
    {
        PopulateMic();
        StartCoroutine(StartMicInput(micDevice1, 0));
        StartCoroutine(StartMicInput(micDevice2, 1));
    }

    private void Update()
    {
        if (mic1Started && mic2Started)
        {
            // Calculate microphone volumes
            float micVolume1 = GetMicrophoneVolume(micInput1, micDevice1)*25f;
            float micVolume2 = GetMicrophoneVolume(micInput2, micDevice2)*25f;

            // Determine the winner
            if (micVolume1 > micVolume2)
            {
                Debug.Log("Player 1 wins with volume: " + micVolume1);
            }
            else if (micVolume2 > micVolume1)
            {
                Debug.Log("Player 2 wins with volume: " + micVolume2);
            }
            else
            {
                Debug.Log("1: " + micVolume1);
                Debug.Log("2: " + micVolume2);
            }
        }
    }

    private IEnumerator StartMicInput(string micDevice, int playerIndex)
    {
        if (!string.IsNullOrEmpty(micDevice))
        {
            AudioClip micInput = Microphone.Start(micDevice, true, 10, AudioSettings.outputSampleRate);

            // Wait until the microphone is ready
            while (Microphone.GetPosition(micDevice) <= 0)
            {
                yield return null;
            }

            // Assign the microphone input to the correct variable
            if (playerIndex == 0)
            {
                micInput1 = micInput;
                mic1Started = true;
            }
            else if (playerIndex == 1)
            {
                micInput2 = micInput;
                mic2Started = true;
            }

            Debug.Log($"Microphone {playerIndex} started: {micDevice}");
        }
        else
        {
            Debug.LogError($"Microphone {playerIndex} device name is null or empty.");
        }
    }

    private void PopulateMic()
    {
        //clear existing list of device check new available ones 
        mikeList.Clear();

        foreach (var device in Microphone.devices)
        {
            mikeList.Add(device);
        }
        micDevice1 = mikeList[0];
        micDevice2= mikeList[1];

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
        { return 0f;
        }

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
        if (micInput1 != null)
        {
            Microphone.End(micDevice1);
        }

        if (micInput2 != null)
        {
            Microphone.End(micDevice2);
        }
    }
}


