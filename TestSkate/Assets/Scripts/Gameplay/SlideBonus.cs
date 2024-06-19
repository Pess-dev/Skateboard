using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBonus : Obstacle
{
    public float costPerLength = 1f;
    
    [SerializeField]
    private float length = 5f;
    [SerializeField]
    private Transform bonusTransform;

    [SerializeField]
    private float neededPitch = 0.5f;
    private float traveledCollided = 0f;

    private float soundTraveledCollided = 100f;

    [SerializeField]
    private float soundDistanceCooldown = 0.5f;

    bool collided = false;

    void Update(){
        if (collided) {
            //print("Sliding "+traveledCollided+" Pitch "+Skate.Instance.pitch+" traveledCollided "+traveledCollided);
            traveledCollided += Time.deltaTime*Track.Instance.trackForwardVelocity;
            soundTraveledCollided += Time.deltaTime*Track.Instance.trackForwardVelocity;
            if (soundTraveledCollided > soundDistanceCooldown) {
                PlaySound();
                soundTraveledCollided = 0f;
            }
            if (Mathf.Abs(Skate.Instance.pitch)>neededPitch) {
                Game.Instance.AddScore(costPerLength*Time.deltaTime);
            }
            else
            {
                Kill();
            }
            if (traveledCollided>length) {
                Kill();
            }
        }
    }

    /// <summary>
    /// Adds score to player and destroys itself
    /// </summary>
    public override void Collided()
    {
        transform.parent = null;
        collided = true;
        anim.SetBool("Collided", true);
    }

}
