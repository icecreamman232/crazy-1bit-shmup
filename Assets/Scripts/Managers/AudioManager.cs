using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public SoundFXDO audioData;

    [SerializeField]
    private AudioSource sfxPlayer;
    [SerializeField]
    private AudioSource musicPlayer;

    private Dictionary<AudioTagName, AudioClip> sfxDictionary;
    private Dictionary<AudioTagName, AudioClip> musicDictionary;

    private void Start()
    {
        sfxDictionary = new Dictionary<AudioTagName, AudioClip>();
        musicDictionary = new Dictionary<AudioTagName, AudioClip>();
        for (int i = 0; i< audioData.sfxList.Count;i++)
        {
            sfxDictionary.Add(audioData.sfxList[i].name, audioData.sfxList[i].audio);
        }

        for (int i = 0; i < audioData.musicList.Count; i++)
        {
            musicDictionary.Add(audioData.musicList[i].name, audioData.musicList[i].audio);
        }
    }
    public void PlaySFX(AudioTagName name,float volume = 1f)
    {
        if(sfxDictionary.ContainsKey(name))
        {
            sfxPlayer.PlayOneShot(sfxDictionary[name], volume);
        }
    }
    public void PlayMusic(AudioTagName name,float volume = 1f)
    {
        if (musicDictionary.ContainsKey(name))
        {
            musicPlayer.clip = musicDictionary[name];
            musicPlayer.volume = volume;
            musicPlayer.Play();
        }
    }
}
