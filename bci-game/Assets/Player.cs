using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : Character
{
    private GameObject camera;

    public float attackRange = 1f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;
    public Transform frontCheck;
    private bool canAttack = true;

    // Player health properties
    public float health;
    public float maxHealth;
    public Image Healthbar;
    public Animator playerAnimator;

    public override float[] GetInput()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        if (hInput != 0)
        {
            animator.SetFloat("X", hInput);
        }

        if (Input.GetMouseButton(0))
        {
            if (canAttack)
            {
                PerformAttack();
            }
        }
       

        return new float[] { hInput, vInput };
    }

    protected override IEnumerator MoveCharacter(Vector3 movement)
    {
        if (camera == null) camera = GameObject.FindWithTag("MainCamera");
        transform.position += movement;
        camera.transform.position += movement;
        yield return null;
    }

    IEnumerator AttackCooldown()
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

            //flip hitbox if it is facing the wrong direction
            float hitboxLoc = frontCheck.position.x - transform.position.x;
            if ((animator.GetFloat("X") > 0.5) ^ (hitboxLoc > 0))
            {//XOR
                frontCheck.position -= new Vector3(hitboxLoc * 2, 0, 0);
            }


            animator.SetTrigger("isMelee");
            Collider[] hitColliders = Physics.OverlapSphere(frontCheck.position, attackRange);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    Enemy enemy = hitCollider.GetComponent<Enemy>();
                    Debug.Log("ENEMY HIT");
                    break; // Exit the loop as soon as a player is found
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        maxHealth = health;
        playerAnimator = GetComponent<Animator>();
    }


    public void decreaseHealth(float n)
    {
        StartCoroutine(HurtAnimations());
        health -= n;
    }

    IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        //set player inactive after death animation plays
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        SceneManager.LoadScene(sceneName); //load death sequence
    }

    IEnumerator HurtAnimations()
    {
        playerAnimator.SetTrigger("isHurt");

        Color original = Healthbar.color;
        Healthbar.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        // Fade out effect
        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            Healthbar.color = Color.Lerp(Color.white, original, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Healthbar.color = original;
        Healthbar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        if (health / maxHealth <= 0 && isAlive)
        {
            isAlive = false;
            playerAnimator.SetTrigger("isDead");
            StartCoroutine(LoadSceneAfterDelay("DeathSeq", 0.6f));
        }
    }
}
