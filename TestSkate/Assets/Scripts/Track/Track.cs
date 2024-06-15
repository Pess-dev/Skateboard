using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    ///singleton
    public static Track Instance { get; private set; }
    void Awake(){
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public TrackData trackData;

    private Skate _skate;

    [SerializeField]
    private float maxForwardSpeed = 5f;
    [SerializeField]
    private float forwardAcceleration = 5f;
    [SerializeField]
    private float friction = 1f;
    
    [SerializeField]
    private float maxRightSpeed = 1;

    [SerializeField]
    private float maxXDistance = 1.5f;

    public Vector3 trackVelocity {get; private set;}

    [SerializeField]
    private bool forwardSpeedAffectToRightSpeed = false;

    void Start()
    {
        _skate = Skate.Instance;
    }

    void Update(){
        float addVelocity = (_skate.localMoveDirection.z*forwardAcceleration-friction)*Time.deltaTime;

        if(_skate.localMoveDirection.z==0) 
            addVelocity -= Mathf.Abs(_skate.pitch)*forwardAcceleration*Time.deltaTime;

        float forwardVelocity = Mathf.Clamp(Vector3.Project(trackVelocity, transform.forward).magnitude+addVelocity, 0, maxForwardSpeed);
       
        Vector3 velocity = -forwardVelocity*transform.forward 
        - transform.rotation*(_skate.localMoveDirection.x*maxRightSpeed*Vector3.right * (forwardSpeedAffectToRightSpeed?Mathf.Sqrt(_skate.localMoveDirection.z):1))*Time.deltaTime;
        
        Vector3 deltaPos = _skate.transform.position - transform.position;

        if (Vector3.Project(deltaPos, transform.right).magnitude >= maxXDistance && Vector3.Dot(deltaPos,velocity)<0){
            velocity -= Vector3.Project(velocity, transform.right);
        }

        Vector3 Xvelocity = Vector3.Project(velocity, transform.right);
        trackVelocity = velocity - Vector3.Project(velocity, transform.right);
        transform.position += Xvelocity;
        //velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        
        //transform.position += velocity * Time.deltaTime;
    }

    public void addForwardVelocity(float value){
        trackVelocity += transform.forward*value;
    }
}
