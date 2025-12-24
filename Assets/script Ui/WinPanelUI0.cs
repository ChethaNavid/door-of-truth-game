using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class WinPanelUI0 : MonoBehaviour
{
    public Sprite solidStar;
    public Sprite borderStar;

    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI bestTimeText;

    public Image[] currentStarSlots; // Size 3
    public Image[] bestStarSlots;    // Size 3

    void OnEnable()
    {
        string path = Path.Combine(Application.dataPath, "..", "Saves", $"best_time_{GameSettings.selectedLevelName}.json");
        if (!File.Exists(path)) return;

        var data = JsonUtility.FromJson<FinishLine0.SaveData>(File.ReadAllText(path));

        if (currentTimeText) currentTimeText.text = data.currentTime;
        if (bestTimeText) bestTimeText.text = data.bestTime;

        UpdateStarImages(currentStarSlots, data.currentStars);
        UpdateStarImages(bestStarSlots, data.bestStars);
    }

    void UpdateStarImages(Image[] images, int score)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].sprite = (i < score) ? solidStar : borderStar;
        }
    }
}
