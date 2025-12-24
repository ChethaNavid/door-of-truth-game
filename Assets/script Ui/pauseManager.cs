using UnityEngine;
using System.Collections;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public TextMeshProUGUI countdownText;

    public AudioSource audioSource;
    public AudioClip countdownBeep;
    public AudioClip goSound;

    private GameTimer timer;

    private void Awake()
    {
        // Automatically find the GameTimer in the scene
        timer = FindObjectOfType<GameTimer>();
        if (timer == null)
            Debug.LogWarning("No GameTimer found in the scene!");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (timer != null) timer.PauseTimer();
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        StartCoroutine(CountdownAndResume());
    }

    private IEnumerator CountdownAndResume()
    {
        if (timer != null) timer.PauseTimer(); // stop timer during countdown
        Time.timeScale = 0f;
        countdownText.gameObject.SetActive(true);

        float basePitch = 1f;
        float pitchStep = 0.2f;

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            if (audioSource != null && countdownBeep != null)
                audioSource.PlayOneShot(countdownBeep);
            yield return new WaitForSecondsRealtime(1f);
        }

        // GO!
        countdownText.text = "GO!";
        if (audioSource != null && goSound != null)
            audioSource.PlayOneShot(goSound);

        yield return new WaitForSecondsRealtime(0.5f);

        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        if (timer != null) timer.ResumeTimer();
    }
}
