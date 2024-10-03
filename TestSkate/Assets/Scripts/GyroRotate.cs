using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroRotate : MonoBehaviour
{   
    void Update(){
       // transform.rotation = Quaternion.Euler(TCPListner.MessageToString.GetX(), TCPListner.MessageToString.GetY(),0);
       //print(TCPListner.MessageToString.GetX()+" "+ TCPListner.MessageToString.GetY());
       TCPListner.MessageToString.GetX();
       TCPListner.MessageToString.GetY();
    }
}
