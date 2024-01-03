// Credits to Shinjingi for the base character controller code:
//    - (11/23) https://github.com/Shinjingi/Unity2D-Platform-Character-Controller

namespace Entity.Utils
{
    using UnityEngine;
    
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterSoundController))]
    public class CharacterMovementController : MonoBehaviour
    {
        protected Rigidbody2D body;
        protected Animator animator;
        protected CharacterSoundController soundController;
        
        [Header("Movement Parameters")]
        [SerializeField, Range(1f, 10f)] protected float maxSpeed = 6.0f;
        [SerializeField, Range(1f, 100f)] protected float maxAcceleration = 60f;
        [SerializeField, Range(0f, 100f)] protected float maxAirAcceleration = 30f;
        [SerializeField, Range(4f, 120f)] protected float jumpForce = 16.0f;
        [SerializeField, Range(1f, 10f)] protected float risingGravityScale = 2.7f;
        [SerializeField, Range(1f, 10f)] protected float fallingGravityScale = 6f;

        [Header("Ground Check")]
        [SerializeField] private LayerMask groundLayer = 1 << 3;
        [SerializeField, Range(0f, 90f)] private float maxNormalTilt = 60f;
        private ContactFilter2D groundFilter;

        protected bool onGround;
        protected bool hasJumped;
        
        protected Vector2 velocity;
        protected Vector2 desiredVelocity;
        private float acceleration;
        
        private Vector2 input;

        protected virtual void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            soundController = GetComponent<CharacterSoundController>();
        
            // Setup ground check
            groundFilter.useLayerMask = true;
            groundFilter.useNormalAngle = true;
            groundFilter.layerMask = groundLayer;
            groundFilter.minNormalAngle = 90f - maxNormalTilt;
            groundFilter.maxNormalAngle = 90f + maxNormalTilt;
        }

        protected virtual void Update()
        {
            input = GetMovementInput();
            desiredVelocity = new Vector2(maxSpeed * input.x, input.y);
        }

        private void FixedUpdate()
        {
            // Initialize current physics states
            onGround = CheckGrounded();
            body.gravityScale = SetGravity();

            
            // Start movement
            velocity = body.velocity;
            
            acceleration = onGround ? maxAcceleration : maxAirAcceleration;

            Walk();
            Jump();
            
            body.velocity = velocity;

            // Flip if switched directions (-1 is left, 1 is right)
            transform.localScale = body.velocity.x switch
            {
                < 0 => new Vector3(-1, 1, 1),
                > 0 => new Vector3(1, 1, 1),
                _ => new Vector3(1, 1, 1)
            };
        }

        private bool CheckGrounded()
        {
            return body.IsTouching(groundFilter);
        }

        private float SetGravity()
        {
            return body.velocity.y switch
            {
                > 0 => risingGravityScale,
                < 0 => fallingGravityScale,
                0 => 1f,
                _ => body.gravityScale
            };
        }

        protected virtual void Walk()
        {
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, acceleration * Time.deltaTime);
        }
        
        protected virtual void Jump()
        {
            if (hasJumped)
            {
                hasJumped = false;
                return;
            }
            
            if (desiredVelocity.y > 0 && onGround)
            {
                velocity.y = jumpForce;
                hasJumped = true;
            }
        }

        protected virtual Vector2 GetMovementInput()
        {
            return new Vector2(0f, 0f);
        }
    }
}

