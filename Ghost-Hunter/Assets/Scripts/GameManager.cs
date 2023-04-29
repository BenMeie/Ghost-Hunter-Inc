using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[Header("Mementos")]
    //how many mementos to spawn
    public int mementosSpawned = 0;

    //all mementos
    private Memento[] mementos;

    //how many have been found
    private int mementosFound = 0;

    //[Header("General")]
    public GameObject ritualSpot;


    // Start is called before the first frame update
    void Start()
    {
        //grabbing all mementos
        //mementos = GameObject.FindGameObjectsWithTag("Memento");

        PlayerController.onMementoFound += findMemento;

        //Randomly choosing mementos to spawn
        for(int i = 0; mementosSpawned < i; i++){
            int choice = Random.Range(0, mementosSpawned);

            //checking if object is already active
            //want to make sure the numbers are unique
            if(mementos[choice].gameObject.active){
                i--; //going to have to choose another
            } else {
                mementos[choice].gameObject.SetActive(true);
                //setting Id of memento
                mementos[choice].id = i;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void findMemento(int mementoId){
        mementos[mementoId].found();
        mementosFound++;
        //add to GUI here

        //when ghost is added
        //Ghost.IncreaseMinAnger();

        if(mementosFound == mementosSpawned){
            //set ritual site to be active to dispell ghost
        }
    }
}
