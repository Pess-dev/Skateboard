using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomTrack", menuName = "TrackData", order = 1)]
public class RandomTrack : TrackData
{
    int currentCount = 0;
    Object lastChunk = null;

    [System.Serializable]
    public class ChunkData{
        public Object chunkPrefab;
        public float weight = 1;//can be higher than 1
        public int count = 1;
    }

    [SerializeField]
    protected List<ChunkData> chunks = new List<ChunkData>();

    /// <summary>
    /// Get the random chunk prefab of the track
    /// </summary>
    /// <returns>Chunk prefab object</returns>
    public override Object GetChunk()
    {
        if (currentCount<=0||lastChunk==null){
            return GetRandomChunk();
        }
        if (currentCount > 0){
            currentCount--;
            return lastChunk;
        }
        return null;
    }

    private Object GetRandomChunk(){
        float totalWeight = 0;
        foreach (ChunkData c in chunks)
        {
            totalWeight += c.weight;
        }
        float random = Random.Range(0, totalWeight);
        float current = 0;
        foreach (ChunkData c in chunks)
        {
            current += c.weight;
            if (random < current)
            {
                lastChunk = c.chunkPrefab;
                currentCount = c.count-1;
                return c.chunkPrefab;
            }
        }
        return null;
    }
}
