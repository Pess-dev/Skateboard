using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collector : MonoBehaviour
{
    Skate _skate;
    [SerializeField]
    private Transform target;

    void Start(){
        _skate = Skate.Instance;
    }
    
    public void Update(){
        transform.position = target.position;
    }
    
    void OnTriggerEnter(Collider other){
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if (obstacle == null || other.isTrigger)return;
        obstacle.Collided();
    }
}
