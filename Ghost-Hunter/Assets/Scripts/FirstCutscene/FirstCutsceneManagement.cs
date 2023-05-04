using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstCutsceneManagement : MonoBehaviour
{
    public SceneFader fader;
    public Text letter;
    public AudioSource pageTurn;
    
    // Start is called before the first frame update
    void Start()
    {
        string character = PlayerPrefs.GetString("Character", "Daisy");
        letter.text = "Dear " + character + ",\n\n" +
                      "As you may know, it has been 3 weeks since Mr. Guerrero did not report back, so he is now reported as missing. " +
                      "We believe that his disappearing is due to his latest hunting mission, at the mansion.\n\n" +
                      "The corporation asks you to go back to the mansion and find Mr. Guerrero.\n\n" +
                      "Ghostly,\n\n" +
                      "Ghost Hunter Inc.";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pageTurn.Play();
            fader.FadeTo("Base");
        }
    }
}
