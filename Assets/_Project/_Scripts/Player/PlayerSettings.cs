using UnityEngine;

namespace PlayerController2D
{
	[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/Player")]
	public class PlayerSettings : ScriptableObject
	{
        [Header("Move State")]
        public float movementVelocity = 10f;

        [Header("Jump State")]
        public float jumpForce = 15f;
        public int   jumpsAmount = 2;

        [Header("In Air State")]
        public float coyoteTime = 0.2f;
        public float variableJumpHeightMultiplier = 0.5f;

        [Header("Wall Slide State")]
        public float wallSlideVelocity = 3f;
        
        [Header("Wall Jump State")]
        public float wallJumpForce = 20f;
        public float wallJumpCoyoteTime = 0.2f;
        public float wallJumpTime = 0.4f;
        public Vector2 wallJumpAngle = new Vector2(1.0f, 2.0f);


        [Header("Check Variables")]
        public float groundCheckRadius = 0.2f;
        public float wallCheckDistance = 0.5f;
        public LayerMask groundLayer;

    }
}
