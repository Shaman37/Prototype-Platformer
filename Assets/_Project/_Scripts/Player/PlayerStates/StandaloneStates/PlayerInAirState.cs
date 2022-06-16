using UnityEngine;

namespace PlayerController2D
{
    public class PlayerInAirState : PlayerState
    {
        private int _inputX;
        private bool _isGrounded;

        public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {

        }

        public override void Enter()
        {
            base.Enter();
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

            _inputX = player.inputController.normalizedInputX;

            if (_isGrounded && player.currentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.landState);
            }
            else
            {
                player.CheckIfShouldFlip(_inputX);
                player.SetVelocityX(playerSettings.movementVelocity * _inputX);
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
