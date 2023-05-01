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
    public Texture2D cursorTexture;
    public Texture2D cursorHover;
    private CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public SceneFader fader;

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        buttonText = button.GetComponent<TextMesh>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorHover, hotSpot, cursorMode);
        buttonText.color = new Color32(52, 46, 94, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        buttonText.color = new Color32(122, 109, 217, 255);
    }


    public void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreditsButton()
    {
        Debug.Log("TODO: credits scene");
    }

    public void QuitButton()
    {
        Debug.Log("Quitting...");
        Application.Quit();

    public void ClickPlay()
    {
        fader.FadeTo("GhostAI");
    }
}
