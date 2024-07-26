using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFruit : MonoBehaviour
{
    [SerializeField]
    public Sprite[] spriteList;
    [SerializeField]
    public GameObject[] platforms;
    public float delayBetweenFruits = 4.0f;

    private float lastFruitSpawnTime;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteList = Resources.LoadAll<Sprite>("Fruits");
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        lastFruitSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        
        foreach (var fruit in fruits)
        {
            if (fruit.GetComponent<HealAnim>() == null)
                fruit.AddComponent<HealAnim>();
        }

        if (fruits.Length < 3 && Time.time - lastFruitSpawnTime >= delayBetweenFruits)
        {
            ReplaceFruits();
            lastFruitSpawnTime = Time.time;
        }
    }

    void ReplaceFruits()
    {
       foreach (var p in platforms)
        {
            if (!IsFruitPresent(p))
            {
                GameObject gameObject = new GameObject();
                Vector3 fruitPosition = p.transform.position;
                fruitPosition.y += 1.0f;
                gameObject.transform.position = fruitPosition;
                gameObject.name = "Fruit X";
                gameObject.tag = "Fruit";
                var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                var boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                var rigidBody = gameObject.AddComponent<Rigidbody>();
                rigidBody.isKinematic = true;
                var sprite = spriteList[Random.Range(0, spriteList.Length)];
                spriteRenderer.sprite = sprite;

            }
        }
    }

    bool IsFruitPresent(GameObject platform)
    {
        Collider[] colliders = Physics.OverlapSphere(platform.transform.position, 1.1f);
        bool hasFruit = false;
        bool hasPlayer = false;
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Fruit"))
            {
                hasFruit = true;
            }
            if (collider.gameObject.CompareTag("Player"))
            {
                hasPlayer = true;
            }
        }

        return hasFruit || hasPlayer;
    }
}
