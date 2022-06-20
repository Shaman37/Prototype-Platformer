using UnityEngine;

namespace PlayerController2D
{
	[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/Player")]
	public class PlayerSettings : ScriptableObject
	{
        [Header("Move State")]
        public float movementVelocity = 10f;

        [Header("Jump State")]
        public float jumpVelocity = 15f;
        public int   jumpsAmount = 2;

        [Header("In Air Sate")]
        public float coyoteTime = 0.2f;
        public float variableJumpHeightMultiplier = 0.5f;

        [Header("Check Variables")]
        public float groundCheckRadius = 0.3f;
        public LayerMask groundLayer;

    }
}
