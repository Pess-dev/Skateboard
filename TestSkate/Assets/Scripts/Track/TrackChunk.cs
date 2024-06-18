using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class TrackChunk : MonoBehaviour
{
    public float ChunkLength = 1f;
    Track _track;
    SplineFollower splineFollower;
    bool flag = true;
    void Start()
    {
        _track = Track.Instance;
        splineFollower = GetComponent<SplineFollower>();
        if (splineFollower == null)
        {
            splineFollower = gameObject.AddComponent<SplineFollower>();
        }
        
        splineFollower.spline = _track.GetSplineComputer();
        splineFollower.onBeginningReached += OnEnd;
        splineFollower.updateMethod = SplineFollower.UpdateMethod.LateUpdate;
        splineFollower.SetPercent(100d);
    }

    void Update()
    {
        if (flag)
        {splineFollower.SetPercent(100d);
        flag = false;}
        splineFollower.followSpeed = -_track.trackForwardVelocity;    
    }

    void OnEnd(double percent)
    {
        Destroy(gameObject);
    }
}
