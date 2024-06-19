using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class TimerText : MonoBehaviour
{
    
    [SerializeField]
    private string prefix = "Time left: ";
    
    [SerializeField]
    private TextMeshPro _text;

    void Update()
    {
        if (Game.Instance.timeLeft > 0 && !float.IsInfinity(Game.Instance.timeLeft)){
            _text.text = prefix + ((int)Game.Instance.timeLeft).ToString();
        }else
        {
            _text.text = "";
        }
    }
}
