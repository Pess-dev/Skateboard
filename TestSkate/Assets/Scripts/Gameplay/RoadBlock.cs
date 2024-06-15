using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlock : Obstacle
{
    public float frictionSpeed = 0.5f;
    public float styleCost = 0.5f;

    public override void Collided()
    {
        Game.Instance.knockDownStyle(styleCost);
        Track.Instance.addForwardVelocity(-frictionSpeed);
        Destroy(gameObject);
    }
}
