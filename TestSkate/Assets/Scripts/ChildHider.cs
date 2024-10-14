using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHider : MonoBehaviour
{
    public KeyCode key = KeyCode.Space;
    bool isHide = false;
    bool isPressed = false;

    void Start()
    {
        Hide();
    }

    void Update()
    {
        if (Input.GetKey(key) && !isPressed)  {
            if (isHide) {
                Show();
            } else {
                Hide();
            }
        }

        isPressed = Input.GetKey(key); 
    }

    public void Hide()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        
        isHide = true;
    }

    public void Show()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        
        isHide = false;
    }
}
