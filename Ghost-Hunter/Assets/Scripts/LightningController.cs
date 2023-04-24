using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    private Animator lightningAnimator;

    // Start is called before the first frame update
    void Start()
    {
        lightningAnimator = GetComponent<Animator>();
        StartCoroutine(LightningTimer());
    }

    void LightningStrike(){
        lightningAnimator.SetTrigger("Lightning Strike");
    }

    
    IEnumerator LightningTimer()
	{
        //want to replace with a random number
        yield return new WaitForSeconds(20f);
        
        
        LightningStrike();
        LightningTimer();
	}

}