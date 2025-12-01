using UnityEngine;

namespace CameraDoorScript
{
	public class CameraOpenDoor : MonoBehaviour
	{
		[Header("Settings")]
		public float DistanceOpen = 10f;

		[Header("References")]
		public Transform player;
		public GameObject text; // Drag your 3D Text object here

		void Start()
		{
			if (text != null) text.SetActive(false);
		}

		void Update()
		{
			if (player == null) return;
			RaycastHit hit;
			// 1. Raycast from Player Eyes (Chest/Head Height)
			// Adjust 1.6f if your camera is higher/lower
			Vector3 origin = player.position + Vector3.up * 1.6f;
			Vector3 direction = player.forward;

			// Debug Line: Enable Gizmos in Game View to see this red line if needed
			Debug.DrawRay(origin, direction * DistanceOpen, Color.red);

			// Use a small SphereCast for more stable detection than a thin Raycast
			bool didHit = Physics.SphereCast(origin, 0.3f, direction, out hit, DistanceOpen);
			if (didHit)
			{
				// 2. Look for the "Door" script you just posted
				// Robust: check on hit, parent, children, or root children
				DoorScript.Door door =
					hit.transform.GetComponent<DoorScript.Door>()
					?? hit.transform.GetComponentInParent<DoorScript.Door>()
					?? hit.transform.GetComponentInChildren<DoorScript.Door>()
					?? hit.transform.root.GetComponentInChildren<DoorScript.Door>();

				if (door != null)
				{
					// 3. We found the door! Show Text.
					if (text != null)
					{
						if (!text.activeSelf) text.SetActive(true);

						// --- POSITIONING LOGIC ---
						// "hit.point" is the exact spot on the wood where your laser hit.
						// We pull it slightly towards you so it floats just off the surface.
						text.transform.position = hit.point + (hit.normal * 0.25f);

						// Make text face the camera so it's readable
						if (Camera.main != null)
						{
							text.transform.LookAt(Camera.main.transform.position);
							text.transform.Rotate(0, 180, 0);
						}
					}

					// 4. Handle Input
					if (Input.GetKeyDown(KeyCode.E))
					{
						door.TryOpenDoor();
					}
				}
				else
				{
					// Hit a wall/floor, hide text
					if (text != null && text.activeSelf) text.SetActive(false);
				}
			}
			else
			{
				// Looking at empty air, hide text
				if (text != null && text.activeSelf) text.SetActive(false);
			}
		}
	}
}