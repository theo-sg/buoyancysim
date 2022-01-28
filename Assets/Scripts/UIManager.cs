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
    public Slider steepnessSlider;
    public Text steepnessText;
    public Slider wavelengthSlider;
    public Text wavelengthText;
    public Slider angleSlider;
    public Text angleText;

    /// <summary>
    /// initialise UI elements
    /// </summary>
    void OnEnable()
    {
        //set slider values and round values for text
        parameterMenu.SetActive(false);
        spawnMenu.SetActive(false);
        steepnessSlider.value = WaveGenerator.Instance.steepness;
        SetSteepnessText();
        wavelengthSlider.value = WaveGenerator.Instance.wavelength;
        SetWavelengthText();
        angleSlider.value = WaveGenerator.Instance.angle;
        SetAngleText();
        
    }

    public void ToggleMenu()
    {
        parameterMenu.SetActive(!parameterMenu.activeSelf);
        spawnMenu.SetActive(!spawnMenu.activeSelf);
    }

    public void SetSteepness()
    {
        WaveGenerator.Instance.steepness = steepnessSlider.value;
        SetSteepnessText();
    }

    public void SetWavelength()
    {
        WaveGenerator.Instance.wavelength = wavelengthSlider.value;
        SetWavelengthText();
    }

    public void SetAngle()
    {
        WaveGenerator.Instance.angle = angleSlider.value;
        SetAngleText();
    }

    void SetSteepnessText()
    {
        steepnessText.text = String.Format(
                             "Steepness: {0}%",
                             (Math.Round(steepnessSlider.value, 2) * 100).ToString());
    }

    void SetWavelengthText()
    {
        wavelengthText.text = String.Format(
                              "Wavelength: {0}m",
                              Math.Round(wavelengthSlider.value, 2).ToString());
    }

    void SetAngleText()
    {
        angleText.text = String.Format(
                         "Angle: {0}°",
                         Math.Round(angleSlider.value, 0).ToString());
    }
}
