using UnityEngine;

namespace PlayerController2D
{
    public class PlayerGroundedState : PlayerState
    {
        // Input
        protected int inputX;
        private bool _jumpInput;
        private bool _dashInput;

        // Checks
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
            player.dashState.ResetDash();
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
            _dashInput = player.inputController.dashInput;

            // [TRANSITION] -> Jump State
            if (_jumpInput && player.jumpState.CheckIfCanJump())
            {
                stateMachine.ChangeState(player.jumpState);
            }
            // [TRANSITION] -> In Air State
            else if (!_isGrounded)
            {
                player.inAirState.StartCoyoteTime();
                stateMachine.ChangeState(player.inAirState);
            }
            // [TRANSITION] -> Dash State
            else if (_dashInput && player.dashState.CheckIfCanDash())
            {
                stateMachine.ChangeState(player.dashState);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
