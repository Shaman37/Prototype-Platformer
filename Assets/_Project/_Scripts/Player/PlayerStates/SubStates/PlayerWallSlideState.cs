using UnityEngine;

namespace PlayerController2D
{
    public class PlayerWallSlideState : PlayerOnWallState
    {
        public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (!isExitingState)
            {
              player.SetVelocityY(-playerSettings.wallSlideVelocity);
            }
        }
    }
}
