using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip clip;

    [Range(0f, 10f)]
    public float volume;

    [Range(.1f, 10f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}

