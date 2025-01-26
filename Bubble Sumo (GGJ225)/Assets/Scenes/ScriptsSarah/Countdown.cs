using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    //public TextMeshProUGUI countdownText; // Reference to the TextMeshPro UI element
    public int countdownTime = 3; // Starting countdown time in seconds
    public string nextSceneName; // Name of the scene to load
    public GameObject one;
    public GameObject two;
    public GameObject three;
    public GameObject blow;

    private void Start()
    {
           
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
        countdownTime = 3;
        // Countdown loop
        while (countdownTime > 0)
        {
            if (countdownTime == 1)
            {
                one.SetActive(true);
                two.SetActive(false);
                three.SetActive(false);

            }
            if (countdownTime == 2)
            {
                one.SetActive(false);
                two.SetActive(true);
                three.SetActive(false);
                
            }
            if (countdownTime == 3)
            {
                one.SetActive(false);
                two.SetActive(false);
                three.SetActive(true);
            }
            
            yield return new WaitForSeconds(1f); // Wait 1 second
            countdownTime= countdownTime -1;
            Debug.Log(countdownTime);
        }

        // Show "GO!" briefly
        one.SetActive(false);
        blow.SetActive(true);
        yield return new WaitForSeconds(1f);

        // Change the scene
        SceneManager.LoadScene(4);
    }
}







