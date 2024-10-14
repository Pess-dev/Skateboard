using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugKeyButton : MonoBehaviour
{
    public KeyCode key;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(key))
        {
            button.onClick.Invoke();
        }
    }
}
