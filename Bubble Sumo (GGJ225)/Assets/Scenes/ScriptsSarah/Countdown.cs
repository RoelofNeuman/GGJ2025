using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Reference to the TextMeshPro UI element
    public float countdownTime = 3f; // Starting countdown time in seconds
    public string nextSceneName; // Name of the scene to load

    private void Start()
    {
        if (countdownText == null)
        {
            Debug.LogError("Countdown Text is not assigned!");
            return;
        }

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("Next scene name is not specified!");
            return;
        }

        // Start the countdown
        StartCoroutine(StartCountdown());
    }

    private System.Collections.IEnumerator StartCountdown()
    {
        float remainingTime = countdownTime;

        // Countdown loop
        while (remainingTime > 0)
        {
            countdownText.text = Mathf.CeilToInt(remainingTime).ToString(); // Update text to show remaining time
            yield return new WaitForSeconds(1f); // Wait 1 second
            remainingTime--;
        }

        // Show "GO!" briefly
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        // Change the scene
        SceneManager.LoadScene(2);
    }
}







