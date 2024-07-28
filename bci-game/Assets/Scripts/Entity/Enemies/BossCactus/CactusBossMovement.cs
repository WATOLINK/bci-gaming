using Entity.Interfaces;
using Entity.Utils;
using UnityEngine;

namespace Entity.Enemies.BossCactus
{
    using Entity.Player;

    public class CactusBossMovement : CharacterMovementController
    {
        private CactusBoss self;

        private IPositionTrackable playerPositionTracker;
        private Vector2 playerPosition;
        
        private void Reset()
        {
            maxSpeed = 2f;
            maxAcceleration = 10f;
            maxAirAcceleration = 1f;
        }
        
        private void Start()
        {
            self = GetComponent<CactusBoss>();
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
            float distanceToPlayer = Mathf.Abs(playerPosition.x - transform.position.x);
            if (distanceToPlayer is > 0.1f and < 16f)
            {
                return playerPosition.x < transform.position.x ? new Vector2 (-1f, 0f) : new Vector2(1f, 0f);
            }

            return new Vector2(0f, 0f);
        }
    }
}