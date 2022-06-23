using UnityEngine;

namespace PlayerController2D
{
	public class Player : MonoBehaviour
	{
        #region [0] - State Fields

        public PlayerStateMachine   stateMachine { get; private set; }

        public PlayerIdleState      idleState { get; private set; }
        public PlayerMoveState      moveState { get; private set; }
        public PlayerJumpState      jumpState { get; private set; }
        public PlayerInAirState     inAirState { get; private set; }
        public PlayerLandState      landState { get; private set; }
        public PlayerWallSlideState wallSlideState { get; private set; }
        public PlayerWallJumpState  wallJumpState { get; private set; }
        public PlayerCrouchState    crouchState { get; private set; }
        public PlayerLedgeState     ledgeState { get; private set; }
        public PlayerDashState      dashState { get; private set; }
        public PlayerSlideState     slideState { get; private set; }

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
        [SerializeField] private Transform _wallCheck;
        [SerializeField] private Transform _ledgeCheck;
        
        #endregion

        #region [4] - Unity Event Methods

        private void Awake()
 	    {
            stateMachine = new PlayerStateMachine();

            idleState = new PlayerIdleState(this, stateMachine, _playerSettings, "idle");
            moveState = new PlayerMoveState(this, stateMachine, _playerSettings, "move");
            crouchState = new PlayerCrouchState(this, stateMachine, _playerSettings, "crouching");
            jumpState = new PlayerJumpState(this, stateMachine, _playerSettings, "inAir");
            inAirState = new PlayerInAirState(this, stateMachine, _playerSettings, "inAir"); 
            landState = new PlayerLandState(this, stateMachine, _playerSettings, "hasLanded");
            wallSlideState = new PlayerWallSlideState(this, stateMachine, _playerSettings, "wallSliding");
            wallJumpState = new PlayerWallJumpState(this, stateMachine, _playerSettings, "inAir");
            ledgeState = new PlayerLedgeState(this, stateMachine, _playerSettings, "ledgeState");
            dashState = new PlayerDashState(this, stateMachine, _playerSettings, "dashing");
            slideState = new PlayerSlideState(this, stateMachine, _playerSettings, "isSliding");

            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody2D>();
            inputController = GetComponent<PlayerInputController>();
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

        #region [5] - Set Methods
            
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();

            Vector2 newVelocity = new Vector2(angle.x * velocity * direction, angle.y * velocity);
            
            rigidBody.velocity = newVelocity;
            currentVelocity = newVelocity;
        }
    
        public void SetVelocity(float velocity, Vector2 direction)
        {
            direction.Normalize();

            Vector2 newVelocity = direction * velocity;
            
            rigidBody.velocity = newVelocity;
            currentVelocity = newVelocity;
        }
        
        public void SetVelocityZero() => rigidBody.velocity = Vector2.zero;

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

        #endregion

        #region [6] - Check Methods
            
        public bool CheckIfGrounded()
        {
            return Physics2D.OverlapCircle(_groundCheck.position, _playerSettings.groundCheckRadius, _playerSettings.groundLayer);
        }

        public bool CheckIfTouchingFacingWall()
        {
            return Physics2D.Raycast(_wallCheck.position, Vector2.right * facingDirection, _playerSettings.wallCheckDistance, _playerSettings.groundLayer);
        }
        public bool CheckIfTouchingLedge()
        {
            return Physics2D.Raycast(_ledgeCheck.position, Vector2.right * facingDirection, _playerSettings.ledgeCheckDistance, _playerSettings.groundLayer);
        }
        
        public bool CheckIfTouchingBackWall()
        {
            return Physics2D.Raycast(_wallCheck.position, Vector2.right * -facingDirection, _playerSettings.wallCheckDistance, _playerSettings.groundLayer);
        }

        public bool CheckIfFalling()
        {
            return currentVelocity.y <= 0.0f;
        }

        public void CheckIfShouldFlip(int inputX)
        {
            if (inputX != 0 && inputX != facingDirection)
            {
                Flip();
            }
        }

        #endregion
    
        public void Flip()
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        private void TriggerAnimation() => stateMachine.currentState.TriggerAnimation();

        private void FinishAnimation() => stateMachine.currentState.FinishAnimation();
        
        public Vector2 CalculateCornerPosition()
        {
            Vector2 rayOrigin = Vector2.zero;
            Vector2 rayDirection = Vector2.right;
            float rayDistance = 0;

            // Calculate distance to wall (X)
            rayOrigin.Set(_wallCheck.position.x, _wallCheck.position.y);
            rayDirection = Vector2.right * facingDirection;
            rayDistance = _playerSettings.wallCheckDistance;
            RaycastHit2D hitX = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, _playerSettings.groundLayer);

            // Calculate distance to ledge ground (Y)
            rayOrigin.Set(_ledgeCheck.position.x + (hitX.distance * facingDirection), 0.0f);
            rayDirection = Vector2.down;
            rayDistance = _ledgeCheck.position.y - _wallCheck.position.y;
            RaycastHit2D hitY = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, _playerSettings.groundLayer);

            Vector2 cornerPosition = new Vector2(_wallCheck.position.x + (hitX.distance * facingDirection), _ledgeCheck.position.y - hitY.distance);

            return cornerPosition;
        }
    }
}
