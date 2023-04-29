using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("General")] 
    public Ghost ghost;
    public GameObject ritualSpot;

    [Header("UI")]
    public GameObject interactPrompt;

    [Header("Mementos")]
    //how many mementos to spawn
    public int mementosSpawned = 0;

    //all mementos
    private Memento[] mementos;

    //how many have been found
    private int mementosFound = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        //Randomly choosing mementos to spawn
        for(int i = 0; mementosSpawned < i; i++){
            int choice = Random.Range(0, mementosSpawned);

            //checking if object is already active
            //want to make sure the numbers are unique
            if(mementos[choice].gameObject.activeSelf){
                i--; //going to have to choose another
            } else {
                mementos[choice].gameObject.SetActive(true);
                //setting Id of memento
                mementos[choice].id = i;
            }
        }

    }

    public void FindMemento(int mementoId){
        mementos[mementoId].Found();
        mementosFound++;
        //add to GUI here

        ghost.IncreaseMinAnger();

        if(mementosFound >= mementosSpawned){
            //ritualSpot.activate(); or something
        }
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
    }
}
