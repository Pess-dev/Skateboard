using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackChunk : MonoBehaviour
{
    Track _track;
    void Start()
    {
        _track = Track.Instance;
    }

    void Update()
    {
        transform.position += _track.trackVelocity;        
    }
}
