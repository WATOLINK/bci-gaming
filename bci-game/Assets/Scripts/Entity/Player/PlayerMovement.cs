namespace Entity.Player
{
    using UnityEngine;
    using Entity.Utils;

    public class PlayerMovement : CharacterMovementController
    {
        private Player self;
        
        // [Header("Movement Buffers")]
        // [SerializeField, Range(0, 10)] private int preJumpBuffer = 3;
        // [SerializeField, Range(0, 10)] private int postJumpBuffer = 3;
        
        [Header("Sounds")]
        [SerializeField] private float footstepInterval = 0.5f;
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private AudioClip[] jumpSounds;
        [SerializeField, Range(0f, 1f)] private float footstepVolume = 0.8f;
        [SerializeField, Range(0f, 1f)] private float jumpVolume = 0.8f;
        
        private float footstepTimer = 0f;
        
        private static readonly int Facing = Animator.StringToHash("X");
        private static readonly int Walking = Animator.StringToHash("IsWalking");
        private static readonly int Jumping = Animator.StringToHash("isJumping");
        private static readonly int Takeoff = Animator.StringToHash("takeoff");

        public bool canJump = true;
        
        private void Start()
        {
            self = GetComponent<Player>();
            animator.SetFloat(Facing, 1);
        }

        protected override void Update()
        {
            if (!self.IsAlive) isAlive = false;
            
            base.Update();
            
            // Animations and sound
            animator.SetBool(Walking, Mathf.Abs(body.velocity.x) > 0.2f);
            animator.SetBool(Jumping, Mathf.Abs(body.velocity.y) > 0.2f);

            PlayFootstepSounds();
        }

        private void PlayFootstepSounds()
        {
            footstepTimer += Time.deltaTime;
            
            if (!onGround || Mathf.Abs(body.velocity.x) < 0.2f) return;
            if (footstepTimer > footstepInterval)
            {
                soundController.PlaySound(footstepSounds, footstepVolume);
                footstepTimer = 0f;
            }
        }

        protected override bool Jump()
        {
            // Check if current jump is valid
            if (!canJump || Input.GetAxisRaw("Vertical") == 0) return false;

            if (!base.Jump()) // If base jump fails, return false
                return false;
            
            // Set jump state
            canJump = false;

            animator.SetTrigger(Takeoff);
            soundController.PlaySound(jumpSounds, jumpVolume);
            
            return true;
        }

        protected override Vector2 GetMovementInput() {
            // Check to reset Jump
            if (!canJump) if (Input.GetAxisRaw("Vertical") == 0 && onGround) canJump = true;
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}
