using UnityEngine;
using System.IO;
using System;

public class FinishLine0 : MonoBehaviour
{
    [Header("Normal Mode Thresholds (seconds)")]
    public float threeStarTime = 40f;
    public float twoStarTime = 60f;

    [Header("Infinite Mode Thresholds (seconds)")]
    public float infiniteThreeStarTime = 120f;
    public float infiniteTwoStarTime = 180f;

    [Header("UI")]
    public string winPanelName = "WinPanel";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip finishSound;

    private float currentThreeStarTime;
    private float currentTwoStarTime;

    private void Start()
    {
        // Allow sound to play even when game is paused
        if (audioSource != null)
            audioSource.ignoreListenerPause = true;

        // Set thresholds based on difficulty
        if (GameSettings.selectedDifficulty == LevelGenerator.Difficulty.Infinite)
        {
            currentThreeStarTime = infiniteThreeStarTime;
            currentTwoStarTime = infiniteTwoStarTime;
        }
        else
        {
            currentThreeStarTime = threeStarTime;
            currentTwoStarTime = twoStarTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 🔊 Play finish sound
        if (audioSource != null && finishSound != null)
        {
            audioSource.PlayOneShot(finishSound);
        }

        GameTimer timer = FindObjectOfType<GameTimer>();
        if (timer != null) timer.PauseTimer();

        Time.timeScale = 0f;

        float rawSeconds = timer != null ? timer.GetCurrentTime() : 0f;
        string formattedTime = timer != null ? timer.GetFormattedTime() : "00:00";

        int starsEarned = CalculateStars(rawSeconds);

        Debug.Log($"[FinishLine0] Time: {rawSeconds}s, Stars: {starsEarned}");

        HandleSaveData(formattedTime, starsEarned);

        // Show Win Panel
        GameObject holder = GameObject.Find("UIholder");
        if (holder != null)
        {
            Transform panel = holder.transform.Find(winPanelName);
            if (panel != null)
                panel.gameObject.SetActive(true);
        }
    }

    private void HandleSaveData(string newTime, int newStars)
    {
        string folder = Path.Combine(Application.dataPath, "..", "Saves");
        Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, $"best_time_{GameSettings.selectedLevelName}.json");

        string bestT = newTime;
        int bestS = newStars;

        if (File.Exists(path))
        {
            SaveData old = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

            bestT = old.bestTime;
            bestS = old.bestStars;

            float newTimeSec = FindObjectOfType<GameTimer>()?.GetCurrentTime() ?? 0f;
            float oldTimeSec = TimeStringToSeconds(old.bestTime);

            if (newStars > old.bestStars ||
                (newStars == old.bestStars && newTimeSec < oldTimeSec))
            {
                bestT = newTime;
                bestS = newStars;
            }
        }

        SaveData data = new SaveData(newTime, newStars, bestT, bestS);
        File.WriteAllText(path, JsonUtility.ToJson(data, true));
    }

    private int CalculateStars(float time)
    {
        return time <= currentThreeStarTime ? 3 :
               time <= currentTwoStarTime ? 2 : 1;
    }

    private float TimeStringToSeconds(string time)
    {
        string[] split = time.Split(':');
        if (split.Length != 2) return 0f;

        if (float.TryParse(split[0], out float m) &&
            float.TryParse(split[1], out float s))
            return m * 60f + s;

        return 0f;
    }

    [Serializable]
    public class SaveData
    {
        public string currentTime, bestTime;
        public int currentStars, bestStars;

        public SaveData(string cT, int cS, string bT, int bS)
        {
            currentTime = cT;
            currentStars = cS;
            bestTime = bT;
            bestStars = bS;
        }
    }
}
