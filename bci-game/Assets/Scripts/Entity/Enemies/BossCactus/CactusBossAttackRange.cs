using System.Collections;
using Entity.Utils;
using UnityEngine;

namespace Entity.Enemies.BossCactus
{
    using Entity.Player;
    
    public class CactusBossAttackRange : MonoBehaviour
    {
        private CactusBoss self;
        private Animator animator;
        private CharacterSoundController soundController;
        
        private Rigidbody2D player;
        
        private bool canAttack = true;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float suctionForce = 10f;
        [SerializeField] private float suctionDuration = 0.8f;
        [SerializeField] private float suctionCooldown = 1f;
        
        [SerializeField] private float visionRange = 16f;
        [SerializeField] private float visionAngle = 75f;
        [SerializeField] private int visionRayCount = 5;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip[] rangeAttackSounds;
        [SerializeField, Range(0f, 1f)] private float rangeAttackVolume = 0.8f;

        private static readonly int Attack = Animator.StringToHash("Attack");


        private void Awake()
        {
            soundController = GetComponent<CharacterSoundController>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            self = GetComponent<CactusBoss>();
            player = FindFirstObjectByType<Player>().GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (canAttack && self.IsAlive && CheckPlayerInRange())
                StartCoroutine(VacuumAttack());
        }
        
        private bool CheckPlayerInRange()
        {
            Vector2 facing = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
            int facingSign = transform.localScale.x < 0 ? -1 : 1;

            float playerDistance = Vector2.Distance(transform.position, player.position);
            Vector2 playerDirection = (player.position - (Vector2) transform.position).normalized;
            float playerAngle = Vector2.Angle(facing, playerDirection);
            
            if (playerAngle > visionAngle / 2 || playerDistance > visionRange) 
                return false;

            for (int i = 0; i < visionRayCount; i++)
            {
                // Start from 0 offset and increment to full offset
                // Boss only looks at horizontal and up
                float offsetAngle = facingSign * ((visionAngle / 2) / (visionRayCount - 1)) * i;
                Vector3 rayDirection = Quaternion.Euler(0, 0, offsetAngle) * facing;

                // Debug.DrawRay(firePoint.position, rayDirection.normalized * visionRange, Color.green, duration: 1);
                RaycastHit2D hit = Physics2D.Raycast(
                    firePoint.position,
                    rayDirection,
                    visionRange,
                    ~LayerMask.GetMask("Enemies"));
                

                // If hit player, return true
                if (hit && hit.collider.CompareTag("Player"))
                    return true;
            }

            return false;
        }

        private IEnumerator VacuumAttack()
        {
            canAttack = false;
            float attackTime = 0f;

            animator.SetTrigger(Attack);
            soundController.PlaySound(rangeAttackSounds, rangeAttackVolume);
            while (attackTime < suctionDuration)
            {
                Vector2 direction = (firePoint.position - player.transform.position).normalized;
                float taper = 1 - (attackTime / suctionDuration);
                player.AddForce(direction * (suctionForce * taper * Time.fixedDeltaTime), ForceMode2D.Force);
                
                attackTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            StartCoroutine(AttackCooldown(suctionCooldown));
        }

        private IEnumerator AttackCooldown(float duration)
        {
            yield return new WaitForSeconds(duration);
            canAttack = true;
        }
    }
}