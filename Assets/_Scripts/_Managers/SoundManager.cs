using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip audioClip;
}


public class SoundManager : MonoBehaviour
{
    #region Variables
    [Header("Background Music")]

    [SerializeField]
    Sound[] bgmStorage; // index[0] - GameScene BGM, 

    [SerializeField]
    AudioSource bgmPlayer;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PlayBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // GameScene BGM
    public void PlayBGM() 
    {
        bgmPlayer.clip = bgmStorage[0].audioClip;
        bgmPlayer.Play();
    }
}
