using UnityEngine;

namespace PlayerController2D
{
    public class PlayerLandState : PlayerGroundedState
    {
        public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {

        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

           if (!isExitingState)
           {
             if (inputX != 0)
             {
                 stateMachine.ChangeState(player.moveState);
             }
             else if (isAnimationFinished)
             {
                 stateMachine.ChangeState(player.idleState);
             }
           }
        }
    }
}
