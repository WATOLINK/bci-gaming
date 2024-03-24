using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public float attackRange = 1f;
    public float attackDamage = 10;
    public float attackCooldown = 1f;
    public Transform frontCheck;
    public LayerMask playerLayer;
    public int walkTime = 5;

    private bool canAttack = true;
    private bool facingRight = true;
    private Vector3 startPosition;


    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
        startPosition = transform.position; // Store the initial position
    }

    void Update()
    {
        float[] inputs = GetInput();

        float hChange = UpdateHorizontal(inputs[0]) * Time.deltaTime;

        // Check if the character has moved past a certain distance and flip if needed.
        if (Mathf.Abs(transform.position.x - startPosition.x) >= walkTime)
        {
            Flip();
        }

        // Flip movement direction if enemy encounters an obstacle
        if (hChange > 0 && movementBlocked[2] > 0)
        {
            hChange = 0;
            Flip();
        }
        else if (hChange < 0 && movementBlocked[3] > 0)
        {
            hChange = 0;
            Flip();
        }

        float vChange = UpdateVertical() * Time.deltaTime;
        if (vChange > 0 && movementBlocked[0] > 0) vChange = 0;
        else if (vChange < 0 && movementBlocked[1] > 0) vChange = 0;

        StartCoroutine(MoveCharacter(new Vector3(hChange, vChange, 0)));

        if (IsPlayerInAttackRange())
        {
            PerformAttack();
        }
    }

    public IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        yield return null;
    }

    void PerformAttack()
    {
        if (canAttack)
        {
            StartCoroutine(AttackCooldown()); // prevent further attacks during cooldown
            animator.SetTrigger("banditAttack");
            Collider[] hitColliders = Physics.OverlapSphere(frontCheck.position, attackRange);

            foreach (var hitCollider in hitColliders)
            {

                if (hitCollider.CompareTag("Player"))
                {
                    Player player = hitCollider.GetComponent<Player>(); // Reference to the Player class
                    player.decreaseHealth(attackDamage); // Call decreaseHealth method from Player class
                    Debug.Log("PLAYER HIT");
                    break; // Exit the loop as soon as a player is found
                }
            }
        }
    }

    public override float[] GetInput()
    {
        if (facingRight)
            return new float[] { 1, 0 };
        else
            return new float[] { -1, 0 };
    }

    public bool IsPlayerInAttackRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(frontCheck.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        startPosition = transform.position; // Update the starting position
    }
}
