using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


/*
 *This class controls the collected memento display UI
 * There will be only one collected memento display UI, and it will change it's
 * appearance based on what memento is found
 * It has an animator, which will run a short animation that will fade it in and out
 */
public class MementoDisplayController : MonoBehaviour
{
    private Animator animator;
    private TextMeshProUGUI myText;

    public GameObject myImage;

    //public Image myImage;

   

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    /*TODO the ultimate goal is to put the memento in a queue, then until the queue is empty,
     display one memento, dequeue it, and check again if there is another memento
    */
    public void displayMemento(Memento memento)
    {

        myText.text = "Found " + memento.name;
        Image display = myImage.GetComponent<Image>();
        display.sprite = memento.mySprite;
        
        animator.SetTrigger("CollectedMemento");
    }

}
