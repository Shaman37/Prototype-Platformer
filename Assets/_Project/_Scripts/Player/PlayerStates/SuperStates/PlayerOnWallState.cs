using UnityEngine;

namespace PlayerController2D
{
    public class PlayerOnWallState : PlayerState
    {
        protected bool _isGrounded;
        protected bool _isTouchingWall;
        protected bool _isTouchingLedge;
        protected bool _jumpInput;
        protected int  _inputX;

        public PlayerOnWallState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
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

            _isGrounded = player.CheckIfGrounded();
            _isTouchingWall = player.CheckIfTouchingFacingWall();
            _isTouchingLedge = player.CheckIfTouchingLedge();

            if (_isTouchingWall && !_isTouchingLedge)
            {
                player.ledgeState.SetInitialPosition(player.transform.position);
            }
        }

        public override void TriggerAnimation()
        {
            base.TriggerAnimation();
        }

		public override void FinishAnimation()
        {
            base.FinishAnimation();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            _inputX = player.inputController.normalizedInputX;
            _jumpInput = player.inputController.jumpInput;

            if (_jumpInput)
            {
                player.wallJumpState.DetermineWallJumpDirection(_isTouchingWall);
                stateMachine.ChangeState(player.wallJumpState);
            }
            else if (_isGrounded)
			{
                stateMachine.ChangeState(player.idleState);
            }
			else if (!_isTouchingWall || _inputX != player.facingDirection)
			{
                stateMachine.ChangeState(player.inAirState);
            }
            else if (_isTouchingWall && !_isTouchingLedge)
            {
                stateMachine.ChangeState(player.ledgeState);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
