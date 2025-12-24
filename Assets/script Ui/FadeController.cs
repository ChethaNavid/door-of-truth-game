using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [Header("Fade")]
    public Animator fadeAnimator;
    public float waitTime = 1.5f;

    [Header("Scene (Optional)")]
    [SerializeField] private bool loadScene = false;
    [SerializeField] private int sceneIndex;

    public void PlayFade()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        Time.timeScale = 1f; // ensure animation runs

        // Fade IN
        fadeAnimator.SetTrigger("End");

        yield return new WaitForSecondsRealtime(waitTime);

        // Load scene ONLY if enabled
        if (loadScene)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        // Fade OUT
        fadeAnimator.SetTrigger("Start");


    }
}
