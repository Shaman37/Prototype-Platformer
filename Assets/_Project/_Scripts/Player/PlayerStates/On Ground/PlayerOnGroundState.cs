using UnityEngine;

namespace PlayerController2D
{
    public class PlayerOnGroundState : PlayerState
    {
        // Input
        protected int inputX;
        protected int inputY;
        protected Vector2 rawInput;
        private bool _jumpInput;
        private bool _dashInput;
        protected bool crouchInput;

        // Checks
        private bool _isGrounded;
        protected bool isTouchingCeiling;


        /// <summary>
        /// 	Grounded State Constructor.
        /// </summary>
        public PlayerOnGroundState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {

        }

        public override void Enter()
        {
            base.Enter();

            player.jumpState.ResetJumpsLeft();
            player.dashState.ResetDash();
            player.slideState.ResetSlide();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void StateCheck()
        {
            base.StateCheck();

            _isGrounded = player.CheckIfGrounded();
            isTouchingCeiling = player.CheckForCeling();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            inputX = player.inputController.normalizedInputX;
            inputY = player.inputController.normalizedInputY;
            rawInput = player.inputController.rawMovementInput;

            _jumpInput = player.inputController.jumpInput;
            _dashInput = player.inputController.dashInput;
            crouchInput = player.inputController.crouchInput;

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
