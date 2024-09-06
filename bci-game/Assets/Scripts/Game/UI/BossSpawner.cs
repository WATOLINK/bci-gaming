using UnityEngine;
using UnityEngine.UI;
using System;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossHealth;
    [SerializeField] private Transform player; 
    [SerializeField] private float spawnTriggerX;
    private bool bossSpawned = false;

    private void Awake()
    {
        player = GetComponent<Transform>();
    }

    private void Update()
    {
        if (!bossSpawned && player.position.x >= spawnTriggerX)
        {
            ActivateBoss();
        }
    }

    // Activates the boss
    private void ActivateBoss()
    {
        boss.SetActive(true);
        bossHealth.SetActive(true);
        bossSpawned = true;
    }
}
