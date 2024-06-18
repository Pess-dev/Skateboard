using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
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

    [SerializeField]
    private SplineMesh trackMesh;

    private Skate _skate;

    private float traveledDistance = 0;
    
    // [SerializeField]
    // private float forwardTrackDistance = 20f;
    // [SerializeField]
    // private float backwardsTrackDistance = 5f;
    // private float forwardTrackDistance01 = 0f;
    // private float backwardsTrackDistance01 = 0f;
    private float currentDistance = 0f;
    
    // [SerializeField]
    // private int startBendingPoint = 3;
    [SerializeField]
    private float splineDistanceBetweenPoints = 5;
    private float trackMeshLength = 0f;
    //private Vector3 startTrackOffset = Vector3.zero;

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

    public float trackForwardVelocity {get; private set;}

    [SerializeField]
    private bool forwardSpeedAffectToRightSpeed = false;


    [SerializeField]
    private float changeSplineDirectionCoolDown = 10f;
    [SerializeField]
    private float maxSplineAngle = 30f; 
    [SerializeField]
    private float splineBendSpeed = 0.1f; 
    private Vector3 splineTargetDirection = Vector3.forward;
    private Vector3 splineCurentDirection = Vector3.forward;
    private float changeSplineDirectionTimer = 0f;

    void Start()
    {
        _skate = Skate.Instance;
        SetTrackSpline();
    }

    void Update(){
        UpdateTrackMovement(Time.deltaTime);
        UpdateTrackSplineTargetDirection(Time.deltaTime);
        UpdateTrackSpline();
    }

    /// <summary>
    /// Updates the values of the track movement 
    /// </summary>
    /// <param name="deltaTime"> Time since last update </param>
    private void UpdateTrackMovement(float deltaTime){
        float addVelocity = (_skate.localMoveDirection.z*forwardAcceleration-friction)*Time.deltaTime;

        if(_skate.localMoveDirection.z==0) 
            addVelocity -= Mathf.Abs(_skate.pitch)*forwardAcceleration*Time.deltaTime;

        trackForwardVelocity = Mathf.Clamp(trackForwardVelocity+addVelocity, 0, maxForwardSpeed);
       
        float rigthVelocity = - (_skate.localMoveDirection.x*maxRightSpeed*(forwardSpeedAffectToRightSpeed?Mathf.Sqrt(_skate.localMoveDirection.z):1));
        
        Vector3 deltaPos = transform.InverseTransformDirection(_skate.transform.position - transform.position);

        ////
        if (Vector3.Project(deltaPos, transform.right).magnitude >= maxXDistance && deltaPos.x>rigthVelocity){
            rigthVelocity = 0;
        }

        Vector3 Xvelocity = rigthVelocity*transform.right;
        transform.position += Xvelocity*Time.deltaTime;

        float deltaPositionDistance = trackForwardVelocity*Time.deltaTime;

        traveledDistance += deltaPositionDistance;
        currentDistance += deltaPositionDistance;
        if (currentDistance > trackMeshLength){
            currentDistance = currentDistance - trackMeshLength;
        }
        
    }

    /// <summary>
    /// Updates the track spline target direction
    /// </summary>
    /// <param name="deltaTime"> Time since last update </param>
    private void UpdateTrackSplineTargetDirection(float deltaTime){
        float speed = trackForwardVelocity/maxForwardSpeed;
        changeSplineDirectionTimer += deltaTime * speed;
        if (changeSplineDirectionTimer > changeSplineDirectionCoolDown){
            changeSplineDirectionTimer = 0f;
            splineTargetDirection = GetRandomDirectionWithinAngle(transform.forward, maxSplineAngle);
        }
        splineCurentDirection = Vector3.Slerp(splineCurentDirection, splineTargetDirection, speed*splineBendSpeed*deltaTime/180f*Mathf.PI);
    }

    /// <summary>
    /// Obtaining a random direction within a given angle from the original vector
    /// </summary>
    /// <param name="baseDirection"> Original vector </param>
    /// <param name="angle"> Angle in degrees </param>
    /// <returns>Vector3 random direction</returns>
    Vector3 GetRandomDirectionWithinAngle(Vector3 baseDirection, float angle)
    {
        float randomAngle = UnityEngine.Random.Range(-angle, angle);
        return Quaternion.AngleAxis(randomAngle, UnityEngine.Random.onUnitSphere) * baseDirection;
    }

    /// <summary>
    /// Bends the tracks spline via function using splineTargetDirection by changing the position of spline points starting from index 3
    /// </summary>
    void UpdateTrackSpline(){
        Vector3 startPosition = trackMesh.spline.GetPoint(2).position;
        float maxDistance = splineDistanceBetweenPoints*(trackMesh.spline.pointCount-3);
        for (int i = 3; i < trackMesh.spline.pointCount; i++){
            SplinePoint trackPoint = trackMesh.spline.GetPoint(3);
            float distance = splineDistanceBetweenPoints*(i-2);
            trackPoint.position = startPosition+Vector3.Lerp(transform.forward,splineCurentDirection,distance/maxDistance).normalized*distance;
            trackPoint.normal = Vector3.up;
            trackMesh.spline.SetPoint(i, trackPoint);
        }
        
    }

    // Vector3 ReflectVectorAxis(Vector3 original, Vector3 axis)
    // {
    //     Vector3 normal = axis.normalized;
    //     return (-original) - Vector3.Project(-original,axis) + Vector3.Project(original,axis);
    // }


    /// <summary>
    /// Sets the track spline
    /// </summary>
    private void SetTrackSpline(){
        //trackMesh.spline = trackData.trackSpline;
        //trackMesh.loopSamples = true;
        trackMeshLength = trackMesh.spline.CalculateLength();
        //forwardTrackDistance01 = forwardTrackDistance/trackMeshLength;
        //backwardsTrackDistance01 = backwardsTrackDistance/trackMeshLength;
        //currentDistance = trackData.startTrackDistance;
        //startTrackOffset = trackMesh.transform.position;
    }

    /// <summary>
    /// Adds forward velocity to the track
    /// </summary>
    public void addForwardVelocity(float value){
        trackForwardVelocity += value;
    }
}
