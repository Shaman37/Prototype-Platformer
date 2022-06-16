using UnityEngine;

namespace PlayerController2D
{
    public class PlayerGroundedState : PlayerState
    {
        protected int inputX;

        private bool jumpInput;


        /// <summary>
        /// 	Grounded State Constructor.
        /// </summary>
        public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
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

            inputX = player.inputController.normalizedInputX;
            jumpInput = player.inputController.jumpInput;

            if (jumpInput)
            {
                player.inputController.UseJumpInput();
                stateMachine.ChangeState(player.jumpState);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
