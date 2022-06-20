using UnityEngine;

namespace PlayerController2D
{
	/// <summary>
    /// 	Represents a player state.
    /// </summary>
	public class PlayerState
	{
       #region [0] - Fields

		 	protected Player 			 player;
		    protected PlayerStateMachine stateMachine;
		    protected PlayerSettings     playerSettings;
			protected float 			 stateStartTime;
        protected bool isAnimationFinished;

        protected string animatorBoolName;

	   #endregion

		/// <summary>
        /// 	Player State Constructor.
        /// </summary>
		public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName)
		{
            this.player = player;
            this.stateMachine = stateMachine;
            this.playerSettings = playerSettings;
            this.animatorBoolName = animatorBoolName;
        }

		/// <summary>
        /// 	Gets called when the player enters a specific state.
        /// </summary>
		public virtual void Enter()
		{
            StateCheck();
            player.animator.SetBool(animatorBoolName, true);
            stateStartTime = Time.time;
            isAnimationFinished = false;

            Debug.Log(animatorBoolName);
        }

		/// <summary>
        /// 	Gets called when the player exits a specific state.
        /// </summary>
		public virtual void Exit()
		{
            player.animator.SetBool(animatorBoolName, false);
		}

		/// <summary>
        /// 	Gets called on Update (every frame).
        /// </summary>
		public virtual void UpdateLogic() {}

		/// <summary>
        /// 	Gets called on Fixed Update.
        /// </summary>
		public virtual void UpdatePhysics()
		{
            StateCheck();
        }

		/// <summary>
        /// 	Gets called when a variable check is needed. Mostly used on 'Enter' and 'UpdatePhysics' methods.
        /// </summary>
		public virtual void StateCheck() {}

		public virtual void TriggerAnimation() {}

        public virtual void FinishAnimation() => isAnimationFinished = true;
    }
}
