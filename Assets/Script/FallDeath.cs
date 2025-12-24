using UnityEngine;
using StarterAssets;

public class FallDeath : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If player Y goes below this number, they die.")]
    public float deathHeight = -10.0f;
    public GameObject UI;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;

    private bool isDead = false; // prevent repeat trigger

    void Start()
    {
        // Allow sound even if game is paused later
        if (audioSource != null)
            audioSource.ignoreListenerPause = true;
    }

    void Update()
    {
        // Check ONLY the Y height
        if (!isDead && transform.position.y < deathHeight)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        isDead = true;

        // 🔊 Play death sound
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Show Game Over UI
        if (UI != null)
            UI.SetActive(true);

        // Optional: stop movement
        Time.timeScale = 0f;
    }
}
