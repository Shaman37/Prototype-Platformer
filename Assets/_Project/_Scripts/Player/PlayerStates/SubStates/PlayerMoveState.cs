using UnityEngine;

namespace PlayerController2D
{
    public class PlayerMoveState : PlayerGroundedState
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
            player.SetVelocityX(playerSettings.movementVelocity * inputX);

            if (inputX == 0f)
            {
                stateMachine.ChangeState(player.idleState);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
