using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("General")] 
    public Ghost ghost;
    public GameObject ritualSpot;
    public SceneFader fader;
    
    [Header("UI")]
    public GameObject interactPrompt;

    public TextMeshProUGUI mementoDisplayUi;

    [Header("Mementos")]
    //how many mementos to spawn
    public int mementosSpawned = 0;

    //all mementos
    public Memento[] mementos;

    //how many have been found
    private int mementosFound = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        //Randomly choosing mementos to spawn
        //will break Unity if all mementos aren't disabled
        for(int i = 0; mementosSpawned > i; i++)
        {
            int choice = Random.Range(0, mementos.Length);
            
            //checking if object is already active
            //want to make sure the numbers are unique
             if(mementos[choice].gameObject.activeSelf){
                 i--; //going to have to choose another
             } else {
                 mementos[choice].gameObject.SetActive(true);
                 //setting Id of memento
                 mementos[choice].id = choice;
             }
        }

    }

    public void FindMemento(Memento memento){
        memento.Found();
        print("Memento: " + memento.id);
        mementosFound++;
        
        
        //displays the UI for finding the memento
        mementoDisplayUi.GetComponent<MementoDisplayController>().displayMemento(memento);
        
        
        ghost.IncreaseMinAnger();

        if(mementosFound >= mementosSpawned){
            //ritualSpot.activate(); or something
        }
    }

    public void RitualStarted()
    {
        print("Ritual Started");
    }

    public void ShowInteractable(Vector3 position){
        interactPrompt.SetActive(true);
        interactPrompt.transform.position = new Vector3(position.x, position.y + 0.5f, 0f);
    }

    public void HideInteractable(){
        interactPrompt.SetActive(false);
    }

    public void GameOver()
    {
        print("player big dead");
        //Disabled for easier testing
        //fader.FadeTo("MainMenu");
    }
}
