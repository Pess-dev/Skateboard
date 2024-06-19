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
    
    // [SerializeField]
    // private int startBendingPoint = 3;
    [SerializeField]
    private float splineDistanceBetweenPoints = 5;
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
    [SerializeField]
    private int startSplineBendPointIndex = 4; 
    private Vector3 splineTargetDirection = Vector3.forward;
    private Vector3 splineCurentDirection = Vector3.forward;
    private float changeSplineDirectionTimer = 1000f;

    void Start()
    {
        _skate = Skate.Instance;
        SetTrack();
        Game.Instance.onGameStarted.AddListener(SetTrack);
        Game.Instance.onGameEnded.AddListener(EndTrack);
    }

    void Update(){
        UpdateTrackMovement(Time.deltaTime);
        UpdateTrackSplineTargetDirection(Time.deltaTime, !Game.Instance.isPlaying);
        UpdateTrackSpline();
    }

    /// <summary>
    /// Updates the values of the track movement 
    /// </summary>
    /// <param name="deltaTime"> Time since last update </param>
    private void UpdateTrackMovement(float deltaTime){
        float addVelocity = (_skate.localMoveDirection.z*(Game.Instance.isPlaying?forwardAcceleration:0)-friction)*Time.deltaTime;

        if(_skate.localMoveDirection.z==0) 
            addVelocity -= Mathf.Abs(_skate.pitch)*forwardAcceleration*Time.deltaTime;

        trackForwardVelocity = Mathf.Clamp(trackForwardVelocity+addVelocity, 0, maxForwardSpeed);
       
        float rigthVelocity = - (_skate.localMoveDirection.x*maxRightSpeed*(forwardSpeedAffectToRightSpeed?Mathf.Sqrt(_skate.localMoveDirection.z):1));
        
        Vector3 deltaPos = transform.InverseTransformDirection(_skate.transform.position - transform.position);

        if (Vector3.Project(deltaPos, transform.right).magnitude >= maxXDistance && Mathf.Sign(deltaPos.x)!=Mathf.Sign(rigthVelocity)){
            rigthVelocity = 0;
        }

        Vector3 Xvelocity = rigthVelocity*transform.right;
        transform.position += Xvelocity*Time.deltaTime;
    }

    /// <summary>
    /// Updates the track spline target direction
    /// </summary>
    /// <param name="deltaTime"> Time since last update </param>
    /// <param name="forward">If true, the bend will be in the forward direction without reference to speed</param>
    private void UpdateTrackSplineTargetDirection(float deltaTime, bool forward = false){
        if (forward) {
            splineCurentDirection = Vector3.forward;
            return;
            }

        float speed = trackForwardVelocity/maxForwardSpeed;
        changeSplineDirectionTimer += deltaTime * speed;
        if (changeSplineDirectionTimer > changeSplineDirectionCoolDown){
            changeSplineDirectionTimer = 0f;
            splineTargetDirection = GetRandomDirectionWithinAngle(Vector3.forward, maxSplineAngle);
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
    /// Bends the tracks spline via function using splineTargetDirection by changing the position of spline points starting from index startSplineBendPointIndex
    /// <para>WARNING: Mesh does not updates if splineCurentDirection equals splineTargetDirection (issue with SplineMesh)</para>
    /// </summary>
    void UpdateTrackSpline(){
        Vector3 startPosition = trackMesh.spline.GetPoint(startSplineBendPointIndex-1, SplineComputer.Space.Local).position;
        float maxDistance = splineDistanceBetweenPoints*(trackMesh.spline.pointCount-startSplineBendPointIndex);
        for (int i = startSplineBendPointIndex; i < trackMesh.spline.pointCount; i++){
            SplinePoint trackPoint = trackMesh.spline.GetPoint(i, SplineComputer.Space.Local); 
            float distance = splineDistanceBetweenPoints*(i-startSplineBendPointIndex+1);
            trackPoint.position = startPosition-Vector3.Lerp(Vector3.forward, splineCurentDirection, distance/maxDistance).normalized*distance;
            trackPoint.normal = Vector3.up;
            trackMesh.spline.SetPoint(i, trackPoint, SplineComputer.Space.Local);
        }
        trackMesh.RebuildImmediate();
    }


    /// <summary>
    /// Sets the starting game values
    /// </summary>
    private void SetTrack(){
        trackForwardVelocity = 0f;
        splineTargetDirection = Vector3.forward;
        changeSplineDirectionTimer = 1000f;
    }
    
    /// <summary>
    /// Sets the starting game values
    /// </summary>
    private void EndTrack(){
        splineTargetDirection = Vector3.forward;
    }

    /// <summary>
    /// Adds forward velocity to the track
    /// </summary>
    public void addForwardVelocity(float value){
        trackForwardVelocity += value;
    }


    /// <summary>
    /// Returns the spline computer of the track
    /// </summary>
    /// <return>SplineComputer</return>
    public SplineComputer GetSplineComputer(){
        return trackMesh.spline;
    }
}
