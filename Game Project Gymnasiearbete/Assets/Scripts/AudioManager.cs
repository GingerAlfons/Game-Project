using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixer audioMixer;
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.group;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
