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

    public void SpawnObject(int n)
    {
        GameObject go = Instantiate(spawns[n], Vector3.up * 3, Quaternion.identity);
    }
}
