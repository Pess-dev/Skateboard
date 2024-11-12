using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateGyro : MonoBehaviour
{
    [SerializeField]
    Quaternion startRotation;

    Quaternion preCalibrate = Quaternion.identity;
    public void Calibrate()
    {
        preCalibrate = transform.rotation;
        //transform.rotation = startRotation;
    }
}
