using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.Mathematics;
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
    
    [SerializeField]
    private float forwardTrackDistance = 20f;
    [SerializeField]
    private float backwardsTrackDistance = 5f;
    private float forwardTrackDistancePercent = 0f;
    private float backwardsTrackDistancePercent = 0f;
    private float currentDistance = 0f;
    private float trackMeshLength = 0f;
    private Vector3 startTrackOffset = Vector3.zero;

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

    void Start()
    {
        _skate = Skate.Instance;
        SetTrackSpline();
    }

    void Update(){
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
        
        UpdateTrackSpline();
    }

    private void UpdateTrackSpline(){
        float clipFrom = currentDistance-backwardsTrackDistancePercent;
        float clipTo = currentDistance+forwardTrackDistancePercent; 
        
        if (clipFrom < 0) clipFrom = 1 + clipFrom;
        if (clipTo > 1) clipTo = clipTo - 1;

        //trackMesh.clipFrom = clipFrom;
        //trackMesh.clipTo = clipTo;

        SplineSample sample = trackMesh.spline.Evaluate(currentDistance/trackMeshLength);
        
        
        Vector3 localSplinePosition = sample.position;
        

        //rotateAround to sample.forward trackMesh.transform.position
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.Reflect(-sample.forward, transform.forward), Vector3.Cross(sample.forward, sample.right));

        //Quaternion targetRotation = Quaternion.LookRotation(Vector3.Reflect(-sample.forward, transform.forward), Vector3.Reflect(-sample.up, transform.up));
        
        print( sample.forward +" "+sample.up);

        //Quaternion targetRotation = Quaternion.LookRotation(Vector3.Reflect(-sample.forward, transform.forward), sample.up)
        //Quaternion targetRotation = Quaternion.FromToRotation(sample.forward, transform.forward);//* Quaternion.FromToRotation(sample.up, transform.up);
        
        trackMesh.transform.rotation = targetRotation;
        trackMesh.transform.position = transform.position - targetRotation*localSplinePosition;

        //trackMesh.transform.RotateAround(position, Vector3.SignedAngle(trackMesh.transform.forward, targetRotation*Vector3.forward, Vector3.up), Vector3.up);
    }

    private void SetTrackSpline(){
        trackMesh.spline = trackData.trackSpline;
        trackMesh.loopSamples = true;
        trackMeshLength = trackMesh.spline.CalculateLength();
        forwardTrackDistancePercent = forwardTrackDistance/trackMeshLength;
        backwardsTrackDistancePercent = backwardsTrackDistance/trackMeshLength;
        currentDistance = trackData.startTrackDistance;
        startTrackOffset = trackMesh.transform.position;
    }

    public void addForwardVelocity(float value){
        trackForwardVelocity += value;
    }

    public Vector3 GetVelocityOnPoint(Vector3 point){
        return Vector3.zero;
    }
     void OnDrawGizmos()
    {
        if (trackMeshLength == 0 || trackMesh.spline == null)
            return;
        
        SplineSample sample = trackMesh.spline.Evaluate(currentDistance/trackMeshLength);

        Vector3 point = transform.position + Vector3.up;
        Quaternion targetRotation = Quaternion.LookRotation(sample.forward, sample.up);
       // Quaternion targetRotation = Quaternion.FromToRotation(sample.forward, transform.forward);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(point, point + targetRotation*sample.up);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(point, point + targetRotation*sample.right);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(point, point + targetRotation*sample.forward);
    }
}
