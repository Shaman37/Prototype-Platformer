using UnityEngine;

namespace shaman37
{   
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float     _walkingSpeed = 10f;

        [Space]
        [Header("Jump Settings")]
        [SerializeField] private float     _jumpForce = 10f;
        [SerializeField] private float     _fallMultiplier = 2.5f;
        [SerializeField] private float     _lowJumpMultiplier = 2f;
    
        [Space]
        [Header("Wall Slide Settings")]
        [SerializeField] private float     _slideSpeed = 5f;

        [Space]
        [Header("Collider Settings")]
        [SerializeField] private LayerMask _mask;
        [SerializeField] private float     _groundCollisionRadius = 0.05f;
        [SerializeField] private float     _wallCollisionRadius = 0.2f;
        [SerializeField] public Vector2    _rightOffset;
        [SerializeField] public Vector2    _leftOffset;
        
        
         
        private bool wallSlide;

        // Animator bools
        [SerializeField] private bool _isJumping = false;
        [SerializeField] private bool _isGrounded = false;
        [SerializeField] private bool _isOnWall = false;
        [SerializeField] private bool _isOnRightWall = false;
        [SerializeField] private bool _isOnLeftWall = false;        
        
        // Cached Components
        private Rigidbody2D       _rb;
        private CapsuleCollider2D _collider;
        private Animator          _animator;
        


        #region [UnityEvents]
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            IsGroundedCheck();
            IsOnWallsCheck();

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector2 direction = new Vector2(x, y);

            Walk(direction);

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _isJumping = true;
                _isGrounded = false;

                _animator.SetTrigger("isJumping");
            }

            if (_rb.velocity.y < 0)
            {
                _animator.SetBool("isFalling", true);
            }else
            {
                _animator.SetBool("isFalling", false);
            }

            // if (wallSlide) return;
        }

        private void FixedUpdate()
        {
            if (_isJumping)
            {
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

                _isJumping = false;
            }

            HandleAirTime();
        }

        #endregion


        #region [Methods]
        private void HandleAirTime()
        {             
            if (_rb.velocity.y < 0)
            {
                _rb.gravityScale = _fallMultiplier;
            }
            else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                _rb.gravityScale = _lowJumpMultiplier;
            }
            else
            {
                _rb.gravityScale = 1;
            }
        }

        private void Walk(Vector2 direction)
        {
            float deltaX = direction.x * _walkingSpeed;
            _rb.velocity = new Vector2(deltaX, _rb.velocity.y);

            _animator.SetFloat("speed", Mathf.Abs(deltaX));
            if (!Mathf.Approximately(deltaX, 0)) {
                // Handle animation flip
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Sign(deltaX) * Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }

        private void WallSlide()
        {
            wallSlide = true;
            bool pushingWall = false;
            if((_rb.velocity.x > 0 && _isOnRightWall) || (_rb.velocity.x < 0 && _isOnLeftWall))
            {
                pushingWall = true;
            }

            float push = pushingWall ? 0 : _rb.velocity.x;

            _rb.velocity = new Vector2(push, -_slideSpeed);
        }

        private void IsGroundedCheck()
        {
            RaycastHit2D rayCastHit = Physics2D.CapsuleCast(
                _collider.bounds.center, // Capsule center
                _collider.bounds.size, // Capsule size
                CapsuleDirection2D.Vertical, // Capsule's direction
                0f, // Angle of the Capsule
                Vector2.down, // Direction to cast the capsule [down -> ground]
                _groundCollisionRadius, // Maximum distance for the Capsule cast
                _mask // Detect collisions only on specified layer [default -> ground]
            );

            _isGrounded = rayCastHit.collider != null;
        
            // Debugging
            Color rayColor = Color.red;
            if(_isGrounded) rayColor = Color.green;

            Debug.DrawRay(_collider.bounds.center, Vector2.down * (_collider.bounds.extents.y + _groundCollisionRadius), rayColor);
        }

        private void IsOnWallsCheck()
        {
            _isOnRightWall = Physics2D.OverlapCircle((Vector2)transform.position + _rightOffset, _wallCollisionRadius, _mask);
            _isOnLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + _leftOffset, _wallCollisionRadius, _mask);

            _isOnWall = _isOnRightWall || _isOnLeftWall;

            Debug.DrawRay((Vector2)transform.position + _rightOffset, Vector2.right * _wallCollisionRadius, _isOnRightWall ? Color.green : Color.red);
            Debug.DrawRay((Vector2)transform.position + _leftOffset, Vector2.left * _wallCollisionRadius, _isOnLeftWall ? Color.green : Color.red);
        }
        #endregion
    }
}