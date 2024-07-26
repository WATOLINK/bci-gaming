using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    void Awake()
    {
        //find all bg music gameobjects present in the scene
        GameObject[] bgSounds = GameObject.FindGameObjectsWithTag("music");

        if (bgSounds.Length >= 1) //destroy current object if there is more than one gameObject playing background music.
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); //so the bg music gaemobject can exist across multiple scenes
        }
    }
}
