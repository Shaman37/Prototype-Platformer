using UnityEngine;

namespace PlayerController2D
{
    public class PlayerJumpState : PlayerAbilityState
    {
        public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {

        }

        public override void Enter()
        {
            base.Enter();

            player.SetVelocityY(playerSettings.jumpVelocity);
            isAbilityDone = true;
        }
    }
}
