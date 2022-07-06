using UnityEngine;

namespace PlayerController2D
{
	public class Player : MonoBehaviour
	{
        #region [0] - State Fields

        public PlayerStateMachine   stateMachine { get; private set; }

        public PlayerIdleState       idleState { get; private set; }
        public PlayerMoveState       moveState { get; private set; }
        public PlayerCrouchIdleState crouchIdleState { get; private set; }
        public PlayerCrouchMoveState crouchMoveState { get; private set; }
        public PlayerSlideState      slideState { get; private set; }
        public PlayerSlideStandState slideStandState { get; private set; }
        public PlayerLandState       landState { get; private set; }

        public PlayerJumpState       jumpState { get; private set; }
        public PlayerDashState       dashState { get; private set; }
        public PlayerInAirState      inAirState { get; private set; }
        
        public PlayerWallSlideState  wallSlideState { get; private set; }
        public PlayerWallJumpState   wallJumpState { get; private set; }

        public PlayerOnLedgeState    ledgeState { get; private set; }

        #endregion

		#region [1] - Component Fields
        
        public Animator 		                animator { get; private set; }
        public BoxCollider2D                    boxCollider { get; private set; }
        public Rigidbody2D                      rigidBody { get; private set; }
        public PlayerInputController            inputController { get; private set; }
        [SerializeField] private PlayerSettings _playerSettings;
        
        #endregion
        
        #region [2] - Movement Fields
        
        public Vector2 currentVelocity { get; private set; }
        public int     facingDirection { get; private set; }
        
        #endregion
        
        #region [3] - Collision Check Fields
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private Transform _wallCheck;
        [SerializeField] private Transform _ledgeCheck;
        [SerializeField] private Transform _ceilingCheck;
        
        #endregion

        #region [4] - Unity Event Methods

        private void Awake()
 	    {
            // State initialization
            stateMachine = new PlayerStateMachine();
         
            idleState = new PlayerIdleState(this, stateMachine, _playerSettings, "idle");
            moveState = new PlayerMoveState(this, stateMachine, _playerSettings, "move");
            crouchIdleState = new PlayerCrouchIdleState(this, stateMachine, _playerSettings, "crouchingIdle");
            crouchMoveState = new PlayerCrouchMoveState(this, stateMachine, _playerSettings, "crouchingMove");
            jumpState = new PlayerJumpState(this, stateMachine, _playerSettings, "inAir");
            inAirState = new PlayerInAirState(this, stateMachine, _playerSettings, "inAir"); 
            landState = new PlayerLandState(this, stateMachine, _playerSettings, "hasLanded");
            wallSlideState = new PlayerWallSlideState(this, stateMachine, _playerSettings, "wallSliding");
            wallJumpState = new PlayerWallJumpState(this, stateMachine, _playerSettings, "inAir");
            ledgeState = new PlayerOnLedgeState(this, stateMachine, _playerSettings, "ledgeState");
            dashState = new PlayerDashState(this, stateMachine, _playerSettings, "dashing");
            slideState = new PlayerSlideState(this, stateMachine, _playerSettings, "isSliding");
            slideStandState = new PlayerSlideStandState(this, stateMachine, _playerSettings, "isStandingUp");

            // Components
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody2D>();
            inputController = GetComponent<PlayerInputController>();
            boxCollider = GetComponent<BoxCollider2D>();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="direction"></param>
        public void SetVelocity(float velocity, Vector2 direction)
        {
            direction.Normalize();

            Vector2 newVelocity = direction * velocity;
            
            rigidBody.velocity = newVelocity;
            currentVelocity = newVelocity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="angle"></param>
        /// <param name="direction"></param>
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();

            Vector2 newVelocity = new Vector2(angle.x * velocity * direction, angle.y * velocity);
            
            rigidBody.velocity = newVelocity;
            currentVelocity = newVelocity;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetVelocityZero() => rigidBody.velocity = Vector2.zero;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocityX"></param>
        public void SetVelocityX(float velocityX)
        {
            Vector2 newVelocity = new Vector2(velocityX, currentVelocity.y);

            rigidBody.velocity = newVelocity;
            currentVelocity = newVelocity;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocityY"></param>
        public void SetVelocityY(float velocityY)
        {
            Vector2 newVelocity = new Vector2(currentVelocity.x, velocityY);
            
            rigidBody.velocity = newVelocity;
            currentVelocity = newVelocity;
        }

        #endregion

        #region [6] - Check Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckIfGrounded()
        {
            return Physics2D.OverlapCircle(_groundCheck.position, _playerSettings.groundCheckRadius, _playerSettings.groundLayer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckForCeling()
        {
            return Physics2D.OverlapCircle(_ceilingCheck.position, _playerSettings.groundCheckRadius, _playerSettings.groundLayer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckIfTouchingFacingWall()
        {
            return Physics2D.Raycast(_wallCheck.position, Vector2.right * facingDirection, _playerSettings.wallCheckDistance, _playerSettings.groundLayer);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckIfTouchingLedge()
        {
            return Physics2D.Raycast(_ledgeCheck.position, Vector2.right * facingDirection, _playerSettings.ledgeCheckDistance, _playerSettings.groundLayer);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckIfTouchingBackWall()
        {
            return Physics2D.Raycast(_wallCheck.position, Vector2.right * -facingDirection, _playerSettings.wallCheckDistance, _playerSettings.groundLayer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckIfFalling()
        {
            return currentVelocity.y <= 0.0f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputX"></param>
        public void CheckIfShouldFlip(int inputX)
        {
            if (inputX != 0 && inputX != facingDirection)
            {
                Flip();
            }
        }

        #endregion

        #region [7] - 
        public void Flip()
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        private void TriggerAnimation() => stateMachine.currentState.TriggerAnimation();

        private void FinishAnimation() => stateMachine.currentState.FinishAnimation();
        
        public Vector2 CalculateLedgeCornerPosition()
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
            rayOrigin.Set(_ledgeCheck.position.x + (hitX.distance * facingDirection), _ledgeCheck.position.y);
            rayDirection = Vector2.down;
            rayDistance = _ledgeCheck.position.y - _wallCheck.position.y;
            RaycastHit2D hitY = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, _playerSettings.groundLayer);

            Vector2 cornerPosition = new Vector2(_wallCheck.position.x + (hitX.distance * facingDirection), _ledgeCheck.position.y - hitY.distance);

            return cornerPosition;
        }

        public void ChangeColliderHeight(float height)
        {
            Vector2 newSize = new Vector2(boxCollider.size.x, height);

            Vector2 newCenter = boxCollider.offset;
            newCenter.y += (height - boxCollider.size.y) * 0.5f;

            boxCollider.size = newSize;
            boxCollider.offset = newCenter;
        }

        #endregion

        private void OnDrawGizmos()
        {
            if(ledgeState != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(ledgeState._holdPosition, 0.1f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(ledgeState._controlPoint1, 0.1f);

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(ledgeState._controlPoint2, 0.1f);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(ledgeState._endPosition, 0.1f);

                Gizmos.color = Color.black;
                Gizmos.DrawSphere(ledgeState._cornerPosition, 0.1f);

                Gizmos.color = Color.black;
                Vector2 rayOrigin = new Vector2(ledgeState._endPosition.x, ledgeState._cornerPosition.y);
                Gizmos.DrawSphere(rayOrigin, 0.05f);

                Debug.DrawRay(rayOrigin, Vector3.up * _playerSettings.standingColliderHeight);
            }
        }
    }
}
