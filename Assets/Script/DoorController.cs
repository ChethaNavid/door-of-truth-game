using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DoorController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10.0f;
    public Vector3 moveDirection = new Vector3(0, 0, -1);
    
    // Start as FALSE so they don't move immediately
    public bool isMoving = false; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; 
    }

    void FixedUpdate()
    {
        if (!isMoving) return; // Do nothing if asleep

        Vector3 displacement = moveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + displacement);
    }

    public void ActivateWall()
    {
        isMoving = true;
    }
}