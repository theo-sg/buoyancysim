using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    //### singleton
    public static UIManager Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;    
    }

    //### reference variables
    public GameObject parameterMenu;
    public GameObject spawnMenu;
    public GameObject deleteButton;
    public Slider steepnessSlider;
    public Text steepnessText;
    public Slider wavelengthSlider;
    public Text wavelengthText;
    public Slider angleSlider;
    public Text angleText;
    
    void OnEnable()
    {
        //initialise all UI elements
        parameterMenu.SetActive(false);
        spawnMenu.SetActive(false);
        deleteButton.SetActive(false);
        steepnessSlider.value = WaveGenerator.Instance.steepness;
        SetSteepnessText();
        wavelengthSlider.value = WaveGenerator.Instance.wavelength;
        SetWavelengthText();
        angleSlider.value = WaveGenerator.Instance.angle;
        SetAngleText();
        
    }

    /// <summary>
    /// sets the menu state
    /// </summary>
    public void ToggleMenu()
    {
        //is the menu meant to be on or off
        bool targetState = !parameterMenu.activeSelf;
        parameterMenu.SetActive(targetState);
        spawnMenu.SetActive(targetState);
        SetDeleteButton(targetState);       
    }


    /// <summary>
    /// sets the delete button state
    /// </summary>
    /// <param name="target">the target state - on or off</param>
    public void SetDeleteButton(bool target)
    {
        //delete button only on if object is being followed
        if (CameraController.Instance.currentState == CameraState.Follow && spawnMenu.activeSelf)
            deleteButton.SetActive(target);
        else
            deleteButton.SetActive(false);
    }

    /// <summary>
    /// sets steepness UI + value
    /// </summary>
    public void SetSteepness()
    {
        WaveGenerator.Instance.steepness = steepnessSlider.value;
        SetSteepnessText();
    }

    /// <summary>
    /// sets wavelength UI + value
    /// </summary>
    public void SetWavelength()
    {
        WaveGenerator.Instance.wavelength = wavelengthSlider.value;
        SetWavelengthText();
    }

    /// <summary>
    /// sets angle UI + value
    /// </summary>
    public void SetAngle()
    {
        WaveGenerator.Instance.angle = angleSlider.value;
        SetAngleText();
    }

    /// <summary>
    /// formats steepness text
    /// </summary>
    void SetSteepnessText()
    {
        steepnessText.text = String.Format(
                             "Steepness: {0}%",
                             (Math.Round(steepnessSlider.value, 2) * 100).ToString());
    }

    /// <summary>
    /// formats wavelength text
    /// </summary>
    void SetWavelengthText()
    {
        wavelengthText.text = String.Format(
                              "Wavelength: {0}m",
                              Math.Round(wavelengthSlider.value, 2).ToString());
    }

    /// <summary>
    /// formats angle text
    /// </summary>
    void SetAngleText()
    {
        angleText.text = String.Format(
                         "Angle: {0}°",
                         Math.Round(angleSlider.value, 0).ToString());
    }
}
