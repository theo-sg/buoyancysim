using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    //### singleton
    public static SpawnManager Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    //list of spawnable objects
    public GameObject[] spawns = new GameObject[2];

    /// <summary>
    /// spawns an object from the array
    /// </summary>
    /// <param name="n">the index of the item to spawn</param>
    public void SpawnObject(int n)
    {
        GameObject go = Instantiate(spawns[n], Vector3.up * 3, Quaternion.identity);
    }

    /// <summary>
    /// deletes the current 
    /// </summary>
    public void DeleteObject()
    {
        Transform t = CameraController.Instance.target;
        CameraController.Instance.UnlinkCamera();
        Destroy(t.gameObject);
        UIManager.Instance.SetDeleteButton(false);
    }
}
