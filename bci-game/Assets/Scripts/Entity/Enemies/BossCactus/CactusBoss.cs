using Entity.Interfaces;
using Entity.Utils;
using UnityEngine;

namespace Entity.Enemies.BossCactus
{
    public class CactusBoss : MonoBehaviour, IDamageable
    {
        private Rigidbody2D body;
        private Animator animator;
        private CharacterSoundController soundController;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip[] hurtSounds;
        [SerializeField, Range(0f, 1f)] private float hurtVolume = 0.8f;
        [SerializeField] private AudioClip deathSound;
        [SerializeField, Range(0f, 1f)] private float deathVolume = 0.8f;  
        
        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            soundController = GetComponent<CharacterSoundController>();
            
            // Set health to max health on game start
            IsAlive = true;
            Health = MaxHealth;
        }
        
        // IDamageable implementation
        public bool IsAlive { get; private set; }
        public float MaxHealth => 150;
        public float Health { get; private set; }
        

        public void Damage(float damageAmount, Vector2 knockbackDirection, float knockbackForce)
        {
            if (Health <= 0) return; // Prevents damage after death
            
            Health -= damageAmount;
            if (Health <= 0) Die();
            
            soundController.PlaySound(hurtSounds, hurtVolume);
            // animator.SetTrigger(IsHurt);

            Vector2 knockback = knockbackDirection.normalized * knockbackForce;
            body.AddForce(knockback, ForceMode2D.Impulse);
        }

        // Stub; CactusBoss does not heal
        public void Heal(float healAmount) {}
        
        public void Die()
        {
            soundController.PlaySound(deathSound, deathVolume);
            // animator.SetTrigger(IsDead);
            
            float deathSoundLen = deathSound != null 
                ? deathSound.length 
                : 0;

            IsAlive = false;
            
            EntityUtils.MarkForDeath(gameObject, deathSoundLen, true);
        }
    }
}