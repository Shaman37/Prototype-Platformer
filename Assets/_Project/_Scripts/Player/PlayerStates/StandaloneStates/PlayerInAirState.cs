using UnityEngine;

namespace PlayerController2D
{
    public class PlayerInAirState : PlayerState
    {
        private int  _inputX;
        private bool _isGrounded;
        private bool _jumpInput;
        private bool _jumpInputStop;
        private bool _coyoteTime;
        private bool _isJumping;

        public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
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
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            CheckCoyoteTime();

            _inputX = player.inputController.normalizedInputX;
            _jumpInput = player.inputController.jumpInput;
            _jumpInputStop = player.inputController.jumpInputStop;

            ApplyJumpMultiplier();

            if (_isGrounded && player.currentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.landState);
            }
            else if (_jumpInput && player.jumpState.CheckIfCanJump())
            {
                stateMachine.ChangeState(player.jumpState);
            }
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

        public void StartCoyoteTime() => _coyoteTime = true;

        public void SetIsJumping() => _isJumping = true;
    }
}
