using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioTagName
{
    MAIN_MUSIC      = 0,

    COLLECT_COIN    = 100,
    EXPLOSION       = 101,


}



[CreateAssetMenu()]
public class SoundFXDO : ScriptableObject
{
    public List<AudioData> sfxList;
    public List<AudioData> musicList;
}

[System.Serializable]
public class AudioData
{
    public AudioTagName name;
    public AudioClip audio;
}
