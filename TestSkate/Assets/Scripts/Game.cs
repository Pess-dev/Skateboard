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
    
    public float timeLeft {get; private set;} = 0;

    public float score {get; private set;} = 0;

    public float deltaScore {get; private set;} = 0;
    
    public float style {get; private set;} = 0f;
    
    public float distanceLeft {get; private set;} = 0f;

    public float hp {get; private set;} = -1;



    public UnityEvent onScoreChanged = new UnityEvent();
    public UnityEvent onStyleChanged = new UnityEvent();

    public bool isPlaying {get;private set;} = false;
    public UnityEvent onGameStarted = new UnityEvent();
    public UnityEvent onGameEnded = new UnityEvent();

    void Start()
    {
        EndGame();
    }
    
    void Update()
    {
        if (!isPlaying)
            return;
        UpdateGame(Time.deltaTime);
    }

    ///<summary>
    /// Updating of game values
    ///</summary>
    ///<param name="deltaTime">Time since last update</param>
    void UpdateGame(float deltaTime){
        if (timeLeft > 0 && !float.IsInfinity(timeLeft))
            timeLeft -= deltaTime;

        if (distanceLeft > 0 && !float.IsInfinity(distanceLeft))
            distanceLeft -= deltaTime * Track.Instance.trackForwardVelocity; 

        onStyleChanged.Invoke();
        knockDownStyle(deltaTime*styleDecreasingSpeed);

        if (timeLeft<=0){
            EndGame();
        }
        if(distanceLeft<=0){
            EndGame();
        }
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

    ///<summary>
    /// Adds hp by value
    ///</summary>
    ///<param name="value">Value to add</param>
    public void AddHP(int value){
        if (hp == -1) return;
        hp += value;
        hp = Mathf.Clamp(hp,0,100);
        if (hp == 0){
            EndGame();
        }
    }

    public void StartGame(TrackData trackData){
        score = 0;
        style = 0;
        onScoreChanged.Invoke();
        onStyleChanged.Invoke();
        timeLeft = trackData.trackTime;
        distanceLeft = trackData.trackLength;
        hp = trackData.hp;
        isPlaying = true;
        onGameStarted.Invoke();
    }

    public void EndGame(){
        isPlaying = false;
        onGameEnded.Invoke();
    }
}
