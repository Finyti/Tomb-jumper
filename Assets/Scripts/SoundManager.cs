using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static AudioSource AC;
    public void Awake()
    {
        AC = GetComponent<AudioSource>();
    }
    public static void Play(AudioClip ac, float volume = 1, float pitch = 1)
    {
        AC.volume = volume;
        AC.pitch = pitch;
        AC.PlayOneShot(ac);

    }
}
