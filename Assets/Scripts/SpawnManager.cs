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

        objectsInScene = Object.FindObjectsOfType<BuoyantObject>().Length;
        uiManager = GetComponent<UIManager>();
        UpdateCounterText();
    }

    //list of spawnable objects
    public GameObject[] spawns = new GameObject[2];
    [SerializeField]   private int objectsInScene;
    public UIManager uiManager;

    /// <summary>
    /// spawns an object from the array
    /// </summary>
    /// <param name="n">the index of the item to spawn</param>
    public void SpawnObject(int n)
    {
        if(objectsInScene < 20)
        {
            GameObject go = Instantiate(spawns[n], Vector3.up * 3, Quaternion.identity);
            objectsInScene++;
            UpdateCounterText();
        }
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
        objectsInScene--;
        UpdateCounterText();
    }

    public void UpdateCounterText()
    {
        uiManager.counterText.text = objectsInScene.ToString() + "/20 objects";
    }
}
