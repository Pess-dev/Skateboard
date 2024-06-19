using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

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
    ///

    public float maxStyle = 100f;
    [SerializeField]
    private float styleMultiplier = 1f;
    [SerializeField]
    private float styleDecreasingSpeed = 1f;
    
    public float timer {get; private set;} = 0;

    public float score {get; private set;} = 0;

    public float deltaScore {get; private set;} = 0;
    
    public float style {get; private set;} = 0;

    public float styledScore {get; private set;} = 0;

    public UnityEvent onScoreChanged = new UnityEvent();
    public UnityEvent onStyleChanged = new UnityEvent();

    void Start()
    {
        
    }
    
    void Update()
    {
        UpdateGame(Time.deltaTime);
    }

    ///<summary>
    /// Updating of game values
    ///</summary>
    ///<param name="deltaTime">Time since last update</param>
    void UpdateGame(float deltaTime){
        timer += deltaTime;
        onStyleChanged.Invoke();
        knockDownStyle(deltaTime*styleDecreasingSpeed);
    }

    ///<summary>
    /// Updates the score by amount multiplied by the style
    ///</summary>
    ///<param name="amount">Amount value to add</param>
    public void AddScore(float amount){
        score += amount * style<1?1:style;
        deltaScore = amount;
        onScoreChanged.Invoke();
        AddStyle(amount*styleMultiplier);
    }

    ///<summary>
    /// Adds the style by value
    ///</summary>
    ///<param name="value">Score value to add</param>
    public void AddStyle(float value){
        style += value;
        style = Mathf.Clamp(style,0,maxStyle);
        onStyleChanged.Invoke();
    }

    ///<summary>
    /// Subtracts the style by value
    ///</summary>
    ///<param name="value">Score value to subtract</param>
    public void knockDownStyle(float value){
        AddStyle(-value);
    }
}
