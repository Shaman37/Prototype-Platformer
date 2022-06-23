using UnityEngine;
using DG.Tweening;

namespace PlayerController2D
{
    public class PlayerLedgeState : PlayerState
    {
        private Vector2 _targetPosition;
        private Vector2 _cornerPosition;
        private Vector2 _startPosition;
        private Vector2 _climbPosition;
        private Vector2 _stopPosition;
        
        private bool _isHangingOnLedge;
        private bool _isClimbingLedge;
        private bool _jumpInput;

        private int _inputX;
        private int _inputY;
        

        public PlayerLedgeState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.SetVelocityZero();
            player.transform.position = _targetPosition;
            _cornerPosition = player.CalculateCornerPosition();
            _startPosition.Set(_cornerPosition.x - (playerSettings.startOffset.x * player.facingDirection), _cornerPosition.y - playerSettings.startOffset.y);
            _climbPosition.Set(_cornerPosition.x - (playerSettings.climbOffset.x * player.facingDirection), _cornerPosition.y - playerSettings.climbOffset.y);
            _stopPosition.Set(_cornerPosition.x + (playerSettings.stopOffset.x * player.facingDirection), _cornerPosition.y + playerSettings.stopOffset.y);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                
                player.SetVelocityZero();
                player.transform.position = _startPosition;

                _inputX = player.inputController.normalizedInputX;
                _inputY = player.inputController.normalizedInputY;
                _jumpInput = player.inputController.jumpInput;

                if (_isHangingOnLedge && !_isClimbingLedge && _inputX == player.facingDirection)
                {
                    _isClimbingLedge = true;
                    player.animator.SetBool("climbingLedge", true);
                }
                else if (_isHangingOnLedge && !_isClimbingLedge && _inputY == -1)
                {
                    stateMachine.ChangeState(player.inAirState);
                }
                else if (_jumpInput)
                {
                    stateMachine.ChangeState(player.wallJumpState);
                }
                else if (_isClimbingLedge)
                {
                    player.wallJumpState.DetermineWallJumpDirection(true);
                    player.transform.position = _climbPosition;
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            _isHangingOnLedge = false;

            if (_isClimbingLedge)
            {
                player.transform.position = _stopPosition;
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

        public void SetInitialPosition(Vector2 pos) => _targetPosition = pos;
    }
}
