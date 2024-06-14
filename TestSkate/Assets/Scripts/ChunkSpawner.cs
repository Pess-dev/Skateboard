using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    private Track _track;

    [SerializeField]
    private Transform SpawnPoint;

    private float lastChunkLength = 0;

    private float traveled = 0;

    void Start(){
        _track = Track.Instance;
    }

    void Update(){
        traveled += _track.trackVelocity.magnitude*Time.deltaTime;

        if (traveled > lastChunkLength){
            TrackData.Chunk chunk = _track.trackData.GetChunk();
            Instantiate(chunk.ChunkPrefab, SpawnPoint.position + _track.transform.forward * (lastChunkLength-traveled), _track.transform.rotation, _track.transform); 
            
            //print(lastChunkLength);
            lastChunkLength = chunk.lenght;
            traveled = 0;
        }
    }
}
