using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform[] items;

    [Header("Visual")]
    public float selectedScale = 1.2f;
    public float unselectedScale = 0.8f;
    public float selectedAlpha = 1f;
    public float unselectedAlpha = 0.4f;
    public float smoothSpeed = 10f;

    [Header("Game Scene")]
    public string gameSceneName = "Easy"; // your game scene

    private float[] positions;
    private int selectedIndex = 0;

    void Start()
    {
        // Precompute normalized positions
        positions = new float[items.Length];
        if (items.Length > 1)
        {
            float step = 1f / (items.Length - 1);
            for (int i = 0; i < items.Length; i++)
                positions[i] = step * i;
        }
        else
        {
            positions[0] = 0;
        }
    }

    void Update()
    {
        AnimateItems();
    }

    void AnimateItems()
    {
        float closest = Mathf.Infinity;

        for (int i = 0; i < items.Length; i++)
        {
            float distance = Mathf.Abs(scrollRect.horizontalNormalizedPosition - positions[i]);

            if (distance < closest)
            {
                closest = distance;
                selectedIndex = i;
            }

            // Smooth scale
            float targetScale = Mathf.Lerp(selectedScale, unselectedScale, distance * 5f);
            items[i].localScale = Vector3.Lerp(items[i].localScale, Vector3.one * targetScale, Time.deltaTime * smoothSpeed);

            // Smooth alpha
            CanvasGroup cg = items[i].GetComponent<CanvasGroup>();
            if (cg)
            {
                float targetAlpha = Mathf.Lerp(selectedAlpha, unselectedAlpha, distance * 5f);
                cg.alpha = Mathf.Lerp(cg.alpha, targetAlpha, Time.deltaTime * smoothSpeed);
            }
        }

        // Smoothly snap scroll to closest item
        scrollRect.horizontalNormalizedPosition =
            Mathf.Lerp(scrollRect.horizontalNormalizedPosition, positions[selectedIndex], Time.deltaTime * smoothSpeed);
    }

    // Call this on Play button
    public void PlayGame()
    {
        // Map selectedIndex to difficulty
        GameSettings.selectedDifficulty = (LevelGenerator.Difficulty)selectedIndex;

        // Map selectedIndex to level name (customize as you want)
        string[] levelNames = { "easy", "normal", "hard", "infinite" };
        GameSettings.selectedLevelName = levelNames[selectedIndex];

        Debug.Log($"Selected: {GameSettings.selectedDifficulty}, Level: {GameSettings.selectedLevelName}");

        // Load the game scene
        SceneManager.LoadScene(gameSceneName);
    }
}
