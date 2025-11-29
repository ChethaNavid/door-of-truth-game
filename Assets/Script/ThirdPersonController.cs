using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterScript
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        public float walkingSpeed = 7.5f;
        public float runningSpeed = 11.5f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;

        public Transform cameraTransform;     // Third-person camera
        public float cameraHeight = 2f;
        public float cameraDistance = 7f;
        public float cameraSmoothSpeed = 8f;
        public float lookSpeed = 2.0f;

        CharacterController characterController;
        Vector3 moveDirection = Vector3.zero;
        Vector3 cameraOffset;
        float rotationY = 0f;

        [HideInInspector]
        public bool canMove = true;

        void Start()
        {
            characterController = GetComponent<CharacterController>();

            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // Player rotation with mouse X
            if (canMove)
            {
                rotationY += Input.GetAxis("Mouse X") * lookSpeed;
                transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
            }

            // Movement
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedZ = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;

            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedZ);

            // Jumping
            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            // Gravity
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Update third-person camera
            UpdateCamera();
        }

        void UpdateCamera()
        {
            if (cameraTransform == null) return;

            // Offset behind and above the player
            cameraOffset = Quaternion.Euler(15f, 0f, 0f) * (-transform.forward * cameraDistance) + Vector3.up * cameraHeight;
            Vector3 desiredPos = transform.position + cameraOffset;

            // Smooth camera movement
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPos, cameraSmoothSpeed * Time.deltaTime);

            // Look at player
            cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);
        }
    }
}
