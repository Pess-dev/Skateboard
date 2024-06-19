using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    private float HeadMoveHeight = 1;

    [SerializeField]
    private Transform board;

    [SerializeField]
    private Transform head;

    [SerializeField]
    private Vector3 localBoardUp;
    [SerializeField]
    private Vector3 localBoardRight;
    [SerializeField]
    private Vector3 localBoardForward;

    public Vector3 localMoveDirection {get; private set;}
    public float pitch {get; private set;}

    void Start()
    {
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
        
        //should be decommented
        //float angleRight = Vector3.SignedAngle(planedForward,board.forward, planedRight)-90;

        float angleRight = Vector3.SignedAngle(planedForward,board.forward, planedRight);
        
        
        // Debug.DrawLine(board.position, board.position + planedForward, Color.red);
        //Debug.DrawLine(board.position, board.position + planedRight, Color.blue);
        // Debug.DrawLine(board.position, board.position + Vector3.up, Color.blue);

        float headHeight = head.position.y -board.position.y;

        if (Mathf.Abs(angleForward)<minForwardAngle)
            angleForward=0; 
        if (Mathf.Abs(angleRight)<minRightAngle)
            angleRight=0;
        
        float forwardValue = Mathf.Clamp(HeadMoveHeight-headHeight, 0, HeadMoveHeight)/HeadMoveHeight;
        
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
