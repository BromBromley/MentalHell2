using UnityEngine;
using System;

/*
    Create Class for Sound files that can be created everywhere
*/

[Serializable]
public class Sound {
    public enum SoundType {
        Music,
        Ambient,
        SFX
    }
    public string name;
    public SoundType type;
    public AudioClip clip;
    public UnityEngine.Audio.AudioMixerGroup output;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    public bool loop = false;

    [Range(0f, 1f)]
    public float spatialBlend = 0f;
    [Range(0f, 25f)]
    public float maxDistance = 25f;
    [Range(0f, 25f)]
    public float minDistance = 0f;

    public AudioRolloffMode rolloffMode;
}