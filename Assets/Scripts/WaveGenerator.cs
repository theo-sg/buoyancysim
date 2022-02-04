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

    //### parameters with default values;
    [Range(0f, 0.7f)]   public float steepness = 0.2f;
    [Range(5f, 50f)]    public float wavelength = 10f;
    [Range(0f, 360f)]   public float angle = 0f;
    [Range(0.1f, 15f)]  public float strength = 9.81f;

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

            //calculate the direction vector from the angle
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
        for (int i = 0; i < 3; i++)
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
}
