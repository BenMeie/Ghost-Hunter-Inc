using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Memento : MonoBehaviour
{
    //is set when it's set to active in the
    [HideInInspector]
    public int id; //each memento will have its own integer
    public GameObject particles;

    [HideInInspector]
    public GameObject memento;

    [HideInInspector]
    public bool interactable = false;


    // Start is called before the first frame update
    void Start()
    {
        memento = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //when a memento is found, each memento will run this function to see if it was the one that was found
    //if it was, it will release particles, and then destroy itself.
    public void Found()
    {   
        //Debug.Log("I have been found");
        GameObject foundParticles = Instantiate(particles, transform.position, transform.rotation);
        Destroy(foundParticles,5f);
        Destroy(gameObject);
    }
}
