using UnityEngine;
using Entity.Interfaces;
using Entity.Utils;

namespace Entity.Enemies.Bandit
{
    using Entity.Player;
    
    public class BanditMovement : CharacterMovementController
    {
        private Bandit self;
        
        private IPositionTrackable playerPositionTracker;
        private Vector2 playerPosition;
        
        private static readonly int Facing = Animator.StringToHash("Bandit_X");
        
        private void Reset()
        {
            maxSpeed = 4f;
            maxAcceleration = 20f;
            maxAirAcceleration = 15f;
        }

        private void Start()
        {
            self = GetComponent<Bandit>();
            playerPositionTracker = FindFirstObjectByType<Player>();
        }

        protected override void Update()
        {
            if (!self.IsAlive) isAlive = false;
            
            base.Update();

            playerPosition = playerPositionTracker.GetPosition();
            
            // Animations and sound
        }

        protected override Vector2 GetMovementInput()
        {
            // Sample AI, can call an AI utils function instead
            float distanceToPlayer = Mathf.Abs(playerPosition.x - transform.position.x);
            if (distanceToPlayer is > 0.1f and < 11f)
            {
                return playerPosition.x < transform.position.x ? new Vector2 (-1f, 0f) : new Vector2(1f, 0f);
            }

            return new Vector2(0f, 0f);
        }
    }
}