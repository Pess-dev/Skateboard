using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    //singleton
    public static MusicPlayer Instance;
    void Awake()
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
    private string trackMusicPath = "Music/TrackMusic.mp3";
    [SerializeField]
    private string menuMusicPath = "Music/MenuMusic.mp3";

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Game.Instance.onGameStarted.AddListener(OnGameStart);
        Game.Instance.onGameEnded.AddListener(OnGameEnd);
        audioSource = GetComponent<AudioSource>();
       // StreamReader saves = new StreamReader();
        
        //print(Directory.GetCurrentDirectory());
		//realsaves = (saves.ReadToEnd());
    }
    void LoadClip(string localPath){
        StartCoroutine(LoadMusic(Directory.GetCurrentDirectory()+localPath));
    }

    void OnGameStart(){
        LoadClip(trackMusicPath);
    }

    void OnGameEnd(){
        LoadClip(menuMusicPath);
    }

    public IEnumerator LoadMusic(string songPath)
    {
        print(songPath);
        if (System.IO.File.Exists(songPath))
        {
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + songPath, AudioType.MPEG))
            {
                ((DownloadHandlerAudioClip)uwr.downloadHandler).streamAudio = true;

                yield return uwr.SendWebRequest();

                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(uwr.error);
                    yield break;
                }

                DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;

                if (dlHandler.isDone)
                {
                    AudioClip audioClip = dlHandler.audioClip;

                    if (audioClip != null)
                    {
                        audioSource.clip = audioClip;
                        audioSource.Play();
                        Debug.Log("Playing song using Audio Source!");
                    }
                    else
                    {
                        Debug.Log("Couldn't find a valid AudioClip :(");
                    }
                }
                else
                {
                    Debug.Log("The download process is not completely finished.");
                }
            }
        }
        else
        {
            Debug.Log("Unable to locate converted song file.");
        }
    }
}
