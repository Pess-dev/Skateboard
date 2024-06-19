using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

//[CreateAssetMenu(fileName = "TrackData", menuName = "Track", order = 1)]
public class TrackData : ScriptableObject
{
    public float trackTime = 120f;

    public float trackLength = 1000f;

    public float hp = 3;
    /// <summary>
    /// Get the chunk of the track
    /// </summary>
    /// <returns>Chunk object</returns>
    virtual public Object GetChunk(){
        return null;
    }
}
