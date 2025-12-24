using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour
{
    public GameObject instructionPanel;

    void Start()
    {
        // Check if instructions were already shown
        if (PlayerPrefs.GetInt("HasShownInstructions", 0) == 0)
        {
            StartCoroutine(PauseAtStart());

            // Mark as shown so it won't show again
            PlayerPrefs.SetInt("HasShownInstructions", 1);
            PlayerPrefs.Save();
        }
    }

    IEnumerator PauseAtStart()
    {
        yield return new WaitForEndOfFrame(); // wait one frame

        Time.timeScale = 0f;              // Pause game
        instructionPanel.SetActive(true);  // Show instructions
    }
    


}
