using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    /// <summary>
    /// Virtual method that should be overriden by logic when the obstacle collides with the player
    /// </summary>
    virtual public void Collided(){
        
    }
}
