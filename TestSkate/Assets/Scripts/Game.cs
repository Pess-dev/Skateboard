using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        timer += Time.deltaTime;
        style -= Time.deltaTime;
        onStyleChanged.Invoke();
        if (style < 0) style = 0;
    }

    public void AddScore(float amount){
        score += amount * style<1?1:style;
        deltaScore = amount;
        onScoreChanged.Invoke();
        AddStyle(amount*styleMultiplier);
    }
    public void AddStyle(float value){
        style += value;
        if (style > maxStyle) style = maxStyle;
        onStyleChanged.Invoke();
    }

    public void knockDownStyle(float value){
        AddStyle(-value);
    }
}
