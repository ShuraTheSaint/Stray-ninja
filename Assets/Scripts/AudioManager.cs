using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    public Sound[] sounds;
    
    private Dictionary<string, Sound> soundDictionary;

    public static bool SoundCD = false; // Sound cooldown flag

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void InitializeAudio()
    {
        soundDictionary = new Dictionary<string, Sound>();
        
        foreach (Sound s in sounds)
        {
            s.player = GameObject.Find("Player");
            s.source = s.player.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            
            soundDictionary[s.name] = s;
        }
    }

    public void Play(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound: {name} not found!");
        }
    }

    public void PlayDelayed(string name, float delay)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.source.PlayDelayed(delay);
        }
        else
        {
            Debug.LogWarning($"Sound: {name} not found!");
        }
    }

    public void Stop(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogWarning($"Sound: {name} not found!");
        }
    }

    public void PlayOneShot(string name)
    {
        if (SoundCD&&(name=="Slash"||name=="Hit")) return;
        else if (name == "Slash" || name == "Hit")
        {
            StartCoroutine(SoundCoolDown());
        }

        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.source.PlayOneShot(s.clip);
        }
        else
        {
            Debug.LogWarning($"Sound: {name} not found!");
        }
    }

    public void StopAllSounds()
    {
        foreach (var soundEntry in soundDictionary)
        {
            Sound s = soundEntry.Value;
            if (s.source != null && s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }

    public void SetPitch(string name, float newPitch)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.pitch = newPitch;
            s.source.pitch = newPitch;
        }
        else
        {
            Debug.LogWarning($"Sound: {name} not found!");
        }
    }

    public void SetVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }

    IEnumerator SoundCoolDown()
    {
        SoundCD = true;
        yield return new WaitForSeconds(0.075f);
        SoundCD = false;
    }
}
