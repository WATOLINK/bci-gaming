using System.Collections;
using UnityEngine;

public class LongRange : Character
{
    public float attackRange = 1f;
    public int attackDamage = 10;
    public float attackCooldown = 2f;
    public Transform frontCheck;
    public LayerMask layer;
    public Transform playerTransform;

    private bool canAttack = true;
    private bool facingRight = true;

    private void Awake()
    {
        layer = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        //cowboy enemy
        if (canAttack)
        {
            if (IsPlayerInAttackRange())
            {
                PerformAttack();
            }
        }
    }

    private bool IsPlayerInAttackRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(frontCheck.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Debug.Log("IsPlayerInAttackRange is true");
                // Make sure the player is within attack range
                return true;
            }
        }
        return false;
    }

    private void PerformAttack()
    {
        canAttack = false; // Prevent further attacks during cooldown
        StartCoroutine(AttackCooldown());

        // Flip to face the player if needed
        if (IsPlayerOnTheOtherSide())
        {
            Flip();
        }

        animator.SetTrigger("CowboyAttack");

    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private bool IsPlayerOnTheOtherSide()
    {
        // Check if the player is on the other side of the character
        if (facingRight && playerTransform.position.x > frontCheck.position.x)
        {
            return true;
        }
        if (!facingRight && playerTransform.position.x < frontCheck.position.x)
        {
            return true;
        }
        return false;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
