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
        steepnessSlider.value = WaveGenerator.Instance.steepness;
        steepnessText.text = "Steepness : " + Math.Round(steepnessSlider.value, 1).ToString();
        wavelengthSlider.value = WaveGenerator.Instance.wavelength;
        wavelengthText.text = "Wavelength : " + Math.Round(wavelengthSlider.value, 1).ToString();
        angleSlider.value = WaveGenerator.Instance.angle;
        angleText.text = "Angle : " + Math.Round(angleSlider.value, 1).ToString();
        
    }

    public void ToggleMenu()
    {
        parameterMenu.SetActive(!parameterMenu.activeSelf);
    }

    public void SetSteepness()
    {
        WaveGenerator.Instance.steepness = steepnessSlider.value;
        steepnessText.text = "Steepness : " + Math.Round(steepnessSlider.value, 1).ToString();
    }

    public void SetWavelength()
    {
        WaveGenerator.Instance.wavelength = wavelengthSlider.value;
        wavelengthText.text = "Wavelength : " + Math.Round(wavelengthSlider.value, 1).ToString();
    }

    public void SetAngle()
    {
        WaveGenerator.Instance.angle = angleSlider.value;
        angleText.text = "Angle : " + Math.Round(angleSlider.value, 1).ToString();
    }
}
