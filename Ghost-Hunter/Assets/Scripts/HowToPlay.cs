using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    public SceneFader fader;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fader.FadeTo("FirstCutscene");
        }
    }
}
