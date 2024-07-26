namespace Game.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using Entity.Enemies.Bandit;
    using System;
    using static UnityEngine.RuleTile.TilingRuleOutput;

    public class EnemyHealthBar : MonoBehaviour
    {
        private GameObject healthFillerMask;
        private GameObject healthGroup;

        private float maxHealth;
        private Bandit bandit;

        private void Start()
        {
            healthFillerMask = transform.parent.Find("Mask").gameObject;
            bandit = FindObjectOfType<Bandit>();
            maxHealth = bandit.MaxHealth;
            healthGroup = transform.parent.gameObject;
        }

        private void Update()
        {
            float healthScaleX = bandit.Health / maxHealth;
            healthFillerMask.transform.localScale = new Vector3(healthScaleX, 1.0f, 0.0f);
            healthGroup.transform.localScale = new Vector3(bandit.transform.localScale.x > 0 ? 1.0f : -1.0f, 1.0f, 1.0f);
        }
    }
}