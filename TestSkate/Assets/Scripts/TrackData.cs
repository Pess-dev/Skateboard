using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "TrackData", menuName = "Track", order = 1)]
public class TrackData : ScriptableObject
{
    public class Chunk{
        public Object ChunkPrefab;
        public float lenght;
        public Chunk(Object _ChunkPrefab, float _lenght){
            ChunkPrefab = _ChunkPrefab;
            lenght = _lenght;
        }
    }

    virtual public Chunk GetChunk(){
        return null;
    }
}
