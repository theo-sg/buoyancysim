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

    void Start()
    {
        //add new vertices
        //go through each triangle
        //find average of each vertex
        //add this new vertex back to the mesh
        //for(int i)
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

            float d = vt.y - WaveGenerator.Instance.SampleGerstnerHeight(vt.x);
            if(d < 0f)
                rb.AddForceAtPosition(-d * WaveGenerator.Instance.SampleGerstnerNormal(vt.x) *
                                      WaveGenerator.Instance.strength,
                                      vt, ForceMode.Acceleration);
            //apply gravity
            rb.AddForceAtPosition(gravity, vt, ForceMode.Acceleration);
        }
    }
}
