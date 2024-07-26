using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossTransition : MonoBehaviour
{
    public Animator transition;
    public float time = 4f;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            //This could be automated for all transition scenes.
            /* Once we have the entire game's scenes decided, then we could
            use build settings where the scene we want to transition to would always
            be one index higher than the scene we transition from. Then, we could
            simply pass an integer which indicates the current scene index
            */
            StartCoroutine(Load());
        }
    }

    IEnumerator Load() {
        transition.SetTrigger("BossFightStart");
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Boss");
    }
}
