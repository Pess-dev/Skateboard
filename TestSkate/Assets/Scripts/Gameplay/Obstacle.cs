using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    protected AudioSource audioSource;
    [SerializeField]
    AudioClip audioClip;
    [SerializeField]
    private float pitchDeviation = 0.1f;
    protected Animator anim;

    private Collider collider;

    virtual public void Start(){
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    /// <summary>
    /// Virtual method that should be overriden by logic when the obstacle collides with the player
    /// </summary>
    virtual public void Collided(){
        PlaySound();
        Kill();
    }

    public void Kill(){
        collider.isTrigger = true;
        anim.SetBool("Dead", true);
        Destroy(gameObject, 1f);
    }

    public void PlaySound(){
        audioSource.pitch = 1f + Random.Range(-pitchDeviation, pitchDeviation);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
