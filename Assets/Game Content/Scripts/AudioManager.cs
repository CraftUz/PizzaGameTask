using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<Sound> soundEffects;
    public List<Sound> musicTracks;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    [Header("Settings")]
    [SerializeField] private BoolValueData SFXEnabledBD;
    [SerializeField] private BoolValueData MusicEnabledBD;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSounds(soundEffects, sfxSource);
            InitializeSounds(musicTracks, musicSource);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Instance.PlayMusic("GhostTheme", true);
    }

    void InitializeSounds(List<Sound> sounds, AudioSource sourcePrefab)
    {
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.playOnAwake = false;
            sound.source.loop = false;
            sound.source.outputAudioMixerGroup = sourcePrefab.outputAudioMixerGroup;
        }
    }

    // Play Sound Effect
    public void PlaySFX(string name)
    {
        if (!SFXEnabledBD.value) return;

        Sound s = soundEffects.Find(x => x.name == name);
        if (s != null)
        {
            s.source.PlayOneShot(s.clip, s.volume);
        }
    }

    // Play Background Music
    public void PlayMusic(string name, bool loop = true)
    {
        if (!MusicEnabledBD.value)
        {
            Debug.Log("Music is Not Enabled");
            return;
        }
        Sound m = musicTracks.Find(x => x.name == name);
        if (m != null)
        {
            musicSource.clip = m.clip;
            musicSource.volume = m.volume;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
