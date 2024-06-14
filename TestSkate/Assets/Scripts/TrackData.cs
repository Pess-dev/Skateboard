using System.Collections;
using System.Collections.Generic;
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

    virtual public Chunk GetChunk(){
        return null;
    }
}
