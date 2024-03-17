using System.Collections;
using UnityEngine;
using Entity.Utils;

namespace Entity.Player
{
    public class PlayerAttackRange : MonoBehaviour
    {
        private Animator animator;
        private CharacterSoundController soundController;
        
        [SerializeField] private Transform firePoint;
        
        [SerializeField] private GameObject rangeProjectile;
        [SerializeField] private float rangeAttackSpeed = 2f;
        private bool canFire = true;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip[] rangeAttackSounds;
        [SerializeField, Range(0f, 1f)] private float rangeAttackVolume = 0.8f;
        
        private float animationTime = 0.5f;
        private static readonly int IsRanged = Animator.StringToHash("isRanged");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            soundController = GetComponent<CharacterSoundController>();
        }

        private void Update()
        {
            if (Input.GetButton("Fire1") && canFire)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            soundController.PlaySound(rangeAttackSounds, rangeAttackVolume);
            animator.SetTrigger(IsRanged);

            canFire = false;
            StartCoroutine(Attack(1f / rangeAttackSpeed));
        }
        
        private IEnumerator Attack(float duration)
        {
            yield return new WaitForSeconds(animationTime);
            //shoot after animation finishes playing
            Instantiate(rangeProjectile, firePoint.position, transform.localScale.x > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0));
            //attack cooldown
            yield return new WaitForSeconds(duration);
            canFire = true;
        }
    }
}