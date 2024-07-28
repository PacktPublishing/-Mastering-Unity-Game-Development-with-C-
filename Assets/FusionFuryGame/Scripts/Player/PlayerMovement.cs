using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FusionFuryGame
{
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerStats playerStats;

        public Transform groundChecker;
        public LayerMask groundLayer;
        public float groundDistance = 0.2f;

        public Rigidbody playerRigidbody;
        private bool isGrounded = true;
        private bool canDash = true;
        private bool canRotate = true;

        private Vector2 movementInput;
        private Vector3 movementVector;

        public Camera mainCamera;
        public Transform playerGraphics; // The graphics object of the player

        private void OnEnable()
        {            
            PlayerInput.onDash += Dash;
            PlayerInput.onMovement += HandleMovementInput;
            PlayerAbility.OnAbilityActivated += DisableRotation;
            PlayerAbility.OnAbilityDeactivated += EnableRotation;
        }

        private void OnDisable()
        {
            PlayerInput.onDash -= Dash;
            PlayerInput.onMovement -= HandleMovementInput;
            PlayerAbility.OnAbilityActivated -= DisableRotation;
            PlayerAbility.OnAbilityDeactivated -= EnableRotation;
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void HandleMovementInput(Vector2 input)
        {
            movementInput = input;
        }

        private void MovePlayer()
        {
            movementVector = new Vector3(movementInput.x, 0f, movementInput.y).normalized * playerStats.MoveSpeed;
            Vector3 newPosition = playerRigidbody.position + movementVector * Time.fixedDeltaTime;
            playerRigidbody.MovePosition(newPosition);
        }


        private void Dash()
        {
            if (canDash)
            {
                Vector3 dashVector = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
                playerRigidbody.AddForce(dashVector * playerStats.DashForce, ForceMode.Impulse);

                canDash = false;
                Invoke(nameof(ResetDash), playerStats.DashCooldown);
            }
        }

        private void Update()
        {
            if (canRotate)
            {
                RotatePlayerToMouse();
            }
        }

        private void FixedUpdate()
        {
            MovePlayer();
            
        }

        private void ResetDash()
        {
            canDash = true;
        }

        private void RotatePlayerToMouse()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane playerPlane = new Plane(Vector3.up, transform.position);

            if (playerPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                Vector3 lookAtPoint = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
                transform.LookAt(lookAtPoint);

                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);
                
            }
        }

        private void DisableRotation()
        {
            canRotate = false;
        }

        private void EnableRotation()
        {
            canRotate = true;
        }
    }
}