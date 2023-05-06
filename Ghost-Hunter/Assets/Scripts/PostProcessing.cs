using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

//using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    private Volume volume;
    private ChromaticAberration chromaticAberration;
    private FilmGrain filmGrain;
    private Bloom bloom;
    private LensDistortion lensDistortion;

    private bool effectOn; //so other things don't override effects

    [Header("General")] 
    public AnimationCurve tempEffectCurve;
    public float tempEffectLength;
    public AnimationCurve generalCurve;
    public float generalLimit; //limit for general traversal, not counting events

    [Header("Lower Limit")] //For when there are the least effects
    public float chromaticAberrationIntensity;
    public float filmGrainIntensity;
    public float bloomIntensity;
    public float lensDistortionIntensity;
    public float lensDistortionScale;
    
    [Header("Max Boost")] //High values for the most effects
    //inputted as the increase from lower limit
    public float chromaticAberrationIntensityBoost;
    public float filmGrainIntensityBoost;
    public float bloomIntensityBoost;
    public float lensDistortionIntensityBoost;
    public float lensDistortionScaleBoost;

    void Start()
    {
        //grabbing post processing volume
        volume = this.GetComponent<Volume>();
        
        //grabbing individual effects
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out filmGrain);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out lensDistortion);

        effectOn = false;
    }
    
    //updates effects based on distance from ghost
    public void UpdateEffects(float distance)
    {
        if (!effectOn)
        {
            distance = Mathf.Clamp(distance, 0, 100)/100;
            float percent = generalCurve.Evaluate(distance);
            
            print("Update Effects: " + percent);

            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberrationIntensity, chromaticAberrationIntensity + (chromaticAberrationIntensityBoost * generalLimit), percent);
            filmGrain.intensity.value = Mathf.Lerp(filmGrainIntensity,  filmGrainIntensity + (filmGrainIntensityBoost * generalLimit), percent);
            bloom.intensity.value = Mathf.Lerp(bloomIntensity, bloomIntensity + (bloomIntensityBoost * generalLimit), percent);
            lensDistortion.intensity.value = Mathf.Lerp(lensDistortionIntensity, lensDistortionIntensity + (lensDistortionIntensityBoost * generalLimit), percent);
            lensDistortion.scale.value = Mathf.Lerp(lensDistortionScale, lensDistortionScale + (lensDistortionScaleBoost * generalLimit), percent);
        }
    }

    public void GhostAngered()
    {
        StartCoroutine(TempEffect(0.9f));
    }

    public void PlayerDamaged()
    {
        StartCoroutine(TempEffect(1f));
    }

    public void GhostStunned()
    {
        StartCoroutine(TempEffect(0.1f));
    }
    
    //ramps up and down a temporary screen effect
    IEnumerator TempEffect(float limit) //percentage of the much of the boost is the limit
    {
        float time = 0f;
        effectOn = true;

        while (time < tempEffectLength)
        {
            time += Time.deltaTime;
            float percent = tempEffectCurve.Evaluate(time);
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberrationIntensity, chromaticAberrationIntensity + (chromaticAberrationIntensityBoost * limit), percent);
            filmGrain.intensity.value = Mathf.Lerp(filmGrainIntensity,  filmGrainIntensity + (filmGrainIntensityBoost * limit), percent);
            bloom.intensity.value = Mathf.Lerp(bloomIntensity, bloomIntensity + (bloomIntensityBoost * limit), percent);
            lensDistortion.intensity.value = Mathf.Lerp(lensDistortionIntensity, lensDistortionIntensity + (lensDistortionIntensityBoost * limit), percent);
            lensDistortion.scale.value = Mathf.Lerp(lensDistortionScale, lensDistortionScale + (lensDistortionScaleBoost * limit), percent);
            yield return 0;
        }

        effectOn = false;
        
    }
    
}
