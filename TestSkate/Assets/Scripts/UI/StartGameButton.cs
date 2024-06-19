using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    [SerializeField]
    private TrackData _trackData;
    
    public void StartGame()
    {
        Game.Instance.StartGame(_trackData);
        UI.Instance.GameUI();
    }
}
