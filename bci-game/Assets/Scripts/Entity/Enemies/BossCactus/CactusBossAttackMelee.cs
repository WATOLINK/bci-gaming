using Entity.Interfaces;
using UnityEngine;

namespace Entity.Enemies.BossCactus
{
    public class CactusBossAttackMelee : MonoBehaviour
    {
        private Animator animator;
        
        [SerializeField] private float damageAmount = 20f;
        [SerializeField] private float knockbackForce = 20f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable == null) return;

                // animator.SetTrigger(SpikesOnlyAttack); 

                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                knockbackDirection.y += 0.8f;

                damageable.Damage(damageAmount, knockbackDirection, knockbackForce);
            }
        }
    }
}