namespace Entity.Enemies.Cowboy
{
    using UnityEngine;
    using Entity.Interfaces;
    using System.Collections;
    
    public class CowboyAttack : MonoBehaviour
    {
        private Animator animator;
        private bool isFacingRight;
        
        private bool isShoot;
        [SerializeField] private float damageAmount = 5f;
        [SerializeField] private float knockbackForce = 15f;

        private static readonly int Attack = Animator.StringToHash("CowboyAttack");
        private static readonly int Attack2 = Animator.StringToHash("CowboyAttack2");
        private float waitTime = 0.7f;
        private float cooldownTime = 3f;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            isShoot = true;
        }

        private void Update()
        {
            CheckDirection();
        }
        
        // Basic attacks
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && isShoot)
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable == null) return;
                
                if(!isFacingRight) {
                    animator.SetTrigger(Attack);
                } else {
                    animator.SetTrigger(Attack2);
                }
                StartCoroutine(AnimationWait(waitTime));

                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                knockbackDirection.y += 0.8f;
                
                damageable.Damage(damageAmount, knockbackDirection, knockbackForce);

                isShoot = false;
                StartCoroutine(AttackCooldown(cooldownTime));
            }
        }

        private IEnumerator AnimationWait(float duration)
        {
            yield return new WaitForSeconds(duration);
        }

        private IEnumerator AttackCooldown(float duration)
        {
            yield return new WaitForSeconds(duration);
            isShoot = true;
        }

        private void CheckDirection()
        {
            Transform player = GameObject.FindWithTag("Player").transform;
            isFacingRight = player.position.x > transform.position.x;
        }
    }
}