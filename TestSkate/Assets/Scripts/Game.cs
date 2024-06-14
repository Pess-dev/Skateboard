using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    //singleton
    public static Game Instance;
    void Awake(){
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    [HideInInspector]
    public int score = 0;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
