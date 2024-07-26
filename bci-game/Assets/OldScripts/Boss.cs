using System.Collections;
using UnityEngine;

public class Boss : Character
{
    public int maxHealth = 100;
    public int currentHealth;
    public float attackRange = 3.0f;
    public float attackCooldown = 2.0f;
    public Transform target; // The player or target the boss will attack
    public int walkTime = 5;
    public Camera mainCamera;
    private float minX, maxX, minY, maxY;

    private bool isAttacking = false;
    private bool facingRight = true; 
    private Vector3 startPosition; 
    public float knockbackForce = 5f;
     private bool canFlip = true;
    public float flipCooldown = 1.0f;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        CalculateCameraBounds();
        // Initialize other components and variables as needed.
    }

    private void Update()
    {
        float[] inputs = GetInput();

        float hChange = UpdateHorizontal(inputs[0]) * Time.deltaTime;

        // Determine the direction to the player
        Vector3 directionToPlayer = target.position - transform.position;

        if (canFlip)
        {
            if (Mathf.Abs(transform.position.x - startPosition.x) >= walkTime)
            {
                Flip();
            }
            else if (hChange > 0 && directionToPlayer.x < 0 && facingRight)
            {
                Flip();
            }
            else if (hChange < 0 && directionToPlayer.x > 0 && !facingRight)
            {
                Flip();
            }

            if (hChange > 0 && movementBlocked[2] > 0)
            {
                hChange = 0;
                StartCoroutine(FlipCooldown());
            }
            else if (hChange < 0 && movementBlocked[3] > 0)
            {
                hChange = 0;
                StartCoroutine(FlipCooldown());
            }
        }

        float vChange = UpdateVertical() * Time.deltaTime;
        // if (vChange > 0 && movementBlocked[0] > 0) vChange = 0;
        // else if (vChange < 0 && movementBlocked[1] > 0) vChange = 0;

        StartCoroutine(MoveCharacter(new Vector3(hChange, vChange, 0)));

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (!isAttacking)
        {
            if (Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                // The boss is in range to attack the player
                StartCoroutine(Attack());

                // Apply knockback to the player
                ApplyKnockback();
            }
            else
            {
                // Move towards the player
                MoveTowardsTarget();
            }
        }
    }


    void CalculateCameraBounds()
    {
        if (mainCamera.orthographic)
        {
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            float cameraHalfHeight = mainCamera.orthographicSize;

            minX = -cameraHalfWidth;
            maxX = cameraHalfWidth;
            minY = -cameraHalfHeight;
            maxY = cameraHalfHeight;
        }
        else
        {
            // Calculate bounds for perspective camera (not shown in this example)
            // You may need to use additional information like camera field of view, distance, etc.
        }
    }

    private IEnumerator FlipCooldown()
    {
        canFlip = false;
        yield return new WaitForSeconds(flipCooldown);
        canFlip = true;
    }

   private void MoveTowardsTarget()
    {

    }

    private void ApplyKnockback()
    {
        // Calculate the knockback direction away from the boss
        Vector3 knockbackDirection = (target.position - transform.position).normalized;

        // Apply force to the player
        target.GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack"); // Trigger the boss's attack animation

        // Implement code to deal damage to the target here.
        // You may use a Coroutine for this and add damage logic.

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void Die()
    {
        // Implement code for boss's death, like playing a death animation, spawning loot, etc.
        // You might want to destroy or deactivate the boss object after a death animation.
    }

    public override float[] GetInput()
    {
        if (facingRight)
            return new float[] { 1, 0 }; 
        else
            return new float[] { -1, 0 }; 
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        startPosition = transform.position; // Update the starting position
    }
}
