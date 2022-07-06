using UnityEngine;

namespace PlayerController2D
{
    public class PlayerIdleState : PlayerOnGroundState
    {
        public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.SetVelocityX(0f);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void StateCheck()
        {
            base.StateCheck();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (!isExitingState)
            {
                // [TRANSITION] -> Move State
                if (inputX != 0)
                {
                    stateMachine.ChangeState(player.moveState);
                }
                // [TRANSITION] -> Crouch Idle State
                else if (crouchInput || isTouchingCeiling)
                {
                    stateMachine.ChangeState(player.crouchIdleState);
                }    
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
