using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScene : MonoBehaviour
{
    public SceneFader fader;
    private float timer = 0f;
    public Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 101f)
        {
            fader.FadeTo("Main Menu");
        }

        if (Input.GetKey(KeyCode.Space))
        {
            animator.speed = 5;
            timer += Time.deltaTime * 4;
        }
        else
        {
            animator.speed = 1;
        }
    }
}
