using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallingTile : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col; // Reference to the collider

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>(); // Get the collider

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Keep it flat
    }

    public void DropTile()
    {
        if (rb == null) return;

        transform.position += Vector3.down * 0.01f;

        rb.isKinematic = false;
        rb.useGravity = true;

        // Add a tiny push if gravity feels too slow
        rb.AddForce(Vector3.down * 2f, ForceMode.VelocityChange);

        Destroy(gameObject, 3f);
    }
}