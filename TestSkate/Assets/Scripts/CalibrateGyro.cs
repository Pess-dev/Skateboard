using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateGyro : MonoBehaviour
{
    [SerializeField]
    Quaternion startRotation;
    public void Calibrate()
    {
        transform.rotation = startRotation;
    }
}
