using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class EyeClick : MonoBehaviour
{
    float timer = 0;
    [SerializeField]
    float setTime = 0.5f;
    [SerializeField]
    float clickTime = 3f;

    public float progress {get; private set;} = 0f; 

    [SerializeField]
    Image image;

    Button button = null;

    [SerializeField]
    private LayerMask layerMask;

    void Update(){  
        image.fillAmount = progress;
        RaycastHit hit = new RaycastHit(); 
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f, layerMask))
        {
            Button newButton = hit.transform.gameObject.GetComponent<Button>();
            if (newButton != button){
                timer = 0;
            }
            button = newButton;

            if(timer > clickTime+setTime){
                button.onClick.Invoke();
                timer = 0;
            }
            if (button != null)
            timer+=Time.deltaTime;
            progress = Mathf.Clamp01((timer-setTime)/clickTime);
        }
        else{
            timer = 0;
            button = null;
            progress = 0f;
        }
    }
}
