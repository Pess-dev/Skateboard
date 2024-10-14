using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroRotate : MonoBehaviour
{   
    void Update(){
        transform.rotation = Quaternion.Euler(TCPListner.eulers);
        //print(TCPListner.eulers);
    }
}
