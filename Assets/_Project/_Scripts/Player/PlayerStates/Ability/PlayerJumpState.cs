using UnityEngine;

namespace PlayerController2D
{
    public class PlayerJumpState : PlayerAbilityState
    {
        private int jumpsLefts;

        public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
            jumpsLefts = playerSettings.jumpsAmount;
        }

        public override void Enter()
        {
            player.animator.SetInteger("jumpsLeft", jumpsLefts);
            base.Enter();

            player.inputController.UseJumpInput();
            
            player.SetVelocityY(playerSettings.jumpForce);
            player.inAirState.SetIsJumping();

            isAbilityDone = true;
            jumpsLefts--;
        }

        public bool CheckIfCanJump() => jumpsLefts > 0 && !player.CheckForCeling() ? true : false;

        public void ResetJumpsLeft() => jumpsLefts = playerSettings.jumpsAmount;

        public void DecreaseJumpsLeft() => jumpsLefts--;
    }
}
