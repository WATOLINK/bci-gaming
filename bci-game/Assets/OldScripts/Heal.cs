using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public playerHealth pHealth;
    public float heal;
    private GameObject consumeObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (OnPlayerCollision())
        {
            HealPlayer(consumeObject);
        }
    }

    public bool OnPlayerCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Fruit"))
            {
                consumeObject = hitCollider.gameObject;
                return true;
            }
        }
        return false;
    }

    public void HealPlayer (GameObject consumeObject)
    {
        pHealth.health += heal;
        Vector3 pos = consumeObject.transform.position;
        Destroy(consumeObject);
    }
}
