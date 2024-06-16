using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    // void OnCollisionEnter(Collision collision){
    //     if(collision.gameObject.tag == "Chunk"){
    //         Destroy(collision.gameObject);
    //     }
    // }
    // void Update (){//use own colliderr to overlap 
    //     killCollider.
    // }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Chunk"){
            Destroy(other.gameObject);
        }
    }
}
