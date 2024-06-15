using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StyleText : MonoBehaviour
{
    [System.Serializable]
    public class Style{
        public string text;
        public float value;
        public Color color = Color.white;
        public int fontSize = 36;

        public Style(string text, float value, Color color = default, int fontSize = 36){
            this.text = text;
            this.value = value;
            this.color = color;
            this.fontSize = fontSize;
        }
        public Style(Style style){
            text = style.text;
            value = style.value;
            color = style.color;
            fontSize = style.fontSize;
        }
    }
//D	C	B	A	S	SS	SSS
    List<Style> styles = new List<Style>(){
        new Style("", 0f, Color.white, 36),
        new Style("Dull", 0.3f, Color.white, 36),
        new Style("Cool!", 0.4f, Color.blue, 50),
        new Style("Bravo!", 0.5f, Color.yellow, 60),
        new Style("Awesome!", 0.6f, new Color(1, 0.4f, 0), 70),
        new Style("Sweet!", 0.80f, Color.red, 80),
        new Style("SShowtime!!", 0.90f, Color.red, 90),
        new Style("SSStylish!!!", 0.95f, Color.red, 100)
    };

     private Rigidbody _rb;

    [SerializeField]
    private string prefix = "";
    
    [SerializeField]
    private TextMeshPro _text;

    private Style currentStyle = new Style("", 0f);

    [SerializeField]
    private float impulseMultiplier = 100f;

    void Start(){
        _rb = GetComponent<Rigidbody>();
        Game.Instance.onStyleChanged.AddListener(UpdateValue);
        
        //sort styles by value
       // styles.Sort((a, b) => b.value.CompareTo(a.value));
        UpdateValue();
    }

    void UpdateValue(){
        float oldStyle = currentStyle.value;
        for (int i = styles.Count-1; i >= 0; i--){
            if (Game.Instance.style/Game.Instance.maxStyle >= styles[i].value){
                currentStyle = styles[i];
                break;
            }
        }

        _text.text = prefix + currentStyle.text;
        _text.color = currentStyle.color;
        _text.fontSize = currentStyle.fontSize;

        if (currentStyle.value > oldStyle){
        Vector3 randomDirection = Random.insideUnitSphere;
        _rb.AddForce(randomDirection * impulseMultiplier * currentStyle.value, ForceMode.Impulse);
        }
    }
    
}
