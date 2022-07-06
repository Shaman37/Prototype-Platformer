using UnityEngine;

namespace PlayerController2D
{
    public class PlayerCrouchIdleState : PlayerOnGroundState
    {
        public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.SetVelocityZero();
            player.ChangeColliderHeight(playerSettings.crouchingColliderHeight);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (!isExitingState)
            {
                // [TRANSITION] -> Crouch Move State
                if (inputX != 0)
                {
                    stateMachine.ChangeState(player.crouchMoveState);
                }
                // [TRANSITION] -> Idle State
                else if (!crouchInput && (!isTouchingCeiling && !player.ledgeState._willTouchCeiling))
                {
                    stateMachine.ChangeState(player.idleState);
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
