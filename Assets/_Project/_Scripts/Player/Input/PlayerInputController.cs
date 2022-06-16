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
            }
        }

        public void UseJumpInput() => jumpInput = false;
    }
}
