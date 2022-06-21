using UnityEngine;

namespace PlayerController2D
{
    public class PlayerIdleState : PlayerGroundedState
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

            if (inputX != 0.0f && !isExitingState)
            {
                stateMachine.ChangeState(player.moveState);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
