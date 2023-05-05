using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessing : MonoBehaviour
{
    // Start is called before the first frame update
    
    //private ChromaticAberration chromaticAberration;
    private Volume volume;

    void Start()
    {
        //chromaticAberration = postProcessing.GetComponent<ChromaticAberration>();
        volume = this.GetComponent<Volume>();
        //chromaticAberration.intensity.value = 50f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GhostAngered()
    {
        print("Testing please work");
    }

    public void GhostStunned()
    {
        
    }
    
    
    
}
