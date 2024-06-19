using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    //singleton
    public static ChunkSpawner Instance;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else{
            Instance = this;
        }
    }


    private Track _track;
    private float lastChunkLength = 0;

    private float traveled = 0;

    private bool randomXZMirroring = true;
    private bool isLastMirrored = false;
    private Object lastChunk = null;

    void Start(){
        _track = Track.Instance;
    }

    void Update(){
        if (!Game.Instance.isPlaying) return;
        SplineComputer computer = _track.GetSplineComputer();
        transform.position = computer.GetPoint(computer.pointCount-1).position;
        traveled += _track.trackForwardVelocity*Time.deltaTime;
        if (traveled > lastChunkLength){
           SpawnChunk();
        }
    }

    /// <summary>
    /// Instantiates a chunk using the track data
    /// </summary>
    private void SpawnChunk(){
        Object chunk = _track.trackData.GetChunk();
        GameObject newChunk = (GameObject)Instantiate(chunk, transform.position, transform.rotation, _track.transform); 
        TrackChunk chunkComponent = newChunk.GetComponent<TrackChunk>();
        lastChunkLength = chunkComponent==null?0f:chunkComponent.chunkLength;
        traveled = 0;

        if (randomXZMirroring && Random.Range(0f, 1f) > 0.5f&&lastChunk!=chunk||isLastMirrored&&lastChunk==chunk){
            foreach (Transform child in newChunk.transform){
                child.transform.localPosition = new Vector3(-child.transform.localPosition.x, child.transform.localPosition.y, -child.transform.localPosition.z);
            }
            isLastMirrored = true;
        }
        else{
            isLastMirrored = false;
        }
        
        lastChunk = chunk;
    }
}
