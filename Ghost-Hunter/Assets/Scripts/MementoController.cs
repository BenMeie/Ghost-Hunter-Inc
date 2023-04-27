using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoController : MonoBehaviour
{
    public int id;//each memento will have its own integer

    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.onMementoFound += find;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //when a memento is found, each memento will run this function to see if it was the one that was found
    //if it was, it will release particles, and then destroy itself.
    void find(int foundId)
    {
        if (foundId != id)//if its the wrong id, return without doing anything
        {
            return;
        }
        
        //Debug.Log("I have been found");
        GameObject foundParticles = Instantiate(particles, transform.position, transform.rotation);
        Destroy(foundParticles,5f);
        Destroy(gameObject);
    }
}
