using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lightning : MonoBehaviour
{
    public AudioClip[] thunderSounds;
    
    private Animator animator;
    private AudioSource audioSource;
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(LightningTimer());
    }

    IEnumerator LightningTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4, 10));
            animator.Play("Lightning Strike");
            yield return new WaitForSeconds(Random.Range(1, 2));
            animator.Play("Idle");
        }
    }

    void PlayThunder()
    {
        audioSource.volume = Random.Range(0.4f, 0.7f);
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.panStereo = Random.Range(-0.5f, 0.5f);
        audioSource.PlayOneShot(thunderSounds[Random.Range(0, thunderSounds.Length)]);
    }
}
