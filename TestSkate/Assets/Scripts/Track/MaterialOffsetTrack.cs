using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class MaterialOffsetTrack : MonoBehaviour
{
    [SerializeField]
    private float scale=1;
    SplineMesh splineMesh;

    void Start()
    {
        splineMesh = GetComponent<SplineMesh>();
    
    }
    
    void Update()
    {
        float newValue = splineMesh.uvOffset.y +Track.Instance.trackForwardVelocity*Time.deltaTime * scale;
        newValue -= Mathf.Floor(newValue);
        splineMesh.uvOffset = new Vector2(splineMesh.uvOffset.x, newValue );
    }
}
