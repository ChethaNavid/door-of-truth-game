using UnityEngine;
using StarterAssets;

public class FallDeath : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If player Y goes below this number, they die.")]
    public float deathHeight = -10.0f; 

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
        Debug.Log("GAME OVER: You fell into the void!");
    }
}