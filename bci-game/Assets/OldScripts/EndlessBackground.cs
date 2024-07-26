using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessBackground : MonoBehaviour
{
    Camera mainCamera;

    private void Awake()
    {
        //find camera (the mainCamera shoudl have a MainCamera tag)
        if (mainCamera == null)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("MainCamera");
            mainCamera = objs[0].GetComponent<Camera>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        //moving to the right
        if (Mathf.Abs(mainCamera.transform.position.x - gameObject.transform.GetChild(1).gameObject.transform.position.x) <
             Mathf.Abs(mainCamera.transform.position.x - transform.position.x))
        {

            transform.position = gameObject.transform.GetChild(1).gameObject.transform.position;

        }
        //moving to the left
        else if (Mathf.Abs(mainCamera.transform.position.x - gameObject.transform.GetChild(0).gameObject.transform.position.x) <
           Mathf.Abs(mainCamera.transform.position.x - transform.position.x))
        {

            transform.position = gameObject.transform.GetChild(0).gameObject.transform.position;

        }

    }
}

