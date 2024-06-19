using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGameButton : MonoBehaviour
{
    public void StopGame(){
        Game.Instance.EndGame();
    }
}
