using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverScreen;
    //public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOverScreen.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Will be called when health bar is depleted
    /* public void gameOver()
    {
        gameOverScreen.SetActive(true);
    } */

    // Restarts level
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Back to menu scene
    public void mainMenu() 
    {
        SceneManager.LoadScene("Menu");
    }

    // Quit game
    public void quit()
    {
        Application.Quit();
    }
}
