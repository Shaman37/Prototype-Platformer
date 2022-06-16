using UnityEngine;

namespace PlayerController2D
{
	public class PlayerStateMachine
	{
        public PlayerState currentState { get; private set; }

		public void Init(PlayerState state	)
		{
            currentState = state;
            currentState.Enter();
        }

		public void ChangeState(PlayerState newState)
		{
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}
