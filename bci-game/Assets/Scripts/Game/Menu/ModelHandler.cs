using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelHandler : MonoBehaviour
{
    public GameObject StartCalibration;
    public GameObject FinishCalibration;

    public void finishCalibration() 
    {
        StartCalibration.SetActive(false);
        FinishCalibration.SetActive(true);
    }
}
