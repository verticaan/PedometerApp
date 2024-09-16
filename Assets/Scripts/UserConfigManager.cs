using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UserConfigManager : MonoBehaviour
{
    public UserConfiguration userConfig;
    public InputField usernameInputField;
    public InputField heightInputField;
    public Toggle genderToggle;
    public Text errorMessage;

    private string filePath;

    void Awake() // better if we want to lead the configuration before the start method
    {
        //used Path.Combine to account for different OS's having different path seperators

        filePath = Path.Combine(Application.persistentDataPath, "userConfig.json");
        LoadConfiguration();

    }

    public void SaveConfiguration()
    {
        userConfig.userName = usernameInputField.text;

        float heightValue;
        if(float.TryParse(heightInputField.text, out heightValue))
        {
            userConfig.height = heightValue;
        }

        else
        {
            errorMessage.text = "Invalid height";
            return;
        }

        userConfig.isMale = genderToggle.isOn;

        if (!userConfig.IsValidConfiguration())
        {
            errorMessage.text = "Make sure the boxes are filled correctly";
            return;
        }

        SaveToJSON();

        errorMessage.text = "Configuration saved successfully";
    }

    private void SaveToJSON()
    {
        try
        {
            string json = JsonUtility.ToJson(userConfig, true);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            errorMessage.text = "Failed to save configuration: " + ex.Message;
            Debug.LogError("Error saving to JSON: " + ex.Message);
        }

    }

    public void LoadConfiguration()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(json, userConfig);

                usernameInputField.text = userConfig.userName;
                heightInputField.text = userConfig.height.ToString();
                genderToggle.isOn = userConfig.isMale;
            }
            else
            {
                errorMessage.text = "No saved configuration found.";
            }
        }
        catch (Exception ex)
        {
            errorMessage.text = "Failed to load configuration: " + ex.Message;
            Debug.LogError("Error loading config" + ex.Message);
        }
    }
}