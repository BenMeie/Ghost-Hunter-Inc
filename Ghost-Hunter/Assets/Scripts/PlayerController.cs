using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mementoCheckingDistance = 1f;
    public GameObject[] mementos;

    public delegate void MementoFound(int id);

    public static event MementoFound onMementoFound;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //gets all the mementos
        findRemainingMementos();
    }

    // Update is called once per frame
    void Update()
    {
        //if the user presses "E", we will check if there's any nearby mementos.
        if (Input.GetKeyDown(KeyCode.E))
        {
            findRemainingMementos();//reset the mementos
            Debug.Log("Pressed E");
            checkForMemento();
        }
    }

    
    //this method will check if there's a memento nearby. If there is, it will send out an event
    //containing the id of the memento that was found
    bool checkForMemento()
    {
        
        Debug.Log($"There are currently {mementos.Length} mementos left");
        
        foreach (GameObject memento in mementos)
        {
            int mementoId;
            
            if (Vector2.Distance(transform.position, memento.transform.position) <=
                mementoCheckingDistance) //here we have to give a condition for if memento is close to the player
            {
                mementoId = memento.GetComponent<MementoController>().id;
                Debug.Log($"Interacted with memento number {mementoId}");
                
                //send out the event
                if (onMementoFound != null)
                {
                    onMementoFound(mementoId);
                }
                
                return true;
            }
        }
        

        return false;
    }

    void findRemainingMementos()
    {
        mementos = GameObject.FindGameObjectsWithTag("Memento");
    }
}
