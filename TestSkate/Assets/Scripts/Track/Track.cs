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

    /** Перемещение сплайна целиком 
    private void OldUpdateTrackSpline(){
        float clipFrom = currentDistance/trackMeshLength-backwardsTrackDistance01;
        float clipTo = currentDistance/trackMeshLength+forwardTrackDistance01; 
        
        if (clipFrom < 0) clipFrom = 1 + clipFrom;
        if (clipTo > 1) clipTo = clipTo - 1;

        trackMesh.clipFrom = clipFrom;
        trackMesh.clipTo = clipTo;
        print(backwardsTrackDistance01+" "+forwardTrackDistance01+" "+clipFrom + " " + clipTo);

        SplineSample sample = trackMesh.spline.Evaluate(currentDistance/trackMeshLength);
        
        
        Vector3 localSplinePosition = sample.position;
        Vector3 newForward = transform.rotation * Vector3.Reflect(Vector3.Reflect(-sample.forward, Vector3.forward),Vector3.up);
        Vector3 newRight = transform.rotation * sample.right;
        Vector3 newUp = transform.rotation * ReflectVectorAxis(sample.up, Vector3.up);
        

        //rotateAround to sample.forward trackMesh.transform.position
        Quaternion targetRotation = Quaternion.LookRotation(newForward, newUp);

        //Quaternion targetRotation = Quaternion.LookRotation(Vector3.Reflect(-sample.forward, transform.forward), Vector3.Reflect(-sample.up, transform.up));
        
        //print( sample.forward +" "+sample.up);

        //Quaternion targetRotation = Quaternion.LookRotation(Vector3.Reflect(-sample.forward, transform.forward), sample.up)
        //Quaternion targetRotation = Quaternion.FromToRotation(sample.forward, transform.forward);//* Quaternion.FromToRotation(sample.up, transform.up);
        
        trackMesh.transform.rotation = targetRotation;
        trackMesh.transform.position = transform.position - targetRotation*localSplinePosition;

        //trackMesh.transform.RotateAround(position, Vector3.SignedAngle(trackMesh.transform.forward, targetRotation*Vector3.forward, Vector3.up), Vector3.up);
    }
    **/


    /** Изгибание сплайна по заготовленному сплайну
    void UpdateTrackSpline(){
        //print(currentDistance);

        SplinePoint trackPoint = trackMesh.spline.GetPoint(2);
        float distance01 = currentDistance/trackMeshLength;
        distance01 -= Mathf.Floor(distance01);
        SplineSample splinePointLocal = trackData.trackSpline.Evaluate(distance01);
        Quaternion additionalRotation = Quaternion.LookRotation(splinePointLocal.forward, splinePointLocal.up);
        Vector3 firstPointLocalPosition = splinePointLocal.position;
        Vector3 startPosition = trackPoint.position;
        
        for (int i = 3; i < trackMesh.spline.pointCount; i++){
            trackPoint = trackMesh.spline.GetPoint(3);
            distance01 = (currentDistance+splineDistanceBetweenPoints*(i-2))/trackMeshLength;
            distance01 -= Mathf.Floor(distance01);
            splinePointLocal = trackData.trackSpline.Evaluate(distance01);
            
            //Vector3 newForward = transform.rotation * Vector3.Reflect(Vector3.Reflect(-splinePointLocal.forward, Vector3.forward),Vector3.up);
            //Vector3 newRight = transform.rotation * splinePointLocal.right;
            //Vector3 newUp = transform.rotation * ReflectVectorAxis(splinePointLocal.up, Vector3.up);
            
            //rotateAround to sample.forward trackMesh.transform.position
            //Quaternion targetRotation = Quaternion.FromToRotation(splinePointLocal.forward,Vector3.forward)*Quaternion.FromToRotation(splinePointLocal.up,Vector3.up);
            Quaternion targetRotation = Quaternion.FromToRotation(splinePointLocal.forward,Vector3.forward);
            trackPoint.position = targetRotation*(-splinePointLocal.position + firstPointLocalPosition);
            trackPoint.normal = targetRotation*splinePointLocal.up;
            trackMesh.spline.SetPoint(i, trackPoint);
        }
        // string printList = "";
        // for (int i = 0; i < distances.Count; i++){
        //     printList += distances[i]+" ";
        // }
        // print(printList);
        //trackMesh.spline.pointCount;
        //trackMesh.spline.GetPoint();
    }
    **/

    private Vector3 splineTargetDirection = Vector3.forward;
    //Изгибание сплайна по функции
    void UpdateTrackSpline(){
        for (int i = 3; i < trackMesh.spline.pointCount; i++){
            SplinePoint trackPoint = trackMesh.spline.GetPoint(3);
            //trackPoint.position = targetRotation*(-splinePointLocal.position + firstPointLocalPosition);
            //trackPoint.normal = targetRotation*splinePointLocal.up;
            trackMesh.spline.SetPoint(i, trackPoint);
        }
        
    }

    Vector3 ReflectVectorAxis(Vector3 original, Vector3 axis)
    {
        Vector3 normal = axis.normalized;
        return (-original) - Vector3.Project(-original,axis) + Vector3.Project(original,axis);
    }

    private void SetTrackSpline(){
        //trackMesh.spline = trackData.trackSpline;
        //trackMesh.loopSamples = true;
        trackMeshLength = trackMesh.spline.CalculateLength();
        //forwardTrackDistance01 = forwardTrackDistance/trackMeshLength;
        //backwardsTrackDistance01 = backwardsTrackDistance/trackMeshLength;
        //currentDistance = trackData.startTrackDistance;
        //startTrackOffset = trackMesh.transform.position;
    }

    public void addForwardVelocity(float value){
        trackForwardVelocity += value;
    }

    public Vector3 GetVelocityOnPoint(Vector3 point){
        return Vector3.zero;
    }
     void OnDrawGizmos()
    {
    //     if (trackMeshLength == 0 || trackMesh.spline == null)
    //         return;
        
    //     SplineSample sample = trackMesh.spline.Evaluate(currentDistance/trackMeshLength);

    //     Vector3 point = transform.position + Vector3.up;
    //     Quaternion targetRotation = Quaternion.LookRotation(sample.forward, sample.up);
    //    // Quaternion targetRotation = Quaternion.FromToRotation(sample.forward, transform.forward);


    //     Gizmos.color = Color.green;
    //     Gizmos.DrawLine(point, point + targetRotation*sample.up);
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawLine(point, point + targetRotation*sample.right);
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(point, point + targetRotation*sample.forward);
    }
}
