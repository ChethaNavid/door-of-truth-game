using UnityEngine;

namespace CharacterScript
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float walkingSpeed = 20f;
        public float runningSpeed = 30f;
        public float jumpSpeed = 10.0f;
        public float gravity = 20.0f;

        [Header("Camera Settings")]
        public Transform cameraTransform;
        public float cameraHeight = 5f;
        public float cameraDistance = 10f;

        // "SmoothDamp" uses time (0.1 is fast, 0.3 is slow/heavy)
        // Set this to 0.1f for snappy, 0.2f for cinematic
        public float cameraSmoothTime = 0.5f;
        public float lookSpeed = 2.0f;

        // Internal Variables
        CharacterController characterController;
        Vector3 verticalVelocity = Vector3.zero;
        Vector3 impactForce = Vector3.zero;
        float rotationY = 0f;

        // Helper for SmoothDamp
        private Vector3 cameraVelocity = Vector3.zero;

        public bool canMove = true;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // 1. Rotation (Mouse)
            rotationY += Input.GetAxis("Mouse X") * lookSpeed;
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

            // 2. Calculate Horizontal Movement (WASD)
            Vector3 horizontalMove = Vector3.zero;

            if (canMove)
            {
                Vector3 forward = transform.forward;
                Vector3 right = transform.right;

                bool isRunning = Input.GetKey(KeyCode.LeftShift);
                float curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
                float curSpeedZ = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");

                horizontalMove = (forward * curSpeedX) + (right * curSpeedZ);
            }

            // 3. Handle Gravity & Jumping
            if (characterController.isGrounded)
            {
                verticalVelocity.y = -2f; // Stick to ground

                if (Input.GetButton("Jump") && canMove)
                {
                    verticalVelocity.y = jumpSpeed;
                }
            }
            else
            {
                verticalVelocity.y -= gravity * Time.deltaTime;
            }

            // 4. Handle Wall Push (Impact) Decay
            if (impactForce.magnitude > 0.2f)
            {
                // Fade out the push
                impactForce = Vector3.Lerp(impactForce, Vector3.zero, 10 * Time.deltaTime);
            }

            // 5. FINAL MOVE APPLICATION (Merge all forces into ONE call)
            // Order: Input + Gravity + WallPush
            Vector3 finalVelocity = horizontalMove + verticalVelocity + impactForce;

            // Execute the single move
            characterController.Move(finalVelocity * Time.deltaTime);
        }

        // Camera must be in LateUpdate to stop jitter
        void LateUpdate()
        {
            if (cameraTransform == null) return;

            // Calculate where the camera WANTS to be
            Vector3 targetPosition = transform.position
                                     - (transform.forward * cameraDistance)
                                     + (Vector3.up * cameraHeight);

            // USE SMOOTHDAMP INSTEAD OF LERP
            // This eliminates the "Laggy/Shaky" feeling
            cameraTransform.position = Vector3.SmoothDamp(
                cameraTransform.position,
                targetPosition,
                ref cameraVelocity,
                cameraSmoothTime
            );

            // Look at player's head height
            cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);
        }

        public void AddImpact(Vector3 dir, float force)
        {
            dir.Normalize();
            if (dir.y < 0) dir.y = -dir.y;
            impactForce = dir.normalized * force / 3.0f;
        }
    }
}