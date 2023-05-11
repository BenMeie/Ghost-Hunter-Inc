using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalCutscene : MonoBehaviour
{
    public GameObject ghost;
    public GameObject ritual;
    public static SceneFader fader;

    private Animator animGhost;

    private Image imgGhost;
    private Image imgRitual;

    public AudioClip ritualSound1;
    public AudioClip ritualSound2;
    
    private AudioSource ghostScream;
    
    // Start is called before the first frame update
    void Start()
    {
        fader = GameObject.FindWithTag("Fader").GetComponent<SceneFader>();
        
        animGhost = ghost.GetComponent<Animator>();

        imgGhost = ghost.GetComponent<Image>();
        imgRitual = ritual.GetComponent<Image>();

        ghostScream = ghost.GetComponent<AudioSource>();
        
        StartCoroutine(scene());
    }

    IEnumerator scene()
    {
        yield return new WaitForSeconds(3);
        
        animGhost.Play("Idle_Img");
        StartCoroutine(FadeIn(imgGhost, fader.curve));

        yield return new WaitForSeconds(2);
        
        StartCoroutine(FadeIn(imgRitual, fader.curve));

        yield return new WaitForSeconds(10);
        
        ghostScream.Play();
        animGhost.Play("Teleport_Img");
        yield return new WaitForSeconds(7);
        fader.FadeTo("Credits");
    }
    
    IEnumerator FadeIn (Image img, AnimationCurve curve)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(1f, 1f, 1f, a);
            yield return 0;
        }
    }
}
