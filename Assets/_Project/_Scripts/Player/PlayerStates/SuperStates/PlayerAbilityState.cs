using UnityEngine;

namespace PlayerController2D
{
    public class PlayerAbilityState : PlayerState
    {
        protected bool isAbilityDone;
        private bool _isGrounded;

        public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
			
        }

        public override void Enter()
        {
            base.Enter();

            isAbilityDone = false;
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

            if (isAbilityDone)
            {
                if (_isGrounded && player.currentVelocity.y < 0.01f)
                {
                    stateMachine.ChangeState(player.idleState);
                }
                else
                {
                    stateMachine.ChangeState(player.inAirState);
                }
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
