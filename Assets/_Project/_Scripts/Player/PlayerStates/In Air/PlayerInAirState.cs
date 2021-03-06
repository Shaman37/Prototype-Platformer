using UnityEngine;

namespace PlayerController2D
{
    public class PlayerInAirState : PlayerState
    {   
        // Input fields
        private int   _inputX;
        private bool  _jumpInput;
        private bool  _jumpInputStop;
        private bool  _dashInput;

        // Check fields
        private bool  _isGrounded;
        private bool  _isJumping;
        private bool  _wasTouchingFacingWall;
        private bool  _isTouchingFacingWall;
        private bool  _wasTouchingBackWall;
        private bool  _isTouchingBackWall;
        private bool  _isTouchingLedge;

        // Game Feel fields
        private bool  _coyoteTime;
        private bool  _wallJumpCoyoteTime;
        private float _wallJumpCoyoteTimeStart;

        public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {

        }

        public override void Enter()
        {
            base.Enter();

            player.dashState.ResetDash();
        }

        public override void Exit()
        {
            base.Exit();

            _wasTouchingBackWall = false;
            _isTouchingBackWall = false;
            _wasTouchingFacingWall = false;
            _isTouchingFacingWall = false;
        }

        public override void StateCheck()
        {
            base.StateCheck();

            _wasTouchingFacingWall = _isTouchingFacingWall;
            _wasTouchingBackWall = _isTouchingBackWall;

            _isGrounded = player.CheckIfGrounded();
            _isTouchingFacingWall = player.CheckIfTouchingFacingWall();
            _isTouchingBackWall = player.CheckIfTouchingBackWall();
            _isTouchingLedge = player.CheckIfTouchingLedge();

            if (_isTouchingFacingWall && !_isTouchingLedge)
            {
                player.ledgeState.SetInitialPosition(player.transform.position);
            }

            if (!_wallJumpCoyoteTime && !_isTouchingBackWall && !_isTouchingFacingWall && (_wasTouchingBackWall || _wasTouchingFacingWall))
            {
                StartWallJumpCoyoteTime();
            }
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            CheckCoyoteTime();
            CheckWallJumpCoyoteTime();

            _inputX = player.inputController.normalizedInputX;
            _jumpInput = player.inputController.jumpInput;
            _jumpInputStop = player.inputController.jumpInputStop;
            _dashInput = player.inputController.dashInput;

            ApplyJumpMultiplier();

            // [TRANSITION] -> Land State
            if (_isGrounded && player.currentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.landState);
            }
            // [TRANSITION] -> Ledge State
            else if (_isTouchingFacingWall && !_isTouchingLedge && !_isGrounded)
            {
                stateMachine.ChangeState(player.ledgeState);
            }
            // [TRANSITION] -> Wall Jump State
            else if (_jumpInput && (_isTouchingFacingWall || _isTouchingBackWall || _wallJumpCoyoteTime))
            {
                StopWallJumpCoyoteTime();
                _isTouchingFacingWall = player.CheckIfTouchingFacingWall();
                player.wallJumpState.DetermineWallJumpDirection(_isTouchingFacingWall);
                stateMachine.ChangeState(player.wallJumpState);
            }
            // [TRANSITION] -> Jump State
            else if (_jumpInput && player.jumpState.CheckIfCanJump())
            {
                stateMachine.ChangeState(player.jumpState);
            }
            // [TRANSITION] -> Wall Slide State
            else if (_isTouchingFacingWall && _inputX == player.facingDirection && player.CheckIfFalling())
            {
                stateMachine.ChangeState(player.wallSlideState);
            }
            // [TRANSITION] -> Dash State
            else if (_dashInput && player.dashState.CheckIfCanDash())
            {
                stateMachine.ChangeState(player.dashState);
            }
            // Air Mobility
            else
            {
                player.CheckIfShouldFlip(_inputX);
                player.SetVelocityX(playerSettings.movementVelocity * _inputX);

                player.animator.SetFloat("velocityY", player.currentVelocity.y);
            }
        }


        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
        private void ApplyJumpMultiplier()
        {
            if (_isJumping)
            {
                if (_jumpInputStop)
                {
                    player.SetVelocityY(player.currentVelocity.y * playerSettings.variableJumpHeightMultiplier);
                    _isJumping = false;
                }
                else if (player.currentVelocity.y <= 0.0f)
                {
                    _isJumping = false;
                }
            }
        }

        private void CheckCoyoteTime()
        {
            if (_coyoteTime && Time.time > stateStartTime + playerSettings.coyoteTime)
            {
                _coyoteTime = false;
                player.jumpState.DecreaseJumpsLeft();
            }
        }

        private void CheckWallJumpCoyoteTime()
        {
            if (_wallJumpCoyoteTime && Time.time > _wallJumpCoyoteTimeStart + playerSettings.wallJumpCoyoteTime)
            {
                _wallJumpCoyoteTime = false;
            }
        }

        public void StartCoyoteTime() => _coyoteTime = true;

        public void StartWallJumpCoyoteTime()
        {
            _wallJumpCoyoteTime = true;
            _wallJumpCoyoteTimeStart = Time.time;
        }

        public void StopWallJumpCoyoteTime() => _wallJumpCoyoteTime = false;
        
        public void SetIsJumping() => _isJumping = true;
    }
}
