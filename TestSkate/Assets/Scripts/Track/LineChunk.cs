using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineChunk : TrackChunk
{
    public int count = 10;
    public float offset = 1f;
    [HideInInspector]
    public float traveled = 0f;
    [HideInInspector]
    public int lineCount = 3;
    public int spawnCount = 1;
    public float sizeX = 3f;

    [SerializeField]
    Transform spawnPoint;

    private bool spawnedNext = false;
    
    [System.Serializable]
    public class Line{
        public Object prefab;
        public float weight;
    } 

    [SerializeField]
    List<Line> lines = new List<Line>();

    private Dictionary<int, Object> choosenLines = new Dictionary<int, Object>(){};

    new void Start(){
        if (count<=0)
        {
            Destroy(gameObject);
            return;
        }
        foreach (Transform child in spawnPoint){
            Destroy(child.gameObject);
        }

        base.Start();
        if (choosenLines.Count == 0){
            lines.Sort((a, b) => b.weight.CompareTo(a.weight));
            ChooseRandomLines();
        }
        SpawnLines();
    }


    /// <summary>
    /// Chooses a random line from the list using weights
    /// </summary>
    void ChooseRandomLines(){
        List<int> freeLines = new List<int>(){};
        List<int> freeObects = new List<int>(){};

        for (int i = 0; i < lineCount; i++){freeLines.Add(i);}
        for (int i = 0; i < lines.Count; i++){freeObects.Add(i);}

        for (int i = 0; i < spawnCount; i++){
            float totalWeight = 0;
            foreach (int fo in freeObects)
            {
                totalWeight += lines[freeObects[fo]].weight;
            }
            float random = Random.Range(0, totalWeight);
            float current = 0;
            int index = 0;
            foreach (int fo in freeObects)
            {
                current += lines[freeObects[fo]].weight;
                if (random < current)
                {
                    index = fo;
                    break;
                }
            }

            int line = Random.Range(0, freeLines.Count);
            choosenLines.Add(line, lines[freeObects[index]].prefab);
            freeObects.RemoveAt(index);
            freeLines.RemoveAt(line);
        }    
    }


    /// <summary>
    /// Spawns choosen object on lines
    /// </summary>
    void SpawnLines(){
        print(choosenLines.Count);
        count--;
        for (int i = 0; i < count; i++){
            if (choosenLines.ContainsKey(i)){
                Instantiate(choosenLines[i], spawnPoint.position+new Vector3(((float)i-(count/2f))*sizeX, 0f, 0f), Quaternion.identity, spawnPoint);
            }
        }
    }



    // new void Update(){   
    //     base.Update();
    //     traveled += Track.Instance.trackForwardVelocity*Time.deltaTime;
    //     if (traveled > offset && !spawnedNext){
    //         ChunkSpawner.Instance.SpawnSubChunk(gameObject);
    //         spawnedNext = true;
    //     }
    // }
}
