using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public AudioSource thunder;
    public Sprite background;
    
    void Update()
    {
        if (GetComponent<Image>().sprite == background)
            thunder.Play();
    }
}
