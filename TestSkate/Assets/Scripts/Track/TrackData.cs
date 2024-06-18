using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

//[CreateAssetMenu(fileName = "TrackData", menuName = "Track", order = 1)]
public class TrackData : ScriptableObject
{
    public class Chunk{
        public Object ChunkPrefab;
        public float lenght = 1;
        public Chunk(Object _ChunkPrefab){
            ChunkPrefab = _ChunkPrefab;
            TrackChunk tc = ChunkPrefab.GetComponent<TrackChunk>();
            if (tc != null)
                lenght = ChunkPrefab.GetComponent<TrackChunk>().ChunkLength;
        }
    }

    
    public SplineComputer trackSpline = null;
    
    public float startTrackDistance = 5f;

    /// <summary>
    /// Get the chunk of the track
    /// </summary>
    /// <returns>Chunk object</returns>
    virtual public Chunk GetChunk(){
        return null;
    }
}
