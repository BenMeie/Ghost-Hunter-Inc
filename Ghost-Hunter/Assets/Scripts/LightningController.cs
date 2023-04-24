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
        StartCoroutine(LightningStrike());
    }

    void LightningStrike(){
        lightningAnimator.SetTrigger("lightning");
    }

    
    IEnumerator LightningTimer()
	{
        //want to replace with a random number
        yield return new WaitForSeconds(20f);
        
        LightningStrike();
        LightningTimer();
	}

}
