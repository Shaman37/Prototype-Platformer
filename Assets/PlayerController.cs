using System;
using UnityEngine;

namespace shaman37
{   
    public class PlayerController : MonoBehaviour
    {
	    [Header("Movement Settings")]
        [SerializeField] private float     _walkingSpeed = 10f;

        [Header("Jump Settings")]
        [SerializeField] private float     _jumpForce = 10f;
        [SerializeField] private float     _fallMultiplier = 2.5f;
        [SerializeField] private float     _lowJumpMultiplier = 2f;
        [SerializeField] private float     _groundExtraHeight = 0.05f;
        [SerializeField] private LayerMask _mask;
        
        // Flags 
        private bool _jumpRequest = false;
        private bool _isGrounded = false;
        
        private Rigidbody2D _rb;
        private CapsuleCollider2D _collider;


        #region [UnityEvents]
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            IsGroundedCheck();

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector2 direction = new Vector2(x, y);

            Walk(direction);
            
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _jumpRequest = true;
            }
        }

        private void FixedUpdate()
        {
            if (_jumpRequest)
            {
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

                _jumpRequest = false;
                _isGrounded = false;
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
            _rb.velocity = new Vector2(direction.x * _walkingSpeed, _rb.velocity.y);
        }

        private void IsGroundedCheck()
        {
            RaycastHit2D rayCastHit = Physics2D.CapsuleCast(
                _collider.bounds.center, // Capsule center
                _collider.bounds.size, // Capsule size
                CapsuleDirection2D.Vertical, // Capsule's direction
                0f, // Angle of the Capsule
                Vector2.down, // Direction to cast the capsule [down -> ground]
                _groundExtraHeight, // Maximum distance for the Capsule cast
                _mask // Detect collisions only on specified layer [default -> ground]
            );

            _isGrounded = rayCastHit.collider != null;
        }
        #endregion
    }
}