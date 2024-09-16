using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public PedometerSimulator pedometerSimulator;
    public PedometerManager pedometerManager;

    public Text stepCountText;
    public Text walkDistanceText;
    public Text caloriesBurnedText;
    public Text elapsedTimeText;
    public Image progressCircle; // Progress circle image
    public Text helloUserText;
    public Text currentDateText;

    public InputField settingsUsername;
    public InputField settingsHeight;
    public Toggle settingsGenderToggle;
    public UserConfigManager userConfigManager;

    public int maxSteps = 10000; // Define the maximum steps for full progress

    void Start()
    {
        UpdateStepText();
        UpdateVitalsText();
        UpdateProgressCircle(); // Initialize progress circle
        helloUserText.text = ("Hi ") + userConfigManager.usernameInputField.text;
        SetDate();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStepText();
        UpdateVitalsText();
        InputFieldUpdate();
        ElapsedTime();
        UpdateProgressCircle(); // Update progress circle
    }

    void SetDate()
    {
        if (currentDateText != null)
        {
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("d MMM yyyy");
            currentDateText.text = formattedDate; // Set the formatted date to the Text component
        }
    }

    void InputFieldUpdate()
    {
        settingsUsername.text = userConfigManager.usernameInputField.text;
        settingsHeight.text = userConfigManager.heightInputField.text;
        settingsGenderToggle.isOn = userConfigManager.genderToggle.isOn;
    }

    void UpdateStepText()
    {
        // Get the total steps from PedometerSimulator and update the UI text
        if (pedometerSimulator != null && stepCountText != null)
        {
            stepCountText.text = "Total Steps: " + pedometerSimulator.TotalSteps.ToString("N0");
        }
    }

    void UpdateVitalsText()
    {
        if (pedometerManager != null)
        {
            walkDistanceText.text = Mathf.RoundToInt(pedometerManager.WalkDistance) + " meters";
            caloriesBurnedText.text = Mathf.RoundToInt(pedometerManager.CaloriesBurned) + " kcal";
        }
    }

    void ElapsedTime()
    {
        if (pedometerSimulator != null && elapsedTimeText != null)
        {
            float elapsedTime = pedometerSimulator.ElapsedTime;
            elapsedTimeText.text = elapsedTime.ToString("F1") + " seconds";
        }
    }

    void UpdateProgressCircle()
    {
        if (progressCircle != null && pedometerSimulator != null)
        {
            // Calculate fill amount based on total steps and maximum steps that has been set...
            float stepRatio = Mathf.Clamp01((float)pedometerSimulator.TotalSteps / maxSteps); //clamp to a value that changes based on a desired step
            progressCircle.fillAmount = stepRatio; // Update the fill amount of the progress circle
        }
    }
}
