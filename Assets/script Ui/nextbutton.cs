using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    [Header("Game Scenes")]
    public string normalGameScene = "Easy";      // scene used for Easy, Medium, Hard, Infinite
    public string creditsScene = "Credits";       // scene after Infinite

    [Header("Fade Animation")]
    public Animator fadeAnimator;
    public float fadeWaitTime = 1.5f; // duration of fade animation

    public void GoToNextLevel()
    {
        LevelGenerator.Difficulty current = GameSettings.selectedDifficulty;
        LevelGenerator.Difficulty next = current;

        string sceneToLoad = normalGameScene; // default gameplay scene

        // Determine the next difficulty
        switch (current)
        {
            case LevelGenerator.Difficulty.Easy:
                next = LevelGenerator.Difficulty.Medium;
                break;
            case LevelGenerator.Difficulty.Medium:
                next = LevelGenerator.Difficulty.Hard;
                break;
            case LevelGenerator.Difficulty.Hard:
                next = LevelGenerator.Difficulty.Infinite; // Infinite starts
                break;
            case LevelGenerator.Difficulty.Infinite:
                // End of game -> go to Credits scene
                if (fadeAnimator != null)
                {
                    StartCoroutine(FadeAndLoadScene(creditsScene));
                }
                else
                {
                    SceneManager.LoadScene(creditsScene);
                }
                return;
        }

        // Update GameSettings for next level
        GameSettings.selectedDifficulty = next;
        GameSettings.selectedLevelName = GetLevelNameForDifficulty(next);

        Debug.Log($"Next Level: {next}, Level Name: {GameSettings.selectedLevelName}, Scene: {sceneToLoad}");

        // Play fade and load scene
        if (fadeAnimator != null)
        {
            StartCoroutine(FadeAndLoadScene(sceneToLoad));
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private string GetLevelNameForDifficulty(LevelGenerator.Difficulty difficulty)
    {
        switch (difficulty)
        {
            case LevelGenerator.Difficulty.Easy: return "easy";
            case LevelGenerator.Difficulty.Medium: return "normal";
            case LevelGenerator.Difficulty.Hard: return "hard";
            case LevelGenerator.Difficulty.Infinite: return "infinite";
            default: return "easy";
        }
    }

    private System.Collections.IEnumerator FadeAndLoadScene(string sceneName)
    {
        Time.timeScale = 1f; // ensure game is unpaused
        fadeAnimator.SetTrigger("End"); // trigger fade-out animation
        yield return new WaitForSecondsRealtime(fadeWaitTime); // wait for fade
        SceneManager.LoadScene(sceneName);
    }
}
