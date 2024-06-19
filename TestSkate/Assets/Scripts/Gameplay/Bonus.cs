using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : Obstacle
{
    public float cost = 10;

    /// <summary>
    /// Adds score to player and destroys itself
    /// </summary>
    public override void Collided()
    {
        PlaySound();
        Game.Instance.AddScore(cost);
        Kill();
    }
}
