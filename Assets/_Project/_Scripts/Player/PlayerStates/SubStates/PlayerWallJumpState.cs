using UnityEngine;

namespace PlayerController2D
{
    public class PlayerWallJumpState : PlayerAbilityState
    {
        private int _wallJumpDirection;
        public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.jumpState.ResetJumpsLeft();
            player.inputController.UseJumpInput();

            player.SetVelocity(playerSettings.wallJumpForce, playerSettings.wallJumpAngle, _wallJumpDirection);
            player.CheckIfShouldFlip(_wallJumpDirection);
            player.jumpState.DecreaseJumpsLeft();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            player.animator.SetFloat("velocityY", player.currentVelocity.y);

            if (Time.time >= stateStartTime + playerSettings.wallJumpTime)
            {
                isAbilityDone = true;
            }
        }

        public void DetermineWallJumpDirection(bool isTouchingWall)
        {
            if (isTouchingWall)
            {
                _wallJumpDirection = -player.facingDirection;
            }
            else
            {
                _wallJumpDirection = player.facingDirection;
            }
        }
    }
}
