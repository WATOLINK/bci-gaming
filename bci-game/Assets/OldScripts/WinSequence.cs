using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSequence : MonoBehaviour
{

    public void NextLevel()
    {
        //since there is currently only 1 level, return to main menu
        SceneManager.LoadScene("Menu");
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Redo()
    {
        //implement once game scene is finalized
        Debug.Log("Replay level");
    }

    public void Info()
    {
        //a possible extra thing we could implement in the future, shows stats for the level (ie. enemies killed, damage taken)
        Debug.Log("Show stats");
    }
}
