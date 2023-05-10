using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void play_sound()
    {
        audioSource.Play();
    }

    public void change_play_sound()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
