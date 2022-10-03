using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IMPORTANT (MTN5): Heavily inspired by Brackeys AudioManager
// https://www.youtube.com/watch?v=6OT43pvUyfY

//NOTE (MTN5): A bit of an AucioSource fields duplications 
[System.Serializable]
public class SoundAsset
{
    public AudioSource Source;
    public AudioClip Clip;
    public float Volume;
    public float Pitch;
    public bool Loop;
    public string Name;
    public bool playOnAwake;
}

public class SoundManager : MonoBehaviour
{
    //Singleton pattern
    public static SoundManager Instance;

    public SoundAsset[] Sounds;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);

        foreach (SoundAsset s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
            s.Source.playOnAwake = s.playOnAwake;
        }

    }

    public void Play(string SoundName)
    {
        SoundAsset S = Array.Find(Sounds, Sound => Sound.Name == SoundName);
        S.Source.Stop();
        if (S == null) Debug.LogError("Sound " + SoundName + " not found!");
        S.Source.Play();
    }

    public void PlayOneShot(string SoundName)
    {
        SoundAsset S = Array.Find(Sounds, Sound => Sound.Name == SoundName);
        //S.Source.Stop();
        if (S == null) Debug.LogError("Sound " + SoundName + " not found!");
        S.Source.PlayOneShot(S.Clip);
    }

    public void Stop(string SoundName)
    {
        SoundAsset S = Array.Find(Sounds, Sound => Sound.Name == SoundName);
        S.Source.Stop();
    }

    public void SetVolume(string SoundName, float Volume)
    {
        SoundAsset S = Array.Find(Sounds, Sound => Sound.Name == SoundName);
        if (S == null) Debug.LogError("Sound " + SoundName + " not found!");
        S.Source.volume = Mathf.Lerp(S.Source.volume, Volume, Time.deltaTime);
    }

    public void SetPaning(string SoundName, float Panning)
    {
        SoundAsset S = Array.Find(Sounds, Sound => Sound.Name == SoundName);
        S.Source.panStereo = (Panning);
    }
}