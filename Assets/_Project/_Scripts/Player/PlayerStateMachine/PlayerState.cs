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
			protected float 			 startTime;
			
			private string _animatorBoolName;

	   #endregion

		/// <summary>
        /// 	Player State Constructor.
        /// </summary>
		public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerSettings playerSettings, string animatorBoolName)
		{
            this.player = player;
            this.stateMachine = stateMachine;
            this.playerSettings = playerSettings;
            this._animatorBoolName = animatorBoolName;
        }

		/// <summary>
        /// 	Gets called when the player enters a specific state.
        /// </summary>
		public virtual void Enter()
		{
            StateCheck();
            player.animator.SetBool(_animatorBoolName, true);
            startTime = Time.time;

			Debug.Log(_animatorBoolName);
        }

		/// <summary>
        /// 	Gets called when the player exits a specific state.
        /// </summary>
		public virtual void Exit()
		{
            player.animator.SetBool(_animatorBoolName, false);
		}

		/// <summary>
        /// 	Gets called on Update (every frame).
        /// </summary>
		public virtual void UpdateLogic()
		{
			
		}

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
		public virtual void StateCheck()
		{
			
		}
    }
}
