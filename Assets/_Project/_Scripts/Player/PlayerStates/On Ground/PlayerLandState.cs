using UnityEngine;

namespace PlayerController2D
{
    public class PlayerLandState : PlayerOnGroundState
    {
        public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {

        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

           if (!isExitingState)
           {
                // [TRANSITION] -> Move State
                if (inputX != 0)
                {
                        stateMachine.ChangeState(player.moveState);
                }
                // [TRANSITION] -> Idle State
                else if (isAnimationFinished)
                {
                        stateMachine.ChangeState(player.idleState);
                }
           }
        }
    }
}
