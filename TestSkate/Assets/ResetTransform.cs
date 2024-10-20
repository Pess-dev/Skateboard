using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTransform : MonoBehaviour
{
    public Transform resetTransform;
    public Transform referenceTransform;


    public void Reset(){
        transform.position = resetTransform.position;
        transform.rotation = Quaternion.Euler(0, -referenceTransform.rotation.eulerAngles.y,0); 
    }
}
