using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject button;
    private TextMesh buttonText;
    public Texture2D cursorTexture;
    public Texture2D cursorHover;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

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
}
