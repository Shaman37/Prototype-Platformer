using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController2D
{
    public class PlayerInputController : MonoBehaviour
    {
        public int normalizedInputX { get; private set; }
        public int normalizedInputY { get; private set; }
        public Vector2 rawMovementInput { get; private set; }
        public bool jumpInput { get; private set; }
        public bool jumpInputStop { get; private set; }
        public bool dashInput { get; private set; }
        public bool dashInputStop { get; private set; }
        public bool crouchInput { get; private set; }

        private float jumpStartTime;
        private float dashStartTime;
        [SerializeField] private float inputHoldTime = 0.2f;

        private void Update() 
        {
            CheckInputHoldTime();
            CheckDashHoldTime();
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            rawMovementInput = context.ReadValue<Vector2>();
            normalizedInputX = (int) (rawMovementInput * Vector2.right).normalized.x;
            normalizedInputY = (int) (rawMovementInput * Vector2.up).normalized.y;
        }
    
        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                jumpInput = true;
                jumpInputStop = false;
                jumpStartTime = Time.time;
            }

            if (context.canceled)
            {
                jumpInputStop = true;
            }
        }

        public void OnDashInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                dashInput = true;
                dashInputStop = false;
                dashStartTime = Time.time;
            }

            if (context.canceled)
            {
                dashInputStop = true;
            }
        }

        public void OnCrouchInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                crouchInput = true;
            }

            if (context.canceled)
            {
                crouchInput = false;
            }
        }

        private void CheckInputHoldTime() 
        {
            if (Time.time >= jumpStartTime + inputHoldTime)
            {
                jumpInput = false;
            }
        }

        private void CheckDashHoldTime() 
        {
            if (Time.time >= dashStartTime + inputHoldTime)
            {
                dashInput = false;
            }
        }

        public void UseJumpInput() => jumpInput = false;
        public void UseDashInput() => dashInput = false;
    }
}
