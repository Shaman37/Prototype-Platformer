using DG.Tweening;

namespace PlayerController2D
{
    public class PlayerSlideStandState : PlayerOnGroundState
    {

        public PlayerSlideStandState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.SetVelocityX(1f);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (!isExitingState)
            {
                if (isAnimationFinished)
                {
                    // [TRANSITION] -> Move State
                    if (inputX != 0)
                    {
                            stateMachine.ChangeState(player.moveState);
                    }
                    // [TRANSITION] -> Idle State
                    else
                    {
                            stateMachine.ChangeState(player.idleState);
                    }
                }
                
            }
        }
    }
}