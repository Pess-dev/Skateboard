using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateRig : MonoBehaviour
{

    [SerializeField]
    LayerMask floor;

    [SerializeField]
    float distance = 10f;
    [SerializeField]
    float offset = 0.5f;
    
    [SerializeField]
    Transform rig;

    void Start()
    {
        
    }

    void Update()
    {
    }

    public void Calibrate(){
        RaycastHit raycastHit = new RaycastHit();

        if(Physics.Raycast(transform.position+Vector3.up*distance, -Vector3.up, out raycastHit, 100, floor)){
            //print(raycastHit.point);
            rig.position = new Vector3(rig.position.x,rig.position.y+raycastHit.point.y-Skate.Instance.transform.position.y+offset, rig.position.z);
        }

        Vector3 projected = Vector3.ProjectOnPlane(Skate.Instance.transform.rotation*Skate.Instance.localBoardForward, Vector3.up);

        Vector3 skatePosition = Skate.Instance.transform.position;

        rig.rotation *= Quaternion.FromToRotation(projected, Track.Instance.transform.forward);


        //rig.position = new Vector3(-Track.Instance.transform.position.x+skatePosition.x, rig.position.y, -Track.Instance.transform.position.z+skatePosition.z);
    }
}
