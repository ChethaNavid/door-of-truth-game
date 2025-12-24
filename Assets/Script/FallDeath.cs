using UnityEngine;
using StarterAssets;

public class FallDeath : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If player Y goes below this number, they die.")]
    public float deathHeight = -10.0f; 
    public GameObject UI;

    void Update()
    {
        // Check ONLY the Y height
        if (transform.position.y < deathHeight)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        UI.SetActive(true);
    }
}