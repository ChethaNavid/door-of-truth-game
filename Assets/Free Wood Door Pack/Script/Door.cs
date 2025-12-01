using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider))]
    public class Door : MonoBehaviour
    {
        [Header("Quiz Settings")]
        public bool isCorrectDoor = false; 

        [Header("Animation")]
        public float smooth = 1.0f;
        private float openAngle = -90.0f;
        private float closeAngle = 0.0f;
		
		[Header("Internal")]
        public bool open = false;
		private Collider myCollider;

        [Header("Audio")]
        public AudioSource asource;
        public AudioClip openClip, closeClip;

        void Start()
        {
			myCollider = GetComponent<Collider>();
            asource = GetComponent<AudioSource>();
        }

        void Update()
        {
            float targetAngle = open ? openAngle : closeAngle;
            Quaternion targetRot = Quaternion.Euler(0, targetAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * 5 * smooth);
        }

        public void TryOpenDoor()
        {
            if (open) return; 

            if (isCorrectDoor)
            {
                // Just Open. Do NOT Stop.
                open = true;
				if(myCollider != null) myCollider.enabled = false;
                PlaySound(openClip);
            }
            else
            {
                Debug.Log("Wrong Door!");
                PlaySound(closeClip);
            }
        }

        void PlaySound(AudioClip clip)
        {
            if (asource != null && clip != null) { asource.clip = clip; asource.Play(); }
        }
    }
}