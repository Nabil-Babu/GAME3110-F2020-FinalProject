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
    // can access from anywhere - singleton
    public static SoundManager instance;
    public string[] playSoundName;

    [Header("SFX")]
    public AudioSource[] audioSourceSFX;
    public Sound[] sfxSounds;

    [Header("BGM")]
    public AudioSource audioSourceBGM;
    public Sound[] bgmSounds; // index 0: Game Scene BGM



    #endregion

    #region UNITY_METHOD

    #region Singleton
    private void Awake()
    {
        //singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        playSoundName = new string[audioSourceSFX.Length];
        PlayBGM();
    }
    #endregion

    #region TO_PLAY_SOUND
    public void PLaySE(string name)
    {
        for(int i = 0; i < sfxSounds.Length; ++i)
        {
            if(name == sfxSounds[i].soundName)
            {
                for(int j = 0; j < audioSourceSFX.Length; ++j)
                {
                    if (!audioSourceSFX[j].isPlaying)
                    {
                        playSoundName[j] = sfxSounds[i].soundName;
                        audioSourceSFX[j].clip = sfxSounds[i].audioClip;
                        audioSourceSFX[j].Play();

                        return;
                    }
                }
                return;
            }
        }
    }
    public void PlayBGM()
    {
        audioSourceBGM.clip = bgmSounds[0].audioClip;
        audioSourceBGM.Play();
    }
    #endregion

    #region TO_STOP_SOUND
    public void StopAllSE()
    {
        for(int i = 0; i < audioSourceSFX.Length; ++i)
        {
            audioSourceSFX[i].Stop();
        }
    }

    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourceSFX.Length; ++i)
        {
            if(playSoundName[i] == name)
            {
                audioSourceSFX[i].Stop();
                break;
            }
        }
    }
    #endregion
}
