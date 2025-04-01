using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public enum AudioFX 
{
    Walk,
    Asteroid,
}

public enum AudioSkills
{
    Pigeon,
    Bunny,
    Hook,
    Blink,
    Split
}

public enum AudioUI
{
    Button,
    Slider
}

public enum AudioMusic
{
    Piano
}

public enum AudioAmbience
{
    Space
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField]
    private bool playMusicOrAmbience;
    [SerializeField] private bool isMusicPlaying;
    [SerializeField] private bool isAmbiencePlaying;
    [SerializeField] private float clipTime;
    [SerializeField] private float timeBetweenClips;
    private bool isDone = false;
    
    
    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> m_walkClips;
    [SerializeField] private List<AudioClip> m_skillsClips;
    [SerializeField] private List<AudioClip> m_UIClips;
    [SerializeField] private List<AudioClip> m_fxClips;
    [SerializeField] private List<AudioClip> m_musicClips;
    [SerializeField] private List<AudioClip> m_fxAmbienceClips;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource ambienceAudioSource;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    
    private bool isFirstTime = true;
    
    private void Awake() 
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        if (playMusicOrAmbience) StartCoroutine(MainMusicAndAmbienceLoop());
    }

    private float PlayMusicWithTime(AudioMusic audioMusic)
    {
        PlayMusic(audioMusic, false);
        return m_musicClips[(int) audioMusic].length;
    }
    
    private float PlayAmbienceWithTime(AudioAmbience audioAmbience)
    {
        PlayAmbience(audioAmbience, false);
        return m_fxAmbienceClips[(int) audioAmbience].length;
    }

    private void PlayMusicOrAmbience()
    {
        /*int random = Random.Range(0, 2);
        isMusicPlaying = random == 0;
        isAmbiencePlaying = random == 1;
        
        if (isMusicPlaying) clipTime = PlayMusicWithTime(AudioMusic.Piano);
        else if (isAmbiencePlaying) clipTime = PlayAmbienceWithTime(Random.Range(0, 2) == 0 ? AudioAmbience.SteamMotor : AudioAmbience.Party);*/
    }
    
    private void ChooseMusicOrAmbience()
    {
        if (isFirstTime || clipTime <= 0)
        {
            if (!isFirstTime)
            {
                isMusicPlaying = false;
                isAmbiencePlaying = false;
                ambienceAudioSource.Stop();
                musicAudioSource.Stop();
                clipTime = timeBetweenClips;
            }
            else
            {
                PlayMusicOrAmbience();
                isFirstTime = false;
            }
        }
        
        clipTime = isMusicPlaying ? musicAudioSource.clip.length - musicAudioSource.time : isAmbiencePlaying ? ambienceAudioSource.clip.length - ambienceAudioSource.time : clipTime - Time.deltaTime;
        if (clipTime <= 0 && !isMusicPlaying && !isAmbiencePlaying) PlayMusicOrAmbience();
    }
    
    public IEnumerator MainMusicAndAmbienceLoop() 
    {
        while (playMusicOrAmbience) 
        {
            ChooseMusicOrAmbience();
            yield return null;
            isFirstTime = false;
        }
        
        clipTime = 0;
        isMusicPlaying = false;
        isAmbiencePlaying = false;
        ambienceAudioSource.Stop();
        musicAudioSource.Stop();
    }

    private void Start()
    {
        StartCoroutine(MainMusicAndAmbienceLoop());
    }

    public void PlayAudioClip(AudioClip audioClip, AudioSource audioSource) 
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void PlayIfFinished(AudioClip audioClip, AudioSource audioSource)
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(audioClip);
    }

    public void PlayAudioSource(AudioSource audioSource) 
    {
        audioSource.Play();
    }

    public void PlaySkill(AudioSkills audioSkill, AudioSource audioSource)
    {
        audioSource.PlayOneShot(m_skillsClips[(int) audioSkill]);
    }

    public void PlayUI(AudioUI audioUI, AudioSource audioSource)
    {
        audioSource.PlayOneShot(m_UIClips[(int) audioUI]);
    }
    
    public void PlayWalkFx(AudioSource audioSource, bool wait = false) 
    {
        if (wait) PlayIfFinished(m_walkClips[Random.Range(0, m_walkClips.Count)], audioSource);
        else audioSource.PlayOneShot(m_walkClips[Random.Range(0, m_walkClips.Count)]);
    }
    
    public void PlayFx(AudioFX audioFX, AudioSource audioSource) 
    {
        audioSource.PlayOneShot(m_fxClips[(int) audioFX]);
    }

    public void PlayMusic(AudioMusic audioMusic, bool isLooping = true) 
    {
        musicAudioSource.clip = m_musicClips[(int) audioMusic];
        musicAudioSource.Play();
        SetAudioSourceLoop(musicAudioSource, isLooping);
    }

    public void PlayMusic(AudioMusic audioMusic, AudioSource audioSource, bool isLooping = true) 
    {
        audioSource.clip = m_musicClips[(int) audioMusic];
        audioSource.Play();
        SetAudioSourceLoop(audioSource, isLooping);
    }
    
    public void PlayAmbience(AudioAmbience audioAmbience, bool isLooping = true) 
    {
        ambienceAudioSource.clip = m_fxAmbienceClips[(int)audioAmbience];
        ambienceAudioSource.Play();
        SetAudioSourceLoop(ambienceAudioSource, isLooping);
    }

    public void PlayAmbience(AudioAmbience audioAmbience, AudioSource audioSource, bool isLooping = true) 
    {
        audioSource.clip = m_fxAmbienceClips[(int)audioAmbience];
        audioSource.Play();
        SetAudioSourceLoop(audioSource, isLooping);
    }

    public void SetAudioSourceLoop(AudioSource audioSource, bool isLoop) 
    {
        audioSource.loop = isLoop;
    }

    public void PauseAudioSource(AudioSource audioSource) 
    {
        audioSource.Pause();
    }

    public void StopAudioSource(AudioSource audioSource) 
    {
        audioSource.Stop();
    }

    public void MuteAudioSource(AudioSource audioSource) 
    {
        audioSource.mute = true;
    }

    public void UnmuteAudioSource(AudioSource audioSource) 
    {
        audioSource.mute = false;
    }

    public void ToggleMuteAudioSource(AudioSource audioSource) 
    {
        audioSource.mute = !audioSource.mute;
    }

    public void SetMusicVolume(float volume) 
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetFXVolume(float volume) 
    {
        audioMixer.SetFloat("FXVolume", volume);
    }

    public void SetAmbienceVolume(float volume) 
    {
        audioMixer.SetFloat("AmbienceVolume", volume);
    }

    public void SaveVolumes() 
    {
        float tempValue;

        if (audioMixer.GetFloat("MusicVolume", out tempValue)) PlayerPrefs.SetFloat("MusicVolume", tempValue);
        if (audioMixer.GetFloat("FXVolume", out tempValue)) PlayerPrefs.SetFloat("FXVolume", tempValue);
        if (audioMixer.GetFloat("AmbienceVolume", out tempValue)) PlayerPrefs.SetFloat("AmbienceVolume", tempValue);

        PlayerPrefs.Save();
    }

    public void SetVolumes() 
    {
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        audioMixer.SetFloat("FXVolume", PlayerPrefs.GetFloat("FXVolume"));
        audioMixer.SetFloat("AmbienceVolume", PlayerPrefs.GetFloat("AmbienceVolume"));
    }
}