using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

//[CreateAssetMenu(fileName = "TrackData", menuName = "Track", order = 1)]
public class TrackData : ScriptableObject
{
    /// <summary>
    /// Get the chunk of the track
    /// </summary>
    /// <returns>Chunk object</returns>
    virtual public Object GetChunk(){
        return null;
    }
}
