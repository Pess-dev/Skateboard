using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackChunk : MonoBehaviour
{
    public float ChunkLength = 1f;
    Track _track;
    void Start()
    {
        _track = Track.Instance;
    }

    void Update()
    {
        transform.position += _track.trackVelocity*Time.deltaTime;        
    }
    public void OnDrawGizmos()
    {
        var r = GetComponent<Renderer>();
        if (r == null)
            return;
        var bounds = r.bounds;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
    }
}
