using UnityEngine;
using DG.Tweening;

namespace PlayerController2D
{
    public class PlayerOnLedgeState : PlayerState
    {
        private Vector2 _startPosition;
        public Vector2 _holdPosition;
        public Vector2 _endPosition;
        public Vector2 _cornerPosition;
        public Vector2 _controlPoint1;
        public Vector2 _controlPoint2;

        private bool _isHangingOnLedge;
        private bool _isClimbingLedge;
        public bool _willTouchCeiling;
        private bool _jumpInput;

        private int _inputX;
        private int _inputY;

        public PlayerOnLedgeState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.SetVelocityZero();
            player.transform.position = _startPosition;

            _cornerPosition = player.CalculateLedgeCornerPosition();
            _holdPosition.Set(_cornerPosition.x - (playerSettings.holdOffset.x * player.facingDirection), _cornerPosition.y - playerSettings.holdOffset.y);
            _endPosition.Set(_cornerPosition.x + (playerSettings.endOffset.x * player.facingDirection), _cornerPosition.y + playerSettings.endOffset.y);
         
            _controlPoint1.Set(_holdPosition.x + (playerSettings.climbControlPoint1.x * player.facingDirection), _holdPosition.y + playerSettings.climbControlPoint1.y);
            _controlPoint2.Set(_endPosition.x + (playerSettings.climbControlPoint2.x * player.facingDirection), _endPosition.y + playerSettings.climbControlPoint2.y);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            _controlPoint1.Set(_holdPosition.x + (playerSettings.climbControlPoint1.x * player.facingDirection), _holdPosition.y + playerSettings.climbControlPoint1.y);
            _controlPoint2.Set(_endPosition.x + (playerSettings.climbControlPoint2.x * player.facingDirection), _endPosition.y + playerSettings.climbControlPoint2.y);
            _endPosition.Set(_cornerPosition.x + (playerSettings.endOffset.x * player.facingDirection), _cornerPosition.y + playerSettings.endOffset.y);

            if (isAnimationFinished)
            {                
                if (_willTouchCeiling)
                {
                    stateMachine.ChangeState(player.crouchIdleState);
                }
                else
                {
                    stateMachine.ChangeState(player.idleState);
                }
            }
            else
            {
                player.SetVelocityZero();

                _inputX = player.inputController.normalizedInputX;
                _inputY = player.inputController.normalizedInputY;
                _jumpInput = player.inputController.jumpInput;

                if (_isHangingOnLedge && !_isClimbingLedge && _inputX == player.facingDirection)
                {
                    _isClimbingLedge = true;
                    player.animator.SetBool("climbingLedge", true);

                    HandleClimb();
                    CheckForSpace();
                }
                // [TRANSITION] -> In Air State
                else if (_isHangingOnLedge && !_isClimbingLedge && _inputY == -1)
                {
                    stateMachine.ChangeState(player.inAirState);
                }
                // [TRANSITION] -> Wall Jump State
                else if (_jumpInput)
                {
                    stateMachine.ChangeState(player.wallJumpState);
                }
                else if (_isClimbingLedge)
                {
                    player.wallJumpState.DetermineWallJumpDirection(true);
                }
                else if (!_isClimbingLedge)
                {
                    player.transform.position = _holdPosition;
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            _isHangingOnLedge = false;

            if (_isClimbingLedge)
            {
                _isClimbingLedge = false;
            }
        }

        public override void TriggerAnimation()
        {
            base.TriggerAnimation();
            
            _isHangingOnLedge = true;
        }

        public override void FinishAnimation()
        {
            base.FinishAnimation();

            player.animator.SetBool("climbingLedge", false);
        }

        public void SetInitialPosition(Vector2 pos) => _startPosition = pos;

        public void HandleClimb()
        {
            player.transform.DOMoveY(_endPosition.y, 0.45f).SetEase(Ease.InSine);
            player.transform.DOMoveX(_endPosition.x, 0.2f).SetEase(Ease.InSine).SetDelay(0.4f);;
        }

        private void CheckForSpace()
        {
            Vector2 rayOrigin = new Vector2(_endPosition.x, _cornerPosition.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = playerSettings.standingColliderHeight;

            _willTouchCeiling = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, playerSettings.groundLayer);

            player.animator.SetBool("isTouchingCeiling", _willTouchCeiling);
        }
    }
}
