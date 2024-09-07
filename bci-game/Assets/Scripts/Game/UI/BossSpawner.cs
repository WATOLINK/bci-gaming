using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossHealth;
    [SerializeField] private Transform player; 
    [SerializeField] private float spawnTriggerX;
    private bool bossSpawned = false;
    [SerializeField] private GameObject bossfight;

    private void Awake()
    {
        player = GetComponent<Transform>();
    }

    private void Update()
    {
        if (!bossSpawned && player.position.x >= spawnTriggerX)
        {
            StartCoroutine(FightStart(1f));
            ActivateBoss();
        }
    }

    private IEnumerator FightStart(float duration)
    {
        bossfight.GetComponent<Animator>().SetTrigger("Fight");
        yield return new WaitForSeconds(duration);
    }

    // Activates the boss
    private void ActivateBoss()
    {
        boss.SetActive(true);
        bossHealth.SetActive(true);
        bossSpawned = true;
    }
}
