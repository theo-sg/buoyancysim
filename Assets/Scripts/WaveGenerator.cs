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
    [Range(0f, 360f)] public float angle = 0f;

    //### simulation parameters
    [Range(0.1f, 15f)] public float strength = 9.81f;

    //### direction vector
    Vector2 d;

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
            float theta = angle * Mathf.Deg2Rad;
            d = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)).normalized;
            Vector4 vec = new Vector4(d.x, d.y, 0, 0);
            waterMaterial.SetVector("_Direction", vec);
        }
    }

    /// <summary>
    /// calculates the normal vector and height for a given x and z position
    /// </summary>
    /// <param name="x">input x - position</param>
    /// <param name="z">input z - position</param>
    /// <returns>vector4 (normal.x, normal.y, normal.z, height)</returns>
    public Vector4 SampleGerstnerWave(float x, float z)
    {
        //k, the wave number
        float k = 2 * Mathf.PI / wavelength;
        //c, the wave speed
        float c = Mathf.Sqrt(9.8f / k);
        //a = s / k
        float a = steepness / k;
        Vector2 v = new Vector2(x, z);

        //j = k( D.(x, z) - ct)
        float j = k * (Vector2.Dot(d, v) - c * Time.time);

        //2 Newton-Raphson iterations 
        for (int i = 0; i < 2; i++)
        {
            v.x -= d.x * steepness * Mathf.Cos(j);
            v.y -= d.y * steepness * Mathf.Cos(j);
            j = k * (Vector2.Dot(d, v) - c * Time.time);
        }   

        //tangent
        Vector3 tangent = new Vector3(1 - (d.x * d.x * steepness * Mathf.Sin(j)),
                                      d.x * steepness * Mathf.Cos(j),
                                      -d.x * d.y * steepness * Mathf.Sin(j));
        //binormal
        Vector3 binormal = new Vector3(-d.x * d.y * steepness * Mathf.Sin(j),
                                       d.y * steepness * Mathf.Cos(j),
                                       1 - (d.y * d.y * steepness * Mathf.Sin(j)));
        //normal
        Vector3 normal = Vector3.Cross(binormal, tangent).normalized;

        return new Vector4(normal.x, normal.y, normal.z, a * Mathf.Sin(j));
    }


    /*
    public float Sample2DGerstnerHeight(float x, float z)
    {
        //k, the wave number
        float k = 2 * Mathf.PI / wavelength;
        //c, the wave speed
        float c = Mathf.Sqrt(9.8f / k);
        //a = s / k
        float a = steepness / k;
        Vector2 v = new Vector2(x, z);

        //j = k( D.(x, z) - ct)
        float j = k * (Vector2.Dot(d, v) - c * Time.time);


        //Newton-Raphson 
        v.x += d.x * a * Mathf.Cos(j);
        v.y += d.y * a * Mathf.Cos(j);
        j = k * (Vector2.Dot(d, v) - c * Time.time);

        //initial output
        return a * Mathf.Sin(j);

    }

    public Vector3 Sample2DGerstnerNormal(float x, float z)
    {
        //k, the wave number
        float k = 2 * Mathf.PI / wavelength;
        //c, the wave speed
        float c = Mathf.Sqrt(9.8f / k);
        //a = s / k
        float a = steepness / k;
        Vector2 v = new Vector2(x, z);

        //j = k( D.(x, z) - ct)
        float j = k * (Vector2.Dot(d, v) - c * Time.time);

        //Newton-Raphson 
        v.x += d.x * a * Mathf.Cos(j);
        v.y += d.y * a * Mathf.Cos(j);
        j = k * (Vector2.Dot(d, v) - c * Time.time);


        //tangent
        Vector3 tangent = new Vector3(1 - (d.x * d.x * steepness * Mathf.Sin(j)),
                                      d.x * steepness * Mathf.Cos(j),
                                      -d.x * d.y * steepness * Mathf.Sin(j));
        //binormal
        Vector3 binormal = new Vector3(-d.x * d.y * steepness * Mathf.Sin(j),
                                       d.y * steepness * Mathf.Cos(j),
                                       1 - (d.y * d.y * steepness * Mathf.Sin(j)));
        //normal
        return Vector3.Cross(binormal, tangent).normalized;

    }

    public Vector3 SampleGerstnerNormal(float x)
    {
        /* GERSTNER WAVE IMPLEMENTATION 
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
    }*/
}
