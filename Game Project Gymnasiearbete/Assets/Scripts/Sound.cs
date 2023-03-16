using UnityEngine.Audio;
using UnityEngine;
[System.Serializable]

public class Sound
{
    //alla variabler som sound behöver
    public AudioClip clip;
    public string name;
    [Range(0f, 1f)]
    public float volume;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
    public AudioMixerGroup group;

}
