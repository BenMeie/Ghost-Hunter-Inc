using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour
{
    public GameObject myImage;
    public GameObject redFlash;
    private Image displayedImage;
    private Animator animations;
    
    // Start is called before the first frame update
    void Start()
    {
        displayedImage = myImage.GetComponent<Image>();
        animations = redFlash.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void acceptImage(Sprite image)
    {
        displayedImage.sprite = image;
        myImage.GetComponent<CanvasGroup>().alpha = 1;
        //animations.SetTrigger("AddItem");
    }

    public void flashRed()
    {
        animations.SetTrigger("FailRitual");
    }
}
