using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DifficultyItem : MonoBehaviour
{
    [Header("Save Settings")]
    [Tooltip("Must match the filename: best_time_easy, best_time_normal, etc.")]
    public string saveFileName; 

    [Header("Star Visuals")]
    public Sprite solidStar;
    public Sprite borderStar;
    public Image[] starSlots; // Drag the 3 stars INSIDE this box here

    void Start()
    {
        LoadDifficultyData();
    }

    public void LoadDifficultyData()
    {
        // Path matches your FinishLine save folder
        string path = Path.Combine(Application.dataPath, "..", "Saves", saveFileName + ".json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            ApplyStars(data.bestStars);
        }
        else
        {
            ApplyStars(0); // If no file exists, show 0 stars (all borders)
        }
    }

    private void ApplyStars(int count)
    {
        for (int i = 0; i < starSlots.Length; i++)
        {
            if (starSlots[i] != null)
            {
                starSlots[i].sprite = (i < count) ? solidStar : borderStar;
            }
        }
    }

    // Structure to read the JSON
    [Serializable]
    public class SaveData {
        public int bestStars;
    }
}