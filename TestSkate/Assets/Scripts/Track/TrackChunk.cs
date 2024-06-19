using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class TrackChunk : MonoBehaviour
{
    public float chunkLength = 1f;
    Track _track;
    SplineFollower splineFollower;
    bool flag = true;
    protected void Start()
    {
        _track = Track.Instance;
        splineFollower = GetComponent<SplineFollower>();
        if (splineFollower == null)
        {
            splineFollower = gameObject.AddComponent<SplineFollower>();
        }
        
        splineFollower.spline = _track.GetSplineComputer();
        splineFollower.onBeginningReached += OnEnd;
        Game.Instance.onGameEnded.AddListener(DestroyChunk);
        splineFollower.updateMethod = SplineFollower.UpdateMethod.LateUpdate;
        splineFollower.SetPercent(100d);
    }

    protected void Update()
    {
        if (flag)
        {splineFollower.SetPercent(100d);
        flag = false;}
        splineFollower.followSpeed = -_track.trackForwardVelocity;    
    }

    void OnEnd(double percent)
    {
        DestroyChunk();
    }
    void DestroyChunk(){
        Destroy(gameObject);
    }
}
