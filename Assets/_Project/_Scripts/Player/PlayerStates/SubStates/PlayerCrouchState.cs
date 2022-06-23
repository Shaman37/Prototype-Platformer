using UnityEngine;

namespace PlayerController2D
{
    public class PlayerCrouchState : PlayerGroundedState
    {
        public PlayerCrouchState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
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

        public override void FinishAnimation()
        {
            base.FinishAnimation();
        }

        public override void StateCheck()
        {
            base.StateCheck();
        }

        public override void TriggerAnimation()
        {
            base.TriggerAnimation();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
