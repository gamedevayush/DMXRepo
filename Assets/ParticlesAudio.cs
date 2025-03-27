using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesAudio : MonoBehaviour
{
    public ParticleSystem particles;
    public AudioSource aud;
    public bool isEmitting;

    void OnValidate()
    {
        aud = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        aud = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (particles.isEmitting && !aud.isPlaying)
        {
            aud.Play();
        }
        else if (!particles.isEmitting && aud.isPlaying)
        {
            aud.Stop();
        }
    }
}
