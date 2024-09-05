using UnityEngine;
using Entity.Interfaces;
using Entity.Utils;

namespace Entity.Enemies.Cowboy
{
    using Entity.Player;
    
    public class CowboyMovement : CharacterMovementController
    {
        private Cowboy self;
        
        private IPositionTrackable playerPositionTracker;
        private Vector2 playerPosition;
        
        private void Reset()
        {
            maxSpeed = 4f;
            maxAcceleration = 20f;
            maxAirAcceleration = 15f;
        }

        private void Start()
        {
            self = GetComponent<Cowboy>();
            playerPositionTracker = FindFirstObjectByType<Player>();
        }

        protected override void Update()
        {
            if (!self.IsAlive) isAlive = false;
            
            base.Update();

            playerPosition = playerPositionTracker.GetPosition();

            // Animations and sound
        }
    }
}
