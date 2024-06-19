using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LastScoreText : MonoBehaviour
{
     private Rigidbody _rb;

    [SerializeField]
    private string prefix = "Your score is: ";
    [SerializeField]
    private string postfix = "!";
    
    [SerializeField]
    private TextMeshPro _text;

    void Start(){
        _rb = GetComponent<Rigidbody>();
        Game.Instance.onGameEnded.AddListener(UpdateValue);
    }

    /// <summary>
    /// Updates score text
    /// </summary>
    void UpdateValue(){
        if (Game.Instance.score > 0)
        _text.text = prefix + ((int)Game.Instance.score).ToString()+ postfix;
        else _text.text = "";
    }
}