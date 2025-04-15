using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    private List<ParticleSystem> _particles;
    private List<AudioSource> _audioSources;

    private void Start()
    {
        _particles = new List<ParticleSystem>();
        _audioSources = new List<AudioSource>();
        
        foreach (Transform child in transform)
        {
            _particles.Add(child.GetComponent<ParticleSystem>());
            _audioSources.Add(child.GetComponent<AudioSource>());
        }
    }

    public void StartConfetti()
    {
        foreach (var particle in _particles) particle.Play();
        foreach (var audioSource in _audioSources) SoundManager.Instance.PlayFx(AudioFX.Confetti, audioSource);
    }
}
