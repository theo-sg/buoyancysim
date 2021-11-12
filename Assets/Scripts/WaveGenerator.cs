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
    [Range(0f, 1f)] public float amplitude = 0.5f;
    [Range(1f, 20f)] public float wavelength = 5f;
    [Range(-10f, 10f)] public float speed = 1f;

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
            waterMaterial.SetFloat("_Amplitude", amplitude);
            waterMaterial.SetFloat("_Wavelength", wavelength);
            waterMaterial.SetFloat("_Speed", speed);
        }
    }

    /// <summary>
    /// DEPRECATED
    /// generates the new array of wave vertices each frame
    /// </summary>
    /// <param name="verts">current vertex positions</param>
    /// <returns>new vertex positions</returns>
    [System.Obsolete("This method is unneccesary, use the shader WaterShader instead")]
    Vector3[] GenerateWaveVertices(Vector3[] verts)
    {
        Vector3[] newVerts = verts;
        for (int i = 0; i < newVerts.Length; i++)
        {
            newVerts[i].y = SampleWaveHeight(newVerts[i].x);
        }
        return newVerts;
    }

    /// <summary>
    /// using wave equation 1
    /// f(x) = a * sin(k(x - ct))
    /// </summary>
    /// <param name="x">the x value</param>
    /// <returns>the y value</returns>
    public float SampleWaveHeight(float x)
    {
        float k = 2 * Mathf.PI / wavelength;
        return Mathf.Sin(k * (x - speed * Time.time)) * amplitude;
    }

    /// <summary>
    /// using wave equation 1
    /// n(x) = (1, -f'(x))
    /// </summary>
    /// <param name="x">the x value</param>
    /// <returns>the normal vector</returns>
    public Vector3 SampleNormal(float x)
    {
        /* SINE WAVE IMPLEMENTATION (PROTOTYPE 1) */
        //k, the wavenumber
        float k = 2 * Mathf.PI / wavelength;

        //xr, our input value for the function P(x, y)
        float xr = k * (x - speed * Time.time);

        //y, the output value (height)
        float y = k * amplitude * Mathf.Cos(xr);

        //normal vector with unit length 1
        Vector3 normal = new Vector3(-y, 1, 0);
        return normal.normalized;
    }

    public float SampleGerstnerHeight(float x)
    {
        /* GERSTNER WAVE IMPLEMENTATION 
        //k, the wavenumber
        float k = 2 * Mathf.PI / wavelength;

        //j = k(x - ct)
        float j = k * (x - speed * Time.time);

        //P(x, y), the output vector
        float y1 = amplitude * Mathf.Sin(j);
        float x1 = x + amplitude * Mathf.Cos(j);

        return y1;*/
        //now the Newton-Raphson method has to be used to approximate the root


        /* GERSTNER WAVE IMPLEMENTATION */
        //k, the wavenumber
        float k = 2 * Mathf.PI / wavelength;

        //j = k(x - ct)
        float j = k * (x - speed * Time.time);


        //round 1
        float x1 = x + amplitude * Mathf.Cos(j);
        float x2 = 2 * x - x1;

        float j2 = k * (x2 - speed * Time.time);
        float y3 = amplitude * Mathf.Sin(j2);

        return y3;
    }

    //GERSTNER TEST
    public Vector3 SampleGerstnerNormal(float x)
    {
        /* GERSTNER WAVE IMPLEMENTATION */
        //k, the wavenumber
        float k = 2 * Mathf.PI / wavelength;

        //j = k(x - ct)
        float j = k * (x - speed * Time.time);

        //resulting point
        float x1 = x + amplitude * Mathf.Cos(j);

        //find new approximation
        float x2 = 2 * x - x1;

        //find new j
        j = k * (x2 - speed * Time.time);

        //find gradient
        float dx = 1 - k * amplitude * Mathf.Sin(j);
        float dy = k * amplitude * Mathf.Cos(j);

        //normal
        return new Vector3(-dy, dx, 0).normalized;
    }
}
