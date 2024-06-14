using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class PlaneDissolveController : MonoBehaviour
{
    [SerializeField] private float strength = 0.5f;
    [SerializeField] private float length = 1f;
    [SerializeField] private Transform plane;
    [SerializeField] private float dissolvePower = 0.5f;
    [SerializeField] private float tiling = 1f;
    [SerializeField] private float blend = 1f;
    
    [SerializeField] private Texture2D pattern;

    private void Update(){
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        if (plane != null)
            props.SetVector("_planePosition", plane.position);
        if (plane != null)
            props.SetVector("_planeNormal", plane.forward);
        props.SetFloat("_strength", strength);
        props.SetFloat("_tiling", tiling);
        props.SetFloat("_blend", blend);
        props.SetFloat("_dissolvePower", dissolvePower);
        props.SetFloat("_length", length);
        if (pattern != null)
            props.SetTexture("_DissolvePattern", pattern);
        if (plane != null)
        foreach(MeshRenderer children in transform.GetComponentsInChildren<MeshRenderer>()){
            children.SetPropertyBlock(props);  
        }
    }
}
