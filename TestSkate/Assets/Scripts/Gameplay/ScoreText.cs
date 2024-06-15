using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ScoreText : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField]
    private string prefix = "Score: ";
    
    [SerializeField]
    private TextMeshPro _text;

    [SerializeField]
    private float impulseMultiplier = 10f;

    void Start(){
        _rb = GetComponent<Rigidbody>();
        Game.Instance.onScoreChanged.AddListener(UpdateValue);
        UpdateValue();
    }

    void UpdateValue(){
        _text.text = prefix + ((int)Game.Instance.score).ToString();
        Vector3 randomDirection = Random.insideUnitSphere;
        _rb.AddForce(randomDirection * impulseMultiplier*Game.Instance.deltaScore, ForceMode.Impulse);
    }
}
