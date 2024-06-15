using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Collector : MonoBehaviour
{
    Skate _skate;
    private Transform target;

    void Start(){
        _skate = Skate.Instance;
        target = _skate.GetHead();
    }
    
    public void Update(){
        transform.position = target.position;
    }
    
    void OnTriggerEnter(Collider other){
        
        Obstacle obstacle = other.GetComponent<Obstacle>();

        if (obstacle != null) obstacle.Collided();
    }
}
