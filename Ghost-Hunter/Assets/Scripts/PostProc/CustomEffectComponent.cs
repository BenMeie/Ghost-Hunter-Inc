using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/CustomEffectComponent", typeof(UniversalRenderPipeline))]
public class CustomEffectComponent : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0, 0, 1, true);
    public NoInterpColorParameter color = new NoInterpColorParameter(Color.cyan);

    public bool IsActive()
    {
        return intensity.value > 0;
    }

    public bool IsTileCompatible()
    {
        return true;
    }
}
