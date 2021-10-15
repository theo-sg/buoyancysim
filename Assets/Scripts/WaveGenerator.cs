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

    public Vector3 SampleNormal(float x)
    {
        float k = 2 * Mathf.PI / wavelength;
        float y = amplitude * Mathf.Cos(k * (x - speed * Time.time));
        Vector3 n = new Vector3(-y, 1, 0);
        return n.normalized;
    }
}
