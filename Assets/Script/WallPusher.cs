using UnityEngine;

public class WallPusher : MonoBehaviour
{
    [Header("Push Settings")]
    [Tooltip("Must be higher than Door Speed (e.g. if Door is 10, set this to 12 or 15)")]
    public float pushForce = 15f; 

    [Header("Connections")]
    [Tooltip("Drag the Child Door object (the one with the Door.cs script) here!")]
    public DoorScript.Door connectedDoor; 

    // Internal reference to the parent that controls movement
    private DoorController parentDoor;

    void Start()
    {
        // Automatically find the movement script on the Parent object
        parentDoor = GetComponentInParent<DoorController>();

        if (parentDoor == null)
        {
            Debug.LogError("WallPusher Error: This object is not a child of a DoorController!");
        }
    }

    // We use OnTriggerStay for continuous, smooth displacement
    private void OnTriggerStay(Collider other)
    {
        // 1. SAFETY CHECK: Is the door open?
        // If the linked door is open, we do NOT push. Let the player walk through.
        if (connectedDoor != null && connectedDoor.open) 
        {
            return; 
        }

        // 2. CHECK TARGET: Is it the player?
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            
            if (controller != null && parentDoor != null)
            {
                // 3. CALCULATE DIRECTION
                // We use the exact direction the parent wall is moving
                Vector3 direction = parentDoor.moveDirection.normalized;

                // 4. APPLY FORCE
                controller.Move(pushForce * Time.deltaTime * direction);
            }
        }
    }

    // *** VISUAL DEBUGGING ***
    // This draws a transparent box in the Scene view to help you see the trigger
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        if (connectedDoor != null && connectedDoor.open)
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f); // Transparent Green
        }
        else
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f); // Transparent Red
        }

        // Draw the shape of the collider
        Gizmos.DrawCube(Vector3.zero, Vector3.one); 
    }
}