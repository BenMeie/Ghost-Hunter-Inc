using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonGameFinished : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //if Finished == 0 then it is considered as false, 1 => true
        if (PlayerPrefs.GetInt("Finished", 0) == 0)
        {
            this.GetComponent<Button>().interactable = false;
        }
    }
}
