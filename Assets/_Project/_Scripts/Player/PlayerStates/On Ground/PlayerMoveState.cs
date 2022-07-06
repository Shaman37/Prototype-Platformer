using UnityEngine;

namespace PlayerController2D
{
    public class PlayerMoveState : PlayerOnGroundState
    {

        public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
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

            player.CheckIfShouldFlip(inputX);
            player.SetVelocityX(playerSettings.movementVelocity * rawInput.x);
            
            if (!isExitingState)
            {
                // [TRANSITION] -> Idle State
                if (inputX == 0)
                {
                    stateMachine.ChangeState(player.idleState);
                }
                // [TRANSITION] -> Crouch Move State
                else if (crouchInput)
                {
                    stateMachine.ChangeState(player.crouchMoveState);
                }
                // [TRANSITION] -> Slide State
                else if (inputY == -1 && player.slideState.CheckIfCanSlide())
                {
                    stateMachine.ChangeState(player.slideState);
                }
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

        }
    }
}
