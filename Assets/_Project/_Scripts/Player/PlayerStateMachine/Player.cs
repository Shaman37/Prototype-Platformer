using UnityEngine;

namespace PlayerController2D
{
	public class Player : MonoBehaviour
	{
        #region [0] - State Fields

        public PlayerStateMachine stateMachine { get; private set; }
        public PlayerIdleState    idleState { get; private set; }
        public PlayerMoveState    moveState { get; private set; }
        public PlayerJumpState    jumpState { get; private set; }
        public PlayerInAirState   inAirState { get; private set; }
        public PlayerLandState    landState { get; private set; }

        #endregion

		#region [1] - Component Fields
        
        public Animator 		                animator { get; private set; }
        public Rigidbody2D                      rigidBody { get; private set; }
        public PlayerInputController            inputController { get; private set; }
        [SerializeField] private PlayerSettings _playerSettings;
        
        #endregion
        
        #region [2] - Movement Fields
        
        public Vector2 currentVelocity { get; private set; }
        public int     facingDirection { get; private set; }
        
        #endregion
        
        #region [3] - Check Fields
        [SerializeField] private Transform _groundCheck;
        
        #endregion

        #region [4] - Unity Event Methods

        private void Awake()
 	    {
            stateMachine = new PlayerStateMachine();

            idleState = new PlayerIdleState(this, stateMachine, _playerSettings, "idle");
            moveState = new PlayerMoveState(this, stateMachine, _playerSettings, "move");
            jumpState = new PlayerJumpState(this, stateMachine, _playerSettings, "inAir");
            inAirState = new PlayerInAirState(this, stateMachine, _playerSettings, "inAir"); 
            landState = new PlayerLandState(this, stateMachine, _playerSettings, "land");

            animator = GetComponent<Animator>();
            inputController = GetComponent<PlayerInputController>();
            rigidBody = GetComponent<Rigidbody2D>();
        }

 	    private void Start()
 	    {            
            facingDirection = 1;

            stateMachine.Init(idleState);
        }

 	    private void Update()
 	    {
            currentVelocity = rigidBody.velocity;
            stateMachine.currentState.UpdateLogic();
        }

 	    private void FixedUpdate()
 	    {
            stateMachine.currentState.UpdatePhysics();
        }

        #endregion

        public void SetVelocityX(float velocityX)
        {
            Vector2 newVelocity = new Vector2(velocityX, currentVelocity.y);
            
            rigidBody.velocity = newVelocity;
            currentVelocity = newVelocity;
        }
        
        public void SetVelocityY(float velocityY)
        {
            Vector2 newVelocity = new Vector2(currentVelocity.x, velocityY);
            
            rigidBody.velocity = newVelocity;
            currentVelocity = newVelocity;
        }

        public void Flip()
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        public bool CheckIfGrounded()
        {
            return Physics2D.OverlapCircle(_groundCheck.position, _playerSettings.groundCheckRadius, _playerSettings.groundLayer);
        }

        public void CheckIfShouldFlip(int inputX)
        {
            if (inputX != 0 && inputX != facingDirection)
            {
                Flip();
            }
        }
    }
}
