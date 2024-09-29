using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using Valve.VR;

public class Skate : MonoBehaviour
{
    ///singleton
    public static Skate Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    ///

    [SerializeField]
    private float maxRigthAngle = 30;
    [SerializeField]
    private float maxForwardAngle = 30;
    [SerializeField]
    private float minRightAngle = 5;
    [SerializeField]
    private float minForwardAngle = 5;
    
    private float calibratedHeight = 1;
    
    [SerializeField]
    private float maxDeltaHeight = 0.3f;
    [SerializeField]
    private float minDeltaHeight = 0.1f;

    [SerializeField]
    private Transform board;

    [SerializeField]
    private Transform head;

    [SerializeField]
    private float headOverBoardRadius = 0.5f;

    [SerializeField]
    public Vector3 localBoardUp;
    [SerializeField]
    public Vector3 localBoardRight;
    [SerializeField]
    public Vector3 localBoardForward;

    public Vector3 localMoveDirection {get; private set;}
    public float pitch {get; private set;}

    void Start()
    { 
        // Dictionary<int, SteamVR_TrackedObject.EIndex> pairs = new Dictionary<int, SteamVR_TrackedObject.EIndex>(){
        //     {0, SteamVR_TrackedObject.EIndex.None},
        //     {1, SteamVR_TrackedObject.EIndex.Device1},
        //     {2, SteamVR_TrackedObject.EIndex.Device2},
        //     {3, SteamVR_TrackedObject.EIndex.Device3},
        //     {4, SteamVR_TrackedObject.EIndex.Device4},
        //     {5, SteamVR_TrackedObject.EIndex.Device5},
        //     {6, SteamVR_TrackedObject.EIndex.Device6},
        //     {7, SteamVR_TrackedObject.EIndex.Device7},
        //     {8, SteamVR_TrackedObject.EIndex.Device8},
        //     {9, SteamVR_TrackedObject.EIndex.Device9},
        //     {10, SteamVR_TrackedObject.EIndex.Device10},
        //     {11, SteamVR_TrackedObject.EIndex.Device11},
        //     {12, SteamVR_TrackedObject.EIndex.Device12},
        //     {13, SteamVR_TrackedObject.EIndex.Device13},
        //     {14, SteamVR_TrackedObject.EIndex.Device14},
        //     {15, SteamVR_TrackedObject.EIndex.Device15},
        //     {16, SteamVR_TrackedObject.EIndex.Device16}
        // };
        // for (int i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++){
        //     var id = new System.Text.StringBuilder(64);
        //     ETrackedPropertyError error = default;
        //     OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_RenderModelName_String, id, 64, ref error);
        //     Debug.Log(id);
        //     if (id.ToString().Contains("vr_tracker")){
        //         GetComponent<SteamVR_TrackedObject>().index = pairs[i];
        //         break;
        //     }
        // }
    }

    void Update()
    {
        UpdateValues();
    }

    ///<summary>
    /// Update the values of the board for control
    ///</summary>
    void UpdateValues(){
        Vector3 boardForward =  board.TransformDirection(localBoardForward);
        Vector3 boardRight =  board.TransformDirection(localBoardRight);
        Vector3 boardUp =  board.TransformDirection(localBoardUp);

        Vector3 planedRight = Vector3.ProjectOnPlane(boardRight, Vector3.up).normalized;
        Vector3 planedForward = Vector3.ProjectOnPlane(boardForward, Vector3.up).normalized;

        float angleForward = -Vector3.SignedAngle(planedRight, board.right, planedForward);
        
        float angleRight = Vector3.SignedAngle(planedForward,board.forward, planedRight)-90;
        
        
        // Debug.DrawLine(board.position, board.position + planedForward, Color.red);
        //Debug.DrawLine(board.position, board.position + planedRight, Color.blue);
        // Debug.DrawLine(board.position, board.position + Vector3.up, Color.blue);

        float headHeight = head.position.y - board.position.y;

        if (Mathf.Abs(angleForward)<minForwardAngle)
            angleForward=0; 
        if (Mathf.Abs(angleRight)<minRightAngle)
            angleRight=0;
        
        float forwardValue = Mathf.Clamp(Mathf.Abs(headHeight-calibratedHeight)-minDeltaHeight, 0, maxDeltaHeight-minDeltaHeight)/(maxDeltaHeight-minDeltaHeight);
        if (!isHeadOverBoard())
            forwardValue = 0;
        
        float rightValue =  Mathf.Clamp(angleForward,-maxForwardAngle,maxForwardAngle)/maxForwardAngle;

        localMoveDirection = forwardValue*Vector3.forward + Vector3.right*rightValue;
        
        pitch = Mathf.Clamp(angleRight, -maxRigthAngle, maxRigthAngle)/maxRigthAngle;
        //print("Pitch "+pitch+" "+angleRight);

        //        print(localMoveDirection + " " + angleForward + " "+angleRight);
        // float speed = Mathf.Clamp(maxRigthAngle-Mathf.Abs(angleRight), 0, maxRigthAngle) / maxRigthAngle*movementSpeed;
        // Debug.Log(angleForward + " "+angleRight+" "+boardUp);
        // transform.position += (boardForward - Vector3.up*boardForward.y) * speed * Time.deltaTime;

        // float rotSpeed =  Math.Clamp(angleForward,-maxForwardAngle,maxForwardAngle)/maxForwardAngle*rotationSpeed * speed/movementSpeed;

        // transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
    }

    private bool isHeadOverBoard(){
        return Vector3.ProjectOnPlane(head.position - transform.position, Vector3.up).magnitude <= headOverBoardRadius;
    }

    public void CalibrateHeight(){
        calibratedHeight = head.position.y;
    }

    void OnDrawGizmos()
    {
        if (board == null)
        {
            return;
        }
        // Gizmos.color = Color.green;
        // Gizmos.DrawLine(board.position, board.position + board.TransformDirection(localBoardUp));
        // Gizmos.color = Color.blue;
        // Gizmos.DrawLine(board.position, board.position + board.TransformDirection(localBoardRight));
        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(board.position, board.position + board.TransformDirection(localBoardForward));
    }


    /// <summary>
    /// Get the head of the board
    /// </summary>
    /// <returns>Transform of player head (camera)</returns>
    public Transform GetHead(){
        return head;
    }
}
