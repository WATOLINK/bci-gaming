using Entity.Interfaces;

namespace Game.UI
{
    using UnityEngine;

    public class EnemyHealthBar : MonoBehaviour
    {
        private GameObject healthBarMask;
        private float healthBarWidth;

        private GameObject enemyObj;
        private float maxHealth;
        private IDamageable enemyHealth;

        private void Start()
        {
            healthBarMask = transform.GetComponentInChildren<SpriteMask>().gameObject;
            healthBarWidth = transform.GetComponent<Renderer>().bounds.size.x;
            
            enemyObj = transform.parent.gameObject;
            enemyHealth = enemyObj.GetComponent<IDamageable>();
            
            maxHealth = enemyHealth.MaxHealth;
        }

        private void Update()
        {
            // Keep health bar facing same direction
            Vector3 healthBarDirectionScale = transform.localScale;
            healthBarDirectionScale.x = enemyObj.transform.localScale.x > 0 ? 1.0f : -1.0f;
            transform.localScale = healthBarDirectionScale;

            // Update health bar percent
            float healthPercentLost = 1f - enemyHealth.Health / maxHealth;

            Vector3 healthBarPos = transform.position;
            Vector3 maskPos = healthBarMask.transform.position;
            maskPos.x = healthBarPos.x - (healthBarWidth * healthPercentLost);
            healthBarMask.transform.position = maskPos;

        }
    }
}