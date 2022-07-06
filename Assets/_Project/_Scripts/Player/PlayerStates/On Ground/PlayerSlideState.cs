using UnityEngine;

namespace PlayerController2D
{
    public class PlayerSlideState : PlayerOnGroundState
    {
        public bool canSlide { get; private set; }
        private bool _shouldStandUp;
        private float _lastSlideTime;

        public PlayerSlideState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            canSlide = false;
            _shouldStandUp = false;

            player.rigidBody.drag = playerSettings.slideDrag;
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (!isExitingState)
            {
                player.SetVelocityX(playerSettings.slideMovementVelocity * player.facingDirection);
                CheckIfShouldStandUp();
                
                if (_shouldStandUp)
                {
                    stateMachine.ChangeState(player.slideStandState);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            _lastSlideTime = Time.time;
            player.rigidBody.drag = 0f;
        }

        private void CheckIfShouldStandUp()
        {
            bool didMinimumSlideTime = playerSettings.minSlideTime >= stateElapsedTime;
            bool didMaximumSlideTime = stateElapsedTime >= (stateStartTime + playerSettings.maxSlideTime);

            if (didMinimumSlideTime && didMaximumSlideTime || inputY != -1)
            {
                _shouldStandUp = true;
            }
        }

        public bool CheckIfCanSlide() => canSlide && (Time.time >= _lastSlideTime + playerSettings.slideCooldown);
        public bool ResetSlide() => canSlide = true;
    }
}