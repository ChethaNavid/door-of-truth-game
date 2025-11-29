using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraDoorScript
{
	public class CameraOpenDoor : MonoBehaviour
	{
		public float DistanceOpen = 10;
		public Transform player;
		public GameObject text;
		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			RaycastHit hit;
			// For third-person: raycast from player position in the direction player is facing
			Vector3 origin = player.position + Vector3.up * 1.5f; // chest height
			Vector3 direction = player.forward;

			if (Physics.Raycast(origin, direction, out hit, DistanceOpen))
			{
				DoorScript.Door door = hit.transform.GetComponent<DoorScript.Door>();
				if (door != null)
				{
					text.SetActive(true);

					// Position text at center height of the door
					Vector3 center = door.transform.position + Vector3.up * 5f;

					// Move text to the LEFT and in FRONT of the door
					Vector3 front = door.transform.forward * -2f;  // In front
					Vector3 left = -door.transform.right * 3f;     // To the left

					text.transform.position = center + front + left;

					text.transform.LookAt(Camera.main.transform.position);
					text.transform.Rotate(0, 180, 0);

					if (Input.GetKeyDown(KeyCode.E))
						hit.transform.GetComponent<DoorScript.Door>().OpenDoor();
				}
				else
				{
					text.SetActive(false);
				}
			}
			else
			{
				text.SetActive(false);
			}
		}
	}
}
