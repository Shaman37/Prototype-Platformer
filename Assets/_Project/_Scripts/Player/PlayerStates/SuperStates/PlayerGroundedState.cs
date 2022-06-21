using UnityEngine;

namespace PlayerController2D
{
    public class PlayerGroundedState : PlayerState
    {
        protected int inputX;

        private bool _jumpInput;
        private bool _isGrounded;


        /// <summary>
        /// 	Grounded State Constructor.
        /// </summary>
        public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {

        }

        public override void Enter()
        {
            base.Enter();

            player.jumpState.ResetJumpsLeft();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void StateCheck()
        {
            base.StateCheck();

            _isGrounded = player.CheckIfGrounded();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            inputX = player.inputController.normalizedInputX;
            _jumpInput = player.inputController.jumpInput;

            if (_jumpInput && player.jumpState.CheckIfCanJump())
            {
                stateMachine.ChangeState(player.jumpState);
            }
            else if (!_isGrounded)
            {
                player.inAirState.StartCoyoteTime();
                stateMachine.ChangeState(player.inAirState);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
