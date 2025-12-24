using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public GameObject easyMode;
    public GameObject mediumMode;
    public GameObject hardMode;
    public GameObject infiniteMode;

    void Start()
    {
        GameDifficulty difficulty =
            (GameDifficulty)PlayerPrefs.GetInt("Difficulty", 0);

        // Disable all
        easyMode.SetActive(false);
        mediumMode.SetActive(false);
        hardMode.SetActive(false);
        infiniteMode.SetActive(false);

        // Enable selected
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                easyMode.SetActive(true);
                break;

            case GameDifficulty.Medium:
                mediumMode.SetActive(true);
                break;

            case GameDifficulty.Hard:
                hardMode.SetActive(true);
                break;

            case GameDifficulty.Infinite:
                infiniteMode.SetActive(true);
                break;
        }

        Debug.Log("Game Mode Loaded: " + difficulty);
    }
}
