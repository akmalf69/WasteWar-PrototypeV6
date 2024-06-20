using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("AUDIO SOURCE")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("AUDIO CLIP")]

    [Header("Music")]
    public AudioClip background;

    [Header("SFX")]
    public AudioClip checkpoint;
    public AudioClip swordSlash;
    public AudioClip bowStrike;
    public AudioClip chestBreak;
    public AudioClip coinSFX;
    public AudioClip healthSFX;
    public AudioClip staminaSFX;

    private void Start()
    {
        musicSource.clip= background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
