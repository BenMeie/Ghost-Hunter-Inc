using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    private Volume volume;
    private ChromaticAberration chromaticAberration;
    private FilmGrain filmGrain;
    private Bloom bloom;
    private LensDistortion lensDistortion;

    [Header("General")] 
    public AnimationCurve curve;
    public float tempEffectLength;

    [Header("Low Values")] //For when there are the least effects
    public float lowChromaticAberrationIntensity;
    public float lowFilmGrainIntensity;
    public float lowBloomIntensity;
    public float lowLensDistortionIntensity;
    public float lowLensDistortionScale;
    
    [Header("High Values")] //High values for the most effects
    public float highChromaticAberrationIntensity;
    public float highFilmGrainIntensity;
    public float highBloomIntensity;
    public float highLensDistortionIntensity;
    public float highLensDistortionScale;

    void Start()
    {
        //grabbing post processing volume
        volume = this.GetComponent<Volume>();
        
        //grabbing individual effects
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out filmGrain);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out lensDistortion);
    }

    public void GhostAngered()
    {
        StartCoroutine(TempEffect());
    }

    public void GhostStunned()
    {
        
    }
    
    //ramps up and down a temporary screen effect
    IEnumerator TempEffect()
    {
        // chromaticAberration.intensity.value = highChromaticAberrationIntensity;
        // filmGrain.intensity.value = highFilmGrainIntensity;
        // bloom.intensity.value = highBloomIntensity;

        float time = 0f;

        while (time < tempEffectLength)
        {
            time += Time.deltaTime;
            float percent = curve.Evaluate(time);
            chromaticAberration.intensity.value = Mathf.Lerp(lowChromaticAberrationIntensity, highChromaticAberrationIntensity, percent);
            filmGrain.intensity.value = Mathf.Lerp(lowFilmGrainIntensity, highFilmGrainIntensity, percent);
            bloom.intensity.value = Mathf.Lerp(lowBloomIntensity, highBloomIntensity, percent);
            lensDistortion.intensity.value = Mathf.Lerp(lowLensDistortionIntensity, highLensDistortionIntensity, percent);
            lensDistortion.scale.value = Mathf.Lerp(lowLensDistortionScale, highLensDistortionScale, percent);
            yield return 0;
        }

        //WaitForSeconds(3);
        //return null;
    }
    
}
