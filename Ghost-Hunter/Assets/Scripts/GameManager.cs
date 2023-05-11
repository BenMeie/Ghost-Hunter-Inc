using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("General")] 
    public Ghost ghost;
    public GameObject ritualSpot;
    public static SceneFader fader;

    [Header("UI")]
    public static Image jumpscare;
    public GameObject interactPrompt;
    public PostProcessing postProcessing;

    public TextMeshProUGUI mementoDisplayUi;
//<<<<<<< Updated upstream
    //public TextMeshProUGUI ritualFailedUi;
//=======
    public GameObject inventory;
//>>>>>>> Stashed changes

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
        jumpscare = GameObject.FindWithTag("Jumpscare").GetComponent<Image>();
        jumpscare.enabled = false;
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
             }
        }

        fader = GameObject.FindWithTag("Fader").GetComponent<SceneFader>();

    }

    public void FindMemento(Memento memento){
        memento.Found();
        print("Memento: " + memento.name);
        mementosFound++;
        
        
        //displays the UI for finding the memento
        mementoDisplayUi.GetComponent<MementoDisplayController>().displayMemento(memento);
        inventory.GetComponent<InventoryScript>().addMemento(memento);
        
        ghost.IncreaseMinAnger();
        

        if(mementosFound >= mementosSpawned){
            //ritualSpot.activate(); or something
        }
    }

    public void RitualStarted()
    {
        Debug.Log("Attempting Ritual");
        if (mementosFound == mementosSpawned)
        {
            PlayerPrefs.SetInt("Finished", 1);
            ghost.updating = false;
            ghost.isattacking = true;
            //GameOver();
            ExorciseGhost();
        }
        else
        {

            print($"{mementosSpawned - mementosFound} More Mementos Needed");

            
            //have the remaining inventory slots flash red
            inventory.GetComponent<InventoryScript>().failRitual();
            print("More Mementos Needed");

        }
    }

    public void AngerGhost(int amount)
    {
        ghost.IncreaseAnger(amount);
        postProcessing.GhostAngered();
        //would be cool to have a sound queue for
        //when the player angers the ghost
    }

    public void StunGhost()
    {
        ghost.Stun();
        postProcessing.GhostStunned();
    }

    public void ShowInteractable(Vector3 position){
        interactPrompt.SetActive(true);
        interactPrompt.transform.position = new Vector3(position.x, position.y + 0.5f, 0f);
    }

    public void HideInteractable(){
        interactPrompt.SetActive(false);
    }

    public static void GameOver()
    {
        jumpscare.enabled = true;
        jumpscare.GetComponent<AudioSource>().Play();
        // print("player big dead");
        //Disabled for easier testing
        fader.FadeToGO("MainMenu");
    }

    //this is the win condition
    public static void ExorciseGhost()
    {
        fader.FadeToGO("FinalCutscene");
    }
}
