using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    //### singleton
    public static WaveGenerator Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    //### reference variables
    MeshFilter filter;
    public Material waterMaterial;

    //### wave equation parameters with default values;
    [Range(0f, 1f)] public float steepness = 0.5f;
    [Range(1f, 20f)] public float wavelength = 5f;

    //### simulation parameters
    [Range(0.1f, 15f)] public float strength = 9.81f;

    void OnEnable()
    {
        filter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        if (waterMaterial != null)
        {
            waterMaterial.SetFloat("_Steepness", steepness);
            waterMaterial.SetFloat("_Wavelength", wavelength);
        }
    }

   
    public float SampleGerstnerHeight(float x)
    {
        /* GERSTNER WAVE IMPLEMENTATION */
        //k, the wavenumber
        float k = 2 * Mathf.PI / wavelength;
        float c = Mathf.Sqrt(9.8f / k);

        //j = k(x - ct)
        float j = k * (x - c * Time.time);

        float a = steepness / k;

        //round 1
        float x1 = x + a * Mathf.Cos(j);
        float x2 = 2 * x - x1;

        float j2 = k * (x2 - c * Time.time);
        float y3 = a * Mathf.Sin(j2);

        return y3;
    }

    public Vector3 SampleGerstnerNormal(float x)
    {
        /* GERSTNER WAVE IMPLEMENTATION */
        //k, the wavenumber
        float k = 2 * Mathf.PI / wavelength;
        float c = Mathf.Sqrt(9.8f / k);

        //j = k(x - ct)
        float j = k * (x - c * Time.time);

        float a = steepness / k;

        //resulting point
        float x1 = x + a * Mathf.Cos(j);

        //find new approximation
        float x2 = 2 * x - x1;

        //find new j
        j = k * (x2 - c * Time.time);

        //find gradient
        float dx = 1 - steepness * Mathf.Sin(j);
        float dy = steepness * Mathf.Cos(j);

        //normal
        return new Vector3(-dy, dx, 0).normalized;
    }
}
