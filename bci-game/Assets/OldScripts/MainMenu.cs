using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame ()
    {
        StartCoroutine(LoadGame());
        return;

        // Required to wait for sound to play before loading next scene
        static IEnumerator LoadGame()
        {
            yield return new WaitForSeconds(0.2f); 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
