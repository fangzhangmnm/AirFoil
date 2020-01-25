using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDensitySetting : MonoBehaviour
{
    public float seaLevelDensity= 1.225f;
    public float halfDensityHeight = 6000f;
    public float maxDensity = 1.5f;
    static AirDensitySetting singleton;
    public static float getDensity(float height)
    {
        return Mathf.Min(singleton.maxDensity, singleton.seaLevelDensity * Mathf.Pow(0.5f, height / singleton.halfDensityHeight));
    }
    
    private void Start()
    {
        singleton = this;
    }
}
