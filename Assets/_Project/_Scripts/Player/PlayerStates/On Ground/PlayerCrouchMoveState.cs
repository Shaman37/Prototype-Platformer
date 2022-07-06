using UnityEngine;

namespace PlayerController2D
{
    public class PlayerCrouchMoveState : PlayerOnGroundState
    {
        public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.ChangeColliderHeight(playerSettings.crouchingColliderHeight);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (!isExitingState)
            {
                player.SetVelocityX(playerSettings.crouchMovementVelocity * player.facingDirection);
                player.CheckIfShouldFlip(inputX);

                // [TRANSITION] -> Crouch Idle State
                if (inputX == 0)
                {
                    stateMachine.ChangeState(player.crouchIdleState);
                }
                // [TRANSITION] -> Move State
                else if (!crouchInput && (!isTouchingCeiling && !player.ledgeState._willTouchCeiling))
                {
                    stateMachine.ChangeState(player.moveState);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            player.ChangeColliderHeight(playerSettings.standingColliderHeight);
        }
    }
}
