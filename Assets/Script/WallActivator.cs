using UnityEngine;

public class WallActivator : MonoBehaviour
{
    [Header("Connections")]
    public DoorController[] allDoors; 

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            // Loop through all assigned doors and wake them up
            foreach (DoorController door in allDoors)
            {
                if (door != null)
                {
                    door.ActivateWall();
                }
            }

            hasTriggered = true;
            Debug.Log("Trap Activated! All doors moving.");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}