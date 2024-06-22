namespace Entity.Enemies.Bandit
{
    using UnityEngine;
    using Entity.Interfaces;
    
    public class BanditAttack : MonoBehaviour
    {
        private Animator animator;
        
        [SerializeField] private float damageAmount = 10f;
        [SerializeField] private float knockbackForce = 15f;

        // private static readonly int Attack = Animator.StringToHash("banditAttack");
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        
        // Basic attacks
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable == null) return;
                
                Debug.Log("AttackRnage");
                animator.SetTrigger("Attack"); // TODO: Fix reversal of animation
                
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                knockbackDirection.y += 0.8f;
                
                damageable.Damage(damageAmount, knockbackDirection, knockbackForce);
            }
        }
    }
}