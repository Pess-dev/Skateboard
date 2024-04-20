using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float maxRigthAngle = 30;
    [SerializeField]
    private float maxForwardAngle = 30;
    [SerializeField]
    private float movementSpeed = 10;
    [SerializeField]
    private float rotationSpeed = 10;

    [SerializeField]
    private Transform board;

    [SerializeField]
    private Vector3 localBoardUp;
    [SerializeField]
    private Vector3 localBoardRight;
    [SerializeField]
    private Vector3 localBoardForward;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 boardForward =  board.TransformDirection(localBoardForward);
        Vector3 boardRight =  board.TransformDirection(localBoardRight);
        Vector3 boardUp =  board.TransformDirection(localBoardUp);

        Vector3 planedRight = Vector3.ProjectOnPlane(boardRight, Vector3.up).normalized;
        Vector3 planedForward = Vector3.ProjectOnPlane(boardForward, Vector3.up).normalized;

        float angleForward = -Vector3.SignedAngle(planedRight, board.right, planedForward);
        float angleRight = Vector3.SignedAngle(planedForward,board.forward, planedRight)-90;
        Debug.DrawLine(board.position, board.position + planedForward, Color.red);
        Debug.DrawLine(board.position, board.position + planedRight, Color.blue);
        Debug.DrawLine(board.position, board.position + Vector3.up, Color.blue);

        float speed = Math.Clamp(maxRigthAngle-Math.Abs(angleRight), 0, maxRigthAngle) / maxRigthAngle*movementSpeed;
        Debug.Log(angleForward + " "+angleRight+" "+boardUp);
        transform.position += (boardForward - Vector3.up*boardForward.y) * speed * Time.deltaTime;

        float rotSpeed =  Math.Clamp(angleForward,-maxForwardAngle,maxForwardAngle)/maxForwardAngle*rotationSpeed * speed/movementSpeed;

        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if (board == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(board.position, board.position + board.TransformDirection(localBoardUp));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(board.position, board.position + board.TransformDirection(localBoardRight));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(board.position, board.position + board.TransformDirection(localBoardForward));
    }
}
