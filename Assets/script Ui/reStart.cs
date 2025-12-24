using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeReloadButton : MonoBehaviour
{
    [Header("Fade Settings")]
    public Animator fadeAnimator;      // Animator controlling fade
    // public float fadeDuration = 2f;  // Length of fade animation

    // Call this from the UI Button OnClick()
    public void ReloadWithFade()
    {
        StartCoroutine(FadeAndReload());
    }

    private IEnumerator FadeAndReload()
    {
        Time.timeScale = 1f; // Make sure game isn't paused

        // 1️⃣ Play fade-in (transparent → black)
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("End");
        }

        // 2️⃣ Wait for fade animation to finish
        // yield return new WaitForSecondsRealtime(fadeDuration);

        // 3️⃣ Reload the current scene asynchronously
        Scene activeScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(activeScene.buildIndex);
        while (!asyncLoad.isDone)
            yield return null;

        // 4️⃣ Optionally, fade-out can play in new scene via Start() in Fade object
    }
}
