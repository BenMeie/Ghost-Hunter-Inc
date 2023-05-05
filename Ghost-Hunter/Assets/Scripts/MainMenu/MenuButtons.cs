using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject button;
    private TextMesh buttonText;
    public SceneFader fader;

    private void Start()
    {
        buttonText = button.GetComponent<TextMesh>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = new Color32(52, 46, 94, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = new Color32(122, 109, 217, 255);
    }

    public void PlayButton()
    {
        fader.FadeTo("CharacterSelect");
    }

    public void CreditsButton()
    {
        fader.FadeTo("Credits");
    }

    public void QuitButton()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}