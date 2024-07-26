using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public enum EnemyType{
        Bandit,
        Cowboy,
        Cactus
    }
    private int numEnemies = 0;
    private GameObject banditTemplate;
    private GameObject cowboyTemplate;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn 3 enemies for now
        //enemy spawning locations will be completed after level design

        //Get references to template objects before deactivating
        banditTemplate = GameObject.Find("BanditTemp");
        banditTemplate.SetActive(false);
        cowboyTemplate = GameObject.Find("CowboyTemp");
        cowboyTemplate.SetActive(false);

        SpawnEnemy(EnemyType.Bandit, 5f, 1.1f);
        SpawnEnemy(EnemyType.Bandit, 7f, 1.1f);
        SpawnEnemy(EnemyType.Cowboy, -10f, 1.1f);
        Debug.Log("3 enemies spawned");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy(EnemyType type, float xPos, float yPos){
        //Copy a hidden enemy object to spawn a new enemy
        if (type == EnemyType.Bandit) {
            GameObject newEnemy = Instantiate(banditTemplate, new Vector3(xPos, yPos, 0), Quaternion.identity);
            newEnemy.SetActive(true);
            newEnemy.name = "Bandit" + numEnemies.ToString();

            Character script = newEnemy.GetComponent<Character>();
            script.moveSpeed = 1;
            script.gravity = 8;
        }

        else if (type == EnemyType.Cowboy) {
            GameObject newEnemy = Instantiate(cowboyTemplate, new Vector3(xPos, yPos, 0), Quaternion.identity);
            newEnemy.SetActive(true);
            newEnemy.name = "Cowboy" + numEnemies.ToString();

            Character script = newEnemy.GetComponent<Character>();
            script.moveSpeed = 1;
            script.gravity = 8;
        }

        numEnemies += 1;
    }

    public void SpawnEnemyButton(string s){
        //This method works with the button, enable the button object if needed for testing 
        //to edit the spawn location, find the button object and edit the text box in OnClick
        //syntax: "[enemy type as an integer] [xlocation] [ylocation]"
        string[] info = s.Split(' ');
        EnemyType type = (EnemyType) int.Parse(info[0]);
        float xPos = float.Parse(info[1]);
        float yPos = float.Parse(info[2]);

        SpawnEnemy(type, xPos, yPos);
    }
}
