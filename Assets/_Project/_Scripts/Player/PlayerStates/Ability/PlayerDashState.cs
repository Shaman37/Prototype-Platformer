using UnityEngine;

namespace PlayerController2D
{
    public class PlayerDashState : PlayerAbilityState
    {
        public bool canDash { get; private set; }
        private float _lastDashTime;

        private bool _isHolding;
        private bool _dashInputStop;
        private Vector2 _dashDirection;
        private Vector2 _lastAfterImagePosition;
        private bool _jumpInput;
        private bool _isTouchingFacingWall;
        

        public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            canDash = false;
            player.inputController.UseDashInput();
            _isHolding = true;
            _dashDirection = Vector2.right * player.facingDirection;

            player.rigidBody.drag = playerSettings.dashDrag;
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            _jumpInput = player.inputController.jumpInput;
            _isTouchingFacingWall = player.CheckIfTouchingFacingWall();

            if (!isExitingState)
            {                
                if (_isHolding)
                {
                    player.SetVelocity(playerSettings.dashVelocity, _dashDirection);
                    _dashInputStop = player.inputController.dashInputStop;

                    CheckIfShouldPlaceAfterImage();

                    if (_dashInputStop || Time.time >= stateStartTime + playerSettings.maxDashTime)
                    {
                        _isHolding = false;
                        isAbilityDone = true;
                    }
                }

                // [TRANSITION] -> Jump State
                if (_jumpInput && player.jumpState.CheckIfCanJump())
                {
                    player.stateMachine.ChangeState(player.jumpState);
                    isAbilityDone = true;
                }
                // [TRANSITION] -> Wall Slide State
                else if (_isTouchingFacingWall && player.CheckIfFalling())
                {
                    stateMachine.ChangeState(player.wallSlideState);
                    isAbilityDone = true;
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            _lastDashTime = Time.time;
            player.rigidBody.drag = 0f;
            PlaceAfterImage();
        }

        public bool CheckIfCanDash() => canDash && (Time.time >= _lastDashTime + playerSettings.dashCooldown);

        public bool ResetDash() => canDash = true;

        private void CheckIfShouldPlaceAfterImage()
        {
            if (Vector2.Distance(player.transform.position, _lastAfterImagePosition) >= playerSettings.dashAfterImageDistance)
            {
                PlaceAfterImage();
            }
        }

        private void PlaceAfterImage()
        {
            AfterImagePool.Instance.GetFromPool();
            _lastAfterImagePosition = player.transform.position;
        }

    }
}
