using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    Camera mainCamera;
    float tolerance = 5;


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
        if (Mathf.Abs(mainCamera.transform.position.x - gameObject.transform.GetChild(1).gameObject.transform.position.x) < 
             Mathf.Abs(mainCamera.transform.position.x - transform.position.x) ) {

            transform.position = gameObject.transform.GetChild(1).gameObject.transform.position;

        } else if (Mathf.Abs(mainCamera.transform.position.x - gameObject.transform.GetChild(0).gameObject.transform.position.x) <
             Mathf.Abs(mainCamera.transform.position.x - transform.position.x) ) {

            transform.position = gameObject.transform.GetChild(0).gameObject.transform.position;

        }

    }
}
