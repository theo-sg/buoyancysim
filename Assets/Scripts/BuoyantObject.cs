using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Rigidbody))]
public class BuoyantObject : MonoBehaviour
{
    //### reference variables
    Rigidbody rb;
    MeshFilter filter;

    void OnEnable()
    {
        filter = GetComponent<MeshFilter>();
        rb = GetComponent<Rigidbody>(); 
    }

    public void Update()
    {
        ApplyBuoyancy();
    }

    /// <summary>
    /// calculates and applies buoyant force to each vertex
    /// </summary>
    void ApplyBuoyancy()
    {
        Vector3 gravity = WaveGenerator.Instance.strength * 
                          Vector3.down / filter.mesh.vertices.Length;

        //for every vertex in the mesh
        foreach (Vector3 vertex in filter.mesh.vertices)
        {

            //convert local to global position
            Vector3 vt = transform.TransformPoint(vertex);

            /* GERSTNER WAVES */
            Vector4 g = WaveGenerator.Instance.SampleGerstnerWave(vt.x, vt.z);
            float d = vt.y - g.w;

            //apply buoyant force if underwater
            if(d < 0f)
            {
                rb.AddForceAtPosition(-d * (Vector3)g *
                                     WaveGenerator.Instance.strength,
                                     vt, ForceMode.Acceleration);
            }
      
            //apply gravity
            rb.AddForceAtPosition(gravity, vt, ForceMode.Acceleration);
        }
    }
}
