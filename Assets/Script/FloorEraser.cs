using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorEraser : MonoBehaviour
{
    [Header("Settings")]
    public float chaseSpeed = 15.0f; // How fast the floor disappears

    void Update()
    {
        transform.Translate(Vector3.forward * chaseSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if we hit a floor tile
        FallingTile tile = other.GetComponent<FallingTile>();

        if (tile != null)
        {
            tile.DropTile();
            return;
        }
    }
    
    // Visualize the Eraser in the Editor
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f); // Red transparent
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
