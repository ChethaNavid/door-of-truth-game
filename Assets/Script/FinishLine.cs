using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("YOU WIN!");
            
            // 1. Destroy the Eraser (The Void) so it stops chasing
            GameObject eraser = GameObject.Find("The_Void");
            if (eraser != null) Destroy(eraser);

            // 2. Optional: Show Win UI Screen
            // UIManager.Instance.ShowWinScreen();
        }
    }
}