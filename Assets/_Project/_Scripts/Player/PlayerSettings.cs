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

        [Header("Check Variables")]
        public float groundCheckRadius = 0.3f;
        public LayerMask groundLayer;

    }
}
