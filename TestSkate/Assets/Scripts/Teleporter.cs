using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    Transform teleportTo;
    public void Teleport(){
        transform.position = teleportTo.position;
    }
}
