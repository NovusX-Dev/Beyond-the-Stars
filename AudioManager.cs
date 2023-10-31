using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] AudioSource _musicAudioSource;

    

    public void BossMusic(AudioClip clip, float delay)
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = clip;
        _musicAudioSource.PlayDelayed(delay);
    }

    public void StopMusic()
    {
        _musicAudioSource.Stop();
    }
}
