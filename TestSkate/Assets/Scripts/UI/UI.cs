using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    //singleton
    public static UI Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    [SerializeField]
    private float lerp = 1f;
    Skate _skate;
    private Transform target;

    [SerializeField]
    private GameObject mainMenuPanel;
    [SerializeField]
    private GameObject properties;
    [SerializeField]
    private GameObject gameUI;

    void Start(){
        _skate = Skate.Instance;
        target = _skate.GetHead();
        
        Game.Instance.onGameStarted.AddListener(GameUI);
        Game.Instance.onGameEnded.AddListener(MainMenuUI);
    }
    void Update(){
        Vector3 newPos = target.position;
        newPos.y = Mathf.Clamp(newPos.y,1.8f,2f);
        transform.position = Vector3.Lerp(transform.position, newPos, lerp * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(target.forward, Vector3.up)), lerp * Time.deltaTime);
    }

    public void GameUI(){
        gameUI.SetActive(true);
        mainMenuPanel.SetActive(false);
        properties.SetActive(false);
    }
    public void MainMenuUI(){
        gameUI.SetActive(false);
        mainMenuPanel.SetActive(true);
        properties.SetActive(false);

    }
    public void PropertiesUI(){
        gameUI.SetActive(false);
        mainMenuPanel.SetActive(false);
        properties.SetActive(true);
    }
}
