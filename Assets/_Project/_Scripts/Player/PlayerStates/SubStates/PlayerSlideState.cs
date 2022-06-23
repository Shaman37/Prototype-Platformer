namespace PlayerController2D
{
    public class PlayerSlideState : PlayerAbilityState
    {
        public PlayerSlideState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName) : base(player, stateMachine, playerSettings, animatorBoolName)
        {
        }
    }
}