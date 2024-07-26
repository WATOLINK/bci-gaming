using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerHealth : Player
{
    public float health;
    public float maxHealth;
    public Image Healthbar;
    public Animator playerAnimator; 

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        maxHealth = health;
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        if (health / maxHealth <= 0 && isAlive)
        {
            isAlive = false;
            playerAnimator.SetTrigger("isDead");
            StartCoroutine(LoadSceneAfterDelay("DeathSeq", 0.6f));
        }
    }

    public void decreaseHealth(float n) {
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
    }
}

