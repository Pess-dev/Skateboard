using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : Obstacle
{
    public float cost = 10;

    public override void Collided()
    {
        Game.Instance.AddScore(cost);
        Destroy(gameObject);
    }
}
