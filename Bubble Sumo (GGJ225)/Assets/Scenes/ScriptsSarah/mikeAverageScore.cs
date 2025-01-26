using System.Collections;
using UnityEngine;

public class mikeAverageScore : MonoBehaviour
{
    public string micDevice1; // Microphone device name for Player 1
    public string micDevice2; // Microphone device name for Player 2

    private AudioClip micInput1; // Microphone input for Player 1
    private AudioClip micInput2; // Microphone input for Player 2

    private bool mic1Started = false;
    private bool mic2Started = false;

    private int sampleWindow = 128; // Number of audio samples for analysis

    private void Start()
    {
        StartCoroutine(StartMicInput(micDevice1, 1));
        StartCoroutine(StartMicInput(micDevice2, 2));
    }

    private void Update()
    {
        if (mic1Started && mic2Started)
        {
            // Calculate microphone volumes
            float micVolume1 = GetMicrophoneVolume(micInput1, micDevice1);
            float micVolume2 = GetMicrophoneVolume(micInput2, micDevice2);

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
                Debug.Log("It's a tie!");
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
            if (playerIndex == 1)
            {
                micInput1 = micInput;
                mic1Started = true;
            }
            else if (playerIndex == 2)
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

    private float GetMicrophoneVolume(AudioClip micInput, string micDevice)
    {
        if (micInput == null)
            return 0f;

        float[] samples = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(micDevice) - sampleWindow + 1;

        if (micPosition < 0)
            return 0f;

        micInput.GetData(samples, micPosition);

        float sum = 0f;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += samples[i] * samples[i];
        }

        return Mathf.Sqrt(sum / sampleWindow); // RMS value
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


