using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomTrack", menuName = "TrackData", order = 1)]
public class RandomTrack : TrackData
{
     [System.Serializable]
    public class ChunkData{
        public Object ChunkPrefab;
        public float length = 1;
        public float weight = 1;//can be higher than 1
    }

    [SerializeField]
    protected List<ChunkData> Chunks = new List<ChunkData>();
    public override Chunk GetChunk()
    {
        float totalWeight = 0;
        foreach (ChunkData c in Chunks)
        {
            totalWeight += c.weight;
        }
        float random = Random.Range(0, totalWeight);
        float current = 0;
        foreach (ChunkData c in Chunks)
        {
            current += c.weight;
            if (random < current)
            {
                return new Chunk(c.ChunkPrefab,c.length);
            }
        }
        return null;
    }
}
